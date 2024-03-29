﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Inter.Helpers;
using Inter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Inter.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class AuditController : Controller
    {
        private readonly InterService _db;
        private readonly AuditHelper _audit;
        private readonly FilterDefinitionBuilder<User> _builder;

        public AuditController()
        {
            _db = new InterService();
            _audit = new AuditHelper(_db);
            _builder = new FilterDefinitionBuilder<User>();
        }

        [HttpGet]
        public async Task<IActionResult> ViewList(int page = 0)
        {
            ViewBag.Page = page;
            
            return View(await _audit.GetAuditInfoAsync());
        }

        [HttpPost]
        public async Task<IActionResult> ViewList(string searchPattern)
        {
            var auditList = await _audit.GetAuditInfoAsync();
            
            return string.IsNullOrEmpty(searchPattern) 
                ? View(auditList.ToList()) 
                : View(auditList.Where(entry => entry.Contains(searchPattern)).ToList());
        }

        public async Task<IActionResult> Info(string id, int page)
        {
            var auditList = await _audit.GetAuditInfoAsync();
            
            return string.IsNullOrEmpty(id) 
                ? RedirectToAction(nameof(ViewList), new{ page }) 
                : View(auditList.Where(entry => string.CompareOrdinal(entry.Id, id) == 0).ToList()[0]);
        }

        [HttpGet]
        public async Task<IActionResult> ClearAudit()
        {
            var user = await GetCurrentUserAsync();
            await _audit.ClearAuditAsync(AccountHelper.GetIpAddress(HttpContext), user, $"USER_ID: {user.Id}");

            return RedirectToAction(nameof(ViewList));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Page404() => View();

        private async Task<User> GetCurrentUserAsync()
        {
            if (HttpContext.User.Identity is null)
                throw new Exception("Class: AccountController; Method: GetCurrentUser.");
            
            var userName = HttpContext.User.Identity.Name;
            
            return await _db.Users.Find(_builder.Regex("Name", new BsonRegularExpression(userName))).FirstAsync();
        }
    }
}