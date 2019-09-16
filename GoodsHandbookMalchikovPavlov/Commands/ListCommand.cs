using System;
using System.Collections.Generic;
using System.Text;
using GoodsHandbookMalchikovPavlov.Model;

namespace GoodsHandbookMalchikovPavlov
{
    internal sealed class ListCommand : ICommand
    {
        private const string NAME = "list";
        private const string USAGE = "no usage instructions yet, sorry...";
        private readonly StringBuilder outputBuffer = new StringBuilder();
        private readonly List<Product> storage;

        public ListCommand(List<Product> storage)
        {
            this.storage = storage;
        }

        public bool ProcessInput(string input, out string output, out bool attention)
        {
            outputBuffer.Length = 0;
            if (input.Equals(NAME))
            {
                outputBuffer.Append("List of stored product records:\n");
                foreach (var product in storage)
                {
                    var productType = product.GetType();
                    var productName = ReflectionMisc.GetTypeName(productType);

                    outputBuffer.Append(string.Format("Product name: \"{0}\"\n", productName));
                    foreach (var info in productType.GetProperties())
                    {
                        var name = ReflectionMisc.GetPropertyName(info);
                        outputBuffer.Append(string.Format("Property name: \"{0}\" ", name));
                        var value = info.GetValue(product);
                        string valueAsString;
                        if (info.PropertyType.Equals(typeof(string)))
                            valueAsString = (string) value;
                        else
                            valueAsString = value.ToString();
                        outputBuffer.Append(string.Format("Property value: \"{0}\"\n", valueAsString));
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

        public static string GetName()
        {
            return NAME;
        }
    }
}