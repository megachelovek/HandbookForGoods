using System;
using System.Text;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;
using GoodsHandbookMalchikovPavlov.Validators;
namespace GoodsHandbookMalchikovPavlov.Commands
{
    internal sealed class CreateCommand : ICommand
    {
        private readonly string productTypeInputRequest =
            "Enter \"product type\"";
        private readonly string productPropertyInputRequest =
            "Enter \"{0}\"";
        private readonly string productTypeDoesntExist =
            "Product type \"{0}\" does not exist" + Environment.NewLine +
            "List of available product types:" + Environment.NewLine;
        private StringBuilder responseBuffer = new StringBuilder();
        private bool firstTimeThrough = true;
        private bool initialized = false;
        private bool inputRequested = false;
        private Product product;
        private PropertyInfo[] productProperties;
        private int propertyIndex;
        private ProductValidator validator;
        private IProductCatalog productCatalog;
        public CreateCommand(IProductCatalog productCatalog)
        {
            this.productCatalog = productCatalog;
            validator = productCatalog.GetProductValidator();
            StringBuilder buffer = new StringBuilder(64);
            buffer.Append(productTypeDoesntExist);
            string[] names = productCatalog.GetProductTypeNames();
            foreach (var name in names)
            {
                buffer.Append("-");
                buffer.Append(name);
                buffer.Append(Environment.NewLine);
            }
            productTypeDoesntExist = buffer.ToString();
        }
        public CommandReturnCode Process(string input)
        {
            responseBuffer.Length = 0;
            string[] args = InputParser.GetWords(input);
            if (firstTimeThrough)
            {
                return HandleFirstTimeThrough(args);
            }
            if (inputRequested)
            {
                if (Quit(args))
                {
                    Reset();
                    return CommandReturnCode.Done;
                }
                if (Update(input))
                {
                    Reset();
                    return CommandReturnCode.Done;
                }
            }
            SkipId();
            RequestInput();
            return CommandReturnCode.Undone;
        }
        public string GetLastResponse()
        {
            return responseBuffer.ToString();
        }
        private CommandReturnCode HandleFirstTimeThrough(string[] args)
        {
            bool success = false;
            if (args.Length == 1)
            {
                if (args[0].Equals("create", StringComparison.OrdinalIgnoreCase))
                {
                    success = true;
                }
            }
            else if (args.Length == 2)
            {
                if (args[0].Equals("create", StringComparison.OrdinalIgnoreCase))
                {
                    if (Init(args[1]))
                    {
                        success = true;
                    }
                    else
                    {
                        responseBuffer.Append(string.Format(productTypeDoesntExist, args[1]));
                    }
                }
            }
            if (success)
            {
                firstTimeThrough = false;
                SkipId();
                RequestInput();
                return CommandReturnCode.Undone;
            }
            return CommandReturnCode.Done;
        }
        private bool Init(string productTypeName)
        {
            try
            {
                product = productCatalog.GetProduct(productTypeName);
            }
            catch (ArgumentException)
            {
                return false;
            }
            Type productType = product.GetType();
            productProperties = productType.GetProperties();
            Misc.SortProperties(productProperties, productType);
            propertyIndex = 0;
            initialized = true;
            return true;
        }
        private void SkipId()
        {
            if (initialized)
            {
                if (productProperties[propertyIndex].Name.Equals("Id"))
                {
                    if ((propertyIndex + 1) < productProperties.Length)
                    {
                        propertyIndex++;
                    }
                }
            }
        }
        private void RequestInput()
        {
            if (!initialized)
            {
                responseBuffer.Append(productTypeInputRequest);
            }
            else
            {
                string propertyName = Misc.GetPropertyName(productProperties[propertyIndex]);
                responseBuffer.Append(string.Format(productPropertyInputRequest, propertyName));
            }
            inputRequested = true;
        }
        private bool Quit(string[] args)
        {
            if (args.Length == 1 && args[0].Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        private void Reset()
        {
            firstTimeThrough = true;
            initialized = false;
            inputRequested = false;
            product = null;
            productProperties = null;
            propertyIndex = 0;
        }
        private bool Update(string input)
        {
            if (!initialized)
            {
                string[] args = InputParser.GetWords(input);
                if (args.Length == 1)
                {
                    if (!Init(args[0]))
                    {
                        responseBuffer.Append(string.Format(productTypeDoesntExist, args[0]));
                    }
                }
            }
            else
            {
                PropertyInfo info = productProperties[propertyIndex];
                bool isValid = validator.Validate(product, info, input);
                if (isValid)
                {
                    info.SetValue(product, validator.GetLastConvertedValue());
                    if ((propertyIndex + 1) < productProperties.Length)
                    {
                        propertyIndex++;
                    }
                    else
                    {
                        productCatalog.AddProduct(product);
                        return true;
                    }
                }
                else
                {
                    responseBuffer.Append(validator.GetLastErrorMessage());
                }
            }
            return false;
        }
    }
}
