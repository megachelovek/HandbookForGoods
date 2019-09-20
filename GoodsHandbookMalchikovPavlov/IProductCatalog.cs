using System.Collections.Generic;
using GoodsHandbookMalchikovPavlov.Models;
using GoodsHandbookMalchikovPavlov.Validators;
namespace GoodsHandbookMalchikovPavlov
{
    interface IProductCatalog
    {
        bool DoesProductExist(int productId);
        void AddProduct(Product product);
        void DeleteProduct(int productId);
        void UpdateProduct(Product product);
        Product GetProduct(int productId);
        Product GetProduct(string productType);
        ProductValidator GetProductValidator();
        string[] GetProductTypeNames();
        IList<Product> GetProducts(string productTypeToFilterBy = null);
        void AddProductCount(int productId, int countToAdd);
        void SubstractProductCount(int productId, int countToSubstract);
    }
}
