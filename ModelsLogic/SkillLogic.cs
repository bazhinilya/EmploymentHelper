﻿using EmploymentHelper.Models.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Reflection;
using EmploymentHelper.BLogic;

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
            Jobopening jobopeningToCheck = null;
            bool isId = Guid.TryParse(jobopeningColumnValue, out Guid id);
            if (isId)
            {
                jobopeningToCheck = db.Jobopenings.FirstOrDefault(j => j.Id == id);
            }
            bool isLink = InnerLogic.IsLink(jobopeningColumnValue);
            if (isLink)
            {
                jobopeningToCheck = db.Jobopenings.FirstOrDefault(j => j.Link == jobopeningColumnValue);
            }
            if (!isId && !isLink)
            {
                jobopeningToCheck = db.Jobopenings.FirstOrDefault(j => j.Name == jobopeningColumnValue);
            }
            if (jobopeningToCheck == null) throw new Exception("Jobopening does not exist.");
            Guid skillId = Guid.NewGuid();
            Skill skillToCreate = new() { Id = skillId, Name = name };
            db.Skills.Add(skillToCreate);
            JobopeningSkill jobopeningSkillToCreate = new() { Id = Guid.NewGuid(), JobopeningId = jobopeningToCheck.Id, SkillId = skillId };
            db.JobopeningsSkills.Add(jobopeningSkillToCreate);
            await db.SaveChangesAsync();
            return skillToCreate;
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
    }
}
