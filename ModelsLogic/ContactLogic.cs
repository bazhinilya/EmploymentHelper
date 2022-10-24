using EmploymentHelper.Models.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Reflection;

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
                                        || c.FullName.Contains(columnValue))
                              .ToList() ?? throw new Exception("Invalid column value.");
        }
        public async Task<ActionResult<Contact>> AddContact(string accountName, string lastName, string firstName,
            bool gender, DateTime? birthDate, string middleName = null)
        {
            await using var db = new VacancyContext();
            var accounts = db.Accounts.Where(a => a.Name == accountName);
            var contacts = db.Contacts.Where(c => c.AccountId == accounts.First().Id && c.FirstName == firstName
                                            && c.LastName == lastName && c.Gender == gender && c.BirthDate == birthDate);
            if (accounts.Count() == 0)
            {
                db.Accounts.Add(new Account { Id = Guid.NewGuid(), Name = accountName });
                await db.SaveChangesAsync();
            }
            if (accounts.Count() == 1 && contacts.Count() == 0)
            {
                db.Contacts.Add(new Contact
                {
                    Id = Guid.NewGuid(),
                    LastName = lastName,
                    FirstName = firstName,
                    MiddleName = middleName,
                    FullName = middleName == null ? $"{lastName} {firstName}" : $"{lastName} {firstName} {middleName}",
                    Gender = gender,
                    BirthDate = birthDate,
                    AccountId = accounts.First().Id
                });
            }
            else
            {
                throw new Exception("Uniqueness error. This contact already exists.");
            }
            await db.SaveChangesAsync();
            return contacts.First();
        }
        public async Task<ActionResult<Contact>> EditContact(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var contacts = db.Contacts.Where(c => c.Id == id);
            if (contacts.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _contactsProperties)
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(contacts.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.Contacts.Where(c => c.AccountId == contacts.First().AccountId
                                        && (c.FullName == contacts.First().FullName || c.BirthDate == contacts.First().BirthDate))
                               .Count() != 1)
                {
                    throw new Exception("Error, you are trying to specify already existing data.");
                }
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error, there are several entities.");
            }

            return contacts.First();
        }
    }//Add, Edit
}
