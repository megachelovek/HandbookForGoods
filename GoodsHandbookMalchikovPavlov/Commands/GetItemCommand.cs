using System;
using System.Diagnostics;
using System.Resources;
using System.Text;
using GoodsHandbookMalchikovPavlov.Models;
using GoodsHandbookMalchikovPavlov.Properties;

namespace GoodsHandbookMalchikovPavlov.Commands
{
    /// <summary>
    ///     Удаление продукта из списка
    /// </summary>
    internal class GetItemCommand : ICommand
    {
        private readonly IProductCatalog productCatalog;
        private readonly StringBuilder responseBuffer = new StringBuilder();
        private readonly string usage;
        private string[] args;
        private readonly ResourceManager resourceManager = new ResourceManager(typeof(Resources));

        public GetItemCommand(IProductCatalog productCatalog, string[] args)
        {
            usage = resourceManager.GetString("GETITEM_USAGE");
            this.productCatalog = productCatalog;
            this.args = args;
        }

        public CommandReturnCode Process(string[] args)
        {
            responseBuffer.Length = 0;
            if (args.Length == 2)
            {
                Debug.Assert(args[0].Equals("get-item", StringComparison.OrdinalIgnoreCase));

                int id;
                var isIdValid = int.TryParse(args[1], out id);
                if (isIdValid)
                {
                    isIdValid = productCatalog.IsExist(id);
                    if (isIdValid)
                    {
                        Product product= productCatalog.GetItem(id);
                        responseBuffer.Append($"Product Id = {product.Id}\nName={product.Name}\nPrice={product.Price}\nCount={product.Count}\nUnit={product.Unit}");
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