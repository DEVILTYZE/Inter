using System;
using Inter.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inter.Controllers
{
    public class ForumController : Controller
    {
        [HttpPost]
        public JsonResult LoadTheme(string theme)
        {
            if (string.IsNullOrEmpty(theme))
                theme = ConstHelper.LightThemeName;
            else if (string.CompareOrdinal(theme, ConstHelper.LightThemeName) != 0 &&
                string.CompareOrdinal(theme, ConstHelper.DarkThemeName) != 0)
                return Json(ConstHelper.Failure);
            
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddMonths(1),
                HttpOnly = true
            };
            
            Response.Cookies.Append("theme", theme, options);

            return Json(ConstHelper.Success);
        }

        [HttpPost]
        public IActionResult Search(string searchPattern)
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Page404() => View();
    }
}