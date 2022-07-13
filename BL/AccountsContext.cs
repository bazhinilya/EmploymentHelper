using EmploymentHelper.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploymentHelper.BL
{
    public class AccountsContext : DbContext
    {
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Communications> Communications { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Jobopenings> Jobopenings { get; set; }
        public DbSet<JobopeningsSkills> JobopeningsSkills { get; set; }
        public DbSet<JobopeningsVacancyPlaces> JobopeningsVacancyPlaces { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<Specializations> Specializations { get; set; }
        public DbSet<SpecializationsSkills> SpecializationsSkills { get; set; }
        public DbSet<VacancyConditions> VacancyConditions { get; set; }
        public DbSet<VacancyPlaces> VacancyPlaces { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LAPTOP-SU0HN95T\\SQLEXPRESS;Database=Vacancies;Trusted_Connection=True;");
        }
    }
}
