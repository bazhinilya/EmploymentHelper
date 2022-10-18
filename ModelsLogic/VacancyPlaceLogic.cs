using EmploymentHelper.Models.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace EmploymentHelper.ModelsLogic
{
    public class VacancyPlaceLogic
    {
        private readonly Type _vacancyPlacesType = typeof(VacancyPlace);
        public async Task<ActionResult<IEnumerable<VacancyPlace>>> GetVacancyPlaces(string columnValue = null)
        {
            await using var db = new VacancyContext();
            bool isId = Guid.TryParse(columnValue, out Guid id);
            var vacancyPlaces = db.VacancyPlaces.Where(p => p.Name == columnValue || p.Code == columnValue || p.Id == id);
            if (isId && columnValue != null)
            {
                return db.VacancyPlaces.Where(p => p.Id == id).ToList();
            }
            else if (vacancyPlaces != null && columnValue != null && vacancyPlaces.Any())
            {
                return vacancyPlaces.ToList();
            }
            else if (columnValue != null)
            {
                throw new Exception("Error, invalid column value.");
            }
            return db.VacancyPlaces.ToList();
        }
        public async Task<ActionResult<VacancyPlace>> AddVacancyPlace(string name, string code)
        {
            await using var db = new VacancyContext();
            var vacancyPlaces = db.VacancyPlaces.Where(vc => vc.Name == name || vc.Code == code);
            if (vacancyPlaces.Count() == 0)
            {
                db.VacancyPlaces.Add(new VacancyPlace { Id = Guid.NewGuid(), Name = name, Code = code });
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error. This vacancy place already exists.");
            }
            return vacancyPlaces.First();
        }
        public async Task<ActionResult<VacancyPlace>> EditVacancyPlace(Guid id, string columnName, string columnValue)
        {
            await using var db = new VacancyContext();
            var vacancyPlaces = db.VacancyPlaces.Where(vp => vp.Id == id);
            if (vacancyPlaces.Count() == 1)
            {
                int isDirty = 0;
                foreach (var item in _vacancyPlacesType.GetProperties())
                {
                    if (item.Name == columnName)
                    {
                        item.SetValue(vacancyPlaces.First(), columnValue);
                    }
                }
                if (isDirty == 0)
                {
                    throw new Exception("Error, the column name is incorrect");
                }
                else if (db.VacancyPlaces.Where(vp => vp.Id == vacancyPlaces.First().Id
                                                && vp.Name == vacancyPlaces.First().Name)
                                         .Count() != 1)
                {
                    throw new Exception("Error, you are trying to specify already existing data.");
                }
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Uniqueness error, there are several entities.");
            }
            return vacancyPlaces.First();
        }
    }
}
