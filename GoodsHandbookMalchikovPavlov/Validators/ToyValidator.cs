using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace GoodsHandbookMalchikovPavlov.Validators
{
    sealed class ToyValidator : ProductValidator
    {
        private static readonly string[] TYPE = new string[]
        {
            "Educational", "Video Game", "Doll", "Electronic"
        };

        private const int MIN_AGE = 0;
        private const int MAX_AGE = 18;
        private static readonly string[] SEX = new string[]
            {
                "Male", "Female"
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
                    case "StartAge":
                    case "EndAge":

                        {
                            result = BasicTypesValidator.Validate(info.PropertyType, value, out lastProperty, out lastError);
                            if (result)
                            {
                                int age = (int)lastProperty;
                                result = (age >= MIN_AGE && age <= MAX_AGE);
                                
                            }
                            if (!result)
                            {
                                lastError = String.Format("Age must be an interger from {0} to {1}\n", MIN_AGE, MAX_AGE);
                            }

                        }
                        break;
                    case "Sex":

                        {
                            result = Misc.FindString(value, 0, value.Length - 1, SEX) > -1;
                            if (result)
                            {
                                lastProperty = value;
                            }
                            else
                            {
                                outputBuffer.Append("Sex must be one of the names listed below:\n");
                                foreach (string s in SEX)
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
