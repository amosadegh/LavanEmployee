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
        [StringLength(10)]
        public string EmployeeNumber { get; set; } = string.Empty;

        [StringLength(50)]
        public string? IdentityNumber { get; set; }

        [StringLength(10)]
        public string? NationalNumber { get; set; }

        [StringLength(50)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? Family { get; set; }

        [StringLength(50)]
        public string? Father { get; set; }

        public DateTime? BirthDate { get; set; }

        public int? BirthProvice { get; set; }

        public int? BirthCity { get; set; }

        [StringLength(10)]
        public string? MaritalStatus { get; set; }

        public int? HomeProvince { get; set; }

        public int? HomeCity { get; set; }

        [StringLength(50)]
        public string? PostalCode { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? HomeTel { get; set; }

        [StringLength(50)]
        public string? Mobile { get; set; }

        [StringLength(50)]
        public string? MilitaryService { get; set; }

        public DateTime? MilitaryDate { get; set; }

        [StringLength(50)]
        public string? MilitaryExemptionType { get; set; }

        [StringLength(50)]
        public string? InsuranceType { get; set; }

        [StringLength(50)]
        public string? InsyranceNo { get; set; }

        [StringLength(50)]
        public string? InsuranceBranch { get; set; }

        public int? StudyLevelId { get; set; }

        public int? StudyFieldId { get; set; }

        public int? EduOrientationId { get; set; }

        public int? ContractTypeId { get; set; }

        public int? JobId { get; set; }

        public int? AdministrationId { get; set; }

        public int? OrganizationId { get; set; }

        public int? SecondOrganizationId { get; set; }

        public DateTime? WorkStartDate { get; set; }

        [StringLength(50)]
        public string? InsuranceOutOfCompany { get; set; }

        public int? EntryProviance { get; set; }

        public int? EntryCity { get; set; }

        public int? PayBank { get; set; }

        [StringLength(50)]
        public string? PayAccountNumber { get; set; }

        [StringLength(50)]
        public string? PayShabaNumber { get; set; }

        public int? PayBackupBank { get; set; }

        [StringLength(50)]
        public string? PayBackupNumber { get; set; }

        [StringLength(50)]
        public string? PayPackupShabaNumber { get; set; }

        public int? CampId { get; set; }

        public int? RoomNo { get; set; }

        public int? RoomPhone { get; set; }

        [StringLength(20)]
        public string? WorkPhone { get; set; }

        public bool? Pollution { get; set; }

        public double? PollutionPercent { get; set; }

        [StringLength(50)]
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

        // Navigation Properties
        [ForeignKey("AdministrationId")]
        public virtual Administration? Administration { get; set; }

        [ForeignKey("PayBank")]
        public virtual Bank? PayBankNavigation { get; set; }

        [ForeignKey("PayBackupBank")]
        public virtual Bank? PayBackupBankNavigation { get; set; }

        [ForeignKey("CampId")]
        public virtual Camp? Camp { get; set; }

        [ForeignKey("BirthCity")]
        public virtual City? BirthCityNavigation { get; set; }

        [ForeignKey("HomeCity")]
        public virtual City? HomeCityNavigation { get; set; }

        [ForeignKey("EntryCity")]
        public virtual City? EntryCityNavigation { get; set; }

        [ForeignKey("ContractTypeId")]
        public virtual ContractType? ContractType { get; set; }

        [ForeignKey("EduOrientationId")]
        public virtual EduOrientation? EduOrientation { get; set; }

        [ForeignKey("JobId")]
        public virtual Job? Job { get; set; }

        [ForeignKey("JobLocId")]
        public virtual JobLocation? JobLocation { get; set; }

        [ForeignKey("JobTypeId")]
        public virtual JobType? JobType { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization? Organization { get; set; }
    }
}
