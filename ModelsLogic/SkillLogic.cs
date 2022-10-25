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
    public class SkillLogic
    {
        private readonly PropertyInfo[] _skillsProperties;
        public SkillLogic() { _skillsProperties = typeof(Account).GetProperties(); }
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills(string columnValue = null)
        {
            await using var db = new VacancyContext();
            if (columnValue == null)
            {
                return db.Skills.ToList();
            }
            if (Guid.TryParse(columnValue, out Guid id))
            {
                return new List<Skill>
                {
                    db.Skills.FirstOrDefault(s => s.Id == id) ?? throw new Exception("Invalid column value.")
                };
            }
            return db.Skills.Where(s => s.Name.Contains(columnValue)).ToList() ?? throw new Exception("Invalid column value.");
        }
        public async Task<ActionResult<Skill>> AddSkill(string jobopeningColumnValue, string name)
        {
            await using var db = new VacancyContext();
            var jobopenings = db.Jobopenings.Where(j => j.Name == jobopeningColumnValue);
            var skills = db.Skills.Where(s => s.Name == name);
            if (jobopenings.Count() == 1 && skills.Count() == 0)
            {
                Guid skillId = Guid.NewGuid();
                db.Skills.Add(new Skill { Id = skillId, Name = name });
                await db.SaveChangesAsync();
                var jobopeningsSkills = db.JobopeningsSkills.Where(js => js.JobopeningId == jobopenings.First().Id
                                                                    && js.SkillId == skillId);
                if (jobopeningsSkills.Count() == 0)
                {
                    db.JobopeningsSkills.Add(new JobopeningSkill
                    {
                        Id = Guid.NewGuid(),
                        JobopeningId = jobopenings.First().Id,
                        SkillId = skillId
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
            return skills.First();
        }
        public async Task<ActionResult<Skill>> EditSkill(string columnValue, string columnName, string newValue)
        {
            await using var db = new VacancyContext();
            bool isId = Guid.TryParse(columnValue, out Guid id);
            if (isId) throw new Exception("Invalid column value.");
            Skill skillToChange = db.Skills.FirstOrDefault(s => s.Id == id);
            if (skillToChange == null) throw new Exception("Skill does not exist.");
            bool isDirty = true;
            foreach (var item in _skillsProperties)
            {
                if (item.Name == columnName)
                {
                    item.SetValue(skillToChange, newValue);
                    isDirty = false;
                }
            }
            if (isDirty) throw new Exception("The column name is incorrect");
            await db.SaveChangesAsync();
            return skillToChange;
        }
    }//Add
}
