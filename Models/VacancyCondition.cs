using System;
namespace EmploymentHelper.Models
{
public class VacancyCondition
{
	public Guid Id { get; set; }
	public string ConditionValue { get; set; }
	public string ConditionType { get; set; }
	public Guid JobopeningId { get; set; }

}
}