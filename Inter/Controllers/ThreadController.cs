using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
    public class ThreadController : BaseController
    {
        // private readonly InterService _db;
        // private readonly AuditHelper _audit;
        // private readonly FilterDefinitionBuilder<Board> _builder;
        // private readonly FilterDefinitionBuilder<Role> _builderRole;
        // private readonly IWebHostEnvironment _environment;
        //
        // public ThreadController(IWebHostEnvironment environment)
        // {
        //     _db = new InterService();
        //     _audit = new AuditHelper(_db);
        //     _builder = new FilterDefinitionBuilder<Board>();
        //     _builderRole = new FilterDefinitionBuilder<Role>();
        //     _environment = environment;
        // }

        public ThreadController(IWebHostEnvironment environment) : base(environment) {}

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ViewList(string boardId)
        {
            if (boardId is null)
                return RedirectToAction("ViewList", "Board");

            var boards = await Db.Boards.Find(Builder.Empty).ToListAsync();
            var board = boards.FirstOrDefault(thisBoard => string.CompareOrdinal(thisBoard.Id, boardId) == 0);

            if (board is null)
                return RedirectToAction("ViewList", "Board");

            board.Threads ??= new List<Thread>();
            var lists = AccountHelper.GetWriteAndReadSelectLists(await Db.Roles.Find(BuilderRole.Empty).ToListAsync());
            var threads = await GetListOfThreads(board.Threads);

            ViewBag.ReadRoles = lists[0];
            ViewBag.WriteRoles = lists[1];
            ViewBag.Boards = boards;
            ViewBag.BoardName = board.Name;
            ViewBag.BoardId = boardId;
            return View(threads);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(string boardId)
        {
            if (boardId is null)
                return RedirectToAction("ViewList", "Board");

            var lists = AccountHelper.GetWriteAndReadSelectLists(await Db.Roles.Find(BuilderRole.Empty)
                .ToListAsync());

            ViewBag.ReadRoles = lists[0];
            ViewBag.WriteRoles = lists[1];
            ViewBag.BoardId = boardId;
            return View();
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Name", "Text", "IsPinned", "ReadRoleName", "WriteRoleName", 
            "BoardId")] ThreadPost threadPost, string filePathInput)
        {
            var lists = AccountHelper.GetWriteAndReadSelectLists(await Db.Roles.Find(BuilderRole.Empty)
                .ToListAsync());

            ViewBag.ReadRoles = lists[0];
            ViewBag.WriteRoles = lists[1];
            
            var user = await AccountHelper.GetCurrentUserAsync(HttpContext, Db);
            
            if (!ModelState.IsValid)
                return View(threadPost);

            if (string.IsNullOrEmpty(threadPost.Text) && string.IsNullOrEmpty(filePathInput))
                return View(threadPost);

            var defaultRole = await Db.Roles.Find(BuilderRole.Eq("Name", RoleName.Anon)).FirstAsync();
            var text = TextHelper.EditPostText(threadPost.Text);
            var filter = Builder.Eq("_id", new ObjectId(threadPost.BoardId));
            var options = new ReplaceOptions { IsUpsert = true };
            var board = await Db.Boards.Find(filter).FirstOrDefaultAsync();
            var thread = new Thread
            {
                Name = TextHelper.EditThreadName(threadPost.Name, text),
                IsPinned = threadPost.IsPinned,
                ReadRoleName = threadPost.ReadRoleName ?? defaultRole.Name,
                WriteRoleName = threadPost.WriteRoleName ?? defaultRole.Name,
                Posts = new List<Post>(),
                BoardId = threadPost.BoardId
            };

            if (board is null)
                return RedirectToAction("ViewList", "Board");
            
            thread.Id = board.Threads.Count > 0 ? (int.Parse(board.Threads.Last().Id) + 1).ToString() : "0";
            thread.FileFolderUrl = PathHelper.GetThreadFolderPath(thread.BoardId, thread.Id);
            
            var post = new Post
            {
                Id = "0",
                Text = HtmlPageHelper.GetHtmlText(text),
                FileNames = new List<string>(),
                CreationTime = DateTime.Now,
                PosterId = user.Id,
                ThreadId = thread.Id,
                BoardId = board.Id
            };

            if (!string.IsNullOrEmpty(filePathInput) && !string.IsNullOrWhiteSpace(filePathInput))
                post.FileNames = FileHelper.ReplaceFiles(filePathInput.Trim().Split(' '), thread.FileFolderUrl, Environment);

            thread.Posts.Add(post);
            board.Threads.Add(thread);

            await Db.Boards.ReplaceOneAsync(filter, board, options);
            await Audit.AddAsync(typeof(Thread), MethodType.Create, ResultType.Success, AccountHelper.GetIpAddress(HttpContext),
                user, $"ID: {thread.Id}, BOARD_ID: {board.Id}, NAME: {thread.Name}, TEXT: {post.Text}");
            return RedirectToAction("ViewList", "Post", new { boardId = board.Id, threadId = thread.Id });
        }

        [HttpGet]
        [Authorize(Roles = RoleName.Admin + ", " + RoleName.Moderator)]
        public async Task<IActionResult> Remove(string boardId, string id)
        {
            if (boardId is null)
                return RedirectToAction("ViewList", "Board");
            
            if (id is null)
                return RedirectToAction(nameof(ViewList));

            var boards = await Db.Boards.Find(Builder.Empty).ToListAsync();
            var board = boards.FirstOrDefault(thisBoard => string.CompareOrdinal(thisBoard.Id, boardId) == 0);

            if (board is null)
                return RedirectToAction("ViewList", "Board");
            
            var thread = board.Threads.FirstOrDefault(thread => string.CompareOrdinal(thread.Id, id) == 0);
            
            ViewBag.Boards = boards;
            return thread is null 
                ? RedirectToAction("ViewList", "Thread", new { boardId }) 
                : View(thread);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.Admin + ", " + RoleName.Moderator)]
        public async Task<IActionResult> Remove(string boardId, int id)
        {
            return await RemoveStatus(boardId, id) switch
            {
                1 => RedirectToAction("ViewList", "Board"),
                _ => RedirectToAction(nameof(ViewList), new { boardId })
            };
        }
        
        [HttpPost]
        [Authorize(Roles = RoleName.Admin)]
        public async Task<IActionResult> ForcedRemove(string boardId, int id)
        {
            return await RemoveStatus(boardId, id, true) switch
            {
                1 => RedirectToAction("ViewList", "Board"),
                _ => RedirectToAction(nameof(ViewList), new { boardId })
            };
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> Upload()
        {
            if (Request.Form.Files.Count == 0)
                return Json(ConstError.FileError404);
            
            var files = Request.Form.Files;
            var resultPaths = new StringBuilder();
            var user = await AccountHelper.GetCurrentUserAsync(HttpContext, Db);

            if (user is null)
                return Json(ConstError.UserError404);
            
            for (var i = 0; i < Math.Min(files.Count, ConstHelper.MaxFilesCount); ++i)
            {
                string fileName;
                
                if (FileHelper.IsImage(files[i].FileName))
                {
                    var bitmap = new Bitmap(files[i].OpenReadStream());
                    fileName = FileHelper.GetNewFileName(files[i], $"({bitmap.Width}x{bitmap.Height})");
                }
                else
                    fileName = FileHelper.GetNewFileName(files[i]);
                
                if (!FileHelper.IsNormalFileSize(files[i].Length))
                    return Json(ConstError.FileError404);
                
                var path = ConstHelper.TempFolderUrl + "/" + fileName;
                await FileHelper.SaveFileAsync(files[i], path, Environment);
                resultPaths.Append(path + " "); 
            }
            
            return Json(resultPaths.ToString());
        }
        
        [HttpPost]
        [Authorize]
        public JsonResult Unload(string data)
        {
            data = data.Trim();
            if (string.IsNullOrEmpty(data) || string.CompareOrdinal(data, ConstError.FileError404) == 0)
                return Json(ConstError.PathError404);

            FileHelper.RemoveFiles(data.Split(' ').ToList(), Environment);
            
            return Json(ConstError.Success);
        }

        private async Task<IEnumerable<ThreadView>> GetListOfThreads(IEnumerable<Thread> threads, 
            IReadOnlyList<string> viewedThreadsIds = null)
        {
            var user = await AccountHelper.GetCurrentUserOrDefaultAsync(HttpContext, Db);
            var users = await Db.Users.Find(new FilterDefinitionBuilder<User>().Empty).ToListAsync();
            var pathHelper = new PathHelper(Url);
            var predicate = viewedThreadsIds is null
                ? (Func<Thread, bool>)(thisThread => AccountHelper.GetAccessIndex(user is null
                    ? RoleName.Anon
                    : user.Role.Name, thisThread.ReadRoleName) >= 0 && !thisThread.IsDeleted)
                : thisThread => AccountHelper.GetAccessIndex(user is null
                    ? RoleName.Anon
                    : user.Role.Name, thisThread.ReadRoleName) >= 0 && !thisThread.IsDeleted && 
                                !viewedThreadsIds.Contains(thisThread.Id);
            var newThreads = threads
                .Where(predicate)
                .Take(ConstHelper.MaxLoadThreadCount)
                .Select(thisThread => new ThreadView(thisThread)
                {
                    Posts = thisThread.Posts
                        .Where(thisPost => !thisPost.IsDeleted)
                        .Take(ConstHelper.MaxLoadPreviewPostCount)
                        .Select(thisPost => new PostView(thisPost, users)
                        {
                            Paths = thisPost.FileNames.Select(thisFileName => pathHelper.GetFilePath(
                                thisThread, thisFileName)).ToList(),
                            CompressedPaths = thisPost.FileNames.Select(thisFileName => pathHelper
                                .GetCompressedFilePath(thisThread, thisFileName)).ToList()
                        }).ToList()
                }).AsEnumerable();

            return newThreads;
        }

        private async Task<int> RemoveStatus(string boardId, int id, bool isForced = false)
        {
            var options = new ReplaceOptions { IsUpsert = true };
            var board = await Db.Boards.Find(Builder.Eq("_id", new ObjectId(boardId))).FirstOrDefaultAsync();

            if (board is null)
                return 1;

            var thread = board.Threads.Find(thread => string.CompareOrdinal(thread.Id, id.ToString()) == 0);

            if (thread is null)
                return 2;

            if (isForced)
            {
                board.Threads.Remove(thread);
                FileHelper.RemoveFilesFolder($"imgbrd/board_{board.Id}/thread_{thread.Id}", Environment);
            }
            else
            {
                foreach (var post in thread.Posts)
                    post.IsDeleted = true;
                
                thread.IsDeleted = true;
            }

            await Db.Boards.ReplaceOneAsync(Builder.Eq("_id", new ObjectId(boardId)), board, options);
            await Audit.AddAsync(typeof(Thread), MethodType.Remove, ResultType.Success, AccountHelper.GetIpAddress(HttpContext), 
                await AccountHelper.GetCurrentUserAsync(HttpContext, Db), $"ID: {id}, BOARD_ID: {boardId}, NAME: {thread.Name}, " +
                $"TEXT: {thread.OriginalPost.Text}");
            return 0;
        }
    }
}