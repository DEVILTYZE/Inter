using System;
using System.IO;
using System.Text.Json;
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
                return Json(ConstError.Failure);
            
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddMonths(1),
                HttpOnly = true
            };
            
            Response.Cookies.Append("theme", theme, options);

            return Json(ConstError.Success);
        }
        
        [HttpGet]
        public JsonResult GetConfig()
        {
            try
            {
                using var sr = new StreamReader(ConstHelper.ConfigName);
                var result = new JsonResult(sr.ReadToEnd())
                {
                    ContentType = "json",
                    SerializerSettings = new JsonSerializerOptions(JsonSerializerDefaults.Web)
                };

                return result;
            }
            catch
            {
                return Json(ConstError.Failure);
            }
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