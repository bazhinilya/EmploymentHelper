using EmploymentHelper.Models;
using EmploymentHelper.Models.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmploymentHelper.BL
{
    public class EHLogic
    {
        public EHLogic() { }
        //Lazy
        private readonly Type _specializationsType = typeof(Specialization);
        private readonly Type _vacancyConditionsType = typeof(VacancyCondition);
        private readonly Type _communicationsType = typeof(Communication);
        private readonly Type _contactsType = typeof(Contact);
        private readonly Type _accountsType = typeof(Account);
        private readonly Type _jobopeningsType = typeof(Jobopening);
        private readonly Type _skillsType = typeof(Skill);
        private readonly Type _vacancyPlacesType = typeof(VacancyPlace);

        //попробовать оставить IQueryable
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var accounts = db.Accounts.Where(a => a.Name == columnValue);//id
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.Accounts.Where(a => a.Id == id).ToList();
            }
            else if (accounts != null && columnValue != null)
            {
                return accounts.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");//перенести Exception
            }
            return db.Accounts.ToList();
        }
        public async Task<ActionResult<Account>> AddAccount(string name, string inn = null)
        {
            await using var db = new VacancyContext();
            var accounts = db.Accounts.Where(a => a.Name == name || a.INN == inn);
            Account account = null;
            if (accounts.Count() == 0)//заменить на !Any() -> проверить, не падает ли ошибка
            {
                account = new Account { Id = Guid.NewGuid(), Name = name, INN = inn };
                db.Accounts.Add(account);//создать экземпляр
                await db.SaveChangesAsync();
                return account;
            }
            else
            {
                throw new Exception("Uniqueness error. This account already exists.");
            }
        }
        public async Task<ActionResult<Account>> EditAccount(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var accounts = db.Accounts.Where(a => a.Id == id);
            if (accounts.Count() == 1)
            {
                bool isDirty = false;//заменить на bool
                foreach (var item in _accountsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(accounts.First(), columnValue);
                        isDirty = true;
                    }
                }
                if (isDirty)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.Accounts.Where(a => a.Name == accounts.First().Name || a.INN == accounts.First().INN).Count() != 1)
                {
                    throw new Exception("Error, you are trying to specify already existing data.");
                }
                await db.SaveChangesAsync();
                return accounts.First();
            }
            else
            {
                throw new Exception("Uniqueness error, there are several entities.");
            }
        }

        public async Task<ActionResult<IEnumerable<Communication>>> GetCommunications(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var communications = db.Communications.Where(c => c.CommType == columnValue || c.CommValue == columnValue);
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.Communications.Where(c => c.Id == id || c.AccountId == id || c.ContactId == id).ToList();
            }
            else if (communications != null && columnValue != null)
            {
                return communications.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.Communications.ToList();
        }
        public async Task<ActionResult<Communication>> AddCommunication(string accountColumnValue, string commType, string commValue,
            string contactColumnValue)
        {
            await using var db = new VacancyContext();
            var accounts = db.Accounts.Where(a => a.Name == accountColumnValue || a.INN == accountColumnValue);
            var contacts = db.Contacts.Where(c => c.FullName == contactColumnValue);
            var communications = db.Communications.Where(c => c.CommType == commType && c.ContactId == contacts.First().Id);
            if (accounts.Count() == 1 && contacts.Count() == 1 && communications.Count() == 0)
            {
                db.Communications.Add(new Communication
                {
                    Id = Guid.NewGuid(),
                    CommType = commType,
                    CommValue = commValue,
                    AccountId = accounts.First().Id,
                    ContactId = contacts.First().Id
                });
            }
            else
            {
                throw new Exception("Uniqueness error. This communication already exists.");
            }
            await db.SaveChangesAsync();
            return communications.First();
        }
        public async Task<ActionResult<Communication>> EditCommunication(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var communications = db.Communications.Where(c => c.Id == id);
            if (communications.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _communicationsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(communications.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.Communications.Where(c => c.CommType == communications.First().CommType
                                            && c.ContactId == communications.First().ContactId).Count() != 1)
                {
                    throw new Exception("Error, you are trying to specify already existing data.");
                }
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error, there are several Communications.");
            }
            return communications.First();
        }

        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var contacts = db.Contacts.Where(c => c.LastName.Contains(columnValue));
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.Contacts.Where(c => c.Id == id).ToList();
            }
            else if (contacts != null && columnValue != null)
            {
                return contacts.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.Contacts.ToList();
        }
        public async Task<ActionResult<Contact>> AddContact(string accountName, string lastName, string firstName, 
            bool gender, DateTime? birthDate, string middleName = null)
        {
            await using var db = new VacancyContext();
            var accounts = db.Accounts.Where(a => a.Name == accountName);
            var contacts = db.Contacts.Where(c => c.AccountId == accounts.First().Id && c.FirstName == firstName 
                                            && c.LastName == lastName && c.Gender == gender && c.BirthDate == birthDate);
            if (accounts.Count() == 0)
            {
                db.Accounts.Add(new Account { Id = Guid.NewGuid(), Name = accountName });
                await db.SaveChangesAsync();
            }
            if (accounts.Count() == 1 && contacts.Count() == 0)
            {
                db.Contacts.Add(new Contact
                {
                    Id = Guid.NewGuid(),
                    LastName = lastName,
                    FirstName = firstName,
                    MiddleName = middleName,
                    FullName = middleName == null ? $"{lastName} {firstName}" : $"{lastName} {firstName} {middleName}",
                    Gender = gender,
                    BirthDate = birthDate,
                    AccountId = accounts.First().Id
                });
            }
            else
            {
                throw new Exception("Uniqueness error. This contact already exists.");
            }
            await db.SaveChangesAsync();
            return contacts.First();
        }
        public async Task<ActionResult<Contact>> EditContact(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var contacts = db.Contacts.Where(c => c.Id == id);
            if (contacts.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _contactsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(contacts.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.Contacts.Where(c => c.AccountId == contacts.First().AccountId
                                        && (c.FullName == contacts.First().FullName || c.BirthDate == contacts.First().BirthDate))
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

            return contacts.First();
        }

        public async Task<ActionResult<IEnumerable<Jobopening>>> GetJobopenings(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var jobopenings = db.Jobopenings.Where(s => s.Name == columnValue);
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.Jobopenings.Where(j => j.Id == id).ToList();
            }
            else if (jobopenings != null && columnValue != null)
            {
                return jobopenings.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.Jobopenings.ToList();
        }
        public async Task<ActionResult<Jobopening>> AddJobopening(string specializationColumnValue, string vacancyPlaceColumnValue, 
            string name, string link, string accountName)
        {
            await using var db = new VacancyContext();
            var specializations = db.Specializations.Where(s => s.Name == specializationColumnValue
                                                            || s.Code == specializationColumnValue);
            var vacancyPlaces = db.VacancyPlaces.Where(s => s.Name == vacancyPlaceColumnValue || s.Code == vacancyPlaceColumnValue);
            var jobopenings = db.Jobopenings.Where(j => j.Name == name && j.Link == link);
            var accounts = db.Accounts.Where(a => a.Name == accountName);
            if (accounts == null || accounts.Count() == 0)
            {
                db.Accounts.Add(new Account { Id = Guid.NewGuid(), Name = accountName });
                await db.SaveChangesAsync();
            }
            if (accounts.Count() == 1 && vacancyPlaces.Count() == 1 && jobopenings.Count() == 0)
            {
                Guid jobopeningId = Guid.NewGuid();
                db.Jobopenings.Add(new Jobopening
                {
                    Id = jobopeningId,
                    Name = name,
                    Link = link,
                    SpecializationId = specializations.First().Id,
                    AccountId = accounts.First().Id
                });

                var jobopeningsVacancyPlaces = db.JobopeningsVacancyPlaces.Where(jvp => jvp.VacancyPlaceId == vacancyPlaces.First().Id
                                                                                && jvp.JobopeningId == jobopeningId);
                if (jobopeningsVacancyPlaces.Count() == 0)
                {
                    db.JobopeningsVacancyPlaces.Add(new JobopeningVacancyPlace 
                    { 
                        Id = Guid.NewGuid(), 
                        JobopeningId = jobopeningId, 
                        VacancyPlaceId = vacancyPlaces.First().Id
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
            return jobopenings.First();
        }
        public async Task<ActionResult<Jobopening>> EditJobopening(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var jobopenings = db.Jobopenings.Where(j => j.Id == id);
            if (jobopenings.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _jobopeningsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(jobopenings.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.Jobopenings.Where(j => j.AccountId == jobopenings.First().AccountId
                                                && j.Name == jobopenings.First().Name)
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
            return jobopenings.First();
        }

        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var skills = db.Skills.Where(s => s.Name == columnValue);
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.Skills.Where(s => s.Id == id).ToList();
            }
            else if (skills != null && columnValue != null)
            {
                return skills.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.Skills.ToList();
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

        public async Task<ActionResult<IEnumerable<Specialization>>> GetSpecializations(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var specializations = db.Specializations.Where(s => s.Name == columnValue || s.Code == columnValue);
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.Specializations.Where(s => s.Id == id).ToList();
            }
            else if (specializations != null && columnValue != null)
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

        public async Task<ActionResult<IEnumerable<VacancyCondition>>> GetVacancyConditions(string columnValue = null)
        {
            await using var db = new VacancyContext();
            var vacancyConditions = db.VacancyConditions.Where(c => c.ConditionType == columnValue || c.ConditionValue == columnValue);
            if (Guid.TryParse(columnValue, out Guid id) && columnValue != null)
            {
                return db.VacancyConditions.Where(c => c.Id == id || c.JobopeningId == id).ToList();
            }
            else if (vacancyConditions != null && columnValue != null)
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

        public async Task<ActionResult<IEnumerable<VacancyPlace>>> GetVacancyPlaces(string columnValue = null)
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
        public async Task<ActionResult<VacancyPlace>> AddVacancyPlace(string name, string code)
        {
            await using var db = new VacancyContext();
            var vacancyPlaces = db.VacancyPlaces.Where(vc => vc.Name == name || vc.Code == code);
            if (vacancyPlaces.Count() == 0)
            {
                db.VacancyPlaces.Add(new VacancyPlace { Id = Guid.NewGuid(), Name = name, Code = code });
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error. This vacancy place already exists.");
            }
            return vacancyPlaces.First();
        }
        public async Task<ActionResult<VacancyPlace>> EditVacancyPlace(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var vacancyPlaces = db.VacancyPlaces.Where(vp => vp.Id == id);
            if (vacancyPlaces.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _vacancyPlacesType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(vacancyPlaces.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.VacancyPlaces.Where(vp => vp.Id == vacancyPlaces.First().Id
                                                && vp.Name == vacancyPlaces.First().Name)
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
            return vacancyPlaces.First();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<ActionResult<IEnumerable<AllSkill>>> GetAllSkillsView(string columnValue = null)
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