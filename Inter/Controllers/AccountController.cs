using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Inter.Helpers;
using Inter.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inter.Controllers
{
    public class AccountController : BaseController
    {
        // private readonly InterService _db;
        // private readonly AuditHelper _audit;
        // private readonly FilterDefinitionBuilder<User> _builder;
        // private readonly FilterDefinitionBuilder<Role> _builderRole;
        // private readonly IWebHostEnvironment _environment;
        //
        // public AccountController(IWebHostEnvironment environment)
        // {
        //     _db = new InterService();
        //     _audit = new AuditHelper(_db);
        //     _builder = new FilterDefinitionBuilder<User>();
        //     _builderRole = new FilterDefinitionBuilder<Role>();
        //     _environment = environment;
        //     _bannedRole = _db.Roles.Find(_builderRole.Eq("Name", RoleName.Banned)).FirstOrDefault();
        // }
        
        private readonly Role _bannedRole;
        private readonly FilterDefinitionBuilder<User> _builderUser;

        public AccountController(IWebHostEnvironment environment) : base(environment)
        {
            _builderUser = new FilterDefinitionBuilder<User>();
            _bannedRole = Db.Roles.Find(BuilderRole.Eq("Name", RoleName.Banned)).FirstOrDefault();
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewList()
        {
            await Audit.AddAsync(typeof(User), MethodType.Look, ResultType.Success, AccountHelper.GetIpAddress(HttpContext), 
                await AccountHelper.GetCurrentUserAsync(HttpContext, Db));
            return View(await Db.Users.Find(_builderUser.Empty).ToListAsync());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Info(string id = null)
        {
            if (id is null)
                return View(await AccountHelper.GetCurrentUserAsync(HttpContext, Db));

            var user = await Db.Users.Find(_builderUser.Eq("_id", new ObjectId(id))).FirstOrDefaultAsync();
            
            return user is null ? RedirectToAction("ViewList", "Account") : View(user);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("EmailOrName", "Password")]Login model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
        
            if (!ModelState.IsValid)
                return View(model);

            var user = await Db.Users.Find(_builderUser.Or(_builderUser.Eq("Name", 
                    model.EmailOrName), _builderUser.Eq("Email", model.EmailOrName))).FirstOrDefaultAsync();

            if (user is null)
            {
                ModelState.AddModelError("", ConstError.WrongEmailOrPassword);
                
                return View(model);
            }

            if (string.CompareOrdinal(user.Role.Id, _bannedRole.Id) == 0)
            {
                ModelState.AddModelError("", ConstError.YouAreBanned);
                
                return View(model);
            }
                
            if (!CheckPassword(model.Password.Trim(), user))
            {
                ModelState.AddModelError("", ConstError.WrongEmailOrPassword);
                
                await Audit.AddAsync(typeof(Login), MethodType.LogIn, ResultType.Failure, AccountHelper.GetIpAddress(HttpContext),
                    null, $"USER_ID: {user.Id}");
                return View(model);
            }

            await AuthenticateAsync(user);
            await Audit.AddAsync(typeof(User), MethodType.LogIn, ResultType.Success, AccountHelper.GetIpAddress(HttpContext),
                user, $"USER_ID: {user.Id}");
            return Redirect(returnUrl);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect(returnUrl);
        }

        [HttpGet]
        public IActionResult Register() => View();
        
        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await Db.Users.Find(_builderUser.Eq("Email", model.Email)).FirstOrDefaultAsync();
                       
            if (user is not null)
            {
                ModelState.AddModelError("Email", ConstError.UserEmailExist);
                
                return View(model);
            }
            
            user = await Db.Users.Find(_builderUser.Eq("Name", model.Name)).FirstOrDefaultAsync();

            if (user is not null)
            {
                ModelState.AddModelError("Name", ConstError.UserNameExist);
                
                return View(model);
            }

            var newUser = new User
            {
                Name = model.Name,
                Email = model.Email,
                IsEmailHidden = true,
                Password = AccountHelper.GetHashedPassword(model.Password, model.Email),
                Role = await Db.Roles.Find(BuilderRole.Eq("Name", RoleName.User)).FirstOrDefaultAsync()
            };
            
            await Db.Users.InsertOneAsync(newUser);
            await AuthenticateAsync(newUser);
            await Audit.AddAsync(typeof(User), MethodType.Create, ResultType.Success, AccountHelper.GetIpAddress(HttpContext),
                newUser, $"USER_ID: {newUser.Id}");
            return RedirectToAction(nameof(Info));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit() => View(await AccountHelper.GetCurrentUserAsync(HttpContext, Db));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit([Bind("Name", "Email", "IsEmailHidden", "AvatarUrl", "Posts")] 
            User user, string filePathInput)
        {
            if (!ModelState.IsValid)
                return View(user);

            var currentUser = await AccountHelper.GetCurrentUserAsync(HttpContext, Db);
            var sameUsers = await Db.Users.Find(_builderUser.Eq("Email", user.Email)).ToListAsync();
            
            if (sameUsers.Count > 1 && sameUsers.Any(thisUser => string.CompareOrdinal(thisUser.Id, currentUser.Id) != 0))
            {
                ModelState.AddModelError("Email", ConstError.UserNameExist);
                
                return View(user);
            }
            
            sameUsers = await Db.Users.Find(_builderUser.Eq("Name", user.Name)).ToListAsync();

            if (sameUsers.Count > 1 && sameUsers.Any(thisUser => string.CompareOrdinal(thisUser.Id, currentUser.Id) != 0))
            {
                ModelState.AddModelError("Name", ConstError.UserNameExist);
                
                return View(user);
            }

            if (currentUser is null)
                return RedirectToAction("Page404", "Forum");

            user.Id = currentUser.Id;
            user.Password = currentUser.Password;
            user.Role = currentUser.Role;
            
            if (!string.IsNullOrEmpty(filePathInput) && !string.IsNullOrWhiteSpace(filePathInput))
                FileHelper.UpdateFilePathsUser(filePathInput, user, Environment);

            // POST
            
            var filter = _builderUser.Eq("_id", new ObjectId(user.Id));
            var options = new ReplaceOptions { IsUpsert = true };
            
            await Db.Users.ReplaceOneAsync(filter, user, options);
            await AuthenticateAsync(user);
            await Audit.AddAsync(typeof(User), MethodType.Edit, ResultType.Success, AccountHelper.GetIpAddress(HttpContext),
                user, $"ID: {user.Id}");
            return RedirectToAction(nameof(Edit));
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword([Bind("CurrentPassword", "NewPassword", "ConfirmPassword")] 
            ChangePassword model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await AccountHelper.GetCurrentUserAsync(HttpContext, Db);
            
            if (CheckPassword(model.CurrentPassword, user))
            {
                user.Password = AccountHelper.GetHashedPassword(model.NewPassword, user.Email);
                var filter = _builderUser.Eq("_id", new ObjectId(user.Id));
                var options = new ReplaceOptions { IsUpsert = true };
            
                await Db.Users.ReplaceOneAsync(filter, user, options);
                await AuthenticateAsync(user);
                await Audit.AddAsync(typeof(ChangePassword), MethodType.Edit, ResultType.Success, 
                    AccountHelper.GetIpAddress(HttpContext), user, $"USER_ID: {user.Id}");
                return RedirectToAction(nameof(Edit));
            }
            
            ModelState.AddModelError("CurrentPassword", ConstError.WrongCurrentPassword);
            
            await Audit.AddAsync(typeof(ChangePassword), MethodType.Edit, ResultType.Failure, 
                AccountHelper.GetIpAddress(HttpContext), user, $"USER_ID: {user.Id}");
            return View(model);

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RemoveAvatar()
        {
            var user = await AccountHelper.GetCurrentUserAsync(HttpContext, Db);

            if (user is null) 
                return RedirectToAction("Page404", "Forum");
            
            user.AvatarUrl = string.Empty;
            var filter = _builderUser.Eq("_id", new ObjectId(user.Id));
            var options = new ReplaceOptions { IsUpsert = true };
            
            await Db.Users.ReplaceOneAsync(filter, user, options);
            await AuthenticateAsync(user);
            await Audit.AddAsync(typeof(User), MethodType.Edit, ResultType.Success, 
                AccountHelper.GetIpAddress(HttpContext), user, $"AVATAR | USER_ID: {user.Id}");
            return RedirectToAction(nameof(Edit));
        }
        
        [HttpPost]
        [Authorize]
        public async Task<JsonResult> Upload()
        {
            if (Request.Form.Files.Count == 0)
                return Json(ConstError.FileError404);
            
            var file = Request.Form.Files[0];
            var fileName = FileHelper.GetNewFileName(file);
            var user = await AccountHelper.GetCurrentUserAsync(HttpContext, Db);

            if (user is null)
                return Json(ConstError.UserError404);
            
            var path = await FileHelper.SaveAccountImageAsyncOrDefault(file, user.Id, fileName, Environment);

            return Json(path ?? ConstError.FileError404);
        }
        
        [HttpPost]
        [Authorize]
        public JsonResult Unload(string data)
        {
            if (string.IsNullOrEmpty(data))
                return Json(ConstError.PathError404);

            var path = data[..data.LastIndexOf('/')];
            var sb = new StringBuilder(data + " ");
            sb.Append(path + "/normal.jpg ");
            sb.Append(path + "/small.jpg");

            FileHelper.RemoveFiles(sb.ToString().Split(' ').ToList(), Environment);
            
            return Json(ConstError.Success);
        }

        [HttpGet]
        [Authorize(Roles = RoleName.Admin + ", " + RoleName.Moderator)]
        public async Task<IActionResult> Ban(string userId, DateTime? date = null)
        {
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction(nameof(ViewList));

            var user = await Db.Users.Find(_builderUser.Eq("_id", new ObjectId(userId))).FirstOrDefaultAsync();
            
            if (user is null)
                return RedirectToAction(nameof(ViewList));

            if (AccountHelper.GetAccessIndex(HttpContext.User, user.Role.Name) <= 0)
                return RedirectToAction("Page404", "Forum");
            
            user.Role = _bannedRole;
            user.DeactivateDate = date ?? DateTime.MaxValue; // Добавить бан по доскам (но это потом)
            
            return RedirectToAction(nameof(Info), new { userId });
        }

        private static bool CheckPassword(string inputPassword, User user)
        {
            var hashedPassword = AccountHelper.GetHashedPassword(inputPassword, user.Email);

            return string.CompareOrdinal(hashedPassword, user.Password) == 0;
        }
        
        private async Task AuthenticateAsync(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name),
                new("email", user.Email),
                new("avatarUrl", user.AvatarUrl ?? string.Empty)
            };
            var id = new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}