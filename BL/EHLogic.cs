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
        public EHLogic(IConfiguration configuration) { }

        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            await using var db = new AccountsContext();
            return db.Accounts.ToList();
        }

        public async Task<ActionResult<IEnumerable<Account>>> GetAccountByName(string name)
        {
            await using var db = new AccountsContext();
            return db.Accounts
                .Where(a => a.Name.Contains(name))?
                .ToList();
        }

        public async Task<ActionResult<IEnumerable<Contact>>> GetContactByLastName(string lastName)
        {
            await using var db = new AccountsContext();
            return db.Contacts
                .Where(с => с.LastName.Contains(lastName))?
                .ToList();
        }

        public async Task<ActionResult<bool>> UpdateContactBirthDate(int id, DateTime birthDate)
        {
            await using var db = new AccountsContext();

            var contactToUpdate = db.Contacts.Where(c => c.Id == id).FirstOrDefault();

            if (contactToUpdate != null)
            {
                contactToUpdate.BirthDate = birthDate;
                await db.SaveChangesAsync();
                return true;
            }
            return false; 
        }

        public async Task<ActionResult<bool>> AddContactAccount(int contactId, int accountId)
        {
            await using var db = new AccountsContext();

            //var contact = db.Contacts.Where(c => c.Id == contactId).FirstOrDefault();
            //var account = db.Accounts.Where(a => a.Id == accountId).FirstOrDefault();

            var contactAccount = db.ContactsAccounts
                    .Where(ca => ca.ContactId == contactId && ca.AccountId == accountId)
                    .FirstOrDefault();

            if (contactAccount == null)
            {
                db.ContactsAccounts.Add(new ContactAccount { ContactId = contactId, AccountId = accountId });
                await db.SaveChangesAsync();


            }
            return true;
        }

    }
}
