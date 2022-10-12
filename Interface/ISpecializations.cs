using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EmploymentHelper.Interface
{
    public interface ISpecializations
    {
        Task<ActionResult<IEnumerable<Specializations>>> GetSpecializations(string columnValue = null);
        Task<ActionResult<Specializations>> AddSpecialization(string name, string code);
        Task<ActionResult<Specializations>> EditSpecialization(Guid id, string columnName, string columnValue);
    }
}
