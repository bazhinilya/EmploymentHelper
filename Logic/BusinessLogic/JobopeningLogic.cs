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
            if (CommonLogic.IsLink(columnValue))
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
            Specialization specializationToCheck = CommonLogic.GetSpecialization(specializationColumnValue, db);
            VacancyPlace vacancyPlaceToCheck = CommonLogic.GetVacancyPlace(vacancyPlaceColumnValue, db);
            Account accountToCheck = CommonLogic.GetAccount(accountColumnValue, db);
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
            Jobopening jobopeningToChange = CommonLogic.GetJobopening(columnValue, db);
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
