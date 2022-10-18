using System.Text.RegularExpressions;

namespace EmploymentHelper.BLogic
{
    public class InnerLogic
    {
        public static bool IsINN(string inn) => Regex.IsMatch(inn, @"^\d{10}");//12
    }
}