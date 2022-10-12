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

        [HttpPost("Accounts/Add")]
        public async Task<ActionResult<Accounts>> AddAccount([FromQuery] string name, [FromQuery] string inn = null)
        {
            try
            {
                return await _eHLogic.AddAccount(name, inn);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Accounts/Update")]
        public async Task<ActionResult<Accounts>> EditAccount([FromQuery] Guid id, [FromQuery] string columnName, 
            [FromQuery] string columnValue)
        {
            try
            {
                return await _eHLogic.EditAccount(id, columnName, columnValue);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }


        [HttpGet("Communications/Get")]
        public async Task<ActionResult<IEnumerable<Communications>>> GetCommunications(string columnValue = null)
        {
            try
            {
                return await _eHLogic.GetCommunications(columnValue);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Communications/Add")]
        public async Task<ActionResult<Communications>> AddCommunication([FromQuery] string accountColumnValue, 
            [FromQuery] string commType, [FromQuery] string commValue, [FromQuery] string contactColumnValue)
        {
            try
            {
                return await _eHLogic.AddCommunication(accountColumnValue, commType, commValue, contactColumnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Communications/Update")]
        public async Task<ActionResult<Communications>> EditCommunication([FromQuery] Guid id, [FromQuery] string columnName,
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



        [HttpGet("Contacts/Get")]
        public async Task<ActionResult<IEnumerable<Contacts>>> GetContacts([FromQuery] string columnValue = null)
        {
            try
            {
                return await _eHLogic.GetContacts(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Contacts/Add")]
        public async Task<ActionResult<Contacts>> AddContact([FromQuery] string accountName, [FromQuery] string lastName, 
            [FromQuery] string firstName, [FromQuery] bool gender, [FromQuery] DateTime? birthDate, 
            [FromQuery] string middleName = null)
        {
            try
            {
                return await _eHLogic.AddContact(accountName, lastName, firstName, gender, birthDate, middleName);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Contacts/Update")]
        public async Task<ActionResult<Contacts>> EditContact([FromQuery] Guid id, [FromQuery] string columnName, 
            [FromQuery] string columnValue)
        {
            try
            {
                return await _eHLogic.EditContact(id, columnName, columnValue);
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
        public async Task<ActionResult<Jobopenings>> AddJobopening([FromQuery] string specializationColumnValue, 
            [FromQuery] string vacancyPlaceColumnValue, [FromQuery] string name, [FromQuery] string link, 
            [FromQuery] string accountName)
        {
            try
            {
                return await _eHLogic.AddJobopening(specializationColumnValue, vacancyPlaceColumnValue, name, link, accountName);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Jobopenings/Update")]
        public async Task<ActionResult<Jobopenings>> EditJobopening([FromQuery] Guid id, [FromQuery] string columnName, 
            [FromQuery] string columnValue)
        {
            try
            {
                return await _eHLogic.EditJobopening(id, columnName, columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }


        [HttpGet("Skills/Get")]
        public async Task<ActionResult<IEnumerable<Skills>>> GetSkills([FromQuery] string columnValue = null)
        {
            try
            {
                return await _eHLogic.GetSkills(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Skills/Add")]
        public async Task<ActionResult<Skills>> AddSkill([FromQuery] string jobopeningColumnValue, [FromQuery] string name)
        {
            try
            {
                return await _eHLogic.AddSkill(jobopeningColumnValue, name);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Skills/Update")]
        public async Task<ActionResult<Skills>> EditSkill([FromQuery] Guid id, [FromQuery] string columnName, 
            [FromQuery] string columnValue)
        {
            try
            {
                return await _eHLogic.EditSkill(id, columnName, columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
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
        public async Task<ActionResult<Specializations>> EditSpecialization([FromQuery] Guid id, [FromQuery] string columnName,
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


        [HttpGet("VacancyConditions/Get")]
        public async Task<ActionResult<IEnumerable<VacancyConditions>>> GetVacancyConditions([FromQuery] string columnValue = null)
        {
            try
            {
                return await _eHLogic.GetVacancyConditions(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("VacancyConditions/Add")]
        public async Task<ActionResult<VacancyConditions>> AddVacancyCondition([FromQuery] string jobopeningColumnValue,
            [FromQuery] string conditionType, [FromQuery] string conditionValue)
        {
            try
            {
                return await _eHLogic.AddVacancyCondition(jobopeningColumnValue, conditionType, conditionValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("VacancyConditions/Update")]
        public async Task<ActionResult<VacancyConditions>> EditVacancyCondition([FromQuery] Guid id, [FromQuery] string columnName, 
            [FromQuery] string columnValue)
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


        [HttpGet("VacancyPlaces/Get")]
        public async Task<ActionResult<IEnumerable<VacancyPlaces>>> GetVacancyPlaces([FromQuery] string columnValue = null)
        {
            try
            {
                return await _eHLogic.GetVacancyPlaces(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("VacancyPlaces/Add")]
        public async Task<ActionResult<VacancyPlaces>> AddVacancyPlace([FromQuery] string name, [FromQuery] string code)
        {
            try
            {
                return await _eHLogic.AddVacancyPlace(name, code);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("VacancyPlaces/Update")]
        public async Task<ActionResult<VacancyPlaces>> EditVacancyPlace([FromQuery] Guid id, [FromQuery] string columnName, 
            [FromQuery] string columnValue)
        {
            try
            {
                return await _eHLogic.EditVacancyPlace(id, columnName, columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("AllSkills/Get")]
        public async Task<ActionResult<IEnumerable<AllSkills>>> GetAllSkillsView([FromQuery] string columnValue = null)
        {
            try
            {
                return await _eHLogic.GetAllSkillsView(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
