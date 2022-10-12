using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EmploymentHelper.Interface
{
    public interface IVacancyPlaces
    {
        Task<ActionResult<IEnumerable<VacancyPlaces>>> GetVacancyPlaces(string columnValue = null);
        Task<ActionResult<VacancyPlaces>> AddVacancyPlace(string name, string code);
        Task<ActionResult<VacancyPlaces>> EditVacancyPlace(Guid id, string columnName, string columnValue);
    }
}
