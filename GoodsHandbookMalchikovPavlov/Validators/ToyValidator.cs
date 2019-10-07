using System;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    public sealed class ToyValidator : IProductValidator
    {
        public const int MinAge = 0;
        public const int MaxAge = 18;
        private readonly IProductCatalog productCatalog;

        public ToyValidator(IProductCatalog productCatalog)
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
                case "MinAge":
                {
                    var validateResult = CommonValidator.ValidateNonNegativeIntInRange(propertyValue, MinAge, MaxAge);
                    if (validateResult == ValidateResult.Ok)
                    {
                        convertedValue = int.Parse(propertyValue);
                        return true;
                    }

                    errorMsg = CommonValidator.GetErrorMessageNonNegativeInteger(validateResult, MinAge, MaxAge,
                        "Minimum Age");
                    return false;
                }
                case "MaxAge":
                {
                    var toy = (Toy) product;
                    var validateResult =
                        CommonValidator.ValidateNonNegativeIntInRange(propertyValue, toy.MinAge, MaxAge);
                    if (validateResult == ValidateResult.Ok)
                    {
                        convertedValue = int.Parse(propertyValue);
                        return true;
                    }

                    errorMsg = CommonValidator.GetErrorMessageNonNegativeInteger(validateResult, toy.MinAge, MaxAge,
                        "Maximum Age");
                    return false;
                }
                case "Sex":
                {
                    var sexes = productCatalog.GetPropertyValidValues(propertyInfo);
                    var validateResult = CommonValidator.ValidateStringContainsInArray(propertyValue, (string[])sexes);
                    if (validateResult == ValidateResult.Ok)
                    {
                        return true;
                    }

                    errorMsg = CommonValidator.GetErrorMessageStringDoesNotContainInArray(validateResult, (string[])sexes, "Sex");
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