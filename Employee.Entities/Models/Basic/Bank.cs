using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.Models.Basic
{
    public class Bank
    {
        [Key]
        public int BankId { get; set; }
        [DisplayName("عنوان بانک")]
        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? BankTitle { get; set; }
    }
}
