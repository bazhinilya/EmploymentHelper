using EmploymentHelper.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmploymentHelper.ModelsLogic
{
    public class ViewLogic
    {
        public async Task<ActionResult<IEnumerable<AllSkill>>> GetAllSkillsView(string columnValue = null)
        {
            await using var db = new VacancyContext();
            if (columnValue == null)
            {
                return db.AllSkills.ToList();
            }
            if (Guid.TryParse(columnValue, out Guid id))
            {
                return new List<AllSkill>
                {
                    db.AllSkills.FirstOrDefault(a => a.Id == id) ?? throw new Exception("Invalid column value.")
                };
            }
            return db.AllSkills.Where(a => a.LevelType.Contains(columnValue) || a.LevelName.Contains(columnValue) || a.Skill.Contains(columnValue))
                               .ToList() ?? throw new Exception("Invalid column value.");
        }
    }
}
