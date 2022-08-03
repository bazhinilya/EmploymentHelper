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

        [HttpGet("Accounts/GetAllAccounts")]
        public async Task<ActionResult<IEnumerable<Accounts>>> GetAllAccounts()
        {
            try
            {
                return await _eHLogic.GetAccounts();
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
        public async Task<ActionResult<Jobopenings>> AddVacancy([FromQuery] string vacancyPlace, [FromQuery] string code, 
            [FromQuery] string jobopeningName, [FromQuery] string specializationCode, 
            [FromQuery] string accountName, [FromQuery] string linkName)
        {
            try
            {
                return await _eHLogic
                    .AddVacancy(vacancyPlace, code, jobopeningName,
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

        [HttpPost("Contacts/AddContact")]
        public async Task<ActionResult<IEnumerable<Contacts>>> AddContact(Guid accountId, string lastName, string firstName,
            string middleName, DateTime birthDate, bool gender)
        {
            try
            {
                return await _eHLogic.AddContact(accountId, lastName, firstName,
            middleName, birthDate, gender);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        //[HttpPost("Communications/AddCommunication")]
        //public async Task<ActionResult<IEnumerable<Communications>>> AddCommunication([FromQuery] string commType, [FromQuery] string commValue)
        //{
        //    try
        //    {
        //        return await _eHLogic.AddCommunication(commType, commValue);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
        //    }
        //}
    }
}
