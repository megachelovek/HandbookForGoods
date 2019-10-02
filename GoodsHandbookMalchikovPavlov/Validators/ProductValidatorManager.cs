using System;
using System.Collections.Generic;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    public sealed class ProductValidatorManager
    {
        private IProductCatalog productCatalog;
        private Dictionary<Type, IProductValidator> validatorMap;
        private string lastErrorMessage;
        private object lastConvertedValue;
        public ProductValidatorManager(IProductCatalog productCatalog)
        {
            this.productCatalog = productCatalog;
            validatorMap = new Dictionary<Type, IProductValidator>()
            {
                { typeof(Product), new ProductValidator() },
                { typeof(Book), new BookValidator(productCatalog) },
                { typeof(Toy), new ToyValidator(productCatalog) },
                { typeof(Appliances), new AppliancesValidator(productCatalog)}
            };
        }
        public bool Validate(Product product, PropertyInfo propertyInfo, string propertyValue)
        {
            if (validatorMap.ContainsKey(propertyInfo.DeclaringType))
            {
                return validatorMap[propertyInfo.DeclaringType].Validate(product, propertyInfo, propertyValue,
                    out lastErrorMessage, out lastConvertedValue);
            }
            else
            {
                throw new ArgumentException();
            }
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
