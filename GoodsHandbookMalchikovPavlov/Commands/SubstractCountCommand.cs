using System;
using System.Text;
using System.Diagnostics;
namespace GoodsHandbookMalchikovPavlov.Commands
{
    /// <summary>
    /// Вычитание количества продуктов
    /// </summary>
    internal  class SubstractCountCommand : ICommand
    {
        private readonly string Usage = "usage: sub-count \"product id\" \"count\"" + Environment.NewLine;
        private readonly StringBuilder responseBuffer = new StringBuilder();
        private readonly IProductCatalog productCatalog;
        private string[] args;

        public SubstractCountCommand(IProductCatalog productCatalog, string[] args)
        {
            this.productCatalog = productCatalog;
            this.args = args;
        }

        public CommandReturnCode Process(string input)
        {
            responseBuffer.Length = 0;
            if (args.Length == 3)
            {
                Debug.Assert(args[0].Equals("sub-count", StringComparison.OrdinalIgnoreCase));
                int id;
                bool isIdValid = int.TryParse(args[1], out id);
                if (isIdValid)
                {
                    int countToSubstract;
                    bool isCountToASubstractValid = int.TryParse(args[2], out countToSubstract) && countToSubstract > 0;
                    if (isCountToASubstractValid)
                    {
                        isIdValid = productCatalog.DoesProductExist(id);
                        if (isIdValid)
                        {
                            try
                            {
                               productCatalog.SubstractProductCount(id, countToSubstract);
                               responseBuffer.Append(string.Format("Specified amount ({0}) has been successfully substracted from \"Count\" of product with Id = \"{1}\"", countToSubstract, id));
                               responseBuffer.Append(Environment.NewLine);
                            }
                            catch(InvalidOperationException)
                            {
                                responseBuffer.Append(string.Format("Cannot substract specified amount"));
                                responseBuffer.Append(Environment.NewLine);
                            }
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
