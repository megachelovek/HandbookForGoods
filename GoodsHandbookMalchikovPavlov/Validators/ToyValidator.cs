using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    public sealed class ToyValidator : IProductValidator
    {
        public const int MinAge = 0;
        public const int MaxAge = 18;
        private IProductCatalog productCatalog;
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
                        string[] categories = productCatalog.GetProductPropertyValidValues(propertyInfo);
                        ValidateResult validateResult = CommonValidator.ValidateStringContainsInArray(propertyValue, categories);
                        if (validateResult == ValidateResult.Ok)
                        {
                            return true;
                        }
                        else
                        {
                            errorMsg = CommonValidator.GetErrorMessageStringDoesNotContainInArray(validateResult, categories, "Category");
                            return false;
                        }
                    }
                case "MinAge":
                    {
                        ValidateResult validateResult = CommonValidator.ValidateNonNegativeIntInRange(propertyValue, MinAge, MaxAge);
                        if (validateResult == ValidateResult.Ok)
                        {
                            convertedValue = int.Parse(propertyValue);
                            return true;
                        }
                        else
                        {
                            errorMsg = CommonValidator.GetErrorMessageNonNegativeInteger(validateResult, MinAge, MaxAge, "Minimum Age");
                            return false;
                        }
                    }
                case "MaxAge":
                    {
                        Toy toy = (Toy)product;
                        ValidateResult validateResult = CommonValidator.ValidateNonNegativeIntInRange(propertyValue, toy.MinAge, MaxAge);
                        if (validateResult == ValidateResult.Ok)
                        {
                            convertedValue = int.Parse(propertyValue);
                            return true;
                        }
                        else
                        {
                            errorMsg = CommonValidator.GetErrorMessageNonNegativeInteger(validateResult, toy.MinAge, MaxAge, "Maximum Age");
                            return false;
                        }
                    }
                case "Sex":
                    {
                        string[] sexes = productCatalog.GetProductPropertyValidValues(propertyInfo);
                        ValidateResult validateResult = CommonValidator.ValidateStringContainsInArray(propertyValue, sexes);
                        if (validateResult == ValidateResult.Ok)
                        {
                            return true;
                        }
                        else
                        {
                            errorMsg = CommonValidator.GetErrorMessageStringDoesNotContainInArray(validateResult, sexes, "Sex");
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
