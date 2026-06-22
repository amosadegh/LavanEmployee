using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.Models.Basic
{
    public class SecondOrganization
    {
        [Key]
        public int SecondOrganizationId { get; set; }
        [DisplayName("عنوان سازمان")]
        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        public int OrganizationId { get; set; }
        [DisplayName("عنوان سازمان فرعی")]
        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? SecondOrganizationTitle { get; set; }
        // Navigation Property (Many-to-One)
        public Organization? Organization { get; set; }
    }
}
