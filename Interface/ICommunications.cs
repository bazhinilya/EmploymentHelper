using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EmploymentHelper.Interface
{
    public interface ICommunications
    {
        Task<ActionResult<IEnumerable<Communications>>> GetCommunications(string columnValue = null);
        Task<ActionResult<Communications>> AddCommunication(string accountColumnValue, string commType, string commValue,
            string contactColumnValue);
        Task<ActionResult<Communications>> EditCommunication(Guid id, string columnName, string columnValue);
    }
}
