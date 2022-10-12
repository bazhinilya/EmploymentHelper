using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EmploymentHelper.Interface
{
    public interface ISkills
    {
        Task<ActionResult<IEnumerable<Skills>>> GetSkills(string columnValue = null);
        Task<ActionResult<Skills>> AddSkill(string jobopeningColumnValue, string name);
        Task<ActionResult<Skills>> EditSkill(Guid id, string columnName, string columnValue);
    }
}
