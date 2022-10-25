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
    public class SpecializationLogic
    {
        private readonly PropertyInfo[] _specializationsProperties;
        public SpecializationLogic() { _specializationsProperties = typeof(Account).GetProperties(); }
        public async Task<ActionResult<IEnumerable<Specialization>>> GetSpecializations(string columnValue = null)
        {
            await using var db = new VacancyContext();
            if (columnValue == null)
            {
                return db.Specializations.ToList();
            }
            if (Guid.TryParse(columnValue, out Guid id))
            {
                return new List<Specialization> 
                {
                    db.Specializations.FirstOrDefault(s => s.Id == id) ?? throw new Exception("Invalid column value.")
                };
            }
            return db.Specializations.Where(s => s.Code == columnValue || s.Name == columnValue)
                                     .ToList() ?? throw new Exception("Invalid column value.");
        }
        public async Task<ActionResult<Specialization>> AddSpecialization(string name, string code)
        {
            await using var db = new VacancyContext();
            Specialization specializationToCheck = db.Specializations.FirstOrDefault(s => s.Code == code || s.Name == name);
            if (specializationToCheck != null) throw new Exception("This specialization already exist.");
            Specialization specializationToCreate = new () { Id = Guid.NewGuid(), Name = name, Code = code };
            db.Specializations.Add(specializationToCreate);
            await db.SaveChangesAsync();
            return specializationToCreate;
        }
        public async Task<ActionResult<Specialization>> EditSpecialization(string columnValue, string columnName, string newValue)
        {
            await using var db = new VacancyContext();
            Specialization specializationToCheck = db.Specializations.FirstOrDefault(s => s.Code == newValue || s.Name == newValue);
            if (specializationToCheck != null) throw new Exception("This specialization already exsist.");
            Specialization specializationToChange = null;
            bool isId = Guid.TryParse(columnValue, out Guid id);
            if (isId)
            {
                specializationToChange = db.Specializations.FirstOrDefault(s => s.Id == id);
            }
            if (columnValue.Length <= 4)
            {
                specializationToChange = db.Specializations.FirstOrDefault(s => s.Code == columnValue || s.Name == columnValue);
            }
            if (columnValue.Length > 4)
            {
                specializationToChange = db.Specializations.FirstOrDefault(s => s.Name == columnValue || s.Code == columnValue);
            }
            if (specializationToChange == null) throw new Exception("Specialization does not exist.");
            bool isDirty = true;
            foreach (var item in _specializationsProperties)
            {
                if (item.Name == columnName)
                {
                    item.SetValue(specializationToChange, newValue);
                    isDirty = false;
                }
            }
            if (isDirty) throw new Exception("The column name is incorrect");
            await db.SaveChangesAsync();
            return specializationToChange;
        }
    }
}
