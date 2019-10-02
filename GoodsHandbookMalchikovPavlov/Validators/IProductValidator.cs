using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;
namespace GoodsHandbookMalchikovPavlov.Validators
{
    interface IProductValidator
    {
        bool Validate(Product product, PropertyInfo propertyInfo, string propertyValue,
            out string errorMsg, out object convertedValue);
    }
}
