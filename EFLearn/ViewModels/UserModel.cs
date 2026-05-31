using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EFLearn.ViewModels
{
    public class UserModel
    {
        [Required(ErrorMessage ="ایمیل الزامی است")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [MinLength(8,ErrorMessage ="پسوورد باید بالای هشت رقم باشد")]
        [DataType(DataType.Password)]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "رمز عبور باید حداقل ۸ کاراکتر و شامل حروف بزرگ، کوچک، عدد و نشانه باشد")]
        public string? Password { get; set; }
        [Compare("Password",ErrorMessage ="رمز عبور یکی نیست")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        [Compare("Email", ErrorMessage = "ایمیل یکی نیست")]
        public string? ConfirmEmail { get; set; }
        public string? RoleSelect { get; set; } 
        public List<SelectListItem>? RoleItem { get; set; }
    }
}
