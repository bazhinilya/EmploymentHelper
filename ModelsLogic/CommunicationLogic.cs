using EmploymentHelper.Models;
using EmploymentHelper.Models.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmploymentHelper.ModelsLogic
{
    public class CommunicationLogic
    {
        private readonly Type _communicationsType = typeof(Communication);
        public async Task<ActionResult<IEnumerable<Communication>>> GetCommunications(string columnValue = null)
        {
            await using var db = new VacancyContext();
            bool isId = Guid.TryParse(columnValue, out Guid id);
            var communications = db.Communications.Where(c => c.CommType == columnValue || c.CommValue == columnValue || c.Id == id
                                                            || c.AccountId == id || c.ContactId == id);
            if (isId && columnValue != null)
            {
                return db.Communications.Where(c => c.Id == id || c.AccountId == id || c.ContactId == id).ToList();
            }
            else if (communications != null && columnValue != null && communications.Any())
            {
                return communications.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.Communications.ToList();
        }
        public async Task<ActionResult<Communication>> AddCommunication(string accountColumnValue, string commType, string commValue,
            string contactColumnValue)
        {
            await using var db = new VacancyContext();
            var accounts = db.Accounts.Where(a => a.Name == accountColumnValue || a.INN == accountColumnValue);
            var contacts = db.Contacts.Where(c => c.FullName == contactColumnValue);
            var communications = db.Communications.Where(c => c.CommType == commType && c.ContactId == contacts.First().Id);
            if (accounts.Count() == 1 && contacts.Count() == 1 && communications.Count() == 0)
            {
                db.Communications.Add(new Communication
                {
                    Id = Guid.NewGuid(),
                    CommType = commType,
                    CommValue = commValue,
                    AccountId = accounts.First().Id,
                    ContactId = contacts.First().Id
                });
            }
            else
            {
                throw new Exception("Uniqueness error. This communication already exists.");
            }
            await db.SaveChangesAsync();
            return communications.First();
        }
        public async Task<ActionResult<Communication>> EditCommunication(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var communications = db.Communications.Where(c => c.Id == id);
            if (communications.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _communicationsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(communications.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.Communications.Where(c => c.CommType == communications.First().CommType
                                            && c.ContactId == communications.First().ContactId).Count() != 1)
                {
                    throw new Exception("Error, you are trying to specify already existing data.");
                }
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error, there are several Communications.");
            }
            return communications.First();
        }
    }
}
