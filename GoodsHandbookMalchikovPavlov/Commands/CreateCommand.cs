using System;
using System.Reflection;
using System.Resources;
using System.Text;
using GoodsHandbookMalchikovPavlov.Models;
using GoodsHandbookMalchikovPavlov.Properties;
using GoodsHandbookMalchikovPavlov.Validators;

namespace GoodsHandbookMalchikovPavlov.Commands
{
    /// <summary>
    ///     Команда добавления продукта в список
    /// </summary>
    internal sealed class CreateCommand : ICommand
    {
        private readonly IProductCatalog productCatalog;

        private readonly string productPropertyInputRequest =
            "Enter \"{0}\"";

        private readonly string productTypeInputRequest =
            "Enter \"product type\"";

        private readonly StringBuilder responseBuffer = new StringBuilder();
        private readonly ProductValidatorManager validator;
        private string[] args;
        private bool firstTimeThrough = true;
        private bool initialized;
        private bool inputRequested;
        private Product product;
        private PropertyInfo[] productProperties;
        private readonly string productTypeDoesntExist;
        private int propertyIndex;
        private readonly ResourceManager resourceManager = new ResourceManager(typeof(Resources));


        public CreateCommand(IProductCatalog productCatalog, string[] args)
        {
            productTypeDoesntExist = resourceManager.GetString("CREATE_PRODUCTTYPEDOESNTEXIST");
            this.args = args;
            this.productCatalog = productCatalog;
            validator = productCatalog.GetValidator();
            var buffer = new StringBuilder(64);
            buffer.Append(productTypeDoesntExist);
            var names = productCatalog.GetTypeNames();
            foreach (var name in names)
            {
                buffer.Append("-");
                buffer.Append(name);
                buffer.Append(Environment.NewLine);
            }

            productTypeDoesntExist = buffer.ToString();
        }

        /// <summary>
        ///     Процесс добавления продукта
        /// </summary>
        /// <param name="input">Новые аргументы</param>
        /// <returns>Код возврата</returns>
        public CommandReturnCode Process(string[] args)
        {
            responseBuffer.Length = 0;
            if (inputRequested)
            {
                if (Quit(args))
                {
                    Reset();
                    return CommandReturnCode.Done;
                }

                if (Update(args))
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

        private bool Init(string productTypeName)
        {
            try
            {
                product = productCatalog.Get(productTypeName);
            }
            catch (ArgumentException)
            {
                return false;
            }

            var productType = product.GetType();
            productProperties = productType.GetProperties();
            Misc.SortProperties(productProperties, productType);
            propertyIndex = 0;
            initialized = true;
            return true;
        }

        /// <summary>
        ///     Пропустить идентификатор
        /// </summary>
        private void SkipId()
        {
            if (initialized)
                if (productProperties[propertyIndex].Name.Equals("Id"))
                    if (propertyIndex + 1 < productProperties.Length)
                        propertyIndex++;
        }

        /// <summary>
        ///     Запрос ввода
        /// </summary>
        private void RequestInput()
        {
            if (!initialized)
            {
                responseBuffer.Append(productTypeInputRequest);
            }
            else
            {
                var propertyName = Misc.GetPropertyName(productProperties[propertyIndex]);
                responseBuffer.Append(string.Format(productPropertyInputRequest, propertyName));
            }

            inputRequested = true;
        }

        /// <summary>
        ///     Выход из процесса команды создания
        /// </summary>
        /// <param name="args">Ожидание команды выхода</param>
        /// <returns></returns>
        private bool Quit(string[] args)
        {
            if (args != null)
                if (args.Length == 1 && args[0].Equals("quit", StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }

        /// <summary>
        ///     Сброс параметров
        /// </summary>
        private void Reset()
        {
            firstTimeThrough = true;
            initialized = false;
            inputRequested = false;
            product = null;
            productProperties = null;
            propertyIndex = 0;
        }

        /// <summary>
        ///     Заполнение полей продукта
        /// </summary>
        /// <param name="input">Ввод информации полей</param>
        /// <returns></returns>
        private bool Update(string[] args)
        {
            if (!initialized)
            {
                if (args.Length == 1)
                    if (!Init(args[0]))
                        responseBuffer.Append(string.Format(productTypeDoesntExist, args[0]));
            }
            else
            {
                var info = productProperties[propertyIndex];
                var isValid = validator.Validate(product, info, args[0]);
                if (isValid)
                {
                    info.SetValue(product, validator.GetLastConvertedValue());
                    if (propertyIndex + 1 < productProperties.Length)
                    {
                        propertyIndex++;
                    }
                    else
                    {
                        productCatalog.Add(product);
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