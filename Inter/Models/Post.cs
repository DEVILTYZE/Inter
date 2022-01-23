using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

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
        public DateTime CreationTime { get; set; }
        
        [HiddenInput]
        public User Poster { get; set; }

        [HiddenInput] 
        public string ThreadId { get; set; }
        
        [HiddenInput]
        public string BoardId { get; set; }
    }
}