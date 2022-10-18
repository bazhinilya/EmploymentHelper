using EmploymentHelper.Models.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using EmploymentHelper.BLogic;
using System.Reflection;

namespace EmploymentHelper.ModelsLogic
{
    public class AccountLogic
    {
        private readonly PropertyInfo[] _accountsType;
        public AccountLogic() { _accountsType = typeof(Account).GetProperties(); }
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
                    db.Accounts.FirstOrDefault(a => a.Id == id) ?? throw new Exception("Error, invalid column value.")
                };
            }
            return db.Accounts.Where(a => a.Name.Contains(columnValue)).ToList(); 
        }
        public async Task<ActionResult<Account>> AddAccount(string name, string inn = null)
        {
            await using var db = new VacancyContext();
            var accountByName = db.Accounts.FirstOrDefault(a => a.Name.Contains(name));
            Account accountByInn = null;
            if (inn != null)
            {
                if (!InnerLogic.IsINN(inn)) throw new Exception("Error, invalid INN value.");
                accountByInn = db.Accounts.FirstOrDefault(a => a.INN == inn);
            }
            if (accountByName != null || accountByInn != null) throw new Exception($"Error. This account already exists.");
            Account accountToCreate = new() { Id = Guid.NewGuid(), Name = name, INN = inn };
            db.Accounts.Add(accountToCreate);
            await db.SaveChangesAsync();
            return accountToCreate;
        }
        public async Task<ActionResult<Account>> EditAccount(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var accountById = db.Accounts.FirstOrDefault(a => a.Id == id);
            if (accountById == null) throw new Exception("Error, account does not exist.");
            bool isInn = InnerLogic.IsINN(columnValue);
            Account accountByInn = null;
            Account accountByName = null;
            if (isInn)
            {
                accountByInn = db.Accounts.FirstOrDefault(a => a.INN == columnValue);
            }
            if (!isInn)
            {
                accountByName = db.Accounts.FirstOrDefault(a => a.Name.Contains(columnValue));
            }
            if (accountByInn == null && accountByName == null) throw new Exception("Error, there are several entities.");
            bool isDirty = true;
            foreach (var item in _accountsType)
            {
                if (item.Name == columnName)
                {
                    item.SetValue(accountById, columnValue);
                    isDirty = false;
                }
            }
            if (isDirty) throw new Exception("Error, the column name is incorrect");
            await db.SaveChangesAsync();
            return db.Accounts.First(a => a.Id == id);
        }
    }
}
