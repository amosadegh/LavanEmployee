using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.Models.Basic
{
    public class JobType
    {
        [Key]
        public int JobTypeId { get; set; }
        [DisplayName("کد نوع کار")]
        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        public int JobTypeCode { get; set; }
        [DisplayName("نوع کار")]
        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? JobTypeTitle { get; set; }
    }
}
