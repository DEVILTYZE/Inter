using System.Collections.Generic;
using System.Linq;
using Inter.Helpers;
using MongoDB.Driver;

namespace Inter.Models
{
    public class InterService
    {
        private const string ConnectionString = "mongodb://localhost:27017/interdb";

        public readonly IMongoCollection<Board> Boards;
        public readonly IMongoCollection<AuditEntry> Audit;
        public readonly IMongoCollection<User> Users;
        public readonly IMongoCollection<Role> Roles;

        public InterService()
        {
            var connection = new MongoUrlBuilder(ConnectionString);
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(connection.DatabaseName);
            Boards = db.GetCollection<Board>("Boards");
            Audit = db.GetCollection<AuditEntry>("Audit");
            Users = db.GetCollection<User>("Users");
            Roles = db.GetCollection<Role>("Roles");
            
            var builder = new FilterDefinitionBuilder<Board>();

            if (Boards.Find(builder.Empty).ToList().Any()) 
                return;
            
            ClearDatabase(client);
            CreateDatabase();
        }

        private void CreateDatabase()
        {
            var defaultBoard = new Board
            {
                Name = "DEFAULT",
                Description = "DEFAULT BOARD",
                Threads = new List<Thread>(),
                AccessRoleName = RoleName.Anon
            };

            var roles = new Role[]
            {
                new() { Name = RoleName.Admin },
                new() { Name = RoleName.User },
                new() { Name = RoleName.Anon },
                new() { Name = RoleName.Banned }
            };

            Roles.InsertMany(roles);
            
            var defaultUser = new User
            {
                Name = "InterServer",
                Email = "inter@EMAIL.NET",
                IsEmailHidden = true,
                Role = roles[0]
            };
            defaultUser.Password = AccountHelper.GetHashedPassword(AccountHelper.ConstString, defaultUser.Email);
            
            Boards.InsertOne(defaultBoard);
            Users.InsertOne(defaultUser);

            var audit = new AuditHelper(this);
            audit.Add(typeof(AuditHelper), MethodType.Create, ResultType.Success, "0.0.0.0", defaultUser, 
                "Hello world!");
        }

        private static void ClearDatabase(IMongoClient client) => client.DropDatabase("interdb");
    }
}