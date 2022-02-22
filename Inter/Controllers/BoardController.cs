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
    public class BoardController : BaseController
    {
        // private readonly InterService _db;
        // private readonly AuditHelper _audit;
        // private readonly FilterDefinitionBuilder<Board> _builder;
        // private readonly FilterDefinitionBuilder<Role> _builderRole;
        // private readonly IWebHostEnvironment _environment;
        //
        // public BoardController(IWebHostEnvironment environment)
        // {
        //     _db = new InterService();
        //     _audit = new AuditHelper(_db);
        //     _builder = new FilterDefinitionBuilder<Board>();
        //     _builderRole = new FilterDefinitionBuilder<Role>();
        //     _environment = environment;
        // }
        
        public BoardController(IWebHostEnvironment environment) : base(environment) {}

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ViewList()
        {
            ViewBag.Roles = await Db.Roles.Find(BuilderRole.Empty).ToListAsync();
            ViewBag.Boards = await Db.Boards.Find(Builder.Empty).ToListAsync();
            
            return View(await Db.Boards.Find(Builder.Empty).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = AccountHelper.GetSelectList(await Db.Roles.Find(BuilderRole.Empty).ToListAsync());
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id", "Name", "Image", "Description", "AccessRoleName", 
            "Threads")] Board board)
        {
            ViewBag.Roles = AccountHelper.GetSelectList(await Db.Roles.Find(BuilderRole.Empty).ToListAsync());
            
            if (!ModelState.IsValid)
                return View(board);

            board.Threads = new List<Thread>();
            
            await Db.Boards.InsertOneAsync(board);

            var options = new ReplaceOptions { IsUpsert = true };
            // FILE UPDATE
            await Db.Boards.ReplaceOneAsync(Builder.Eq("_id", new ObjectId(board.Id)), board, options);

            await Audit.AddAsync(typeof(Board), MethodType.Create, ResultType.Success, AccountHelper.GetIpAddress(HttpContext), 
                await AccountHelper.GetCurrentUserAsync(HttpContext, Db), $"ID: {board.Id}");
            return RedirectToAction(nameof(ViewList));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id is null)
                return RedirectToAction(nameof(ViewList));

            var board = await Db.Boards.Find(Builder.Eq("_id", new ObjectId(id))).FirstOrDefaultAsync();

            if (board is null)
                return RedirectToAction(nameof(ViewList));
            
            ViewBag.Roles = AccountHelper.GetSelectList(await Db.Roles.Find(BuilderRole.Empty).ToListAsync());
            return View(board);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id", "Name", "Image", "Description", "AccessRoleName", 
            "Threads")] Board board)
        {
            ViewBag.Roles = AccountHelper.GetSelectList(await Db.Roles.Find(BuilderRole.Empty).ToListAsync());
            var user = await AccountHelper.GetCurrentUserAsync(HttpContext, Db);
            
            if (!ModelState.IsValid)
            {
                await Audit.AddAsync(typeof(Board), MethodType.Edit, ResultType.Failure, AccountHelper.GetIpAddress(HttpContext), 
                    user, $"ID: {board.Id}");
                return View(board);
            }
            
            // FILE UPDATE

            var options = new ReplaceOptions { IsUpsert = true };

            await Db.Boards.ReplaceOneAsync(Builder.Eq("_id", new ObjectId(board.Id)), board, options);
            await Audit.AddAsync(typeof(Board), MethodType.Edit, ResultType.Success, AccountHelper.GetIpAddress(HttpContext), 
                user, $"ID: {board.Id}");
            return RedirectToAction("ViewList", "Thread", new { boardId = board.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Remove(string id)
        {
            if (id is null)
                return RedirectToAction(nameof(ViewList));
            
            var board = await Db.Boards.Find(Builder.Eq("_id", new ObjectId(id))).FirstOrDefaultAsync();
          
            return board is null ? RedirectToAction(nameof(ViewList)) : View(board);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var board = await Db.Boards.Find(Builder.Eq("_id", new ObjectId(id.ToString()))).FirstOrDefaultAsync();
            FileHelper.RemoveFilesFolder(board?.ImageUrl[..board.ImageUrl.LastIndexOf('/')], Environment);
            
            await Db.Boards.DeleteOneAsync(Builder.Eq("_id", new ObjectId(id.ToString())));
            await Audit.AddAsync(typeof(Board), MethodType.Remove, ResultType.Success, AccountHelper.GetIpAddress(HttpContext),
                await AccountHelper.GetCurrentUserAsync(HttpContext, Db), $"ID: {id.ToString()}");
            return RedirectToAction(nameof(ViewList));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}