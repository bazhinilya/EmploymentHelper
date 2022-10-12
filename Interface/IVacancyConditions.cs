using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EmploymentHelper.Interface
{
    public interface IVacancyConditions
    {
        Task<ActionResult<IEnumerable<VacancyConditions>>> GetVacancyConditions(string columnValue = null);
        Task<ActionResult<VacancyConditions>> AddVacancyCondition(string jobopeningColumnValue, string conditionType,
            string conditionValue);
        Task<ActionResult<VacancyConditions>> EditVacancyCondition(Guid id, string columnName, string columnValue);
    }
}
