using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace GoodsHandbookMalchikovPavlov.Validators
{
    sealed class HomeAppliancesValidator : ProductValidator
    {
        private static readonly string[] TYPE = new string[]
        {
            "Phone", "Refrigerator", "Microwave"
        };

       
        public override bool Validate(Type productType, PropertyInfo info, string value)
        {
            lastProperty = null;
            lastError = null;
            outputBuffer.Length = 0;
            bool result = false;
            if (productType.Equals(info.DeclaringType))
            {
                switch (info.Name)
                {
                    case "Type":
                        {
                            result = Misc.FindString(value, 0, value.Length - 1, TYPE) > -1;
                            if (result)
                            {
                                lastProperty = value;
                            }
                            else
                            {
                                outputBuffer.Append("Type must be one of the names listed below:\n");
                                foreach (string s in TYPE)
                                {
                                    outputBuffer.Append("- ");
                                    outputBuffer.Append(s);
                                    outputBuffer.Append("\n");
                                }
                                lastError = outputBuffer.ToString();
                                lastProperty = null;
                            }
                        }
                        break;
                    default:
                        {
                            result = BasicTypesValidator.Validate(info.PropertyType, value, out lastProperty, out lastError);
                        }
                        break;

                }
            }
            else
            {
                result = base.Validate(productType, info, value);
            }
            return result;
        }
    }
}
