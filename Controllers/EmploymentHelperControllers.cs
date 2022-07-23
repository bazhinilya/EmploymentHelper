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

        [HttpGet("Accounts/{account}")]
        public async Task<ActionResult<IEnumerable<Accounts>>> GetAccountByName(string name)
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

        [HttpPost("Accounts")]
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

        [HttpGet("Jobopenings")]
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

        [HttpGet("SkillsForJobopening/{jobopening}")]
        public async Task<ActionResult<IEnumerable<AllSkills>>> GetAllJobSkills(string jobopening)
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

        [HttpPost("AddVacancy")]
        public async Task<ActionResult<bool>> AddVacancy([FromQuery] string vacancyPlace, [FromQuery] string code, 
            [FromQuery] string jobopeningName, [FromQuery] string specializationCode, 
            [FromQuery] string accountName, [FromQuery] string linkName)
        {
            try
            {
                return await _eHLogic
                    .AddVacancy(vacancyPlace, code, jobopeningName,
                    specializationCode, accountName, linkName);
            }
            catch (Exception)
            {
                return false;
            }
        }

        //[HttpPost("")]
        //public async Task<ActionResult<bool>> Add
    }
}
