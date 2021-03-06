using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmploymentHelper.BL
{
    public class EHLogic
    {
        public EHLogic(IConfiguration configuration) { }

        public async Task<ActionResult<IEnumerable<Accounts>>> GetAccounts()
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

        public async Task<ActionResult<IEnumerable<Jobopenings>>> GetJobopenings()
        {
            await using var db = new VacancyContext();
            return db.Jobopenings.ToList();
        }

        public async Task<ActionResult<IEnumerable<AllSkills>>> GetAllSkillsView(string jobopening)
        {
            await using var db = new VacancyContext();
            return db.AllSkills.ToList();
        }

        public async Task<ActionResult<bool>> AddVacancy(string vacancyPlaceName, string vacancyPlaceCode, string jobopeningName,
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
                db.VacancyPlaces.Add(new VacancyPlaces { Name = vacancyPlaceName, Id = vacancyPlaceId, Code = vacancyPlaceCode });
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
            return true;
        }

        public async Task<ActionResult<bool>> AddVacancyConditions(string jobopeningName, string conditionValue, string conditionType)
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
            await db.SaveChangesAsync();
            return true;
        }
    }
}
