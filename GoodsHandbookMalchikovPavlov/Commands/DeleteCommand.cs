﻿using System;
using System.Text;
using System.Diagnostics;
namespace GoodsHandbookMalchikovPavlov.Commands
{
    internal sealed class DeleteCommand : ICommand
    {
        private readonly string Usage = "usage: delete \"product id\"" + Environment.NewLine;
        private StringBuilder responseBuffer = new StringBuilder();
        private IProductCatalog productCatalog;
        public DeleteCommand(IProductCatalog productCatalog)
        {
            this.productCatalog = productCatalog;
        }
        public CommandReturnCode Process(string input)
        {
            responseBuffer.Length = 0;
            string[] args = InputParser.GetWords(input);
            if (args.Length == 2)
            {
                Debug.Assert(args[0].Equals("delete", StringComparison.OrdinalIgnoreCase));

                int id;
                bool isIdValid = int.TryParse(args[1], out id);
                if (isIdValid)
                {
                    isIdValid = productCatalog.DoesProductExist(id);
                    if (isIdValid)
                    {
                        productCatalog.DeleteProduct(id);
                        responseBuffer.Append(string.Format("Product with Id = \"{0}\" has been successfully deleted", id));
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