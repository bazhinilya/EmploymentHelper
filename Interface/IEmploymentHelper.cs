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
        Task<ActionResult<Accounts>> AddAccount(string name, string inn = null);
        Task<ActionResult<Accounts>> EditAccount(Guid id, string columnName, string columnValue);
        Task<ActionResult<IEnumerable<Accounts>>> DeleteAccounts(Guid? id);

        Task<ActionResult<IEnumerable<Communications>>> GetCommunications(string columnValue = null);
        Task<ActionResult<Communications>> AddCommunication(string accountColumnValue, string commType, string commValue,
            string contactColumnValue);
        Task<ActionResult<Communications>> EditCommunication(Guid id, string columnName, string columnValue);
        Task<ActionResult<IEnumerable<Communications>>> DeleteCommunications(Guid? id);

        Task<ActionResult<IEnumerable<Contacts>>> GetContacts(string columnValue = null);
        Task<ActionResult<Contacts>> AddContact(string accountName, string lastName, string firstName, bool gender, DateTime? birthDate,
            string middleName = null);
        Task<ActionResult<Contacts>> EditContact(Guid id, string columnName, string columnValue);
        Task<ActionResult<IEnumerable<Contacts>>> DeleteContacts(Guid? id);

        Task<ActionResult<IEnumerable<Jobopenings>>> GetJobopenings(string columnValue = null);
        Task<ActionResult<Jobopenings>> AddJobopening(string specializationColumnValue, string vacancyPlaceColumnValue, string name, 
            string link, string accountName);
        Task<ActionResult<Jobopenings>> EditJobopening(Guid id, string columnName, string columnValue);
        Task<ActionResult<IEnumerable<Jobopenings>>> DeleteJobopenings(Guid? id);

        Task<ActionResult<IEnumerable<Skills>>> GetSkills(string columnValue = null);
        Task<ActionResult<Skills>> AddSkill(string jobopeningColumnValue, string name);
        Task<ActionResult<Skills>> EditSkill(Guid id, string columnName, string columnValue);
        Task<ActionResult<IEnumerable<Skills>>> DeleteSkills(Guid? id);

        Task<ActionResult<IEnumerable<Specializations>>> GetSpecializations(string columnValue = null);
        Task<ActionResult<Specializations>> AddSpecialization(string name, string code);
        Task<ActionResult<Specializations>> EditSpecialization(Guid id, string columnName, string columnValue);
        Task<ActionResult<IEnumerable<Specializations>>> DeleteSpecializations(Guid? id);

        Task<ActionResult<IEnumerable<VacancyConditions>>> GetVacancyConditions(string columnValue = null);
        Task<ActionResult<VacancyConditions>> AddVacancyCondition(string jobopeningColumnValue, string conditionType, 
            string conditionValue);
        Task<ActionResult<VacancyConditions>> EditVacancyCondition(Guid id, string columnName, string columnValue);
        Task<ActionResult<IEnumerable<VacancyConditions>>> DeleteVacancyConditions(Guid? id);

        Task<ActionResult<IEnumerable<VacancyPlaces>>> GetVacancyPlaces(string columnValue = null);
        Task<ActionResult<VacancyPlaces>> AddVacancyPlace(string name, string code);
        Task<ActionResult<VacancyPlaces>> EditVacancyPlace(Guid id, string columnName, string columnValue);
        Task<ActionResult<IEnumerable<VacancyPlaces>>> DeleteVacancyPlaces(Guid? id);
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Task<ActionResult<IEnumerable<AllSkills>>> GetAllSkillsView(string columnValue = null);
    }
}
