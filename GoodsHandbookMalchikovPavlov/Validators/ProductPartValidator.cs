using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;
using System.Linq;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    internal static class ProductPartValidator
    {
        private const int MinNameLength = 4;
        private const int MaxNameLength = 32;
        private static readonly string NameHasZeroLengthErrorMsg =
            "\"Name\" cannot have zero length" + Environment.NewLine;
        private static readonly string NameIsTooLongErrorMsg =
            "\"Name\" cannot have length more than " + MaxNameLength + " characters long"+ Environment.NewLine;
        private static readonly string NameIsTooShortErrorMsg =
            "\"Name\" cannot have length less than " + MinNameLength + " characters long" + Environment.NewLine;
        private static readonly string NameConsistOfMoreThanOneWordErrorMsg =
           "\"Name\" cannot consist of more than one word" + Environment.NewLine;
        private static readonly string NameContainsWrongPatternErrorMsg =
          "\"Name\" can only contain letters and optionally \"-\" symbol in between" + Environment.NewLine;
        private const int MinUnitLength = 4;
        private const int MaxUnitLength = 16;
        private static readonly string UnitHasZeroLengthErrorMsg =
            "\"Unit\" cannot have zero length" + Environment.NewLine;
        private static readonly string UnitIsTooShortErrorMsg =
            "\"Unit\" cannot have length less than " + MinUnitLength + " characters long" + Environment.NewLine;
        private static readonly string UnitIsTooLongErrorMsg =
            "\"Unit\" cannot have length more than " + MaxUnitLength + " characters long" + Environment.NewLine;
        private static readonly string UnitConsistOfMoreThanOneWordErrorMsg =
           "\"Unit\" cannot consist of more than one word" + Environment.NewLine;
        private static readonly string UnitContainsWrongPatternErrorMsg =
         "\"Unit\" should only contain letters" + Environment.NewLine;
        private static readonly string PriceGarbageInputErrorMsg =
            "\"Price\" should be a non-negative floating point number with two digits after decimal point" + Environment.NewLine +
            "for example: \"10" + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "95\"" + Environment.NewLine;
        private const int MaxCount = 100000000;
        private static readonly string CountGarbageInputErrorMsg =
            "\"Count\" should be a non-negative integer number"+ Environment.NewLine;
        private static readonly string CountTooLargeErrorMsg =
           "\"Count\" cannot be larget than " + MaxCount + Environment.NewLine;


        public static bool Validate(Product product, PropertyInfo propertyInfo, string propertyValue,
            out string errorMsg, out object convertedValue)
        {
            errorMsg = null;
            convertedValue = propertyValue;
            switch (propertyInfo.Name)
            {
                case "Name":
                    {
                        return ValidateName(product, propertyInfo, propertyValue, ref errorMsg, ref convertedValue);
                    }
                case "Unit":
                    {
                        return ValidateUnit(product, propertyInfo, propertyValue, ref errorMsg, ref convertedValue);
                    }
                case "Price":
                    {
                        return ValidatePrice(product, propertyInfo, propertyValue, ref errorMsg, ref convertedValue);
                    }
                case "Count":
                    {
                        return ValidateCount(product, propertyInfo, propertyValue, ref errorMsg, ref convertedValue);
                    }
                default:
                    {
                        throw new ArgumentException();
                    }
            }
        }
        /// <summary>
        /// Возвращает тру если propertyValue - строка состоящая из букв и дефисa, дефис может находиться только
        /// в середине строки, дефисов может быть несколько, длина строки должна быть от 4 до 32 символов включительно
        /// </summary>
        private static bool ValidateName(Product product, PropertyInfo propertyInfo, string propertyValue,
            ref string errorMsg, ref object convertedValue)
        {
            string[] words = InputParser.GetWords(propertyValue);
            if (words.Length == 1)
            {
                if (words[0].Length >= MinNameLength && words[0].Length <= MaxNameLength)
                {
                    bool check = words[0].All(c => Char.IsLetter(c) || c == '-');
                    Regex regex = new Regex(@"^\w(\w|-)*\w$");
                    if (check  && regex.IsMatch(words[0]))
                    {
                        return true;
                    }
                    else
                    {
                        errorMsg = NameContainsWrongPatternErrorMsg;
                    }
                }
                else
                {
                    if (words[0].Length < MinNameLength)
                    {
                        errorMsg = NameIsTooShortErrorMsg;
                    }
                    else
                    {
                        errorMsg = NameIsTooLongErrorMsg;
                    }
                }
            }
            else if (words.Length == 0)
            {
                errorMsg = NameHasZeroLengthErrorMsg;
            }
            else if (words.Length > 1)
            {
                errorMsg = NameConsistOfMoreThanOneWordErrorMsg;
            }
            return false;
        }
        /// <summary>
        /// Возвращает тру если propertyValue - строка состоящая из букв и дефисa, дефис может находиться только
        /// в середине строки, дефисов может быть несколько, длина строки должна быть от 4 до 16 символов включительно
        /// </summary>
        private static bool ValidateUnit(Product product, PropertyInfo propertyInfo, string propertyValue,
            ref string errorMsg, ref object convertedValue)
        {
            string[] words = InputParser.GetWords(propertyValue);
            if (words.Length == 1)
            {
                if (words[0].Length >= MinUnitLength && words[0].Length <= MaxUnitLength)
                {
                    if (InputParser.ContainsOnlyLetters(words[0]))
                    {
                        return true;
                    }
                    else
                    {
                        errorMsg = UnitContainsWrongPatternErrorMsg;
                    }
                }
                else
                {
                    if (words[0].Length < MinUnitLength)
                    {
                        errorMsg = UnitIsTooShortErrorMsg;
                    }
                    else
                    {
                        errorMsg = UnitIsTooLongErrorMsg;
                    }
                }
            }
            else if (words.Length == 0)
            {
                errorMsg = UnitHasZeroLengthErrorMsg;
            }
            else if (words.Length > 1)
            {
                errorMsg = UnitConsistOfMoreThanOneWordErrorMsg;
            }
            return false;
        }
        /// <summary>
        /// Возвращает тру если propertyValue - содежржит число с плавающей точкой, например 10,95
        /// ровно две цифры после запятой обязательны.
        /// </summary>
        private static bool ValidatePrice(Product product, PropertyInfo propertyInfo, string propertyValue,
            ref string errorMsg, ref object convertedValue)
        {
            string[] words = InputParser.GetWords(propertyValue);
            if (words.Length == 1)
            {
                CultureInfo culture = CultureInfo.CurrentCulture;
                string decimalSeparator = culture.NumberFormat.NumberDecimalSeparator;
                Regex regex = new Regex(@"^[0-9]+[0-9]*" + decimalSeparator + @"[0-9]{2}$");
                if (regex.IsMatch(words[0]))
                {
                    float temp;
                    if (float.TryParse(words[0], out temp))
                    {
                        convertedValue = temp;
                        return true;
                    }
                    else
                    {
                        errorMsg = PriceGarbageInputErrorMsg;
                    }
                }
                else
                {
                    errorMsg = PriceGarbageInputErrorMsg;
                }
            }
            else
            {
                errorMsg = PriceGarbageInputErrorMsg;
            }
            
            return false;
        }
        /// <summary>
        /// Возвращает тру если propertyValue - целое число не больше чем 1000000000
        /// ровно две цифры после запятой обязательны.
        /// </summary>
        private static bool ValidateCount(Product product, PropertyInfo propertyInfo, string propertyValue,
            ref string errorMsg, ref object convertedValue)
        {
            string[] words = InputParser.GetWords(propertyValue);
            if (words.Length == 1)
            {
                Regex regex = new Regex(@"^(0|[1-9][0-9]*)$");
                if (regex.IsMatch(words[0]))
                {
                    int temp;
                    if (int.TryParse(words[0], out temp))
                    {
                        if (temp <= MaxCount)
                        {
                            convertedValue = temp;
                            return true;
                        }
                        else
                        {
                            errorMsg = CountTooLargeErrorMsg;
                        }

                    }
                    else
                    {
                        errorMsg = CountTooLargeErrorMsg;
                    }
                }
                else
                {
                    errorMsg = CountGarbageInputErrorMsg;
                }
            }
            else
            {
                errorMsg = CountGarbageInputErrorMsg;
            }

            return false;
        }
    }
}
