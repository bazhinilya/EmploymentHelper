using System;
namespace EmploymentHelper.Models
{
public class AllAccounts
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null;
	public string INN { get; set; } = null;
	public Guid? ContactId { get; set; }
	public string FullName { get; set; } = null;
	public bool? Gender { get; set; }
	public DateTime? BirthDate { get; set; } 
	public Guid? CommunicationId { get; set; }
	public string CommType { get; set; } = null;
	public string CommValue { get; set; } = null;

}
}