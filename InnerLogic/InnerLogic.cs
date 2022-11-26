using EmploymentHelper.Context;
using EmploymentHelper.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace EmploymentHelper.BLogic
{
    internal class InnerLogic
    {
        public static bool IsINN(string inn) => Regex.IsMatch(inn, @"^(\d{10}|\d{12})$");
        public static bool IsWord(string word) => Regex.IsMatch(word, @"\s*[a-zA-Zа-яА-Я]+\s*");
        public static bool IsLink(string value) => value.StartsWith("https", StringComparison.OrdinalIgnoreCase);

        public static Account GetAccount(string columnValue, VacancyContext db)
        {
            bool isId = Guid.TryParse(columnValue, out Guid id);
            if (isId)
            {
                return db.Accounts.FirstOrDefault(a => a.Id == id);
            }
            bool isInn = IsINN(columnValue);
            if (isInn)
            {
                return db.Accounts.FirstOrDefault(a => a.INN == columnValue);
            }
            if (!isInn && !isId)
            {
                return db.Accounts.FirstOrDefault(a => a.Name == columnValue);
            }
            throw new Exception("This account does not exist.");
        }

        public static Contact GetContact(string columnValue, VacancyContext db)
        {
            bool isBirthDate = DateTime.TryParse(columnValue, out DateTime birthDate);
            if (isBirthDate)
            {
                return db.Contacts.FirstOrDefault(c => c.BirthDate == birthDate);
            }
            bool isId = Guid.TryParse(columnValue, out Guid id);
            if (isId)
            {
                return db.Contacts.FirstOrDefault(c => c.Id == id);
            }
            if (!isId && !isBirthDate)
            {
                return db.Contacts.FirstOrDefault(c => c.FullName == columnValue);
            }
            throw new Exception("Contact does not exist.");
        }

        public static Jobopening GetJobopening(string columnValue, VacancyContext db)
        {
            bool isId = Guid.TryParse(columnValue, out Guid id);
            if (isId)
            {
                return db.Jobopenings.FirstOrDefault(j => j.Id == id);
            }
            bool isLink = IsLink(columnValue);
            if (isLink)
            {
                return db.Jobopenings.FirstOrDefault(j => j.Link == columnValue);
            }
            if (!isId && !isLink)
            {
                return db.Jobopenings.FirstOrDefault(j => j.Name == columnValue);
            }
            throw new Exception("Jobopening does not exist.");
        }

        public static Specialization GetSpecialization(string columnValue, VacancyContext db)
        {
            bool isId = Guid.TryParse(columnValue, out Guid id);
            if (isId)
            {
                return db.Specializations.FirstOrDefault(s => s.Id == id);
            }
            if (columnValue.Length <= 4)
            {
                return db.Specializations.FirstOrDefault(s => s.Code == columnValue || s.Name == columnValue);
            }
            if (columnValue.Length > 4)
            {
                return db.Specializations.FirstOrDefault(s => s.Name == columnValue || s.Code == columnValue);
            }
            throw new Exception("Specialization does not exist.");
        }

        public static VacancyPlace GetVacancyPlace(string columnValue, VacancyContext db)
        {
            bool isId = Guid.TryParse(columnValue, out Guid id);
            if (isId)
            {
                return db.VacancyPlaces.FirstOrDefault(vp => vp.Id == id);
            }
            if (columnValue.Length <= 4)
            {
                return db.VacancyPlaces.FirstOrDefault(vp => vp.Code == columnValue || vp.Name == columnValue);
            }
            if (columnValue.Length > 4)
            {
                return db.VacancyPlaces.FirstOrDefault(vp => vp.Name == columnValue || vp.Code == columnValue);
            }
            throw new Exception("Vacancy places does not exist.");
        }
    }
}