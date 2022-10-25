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
    public class VacancyConditionLogic
    {
        private readonly PropertyInfo[] _vacancyConditionsProperties;
        public VacancyConditionLogic() { _vacancyConditionsProperties = typeof(Account).GetProperties(); }
        public async Task<ActionResult<IEnumerable<VacancyCondition>>> GetVacancyConditions(string columnValue = null)
        {
            await using var db = new VacancyContext();
            if (columnValue == null)
            {
                return db.VacancyConditions.ToList();
            }
            if (Guid.TryParse(columnValue, out Guid id))
            {
                return new List<VacancyCondition>
                {
                    db.VacancyConditions.FirstOrDefault(vc => vc.Id == id) ?? throw new Exception("Invalid column value.")
                };
            }
            return db.VacancyConditions.Where(vc => vc.ConditionType.Contains(columnValue) || vc.ConditionValue.Contains(columnValue))
                                       .ToList() ?? throw new Exception("Invalid column value.");
        }
        public async Task<ActionResult<VacancyCondition>> AddVacancyCondition(string jobopeningColumnValue, string conditionType,
            string conditionValue)
        {
            await using var db = new VacancyContext();
            var jobopenings = db.Jobopenings.Where(j => j.Id == Guid.Parse(jobopeningColumnValue) || j.Name == jobopeningColumnValue);
            var vacancyConditions = db.VacancyConditions.Where(vc => vc.ConditionType == conditionType
                                                                && vc.JobopeningId == jobopenings.First().Id);
            if (jobopenings.Count() == 1 && vacancyConditions.Count() == 0)
            {
                db.VacancyConditions.Add(new VacancyCondition
                {
                    Id = Guid.NewGuid(),
                    ConditionType = conditionType,
                    ConditionValue = conditionValue,
                    JobopeningId = jobopenings.First().Id
                });
            }
            else
            {
                throw new Exception("Uniqueness error. This vacancy condition already exists.");
            }
            await db.SaveChangesAsync();
            return db.VacancyConditions.FirstOrDefault(vc => vc.JobopeningId == jobopenings.First().Id
                                                        && vc.ConditionType == conditionType);
        }
        public async Task<ActionResult<VacancyCondition>> EditVacancyCondition(string columnValue, string columnName, string newValue)
        {
            await using var db = new VacancyContext();
            bool isId = Guid.TryParse(columnValue, out Guid id);
            if (isId) throw new Exception("Invalid column value.");
            VacancyCondition vacancyConditionToChange = db.VacancyConditions.FirstOrDefault(vc => vc.Id == id);
            if (vacancyConditionToChange == null) throw new Exception("Vacancy condition does not exist.");
            bool isDirty = true;
            foreach (var item in _vacancyConditionsProperties)
            {
                if (item.Name == columnName)
                {
                    item.SetValue(vacancyConditionToChange, newValue);
                    isDirty = false;
                }
            }
            if (isDirty) throw new Exception("The column name is incorrect");
            await db.SaveChangesAsync();
            return vacancyConditionToChange;
        }
    }//Add
}
