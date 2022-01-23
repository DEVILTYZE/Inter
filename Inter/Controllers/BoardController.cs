using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Inter.Helpers;
using Microsoft.AspNetCore.Mvc;
using Inter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inter.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class BoardController : Controller
    {
        private readonly InterService _db;
        private readonly AuditHelper _audit;
        private readonly FilterDefinitionBuilder<Board> _builder;
        private readonly FilterDefinitionBuilder<Role> _builderRole;
        private readonly IWebHostEnvironment _environment;

        public BoardController(IWebHostEnvironment environment)
        {
            _db = new InterService();
            _audit = new AuditHelper(_db);
            _builder = new FilterDefinitionBuilder<Board>();
            _builderRole = new FilterDefinitionBuilder<Role>();
            _environment = environment;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ViewList()
        {
            ViewBag.Roles = await _db.Roles.Find(_builderRole.Empty).ToListAsync();
            ViewBag.Boards = await _db.Boards.Find(_builder.Empty).ToListAsync();
            
            return View(await _db.Boards.Find(_builder.Empty).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = AccountHelper.GetSelectList(await _db.Roles.Find(_builderRole.Empty).ToListAsync());
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id", "Name", "Image", "Description", "AccessRoleName", 
            "Threads")] Board board)
        {
            ViewBag.Roles = AccountHelper.GetSelectList(await _db.Roles.Find(_builderRole.Empty).ToListAsync());
            
            if (!ModelState.IsValid)
                return View(board);

            board.Threads = new List<Thread>();
            
            await _db.Boards.InsertOneAsync(board);

            var options = new ReplaceOptions { IsUpsert = true };
            // FILE UPDATE
            await _db.Boards.ReplaceOneAsync(_builder.Eq("_id", new ObjectId(board.Id)), board, options);

            await _audit.AddAsync(typeof(Board), MethodType.Create, ResultType.Success, AccountHelper.GetIpAddress(HttpContext), 
                await GetCurrentUserAsync(), $"ID: {board.Id}");
            return RedirectToAction(nameof(ViewList));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id is null)
                return RedirectToAction(nameof(ViewList));

            var board = await _db.Boards.Find(_builder.Eq("_id", new ObjectId(id))).FirstOrDefaultAsync();

            if (board is null)
                return RedirectToAction(nameof(ViewList));
            
            ViewBag.Roles = AccountHelper.GetSelectList(await _db.Roles.Find(_builderRole.Empty).ToListAsync());
            return View(board);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id", "Name", "Image", "Description", "AccessRoleName", 
            "Threads")] Board board)
        {
            ViewBag.Roles = AccountHelper.GetSelectList(await _db.Roles.Find(_builderRole.Empty).ToListAsync());
            var user = await GetCurrentUserAsync();
            
            if (!ModelState.IsValid)
            {
                await _audit.AddAsync(typeof(Board), MethodType.Edit, ResultType.Failure, AccountHelper.GetIpAddress(HttpContext), 
                    user, $"ID: {board.Id}");
                return View(board);
            }
            
            // FILE UPDATE

            var options = new ReplaceOptions { IsUpsert = true };

            await _db.Boards.ReplaceOneAsync(_builder.Eq("_id", new ObjectId(board.Id)), board, options);
            await _audit.AddAsync(typeof(Board), MethodType.Edit, ResultType.Success, AccountHelper.GetIpAddress(HttpContext), 
                user, $"ID: {board.Id}");
            return RedirectToAction("ViewList", "Thread", new { boardId = board.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Remove(string id)
        {
            if (id is null)
                return RedirectToAction(nameof(ViewList));
            
            var board = await _db.Boards.Find(_builder.Eq("_id", new ObjectId(id))).FirstOrDefaultAsync();
          
            return board is null ? RedirectToAction(nameof(ViewList)) : View(board);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var board = await _db.Boards.Find(_builder.Eq("_id", new ObjectId(id.ToString()))).FirstOrDefaultAsync();
            FileHelper.RemoveFilesFolder(board?.ImageUrl[..board.ImageUrl.LastIndexOf('/')], _environment);
            
            await _db.Boards.DeleteOneAsync(_builder.Eq("_id", new ObjectId(id.ToString())));
            await _audit.AddAsync(typeof(Board), MethodType.Remove, ResultType.Success, AccountHelper.GetIpAddress(HttpContext),
                await GetCurrentUserAsync(), $"ID: {id.ToString()}");
            return RedirectToAction(nameof(ViewList));
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var builder = new FilterDefinitionBuilder<User>();

            if (HttpContext.User.Identity is null)
                throw new Exception("Class: AccountController; Method: GetCurrentUser.");
            
            var userName = HttpContext.User.Identity.Name;
            
            return await _db.Users.Find(builder.Regex("Name", new BsonRegularExpression(userName))).FirstAsync();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}