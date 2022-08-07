using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmploymentHelper.BL
{
    public class EHLogic /*: IEmploymentHelper*/
    {
        public EHLogic() { }

        public async Task<ActionResult<IEnumerable<Accounts>>> GetAllAccounts()
        {
            await using var db = new VacancyContext();
            return db.Accounts.ToList();
        }

        public async Task<ActionResult<IEnumerable<Accounts>>> GetAccount(string name)
        {
            await using var db = new VacancyContext();
            return db.Accounts
                .Where(a => a.Name.Contains(name))?
                .ToList();
        }

        public async Task<ActionResult<Accounts>> AddInn(string name, string inn)
        {
            await using var db = new VacancyContext();
            var accounts = db.Accounts.Where(a => a.Name == name);
            if (accounts != null && accounts.Count() == 1)
            {
                accounts.First().INN = inn;
            }
            else
            {
                throw new Exception("Uniqueness error, more than one account was found");
            }
            await db.SaveChangesAsync();
            return accounts.First();
        }

        public async Task<ActionResult<IEnumerable<AllSkills>>> GetAllSkillsView()
        {
            await using var db = new VacancyContext();
            return db.AllSkills.ToList();
        }

        public async Task<ActionResult<IEnumerable<Specializations>>> GetAllSpecializations()
        {
            await using var db = new VacancyContext();
            return db.Specializations.ToList();
        }

        public async Task<ActionResult<Specializations>> AddSpecialization(string name, string code)
        {
            await using var db = new VacancyContext();
            var specialization = db.Specializations.Where(s => s.Name == name);

            if (specialization == null || !specialization.Any())
            {
                db.Specializations.Add(new Specializations { Id = Guid.NewGuid(), Name = name, Code = code });
            }
            else
            {
                throw new Exception("This specialization already exists.");
            }
            await db.SaveChangesAsync();
            return specialization.First();
        }

        public async Task<ActionResult<IEnumerable<Jobopenings>>> GetAllJobopenings()
        {
            await using var db = new VacancyContext();
            return db.Jobopenings.ToList();
        }

        public async Task<ActionResult<Jobopenings>> AddVacancy(string vacancyPlaceName, string jobopeningName,
            string specializationCode, string accountName, string link)
        {
            await using var db = new VacancyContext();
            var vacancyPlace = db.VacancyPlaces.FirstOrDefault(vp => vp.Name == vacancyPlaceName);
            var account = db.Accounts.FirstOrDefault(a => a.Name == accountName);
            var specializations = db.Specializations.Where(s => s.Code == specializationCode);
            var jobopening = db.Jobopenings.Where(j => j.Name == jobopeningName);

            Guid vacancyPlaceId;
            if (vacancyPlace == null)
            {
                vacancyPlaceId = Guid.NewGuid();
                db.VacancyPlaces.Add(new VacancyPlaces { Name = vacancyPlaceName, Id = vacancyPlaceId });
            }
            else
            {
                vacancyPlaceId = vacancyPlace.Id;
            }

            Guid accountId;
            if (account == null)
            {
                accountId = Guid.NewGuid();
                db.Accounts.Add(new Accounts { Name = accountName, Id = accountId });
            }
            else
            {
                accountId = account.Id;
            }

            Guid specializationId;
            if (specializations != null && specializations.Count() == 1)
            {
                specializationId = specializations.First().Id;
            }
            else
            {
                throw new Exception("This specialization does not exist or more than one has been found.");
            }

            if (!jobopening?.Any() ?? true)
            {
                db.Jobopenings.Add(new Jobopenings
                {
                    Name = jobopeningName,
                    Id = Guid.NewGuid(),
                    AccountId = accountId,
                    SpecializationId = specializationId,
                    Link = link
                });
            }
            else
            {
                throw new Exception("This vacancy already exists, more than one has been found.");
            }
            await db.SaveChangesAsync();
            return jobopening.First();
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

        public async Task<ActionResult<IEnumerable<SkillsJobopening>>> AddSkill(string jobopeningName, string skillName)
        {
            await using var db = new VacancyContext();
            var jobopening = db.Jobopenings.Where(j => j.Name == jobopeningName);
            var skill = db.Skills.Where(sk => sk.Name == skillName);

            Guid skillId;
            Guid jobopeningId = jobopening.First().Id;

            if (jobopening != null )
            {
                skillId = Guid.NewGuid();
                db.Add(new Skills { Id = skillId, Name = skillName });
                await db.SaveChangesAsync();

                var jobopeningSkill = db.JobopeningsSkills.FirstOrDefault(js => js.SkillId == skillId);
                if (jobopeningSkill == null)
                {
                    db.Add(new JobopeningsSkills { Id = Guid.NewGuid(), SkillId = skillId, JobopeningId = jobopeningId });
                    await db.SaveChangesAsync();
                }
            }
            else
            {
                throw new Exception("Uniqueness error, this skill exists for this vacancy.");
            }
            return db.SkillsJobopening.Where(j => j.IdJobopening == jobopening.First().Id).ToList();
        }

        public async Task<ActionResult<IEnumerable<VacancyConditions>>> AddVacancyCondition(string jobopeningName, 
            string conditionValue, string conditionType)
        {
            await using var db = new VacancyContext();
            var vacancyConditions = db.VacancyConditions.FirstOrDefault(vc => vc.ConditionValue == conditionValue);
            var jobopening = db.Jobopenings.Where(j => j.Name == jobopeningName);

            Guid jobopeningId;
            if (jobopening != null && jobopening.Count() == 1)
            {
                jobopeningId = jobopening.First().Id;
            }
            else
            {
                throw new Exception("Uniqueness error, more than one vacancy was found.");
            }

            if (vacancyConditions == null)
            {
                db.VacancyConditions.Add(new VacancyConditions
                {
                    Id = Guid.NewGuid(),
                    ConditionValue = conditionValue,
                    ConditionType = conditionType,
                    JobopeningId = jobopeningId,
                });
            }
            else
            {
                jobopeningId = jobopening.First().Id;
            }
            await db.SaveChangesAsync();
            return db.VacancyConditions.Where(vc => vc.JobopeningId == jobopeningId).ToList();
        }

        public async Task<ActionResult<IEnumerable<VacancyConditions>>> DeleteCondition(Guid idFromVacancyConditions)
        {
            await using var db = new VacancyContext();
            var vacancyCondition = db.VacancyConditions.FirstOrDefault(vc => vc.Id == idFromVacancyConditions);
            if (vacancyCondition != null)
            {
                db.VacancyConditions.Remove(vacancyCondition);
                await db.SaveChangesAsync();
            }
            return db.VacancyConditions.ToList();
        }

        public async Task<ActionResult<Jobopenings>> DeleteConditions(Guid jobopeningId)
        {
            await using var db = new VacancyContext();
            var vacancyConditions = db.VacancyConditions.Where(vc => vc.JobopeningId == jobopeningId);
            if (vacancyConditions != null)
            {
                foreach (var vacancyCondition in vacancyConditions)
                {
                    db.VacancyConditions.Remove(vacancyCondition);
                }
                await db.SaveChangesAsync();
            }
            return db.Jobopenings.FirstOrDefault(j => j.Id == jobopeningId);
        }

        public async Task<ActionResult<Contacts>> AddContactAndCommunication(Guid accountId, string lastName, string firstName, 
            bool gender, DateTime? birthDate, string middleName = null, string commType = null, string commValue = null) 
        {
            await using var db = new VacancyContext();
            var contact = db.Contacts.Where(c => c.AccountId == accountId 
                                            && c.LastName == lastName && c.BirthDate == birthDate);
            var communication = db.Communications.Where(c => c.AccountId == accountId 
                                                        && c.CommType == commType && c.CommValue == commValue);
            Guid contactId;
            if (contact == null || !contact.Any())
            {
                contactId = Guid.NewGuid();
                db.Contacts.Add(new Contacts
                {
                    AccountId = accountId,
                    LastName = lastName,
                    FirstName = firstName,
                    MiddleName = middleName,
                    FullName = middleName == null ? $"{lastName} {firstName}" : $"{lastName} {firstName} {middleName}",
                    BirthDate = birthDate,
                    Gender = gender,
                    Id = contactId
                });
                await db.SaveChangesAsync();

                if ((communication == null || !communication.Any()) && (commType != null && commValue != null))
                {
                    db.Communications.Add(new Communications
                    {
                        AccountId = accountId,
                        CommType = commType,
                        CommValue = commValue,
                        ContactId = contactId,
                        Id = contactId
                    });
                    await db.SaveChangesAsync();
                }
            }
            else
            {
                throw new Exception("Uniqueness error, this contact or communication exists.");
            }

            return contact.First();//view contacts/communications
        }

        public async Task<ActionResult<Communications>> AddCommunication(Guid contactId, string commType, string commValue)
        {
            await using var db = new VacancyContext();
            var communication = db.Communications.Where(c => c.ContactId == contactId 
                                                        && c.CommType == commType && c.CommValue == commValue);
            var contact = db.Contacts.FirstOrDefault(c => c.Id == contactId);
            if (communication == null || !communication.Any())
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
                throw new Exception("Uniqueness error, more than one communication value was found.");
            }
            return communication.First();
        }
    }
}
