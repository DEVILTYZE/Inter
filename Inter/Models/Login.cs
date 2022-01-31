using System.ComponentModel.DataAnnotations;

namespace Inter.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Не указан email или имя")]
        [Display(Name = "Email или имя")]
        public string EmailOrName { get; set; }
        
        [Required(ErrorMessage = "Не указан пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}