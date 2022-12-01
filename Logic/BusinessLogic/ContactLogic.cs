using EmploymentHelper.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmploymentHelper.Logic.BusinessLogic
{
    public class ContactLogic
    {
        private readonly PropertyInfo[] _contactsProperties;
        public ContactLogic() { _contactsProperties = typeof(Account).GetProperties(); }
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts(string columnValue = null)
        {
            await using var db = new VacancyContext();
            if (columnValue == null)
            {
                return db.Contacts.ToList();
            }
            if (Guid.TryParse(columnValue, out Guid id))
            {
                return new List<Contact>
                {
                    db.Contacts.FirstOrDefault(c => c.Id == id) ?? throw new Exception("Invalid column value.")
                };
            }
            if (DateTime.TryParse(columnValue, out DateTime birthDate))
            {
                return db.Contacts.Where(c => c.BirthDate == birthDate).ToList() ?? throw new Exception("Invalid column value.");
            }
            if (bool.TryParse(columnValue, out bool gender))
            {
                return db.Contacts.Where(c => c.Gender == gender).ToList() ?? throw new Exception("Invalid column value.");
            }
            return db.Contacts.Where(c => c.FullName.Contains(columnValue)).ToList() ?? throw new Exception("Invalid column value.");
        }
        public async Task<ActionResult<Contact>> AddContact(string accountColumnValue, string fullName,
            bool gender, DateTime? birthDate)
        {
            await using var db = new VacancyContext();
            Account accountToCheck = CommonLogic.GetAccount(accountColumnValue, db);
            Contact contactToCheck = db.Contacts.FirstOrDefault(c => c.FullName == fullName && c.AccountId == accountToCheck.Id);
            if (contactToCheck != null) throw new Exception("This contact already exist.");
            Contact contactToCreate = new()
            {
                Id = Guid.NewGuid(),
                FullName = fullName,
                Gender = gender,
                BirthDate = birthDate,
                AccountId = accountToCheck.Id
            };
            db.Contacts.Add(contactToCreate);
            await db.SaveChangesAsync();
            return contactToCreate;
        }
        public async Task<ActionResult<Contact>> EditContact(string columnValue, string columnName, string newValue)
        {
            await using var db = new VacancyContext();
            Contact contactToChange = CommonLogic.GetContact(columnValue, db);
            bool isDirty = true;
            foreach (var item in _contactsProperties)
            {
                if (item.Name == columnName)
                {
                    item.SetValue(contactToChange, newValue);
                    isDirty = false;
                }
            }
            if (isDirty) throw new Exception("The column name is incorrect");
            await db.SaveChangesAsync();
            return contactToChange;
        }


    }
}