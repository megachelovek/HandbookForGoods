using System;
using System.Collections.Generic;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    public sealed class ProductValidatorManager
    {
        private object lastConvertedValue;
        private string lastErrorMessage;
        private IProductCatalog productCatalog;
        private readonly Dictionary<Type, IProductValidator> validatorMap;

        public ProductValidatorManager(IProductCatalog productCatalog)
        {
            this.productCatalog = productCatalog;
            validatorMap = new Dictionary<Type, IProductValidator>
            {
                {typeof(Product), new ProductValidator()},
                {typeof(Book), new BookValidator(productCatalog)},
                {typeof(Toy), new ToyValidator(productCatalog)},
                {typeof(Appliances), new AppliancesValidator(productCatalog)}
            };
        }

        public bool Validate(Product product, PropertyInfo propertyInfo, string propertyValue)
        {
            if (validatorMap.ContainsKey(propertyInfo.DeclaringType))
                return validatorMap[propertyInfo.DeclaringType].Validate(product, propertyInfo, propertyValue,
                    out lastErrorMessage, out lastConvertedValue);
            throw new ArgumentException();
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