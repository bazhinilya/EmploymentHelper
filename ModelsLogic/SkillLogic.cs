using EmploymentHelper.Models.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace EmploymentHelper.ModelsLogic
{
    public class SkillLogic
    {
        private readonly Type _skillsType = typeof(Skill);
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
        public async Task<ActionResult<Skill>> EditSkill(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var skills = db.Skills.Where(s => s.Id == id);
            if (skills.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _skillsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(skills.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.Skills.Where(s => s.Id == skills.First().Id && s.Name == skills.First().Name).Count() != 1)
                {
                    throw new Exception("Error, you are trying to specify already existing data.");
                }
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error, there are several entities.");
            }
            return skills.First();
        }
    }//Add, Edit
}
