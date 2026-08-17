using Employee.Entities.Models.Personel;
using Microsoft.AspNetCore.Identity;

namespace Employee.Entities.Models.User
{
    public class ApplicationUser:IdentityUser
    {
        public string? FullName { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual TblEmployee? Employee { get; set; }
    }
}
