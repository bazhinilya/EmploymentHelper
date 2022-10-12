using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EmploymentHelper.Interface
{
    public interface IContacts
    {
        Task<ActionResult<IEnumerable<Contacts>>> GetContacts(string columnValue = null);
        Task<ActionResult<Contacts>> AddContact(string accountName, string lastName, string firstName, bool gender, DateTime? birthDate,
            string middleName = null);
        Task<ActionResult<Contacts>> EditContact(Guid id, string columnName, string columnValue);
    }
}
