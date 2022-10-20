using EmploymentHelper.ModelsLogic;
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
        private readonly AccountLogic _acLogic;
        private readonly CommunicationLogic _communicationLogic;
        private readonly ContactLogic _contactLogic;
        private readonly JobopeningLogic _jobopeningLogic;
        private readonly SkillLogic _skillLogic;
        private readonly SpecializationLogic _specializationLogic;
        private readonly VacancyConditionLogic _vacancyConditionLogic;
        private readonly VacancyPlaceLogic _vacancyPlaceLogic;
        private readonly ViewLogic _viewLogic;

        public EmploymentHelperControllers()
        {
            _acLogic = new AccountLogic();
            _communicationLogic = new CommunicationLogic();
            _contactLogic = new ContactLogic();
            _jobopeningLogic = new JobopeningLogic();
            _skillLogic = new SkillLogic();
            _specializationLogic = new SpecializationLogic();
            _vacancyConditionLogic = new VacancyConditionLogic();
            _vacancyPlaceLogic = new VacancyPlaceLogic();
            _viewLogic = new ViewLogic();
        }

        [HttpGet("Accounts/Get")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts([FromQuery] string columnValue = null)
        {
            try
            {
                return await _acLogic.GetAccounts(columnValue);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Accounts/Add")]
        public async Task<ActionResult<Account>> AddAccount([FromQuery] string name, [FromQuery] string inn = null)
        {
            try
            {
                return await _acLogic.AddAccount(name, inn);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Accounts/Update")]
        public async Task<ActionResult<Account>> EditAccount([FromQuery] string columnValue, [FromQuery] string columnName, 
            [FromQuery] string newValue)
        {
            try
            {
                return await _acLogic.EditAccount(columnValue, columnName, newValue);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }


        [HttpGet("Communications/Get")]
        public async Task<ActionResult<IEnumerable<Communication>>> GetCommunications(string columnValue = null)
        {
            try
            {
                return await _communicationLogic.GetCommunications(columnValue);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Communications/Add")]
        public async Task<ActionResult<Communication>> AddCommunication([FromQuery] string contactColumnValue, 
            [FromQuery] string commType, [FromQuery] string commValue)
        {
            try
            {
                return await _communicationLogic.AddCommunication(contactColumnValue, commType, commValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Communications/Update")]
        public async Task<ActionResult<Communication>> EditCommunication([FromQuery] string columnValue, 
            [FromQuery] string columnName, [FromQuery] string newValue)
        {
            try
            {
                return await _communicationLogic.EditCommunication(columnValue, columnName, newValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }


        [HttpGet("Contacts/Get")]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts([FromQuery] string columnValue = null)
        {
            try
            {
                return await _contactLogic.GetContacts(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Contacts/Add")]
        public async Task<ActionResult<Contact>> AddContact([FromQuery] string accountName, [FromQuery] string lastName, 
            [FromQuery] string firstName, [FromQuery] bool gender, [FromQuery] DateTime? birthDate, 
            [FromQuery] string middleName = null)
        {
            try
            {
                return await _contactLogic.AddContact(accountName, lastName, firstName, gender, birthDate, middleName);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Contacts/Update")]
        public async Task<ActionResult<Contact>> EditContact([FromQuery] Guid id, [FromQuery] string columnName, 
            [FromQuery] string columnValue)
        {
            try
            {
                return await _contactLogic.EditContact(id, columnName, columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }


        [HttpGet("Jobopenings/Get")]
        public async Task<ActionResult<IEnumerable<Jobopening>>> GetJobopenings([FromQuery] string columnValue = null)
        {
            try
            {
                return await _jobopeningLogic.GetJobopenings(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Jobopenings/Add")]
        public async Task<ActionResult<Jobopening>> AddJobopening([FromQuery] string specializationColumnValue, 
            [FromQuery] string vacancyPlaceColumnValue, [FromQuery] string name, [FromQuery] string link, 
            [FromQuery] string accountName)
        {
            try
            {
                return await _jobopeningLogic.AddJobopening(specializationColumnValue, vacancyPlaceColumnValue, name, link, accountName);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Jobopenings/Update")]
        public async Task<ActionResult<Jobopening>> EditJobopening([FromQuery] Guid id, [FromQuery] string columnName, 
            [FromQuery] string columnValue)
        {
            try
            {
                return await _jobopeningLogic.EditJobopening(id, columnName, columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }


        [HttpGet("Skills/Get")]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills([FromQuery] string columnValue = null)
        {
            try
            {
                return await _skillLogic.GetSkills(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Skills/Add")]
        public async Task<ActionResult<Skill>> AddSkill([FromQuery] string jobopeningColumnValue, [FromQuery] string name)
        {
            try
            {
                return await _skillLogic.AddSkill(jobopeningColumnValue, name);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Skills/Update")]
        public async Task<ActionResult<Skill>> EditSkill([FromQuery] Guid id, [FromQuery] string columnName, 
            [FromQuery] string columnValue)
        {
            try
            {
                return await _skillLogic.EditSkill(id, columnName, columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }


        [HttpGet("Specializations/Get")]
        public async Task<ActionResult<IEnumerable<Specialization>>> GetSpecializations([FromQuery] string columnValue = null)
        {
            try
            {
                return await _specializationLogic.GetSpecializations(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Specializations/Add")]
        public async Task<ActionResult<Specialization>> AddSpecialization([FromQuery] string name, [FromQuery] string code)
        {
            try
            {
                return await _specializationLogic.AddSpecialization(name, code);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("Specializations/Update")]
        public async Task<ActionResult<Specialization>> EditSpecialization([FromQuery] Guid id, [FromQuery] string columnName,
            [FromQuery] string columnValue)
        {
            try
            {
                return await _specializationLogic.EditSpecialization(id, columnName, columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }


        [HttpGet("VacancyConditions/Get")]
        public async Task<ActionResult<IEnumerable<VacancyCondition>>> GetVacancyConditions([FromQuery] string columnValue = null)
        {
            try
            {
                return await _vacancyConditionLogic.GetVacancyConditions(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("VacancyConditions/Add")]
        public async Task<ActionResult<VacancyCondition>> AddVacancyCondition([FromQuery] string jobopeningColumnValue,
            [FromQuery] string conditionType, [FromQuery] string conditionValue)
        {
            try
            {
                return await _vacancyConditionLogic.AddVacancyCondition(jobopeningColumnValue, conditionType, conditionValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("VacancyConditions/Update")]
        public async Task<ActionResult<VacancyCondition>> EditVacancyCondition([FromQuery] Guid id, [FromQuery] string columnName, 
            [FromQuery] string columnValue)
        {
            try
            {
                return await _vacancyConditionLogic.EditVacancyCondition(id, columnName, columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }


        [HttpGet("VacancyPlaces/Get")]
        public async Task<ActionResult<IEnumerable<VacancyPlace>>> GetVacancyPlaces([FromQuery] string columnValue = null)
        {
            try
            {
                return await _vacancyPlaceLogic.GetVacancyPlaces(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("VacancyPlaces/Add")]
        public async Task<ActionResult<VacancyPlace>> AddVacancyPlace([FromQuery] string name, [FromQuery] string code)
        {
            try
            {
                return await _vacancyPlaceLogic.AddVacancyPlace(name, code);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("VacancyPlaces/Update")]
        public async Task<ActionResult<VacancyPlace>> EditVacancyPlace([FromQuery] string columnValue, [FromQuery] string columnName, 
            [FromQuery] string newValue)
        {
            try
            {
                return await _vacancyPlaceLogic.EditVacancyPlace(columnValue, columnName, newValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("AllSkills/Get")]
        public async Task<ActionResult<IEnumerable<AllSkill>>> GetAllSkillsView([FromQuery] string columnValue = null)
        {
            try
            {
                return await _viewLogic.GetAllSkillsView(columnValue);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
