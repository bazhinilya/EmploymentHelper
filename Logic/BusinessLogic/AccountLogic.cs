﻿using EmploymentHelper.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmploymentHelper.Logic.BusinessLogic
{
    public class AccountLogic
    {
        private readonly PropertyInfo[] _accountsProperties;
        public AccountLogic() { _accountsProperties = typeof(Account).GetProperties(); }
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts(string columnValue = null)
        {
            await using var db = new VacancyContext();
            if (columnValue == null)
            {
                return db.Accounts.ToList();
            }
            if (Guid.TryParse(columnValue, out Guid id))
            {
                return new List<Account>
                {
                    db.Accounts.FirstOrDefault(a => a.Id == id) ?? throw new Exception("Invalid column value.")
                };
            }
            return db.Accounts.Where(a => a.Name.Contains(columnValue)).ToList() ?? throw new Exception("Invalid column value.");
        }
        public async Task<ActionResult<Account>> AddAccount(string name, string inn = null)
        {
            await using var db = new VacancyContext();
            Account accountByName = db.Accounts.FirstOrDefault(a => a.Name == name);
            if (accountByName != null) throw new Exception($"This account already exist.");
            if (inn != null)
            {
                if (!CommonLogic.IsINN(inn)) throw new Exception("Invalid INN value.");
                Account accountByInn = db.Accounts.FirstOrDefault(a => a.INN == inn);
                if (accountByInn != null) throw new Exception($"This account already exist.");
            }
            Account accountToCreate = new() { Id = Guid.NewGuid(), Name = name, INN = inn };
            db.Accounts.Add(accountToCreate);
            await db.SaveChangesAsync();
            return accountToCreate;
        }
        public async Task<ActionResult<Account>> EditAccount(string columnValue, string columnName, string newValue)
        {
            await using var db = new VacancyContext();
            Account accountToCheck = db.Accounts.FirstOrDefault(a => a.INN == newValue || a.Name == columnName);
            if (accountToCheck != null) throw new Exception("This account already exsist.");
            Account accountToChange = CommonLogic.GetAccount(columnValue, db);
            bool isDirty = true;
            foreach (var item in _accountsProperties)
            {
                if (item.Name == columnName)
                {
                    item.SetValue(accountToChange, newValue);
                    isDirty = false;
                }
            }
            if (isDirty) throw new Exception("The column name is incorrect");
            await db.SaveChangesAsync();
            return accountToChange;
        }
    }
}
