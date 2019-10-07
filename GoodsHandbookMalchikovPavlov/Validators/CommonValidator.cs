using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    public enum ValidateResult
    {
        Ok,
        NotNonNegativeInteger,
        NotNonNegativeFloat,
        OutsideRange,
        DigitsAfterDecimalPointOutsideRange
    }

    public static class CommonValidator
    {
        public static ValidateResult ValidateNonNegativeIntInRange(string value, int min, int max)
        {
            int parsed;
            try
            {
                parsed = int.Parse(value,
                    NumberStyles.None | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);
            }
            catch
            {
                return ValidateResult.NotNonNegativeInteger;
            }

            if (parsed >= min && parsed <= max)
                return ValidateResult.Ok;
            return ValidateResult.OutsideRange;
        }

        public static ValidateResult ValidateNonNegativeFloatInRange(string value, float min, float max,
            int digitsAfterDecimalPoint)
        {
            float parsed;
            try
            {
                parsed = float.Parse(value,
                    NumberStyles.None | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite |
                    NumberStyles.AllowTrailingWhite);
            }
            catch
            {
                return ValidateResult.NotNonNegativeFloat;
            }

            var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var regex = new Regex($"{decimalSeparator}[0-9]{{{digitsAfterDecimalPoint}}}\\s*$");
            if (regex.IsMatch(value))
            {
                if (parsed >= min && parsed <= max)
                    return ValidateResult.Ok;
                return ValidateResult.OutsideRange;
            }

            return ValidateResult.DigitsAfterDecimalPointOutsideRange;
        }

        public static ValidateResult ValidateStringContainsInArray(string value, string[] arr)
        {
            if (Misc.FindString(value, arr) > -1) return ValidateResult.Ok;
            return ValidateResult.OutsideRange;
        }

        public static string GetErrorMessageNonNegativeInteger(ValidateResult error, int min, int max, string name)
        {
            switch (error)
            {
                case ValidateResult.NotNonNegativeInteger:
                {
                    return $"\"{name}\" should be a non-negative integer number" + Environment.NewLine;
                }
                case ValidateResult.OutsideRange:
                {
                    return $"\"{name}\" should be a non-negative integer number from {min} to {max}" +
                           Environment.NewLine;
                }
                default:
                {
                    throw new ArgumentException();
                }
            }
        }

        public static string GetErrorMessageNonNegativeFloat(ValidateResult error, float min, float max,
            int digitsAfterDecimalPoint, string name)
        {
            switch (error)
            {
                case ValidateResult.NotNonNegativeFloat:
                {
                    return $"\"{name}\" should be a non-negative floating point number" + Environment.NewLine;
                }
                case ValidateResult.OutsideRange:
                {
                    return
                        $"\"{name}\" should be a non-negative floating point number from {min.ToString($"F{digitsAfterDecimalPoint}")} to {max.ToString($"F{digitsAfterDecimalPoint}")}" +
                        Environment.NewLine;
                }
                case ValidateResult.DigitsAfterDecimalPointOutsideRange:
                {
                    return
                        $"\"{name}\" should be a non-negative floating point number with {digitsAfterDecimalPoint} digits after decimal point" +
                        Environment.NewLine;
                }
                default:
                {
                    throw new ArgumentException();
                }
            }
        }

        public static string GetErrorMessageStringDoesNotContainInArray(ValidateResult error, string[] arr, string name)
        {
            switch (error)
            {
                case ValidateResult.OutsideRange:
                {
                    var buffer = new StringBuilder(128);
                    buffer.Append($"\"{name}\" should be one of the following:");
                    buffer.Append(Environment.NewLine);
                    foreach (var s in arr)
                    {
                        buffer.Append(s);
                        buffer.Append(Environment.NewLine);
                    }

                    return buffer.ToString();
                }
                default:
                {
                    throw new ArgumentException();
                }
            }
        }
    }
}