using System;
namespace EmploymentHelper.Models
{
public class Contact
{
	public Guid Id { get; set; }
	public string LastName { get; set; }
	public string FirstName { get; set; }
	public string MiddleName { get; set; }
    public DateTime? BirthDate { get; set; }
	public bool Gender { get; set; }
	public Guid AccountId { get; set; }

}
}