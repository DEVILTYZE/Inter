using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inter.Models
{
    public class Role
    {
        [HiddenInput]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [Display(Name = "Название")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Размер имени может быть от 3 до 30 символов")]
        public string Name { get; set; }
    }
}