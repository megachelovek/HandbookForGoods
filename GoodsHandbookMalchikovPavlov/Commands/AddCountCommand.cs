using System;
using System.Text;
using System.Diagnostics;
namespace GoodsHandbookMalchikovPavlov.Commands
{
    /// <summary>
    /// Команда добавления количества продуктов
    /// </summary>
    internal  class AddCountCommand : ICommand
    {
        private readonly string Usage = "usage: add-count \"product id\" \"count\"" + Environment.NewLine;
        private readonly StringBuilder responseBuffer = new StringBuilder();
        private readonly IProductCatalog productCatalog;
        private string[] args;

        public AddCountCommand(IProductCatalog productCatalog, string[] args)
        {
            this.productCatalog = productCatalog;
            this.args = args;
        }

        public CommandReturnCode Process(string input)
        {
            responseBuffer.Length = 0;
            if (args.Length == 3)
            {
                Debug.Assert(args[0].Equals("add-count", StringComparison.OrdinalIgnoreCase));

                int id;
                bool isIdValid = int.TryParse(args[1], out id);
                if (isIdValid)
                {
                    int countToAdd;
                    bool isCountToAddValid = int.TryParse(args[2], out countToAdd) && countToAdd > 0;
                    if (isCountToAddValid)
                    {
                        isIdValid = productCatalog.DoesProductExist(id);
                        if (isIdValid)
                        {
                            productCatalog.AddProductCount(id, countToAdd);
                            responseBuffer.Append(string.Format("\"Count\" of product with Id = \"{0}\" increased by \"{1}\"",id, countToAdd));
                            responseBuffer.Append(Environment.NewLine);
                        }
                    }
                    else
                    {
                        responseBuffer.Append(string.Format("\"count\" must be an integer number from 0 to {0}", int.MaxValue));
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
                responseBuffer.Append(Usage);
            }
            return CommandReturnCode.Done;
        }

        public string GetLastResponse()
        {
            return responseBuffer.ToString();
        }
    }
}
