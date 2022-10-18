using EmploymentHelper.Models.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace EmploymentHelper.ModelsLogic
{
    public class ViewLogic
    {
        public async Task<ActionResult<IEnumerable<AllSkill>>> GetAllSkillsView(string columnValue = null)
        {
            await using var db = new VacancyContext();
            bool isId = Guid.TryParse(columnValue, out Guid id);
            var allSkills = db.AllSkills.Where(s => s.LevelType == columnValue || s.LevelName == columnValue || s.Id == id);
            if (isId && columnValue != null)
            {
                return db.AllSkills.Where(s => s.Id == id).ToList();
            }
            else if (allSkills != null && columnValue != null && allSkills.Any())
            {
                return allSkills.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.AllSkills.ToList();
        }

    }
}
