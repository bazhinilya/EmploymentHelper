﻿using EmploymentHelper.Interface;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmploymentHelper.BL
{
    public class EHLogic : IEmploymentHelper
    {
        public EHLogic() { }

        private readonly Type _specializationsType = typeof(Specializations);
        private readonly Type _vacancyConditionsType = typeof(VacancyConditions);
        private readonly Type _communicationsType = typeof(Communications);
        private readonly Type _contactsType = typeof(Contacts);
        private readonly Type _accountsType = typeof(Accounts);
        private readonly Type _jobopeningsType = typeof(Jobopenings);
        private readonly Type _skillsType = typeof(Skills);
        private readonly Type _vacancyPlacesType = typeof(VacancyPlaces);

        public async Task<ActionResult<IEnumerable<Accounts>>> GetAccounts(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var account = db.Accounts.Where(a => a.Name == columnValue);
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.Accounts.Where(a => a.Id == id).ToList();
            }
            else if (account != null && columnValue != null)
            {
                return account.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.Accounts.ToList();
        }
        public async Task<ActionResult<Accounts>> AddAccount(string name, string inn = null)
        {
            await using var db = new VacancyContext();
            var account = db.Accounts.Where(a => a.Name == name || a.INN == inn);
            if (account == null || !account.Any())
            {
                db.Accounts.Add(new Accounts { Id = Guid.NewGuid(), Name = name, INN = inn });
            }
            else
            {
                throw new Exception("Uniqueness error. This account already exists.");
            }
            await db.SaveChangesAsync();
            return account.First();
        }
        public async Task<ActionResult<Accounts>> EditAccount(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var account = db.Accounts.Where(a => a.Id == id);
            if (account != null && account.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _accountsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(account.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.Accounts.Where(a => a.Name == account.First().Name || a.INN == account.First().INN).Count() != 1)
                {
                    throw new Exception("Error, you are trying to specify already existing data.");
                }
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error, there are several entities.");
            }

            return account.First();
        }

        public async Task<ActionResult<IEnumerable<Communications>>> GetCommunications(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var communication = db.Communications.Where(c => c.CommType == columnValue || c.CommValue == columnValue);
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.Communications.Where(c => c.Id == id || c.AccountId == id || c.ContactId == id).ToList();
            }
            else if (communication != null && columnValue != null)
            {
                return communication.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.Communications.ToList();
        }
        public async Task<ActionResult<Communications>> AddCommunication(string accountColumnValue, string commType, string commValue,
            string contactColumnValue)
        {
            await using var db = new VacancyContext();
            var account = db.Accounts.Where(a => a.Id == Guid.Parse(accountColumnValue) || a.Name == accountColumnValue
                                            || a.INN == accountColumnValue);
            var contact = db.Contacts.Where(c => c.Id == Guid.Parse(contactColumnValue) || c.FullName == accountColumnValue);
            var communication = db.Communications.Where(c => c.CommType == commType && c.ContactId == contact.First().Id);
            if (account.Count() == 1 && contact.Count() == 1
                && (communication == null || !communication.Any()))
            {
                db.Communications.Add(new Communications
                {
                    Id = Guid.NewGuid(),
                    CommType = commType,
                    CommValue = commValue,
                    AccountId = account.First().Id,
                    ContactId = contact.First().Id
                });
            }
            else
            {
                throw new Exception("Uniqueness error. This communication already exists.");
            }
            return communication.First();
        }
        public async Task<ActionResult<Communications>> EditCommunication(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var communication = db.Communications.Where(c => c.Id == id);
            if (communication != null && communication.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _communicationsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(communication.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.Communications.Where(c => c.CommType == communication.First().CommType
                                            && c.ContactId == communication.First().ContactId).Count() != 1)
                {
                    throw new Exception("Error, you are trying to specify already existing data.");
                }
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error, there are several Communications.");
            }
            return communication.First();
        }
        public async Task<ActionResult<IEnumerable<Communications>>> DeleteCommunications(Guid? id)
        {
            await using var db = new VacancyContext();
            var communications = db.Communications.Where(c => c.Id == id || c.AccountId == id || c.ContactId == id);
            if (communications != null)
            {
                foreach (var communication in communications)
                {
                    db.Communications.Remove(communication);
                }
            }
            else
            {
                foreach (var communication in db.Communications)
                {
                    db.Communications.Remove(communication);
                }
            }
            await db.SaveChangesAsync();
            return db.Communications.ToList();
        }

        public async Task<ActionResult<IEnumerable<Contacts>>> GetContacts(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var contact = db.Contacts.Where(c => c.LastName.Contains(columnValue));
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.Contacts.Where(c => c.Id == id).ToList();
            }
            else if (contact != null && columnValue != null)
            {
                return contact.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.Contacts.ToList();
        }
        public async Task<ActionResult<Contacts>> AddContact(string accountName, string lastName, string firstName, 
            bool gender, DateTime? birthDate, string middleName = null)
        {
            await using var db = new VacancyContext();
            var account = db.Accounts.Where(a => a.Name == accountName);
            var contact = db.Contacts.Where(c => c.AccountId == account.First().Id && c.FirstName == firstName 
                                            && c.LastName == lastName && c.Gender == gender && c.BirthDate == birthDate);
            if (account == null || !account.Any())
            {
                db.Accounts.Add(new Accounts { Id = Guid.NewGuid(), Name = accountName });
                await db.SaveChangesAsync();
            }
            if (account.Count() == 1 && (contact == null || !contact.Any()))
            {
                db.Contacts.Add(new Contacts
                {
                    Id = Guid.NewGuid(),
                    LastName = lastName,
                    FirstName = firstName,
                    MiddleName = middleName,
                    FullName = middleName == null ? $"{lastName} {firstName}" : $"{lastName} {firstName} {middleName}",
                    Gender = gender,
                    BirthDate = birthDate,
                    AccountId = account.First().Id
                });
            }
            else
            {
                throw new Exception("Uniqueness error. This contact already exists.");
            }
            await db.SaveChangesAsync();
            return contact.First();
        }
        public async Task<ActionResult<Contacts>> EditContact(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var contact = db.Contacts.Where(c => c.Id == id);
            if (contact != null && contact.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _contactsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(contact.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.Contacts.Where(c => c.AccountId == contact.First().AccountId
                                        && (c.FullName == contact.First().FullName || c.BirthDate == contact.First().BirthDate))
                               .Count() != 1)
                {
                    throw new Exception("Error, you are trying to specify already existing data.");
                }
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error, there are several entities.");
            }

            return contact.First();
        }

        public async Task<ActionResult<IEnumerable<Jobopenings>>> GetJobopenings(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var jobopening = db.Jobopenings.Where(s => s.Name == columnValue);
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.Jobopenings.Where(j => j.Id == id).ToList();
            }
            else if (jobopening != null && columnValue != null)
            {
                return jobopening.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.Jobopenings.ToList();
        }
        public async Task<ActionResult<Jobopenings>> AddJobopening(string vacancyPlaceColumnValue, string name, string link,
            string accountName)
        {
            await using var db = new VacancyContext();
            var specialization = db.Specializations.Where(s => s.Id == Guid.Parse(vacancyPlaceColumnValue) 
                                                            || s.Name == vacancyPlaceColumnValue
                                                            || s.Code == vacancyPlaceColumnValue);
            var jobopening = db.Jobopenings.Where(j => (j.Name == name || j.Link == link)
                                                    && j.SpecializationId == specialization.First().Id);
            var account = db.Accounts.Where(a => a.Name == accountName);
            if (account == null || !account.Any())
            {
                db.Accounts.Add(new Accounts { Id = Guid.NewGuid(), Name = accountName });
                await db.SaveChangesAsync();
            }
            if (account.Count() == 1 && specialization.Count() == 1
                && (jobopening == null || !jobopening.Any()))
            {
                db.Jobopenings.Add(new Jobopenings
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Link = link,
                    SpecializationId = specialization.First().Id,
                    AccountId = account.First().Id
                });
            }
            else
            {
                throw new Exception("Uniqueness error. This jobopening already exists.");
            }
            await db.SaveChangesAsync();
            return jobopening.First();
        }
        public async Task<ActionResult<Jobopenings>> EditJobopening(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var jobopening = db.Jobopenings.Where(j => j.Id == id);
            if (jobopening != null && jobopening.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _jobopeningsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(jobopening.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.Jobopenings.Where(j => j.AccountId == jobopening.First().AccountId
                                                && j.Name == jobopening.First().Name)
                                       .Count() != 1)
                {
                    throw new Exception("Error, you are trying to specify already existing data.");
                }
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error, there are several entities.");
            }
            return jobopening.First();
        }

        public async Task<ActionResult<Skills>> EditSkill(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var skill = db.Skills.Where(s => s.Id == id);
            if (skill != null && skill.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _skillsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(skill.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.Skills.Where(s => s.Id == skill.First().Id && s.Name == skill.First().Name).Count() != 1)
                {
                    throw new Exception("Error, you are trying to specify already existing data.");
                }
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error, there are several entities.");
            }
            return skill.First();
        }

        public async Task<ActionResult<IEnumerable<Specializations>>> GetSpecializations(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var specialization = db.Specializations.Where(s => s.Name == columnValue || s.Code == columnValue);
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.Specializations.Where(s => s.Id == id).ToList();
            }
            else if (specialization != null && columnValue != null)
            {
                return specialization.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.Specializations.ToList();
        }
        public async Task<ActionResult<Specializations>> AddSpecialization(string name, string code)
        {
            await using var db = new VacancyContext();
            var specialization = db.Specializations.Where(s => s.Name == name || s.Code == code);

            if (specialization == null || !specialization.Any())
            {
                db.Specializations.Add(new Specializations { Id = Guid.NewGuid(), Name = name, Code = code });
            }
            else
            {
                throw new Exception("Uniqueness error. This specialization already exists.");
            }
            await db.SaveChangesAsync();
            return specialization.First();
        }
        public async Task<ActionResult<Specializations>> EditSpecialization(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var specialization = db.Specializations.Where(s => s.Id == id);
            if (specialization != null && specialization.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _specializationsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(specialization.First(), columnValue);
                        isDirty++;
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.Specializations.Where(s => s.Name == specialization.First().Name
                                                || s.Code == specialization.First().Code)
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
            return specialization.First();
        }

        public async Task<ActionResult<IEnumerable<VacancyConditions>>> GetVacancyConditions(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var conditions = db.VacancyConditions.Where(c => c.ConditionType == columnValue || c.ConditionValue == columnValue);
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.VacancyConditions.Where(c => c.Id == id || c.JobopeningId == id).ToList();
            }
            else if (conditions != null && columnValue != null)
            {
                return conditions.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.VacancyConditions.ToList();
        }
        public async Task<ActionResult<VacancyConditions>> AddVacancyCondition(string jobopeningColumnValue, string conditionType,
            string conditionValue)
        {
            await using var db = new VacancyContext();
            var jobopening = db.Jobopenings.Where(j => j.Id == Guid.Parse(jobopeningColumnValue) || j.Name == jobopeningColumnValue);
            var vacancyCondition = db.VacancyConditions.Where(vc => vc.ConditionType == conditionType
                                                                && vc.JobopeningId == jobopening.First().Id);
            if (jobopening.Count() == 1 && (vacancyCondition == null || !vacancyCondition.Any()))
            {
                db.VacancyConditions.Add(new VacancyConditions
                {
                    Id = Guid.NewGuid(),
                    ConditionType = conditionType,
                    ConditionValue = conditionValue,
                    JobopeningId = jobopening.First().Id
                });
            }
            else
            {
                throw new Exception("Uniqueness error. This vacancy condition already exists.");
            }
            await db.SaveChangesAsync();
            return db.VacancyConditions.FirstOrDefault(vc => vc.JobopeningId == jobopening.First().Id
                                                        && vc.ConditionType == conditionType);
        }
        public async Task<ActionResult<VacancyConditions>> EditVacancyCondition(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var vacancyCondition = db.VacancyConditions.Where(vc => vc.Id == id);
            if (vacancyCondition != null && vacancyCondition.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _vacancyConditionsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(vacancyCondition.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.VacancyConditions.Where(vc => vc.JobopeningId == vacancyCondition.First().JobopeningId
                                                    && vc.ConditionType == vacancyCondition.First().ConditionType)
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
            return vacancyCondition.First();
        }
        public async Task<ActionResult<IEnumerable<VacancyConditions>>> DeleteVacancyConditions(Guid? id)
        {
            await using var db = new VacancyContext();
            var vacancyConditions = db.VacancyConditions.Where(vc => vc.Id == id || vc.JobopeningId == id);
            if (vacancyConditions != null)
            {
                foreach (var condition in vacancyConditions)
                {
                    db.VacancyConditions.Remove(condition);
                }
            }
            else
            {
                foreach (var condition in db.VacancyConditions)
                {
                    db.VacancyConditions.Remove(condition);
                }
            }
            await db.SaveChangesAsync();
            return db.VacancyConditions.ToList();
        }

        public async Task<ActionResult<IEnumerable<VacancyPlaces>>> GetVacancyPlaces(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var vacancyPlaces = db.VacancyPlaces.Where(p => p.Name == columnValue);
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.VacancyPlaces.Where(p => p.Id == id).ToList();
            }
            else if (vacancyPlaces != null && columnValue != null)
            {
                return vacancyPlaces.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.VacancyPlaces.ToList();
        }
        public async Task<ActionResult<VacancyPlaces>> AddVacancyPlace(string name, string code)
        {
            await using var db = new VacancyContext();
            var vacancyPlace = db.VacancyPlaces.Where(vc => vc.Name == name || vc.Code == code);
            if (vacancyPlace == null || !vacancyPlace.Any())
            {
                db.VacancyPlaces.Add(new VacancyPlaces { Id = Guid.NewGuid(), Name = name, Code = code });
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error. This vacancy place already exists.");
            }
            return vacancyPlace.First();
        }
        public async Task<ActionResult<VacancyPlaces>> EditVacancyPlace(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var vacancyPlace = db.VacancyPlaces.Where(vp => vp.Id == id);
            if (vacancyPlace != null && vacancyPlace.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _vacancyPlacesType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(vacancyPlace.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.VacancyPlaces.Where(vp => vp.Id == vacancyPlace.First().Id
                                                && vp.Name == vacancyPlace.First().Name)
                                         .Count() != 1)
                {
                    throw new Exception("Error, you are trying to specify already existing data.");
                }
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error, there are several entities.");
            }
            return vacancyPlace.First();
        }
        public async Task<ActionResult<IEnumerable<VacancyPlaces>>> DeleteVacancyPlaces(Guid? id)
        {
            await using var db = new VacancyContext();
            var vacancyPlace = db.VacancyPlaces.Where(vc => vc.Id == id);
            if (vacancyPlace != null)
            {
                foreach (var place in vacancyPlace)
                {
                    db.VacancyPlaces.Remove(place);
                }
            }
            else
            {
                foreach (var place in db.VacancyPlaces)
                {
                    db.VacancyPlaces.Remove(place);
                }
            }
            await db.SaveChangesAsync();
            return db.VacancyPlaces.ToList();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        


        public async Task<ActionResult<Communications>> AddCommunication(Guid contactId, string commType, string commValue)
        {
            await using var db = new VacancyContext();
            var communication = db.Communications.Where(c => c.ContactId == contactId
                                                        && c.CommType == commType && c.CommValue == commValue);
            var contact = db.Contacts.FirstOrDefault(c => c.Id == contactId);
            if (communication == null || communication.Count() == 1)
            {
                db.Add(new Communications
                {
                    Id = Guid.NewGuid(),
                    CommType = commType,
                    CommValue = commValue,
                    AccountId = contact.AccountId,
                    ContactId = contactId
                });
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error. This communication method already exists");
            }
            return communication.First();
        }

        public async Task<ActionResult<IEnumerable<Jobopenings>>> DeleteVacancy(Guid idFromJobopenings)
        {
            await using var db = new VacancyContext();
            var vacancyConditions = db.VacancyConditions.Where(vc => vc.JobopeningId == idFromJobopenings);
            var jobopening = db.Jobopenings.FirstOrDefault(j => j.Id == idFromJobopenings);
            var account = db.Accounts.FirstOrDefault(a => a.Id == jobopening.AccountId);

            if (vacancyConditions != null)
            {
                foreach (var vacancyCondition in vacancyConditions)
                {
                    db.VacancyConditions.Remove(vacancyCondition);
                }
                await db.SaveChangesAsync();
            }

            if (jobopening != null)
            {
                db.Jobopenings.Remove(jobopening);
                await db.SaveChangesAsync();
            }

            if (account != null)
            {
                db.Accounts.Remove(account);
                await db.SaveChangesAsync();
            }

            return db.Jobopenings.ToList();
        }

        public async Task<ActionResult<IEnumerable<AllSkills>>> GetSkillsView(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var skills = db.AllSkills.Where(s => s.LevelType == columnValue || s.LevelName == columnValue);
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.AllSkills.Where(s => s.Id == id).ToList();
            }
            else if (skills != null && columnValue != null)
            {
                return skills.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.AllSkills.ToList();
        }

        //public async Task<ActionResult<IEnumerable<SkillsJobopening>>> AddSkill(string jobopeningName, string skillName)
        //{
        //    await using var db = new VacancyContext();
        //    var jobopening = db.Jobopenings.Where(j => j.Name == jobopeningName);
        //    var skill = db.Skills.Where(sk => sk.Name == skillName);

        //    Guid skillId;
        //    Guid jobopeningId = jobopening.First().Id;

        //    if (jobopening != null)
        //    {
        //        skillId = Guid.NewGuid();
        //        db.Add(new Skills { Id = skillId, Name = skillName });
        //        await db.SaveChangesAsync();

        //        var jobopeningSkill = db.JobopeningsSkills.FirstOrDefault(js => js.SkillId == skillId);
        //        if (jobopeningSkill == null)
        //        {
        //            db.Add(new JobopeningsSkills { Id = Guid.NewGuid(), SkillId = skillId, JobopeningId = jobopeningId });
        //            await db.SaveChangesAsync();
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("Uniqueness error, this skill exists for this vacancy.");
        //    }
        //    return db.SkillsJobopening.Where(j => j.IdJobopening == jobopening.First().Id).ToList();
        //}




    }
}