using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inter.Helpers;
using Inter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inter.Controllers
{
    public class PostController : BaseController
    {
        // private readonly InterService _db;
        // private readonly AuditHelper _audit;
        // private readonly FilterDefinitionBuilder<Board> _builder;
        // private readonly FilterDefinitionBuilder<User> _userBuilder;
        // private readonly IWebHostEnvironment _environment;
        //
        // public PostController(IWebHostEnvironment environment)
        // {
        //     _db = new InterService();
        //     _audit = new AuditHelper(_db);
        //     _builder = new FilterDefinitionBuilder<Board>();
        //     _userBuilder = new FilterDefinitionBuilder<User>();
        //     _environment = environment;
        // }

        public PostController(IWebHostEnvironment environment) : base(environment) {}

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ViewList(string boardId, string threadId)
        {
            if (boardId is null)
                return RedirectToAction("ViewList", "Board");

            if (threadId is null)
                return RedirectToAction("ViewList", "Thread", new { boardId });
            
            var boards = await Db.Boards.Find(Builder.Empty).ToListAsync();
            var board = boards.FirstOrDefault(thisBoard => string.CompareOrdinal(thisBoard.Id, boardId) == 0);

            if (board is null)
                return RedirectToAction("ViewList", "Board");

            var thread = board.Threads.FirstOrDefault(thread => string.CompareOrdinal(thread.Id, threadId) == 0);

            if (thread is null)
                return RedirectToAction("ViewList", "Thread", new { boardId });

            var posts = await GetListOfPosts(thread.Posts, thread);
            
            // ViewBag.BoardId = boardId;
            // ViewBag.ThreadId = threadId;
            ViewBag.Boards = boards;
            ViewBag.ThreadName = thread.Name;
            return View(posts);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(string boardId, string threadId)
        {
            if (boardId is null)
                return RedirectToAction("ViewList", "Board");

            if (threadId is null)
                return RedirectToAction("ViewList", "Thread", new { boardId });

            var user = await AccountHelper.GetCurrentUserAsync(HttpContext, Db);

            ViewBag.UserId = user.Id;
            ViewBag.BoardId = boardId;
            ViewBag.ThreadId = threadId;
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id", "ThreadId", "BoardId", "PosterId")] Post post, 
            string text)
        {
            if (!ModelState.IsValid)
                return View(post);

            var board = await Db.Boards.Find(Builder.Eq("_id", new ObjectId(post.BoardId))).FirstOrDefaultAsync();

            if (board is null)
                return RedirectToAction("ViewList", "Board");

            var thread = board.Threads.FirstOrDefault(thread => string.CompareOrdinal(thread.Id, post.ThreadId) == 0);

            if (thread is null)
                return RedirectToAction("ViewList", "Thread", new { post.BoardId });

            var user = await Db.Users.Find(new FilterDefinitionBuilder<User>().Eq("_id", 
                new ObjectId(post.PosterId))).FirstOrDefaultAsync();

            if (user is null)
                return RedirectToAction("Page404", "Forum");

            post.Text = HtmlPageHelper.GetHtmlText(TextHelper.EditPostText(text));
            post.Id = thread.Posts.Count > 0 ? (int.Parse(thread.Posts.Last().Id) + 1).ToString() : "0";
            post.FileNames = new List<string>();
            post.CreationTime = DateTime.Now;
            post.PosterId = user.Id;
            
            thread.Posts.Add(post);
            var options = new ReplaceOptions { IsUpsert = true };
            
            // FILE UPDATE

            await Db.Boards.ReplaceOneAsync(Builder.Eq("_id", new ObjectId(post.BoardId)), board, options);
            return RedirectToAction("ViewList", "Post", new { boardId = post.BoardId, threadId = post.ThreadId });
        }

        [HttpGet]
        [Authorize(Roles = RoleName.Admin + ", " + RoleName.Moderator)]
        public async Task<IActionResult> Remove(string boardId, string threadId, string id)
        {
            if (boardId is null)
                return RedirectToAction("ViewList", "Board");

            if (threadId is null)
                return RedirectToAction("ViewList", "Thread", new { boardId });

            if (id is null || string.CompareOrdinal(id, "0") == 0)
                return RedirectToAction(nameof(ViewList));
            
            var board = await Db.Boards.Find(Builder.Eq("_id", new ObjectId(boardId))).FirstOrDefaultAsync();
            
            if (board is null)
                return RedirectToAction("ViewList", "Board");
            
            var thread = board.Threads.FirstOrDefault(thread => string.CompareOrdinal(thread.Id, threadId) == 0);

            if (thread is null)
                return RedirectToAction("ViewList", "Thread", new { boardId });

            var post = thread.Posts.FirstOrDefault(post => string.CompareOrdinal(post.Id, id) == 0);

            return post is null ? RedirectToAction(nameof(ViewList)) : View(post);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.Admin + ", " + RoleName.Moderator)]
        public async Task<IActionResult> Remove(string boardId, string threadId, int id)
        {
            return await RemoveStatus(boardId, threadId, id) switch
            {
                1 => RedirectToAction("ViewList", "Board"),
                2 => RedirectToAction("ViewList", "Thread", new { boardId }),
                _ => RedirectToAction(nameof(ViewList), new { boardId, threadId })
            };
        }

        [HttpGet]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> ForcedRemove(string boardId, string threadId, int id)
        {
            return await RemoveStatus(boardId, threadId, id, true) switch
            {
                1 => RedirectToAction("ViewList", "Board"),
                2 => RedirectToAction("ViewList", "Thread", new { boardId }),
                _ => RedirectToAction(nameof(ViewList), new { boardId, threadId })
            };
        }

        private async Task<IEnumerable<PostView>> GetListOfPosts(IEnumerable<Post> posts, Thread thread,
            IReadOnlyList<string> viewedPostsIds = null)
        {
            var users = await Db.Users.Find(new FilterDefinitionBuilder<User>().Empty).ToListAsync();
            var pathHelper = new PathHelper(Url);
            var predicate = viewedPostsIds is null
                ? (Func<Post, bool>)(thisPost => !thisPost.IsDeleted)
                : thisPost => !thisPost.IsDeleted && !viewedPostsIds.Contains(thisPost.Id);
            var newPosts = posts
                .Where(predicate)
                .Take(ConstHelper.MaxLoadPostCount)
                .Select(thisPost => new PostView(thisPost, users)
                {
                    Paths = thisPost.FileNames.Select(thisFileName => pathHelper.GetFilePath(
                        thread, thisFileName)).ToList(),
                    CompressedPaths = thisPost.FileNames.Select(thisFileName => pathHelper
                        .GetCompressedFilePath(thread, thisFileName)).ToList()
                }).AsEnumerable();

            return newPosts;
        }
        
        private async Task<int> RemoveStatus(string boardId, string threadId, int id, bool isForced = false)
        {
            var options = new ReplaceOptions { IsUpsert = true };
            var board = await Db.Boards.Find(Builder.Eq("_id", new ObjectId(boardId))).FirstOrDefaultAsync();

            if (board is null)
                return 1;
            
            var thread = board.Threads.FirstOrDefault(thread => string.CompareOrdinal(thread.Id, threadId) == 0);

            if (thread is null)
                return 2;

            var post = thread.Posts.FirstOrDefault(post => string.CompareOrdinal(post.Id, id.ToString()) == 0);

            if (post is null)
                return 3;
            
            FileHelper.RemoveFiles(post.FileNames, Environment);

            post.IsDeleted = true;

            if (isForced)
                thread.Posts.Remove(post);

            await Db.Boards.ReplaceOneAsync(Builder.Eq("_id", new ObjectId(boardId)), board, options);
            await Audit.AddAsync(typeof(Post), MethodType.Remove, ResultType.Success, AccountHelper.GetIpAddress(HttpContext), 
                await AccountHelper.GetCurrentUserAsync(HttpContext, Db), $"ID: {id}, THREAD_ID: {threadId}, BOARD_ID: {boardId}, " +
                $"TEXT: {post.Text}");
            return 0;
        }
    }
}