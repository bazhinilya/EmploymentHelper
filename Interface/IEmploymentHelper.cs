using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmploymentHelper.Interface
{
    public interface IEmploymentHelper
    {
        Task<ActionResult<IEnumerable<Accounts>>> GetAccounts(string columnValue = null);
        Task<ActionResult<Accounts>> EditAccount(Guid id, string columnName, string columnValue);
        Task<ActionResult<Communications>> EditCommunication(Guid id, string columnName, string columnValue);
        Task<ActionResult<Contacts>> EditContact(Guid id, string columnName, string columnValue);
        Task<ActionResult<IEnumerable<Jobopenings>>> GetJobopenings(string columnValue = null);
        Task<ActionResult<Jobopenings>> EditJobopening(Guid id, string columnName, string columnValue);
        Task<ActionResult<Skills>> EditSkill(Guid id, string columnName, string columnValue);
        Task<ActionResult<IEnumerable<Specializations>>> GetSpecializations(string columnValue = null);
        Task<ActionResult<Specializations>> AddSpecialization(string name, string code);
        Task<ActionResult<Specializations>> EditSpecialization(Guid id, string columnName, string columnValue);
        Task<ActionResult<IEnumerable<VacancyConditions>>> GetVacancyConditions(string columnValue = null);
        Task<ActionResult<VacancyConditions>> EditVacancyCondition(Guid id, string columnName, string columnValue);
        Task<ActionResult<VacancyPlaces>> EditVacancyPlace(Guid id, string columnName, string columnValue);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Task<ActionResult<Jobopenings>> AddVacancy(string vacancyPlaceName, string jobopeningName,
            string specializationCode, string accountName, string link);
        Task<ActionResult<IEnumerable<AllSkills>>> GetSkillsView(string columnValue = null);
        //Task<ActionResult<IEnumerable<SkillsJobopening>>> AddSkill(string jobopeningName, string skillName);
        Task<ActionResult<IEnumerable<VacancyConditions>>> AddVacancyCondition(string jobopeningName,
            string conditionType, string conditionValue);
        Task<ActionResult<IEnumerable<AllAccounts>>> GetContactsView(string columnValue = null);
        Task<ActionResult<IEnumerable<AllAccounts>>> AddContactAndCommunication(Guid accountId, string lastName, string firstName,
            bool gender, DateTime? birthDate, string middleName = null, string commType = null, string commValue = null);
    }
}
