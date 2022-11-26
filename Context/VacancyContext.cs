using EmploymentHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace EmploymentHelper.Context
{
    public class VacancyContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AllSkill> AllSkills { get; set; }
        public DbSet<Communication> Communications { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Jobopening> Jobopenings { get; set; }
        public DbSet<JobopeningSkill> JobopeningsSkills { get; set; }
        public DbSet<JobopeningVacancyPlace> JobopeningsVacancyPlaces { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<SpecializationSkill> SpecializationsSkills { get; set; }
        public DbSet<VacancyCondition> VacancyConditions { get; set; }
        public DbSet<VacancyPlace> VacancyPlaces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MSSQLConn"));
            optionsBuilder.LogTo(Console.WriteLine);
        }
    }
}
