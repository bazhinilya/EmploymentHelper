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


        [HttpGet("Accounts")]
        public async Task<ActionResult<IEnumerable<Accounts>>> Get()
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

        [HttpGet("Accounts/{name}")]
        public async Task<ActionResult<IEnumerable<Accounts>>> Get(string name)
        {
            try
            {
                return await _eHLogic.GetAccountByName(name);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpGet("Jobopenings")]
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

        [HttpGet("SkillsForJobopening/{jobopening}")]
        public async Task<ActionResult<IEnumerable<Skills>>> GetSkillsForJobopening(string jobopening)
        {

            try
            {
                return await _eHLogic.GetSkillsForJobopening(jobopening);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("AddVacancy")]
        public async Task<ActionResult<bool>> AddVacancy([FromQuery] string vacancyPlaceName, [FromQuery] string code, 
            [FromQuery] string jobopeningName, [FromQuery] string specializationName, 
            [FromQuery] byte workExperienceInYears, [FromQuery] string accountName, [FromQuery] string linkName)
        {
            try
            {
                return await _eHLogic
                    .AddVacancy(vacancyPlaceName, code, jobopeningName, 
                    specializationName, workExperienceInYears, accountName, linkName);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
