using System;
using System.Collections.Generic;
using System.Text;
using GoodsHandbookMalchikovPavlov.Model;
namespace GoodsHandbookMalchikovPavlov
{
    sealed class ListCommand : ICommand
    {
        private const string NAME = "list";
        private const string USAGE = "no usage instructions yet, sorry...";
        private StringBuilder outputBuffer = new StringBuilder();
        private List<Product> storage = null;

        public ListCommand(List<Product> storage)
        {
            this.storage = storage;
        }
        
        public static string GetName()
        {
            return NAME;
        }
        public bool ProcessInput(string input, out string output, out bool attention)
        {
            outputBuffer.Length = 0;
            if (input.Equals(NAME))
            {
                outputBuffer.Append("List of stored product records:\n");
                foreach (Product product in storage)
                {
                    Type productType = product.GetType();
                    string productName = ReflectionMisc.GetTypeName(productType);

                    outputBuffer.Append(String.Format("Product name: \"{0}\"\n", productName));
                    foreach(var info in productType.GetProperties())
                    {
                        string name = ReflectionMisc.GetPropertyName(info);
                        outputBuffer.Append(String.Format("Property name: \"{0}\" ", name));
                        object value = info.GetValue(product);
                        string valueAsString;
                        if (info.PropertyType.Equals(typeof(string)))
                        {
                            valueAsString = (string)value;
                        }
                        else
                        {
                            valueAsString = value.ToString();
                        }
                        outputBuffer.Append(String.Format("Property value: \"{0}\"\n", valueAsString));
                    }
                    outputBuffer.Append("\n");
                }
            }
           
            output = outputBuffer.ToString();
            attention = false;
            return true;
        }
        public bool ProcessCtrlCombinations(ConsoleKeyInfo keyInfo, out string output, out bool attention)
        {
            throw new NotImplementedException();
        }

        public string GetCommandName()
        {
            return NAME;
        }

        public string GetCommandUsageText()
        {
            return USAGE;
        }

    }
}
