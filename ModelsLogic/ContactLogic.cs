using EmploymentHelper.Models.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Reflection;
using EmploymentHelper.BLogic;

namespace EmploymentHelper.ModelsLogic
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
            return db.Contacts.Where(c => c.LastName.Contains(columnValue)
                                        || c.FirstName.Contains(columnValue)
                                        || c.MiddleName.Contains(columnValue)
                                        /*|| c.FullName.Contains(columnValue)*/)
                              .ToList() ?? throw new Exception("Invalid column value.");
        }
        public async Task<ActionResult<Contact>> AddContact(string accountColumnValue, string lastName, string firstName,
            bool gender, DateTime? birthDate, string middleName = null)
        {
            await using var db = new VacancyContext();
            Account accountToCheck = null;
            bool isId = Guid.TryParse(accountColumnValue, out Guid id);
            if (isId)
            {
                accountToCheck = db.Accounts.FirstOrDefault(a => a.Id == id);
            }
            bool isInn = InnerLogic.IsINN(accountColumnValue);
            if (isInn)
            {
                accountToCheck = db.Accounts.FirstOrDefault(a => a.INN == accountColumnValue);
            }
            if (!isId && !isInn)
            {
                accountToCheck = db.Accounts.FirstOrDefault(a => a.Name == accountColumnValue);
            }
            if (accountToCheck == null) throw new Exception("Account does not exist.");
            Contact contactToCheck = db.Contacts.FirstOrDefault(c => c.LastName == lastName && c.AccountId == accountToCheck.Id);
            if (contactToCheck != null) throw new Exception("This contact already exist.");
            Contact contactToCreate = new()
            {
                Id = Guid.NewGuid(),
                LastName = lastName,
                FirstName = firstName,
                MiddleName = middleName,
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
            bool isId = Guid.TryParse(columnValue, out Guid id);
            if (isId) throw new Exception("Invalid column value.");
            Contact contactToChange = db.Contacts.FirstOrDefault(c => c.Id == id);
            if (contactToChange == null) throw new Exception("Contact does not exist.");
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