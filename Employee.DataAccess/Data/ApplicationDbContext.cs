using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Employee.Entities.Models.Basic;
using Employee.Entities.Models.Personel;

namespace Employee.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
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
        public  DbSet<Bank> Banks { get; set; }
        public DbSet<JobLocation> JobLocations { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        #endregion
        #region Personel    
        public DbSet<TblEmployee> Employees { get; set; }
        #endregion
    }

}
