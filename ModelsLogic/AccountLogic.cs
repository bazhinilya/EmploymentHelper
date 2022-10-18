using EmploymentHelper.Models.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using EmploymentHelper.BLogic;

namespace EmploymentHelper.ModelsLogic
{
    public class AccountLogic
    {
        private readonly Type _accountsType = typeof(Account);
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts(string columnValue = null)
        {
            await using var db = new VacancyContext();
            bool isId = Guid.TryParse(columnValue, out Guid id);
            var accounts = db.Accounts.Where(a => a.Name.Contains(columnValue) || a.Id == id);
            if (isId && columnValue != null)
            {
                return db.Accounts.Where(a => a.Id == id).ToList();
            }
            else if (accounts != null && columnValue != null && accounts.Any())
            {
                return accounts.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");//перенести Exception
            }
            return db.Accounts.ToList();
        }
        public async Task<ActionResult<Account>> AddAccount(string name, string inn = null)
        {
            await using var db = new VacancyContext();
            var accounts = db.Accounts.FirstOrDefault(a => a.Name.Contains(name));
            Account accountToNull = null;
            if (inn != null && InnerLogic.IsINN(inn))
            {
                accountToNull = db.Accounts.FirstOrDefault(a => a.INN.Contains(inn));
            }
            else if (inn != null)
            {
                throw new Exception("Error, invalid INN value. Check data.");
            }
            Account account = null;
            if (accounts != null || (inn != null && accountToNull != null))
            {
                throw new Exception($"Uniqueness error. This account already exists. Check data.");
            }
            else
            {
                account = new Account { Id = Guid.NewGuid(), Name = name, INN = inn };
                db.Accounts.Add(account);
                await db.SaveChangesAsync();
                return account;
            }
        }
        public async Task<ActionResult<Account>> EditAccount(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var accounts = db.Accounts.Where(a => a.Id == id);
            var accountToNull = db.Accounts.FirstOrDefault(a => (a.INN == columnValue && InnerLogic.IsINN(columnValue)) || a.Name == columnValue);
            if (accounts.Count() == 1 && accountToNull == null)
            {
                bool isDirty = true;
                foreach (var item in _accountsType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(accounts.First(), columnValue);
                        isDirty = false;
                    }
                }
                if (isDirty)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                await db.SaveChangesAsync();
                return db.Accounts.First(a => a.Id == id);
            }
            else
            {
                throw new Exception("Uniqueness error, there are several entities.");
            }
        }
    }
}
