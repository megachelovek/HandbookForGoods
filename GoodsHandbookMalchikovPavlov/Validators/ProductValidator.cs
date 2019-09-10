using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    abstract class ProductValidator
    {
        protected object lastProperty;
        protected string lastError;
        protected StringBuilder outputBuffer = new StringBuilder();
        public virtual bool Validate(Type productType, PropertyInfo info, string value)
        {
            bool result = BasicTypesValidator.Validate(info.PropertyType, value, out lastProperty, out lastError);
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
