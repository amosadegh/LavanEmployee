using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.Models.Basic
{
    public class City
    {
        public int Id { get; set; } // Corresponding to 'int IDENTITY(1,1) NOT NULL'

        [DisplayName("نام شهر")]
        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; } = null!; // Corresponding to 'nvarchar(100) NOT NULL'
        [DisplayName("نام استان")]
        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        public int ProvinceId { get; set; } // Foreign key to Province
                                            // Navigation Property: Each City belongs to one Province
                                            // Assumes a Province class exists and ProvinceId is its primary key
        
        public Province? Province { get; set; } 
    }
}
