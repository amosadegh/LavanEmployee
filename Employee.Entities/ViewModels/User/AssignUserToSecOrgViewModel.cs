using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Entities.ViewModels.User
{
    public class AssignUserToSecOrgViewModel
    {
        public int SecondOrganizationId { get; set; }
        public string SecondOrganizationTitle { get; set; } = string.Empty;

        // کاربران انتخاب‌شده با نقش Representative
        public List<string> RepresentativeUserIds { get; set; } = new();

        // کاربران انتخاب‌شده با نقش Manager
        public List<string> ManagerUserIds { get; set; } = new();

        
    }

}
