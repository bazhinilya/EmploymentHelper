using System.Runtime.Serialization;

namespace EmploymentHelper.Models
{
    [DataContract]
    public class Account
    {
        [DataMember(IsRequired = true)]
        public int Id { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        
        [DataMember(IsRequired = false)]
        public string FullName { get; set; }

        [DataMember(IsRequired = true)]
        public string INN { get; set; }
    }
}
