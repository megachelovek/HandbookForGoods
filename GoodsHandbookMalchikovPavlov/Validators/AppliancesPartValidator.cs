using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;
using System.Linq;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    internal static class AppliancesPartValidator
    {
        private const int MinModelLength = 2;
        private const int MaxModelLength = 32;
        private static readonly string ModelHasZeroLengthErrorMsg =
            "\"Model\" cannot have zero length" + Environment.NewLine;
        private static readonly string ModelIsTooLongErrorMsg =
            "\"Model\" cannot have length more than " + MaxModelLength + " characters long" + Environment.NewLine;
        private static readonly string ModelIsTooShortErrorMsg =
            "\"Model\" cannot have length less than " + MinModelLength + " characters long" + Environment.NewLine;
        private static readonly string ModelConsistOfMoreThanOneWordErrorMsg =
           "\"Model\" cannot consist of more than one word" + Environment.NewLine;
        private static readonly string ModelContainsWrongPatternErrorMsg =
          "\"Model\" can only contain letters, digits and optionally \"-\" symbol in between" + Environment.NewLine;
        private const int MinCompanyLength = 2;
        private const int MaxCompanyLength = 32;
        private static readonly string CompanyHasZeroLengthErrorMsg =
            "\"Company\" cannot have zero length" + Environment.NewLine;
        private static readonly string CompanyIsTooLongErrorMsg =
            "\"Company\" cannot have length more than " + MaxCompanyLength + " characters long" + Environment.NewLine;
        private static readonly string CompanyIsTooShortErrorMsg =
            "\"Company\" cannot have length less than " + MinCompanyLength + " characters long" + Environment.NewLine;
        private static readonly string CompanyConsistOfMoreThanOneWordErrorMsg =
           "\"Company\" cannot consist of more than one word" + Environment.NewLine;
        private static readonly string CompanyContainsWrongPatternErrorMsg =
          "\"Company\" can only contain letters, digits and optionally \"-\" symbol in between" + Environment.NewLine;
        private static readonly string[] Categories = new string[]
        {
          "Refrigerator",
          "Stove",
          "Teapot",
        };
        private static readonly string CategoriesDesc;
        static AppliancesPartValidator()
        {
            StringBuilder buffer = new StringBuilder(128);
            buffer.Append("Value must be one of the following:");
            buffer.Append(Environment.NewLine);
            foreach (var s in Categories)
            {
                buffer.Append(s);
                buffer.Append(Environment.NewLine);
            }
            CategoriesDesc = buffer.ToString();
        }
        public static bool Validate(Product product, PropertyInfo propertyInfo, string propertyValue,
          out string errorMsg, out object convertedValue)
        {
            errorMsg = null;
            convertedValue = propertyValue;
            switch (propertyInfo.Name)
            {
                case "Category":
                    {
                        return ValidateCategory(product, propertyInfo, propertyValue, ref errorMsg, ref convertedValue);
                    }
                case "Model":
                    {
                        return ValidateModel(product, propertyInfo, propertyValue, ref errorMsg, ref convertedValue);
                    }
                case "Company":
                    {
                        return ValidateCompany(product, propertyInfo, propertyValue, ref errorMsg, ref convertedValue);
                    }
                default:
                    {
                        throw new ArgumentException();
                    }
            }
        }
        /// <summary>
        /// Возвращает тру если propertyValue равна одной из строк в Categories выше
        /// </summary>
        private static bool ValidateCategory(Product product, PropertyInfo propertyInfo, string propertyValue,
            ref string errorMsg, ref object convertedValue)
        {
            if (Misc.FindString(propertyValue, Categories) > -1)
            {
                return true;
            }
            else
            {
                errorMsg = CategoriesDesc;
            }
            return false;
        }
        /// <summary>
        /// Возвращает тру если propertyValue - строка состоящая из букв, цифр и дефиса, дефис может находиться только
        /// в середине строки, дефисов может быть несколько, длина строки должна быть от 2 до 32 символов включительно
        /// </summary>
        private static bool ValidateModel(Product product, PropertyInfo propertyInfo, string propertyValue,
            ref string errorMsg, ref object convertedValue)
        {
            string[] words = InputParser.GetWords(propertyValue);
            if (words.Length == 1)
            {
                if (words[0].Length >= MinModelLength && words[0].Length <= MaxModelLength)
                {
                    bool check = words[0].All(c => Char.IsLetterOrDigit(c) || c == '-');
                    Regex regex = new Regex(@"^\w(\w|-)*\w$");
                    if (check && regex.IsMatch(words[0]))
                    {
                        return true;
                    }
                    else
                    {
                        errorMsg = ModelContainsWrongPatternErrorMsg;
                    }
                }
                else
                {
                    if (words[0].Length < MinModelLength)
                    {
                        errorMsg = ModelIsTooShortErrorMsg;
                    }
                    else
                    {
                        errorMsg = ModelIsTooLongErrorMsg;
                    }
                }
            }
            else if (words.Length == 0)
            {
                errorMsg = ModelHasZeroLengthErrorMsg;
            }
            else if (words.Length > 1)
            {
                errorMsg = ModelConsistOfMoreThanOneWordErrorMsg;
            }
            return false;
        }
        /// <summary>
        /// Возвращает тру если propertyValue - строка состоящая из букв, цифр и дефиса, дефис может находиться только
        /// в середине строки, дефисов может быть несколько, длина строки должна быть от 2 до 32 символов включительно
        /// </summary>
        private static bool ValidateCompany(Product product, PropertyInfo propertyInfo, string propertyValue,
            ref string errorMsg, ref object convertedValue)
        {
            string[] words = InputParser.GetWords(propertyValue);
            if (words.Length == 1)
            {
                if (words[0].Length >= MinCompanyLength && words[0].Length <= MaxCompanyLength)
                {
                    bool check = words[0].All(c => Char.IsLetterOrDigit(c) || c == '-');
                    Regex regex = new Regex(@"^\w(\w|-)*\w$");
                    if (check && regex.IsMatch(words[0]))
                    {
                        return true;
                    }
                    else
                    {
                        errorMsg = CompanyContainsWrongPatternErrorMsg;
                    }
                }
                else
                {
                    if (words[0].Length < MinModelLength)
                    {
                        errorMsg = CompanyIsTooShortErrorMsg;
                    }
                    else
                    {
                        errorMsg = CompanyIsTooLongErrorMsg;
                    }
                }
            }
            else if (words.Length == 0)
            {
                errorMsg = CompanyHasZeroLengthErrorMsg;
            }
            else if (words.Length > 1)
            {
                errorMsg = CompanyConsistOfMoreThanOneWordErrorMsg;
            }
            return false;
        }
    }
}
