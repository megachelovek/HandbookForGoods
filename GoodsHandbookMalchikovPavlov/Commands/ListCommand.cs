using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Resources;
using System.Text;
using GoodsHandbookMalchikovPavlov.Models;
using GoodsHandbookMalchikovPavlov.Properties;

namespace GoodsHandbookMalchikovPavlov.Commands
{
    /// <summary>
    ///     Вывод списка продуктов
    /// </summary>
    internal class ListCommand : ICommand
    {
        private readonly IProductCatalog productCatalog;
        private readonly string productTypeDoesntExist;
        private readonly StringBuilder responseBuffer = new StringBuilder();
        private readonly ResourceManager resourceManager = new ResourceManager(typeof(Resources));
        private readonly string usage;

        public ListCommand(IProductCatalog productCatalog, string[] args)
        {
            productTypeDoesntExist = resourceManager.GetString("LIST_PRODUCTTYPEDOESNTEXIST");
            usage = resourceManager.GetString("LIST_USAGE");
            this.productCatalog = productCatalog;
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

        public CommandReturnCode Process(string[] args)
        {
            responseBuffer.Length = 0;
            if (args.Length == 1)
            {
                Debug.Assert(args[0].Equals("list", StringComparison.OrdinalIgnoreCase));
                var products = productCatalog.GetProducts();
                ListBrief(products);
            }
            else if (args.Length == 2)
            {
                Debug.Assert(args[0].Equals("list", StringComparison.OrdinalIgnoreCase));
                if (args[1].Equals("-full"))
                {
                    var products = productCatalog.GetProducts();
                    ListFull(products);
                }
                else
                {
                    try
                    {
                        var products = productCatalog.GetProducts(args[1]);
                        ListBrief(products);
                    }
                    catch (ArgumentException)
                    {
                        responseBuffer.Append(string.Format(productTypeDoesntExist, args[1]));
                    }
                }
            }
            else if (args.Length == 3)
            {
                Debug.Assert(args[0].Equals("list", StringComparison.OrdinalIgnoreCase));
                if (args[2].Equals("-full"))
                    try
                    {
                        var products = productCatalog.GetProducts(args[1]);
                        ListFull(products);
                    }
                    catch (ArgumentException)
                    {
                        responseBuffer.Append(string.Format(productTypeDoesntExist, args[1]));
                    }
                else
                    responseBuffer.Append(usage);
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

        private void ListBrief(IList<Product> products)
        {
            if (products.Count > 0)
            {
                responseBuffer.Append("List of products:");
                responseBuffer.Append(Environment.NewLine);
                foreach (var p in products)
                {
                    var productType = p.GetType();
                    var info = productType.GetProperty("Id");
                    responseBuffer.Append(string.Format("Id - {0,-5} Type - {1,-15} Name - {2}", info.GetValue(p),
                        Misc.GetTypeName(productType), p.Name));
                    responseBuffer.Append(Environment.NewLine);
                }
            }
        }

        private void ListFull(IList<Product> products)
        {
            if (products.Count > 0)
            {
                responseBuffer.Append("List of products:");
                responseBuffer.Append(Environment.NewLine);
                foreach (var p in products)
                {
                    var productType = p.GetType();
                    responseBuffer.Append(Misc.GetTypeName(productType));
                    responseBuffer.Append(Environment.NewLine);
                    var properties = productType.GetProperties();
                    Misc.SortProperties(properties, productType);
                    foreach (var prop in properties)
                    {
                        responseBuffer.Append(string.Format("\t{0,-15} - {1,-20}", Misc.GetPropertyName(prop),
                            prop.GetValue(p)));
                        responseBuffer.Append(Environment.NewLine);
                    }
                }
            }
        }
    }
}