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
    public class PostController : Controller
    {
        private readonly InterService _db;
        private readonly AuditHelper _audit;
        private readonly FilterDefinitionBuilder<Board> _builder;
        private readonly FilterDefinitionBuilder<User> _userBuilder;
        private readonly IWebHostEnvironment _environment;
        
        public PostController(IWebHostEnvironment environment)
        {
            _db = new InterService();
            _audit = new AuditHelper(_db);
            _builder = new FilterDefinitionBuilder<Board>();
            _userBuilder = new FilterDefinitionBuilder<User>();
            _environment = environment;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ViewList(string boardId, string threadId)
        {
            if (boardId is null)
                return RedirectToAction("ViewList", "Board");

            if (threadId is null)
                return RedirectToAction("ViewList", "Thread", new { boardId });
            
            var board = await _db.Boards.Find(_builder.Eq("_id", new ObjectId(boardId))).FirstOrDefaultAsync();

            if (board is null)
                return RedirectToAction("ViewList", "Board");

            var thread = board.Threads.FirstOrDefault(thread => string.CompareOrdinal(thread.Id, threadId) == 0);

            if (thread is null)
                return RedirectToAction("ViewList", "Thread", new { boardId });

            ViewBag.BoardId = boardId;
            ViewBag.ThreadId = threadId;
            ViewBag.ThreadName = thread.Name;
            return View(thread.Posts);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(string boardId, string threadId)
        {
            if (boardId is null)
                return RedirectToAction("ViewList", "Board");

            if (threadId is null)
                return RedirectToAction("ViewList", "Thread", new { boardId });

            var user = await GetCurrentUserAsync();

            ViewBag.UserId = user.Id;
            ViewBag.BoardId = boardId;
            ViewBag.ThreadId = threadId;
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id", "Text", "ThreadId", "BoardId")] Post post, string posterId)
        {
            if (!ModelState.IsValid)
                return View(post);

            var board = await _db.Boards.Find(_builder.Eq("_id", new ObjectId(post.BoardId))).FirstOrDefaultAsync();

            if (board is null)
                return RedirectToAction("ViewList", "Board");

            var thread = board.Threads.FirstOrDefault(thread => string.CompareOrdinal(thread.Id, post.ThreadId) == 0);

            if (thread is null)
                return RedirectToAction("ViewList", "Thread", new { post.BoardId });

            var poster = await _db.Users.Find(_userBuilder.Eq("_id", new ObjectId(posterId))).FirstOrDefaultAsync();

            if (poster is null)
                return RedirectToAction("Page404", "Audit");

            post.Id = thread.Posts.Count > 0 ? (int.Parse(thread.Posts.Last().Id) + 1).ToString() : "0";
            post.FileNames = new List<string>();
            post.CreationTime = DateTime.Now;
            post.Poster = poster;
            thread.Posts.Add(post);
            var options = new ReplaceOptions { IsUpsert = true };
            
            // FILE UPDATE

            await _db.Boards.ReplaceOneAsync(_builder.Eq("_id", new ObjectId(post.BoardId)), board, options);
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
            
            var board = await _db.Boards.Find(_builder.Eq("_id", new ObjectId(boardId))).FirstOrDefaultAsync();
            
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
            var filter = _builder.Eq("_id", new ObjectId(boardId));
            var options = new ReplaceOptions { IsUpsert = true };
            var board = await _db.Boards.Find(filter).FirstOrDefaultAsync();
            
            if (board is null)
                return RedirectToAction("ViewList", "Board");
            
            var thread = board.Threads.FirstOrDefault(thread => string.CompareOrdinal(thread.Id, threadId) == 0);

            if (thread is null)
                return RedirectToAction("ViewList", "Thread", new { boardId });

            var post = thread.Posts.FirstOrDefault(post => string.CompareOrdinal(post.Id, id.ToString()) == 0);

            if (post is null)
                return RedirectToAction(nameof(ViewList));
            
            FileHelper.RemoveFiles(post.FileNames, _environment);
            
            thread.Posts.Remove(post);
            
            await _db.Boards.ReplaceOneAsync(filter, board, options);
            await _audit.AddAsync(typeof(Post), MethodType.Remove, ResultType.Success, AccountHelper.GetIpAddress(HttpContext), 
                await GetCurrentUserAsync(), $"ID: {id}, THREAD_ID: {threadId}, BOARD_ID: {boardId}, " +
                $"TEXT: {post.Text}");
            return RedirectToAction("ViewList", "Post", new { boardId, threadId });
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var builder = new FilterDefinitionBuilder<User>();

            if (HttpContext.User.Identity is null)
                throw new Exception("Class: AccountController; Method: GetCurrentUser.");
            
            var userName = HttpContext.User.Identity.Name;
            
            return await _db.Users.Find(builder.Regex("Name", new BsonRegularExpression(userName))).FirstAsync();
        }
    }
}