using EmploymentHelper.BLogic;
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
    public class JobopeningLogic
    {
        private readonly PropertyInfo[] _jobopeningsProperties;
        public JobopeningLogic() { _jobopeningsProperties = typeof(Account).GetProperties(); }
        public async Task<ActionResult<IEnumerable<Jobopening>>> GetJobopenings(string columnValue = null)
        {
            await using var db = new VacancyContext();
            if (columnValue == null)
            {
                return db.Jobopenings.ToList();
            }
            if (Guid.TryParse(columnValue, out Guid id))
            {
                return new List<Jobopening>
                {
                    db.Jobopenings.FirstOrDefault(j => j.Id == id) ?? throw new Exception("Invalid column value.")
                };
            }
            if (InnerLogic.IsLink(columnValue))
            {
                return new List<Jobopening>
                {
                    db.Jobopenings.FirstOrDefault(j => j.Link == columnValue) ?? throw new Exception("Invalid column value.")
                };
            }
            return db.Jobopenings.Where(j => j.Name.Contains(columnValue)).ToList() ?? throw new Exception("Invalid column value.");
        }
        public async Task<ActionResult<Jobopening>> AddJobopening(string specializationColumnValue, string vacancyPlaceColumnValue,
            string accountColumnValue, string name, string link)
        {
            await using var db = new VacancyContext();
            Specialization specializationToCheck = null;
            bool isId = Guid.TryParse(specializationColumnValue, out Guid id);
            if (isId)
            {
                specializationToCheck = db.Specializations.FirstOrDefault(s => s.Id == id);
            }
            if (specializationColumnValue.Length <= 4)
            {
                specializationToCheck = db.Specializations.FirstOrDefault(s => s.Code == specializationColumnValue || s.Name == specializationColumnValue);
            }
            if (specializationColumnValue.Length > 4)
            {
                specializationToCheck = db.Specializations.FirstOrDefault(s => s.Name == specializationColumnValue || s.Code == specializationColumnValue);
            }
            if (specializationToCheck == null) throw new Exception("Specialization does not exist.");
            VacancyPlace vacancyPlaceToCheck = null;
            bool isVacancyPlaceId = Guid.TryParse(vacancyPlaceColumnValue, out Guid vacancyPlaceId);
            if (isId)
            {
                vacancyPlaceToCheck = db.VacancyPlaces.FirstOrDefault(vp => vp.Id == vacancyPlaceId);
            }
            if (vacancyPlaceColumnValue.Length <= 4)
            {
                vacancyPlaceToCheck = db.VacancyPlaces.FirstOrDefault(vp => vp.Code == vacancyPlaceColumnValue || vp.Name == vacancyPlaceColumnValue);
            }
            if (vacancyPlaceColumnValue.Length > 4)
            {
                vacancyPlaceToCheck = db.VacancyPlaces.FirstOrDefault(vp => vp.Name == vacancyPlaceColumnValue || vp.Code == vacancyPlaceColumnValue);
            }
            if (vacancyPlaceToCheck == null) throw new Exception("Vacancy places does not exist.");
            Account accountToCheck = null;
            bool isAccountId = Guid.TryParse(accountColumnValue, out Guid accountId);
            if (isAccountId)
            {
                accountToCheck = db.Accounts.FirstOrDefault(a => a.Id == id);
            }
            bool isInn = InnerLogic.IsINN(accountColumnValue);
            if (isInn)
            {
                accountToCheck = db.Accounts.FirstOrDefault(a => a.INN == accountColumnValue);
            }
            if (!isInn && !isId)
            {
                accountToCheck = db.Accounts.FirstOrDefault(a => a.Name == accountColumnValue);
            }
            if (accountToCheck == null) throw new Exception("Account does not exist.");
            Jobopening jobopeningToCheck = db.Jobopenings.FirstOrDefault(j => j.Name == name || j.Link == link);
            if (jobopeningToCheck != null) throw new Exception("This jobopening already exsist.");
            Guid jobopeningId = Guid.NewGuid();
            Jobopening jobopeningToCreate = new()
            {
                Id = jobopeningId,
                SpecializationId = specializationToCheck.Id,
                AccountId = accountToCheck.Id,
                Name = name,
                Link = link
            };
            db.Jobopenings.Add(jobopeningToCreate);
            JobopeningVacancyPlace jobopeningVacancyPlaceToCreate = new()
            {
                Id = Guid.NewGuid(),
                JobopeningId = jobopeningId,
                VacancyPlaceId = vacancyPlaceToCheck.Id
            };
            db.JobopeningsVacancyPlaces.Add(jobopeningVacancyPlaceToCreate);
            await db.SaveChangesAsync();
            return jobopeningToCreate;
        }
        public async Task<ActionResult<Jobopening>> EditJobopening(string columnValue, string columnName, string newValue)
        {
            await using var db = new VacancyContext();
            Jobopening jobopeningToCheck = db.Jobopenings.FirstOrDefault(j => j.Name == newValue || j.Link == newValue);
            if (jobopeningToCheck != null) throw new Exception("This jobopening already exsist.");
            Jobopening jobopeningToChange = null;
            bool isId = Guid.TryParse(columnValue, out Guid id);
            if (isId)
            {
                jobopeningToChange = db.Jobopenings.FirstOrDefault(j => j.Id == id);
            }
            bool isLink = InnerLogic.IsLink(columnValue);
            if (isLink)
            {
                jobopeningToChange = db.Jobopenings.FirstOrDefault(j => j.Link == columnValue);
            }
            if (!isId && !isLink)
            {
                jobopeningToChange = db.Jobopenings.FirstOrDefault(j => j.Name == columnValue);
            }
            if (jobopeningToChange == null) throw new Exception("Jobopening does not exist.");
            bool isDirty = true;
            foreach (var item in _jobopeningsProperties)
            {
                if (item.Name == columnName)
                {
                    item.SetValue(jobopeningToChange, newValue);
                    isDirty = false;
                }
            }
            if (isDirty) throw new Exception("The column name is incorrect");
            await db.SaveChangesAsync();
            return jobopeningToChange;
        }
    }
}
