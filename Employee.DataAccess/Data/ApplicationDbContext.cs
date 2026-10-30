using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Employee.Entities.Models.Basic;
using Employee.Entities.Models.Personel;
using Employee.Entities.Models.User;
using Employee.Entities.Models;
using Employee.Entities.Models.TimeSheet;
using System.Reflection.Emit;

namespace Employee.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        #region BasicTables 
        public DbSet<Administration> Administrations { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<StudyLevel> StudyLevels { get; set; }
        public DbSet<StudyField> StudyFields { get; set; }
        public DbSet<EduOrientation> EduOrientations { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<SecondOrganization> SecondOrganizations { get; set; }
        public DbSet<Camp> Camps { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<JobLocation> JobLocations { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        #endregion
        #region Personel    
        public DbSet<TblEmployee> Employees { get; set; }
        #endregion
        #region TimeSheet   
        public DbSet<WorkRecord> WorkRecords { get; set; }
        public DbSet<WorkRecordCorrection> WorkRecordCorrections { get; set; }
        public DbSet<MonthlyTimeSheet> MonthlyTimeSheets { get; set; }
        #endregion
        #region User    
        // در ApplicationDbContext اضافه کن:
        public DbSet<SecondOrganizationUser> SecondOrganizationUsers { get; set; }

        #endregion
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TblEmployee>()
                .HasOne(e => e.ApplicationUser)
                .WithOne(u => u.Employee)
                .HasForeignKey<TblEmployee>(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<WorkRecord>(entity =>
            {
                // ایندکس برای کوئری‌های ماهانه
                entity.HasIndex(e => new { e.EmpId, e.FromDate, e.ToDate })
                      .HasDatabaseName("IX_WorkRecords_Emp_DateRange");

                // جلوگیری از multiple cascade paths
                entity.HasOne(e => e.Employee)
                      .WithMany()
                      .HasForeignKey(e => e.EmpId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //builder.Entity<WorkRecordCorrection>(entity =>
            //{
            //    entity.HasOne(e => e.WorkRecord)
            //          .WithMany(w => w.Corrections)
            //          .HasForeignKey(e => e.WorkRecordId)
            //          .OnDelete(DeleteBehavior.Cascade);
            //});

        }
    }
}