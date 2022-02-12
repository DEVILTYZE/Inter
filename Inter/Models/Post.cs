using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;

namespace Inter.Models
{
    public class Post
    {
        [HiddenInput]
        public string Id { get; set; }

        [Required(ErrorMessage = "Не указан текст")]
        [Display(Name = "Текст поста")]
        [StringLength(50000, ErrorMessage = "Размер текста выходит за границы")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        
        [Display(Name = "Файлы")]
        public List<string> FileNames { get; set; }
        
        [Display(Name = "Время создания")]
        [BsonElement]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreationTime { get; set; }
        
        [HiddenInput]
        public string PosterId { get; set; }

        [HiddenInput] 
        public string ThreadId { get; set; }
        
        [HiddenInput]
        public string BoardId { get; set; }
        
        [HiddenInput]
        public bool IsDeleted { get; set; }
    }
}