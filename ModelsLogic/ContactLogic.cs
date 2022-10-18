using EmploymentHelper.Models.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace EmploymentHelper.ModelsLogic
{
    public class ContactLogic
    {
        private readonly Type _contactsType = typeof(Contact);
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts(string columnValue = null)
        {
            await using var db = new VacancyContext();
            bool isId = Guid.TryParse(columnValue, out Guid id);
            var contacts = db.Contacts.Where(c => c.LastName.Contains(columnValue) || c.Id == id);
            if (isId && columnValue != null)
            {
                return db.Contacts.Where(c => c.Id == id).ToList();
            }
            else if (contacts != null && columnValue != null && contacts.Any())
            {
                return contacts.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.Contacts.ToList();
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
                foreach (var item in _contactsType.GetProperties())
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
    }
}
