using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inter.Models
{
    public enum MethodType
    {
        [Description("Создать")]
        Create,
        
        [Description("Удалить")]
        Remove,
        
        [Description("Редактировать")]
        Edit,
        
        [Description("Очистить")]
        Clear,
        
        [Description("Вход")]
        LogIn,
        
        [Description("Просмотр")]
        Look
    }

    public enum ResultType
    {
        [Description("Успех")]
        Success,
        
        [Description("Провал")]
        Failure
    }
    
    public class AuditEntry
    {
        [HiddenInput]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [HiddenInput]
        [DataType(DataType.Date)]
        public DateTime Time { get; set; }
        
        [HiddenInput]
        public string IpAddress { get; set; }
        
        [HiddenInput]
        public User User { get; set; }

        [HiddenInput]
        public MethodType Method { get; set; }
        
        [HiddenInput]
        public ResultType Result { get; set; }

        [HiddenInput] 
        public string Item { get; set; }
        
        [HiddenInput]
        public string Info { get; set; }

        public bool Contains(string pattern) 
            => Time.ToString(CultureInfo.CurrentCulture).ToUpper().Contains(pattern.ToUpper()) || IpAddress.Contains(pattern) ||
               User.Name.ToUpper().Contains(pattern.ToUpper()) || User.Email.ToUpper().Contains(pattern.ToUpper()) || 
               string.CompareOrdinal(Method.ToString().ToUpper(), pattern.ToUpper()) == 0 || 
               string.CompareOrdinal(Result.ToString().ToUpper(), pattern.ToUpper()) == 0 || 
               Item.ToUpper().Contains(pattern.ToUpper()) || Info is not null && 
               Info.ToUpper().Contains(pattern.ToUpper());
        
    }
}