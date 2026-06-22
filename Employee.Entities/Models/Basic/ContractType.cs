using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.Models.Basic
{
    public class ContractType
    {
        [Key]
        public int ContractTypeId { get; set; }
        [DisplayName("نوع قرارداد")]
        [Required(ErrorMessage = "عنوان نوع قرارداد را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? ContractTypeTitle { get; set; }
    }
}
