using System;
using System.Collections.Generic;
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
    public class ThreadController : Controller
    {
        private readonly InterService _db;
        private readonly AuditHelper _audit;
        private readonly FilterDefinitionBuilder<Board> _builder;
        private readonly FilterDefinitionBuilder<Role> _builderRole;
        private readonly IWebHostEnvironment _environment;

        public ThreadController(IWebHostEnvironment environment)
        {
            _db = new InterService();
            _audit = new AuditHelper(_db);
            _builder = new FilterDefinitionBuilder<Board>();
            _builderRole = new FilterDefinitionBuilder<Role>();
            _environment = environment;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ViewList(string boardId)
        {
            if (boardId is null)
                return RedirectToAction("ViewList", "Board");
            
            var board = await _db.Boards.Find(_builder.Eq("_id", new ObjectId(boardId))).FirstOrDefaultAsync();

            if (board is null)
                return RedirectToAction("ViewList", "Board");

            board.Threads ??= new List<Thread>();
            
            ViewBag.Roles = AccountHelper.GetSelectList(await _db.Roles.Find(_builderRole.Empty).ToListAsync());
            ViewBag.Boards = await _db.Boards.Find(_builder.Empty).ToListAsync();
            ViewBag.BoardName = board.Name;
            ViewBag.BoardId = boardId;
            return View(board.Threads);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(string boardId)
        {
            if (boardId is null)
                return RedirectToAction("ViewList", "Board");

            ViewBag.Roles = AccountHelper.GetSelectList(await _db.Roles.Find(_builderRole.Empty).ToListAsync());
            ViewBag.BoardId = boardId;
            return View();
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Name", "Text", "IsPinned", "ReadRoleName", "WriteRoleName", 
            "BoardId")] ThreadPost threadPost, string filePathInput)
        {
            ViewBag.Roles = AccountHelper.GetSelectList(await _db.Roles.Find(_builderRole.Empty).ToListAsync());
            var user = await GetCurrentUserAsync();
            
            if (!ModelState.IsValid)
                return View(threadPost);

            if (string.IsNullOrEmpty(threadPost.Text) && string.IsNullOrEmpty(filePathInput))
                return View(threadPost);

            var defaultRole = await _db.Roles.Find(_builderRole.Eq("Name", RoleName.Anon)).FirstAsync();
            var text = string.IsNullOrEmpty(threadPost.Text) || string.IsNullOrWhiteSpace(threadPost.Text) 
                ? string.Empty 
                : threadPost.Text.Trim();
            var textName = text.Length > ConstHelper.MaxNameLength ? text[..ConstHelper.MaxNameLength] : text;
            textName = string.IsNullOrEmpty(textName) ? ConstHelper.RandomThreadName : textName;
            
            var thread = new Thread
            {
                Name = string.IsNullOrEmpty(threadPost.Name) ? textName : threadPost.Name.Trim(),
                IsPinned = threadPost.IsPinned,
                ReadRoleName = threadPost.ReadRoleName ?? defaultRole.Name,
                WriteRoleName = threadPost.WriteRoleName ?? defaultRole.Name,
                Posts = new List<Post>(),
                BoardId = threadPost.BoardId
            };

            var filter = _builder.Eq("_id", new ObjectId(threadPost.BoardId));
            var options = new ReplaceOptions { IsUpsert = true };
            var board = await _db.Boards.Find(filter).FirstOrDefaultAsync();

            if (board is null)
                return RedirectToAction("ViewList", "Board");
            
            thread.Id = board.Threads.Count > 0 ? (int.Parse(board.Threads.Last().Id) + 1).ToString() : "0";
            thread.FileFolderUrl = $"imgbrd/board_{thread.BoardId}/thread_{thread.Id}";
            
            var post = new Post
            {
                Id = "0",
                Text = text,
                FileNames = new List<string>(),
                CreationTime = DateTime.Now,
                Poster = user,
                ThreadId = thread.Id,
                BoardId = board.Id
            };

            if (!string.IsNullOrEmpty(filePathInput) && !string.IsNullOrWhiteSpace(filePathInput))
                post.FileNames = FileHelper.ReplaceFiles(filePathInput.Split(' '), thread.FileFolderUrl, _environment);

            thread.Posts.Add(post);
            board.Threads.Add(thread);

            await _db.Boards.ReplaceOneAsync(filter, board, options);
            await _audit.AddAsync(typeof(Thread), MethodType.Create, ResultType.Success, AccountHelper.GetIpAddress(HttpContext),
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

            var board = await _db.Boards.Find(_builder.Eq("_id", new ObjectId(boardId))).FirstOrDefaultAsync();

            if (board is null)
                return RedirectToAction("ViewList", "Board");
            
            var thread = board.Threads.FirstOrDefault(thread => string.CompareOrdinal(thread.Id, id) == 0);

            return thread is null ? RedirectToAction("ViewList", "Thread", new { boardId }) : View(thread);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.Admin + ", " + RoleName.Moderator)]
        public async Task<IActionResult> Remove(string boardId, int id)
        {
            var filter = _builder.Eq("_id", new ObjectId(boardId));
            var options = new ReplaceOptions { IsUpsert = true };
            var board = await _db.Boards.Find(filter).FirstOrDefaultAsync();
            
            if (board is null)
                return RedirectToAction("ViewList", "Board");

            var thread = board.Threads.Find(thread => string.CompareOrdinal(thread.Id, id.ToString()) == 0);
            
            if (thread is null)
                return RedirectToAction("ViewList", "Board");
            
            FileHelper.RemoveFilesFolder($"imgbrd/board_{board.Id}/thread_{thread.Id}", _environment);
            
            board.Threads.Remove(thread);

            await _db.Boards.ReplaceOneAsync(filter, board, options);
            await _audit.AddAsync(typeof(Thread), MethodType.Remove, ResultType.Success, AccountHelper.GetIpAddress(HttpContext), 
                await GetCurrentUserAsync(), $"ID: {id}, BOARD_ID: {boardId}, NAME: {thread.Name}, " +
                $"TEXT: {thread.OriginalPost.Text}");
            return RedirectToAction("ViewList", "Thread", new { boardId });
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> Upload()
        {
            if (Request.Form.Files.Count == 0)
                return Json(ConstHelper.FileError404);
            
            var files = Request.Form.Files;
            var resultPaths = new StringBuilder();
            var user = await GetCurrentUserAsync();

            if (user is null)
                return Json(ConstHelper.UserError404);
            
            for (var i = 0; i < Math.Min(files.Count, ConstHelper.MaxFilesCount); ++i)
            {
                if (!FileHelper.IsNormalFileSize(files[i].Length))
                    return Json(ConstHelper.FileError404);
                    
                var fileName = FileHelper.GetNewFileName(files[i]);
                var path = ConstHelper.TempFolderUrl + "/" + fileName;
                await FileHelper.SaveFileAsync(files[i], path, _environment);
                resultPaths.Append(path + " "); 
            }
            
            return Json(resultPaths.ToString());
        }
        
        [HttpPost]
        [Authorize]
        public JsonResult Unload(string data)
        {
            if (string.IsNullOrEmpty(data) || string.CompareOrdinal(data, ConstHelper.FileError404) == 0)
                return Json(ConstHelper.PathError404);

            FileHelper.RemoveFiles(data.Split(' ').ToList(), _environment);
            
            return Json(ConstHelper.Success);
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