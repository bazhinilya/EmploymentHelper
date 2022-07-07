using System;
using System.Runtime.Serialization;

namespace EmploymentHelper.Models
{
    [DataContract]
    public class Account
    {
        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        
        [DataMember(IsRequired = true)]
        public string INN { get; set; }
    }
}
