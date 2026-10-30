using Microsoft.EntityFrameworkCore;
using Employee.Entities.Models.Personel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.Entities.Models.TimeSheet
{
    public enum MonthlyStatus
    {
        Draft = 0,
        Submitted = 1,
        ManagerApproved = 2,
        FinalApproved = 3
    }

    /// <summary>کارکرد یک ماه شمسی از یک کارمند؛ وضعیت تأیید در این سطح نگهداری می‌شود</summary>
    [Table("MonthlyTimeSheets")]
    [Index(nameof(EmpId), nameof(Year), nameof(Month), IsUnique = true)]
    public class MonthlyTimeSheet
    {
        [Key]
        public long TimeSheetId { get; set; }

        public int EmpId { get; set; }
        public int Year { get; set; }   // شمسی
        public int Month { get; set; }  // 1..12

        public MonthlyStatus Status { get; set; } = MonthlyStatus.Draft;

        public DateTime? SubmittedAt { get; set; }

        [MaxLength(450)] public string? ManagerApprovedByUserId { get; set; }
        public DateTime? ManagerApprovedAt { get; set; }

        [MaxLength(450)] public string? FinalApprovedByUserId { get; set; }
        public DateTime? FinalApprovedAt { get; set; }

        [MaxLength(500)] public string? RejectReason { get; set; }

        [Timestamp] public byte[]? RowVersion { get; set; }

        [NotMapped] public bool IsEditable => Status == MonthlyStatus.Draft;

        [ForeignKey(nameof(EmpId))] public TblEmployee? Employee { get; set; }
        public ICollection<WorkRecord> Records { get; set; } = new List<WorkRecord>();
    }
}
