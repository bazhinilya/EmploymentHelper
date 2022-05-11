using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace EmploymentHelper.Models
{
    [DataContract]
    public class ContactAccount
    {
        [DataMember(IsRequired = true)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [DataMember(IsRequired = true)]
        public /*List<Account>*/ int AccountId { get; set; }
        
        [DataMember(IsRequired = true)]
        public /*List<Contact>*/ int ContactId { get; set; }
    }
}
