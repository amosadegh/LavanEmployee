using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "شماره پرسنلی را وارد نمایید.")]
        //[EmailAddress]
        [DisplayName("شماره پرسنلی")]
        public string? EmployeeNumber { get; set; }
        [Required(ErrorMessage = "رمز را وارد نمایدد")]
        [DataType(DataType.Password)]
        [DisplayName("رمز ورود")]
        public string? Password { get; set; }

        [Display(Name = "مرا بخاطر بسپار ")]
        public bool RememberMe { get; set; }
    }
}

