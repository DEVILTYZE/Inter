using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inter.Models;
using MongoDB.Driver;

namespace Inter.Helpers
{
    public class AuditHelper
    {
        public int Length => (int)_db.Audit.CountDocuments(_builder.Empty);
        
        private const int MaxCountOfEntries = 5000;
        private readonly InterService _db;
        private readonly FilterDefinitionBuilder<AuditEntry> _builder;
        
        public AuditHelper(InterService db)
        {
            _db = db;
            _builder = new FilterDefinitionBuilder<AuditEntry>();
        }

        public void Add(Type item, MethodType method, ResultType result, string ipAddress, User user = null, string info = null)
        {
            var entry = new AuditEntry
            {
                Time = DateTime.Now,
                IpAddress = ipAddress,
                Item = item.Name,
                Method = method,
                Result = result,
                Info = info,
                User = user
            };
            
            _db.Audit.InsertOne(entry);
        }
        
        public async Task AddAsync(Type item, MethodType method, ResultType result, string ipAddress, User user = null, 
            string info = null)
        {
            var entry = new AuditEntry
            {
                Time = DateTime.Now,
                IpAddress = ipAddress,
                Item = item.Name,
                Method = method,
                Result = result,
                Info = info,
                User = user
            };

            await CheckingTheFullness();
            await _db.Audit.InsertOneAsync(entry);
        }

        public async Task<List<AuditEntry>> GetAuditInfoAsync() => await _db.Audit.Find(_builder.Empty).ToListAsync();

        private async Task CheckingTheFullness()
        {
            if (await _db.Audit.CountDocumentsAsync(_builder.Empty) > MaxCountOfEntries)
                await ClearAuditAsync("0.0.0.0", null, " !CLEARED BY BOT!");
        }
        
        public async Task ClearAuditAsync(string ipAddress, User user, string additionalInfo = null)
        {
            await _db.Audit.DeleteManyAsync(_builder.Empty);
            await AddAsync(typeof(AuditHelper), MethodType.Clear, ResultType.Success, ipAddress, user, additionalInfo);
        }
    }
}