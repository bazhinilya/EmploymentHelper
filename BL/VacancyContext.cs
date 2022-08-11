using EmploymentHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace EmploymentHelper.BL
{
    public class VacancyContext : DbContext
    {
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<AllAccounts> AllAccounts { get; set; }
        public DbSet<AllSkills> AllSkills { get; set; }
        public DbSet<Communications> Communications { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Jobopenings> Jobopenings { get; set; }
        public DbSet<JobopeningsSkills> JobopeningsSkills { get; set; }
        public DbSet<JobopeningsVacancyPlaces> JobopeningsVacancyPlaces { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<SkillsJobopening> SkillsJobopening { get; set; } 
        public DbSet<Specializations> Specializations { get; set; }
        public DbSet<SpecializationsSkills> SpecializationsSkills { get; set; }
        public DbSet<VacancyConditions> VacancyConditions { get; set; }
        public DbSet<VacancyPlaces> VacancyPlaces { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MSSQLConn"));
        }
    }
}
