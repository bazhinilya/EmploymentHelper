using EmploymentHelper.Models.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace EmploymentHelper.ModelsLogic
{
    public class SpecializationLogic
    {
        private readonly Type _specializationsType = typeof(Specialization);
        public async Task<ActionResult<IEnumerable<Specialization>>> GetSpecializations(string columnValue = null)
        {
            await using var db = new VacancyContext();
            bool isId = Guid.TryParse(columnValue, out Guid id);
            var specializations = db.Specializations.Where(s => s.Name == columnValue || s.Code == columnValue || s.Id == id);
            if (isId && columnValue != null)
            {
                return db.Specializations.Where(s => s.Id == id).ToList();
            }
            else if (specializations != null && columnValue != null && specializations.Any())
            {
                return specializations.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.Specializations.ToList();
        }
        public async Task<ActionResult<Specialization>> AddSpecialization(string name, string code)
        {
            await using var db = new VacancyContext();
            var specializations = db.Specializations.Where(s => s.Name == name || s.Code == code);

            if (specializations.Count() == 0)
            {
                db.Specializations.Add(new Specialization { Id = Guid.NewGuid(), Name = name, Code = code });
            }
            else
            {
                throw new Exception("Uniqueness error. This specialization already exists.");
            }
            await db.SaveChangesAsync();
            return specializations.First();
        }
        public async Task<ActionResult<Specialization>> EditSpecialization(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var specializations = db.Specializations.Where(s => s.Id == id);
            if (specializations.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _specializationsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(specializations.First(), columnValue);
                        isDirty++;
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.Specializations.Where(s => s.Name == specializations.First().Name
                                                || s.Code == specializations.First().Code)
                                           .Count() != 1)
                {
                    throw new Exception("Error, you are trying to specify already existing data.");
                }
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error, there are several Specializations.");
            }
            return specializations.First();
        }
    }
}
