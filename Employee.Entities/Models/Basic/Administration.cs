using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.Models.Basic
{
    public class Administration
    {
        [Key]
        public int AdministrationId { get; set; }
        [DisplayName("مدیریت")]
        [Required(ErrorMessage ="عنوان مدیریت را وارد نمایید")]
        [MaxLength(50,ErrorMessage ="{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? AdministrationTitle { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
