using System.ComponentModel.DataAnnotations;

namespace Inter.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Не указан текущий пароль пароль")]
        [Display(Name = "Текущий пароль")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Размер пароля может быть от 6 до 50 символов")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        
        [Required(ErrorMessage = "Не указан новый пароль")]
        [Display(Name = "Новый пароль")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Размер пароля может быть от 6 до 50 символов")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}