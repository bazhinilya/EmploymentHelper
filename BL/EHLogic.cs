using EmploymentHelper.Interface;
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
            if (account.Count() == 0)
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
            if (account.Count() == 1)
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
        public async Task<ActionResult<IEnumerable<Accounts>>> DeleteAccounts(Guid? id)
        {
            await DeleteContacts(id);
            var db = new VacancyContext();
            var accounts = db.Accounts.Where(a => a.Id == id);
            if (accounts != null)
            {
                foreach (var account in accounts)
                {
                    db.Accounts.Remove(account);
                }
            }
            else if (id == null)
            {
                foreach (var account in db.Accounts)
                {
                    db.Accounts.Remove(account);
                }
            }
            await db.SaveChangesAsync();
            return db.Accounts.ToList();
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
            var account = db.Accounts.Where(a => a.Name == accountColumnValue || a.INN == accountColumnValue);
            var contact = db.Contacts.Where(c => c.FullName == contactColumnValue);
            var communication = db.Communications.Where(c => c.CommType == commType && c.ContactId == contact.First().Id);
            if (account.Count() == 1 && contact.Count() == 1 && communication.Count() == 0)
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
            await db.SaveChangesAsync();
            return communication.First();
        }
        public async Task<ActionResult<Communications>> EditCommunication(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var communication = db.Communications.Where(c => c.Id == id);
            if (communication.Count() == 1)
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
            else if (id == null)
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
            if (account.Count() == 0)
            {
                db.Accounts.Add(new Accounts { Id = Guid.NewGuid(), Name = accountName });
                await db.SaveChangesAsync();
            }
            if (account.Count() == 1 && contact.Count() == 0)
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
            if (contact.Count() == 1)
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
        public async Task<ActionResult<IEnumerable<Contacts>>> DeleteContacts(Guid? id)
        {
            await DeleteCommunications(id);
            await using var db = new VacancyContext();
            var contacts = db.Contacts.Where(c => c.Id == id || c.AccountId == id);
            if (contacts != null)
            {
                foreach (var contact in contacts)
                {
                    db.Contacts.Remove(contact);
                }
            }
            else if (id == null)
            {
                foreach (var contact in db.Contacts)
                {
                    db.Contacts.Remove(contact);
                }
            }
            await db.SaveChangesAsync();
            return db.Contacts.ToList();
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
        public async Task<ActionResult<Jobopenings>> AddJobopening(string specializationColumnValue, string vacancyPlaceColumnValue, 
            string name, string link, string accountName)
        {
            await using var db = new VacancyContext();
            var specialization = db.Specializations.Where(s => s.Name == specializationColumnValue
                                                            || s.Code == specializationColumnValue);
            var vacancyPlace = db.VacancyPlaces.Where(s => s.Name == vacancyPlaceColumnValue || s.Code == vacancyPlaceColumnValue);
            var jobopening = db.Jobopenings.Where(j => j.Name == name && j.Link == link);
            var account = db.Accounts.Where(a => a.Name == accountName);
            if (account == null || account.Count() == 0)
            {
                db.Accounts.Add(new Accounts { Id = Guid.NewGuid(), Name = accountName });
                await db.SaveChangesAsync();
            }
            if (account.Count() == 1 && vacancyPlace.Count() == 1 && jobopening.Count() == 0)
            {
                Guid jobopeningId = Guid.NewGuid();
                db.Jobopenings.Add(new Jobopenings
                {
                    Id = jobopeningId,
                    Name = name,
                    Link = link,
                    SpecializationId = specialization.First().Id,
                    AccountId = account.First().Id
                });

                var jobopeningVacancyPlace = db.JobopeningsVacancyPlaces.Where(jvp => jvp.VacancyPlaceId == vacancyPlace.First().Id
                                                                                && jvp.JobopeningId == jobopeningId);
                if (jobopeningVacancyPlace.Count() == 0)
                {
                    db.JobopeningsVacancyPlaces.Add(new JobopeningsVacancyPlaces 
                    { 
                        Id = Guid.NewGuid(), 
                        JobopeningId = jobopeningId, 
                        VacancyPlaceId = vacancyPlace.First().Id
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
            return jobopening.First();
        }
        public async Task<ActionResult<Jobopenings>> EditJobopening(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var jobopening = db.Jobopenings.Where(j => j.Id == id);
            if (jobopening.Count() == 1)
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
        public async Task<ActionResult<IEnumerable<Jobopenings>>> DeleteJobopenings(Guid? id)
        {
            //контрагент остается
            await DeleteJobopeningVacancyPlace(id);
            await DeleteVacancyConditions(id);
            await DeleteSkills(id);
            await using var db = new VacancyContext();
            var jobopenings = db.Jobopenings.Where(j => j.Id == id || j.AccountId == id || j.SpecializationId == id);
            if (jobopenings != null)
            {
                foreach (var jobopening in jobopenings)
                {
                    db.Jobopenings.Remove(jobopening);
                }
            }
            else if (id == null)
            {
                foreach (var jobopening in db.Jobopenings)
                {
                    db.Jobopenings.Remove(jobopening);
                }
            }
            await db.SaveChangesAsync();
            return db.Jobopenings.ToList();
        }

        public async Task<ActionResult<IEnumerable<Skills>>> GetSkills(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var skill = db.Skills.Where(s => s.Name == columnValue);
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.Skills.Where(s => s.Id == id).ToList();
            }
            else if (skill != null && columnValue != null)
            {
                return skill.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.Skills.ToList();
        }
        public async Task<ActionResult<Skills>> AddSkill(string jobopeningColumnValue, string name)
        {
            await using var db = new VacancyContext();
            var jobopening = db.Jobopenings.Where(j => j.Name == jobopeningColumnValue);
            var skill = db.Skills.Where(s => s.Name == name);
            if (jobopening.Count() == 1 && skill.Count() == 0)
            {
                Guid skillId = Guid.NewGuid();
                db.Skills.Add(new Skills { Id = skillId, Name = name });
                await db.SaveChangesAsync();
                var jobopeningSkill = db.JobopeningsSkills.Where(js => js.JobopeningId == jobopening.First().Id 
                                                                    && js.SkillId == skillId);
                if (jobopeningSkill.Count() == 0)
                {
                    db.JobopeningsSkills.Add(new JobopeningsSkills
                    {
                        Id = Guid.NewGuid(),
                        JobopeningId = jobopening.First().Id,
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
            return skill.First();
        }
        public async Task<ActionResult<Skills>> EditSkill(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var skill = db.Skills.Where(s => s.Id == id);
            if (skill.Count() == 1)
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
        public async Task<ActionResult<IEnumerable<Skills>>> DeleteSkills(Guid? id)
        {
            await DeleteSpecializationSkill(id);
            await DeleteJobopeningSkill(id);
            await using var db = new VacancyContext();
            var skills = db.Skills.Where(s => s.Id == id);
            if (skills != null)
            {
                foreach (var skill in skills)
                {
                    db.Skills.Remove(skill);
                }
            }
            else if (id == null)
            {
                foreach (var skill in db.Skills)
                {
                    db.Skills.Remove(skill);
                }
            }
            await db.SaveChangesAsync();
            return db.Skills.ToList();
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

            if (specialization.Count() == 0)
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
            if (specialization.Count() == 1)
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
        public async Task<ActionResult<IEnumerable<Specializations>>> DeleteSpecializations(Guid? id)
        {
            await DeleteJobopenings(id);
            await DeleteSkills(id);
            await using var db = new VacancyContext();
            var specializations = db.Specializations.Where(s => s.Id == id);
            if (specializations != null)
            {
                foreach (var specialization in specializations)
                {
                    db.Specializations.Remove(specialization);
                }
            }
            else if (id == null)
            {
                foreach (var specialization in db.Specializations)
                {
                    db.Specializations.Remove(specialization);
                }
            }
            await db.SaveChangesAsync();
            return db.Specializations.ToList();
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
            if (jobopening.Count() == 1 && vacancyCondition.Count() == 0)
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
            if (vacancyCondition.Count() == 1)
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
            else if (id == null)
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
            if (vacancyPlace.Count() == 0)
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
            if (vacancyPlace.Count() == 1)
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
            await DeleteJobopeningVacancyPlace(id);
            await using var db = new VacancyContext();
            var vacancyPlace = db.VacancyPlaces.Where(vc => vc.Id == id);
            if (vacancyPlace != null)
            {
                foreach (var place in vacancyPlace)
                {
                    db.VacancyPlaces.Remove(place);
                }
            }
            else if (id == null)
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
        private async Task<ActionResult<IEnumerable<JobopeningsVacancyPlaces>>> DeleteJobopeningVacancyPlace(Guid? id)
        {
            await using var db = new VacancyContext();
            var jobopeningsVacancyPlaces = db.JobopeningsVacancyPlaces.Where(jvs => jvs.VacancyPlaceId == id || jvs.JobopeningId == id);
            if (jobopeningsVacancyPlaces != null)
            {
                foreach (var item in jobopeningsVacancyPlaces)
                {
                    db.JobopeningsVacancyPlaces.Remove(item);
                }
            }
            else
            {
                throw new Exception("Link error. These links do not exist.");
            }
            await db.SaveChangesAsync();
            return db.JobopeningsVacancyPlaces.ToList();
        }
        private async Task<ActionResult<IEnumerable<JobopeningsSkills>>> DeleteJobopeningSkill(Guid? id)
        {
            await using var db = new VacancyContext();
            var jobopeningsSkills = db.JobopeningsSkills.Where(js => js.JobopeningId == id || js.SkillId == id);
            if (jobopeningsSkills != null)
            {
                foreach (var item in jobopeningsSkills)
                {
                    db.JobopeningsSkills.Remove(item);
                }
            }
            else
            {
                throw new Exception("Link error. These links do not exist.");
            }
            await db.SaveChangesAsync();
            return db.JobopeningsSkills.ToList();
        }
        private async Task<ActionResult<IEnumerable<SpecializationsSkills>>> DeleteSpecializationSkill(Guid? id)
        {
            await using var db = new VacancyContext();
            var specializationsSkills = db.SpecializationsSkills.Where(ss => ss.SkillId == id || ss.SpecializationId == id);
            if (specializationsSkills != null)
            {
                foreach (var item in specializationsSkills)
                {
                    db.SpecializationsSkills.Remove(item);
                }
            }
            else
            {
                throw new Exception("Link error. These links do not exist.");
            }
            await db.SaveChangesAsync();
            return db.SpecializationsSkills.ToList();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<ActionResult<IEnumerable<AllSkills>>> GetAllSkillsView(string columnValue = null)
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
    }
}