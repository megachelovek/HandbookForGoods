using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    internal static class ToyPartValidator
    {
        private static readonly string[] Categories = new string[]
         {
            "Educational", "Video Game", "Doll", "Electronic"
         };
        private const int MinAge = 0;
        private const int MaxAge = 18;
        private static readonly string MinAgeInvalidErrorMsg =
            "\"Minimun Age\" should be an ingeger from {0} to {1}";
        private static readonly string MaxAgeInvalidErrorMsg =
            "\"Maximum Age\" should be an ingeger from {0} to {1}";
        private static readonly string[] Sex = new string[]
            {
                "Male", "Female", "Any"
            };
        private static readonly string CategoriesDesc;
        private static readonly string SexDesc;
        static ToyPartValidator()
        {
            StringBuilder buffer = new StringBuilder(128);
            buffer.Append("\"Category\" must be one of the following:");
            buffer.Append(Environment.NewLine);
            foreach (var s in Categories)
            {
                buffer.Append(s);
                buffer.Append(Environment.NewLine);
            }
            CategoriesDesc = buffer.ToString();

            buffer.Length = 0;
            buffer.Append("\"Sex\" must be one of the following:");
            buffer.Append(Environment.NewLine);
            foreach (var s in Sex)
            {
                buffer.Append(s);
                buffer.Append(Environment.NewLine);
            }
            SexDesc = buffer.ToString();
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
                case "MinAge":
                    {
                        return ValidateMinAge(product, propertyInfo, propertyValue, ref errorMsg, ref convertedValue);
                    }
                case "MaxAge":
                    {
                        return ValidateMaxAge(product, propertyInfo, propertyValue, ref errorMsg, ref convertedValue);
                    }
                case "Sex":
                    {
                        return ValidateSex(product, propertyInfo, propertyValue, ref errorMsg, ref convertedValue);
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
        /// Возвращает тру если propertyValue равна целому числу от 0 до 18 включительно
        /// </summary>
        private static bool ValidateMinAge(Product product, PropertyInfo propertyInfo, string propertyValue,
            ref string errorMsg, ref object convertedValue)
        {
            string[] words = InputParser.GetWords(propertyValue);
            int currentMaxAge = (int)product.GetType().GetProperty("MaxAge").GetValue(product);
            if (words.Length == 1)
            {
                int newMinAge;
                if (int.TryParse(words[0], out newMinAge))
                {
                    bool isValid = newMinAge >= 0 && (newMinAge <= (currentMaxAge == 0 ? MaxAge : currentMaxAge));
                    if (isValid)
                    {
                        convertedValue = newMinAge;
                        return true;
                    }
                }
            }
            errorMsg = string.Format(MinAgeInvalidErrorMsg, MinAge, currentMaxAge == 0 ? MaxAge : currentMaxAge) + Environment.NewLine;
            return false;
        }
        /// <summary>
        /// Возвращает тру если propertyValue равна целому числу от ранее введенного MinAge до 18 включительно
        /// </summary>
        private static bool ValidateMaxAge(Product product, PropertyInfo propertyInfo, string propertyValue,
            ref string errorMsg, ref object convertedValue)
        {
            string[] words = InputParser.GetWords(propertyValue);
            int currentMinAge = (int)product.GetType().GetProperty("MinAge").GetValue(product);
            if (words.Length == 1)
            {
                int newMaxAge;
                if (int.TryParse(words[0], out newMaxAge))
                {
                    bool isValid = newMaxAge >= currentMinAge && (newMaxAge <= MaxAge);
                    if (isValid)
                    {
                        convertedValue = newMaxAge;
                        return true;
                    }
                }
            }
            errorMsg = string.Format(MaxAgeInvalidErrorMsg, currentMinAge, MaxAge) + Environment.NewLine;
            return false;
        }
        /// <summary>
        /// Возвращает тру если propertyValue равна "Male" или "Female" или "Any"
        /// </summary>
        private static bool ValidateSex(Product product, PropertyInfo propertyInfo, string propertyValue,
            ref string errorMsg, ref object convertedValue)
        {
            if (Misc.FindString(propertyValue, Sex) > -1)
            {
                return true;
            }
            else
            {
                errorMsg = SexDesc;
            }
            return false;
        }
    }
}
