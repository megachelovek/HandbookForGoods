using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Model;
using GoodsHandbookMalchikovPavlov.Validators;
namespace GoodsHandbookMalchikovPavlov.Commands
{
    sealed class CreateCommand : ICommand
    {
        private const string NAME = "create";
        private const string USAGE = "create command is designed to be used in interactive fashion so just follow what it's saying";
        private bool sessionStarted;
        private bool inputRequested = false;
        private bool attemptingToExit = false;
        private StringBuilder outputBuffer = new StringBuilder();
        private readonly Dictionary<string, Type> nameToProductMap;
        private readonly Dictionary<Type, ProductValidator> productToValidatorMap;
        private ProductValidator validator;
        private Type productType;
        private Product product;
        PropertyInfo[] propertyInfos;
        private int propertyIndex;
        private string productName;
        private List<Product> storage = null;

        public CreateCommand(List<Product> storage, Dictionary<string, Type> nameToProductMap,
           Dictionary<Type, ProductValidator> productToValidatorMap)
        {
            this.storage = storage;
            this.nameToProductMap = nameToProductMap;
            this.productToValidatorMap = productToValidatorMap;
        }
        public bool ProcessInput(string input, out string output, out bool attention)
        {
            attention = false;
            outputBuffer.Length = 0;
            if (attemptingToExit)
            {
                attemptingToExit = false;
            }

            if (!sessionStarted)
            {
                if (!input.Equals(NAME))
                {
                    output = GetCommandUsageText();
                    return true;
                }
                else
                {
                    sessionStarted = true;
                }
            }

            if (productType == null)
            {
                if (inputRequested && nameToProductMap.ContainsKey(input))
                {
                    productType = nameToProductMap[input];
                    propertyInfos = productType.GetProperties();
                    propertyIndex = 0;
                    product = (Product)Activator.CreateInstance(productType);
                    productName = productType.Name;
                    validator = productToValidatorMap[productType];
                    AppendFieldRequest();
                }
                else
                {
                    AppendProductNameRequest();
                    inputRequested = true;
                }
            }
            else
            {
                PropertyInfo info = propertyInfos[propertyIndex];
                if (validator.Validate(productType, info, input))
                {
                    info.SetValue(product, validator.GetLastProperty());

                    if ((propertyIndex + 1) < propertyInfos.Length)
                    {
                        propertyIndex++;
                    }
                    else
                    {
                        storage.Add(product);
                        product = null;
                        productType = null;
                        propertyIndex = 0;
                        propertyInfos = null;

                        sessionStarted = false;
                        inputRequested = false;
                        outputBuffer.Append("Product has been successfully created");
                        output = outputBuffer.ToString();
                        return true;
                    }

                }
                else
                {
                    outputBuffer.Append(validator.GetLastError());
                    attention = true;
                }

                AppendFieldRequest();
            }
            output = outputBuffer.ToString();
            return false;

        }
        public bool ProcessCtrlCombinations(ConsoleKeyInfo keyInfo, out string output, out bool attention)
        {
            attention = false;
            output = "";
            if ((keyInfo.Key == ConsoleKey.Q))
            {
                if (attemptingToExit)
                {
                    product = null;
                    productType = null;
                    propertyIndex = 0;
                    propertyInfos = null;

                    sessionStarted = false;
                    inputRequested = false;
                    return true;
                }
                output = "Go back? Confirm action by pressing CTRL+Q again";
                attention = true;
                attemptingToExit = true;

            }
            else if (attemptingToExit)
            {
                attemptingToExit = false;
            }
            return false;
        }
        public string GetCommandName()
        {
            return NAME;
        }

        public string GetCommandUsageText()
        {
            return USAGE;
        }

        private void AppendProductNameRequest()
        {
            outputBuffer.Append("List of product names:\n");
            foreach (var pair in nameToProductMap)
            {
                outputBuffer.Append("- ");
                outputBuffer.Append(pair.Key);
                outputBuffer.Append("\n");
            }
            outputBuffer.Append("Enter product name:");
        }

        private void AppendFieldRequest()
        {
            //string propertyDisplayedName = ((ProductPropertyNameAttribute)propertyInfos[propertyIndex].GetCustomAttribute(typeof(ProductPropertyNameAttribute))).Name;
            outputBuffer.Append(string.Format("Enter value for the property \"{0}\" ( {1} out of {2} total fields of product \"{3}\" )", propertyInfos[propertyIndex].Name, propertyIndex + 1, propertyInfos.Length, productName));
        }

        public static string GetName()
        {
            return NAME;
        }
    }
}
