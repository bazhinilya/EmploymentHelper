using System;
namespace EmploymentHelper.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool Gender { get; set; }
        public Guid AccountId { get; set; }
    }
}