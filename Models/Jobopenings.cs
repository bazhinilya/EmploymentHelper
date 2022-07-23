using System;
namespace EmploymentHelper.Models
{
public class Jobopenings
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public Guid AccountId { get; set; }
	public string Link { get; set; }
	public Guid SpecializationId { get; set; }

}
}