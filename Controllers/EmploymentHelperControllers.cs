using EmploymentHelper.BL;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmploymentHelper
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmploymentHelperControllers : ControllerBase
    {
        private readonly EHLogic _eHLogic;

        public EmploymentHelperControllers()
        {
            _eHLogic = new EHLogic();
        }

        [HttpGet("Accounts/GetAccounts")]
        public async Task<ActionResult<IEnumerable<Accounts>>> GetAccounts()
        {
            try
            {
                return await _eHLogic.GetAllAccounts();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpGet("Accounts/GetAccountByName")]
        public async Task<ActionResult<IEnumerable<Accounts>>> GetAccountByName([FromQuery] string name)
        {
            try
            {
                return await _eHLogic.GetAccount(name);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Accounts/AddINNByName")]
        public async Task<ActionResult<Accounts>> AddINNByName([FromQuery] string name, [FromQuery] string inn)
        {
            try
            {
                return await _eHLogic.AddInn(name, inn);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpGet("AllSkills/GetAllJobSkills")]
        public async Task<ActionResult<IEnumerable<AllSkills>>> GetAllJobSkills()
        {

            try
            {
                return await _eHLogic.GetAllSkillsView();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpGet("Specializations/GetSpecializations")]
        public async Task<ActionResult<IEnumerable<Specializations>>> GetSpecializations()
        {
            try
            {
                return await _eHLogic.GetAllSpecializations();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Specializations/AddSpecialization")]
        public async Task<ActionResult<Specializations>> AddSpecialization([FromQuery] string name, [FromQuery] string code)
        {
            try
            {
                return await _eHLogic.AddSpecialization(name, code);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpGet("Jobopenings/GetJobopenings")]
        public async Task<ActionResult<IEnumerable<Jobopenings>>> GetJobopenings()
        {
            try
            {
                return await _eHLogic.GetAllJobopenings();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Jobopenings/AddVacancy")]
        public async Task<ActionResult<Jobopenings>> AddVacancy([FromQuery] string vacancyPlace, 
            [FromQuery] string jobopeningName, [FromQuery] string specializationCode, 
            [FromQuery] string accountName, [FromQuery] string linkName)
        {
            try
            {
                return await _eHLogic
                    .AddVacancy(vacancyPlace, jobopeningName,
                    specializationCode, accountName, linkName);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpDelete("Jobopenings/DeleteVacancy")]
        public async Task<ActionResult<IEnumerable<Jobopenings>>> DeleteVacancy([FromQuery] Guid idFromJobopenings)
        {
            try
            {
                return await _eHLogic.DeleteVacancy(idFromJobopenings);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Skills/AddSkill")]
        public async Task<ActionResult<IEnumerable<SkillsJobopening>>> AddSkill([FromQuery] string jobopeningName, [FromQuery] string skillName)
        {
            try
            {
                return await _eHLogic.AddSkill(jobopeningName, skillName);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("VacancyConditions/AddConditionForVacancy")]
        public async Task<ActionResult<IEnumerable<VacancyConditions>>> AddConditionForVacancy([FromQuery] string jobopeningName, 
            [FromQuery] string conditionValue, [FromQuery] string conditionType)
        {
            try
            {
                return await _eHLogic.AddVacancyCondition(jobopeningName, conditionValue, conditionType);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpDelete("VacancyConditions/DeleteVacancyCondition")]
        public async Task<ActionResult<IEnumerable<VacancyConditions>>> DeleteVacancyCondition([FromQuery] Guid idFromVacancyConditions)
        {
            try
            {
                return await _eHLogic.DeleteCondition(idFromVacancyConditions);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpDelete("VacancyConditions/DeleteVacancyConditions")]
        public async Task<ActionResult<Jobopenings>> DeleteVacancyConditions([FromQuery] Guid jobopeningId)
        {
            try
            {
                return await _eHLogic.DeleteConditions(jobopeningId);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Contacts/AddContact")]//add view
        public async Task<ActionResult<Contacts>> AddContact([FromQuery] Guid accountId, [FromQuery] string lastName, 
            [FromQuery] string firstName, [FromQuery] bool gender, DateTime? birthDate, string middleName = null, 
            string commType = null, string commValue = null)
        {
            try
            {
                return await _eHLogic.AddContactAndCommunication(accountId, lastName, firstName,
            gender, birthDate, middleName, commType, commValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Communications/AddCommunication")]
        public async Task<ActionResult<Communications>> AddCommunication([FromQuery] Guid contactId, [FromQuery] string commType, 
            [FromQuery] string commValue)
        {
            try
            {
                return await _eHLogic.AddCommunication(contactId, commType, commValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
