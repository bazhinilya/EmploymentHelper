using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EmploymentHelper.Interface
{
    public interface IJobopenings
    {
        Task<ActionResult<IEnumerable<Jobopenings>>> GetJobopenings(string columnValue = null);
        Task<ActionResult<Jobopenings>> AddJobopening(string specializationColumnValue, string vacancyPlaceColumnValue, string name,
            string link, string accountName);
        Task<ActionResult<Jobopenings>> EditJobopening(Guid id, string columnName, string columnValue);
    }
}
