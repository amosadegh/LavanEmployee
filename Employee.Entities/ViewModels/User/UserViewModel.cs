using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.ViewModels.User
{
    // ViewModels/UserViewModel.cs
    public class UserViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "نام و نام خانوادگی الزامی است")]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "شماره موبایل الزامی است")]
        [Display(Name = "شماره پرسنلی (نام کاربری)")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "تکرار رمز عبور")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "رمزها یکسان نیستند")]
        public string? ConfirmPassword { get; set; }
        [Required(ErrorMessage = "انتخاب نقش الزامی است")]
        [Display(Name = "نقش")]
        public string RoleName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

    }

}
