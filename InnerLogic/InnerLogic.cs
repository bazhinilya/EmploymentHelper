using System;
using System.Text.RegularExpressions;

namespace EmploymentHelper.BLogic
{
    public class InnerLogic
    {
        public static bool IsINN(string inn) => Regex.IsMatch(inn, @"^\d{10}");//12
        public static bool IsWord(string word) => Regex.IsMatch(word, @"\s*[a-zA-Zа-яА-Я]+\s*");
        public static bool IsLink(string value) => value.StartsWith("https", StringComparison.OrdinalIgnoreCase);
    }
}