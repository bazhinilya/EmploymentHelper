using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
                .Where(a => a.Name.Contains(name, System.StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }
    }
}
