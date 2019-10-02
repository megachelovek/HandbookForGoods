using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;
using System.Linq;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    public sealed class ProductValidator : IProductValidator
    {
        public const int MinNameLength = 4;
        public const int MaxNameLength = 32;
        public const int MinUnitLength = 4;
        public const int MaxUnitLength = 16;
        public const int MaxCount = 100000000;
        public const int MaxCountDigits = 10;
        public const float MaxPrice = 1000000000.0f;
        public const int PriceDigitsAfterDecimalPoint = 2;
        public const int PriceMaxDigitsBeforeDecimalPoint = 10;
       
        public bool Validate(Product product, PropertyInfo propertyInfo, string propertyValue,
            out string errorMsg, out object convertedValue)
        {
            errorMsg = null;
            convertedValue = propertyValue;
            switch (propertyInfo.Name)
            {
                case "Name":
                    {
                        if (propertyValue.Length >= MinNameLength && propertyValue.Length <= MaxNameLength)
                        {
                            Regex regex = new Regex(@"(^[\S]+ ?){1,5}$");
                            if (regex.IsMatch(propertyValue))
                            {
                                Regex regex2 = new Regex(@"[\p{P}]{4,}");
                                if (!regex2.IsMatch(propertyValue))
                                {
                                    Regex regex3 = new Regex(@"^[\p{P}]+$");
                                    if (!regex3.IsMatch(propertyValue))
                                    {
                                        return true;

                                    }
                                    else
                                    {
                                        errorMsg = "\"Name\" cannot consists of punctuation marks only" + Environment.NewLine;
                                        return false;
                                    }
                                }
                                else
                                {
                                    errorMsg = "\"Name\" should not contain more than 3 punctuation marks adjacent to each other" + Environment.NewLine;
                                    return false;
                                }
                            }
                            else
                            {
                                errorMsg = "\"Name\" should consist of maximum 5 words, double whitespace characters is not allowed" + Environment.NewLine;
                                return false;
                            }
                        }
                        else
                        {
                            errorMsg = $"\"Name\" should have length from {MinNameLength} to {MaxNameLength}" + Environment.NewLine;
                            return false;
                        }
                    }
                case "Unit":
                    {
                        Regex regex = new Regex($"[\\p{{L}}]{{{MinUnitLength},{MaxUnitLength}}}");
                        if (regex.IsMatch(propertyValue))
                        {
                            return true;
                        }
                        else
                        {
                            errorMsg = $"\"Unit\" should consist of letters only and have length from {MinUnitLength} to {MaxUnitLength}" + Environment.NewLine;
                            return false;
                        }
                       
                    }
                case "Price":
                    {
                        string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                        Regex regex = new Regex($"^[0-9]{{1,{PriceMaxDigitsBeforeDecimalPoint}}}{decimalSeparator}[0-9]{{{PriceDigitsAfterDecimalPoint}}}$");
                        if (regex.IsMatch(propertyValue))
                        {
                            convertedValue = float.Parse(propertyValue);
                            return true;
                        }
                        else
                        {
                            errorMsg = $"\"Price\" should be a decimal floating point number with exactly {PriceDigitsAfterDecimalPoint} digits after decimal point" + Environment.NewLine;
                            return false;
                        }
                        
                    }
                case "Count":
                    {
                        Regex regex = new Regex($"^[0-9]{{1,{MaxCountDigits}}}$");
                        if (regex.IsMatch(propertyValue))
                        {
                            convertedValue = int.Parse(propertyValue);
                            return true;
                        }
                        else
                        {
                            errorMsg = $"\"Count\" should be an integer number of maximum {MaxCountDigits} digits long" + Environment.NewLine;
                            return false;
                        }
                    }
                default:
                    {
                        throw new ArgumentException();
                    }
            }
        }
    }
}
