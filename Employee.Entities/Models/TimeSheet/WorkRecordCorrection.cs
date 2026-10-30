using Employee.Entities.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.Entities.Models.TimeSheet
{
    [Table("WorkRecordCorrections")]
    public class WorkRecordCorrection
    {
        [Key]
        public long CorrectionId { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        [Display(Name = "رکورد کاری")]
        public long WorkRecordId { get; set; }

        /// <summary>نام فیلد تغییریافته — مثلاً nameof(WorkRecord.WorkStatus)</summary>
        [Required]
        [MaxLength(100)]
        [Display(Name = "نام فیلد")]
        public string FieldName { get; set; } = string.Empty;

        [MaxLength(500)]
        [Display(Name = "مقدار قبلی")]
        public string? OldValue { get; set; }

        [MaxLength(500)]
        [Display(Name = "مقدار جدید")]
        public string? NewValue { get; set; }

        [MaxLength(500)]
        [Display(Name = "دلیل اصلاح")]
        public string? Reason { get; set; }

        [Required]
        [MaxLength(450)]
        [Display(Name = "ثبت‌کننده")]
        public string CreatedByUserId { get; set; } = string.Empty;

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "اعمال شده")]
        public bool IsApplied { get; set; } = false;

        [Display(Name = "تاریخ اعمال")]
        public DateTime? AppliedAt { get; set; }

        // --- Navigation Properties ---
        [ForeignKey(nameof(WorkRecordId))]
        public WorkRecord? WorkRecord { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public ApplicationUser? CreatedBy { get; set; }
    }
}
