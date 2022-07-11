﻿using EmploymentHelper.BL;
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

        //[HttpGet("Contacts/{lastName}")]
        //public async Task<ActionResult<IEnumerable<Contact>>> GetContact(string lastName)
        //{
        //    try
        //    {
        //        return await _eHLogic.GetContactByLastName(lastName);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new BadRequestObjectResult($"Inner error. {ex.Message}\n{ex.StackTrace}");
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult<bool>> Post(int id, DateTime birthDate)
        //{
        //    try
        //    {
        //        return await _eHLogic.UpdateContactBirthDate(id, birthDate);
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //[HttpPost("AddContactsAccounts/{contactId}")]
        //public async Task<ActionResult<bool>> AddContactAccountRelation(int contactId, [FromQuery] int accountId)
        //{
        //    try
        //    {
        //        return await _eHLogic.AddContactAccount(contactId, accountId);
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
    }
}
