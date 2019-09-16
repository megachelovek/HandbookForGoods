using System;
using System.Reflection;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    internal sealed class BookValidator : ProductValidator
    {
        private static readonly string[] GENRES =
        {
            "Fairy Tale",
            "Mystic",
            "Fantasy",
            "Detective",
            "Psychology",
            "Popular Science",
            "Educational",
            "Sentimental Novel",
            "Teenage Prose"
        };

        public override bool Validate(Type productType, PropertyInfo info, string value)
        {
            lastProperty = null;
            lastError = null;
            outputBuffer.Length = 0;
            var result = false;
            if (productType.Equals(info.DeclaringType))
                switch (info.Name)
                {
                    case "Year":
                    {
                        result = BasicTypesValidator.Validate(info.PropertyType, value, out lastProperty,
                            out lastError);
                        if (result)
                        {
                            var year = (int) lastProperty;
                            result = year >= 0 && year <= DateTime.Now.Year;
                        }

                        if (!result)
                            lastError = string.Format("Year must be an interger from {0} to {1}\n", 0,
                                DateTime.Now.Year);
                    }
                        break;
                    case "Genre":
                    {
                        result = Misc.FindString(value, 0, value.Length - 1, GENRES) > -1;
                        if (result)
                        {
                            lastProperty = value;
                        }
                        else
                        {
                            outputBuffer.Append("Genre must be one of the names listed below:\n");
                            foreach (var s in GENRES)
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
                        result = BasicTypesValidator.Validate(info.PropertyType, value, out lastProperty,
                            out lastError);
                    }
                        break;
                }
            else
                result = base.Validate(productType, info, value);
            return result;
        }
    }
}