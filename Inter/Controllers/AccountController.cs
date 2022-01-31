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
    public class AccountController : Controller
    {
        private readonly InterService _db;
        private readonly AuditHelper _audit;
        private readonly FilterDefinitionBuilder<User> _builder;
        private readonly FilterDefinitionBuilder<Role> _builderRole;
        private readonly IWebHostEnvironment _environment;
        private readonly Role _bannedRole;

        public AccountController(IWebHostEnvironment environment)
        {
            _db = new InterService();
            _audit = new AuditHelper(_db);
            _builder = new FilterDefinitionBuilder<User>();
            _builderRole = new FilterDefinitionBuilder<Role>();
            _environment = environment;
            _bannedRole = _db.Roles.Find(_builderRole.Eq("Name", RoleName.Banned)).FirstOrDefault();
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewList()
        {
            await _audit.AddAsync(typeof(User), MethodType.Look, ResultType.Success, AccountHelper.GetIpAddress(HttpContext), 
                await GetCurrentUserAsync());
            return View(await _db.Users.Find(_builder.Empty).ToListAsync());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Info(string id = null)
        {
            if (id is null)
                return View(await GetCurrentUserAsync());

            var user = await _db.Users.Find(_builder.Eq("_id", new ObjectId(id))).FirstOrDefaultAsync();
            
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

            var user = await _db.Users.Find(_builder.Regex(
                "Email", new BsonRegularExpression(model.EmailOrName))).FirstOrDefaultAsync() 
                       ?? await _db.Users.Find(_builder.Regex(
                "Name", new BsonRegularExpression(model.EmailOrName))).FirstOrDefaultAsync();

            if (user is null)
            {
                ModelState.AddModelError("", "Некорректные email или пароль");
                
                return View(model);
            }

            if (string.CompareOrdinal(user.Role.Id, _bannedRole.Id) == 0)
            {
                ModelState.AddModelError("", "Вы забанены");
                
                return View(model);
            }
                
            if (!CheckPassword(model.Password, user))
            {
                ModelState.AddModelError("", "Некорректные email или пароль");
                
                await _audit.AddAsync(typeof(Login), MethodType.LogIn, ResultType.Failure, AccountHelper.GetIpAddress(HttpContext),
                    null, $"USER_ID: {user.Id}");
                return View(model);
            }

            await AuthenticateAsync(user);
            await _audit.AddAsync(typeof(User), MethodType.LogIn, ResultType.Success, AccountHelper.GetIpAddress(HttpContext),
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

            var user = await _db.Users.Find(_builder.Regex(
                "Email", new BsonRegularExpression(model.Email))).FirstOrDefaultAsync();
                       
            if (user is not null)
            {
                ModelState.AddModelError("Email", "Пользователь с таким email'ом уже существует");
                
                return View(model);
            }
            
            user = await _db.Users.Find(_builder.Regex(
                "Name", new BsonRegularExpression(model.Email))).FirstOrDefaultAsync();

            if (user is not null)
            {
                ModelState.AddModelError("Name", "Пользователь с таким именем уже существует");
                
                return View(model);
            }

            var newUser = new User
            {
                Name = model.Name,
                Email = model.Email,
                IsEmailHidden = true,
                Password = AccountHelper.GetHashedPassword(model.Password, model.Email),
                Role = await _db.Roles.Find(_builderRole.Eq("Name", RoleName.User)).FirstOrDefaultAsync()
            };
            
            await _db.Users.InsertOneAsync(newUser);
            await AuthenticateAsync(newUser);
            await _audit.AddAsync(typeof(User), MethodType.Create, ResultType.Success, AccountHelper.GetIpAddress(HttpContext),
                newUser, $"USER_ID: {newUser.Id}");
            return RedirectToAction(nameof(Info));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit() => View(await GetCurrentUserAsync());

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit([Bind("Name", "Email", "IsEmailHidden", "AvatarUrl")] 
            User user, string filePathInput)
        {
            if (!ModelState.IsValid)
                return View(user);

            var filter = _builder.Eq("_id", new ObjectId(user.Id));
            var options = new ReplaceOptions { IsUpsert = true };
            var currentUser = await GetCurrentUserAsync();

            if (currentUser is null)
                return RedirectToAction("Page404", "Forum");

            user.Id = currentUser.Id;
            user.Password = currentUser.Password;
            user.Role = currentUser.Role;
            
            if (!string.IsNullOrEmpty(filePathInput) && !string.IsNullOrWhiteSpace(filePathInput))
                FileHelper.UpdateFilePathsUser(filePathInput, user, _environment);
            
            await _db.Users.ReplaceOneAsync(filter, user, options);
            await AuthenticateAsync(user);
            await _audit.AddAsync(typeof(User), MethodType.Edit, ResultType.Success, AccountHelper.GetIpAddress(HttpContext),
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

            var user = await GetCurrentUserAsync();
            
            if (CheckPassword(model.CurrentPassword, user))
            {
                user.Password = AccountHelper.GetHashedPassword(model.NewPassword, user.Email);
                var filter = _builder.Eq("_id", new ObjectId(user.Id));
                var options = new ReplaceOptions { IsUpsert = true };
            
                await _db.Users.ReplaceOneAsync(filter, user, options);
                await AuthenticateAsync(user);
                await _audit.AddAsync(typeof(ChangePassword), MethodType.Edit, ResultType.Success, 
                    AccountHelper.GetIpAddress(HttpContext), user, $"USER_ID: {user.Id}");
                return RedirectToAction(nameof(Edit));
            }
            
            ModelState.AddModelError("CurrentPassword", "Некорректный текущий пароль");
            
            await _audit.AddAsync(typeof(ChangePassword), MethodType.Edit, ResultType.Failure, 
                AccountHelper.GetIpAddress(HttpContext), user, $"USER_ID: {user.Id}");
            return View(model);

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RemoveAvatar()
        {
            var user = await GetCurrentUserAsync();

            if (user is null) 
                return RedirectToAction("Page404", "Forum");
            
            user.AvatarUrl = string.Empty;
            var filter = _builder.Eq("_id", new ObjectId(user.Id));
            var options = new ReplaceOptions { IsUpsert = true };
            
            await _db.Users.ReplaceOneAsync(filter, user, options);
            await AuthenticateAsync(user);
            await _audit.AddAsync(typeof(User), MethodType.Edit, ResultType.Success, 
                AccountHelper.GetIpAddress(HttpContext), user, $"AVATAR | USER_ID: {user.Id}");
            return RedirectToAction(nameof(Edit));
        }
        
        [HttpPost]
        [Authorize]
        public async Task<JsonResult> Upload()
        {
            if (Request.Form.Files.Count == 0)
                return Json("FILE_ERROR_404");
            
            var file = Request.Form.Files[0];
            var fileName = FileHelper.GetNewFileName(file);
            var user = await GetCurrentUserAsync();

            if (user is null)
                return Json("USER_ERROR_404");
            
            var path = await FileHelper.SaveAccountImageAsyncOrDefault(file, user.Id, fileName, _environment);
            return Json(path);
        }
        
        [HttpPost]
        [Authorize]
        public JsonResult Unload(string data)
        {
            if (string.IsNullOrEmpty(data))
                return Json("PATH_ERROR_404");

            var path = data[..data.LastIndexOf('/')];
            var sb = new StringBuilder(data + " ");
            sb.Append(path + "/normal.jpg ");
            sb.Append(path + "/small.jpg");

            FileHelper.RemoveFiles(sb.ToString().Split(' ').ToList(), _environment);
            
            return Json("SUCCESS");
        }

        [HttpGet]
        [Authorize(Roles = RoleName.Admin + ", " + RoleName.Moderator)]
        public async Task<IActionResult> Ban(string userId, DateTime? date = null)
        {
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction(nameof(ViewList));

            var user = await _db.Users.Find(_builder.Eq("_id", new ObjectId(userId))).FirstOrDefaultAsync();
            
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

        private async Task<User> GetCurrentUserAsync()
        {
            if (HttpContext.User.Identity is null)
                throw new Exception("Class: AccountController; Method: GetCurrentUser.");
            
            var userName = HttpContext.User.Identity.Name;
            
            return await _db.Users.Find(_builder.Regex("Name", new BsonRegularExpression(userName))).FirstAsync();
        }
    }
}