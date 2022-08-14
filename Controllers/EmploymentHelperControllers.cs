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

        [HttpGet("Specializations/Get")]
        public async Task<ActionResult<IEnumerable<Specializations>>> GetSpecializations([FromQuery] string columnValue = null)
        {
            try
            {
                return await _eHLogic.GetSpecializations(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Specializations/Add")]
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

        [HttpPost("Specializations/Update")]
        public async Task<ActionResult<Specializations>> UpdateSpecialization([FromQuery] Guid id, [FromQuery] string columnName, 
            [FromQuery] string columnValue)
        {
            try
            {
                return await _eHLogic.EditSpecialization(id, columnName, columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpGet("Jobopenings/Get")]
        public async Task<ActionResult<IEnumerable<Jobopenings>>> GetJobopenings([FromQuery] string columnValue = null)
        {
            try
            {
                return await _eHLogic.GetJobopenings(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Jobopenings/Add")]
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

        [HttpDelete("Jobopenings/Delete")]
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

        [HttpGet("Accounts/Get")]
        public async Task<ActionResult<IEnumerable<Accounts>>> GetAccounts([FromQuery] string columnValue = null)
        {
            try
            {
                return await _eHLogic.GetAccounts(columnValue);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpGet("AllSkills/Get")]
        public async Task<ActionResult<IEnumerable<AllSkills>>> GetSkillsJobopening([FromQuery] string columnValue = null)
        {
            try
            {
                return await _eHLogic.GetSkillsView(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("SkillsJobopening/Add")]
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

        [HttpGet("VacancyConditions/Get")]
        public async Task<ActionResult<IEnumerable<VacancyConditions>>> GetConditions([FromQuery] string columnValue = null)
        {
            try
            {
                return await _eHLogic.GetAllConditions(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("VacancyConditions/Add")]
        public async Task<ActionResult<IEnumerable<VacancyConditions>>> AddConditionForVacancy([FromQuery] string jobopeningName,
            [FromQuery] string conditionType, [FromQuery] string conditionValue)
        {
            try
            {
                return await _eHLogic.AddVacancyCondition(jobopeningName, conditionType, conditionValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("VacancyConditions/Update")]
        public async Task<ActionResult<VacancyConditions>> UpdateVacancyCondition(Guid id, string columnName, string columnValue)
        {
            try
            {
                return await _eHLogic.EditVacancyCondition(id, columnName, columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpDelete("VacancyConditions/Delete")]
        public async Task<ActionResult<IEnumerable<VacancyConditions>>> DeleteVacancyConditions([FromQuery] Guid? id, [FromQuery] Guid? jobopeningId)
        {
            try
            {
                return await _eHLogic.DeleteVacancyConditions(id, jobopeningId);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpGet("AllAccounts/Get")]
        public async Task<ActionResult<IEnumerable<AllAccounts>>> GetContactsAccount([FromQuery] string columnValue = null)
        {
            try
            {
                return await _eHLogic.GetContactsView(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("AllAccounts/Add")]
        public async Task<ActionResult<IEnumerable<AllAccounts>>> AddContact([FromQuery] Guid accountId, [FromQuery] string lastName,
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

        [HttpPost("AllAccounts/Update")]
        public async Task<ActionResult<AllAccounts>> UpdateContact(Guid id, string columnName, string columnValue)
        {
            try
            {
                return await _eHLogic.EditAllAccounts(id, columnName, columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Communications/Add")]
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

        [HttpPost("Communications/Update")]
        public async Task<ActionResult<Communications>> UpdateCommunication([FromQuery] Guid id, [FromQuery] string columnName, 
            [FromQuery] string columnValue)
        {
            try
            {
                return await _eHLogic.EditCommunication(id, columnName, columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpDelete("Communications/Delete")]
        public async Task<ActionResult<IEnumerable<Communications>>> DeleteCommunications(Guid? id)
        {
            try
            {
                return await _eHLogic.DeleteCommunications(id);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }
        
    }
}
