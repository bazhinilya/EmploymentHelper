using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmploymentHelper.Interface
{
    public interface IEmploymentHelper
    {
        Task<ActionResult<IEnumerable<AllSkills>>> GetAllSkillsView(string columnValue = null);
    }
}
