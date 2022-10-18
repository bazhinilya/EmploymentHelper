using EmploymentHelper.Models.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace EmploymentHelper.ModelsLogic
{
    public class VacancyConditionLogic
    {
        private readonly Type _vacancyConditionsType = typeof(VacancyCondition);
        public async Task<ActionResult<IEnumerable<VacancyCondition>>> GetVacancyConditions(string columnValue = null)
        {
            await using var db = new VacancyContext();
            bool isId = Guid.TryParse(columnValue, out Guid id);
            var vacancyConditions = db.VacancyConditions.Where(c => c.ConditionType == columnValue || c.ConditionValue == columnValue
                                                                 || c.Id == id || c.JobopeningId == id);
            if (isId && columnValue != null)
            {
                return db.VacancyConditions.Where(c => c.Id == id || c.JobopeningId == id).ToList();
            }
            else if (vacancyConditions != null && columnValue != null && vacancyConditions.Any())
            {
                return vacancyConditions.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.VacancyConditions.ToList();
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
        public async Task<ActionResult<VacancyCondition>> EditVacancyCondition(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var vacancyConditions = db.VacancyConditions.Where(vc => vc.Id == id);
            if (vacancyConditions.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _vacancyConditionsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(vacancyConditions.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.VacancyConditions.Where(vc => vc.JobopeningId == vacancyConditions.First().JobopeningId
                                                    && vc.ConditionType == vacancyConditions.First().ConditionType)
                                             .Count() != 1)
                {
                    throw new Exception("Error, you are trying to specify already existing data.");
                }
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error, there are several VacancyConditions.");
            }
            return vacancyConditions.First();
        }
    }
}
