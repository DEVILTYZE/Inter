using System.ComponentModel.DataAnnotations;

namespace Inter.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Не указано имя")]
        [Display(Name = "Имя:")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Размер имени может быть от 3 до 30 символов")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Не указан email")]
        [EmailAddress(ErrorMessage = "Указан некорректный email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email:")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Не указан пароль")]
        [Display(Name = "Пароль:")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Размер пароля может быть от 6 до 50 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}