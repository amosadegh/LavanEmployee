using Employee.Entities.Models.Enums;
using Employee.Entities.Models.Personel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.Entities.Models.TimeSheet
{
    [Table("WorkRecords")]
    public class WorkRecord
    {
        [Key]
        public long WorkRecordId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "کارمند")]
        public int EmpId { get; set; }

        [Required]
        [Display(Name = "از تاریخ")]
        public DateOnly FromDate { get; set; }

        [Required]
        [Display(Name = "تا تاریخ")]
        public DateOnly ToDate { get; set; }

        [Required]
        [Display(Name = "وضعیت کاری")]
        public WorkStatusType WorkStatus { get; set; }

        [MaxLength(500)]
        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "کارکرد ماهانه")]
        public long TimeSheetId { get; set; }

        // --- ثبت ---
        [Required]
        [MaxLength(450)]
        [Display(Name = "ثبت‌کننده")]
        public string CreatedByUserId { get; set; } = string.Empty;

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // --- محاسباتی ---
        [NotMapped]
        public int DayCount =>
            (ToDate.ToDateTime(TimeOnly.MinValue) - FromDate.ToDateTime(TimeOnly.MinValue)).Days + 1;

        /// <summary>
        /// تعداد روزهای این بازه که داخل ماه مشخص‌شده قرار می‌گیرد
        /// </summary>
        public int GetDaysInMonth(int year, int month) =>
            GetDaysInRange(new DateOnly(year, month, 1),
                           new DateOnly(year, month, DateTime.DaysInMonth(year, month)));

        /// <summary>
        /// تعداد روزهای این بازه که داخل بازه‌ی مشخص‌شده (مثلاً یک ماه شمسی) قرار می‌گیرد
        /// </summary>
        public int GetDaysInRange(DateOnly rangeStart, DateOnly rangeEnd)
        {
            var from = FromDate < rangeStart ? rangeStart : FromDate;
            var to = ToDate > rangeEnd ? rangeEnd : ToDate;

            return from <= to ? to.DayNumber - from.DayNumber + 1 : 0;
        }

        // --- Navigation Properties ---
        [ForeignKey(nameof(EmpId))]
        public TblEmployee? Employee { get; set; }

        [ForeignKey(nameof(TimeSheetId))]
        public MonthlyTimeSheet? TimeSheet { get; set; }

    }
}
