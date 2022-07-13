using System;

namespace EmploymentHelper.Models
{
public class VacancyConditions
{
	public Guid Id { get; set; }
	public string ConditionValue { get; set; }
	public string ConditionType { get; set; }
	public Guid jobopeningId { get; set; }

}
}