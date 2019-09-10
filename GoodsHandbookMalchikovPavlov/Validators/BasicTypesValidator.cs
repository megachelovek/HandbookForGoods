using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    static class BasicTypesValidator
    {
        private static readonly Dictionary<Type, int> map = new Dictionary<Type, int>()
        {
            { typeof(string), 0 },
            { typeof(int), 1 },
            { typeof(short), 2 },
            { typeof(char), 3 },
            { typeof(byte), 4 },
            { typeof(float), 5 },
            { typeof(double), 6 }
        };
        public static bool Validate(Type type, string value, out object converted, out string error)
        {
            bool result = false;
            converted = null;
            error = null;
            if (map.ContainsKey(type))
            {
                
                int caseIndex = map[type];
                switch (caseIndex)
                {
                    case 0:
                        {
                            converted = value;
                            result = true;
                        }break;
                    case 1:
                        {
                            int temp;
                            result = int.TryParse(value, out temp);
                            if (result)
                            {
                                converted = temp;
                            }
                            else
                            {
                                error = "Value must be an integer number\n";
                            }
                        }
                        break;
                    case 2:
                        {
                            short temp;
                            result = short.TryParse(value, out temp);
                            if (result)
                            {
                                converted = temp;
                            }
                            else
                            {
                                error = "Value must be a short integer\n";
                            }
                        }
                        break;
                    case 3:
                        {
                            char temp;
                            result = char.TryParse(value, out temp);
                            if (result)
                            {
                                converted = temp;
                            }
                            else
                            {
                                error = "Value must be a character\n";
                            }
                        }
                        break;
                    case 4:
                        {
                            byte temp;
                            result = byte.TryParse(value, out temp);
                            if (result)
                            {
                                converted = temp;
                            }
                            else
                            {
                                error = "Value must be a byte\n";
                            }
                        }
                        break;
                    case 5:
                        {
                            float temp;
                            result = float.TryParse(value, out temp);
                            if (result)
                            {
                                converted = temp;
                            }
                            else
                            {
                                error = "Value must be a floating point number\n";
                            }
                        }
                        break;
                    case 6:
                        {
                            double temp;
                            result = double.TryParse(value, out temp);
                            if (result)
                            {
                                converted = temp;
                            }
                            else
                            {
                                error = "Value is not a double\n";
                            }
                        }
                        break;
                }
            }
            return result;
        }
    }
}
