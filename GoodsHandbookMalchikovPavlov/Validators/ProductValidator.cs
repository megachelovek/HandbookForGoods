using System;
using System.Reflection;
using System.Text;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    internal abstract class ProductValidator
    {
        protected string lastError;
        protected object lastProperty;
        protected StringBuilder outputBuffer = new StringBuilder();

        public virtual bool Validate(Type productType, PropertyInfo info, string value)
        {
            var result = BasicTypesValidator.Validate(info.PropertyType, value, out lastProperty, out lastError);
            return result;
        }

        public object GetLastProperty()
        {
            return lastProperty;
        }

        public string GetLastError()
        {
            return lastError;
        }
    }
}