using Employee.Entities.Models.Personel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.Models.User
{
    public class ApplicationUser:IdentityUser
    {
        public virtual TblEmployee? Employee { get; set; }
    }
}
