using System.ComponentModel.DataAnnotations;
using Employee.Entities.Models.Enums;

namespace Employee.Models.TimeSheet
{
    public class WorkRecordCreateVm
    {
        // شناسه رکورد (فقط در ویرایش)
        public long? WorkRecordId { get; set; }

        // کارمند هدف (فقط در area نماینده استفاده می‌شود)
        public int EmpId { get; set; }

        // سال و ماه انتخاب‌شده در Index (hidden) - بازه مجاز تاریخ
        [Range(1300, 1600)]
        public int Year { get; set; }

        [Range(1, 12)]
        public int Month { get; set; }

        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        [Display(Name = "از تاریخ")]
        public string FromDate { get; set; } = string.Empty;   // شمسی YYYY/MM/DD

        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        [Display(Name = "تا تاریخ")]
        public string ToDate { get; set; } = string.Empty;     // شمسی YYYY/MM/DD

        [Required(ErrorMessage = "لطفاً {0} را انتخاب نمایید")]
        [Display(Name = "وضعیت کارکرد")]
        public WorkStatusType? WorkStatus { get; set; }

        [MaxLength(500)]
        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
    }
}
