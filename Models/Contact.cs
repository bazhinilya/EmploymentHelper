using System;
using System.Runtime.Serialization;

namespace EmploymentHelper.Models
{
    [DataContract]
    public class Contact
    {
        [DataMember(IsRequired = true)]
        public int Id { get; set; }

        [DataMember(IsRequired = true)]
        public string LastName { get; set; }

        [DataMember(IsRequired = true)]
        public string FirstName { get; set; }

        [DataMember(IsRequired = true)]
        public string MiddleName { get; set; }

        [DataMember(IsRequired = true)]
        public string FullName { get; set; }

        [DataMember(IsRequired = false)]
        public DateTime? BirthDate { get; set; }

        [DataMember(IsRequired = true)]
        public bool Gender { get; set; }
    }
}
