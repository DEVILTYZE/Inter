using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inter.Models
{
    public class Board
    {
        [HiddenInput]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [Required(ErrorMessage = "Не указано название")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Размер имени может быть от 3 до 20 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        
        [Display(Name = "Аватар")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
        
        [Required(ErrorMessage = "Не указано описание")]
        [StringLength(270, ErrorMessage = "Размер описания может быть от 1 до 270 символов")]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        
        [HiddenInput]
        public List<Thread> Threads { get; set; }
        
        [Display(Name = "Доступ для роли")]
        public string AccessRoleName { get; set; }
    }
}