using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Inter.Models
{
    public class ThreadPost
    {
        [Display(Name = "Название:")]
        public string Name { get; set; }

        [Display(Name = "Текст поста:")]
        [StringLength(50000, ErrorMessage = "Размер текста выходит за границы")]
        public string Text { get; set; }
        
        [Display(Name = "Файлы:")]
        public string[] FileUrls { get; set; }
        
        [Display(Name = "Закреплён:")]
        public bool IsPinned { get; set; }

        [Required(ErrorMessage = "Не указана роль")]
        [Display(Name = "Чтение для:")]
        public string ReadRoleName { get; set; }
        
        [Required(ErrorMessage = "Не указана роль")]
        [Display(Name = "Отвечать для:")]
        public string WriteRoleName { get; set; }
        
        [Required]
        [HiddenInput]
        public string BoardId { get; set; }
    }
}