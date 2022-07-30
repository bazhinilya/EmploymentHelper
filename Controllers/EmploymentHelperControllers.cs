using EmploymentHelper.BL;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        public EmploymentHelperControllers(IConfiguration configuration)
        {
            _eHLogic = new EHLogic(configuration);
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

        [HttpGet("Jobopenings/GetAllJobopenings")]
        public async Task<ActionResult<IEnumerable<Jobopenings>>> GetAllJobopenings()
        {
            try
            {
                return await _eHLogic.GetJobopenings();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpGet("AllSkills/GetAllJobSkills")]
        public async Task<ActionResult<IEnumerable<AllSkills>>> GetAllJobSkills([FromQuery] string jobopening)
        {

            try
            {
                return await _eHLogic.GetAllSkillsView(jobopening);
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
    }
}
