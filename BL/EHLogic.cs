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
            await using var db = new AccountsContext();
            return db.Accounts.ToList();
        }

        public async Task<ActionResult<IEnumerable<Accounts>>> GetAccountByName(string name)
        {
            await using var db = new AccountsContext();
            return db.Accounts
                .Where(a => a.Name.Contains(name))?
                .ToList();
        }

        public async Task<ActionResult<IEnumerable<Jobopenings>>> GetAllJobopenings()
        {
            await using var db = new AccountsContext();
            return db.Jobopenings.ToList();
        }

        public async Task<ActionResult<IEnumerable<Skills>>> GetSkillsForJobopening(string jobopening)
        {
            await using var db = new AccountsContext();
            return db.Jobopenings
                .Where(j => j.Name.Contains(jobopening))
                .Join(db.JobopeningsSkills,
                    j => j.Id,
                    js => js.JobopeningId,
                    (j, js) => new
                    {
                        SkillId = js.SkillId,
                    })?
                .Join(db.Skills,
                    js => js.SkillId,
                    s => s.Id,
                    (js, s) => new Skills
                    {
                        Name = s.Name,
                        Id = s.Id,
                    })?
                .ToList();
            #region 2 approach
            //return
            //    (from job in db.Jobopenings.ToList()
            //     join jobSkl in db.JobopeningsSkills.ToList()
            //         on job.Id equals jobSkl.JobopeningId
            //     join skl in db.Skills.ToList()
            //         on jobSkl.SkillId equals skl.Id
            //     where job.Name.Contains(jobopening)
            //     orderby skl.Name
            //     select new Skills
            //     {
            //         Id = skl.Id,
            //         Name = skl.Name
            //     })?
            //    .ToList();
            #endregion
        }

        public async Task<ActionResult<bool>> AddVacancy(string vacancyPlaceName, string code, string jobopeningName, 
            string specializationName, byte workExperienceInYears, string accountName, string link)
        {
            await using var db = new AccountsContext();
            var vacancyPlace = db.VacancyPlaces.FirstOrDefault(vp => vp.Name == vacancyPlaceName);
            var account = db.Accounts.FirstOrDefault(a => a.Name == accountName);
            var jobopening = db.Jobopenings.Where(j => j.Name == jobopeningName);

            Guid vacancyPlaceId;
            if (vacancyPlace == null)
            {
                vacancyPlaceId = Guid.NewGuid();
                db.VacancyPlaces.Add(new VacancyPlaces { Name = vacancyPlaceName, Id = vacancyPlaceId, Code = code });
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

            if (!jobopening?.Any() ?? true)
            {
                db.Jobopenings.Add(new Jobopenings
                {
                    Name = jobopeningName,
                    Id = Guid.NewGuid(),
                    AccountId = accountId,
                    Specialization = specializationName,
                    WorkExperienceInYears = workExperienceInYears,
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
    }
}
