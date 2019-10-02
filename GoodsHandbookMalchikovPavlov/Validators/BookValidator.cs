using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;
using System.Linq;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    public sealed class BookValidator : IProductValidator
    {
        public const int MinAuthorLength = 2;
        public const int MaxAuthorLength = 32;
        private IProductCatalog productCatalog;
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
                            Regex regex = new Regex(@"(^[\p{L}-']+ ?){1,2}$");
                            if (regex.IsMatch(propertyValue))
                            {
                                Regex regex2 = new Regex(@"[\p{P}]{2,}");
                                if (!regex2.IsMatch(propertyValue))
                                {
                                    return true;
                                }
                                else
                                {
                                    errorMsg = $"\"{Misc.GetPropertyName(propertyInfo)}\" should not contain more than 1 punctuation marks adjacent to each other" + Environment.NewLine;
                                    return false;
                                }
                            }
                            else
                            {
                                errorMsg = $"\"{Misc.GetPropertyName(propertyInfo)}\" should consist of maximum 2 words. Only letters, \"'\", \"-\" marks are allowed. Double whitespace characters is not allowed" + Environment.NewLine;
                                return false;
                            }
                        }
                        else
                        {
                            if (propertyInfo.Name.Equals("AuthorMiddleName") && propertyValue.Length == 0)
                            {
                                return true;
                            }
                            else
                            {
                                errorMsg = $"\"{Misc.GetPropertyName(propertyInfo)}\" should have length from {MinAuthorLength} to {MaxAuthorLength}" + Environment.NewLine;
                                return false;
                            }
                        }
                    }
                case "Genre":
                    {
                        string[] genres = productCatalog.GetProductPropertyValidValues(propertyInfo);
                        ValidateResult validateResult =
                            CommonValidator.ValidateStringContainsInArray(propertyValue, genres);
                        if (validateResult == ValidateResult.Ok)
                        {
                            return true;
                        }
                        else
                        {
                            errorMsg = CommonValidator.GetErrorMessageStringDoesNotContainInArray(validateResult, genres, "Genre");
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
