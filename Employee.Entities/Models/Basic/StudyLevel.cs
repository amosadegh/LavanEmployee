using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.Models.Basic
{
    public class StudyLevel
    {
        [Key]
        public int StudyLevelId { get; set; }
        [DisplayName("عنوان مقطع تحصیلی")]
        [Required(ErrorMessage = "عنوان مقطع را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? StudyLevelTitle { get; set; }
    }
}
