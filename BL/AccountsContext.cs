using EmploymentHelper.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploymentHelper.BL
{
    public class AccountsContext : DbContext
    {
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<Jobopenings> Jobopenings { get; set; }
        public DbSet<JobopeningsSkills> JobopeningsSkills { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LAPTOP-SU0HN95T\\SQLEXPRESS;Database=Vacancies;Trusted_Connection=True;");
        }
    }
}
