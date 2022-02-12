using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inter.Models
{
    public class User
    {
        [HiddenInput]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [Required(ErrorMessage = "Не указано имя")]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Не указан email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Display(Name = "Email скрыт")]
        public bool IsEmailHidden { get; set; }
        
        [HiddenInput]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Display(Name = "Аватар")]
        public string AvatarUrl { get; set; }
        
        [Display(Name = "Роль")]
        public Role Role { get; set; }
        
        [Display(Name = "Дата деактивации")]
        public DateTime DeactivateDate { get; set; }
        
        [HiddenInput]
        public bool IsDeleted { get; set; }
    }
}