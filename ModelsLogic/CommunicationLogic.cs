using EmploymentHelper.Models;
using EmploymentHelper.Models.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmploymentHelper.ModelsLogic
{
    public class CommunicationLogic
    {
        private readonly PropertyInfo[] _communicationsProperties;
        public CommunicationLogic() { _communicationsProperties = typeof(Account).GetProperties(); }
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
            return db.Communications.Where(c => c.CommType.Contains(columnValue) || c.CommValue.Contains(columnValue))
                                    .ToList() ?? throw new Exception("Invalid column value.");
        }
        public async Task<ActionResult<Communication>> AddCommunication(string contactColumnValue, string commType, string commValue)
        {
            await using var db = new VacancyContext();
            Contact contactToCheck = null;
            bool isBirthDate = DateTime.TryParse(contactColumnValue, out DateTime birthDate);
            if (isBirthDate)
            {
                contactToCheck = db.Contacts.FirstOrDefault(c => c.BirthDate == birthDate);
            }
            bool isId = Guid.TryParse(contactColumnValue, out Guid id);
            if (isId)
            {
                contactToCheck = db.Contacts.FirstOrDefault(c => c.Id == id);
            }
            if (!isId && !isBirthDate)
            {
                contactToCheck = db.Contacts.FirstOrDefault(c => c.LastName == contactColumnValue);
            }
            if (contactToCheck == null) throw new Exception("Contact does not exist.");
            Communication communicationToCheck = db.Communications.FirstOrDefault(c => c.CommType == commType && c.ContactId == contactToCheck.Id);
            if (communicationToCheck != null) throw new Exception("This communication already exist.");
            Communication communicationToCreate = new()
            {
                Id = Guid.NewGuid(),
                CommType = commType,
                CommValue = commValue,
                AccountId = contactToCheck.AccountId,
                ContactId = contactToCheck.Id
            };
            db.Communications.Add(communicationToCreate);
            await db.SaveChangesAsync();
            return communicationToCreate;
        }
        public async Task<ActionResult<Communication>> EditCommunication(string columnValue, string columnName, string newValue)
        {
            await using var db = new VacancyContext();
            bool isId = Guid.TryParse(columnValue, out Guid id);
            if (isId) throw new Exception("Invalid column value.");
            Communication communicationToChange = db.Communications.FirstOrDefault(c => c.Id == id);
            if (communicationToChange == null) throw new Exception("Communication does not exist.");
            bool isDirty = true;
            foreach (var item in _communicationsProperties)
            {
                if (item.Name == columnName)
                {
                    item.SetValue(communicationToChange, newValue);
                    isDirty = false;
                }
            }
            if (isDirty) throw new Exception("The column name is incorrect");
            await db.SaveChangesAsync();
            return communicationToChange;
        }
    }
}
