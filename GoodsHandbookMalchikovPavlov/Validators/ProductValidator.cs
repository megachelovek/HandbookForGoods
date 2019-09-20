using System;
using System.Collections.Generic;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    internal sealed class ProductValidator
    {
        delegate bool ValidateDelegate(Product product, PropertyInfo propertyInfo, string propertyValue,
           out string errorMsg, out object convertedValue);
        private Dictionary<Type, ValidateDelegate> validateMap;
        private string lastErrorMessage;
        private object lastConvertedValue;
        public ProductValidator()
        {
            validateMap = new Dictionary<Type, ValidateDelegate>()
            {
                { typeof(Product), ProductPartValidator.Validate },
                { typeof(Book), BookPartValidator.Validate },
                { typeof(Toy), ToyPartValidator.Validate },
                { typeof(Appliances), AppliancesPartValidator.Validate }


            };
        }
        public bool Validate(Product product, PropertyInfo propertyInfo, string propertyValue)
        {
            if (validateMap.ContainsKey(propertyInfo.DeclaringType))
            {
                return validateMap[propertyInfo.DeclaringType](product, propertyInfo, propertyValue,
                    out lastErrorMessage, out lastConvertedValue);
            }
            return false;
        }
        public string GetLastErrorMessage()
        {
            return lastErrorMessage;
        }
        public object GetLastConvertedValue()
        {
            return lastConvertedValue;
        }
    }
}
