using System;
using System.Diagnostics;
using System.Resources;
using System.Text;
using GoodsHandbookMalchikovPavlov.Properties;

namespace GoodsHandbookMalchikovPavlov.Commands
{
    /// <summary>
    /// Вычитание количества продуктов
    /// </summary>
    internal class SubstractCountCommand : ICommand
    {
        private readonly IProductCatalog productCatalog;
        private readonly StringBuilder responseBuffer = new StringBuilder();
        private readonly string usage;
        private string[] args;
        private readonly ResourceManager resourceManager = new ResourceManager(typeof(Resources));

        public SubstractCountCommand(IProductCatalog productCatalog, string[] args)
        {
            usage = resourceManager.GetString("SUBSTRACTCOUNT_USAGE");
            this.productCatalog = productCatalog;
            this.args = args;
        }

        public CommandReturnCode Process(string[] args)
        {
            responseBuffer.Length = 0;
            if (args.Length == 3)
            {
                Debug.Assert(args[0].Equals("sub-count", StringComparison.OrdinalIgnoreCase));
                int id;
                var isIdValid = int.TryParse(args[1], out id);
                if (isIdValid)
                {
                    int countToSubstract;
                    var isCountToASubstractValid = int.TryParse(args[2], out countToSubstract) && countToSubstract > 0;
                    if (isCountToASubstractValid)
                    {
                        isIdValid = productCatalog.IsExist(id);
                        if (isIdValid)
                            try
                            {
                                productCatalog.SubstractCount(id, countToSubstract);
                                responseBuffer.Append(string.Format(
                                    "Specified amount ({0}) has been successfully substracted from \"Count\" of product with Id = \"{1}\"",
                                    countToSubstract, id));
                                responseBuffer.Append(Environment.NewLine);
                            }
                            catch (InvalidOperationException)
                            {
                                responseBuffer.Append("Cannot substract specified amount");
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