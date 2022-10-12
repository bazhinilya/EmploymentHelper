using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EmploymentHelper.Interface
{
    public interface IAccounts
    {
        Task<ActionResult<IEnumerable<Accounts>>> GetAccounts(string columnValue = null);
        Task<ActionResult<Accounts>> AddAccount(string name, string inn = null);
        Task<ActionResult<Accounts>> EditAccount(Guid id, string columnName, string columnValue);
    }
}
