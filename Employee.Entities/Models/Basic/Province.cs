using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.Models.Basic
{
    public class Province
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("نام استان")]
        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? Name { get; set; }
        // Navigation Property: A Province can have multiple Cities
        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
