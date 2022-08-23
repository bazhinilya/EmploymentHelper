using EmploymentHelper.BL;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        [HttpDelete("Communications/Delete")]
        public async Task<ActionResult<IEnumerable<Communications>>> DeleteCommunications([FromQuery] Guid? id)
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
        public async Task<ActionResult<VacancyConditions>> AddVacancyCondition([FromQuery] string jobopeningName,
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

        [HttpDelete("VacancyConditions/Delete")]
        public async Task<ActionResult<IEnumerable<VacancyConditions>>> DeleteVacancyConditions([FromQuery] Guid? id)
        {
            try
            {
                return await _eHLogic.DeleteVacancyConditions(id);
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

        [HttpDelete("VacancyPlaces/Delete")]
        public async Task<ActionResult<IEnumerable<VacancyPlaces>>> DeleteVacancyPlaces([FromQuery] Guid? id)
        {
            try
            {
                return await _eHLogic.DeleteVacancyPlaces(id);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
    }
}
