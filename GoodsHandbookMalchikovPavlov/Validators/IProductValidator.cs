using GoodsHandbookMalchikovPavlov.Models;
namespace GoodsHandbookMalchikovPavlov.Validators
{
    interface IProductValidator
    {
        void Reset(Product product);
        bool Validate(string propertyName, string propertyValue);
        string GetLastErrorMessage();
        object GetLastConvertedValue();
    }
}
