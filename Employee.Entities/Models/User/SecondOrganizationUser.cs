// Employee.Entities.Models/SecondOrganizationUser.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Employee.Entities.Models.Basic;
using Employee.Entities.Models.User;

namespace Employee.Entities.Models
{
    public class SecondOrganizationUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int SecondOrganizationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = string.Empty; // Manager | Representative

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser? User { get; set; }

        [ForeignKey(nameof(SecondOrganizationId))]
        public virtual SecondOrganization? SecondOrganization { get; set; }
    }
}
