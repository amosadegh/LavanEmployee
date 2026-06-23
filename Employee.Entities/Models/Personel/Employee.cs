using Employee.Entities.Models.Basic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.Entities.Models.Personel
{
    [Table("Employees")]
    public class TblEmployee
    {
        [Key]
        public int EmpId { get; set; }

        [Required]
        [MaxLength(10)]
        public string EmployeeNumber { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? IdentityNumber { get; set; }

        [MaxLength(10)]
        public string? NationalNumber { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(50)]
        public string? Family { get; set; }

        [MaxLength(50)]
        public string? Father { get; set; }

        public DateOnly? BirthDate { get; set; }

        public int? BirthProvice { get; set; }

        public int? BirthCity { get; set; }

        [MaxLength(10)]
        public string? MaritalStatus { get; set; }

        public int? HomeProvince { get; set; }

        public int? HomeCity { get; set; }

        [MaxLength(50)]
        public string? PostalCode { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? HomeTel { get; set; }

        [MaxLength(50)]
        public string? Mobile { get; set; }

        [MaxLength(50)]
        public string? MilitaryService { get; set; }

        public DateOnly? MilitaryDate { get; set; }

        [MaxLength(50)]
        public string? MilitaryExemptionType { get; set; }

        [MaxLength(50)]
        public string? InsuranceType { get; set; }

        [MaxLength(50)]
        public string? InsyranceNo { get; set; }

        [MaxLength(50)]
        public string? InsuranceBranch { get; set; }

        public int? StudyLevelId { get; set; }

        public int? StudyFieldId { get; set; }

        public int? EduOrientationId { get; set; }

        public int? ContractTypeId { get; set; }

        public int? JobId { get; set; }

        public int? AdministrationId { get; set; }

        public int? OrganizationId { get; set; }

        public int? SecondOrganizationId { get; set; }

        public DateOnly? WorkStartDate { get; set; }

        [MaxLength(50)]
        public string? InsuranceOutOfCompany { get; set; }

        public int? EntryProviance { get; set; }

        public int? EntryCity { get; set; }

        public int? PayBank { get; set; }

        [MaxLength(50)]
        public string? PayAccountNumber { get; set; }

        [MaxLength(50)]
        public string? PayShabaNumber { get; set; }

        public int? PayBackupBank { get; set; }

        [MaxLength(50)]
        public string? PayBackupNumber { get; set; }

        [MaxLength(50)]
        public string? PayPackupShabaNumber { get; set; }

        public int? CampId { get; set; }

        public int? RoomNo { get; set; }

        public int? RoomPhone { get; set; }

        [MaxLength(20)]
        public string? WorkPhone { get; set; }

        public bool? Pollution { get; set; }

        public double? PollutionPercent { get; set; }

        [MaxLength(50)]
        public string? PollutionRow { get; set; }

        public bool? Height { get; set; }

        public double? HeightMeter { get; set; }

        public bool? Shift { get; set; }

        public bool? Welder { get; set; }

        public bool? HardShip { get; set; }

        public bool? SpecialExtraordinary { get; set; }

        public bool? ExpertiseExtraordinary { get; set; }

        public int? AnnualLeaveAmont { get; set; }

        public int? RemainingAnnualLeave { get; set; }

        public int? JobLocId { get; set; }

        public int? JobTypeId { get; set; }

        // ── Navigation Properties ──────────────────────────────

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
        public virtual Organization? SecondOrganization { get; set; }

        [ForeignKey(nameof(StudyLevelId))]
        public virtual StudyLevel? StudyLevel { get; set; }

        [ForeignKey(nameof(StudyFieldId))]
        public virtual StudyField? StudyField { get; set; }

        [ForeignKey(nameof(BirthProvice))]
        public virtual Province? BirthProvince { get; set; }

        [ForeignKey(nameof(HomeProvince))]
        public virtual Province? HomeProvinceNavigation { get; set; }

        [ForeignKey(nameof(EntryProviance))]
        public virtual Province? EntryProvinceNavigation { get; set; }
    }
}
