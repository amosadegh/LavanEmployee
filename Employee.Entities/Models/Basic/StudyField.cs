using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.Models.Basic
{
    public class StudyField
    {
        [Key]   
        public int StudyFieldId { get; set; }
        [DisplayName("عنوان رشته تحصیلی")]
        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? StudyFieldTitle { get; set; }

        // Navigation Property (One-to-Many)
        public ICollection<EduOrientation> EduOrientations { get; set; } = new List<EduOrientation>();
    }
}
