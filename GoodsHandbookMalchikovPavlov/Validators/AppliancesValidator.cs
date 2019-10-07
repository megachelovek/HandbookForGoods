using System;
using System.Reflection;
using System.Text.RegularExpressions;
using GoodsHandbookMalchikovPavlov.Models;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    public sealed class AppliancesValidator : IProductValidator
    {
        public const int MinModelLength = 2;
        public const int MaxModelLength = 32;
        public const int MinCompanyLength = 2;
        public const int MaxCompanyLength = 32;
        private readonly IProductCatalog productCatalog;

        public AppliancesValidator(IProductCatalog productCatalog)
        {
            this.productCatalog = productCatalog;
        }

        public bool Validate(Product product, PropertyInfo propertyInfo, string propertyValue,
            out string errorMsg, out object convertedValue)
        {
            errorMsg = null;
            convertedValue = propertyValue;
            switch (propertyInfo.Name)
            {
                case "Category":
                {
                    var categories = productCatalog.GetPropertyValidValues(propertyInfo);
                    var validateResult = CommonValidator.ValidateStringContainsInArray(propertyValue, (string[])categories);
                    if (validateResult == ValidateResult.Ok) return true;

                    errorMsg = CommonValidator.GetErrorMessageStringDoesNotContainInArray(validateResult, (string[])categories,
                        "Category");
                    return false;
                }
                case "Model":
                case "Company":
                {
                    if (propertyValue.Length >= MinModelLength && propertyValue.Length <= MaxModelLength)
                    {
                        var regex = new Regex(@"(^[\S]+ ?){1,5}$");
                        if (regex.IsMatch(propertyValue))
                        {
                            var regex2 = new Regex(@"[\p{P}]{4,}");
                            if (!regex2.IsMatch(propertyValue))
                            {
                                return true;
                            }

                            errorMsg =
                                $"\"{Misc.GetPropertyName(propertyInfo)}\" should not contain more than 3 punctuation marks adjacent to each other" +
                                Environment.NewLine;
                            return false;
                        }

                        errorMsg =
                            $"\"{Misc.GetPropertyName(propertyInfo)}\" should consist of maximum 5 words, double whitespace characters is not allowed" +
                            Environment.NewLine;
                        return false;
                    }

                    errorMsg =
                        $"\"{Misc.GetPropertyName(propertyInfo)}\" should have length from {MinModelLength} to {MaxModelLength}" +
                        Environment.NewLine;
                    return false;
                }
                default:
                {
                    throw new ArgumentException();
                }
            }
        }
    }
}