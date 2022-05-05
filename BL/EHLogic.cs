using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmploymentHelper.BL
{
    public class EHLogic
    {
        public EHLogic(IConfiguration configuration)
        {

        }

        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            await using var db = new AccountsContext();
            return db.Accounts.ToList();
        }

        public async Task<ActionResult<IEnumerable<Account>>> GetAccountsByName(string name)
        {
            await using var db = new AccountsContext();
            return db.Accounts
                .Where(a => a.Name == name)?/*.Contains(name, StringComparison.InvariantCultureIgnoreCase))*/
                .ToList();
        }

        public async Task<ActionResult<IEnumerable<Contact>>> GetContactsByLastName(string lastName)
        {
            await using var db = new AccountsContext();
            return db.Contacts
                //.Where(a => a.LastName.Contains(lastName, StringComparison.InvariantCultureIgnoreCase))?
                .ToList();
        }
    }
}
