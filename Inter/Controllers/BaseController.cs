using Inter.Helpers;
using Inter.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Inter.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly InterService Db;
        protected readonly AuditHelper Audit;
        protected readonly FilterDefinitionBuilder<Board> Builder;
        protected readonly FilterDefinitionBuilder<Role> BuilderRole;
        protected readonly IWebHostEnvironment Environment;

        protected BaseController(IWebHostEnvironment environment)
        {
            Db = new InterService();
            Audit = new AuditHelper(Db);
            Builder = new FilterDefinitionBuilder<Board>();
            BuilderRole = new FilterDefinitionBuilder<Role>();
            Environment = environment;
        }
    }
}