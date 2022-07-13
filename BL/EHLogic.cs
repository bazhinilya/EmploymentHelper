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
            /*new Regex($"{job.Name}").IsMatch(jobopening)*/
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

        #region other methods
        //public async Task<ActionResult<IEnumerable<Contact>>> GetContactByLastName(string lastName)
        //{
        //    await using var db = new AccountsContext();
        //    return db.Contacts
        //        .Where(с => с.LastName.Contains(lastName))?
        //        .ToList();
        //}

        //public async Task<ActionResult<bool>> UpdateContactBirthDate(int id, DateTime birthDate)
        //{
        //    await using var db = new AccountsContext();

        //    var contactToUpdate = db.Contacts.Where(c => c.Id == id).FirstOrDefault();

        //    if (contactToUpdate != null)
        //    {
        //        contactToUpdate.BirthDate = birthDate;
        //        await db.SaveChangesAsync();
        //        return true;
        //    }
        //    return false; 
        //}

        public async Task<ActionResult<bool>> AddContactAccount(int contactId, int accountId)
        {
            await using var db = new AccountsContext();
            var contactAccount = db.ContactsAccounts
                    .Where(ca => ca.ContactId == contactId && ca.AccountId == accountId)
                    .FirstOrDefault();

            if (contactAccount == null)
            {
                db.ContactsAccounts.Add(new ContactAccount { ContactId = contactId, AccountId = accountId });
                await db.SaveChangesAsync();
            }
            return true;
        }
        #endregion


        public async Task<ActionResult<bool>> AddVacancy(string vacancyPlacesName, string jobopeningName, string accountName)
        {
            await using var db = new AccountsContext();

            var vacancyToAdd = db.VacancyPlaces
                    .Where(vp => vp.Name == vacancyPlacesName)
                    .FirstOrDefault();

            var jobopeningToAdd = db.Jobopenings
                    .Where(j => j.Name == jobopeningName)
                    .FirstOrDefault();
            
            var accountToAdd = db.Accounts
                    .Where(a => a.Name == accountName)
                    .FirstOrDefault();

            if (vacancyToAdd == null)
            {
                db.VacancyPlaces.Add(new VacancyPlaces { Name = vacancyPlacesName, Id = Guid.NewGuid(), Code = });
                await db.SaveChangesAsync();
            }
            if (jobopeningName == null)
            {
                db.Jobopenings.Add(new Jobopenings { Name = jobopeningName, Id = Guid.NewGuid(), AccountId = , Specialization = , WorkExperienceInYears =  });
                await db.SaveChangesAsync();
            }
            if (accountToAdd == null)
            {
                db.Accounts.Add(new Accounts { Name = accountName, Id = Guid.NewGuid(), INN = });
                await db.SaveChangesAsync();
            }
            return true;
        }
    }
}
