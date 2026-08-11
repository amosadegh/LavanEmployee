using Employee.Entities.Models.Basic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Employee.Entities.Models.User;
namespace Employee.Entities.Models.Personel
{
    [Table("Employees")]
    public class TblEmployee
    {
        [Key]
        public int EmpId { get; set; }

        [DisplayName(" شماره پرسنلی")]
        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        [MaxLength(10, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string EmployeeNumber { get; set; } = string.Empty;

        [DisplayName(" نام")]
        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? Name { get; set; }

        [DisplayName("نام خانوادگی ")]
        [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? Family { get; set; }

        [DisplayName(" نام پدر")]
       // [Required(ErrorMessage = "لطفاً {0} را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? Father { get; set; }

        [DisplayName(" کد ملی ")]      
        [MaxLength(10, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? IdentityNumber { get; set; }

        [DisplayName(" شماره شناسنامه")]
        [MaxLength(10, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? NationalNumber { get; set; }
        [DisplayName(" تاریخ تولد")]
      
        public DateOnly? BirthDate { get; set; }
        [DisplayName(" استان محل تولد")]
        public int? BirthProvince { get; set; }
        [DisplayName(" شهر محل تولد")]
        public int? BirthCity { get; set; }
        [DisplayName(" استان محل صدور")]
        public int? EntryProvince { get; set; }
        [DisplayName(" شهر محل صدور")]
        public int? EntryCity { get; set; }
        [DisplayName(" وضعیت تاهل")]        
        [MaxLength(10, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? MaritalStatus { get; set; }
        [DisplayName(" استان محل سکونت")]
        public int? HomeProvince { get; set; }
        [DisplayName(" شهر محل سکونت")]
        public int? HomeCity { get; set; }
        [DisplayName(" کد پستی ")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]    
        public string? PostalCode { get; set; }
        [DisplayName(" آدرس ")]
        [MaxLength(200, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? Address { get; set; }

        [DisplayName(" تلفن منزل ")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? HomeTel { get; set; }

        [DisplayName(" موبایل ")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? Mobile { get; set; }

        [DisplayName(" وضعیت نظام وظیفه ")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? MilitaryService { get; set; }
        [DisplayName(" تاریخ پایان خدمت / معافیت ")]
       
        public DateOnly? MilitaryDate { get; set; }

        [DisplayName(" نوع معافیت ")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? MilitaryExemptionType { get; set; }

        [DisplayName("  نوع بیمه")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? InsuranceType { get; set; }

        [DisplayName(" شماره بیمه ")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? InsuranceNo { get; set; }

        [DisplayName(" شعبه بیمه ")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? InsuranceBranch { get; set; }

        [DisplayName(" مدت سوابق خارج از شرکت ")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? InsuranceOutOfCompany { get; set; }
        [DisplayName(" مقطع تحصیلی ")]
     
        public int? StudyLevelId { get; set; }
        [DisplayName(" رشته تحصیلی ")]
        public int? StudyFieldId { get; set; }
        [DisplayName(" گرایش رشته تحصیلی ")]
        public int? EduOrientationId { get; set; }
        [DisplayName(" نوع قرارداد ")]
        public int? ContractTypeId { get; set; }
        [DisplayName(" شغل ")]
        public int? JobId { get; set; }
        [DisplayName(" مدیریت ")]
        public int? AdministrationId { get; set; }
        [DisplayName(" سازمان ")]
        public int? OrganizationId { get; set; }
        [DisplayName(" واحد سازمانی ")]
        public int? SecondOrganizationId { get; set; }
        [DisplayName(" محل خدمت ")]
        public int? JobLocId { get; set; }
        [DisplayName(" کد شغل ")]
        public int? JobTypeId { get; set; }
        [DisplayName(" تاریخ شروع به کار ")]
        public DateOnly? WorkStartDate { get; set; }
        [DisplayName(" بانک حقوق ")]
        public int? PayBank { get; set; }

        [DisplayName(" شماره حساب حقوق")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? PayAccountNumber { get; set; }

        [DisplayName(" شماره شبا حقوق")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? PayShabaNumber { get; set; }
        [DisplayName(" بانک پشتیبان")]    
        public int? PayBackupBank { get; set; }

        [DisplayName(" شماره حساب پشتیبان")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? PayBackupNumber { get; set; }

        [DisplayName(" شماره شبا پشتیبان")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? PayPackupShabaNumber { get; set; }
        [DisplayName(" کمپ  ")]       
        public int? CampId { get; set; }
        [DisplayName(" اتاق ")]
        public int? RoomNo { get; set; }
        [DisplayName(" تلفن اتاق  ")]
        public int? RoomPhone { get; set; }
        [DisplayName(" تلفن محل کار")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? WorkPhone { get; set; }
        [DisplayName(" آلایندگی ")]
        public bool? Pollution { get; set; } 
        [DisplayName(" درصد آلایندگی ")]
        public double? PollutionPercent { get; set; }
        [DisplayName(" ردیف تعلق آلایندگی")]
        [MaxLength(50, ErrorMessage = "{0} نباید بیشتر از {1} کاراکتر باشد")]
        public string? PollutionRow { get; set; }
        [DisplayName(" حق ارتفاع ")]
        public bool? Height { get; set; } 
        [DisplayName(" متر حق ارتفاع  ")]
        public double? HeightMeter { get; set; }
        [DisplayName(" نوبتکار ")]
        public bool? Shift { get; set; }
        [DisplayName(" جوشکاری ")]
        public bool? Welder { get; set; } 
        [DisplayName(" شرایط سخت ")]
        public bool? HardShip { get; set; } 
        [DisplayName(" فوق العاده مخصوص  ")]
        public bool? SpecialExtraordinary { get; set; } 
        [DisplayName(" فوق العاده تخصصی  ")]
        public bool? ExpertiseExtraordinary { get; set; }
        [DisplayName(" مقدار مرخصی سالیانه ")]
        public int? AnnualLeaveAmont { get; set; }
        [DisplayName(" مانده مرخصی سالیانه ")]
        public int? RemainingAnnualLeave { get; set; }
        // ✅ اضافه شد
        [MaxLength(450)]
        public string? UserId { get; set; }

        // ── Navigation Properties ──────────────────────────────
        // ✅ اضافه شد
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        [ForeignKey(nameof(AdministrationId))]
        public virtual Administration? Administration { get; set; }

        [ForeignKey(nameof(PayBank))]
        public virtual Bank? PayBankNavigation { get; set; }

        [ForeignKey(nameof(PayBackupBank))]
        public virtual Bank? PayBackupBankNavigation { get; set; }

        [ForeignKey(nameof(CampId))]
        public virtual Camp? Camp { get; set; }

        [ForeignKey(nameof(BirthCity))]
        public virtual City? BirthCityNavigation { get; set; }

        [ForeignKey(nameof(HomeCity))]
        public virtual City? HomeCityNavigation { get; set; }

        [ForeignKey(nameof(EntryCity))]
        public virtual City? EntryCityNavigation { get; set; }

        [ForeignKey(nameof(ContractTypeId))]
        public virtual ContractType? ContractType { get; set; }

        [ForeignKey(nameof(EduOrientationId))]
        public virtual EduOrientation? EduOrientation { get; set; }

        [ForeignKey(nameof(JobLocId))]
        public virtual JobLocation? JobLocation { get; set; }

        [ForeignKey(nameof(JobId))]
        public virtual Job? Job { get; set; }

        [ForeignKey(nameof(JobTypeId))]
        public virtual JobType? JobType { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization? Organization { get; set; }

        [ForeignKey(nameof(SecondOrganizationId))]
        public virtual SecondOrganization? SecondOrganization { get; set; }

        [ForeignKey(nameof(StudyLevelId))]
        public virtual StudyLevel? StudyLevel { get; set; }

        [ForeignKey(nameof(StudyFieldId))]
        public virtual StudyField? StudyField { get; set; }

        [ForeignKey(nameof(BirthProvince))]
        public virtual Province? BirthProvinceNavigation { get; set; }

        [ForeignKey(nameof(HomeProvince))]
        public virtual Province? HomeProvinceNavigation { get; set; }

        [ForeignKey(nameof(EntryProvince))]
        public virtual Province? EntryProvinceNavigation { get; set; }
    }
}
