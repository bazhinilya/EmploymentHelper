using System;
namespace EmploymentHelper.Models
{
public class Communications
{
	public Guid Id { get; set; }
	public string CommType { get; set; }
	public string CommValue { get; set; }
	public Guid AccountId { get; set; }
	public Guid ContactId { get; set; }

}
}