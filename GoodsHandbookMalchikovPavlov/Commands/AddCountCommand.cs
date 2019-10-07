using System;
using System.Diagnostics;
using System.Resources;
using System.Text;
using GoodsHandbookMalchikovPavlov.Properties;

namespace GoodsHandbookMalchikovPavlov.Commands
{
    /// <summary>
    ///     Команда добавления количества продуктов
    /// </summary>
    internal class AddCountCommand : ICommand
    {
        private readonly IProductCatalog productCatalog;
        private readonly StringBuilder responseBuffer = new StringBuilder();
        private readonly string usage;
        private string[] args;
        private readonly ResourceManager resourceManager = new ResourceManager(typeof(Resources));

        public AddCountCommand(IProductCatalog productCatalog, string[] args)
        {
            usage = resourceManager.GetString("ADDCOUNT_USAGE");
            this.productCatalog = productCatalog;
            this.args = args;
        }

        public CommandReturnCode Process(string[] args)
        {
            responseBuffer.Length = 0;
            if (args.Length == 3)
            {
                Debug.Assert(args[0].Equals("add-count", StringComparison.OrdinalIgnoreCase));

                int id;
                var isIdValid = int.TryParse(args[1], out id);
                if (isIdValid)
                {
                    int countToAdd;
                    var isCountToAddValid = int.TryParse(args[2], out countToAdd) && countToAdd > 0;
                    if (isCountToAddValid)
                    {
                        isIdValid = productCatalog.IsExist(id);
                        if (isIdValid)
                        {
                            productCatalog.AddCount(id, countToAdd);
                            responseBuffer.Append(
                                $"\"Count\" of product with Id = {id} increased by {countToAdd}");
                            responseBuffer.Append(Environment.NewLine);
                        }
                    }
                    else
                    {
                        responseBuffer.Append(string.Format("\"count\" must be an integer number from 0 to {0}",
                            int.MaxValue));
                        responseBuffer.Append(Environment.NewLine);
                    }
                }

                if (!isIdValid)
                {
                    responseBuffer.Append(string.Format("Product with Id = \"{0}\" does not exist", args[1]));
                    responseBuffer.Append(Environment.NewLine);
                }
            }
            else
            {
                responseBuffer.Append(usage);
            }

            return CommandReturnCode.Done;
        }

        public string GetLastResponse()
        {
            return responseBuffer.ToString();
        }
    }
}