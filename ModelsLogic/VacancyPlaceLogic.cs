using EmploymentHelper.BLogic;
using EmploymentHelper.Context;
using EmploymentHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmploymentHelper.ModelsLogic
{
    public class VacancyPlaceLogic
    {
        private readonly PropertyInfo[] _vacancyPlacesProperties; 
        public VacancyPlaceLogic() { _vacancyPlacesProperties = typeof(Account).GetProperties(); }
        public async Task<ActionResult<IEnumerable<VacancyPlace>>> GetVacancyPlaces(string columnValue = null)
        {
            await using var db = new VacancyContext();
            if (columnValue == null)
            {
                return db.VacancyPlaces.ToList();
            }
            if (Guid.TryParse(columnValue, out Guid id))
            {
                return new List<VacancyPlace>
                {
                    db.VacancyPlaces.FirstOrDefault(vp => vp.Id == id) ?? throw new Exception("Invalid column value.")
                };
            }
            return db.VacancyPlaces.Where(vp => vp.Code.Contains(columnValue) || vp.Name.Contains(columnValue))
                                   .ToList() ?? throw new Exception("Invalid column value.");
        }
        public async Task<ActionResult<VacancyPlace>> AddVacancyPlace(string name, string code)
        {
            await using var db = new VacancyContext();
            VacancyPlace vacancyPlaceToCheck = db.VacancyPlaces.FirstOrDefault(vp => vp.Code == code || vp.Name == name);
            if (vacancyPlaceToCheck != null) throw new Exception("This vacancy place already exist.");
            VacancyPlace vacancyPlaceToCreate = new() { Id = Guid.NewGuid(), Name = name, Code = code };
            db.VacancyPlaces.Add(vacancyPlaceToCreate);
            await db.SaveChangesAsync();
            return vacancyPlaceToCreate;
        }
        public async Task<ActionResult<VacancyPlace>> EditVacancyPlace(string columnValue, string columnName, string newValue)
        {
            await using var db = new VacancyContext();
            VacancyPlace vacancyPlaceToCheck = db.VacancyPlaces.FirstOrDefault(vp => vp.Code == newValue || vp.Name == newValue);
            if (vacancyPlaceToCheck != null) throw new Exception("This data already exsist.");
            VacancyPlace vacancyPlaceToChange = InnerLogic.GetVacancyPlace(columnValue, db);
            bool isDirty = true;
            foreach (var item in _vacancyPlacesProperties)
            {
                if (item.Name == columnName)
                {
                    item.SetValue(vacancyPlaceToChange, newValue);
                    isDirty = false;
                }
            }
            if (isDirty) throw new Exception("The column name is incorrect");
            await db.SaveChangesAsync();
            return vacancyPlaceToChange;
        }
    }
}
