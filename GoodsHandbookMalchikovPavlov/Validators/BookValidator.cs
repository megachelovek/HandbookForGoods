using System;
using System.Reflection;
using System.Text.RegularExpressions;
using GoodsHandbookMalchikovPavlov.Models;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    public sealed class BookValidator : IProductValidator
    {
        public const int MinAuthorLength = 2;
        public const int MaxAuthorLength = 32;
        private readonly IProductCatalog productCatalog;

        public BookValidator(IProductCatalog productCatalog)
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
                case "AuthorFirstName":
                case "AuthorLastName":
                case "AuthorMiddleName":
                {
                    if (propertyValue.Length >= MinAuthorLength && propertyValue.Length <= MaxAuthorLength)
                    {
                        var regex = new Regex(@"(^[\p{L}-']+ ?){1,2}$");
                        if (regex.IsMatch(propertyValue))
                        {
                            var regex2 = new Regex(@"[\p{P}]{2,}");
                            if (!regex2.IsMatch(propertyValue)) return true;

                            errorMsg =
                                $"\"{Misc.GetPropertyName(propertyInfo)}\" should not contain more than 1 punctuation marks adjacent to each other" +
                                Environment.NewLine;
                            return false;
                        }

                        errorMsg =
                            $"\"{Misc.GetPropertyName(propertyInfo)}\" should consist of maximum 2 words. Only letters, \"'\", \"-\" marks are allowed. Double whitespace characters is not allowed" +
                            Environment.NewLine;
                        return false;
                    }

                    if (propertyInfo.Name.Equals("AuthorMiddleName") && propertyValue.Length == 0) return true;

                    errorMsg =
                        $"\"{Misc.GetPropertyName(propertyInfo)}\" should have length from {MinAuthorLength} to {MaxAuthorLength}" +
                        Environment.NewLine;
                    return false;
                }
                case "Genre":
                {
                    var genres = productCatalog.GetPropertyValidValues(propertyInfo);
                    var validateResult =
                        CommonValidator.ValidateStringContainsInArray(propertyValue, (string[])genres);
                    if (validateResult == ValidateResult.Ok)
                    {
                        return true;
                    }

                    errorMsg = CommonValidator.GetErrorMessageStringDoesNotContainInArray(validateResult, (string[])genres,
                        "Genre");
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