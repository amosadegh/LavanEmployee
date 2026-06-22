using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.Models.Basic
{
    public class EduOrientation
    {
        [Key]
        public int EduOrientationId { get; set; }
        [DisplayName("عنوان رشته تحصیلی")]
        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        public int StudyFieldId { get; set; }
        [DisplayName("عنوان گرایش")]
        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? EduOrientationTitle { get; set; }

        // Navigation Property (Many-to-One)
        public StudyField? StudyField { get; set; }
    }
}
