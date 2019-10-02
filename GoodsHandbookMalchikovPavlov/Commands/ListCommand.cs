using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using GoodsHandbookMalchikovPavlov.Models;

namespace GoodsHandbookMalchikovPavlov.Commands
{
    /// <summary>
    /// Вывод списка продуктов
    /// </summary>
    internal  class ListCommand : ICommand
    {
        private readonly string Usage =
            "list [product type] [-full]" + Environment.NewLine;
        private readonly string ProductTypeDoesntExist =
            "Product type \"{0}\" does not exist" + Environment.NewLine +
            "List of available product types:" + Environment.NewLine;
        private readonly StringBuilder responseBuffer = new StringBuilder();
        private readonly IProductCatalog productCatalog;
        private string[] args;

        public ListCommand(IProductCatalog productCatalog, string[] args)
        {
            this.productCatalog = productCatalog;
            this.args = args;
            StringBuilder buffer = new StringBuilder(64);
            buffer.Append(ProductTypeDoesntExist);
            string[] names = productCatalog.GetProductTypeNames();
            foreach (var name in names)
            {
                buffer.Append("-");
                buffer.Append(name);
                buffer.Append(Environment.NewLine);
            }
            ProductTypeDoesntExist = buffer.ToString();
        }

        public CommandReturnCode Process(string input)
        {
            responseBuffer.Length = 0;
            if (args.Length == 1)
            {
                Debug.Assert(args[0].Equals("list", StringComparison.OrdinalIgnoreCase));
                IList<Product> products = productCatalog.GetProducts();
                ListBrief(products);
            }
            else if (args.Length == 2)
            {
                Debug.Assert(args[0].Equals("list", StringComparison.OrdinalIgnoreCase));
                if (args[1].Equals("-full"))
                {
                    IList<Product> products = productCatalog.GetProducts();
                    ListFull(products);
                }
                else
                {
                    try
                    {
                        IList<Product> products = productCatalog.GetProducts(args[1]);
                        ListBrief(products);
                    }
                    catch (ArgumentException)
                    {
                        responseBuffer.Append(string.Format(ProductTypeDoesntExist, args[1]));
                    }
                }
            }
            else if (args.Length == 3)
            {
                Debug.Assert(args[0].Equals("list", StringComparison.OrdinalIgnoreCase));
                if (args[2].Equals("-full"))
                {
                        try
                        {
                            IList<Product> products = productCatalog.GetProducts(args[1]);
                            ListFull(products);
                        }
                        catch (ArgumentException)
                        {
                            responseBuffer.Append(string.Format(ProductTypeDoesntExist, args[1]));
                        }
                }
                else
                {
                    responseBuffer.Append(Usage);
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

        private void ListBrief(IList<Product> products)
        {
            if (products.Count > 0)
            {
                responseBuffer.Append("List of products:");
                responseBuffer.Append(Environment.NewLine);
                foreach (var p in products)
                {
                    Type productType = p.GetType();
                    PropertyInfo info = productType.GetProperty("Id");
                    responseBuffer.Append(string.Format("Id - {0,-5} Type - {1,-15} Name - {2}", info.GetValue(p), Misc.GetTypeName(productType), p.Name));
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
                    Type productType = p.GetType();
                    responseBuffer.Append(Misc.GetTypeName(productType));
                    responseBuffer.Append(Environment.NewLine);
                    PropertyInfo[] properties = productType.GetProperties();
                    Misc.SortProperties(properties, productType);
                    foreach (var prop in properties)
                    {
                        responseBuffer.Append(string.Format("\t{0,-15} - {1,-20}", Misc.GetPropertyName(prop), prop.GetValue(p).ToString()));
                        responseBuffer.Append(Environment.NewLine);
                    }
                }
            }
        }
    }
}
