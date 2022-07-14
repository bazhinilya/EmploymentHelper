using System;
using System.Runtime.Serialization;

namespace EmploymentHelper.Models
{
    [DataContract]
    public class Accounts
    {
        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        
        [DataMember(IsRequired = false)]
        public string INN { get; set; }
    }
}
