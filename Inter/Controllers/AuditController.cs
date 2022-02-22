using System.Linq;
using System.Threading.Tasks;
using Inter.Helpers;
using Inter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inter.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class AuditController : Controller
    {
        private readonly InterService _db;
        private readonly AuditHelper _audit;

        public AuditController()
        {
            _db = new InterService();
            _audit = new AuditHelper(_db);
        }

        [HttpGet]
        public async Task<IActionResult> ViewList(int page = 0)
        {
            page = page < 0 ? 0 : page;
            page = page * ConstHelper.CountAuditDocumentsPerPage > _audit.Length 
                ? _audit.Length / ConstHelper.CountAuditDocumentsPerPage 
                : page;
            ViewBag.Page = page;
            
            return View((await _audit.GetAuditInfoAsync()).Skip(page * ConstHelper.CountAuditDocumentsPerPage).ToList());
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
            var user = await AccountHelper.GetCurrentUserAsync(HttpContext, _db);
            await _audit.ClearAuditAsync(AccountHelper.GetIpAddress(HttpContext), user, $"USER_ID: {user.Id}");

            return RedirectToAction(nameof(ViewList));
        }
    }
}