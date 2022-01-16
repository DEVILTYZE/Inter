using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Inter.Models
{
    public class Thread
    {
        [HiddenInput]
        public string Id { get; set; }
        
        [Display(Name = "Название")]
        public string Name { get; set; }

        [HiddenInput]
        public List<Post> Posts { get; set; }
        
        public Post OriginalPost => Posts.First();
        
        [Display(Name = "Закреплён")]
        public bool IsPinned { get; set; }

        [Required(ErrorMessage = "Не указана роль")]
        [Display(Name = "Чтение для")]
        public string ReadRoleName { get; set; }
        
        [Required(ErrorMessage = "Не указана роль")]
        [Display(Name = "Писать для")]
        public string WriteRoleName { get; set; }
        
        [HiddenInput]
        public string FileFolderUrl { get; set; }

        [HiddenInput]
        public string BoardId { get; set; }
    }
}