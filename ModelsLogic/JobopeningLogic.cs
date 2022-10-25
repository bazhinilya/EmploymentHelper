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
            if (columnValue.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                return new List<Jobopening> 
                { 
                    db.Jobopenings.FirstOrDefault(j => j.Link == columnValue) ?? throw new Exception("Invalid column value.")
                };
            }
            return db.Jobopenings.Where(j => j.Name.Contains(columnValue)).ToList() ?? throw new Exception("Invalid column value.");
        }
        public async Task<ActionResult<Jobopening>> AddJobopening(string specializationColumnValue, string vacancyPlaceColumnValue,
            string name, string link, string accountName)
        {
            await using var db = new VacancyContext();
            var specializations = db.Specializations.Where(s => s.Name == specializationColumnValue
                                                            || s.Code == specializationColumnValue);
            var vacancyPlaces = db.VacancyPlaces.Where(s => s.Name == vacancyPlaceColumnValue || s.Code == vacancyPlaceColumnValue);
            var jobopenings = db.Jobopenings.Where(j => j.Name == name && j.Link == link);
            var accounts = db.Accounts.Where(a => a.Name == accountName);
            if (accounts == null || accounts.Count() == 0)
            {
                db.Accounts.Add(new Account { Id = Guid.NewGuid(), Name = accountName });
                await db.SaveChangesAsync();
            }
            if (accounts.Count() == 1 && vacancyPlaces.Count() == 1 && jobopenings.Count() == 0)
            {
                Guid jobopeningId = Guid.NewGuid();
                db.Jobopenings.Add(new Jobopening
                {
                    Id = jobopeningId,
                    Name = name,
                    Link = link,
                    SpecializationId = specializations.First().Id,
                    AccountId = accounts.First().Id
                });

                var jobopeningsVacancyPlaces = db.JobopeningsVacancyPlaces.Where(jvp => jvp.VacancyPlaceId == vacancyPlaces.First().Id
                                                                                && jvp.JobopeningId == jobopeningId);
                if (jobopeningsVacancyPlaces.Count() == 0)
                {
                    db.JobopeningsVacancyPlaces.Add(new JobopeningVacancyPlace
                    {
                        Id = Guid.NewGuid(),
                        JobopeningId = jobopeningId,
                        VacancyPlaceId = vacancyPlaces.First().Id
                    });
                }
                else
                {
                    throw new Exception("Link error. Their number exceeds the allowed value.");
                }
            }
            else
            {
                throw new Exception("Uniqueness error. This jobopening already exists.");
            }
            await db.SaveChangesAsync();
            return jobopenings.First();
        }
        public async Task<ActionResult<Jobopening>> EditJobopening(string columnValue, string columnName, string newValue)
        {
            await using var db = new VacancyContext();
            Jobopening jobopeningToCheck = db.Jobopenings.FirstOrDefault(j => j.Name == newValue || j.Link == newValue);
            if (jobopeningToCheck != null) throw new Exception("This jobopening already exsist.");
            //добавить методы поиска по Name и LINK, вынести метод с определением LINK

            bool isId = Guid.TryParse(columnValue, out Guid id);
            if (isId) throw new Exception("Invalid column value.");
            Jobopening jobopeningToChange = db.Jobopenings.FirstOrDefault(j => j.Id == id);
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
}//Add
