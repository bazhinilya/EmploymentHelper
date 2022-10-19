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
        private readonly Type _communicationsType;
        public CommunicationLogic() { _communicationsType = typeof(Communication); }
        public async Task<ActionResult<IEnumerable<Communication>>> GetCommunications(string columnValue = null)
        {
            await using var db = new VacancyContext();
            if (columnValue == null)
            {
                return db.Communications.ToList();
            }
            if (Guid.TryParse(columnValue, out Guid id))
            {
                return new List<Communication>
                {
                    db.Communications.FirstOrDefault(c => c.Id == id) ?? throw new Exception("Invalid column value.")
                };
            }
            return db.Communications.Where(c => c.CommType.Contains(columnValue) || c.CommValue.Contains(columnValue)).ToList();
        }
        public async Task<ActionResult<Communication>> AddCommunication(string contactColumnValue, string commType, string commValue)
        {
            await using var db = new VacancyContext();
            Contact contactToFind = null;
            bool isId = Guid.TryParse(contactColumnValue, out Guid id);
            bool isBirthDate = DateTime.TryParse(contactColumnValue, out DateTime birthDate);
            if (isBirthDate)
            {
                contactToFind = db.Contacts.FirstOrDefault(c => c.BirthDate == birthDate) ?? throw new Exception("Contact does not exist.");
            }
            if (isId)
            {
                contactToFind = db.Contacts.FirstOrDefault(c => c.Id == id) ?? throw new Exception("Contact does not exist.");
            }
            if (!isId)
            {
                contactToFind = db.Contacts
                    .FirstOrDefault(c => c.FullName == contactColumnValue || c.LastName == contactColumnValue) ?? throw new Exception("Contact does not exist.");
            }
            if (db.Communications.Where(c => c.CommType == commType && c.ContactId == contactToFind.Id).Any()) throw new Exception("This data already exsist.");
            Communication communicationToCreate = new()
            {
                Id = Guid.NewGuid(),
                CommType = commType,
                CommValue = commValue,
                AccountId = contactToFind.AccountId,
                ContactId = contactToFind.Id
            };
            db.Communications.Add(communicationToCreate);
            await db.SaveChangesAsync();
            return communicationToCreate;
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
