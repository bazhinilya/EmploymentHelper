using EmploymentHelper.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploymentHelper.BL
{
    public class AccountsContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactAccount> ContactsAccounts { get;  set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LAPTOP-SU0HN95T\\SQLEXPRESS;Database=Vacancies;Trusted_Connection=True;");
        }
    }
}
