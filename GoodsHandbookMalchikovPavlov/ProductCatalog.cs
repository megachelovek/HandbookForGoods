using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GoodsHandbookMalchikovPavlov.Models;
using GoodsHandbookMalchikovPavlov.Validators;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ProductCatalogTests")]
namespace GoodsHandbookMalchikovPavlov
{
    [Serializable]
    internal class ProductCatalogData
    {
        public int MaxId { get; set; }
        public Dictionary<int, Product> ProductMap { get; set; } = new Dictionary<int, Product>();
        public string[] ProductTypeNames = new string[]
            {
                "Appliances",
                "Book",
                "Toy"
            };
    }
    internal sealed class ProductCatalog : IProductCatalog
    {
        private readonly string catalogDataFileName;
        private ProductCatalogData catalogData;
        private readonly ProductValidator productValidator = new ProductValidator();
        public ProductCatalog(string catalogDataFileName)
        {
            this.catalogDataFileName = catalogDataFileName;
            bool success = File.Exists(catalogDataFileName);
            CreateCatalogData( success);
        }

        private void CreateCatalogData( bool success)
        {
            if (success)
            {
                var fs = File.Open(this.catalogDataFileName, FileMode.Open, FileAccess.Read);
                success = fs.Length != 0;
                if (success)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    catalogData = (ProductCatalogData) formatter.Deserialize(fs);
                }

                fs.Dispose();
            }
            else
            { 
                catalogData = new ProductCatalogData();
            }
        }

        public bool DoesProductExist(int productId)
        {
            return catalogData.ProductMap.ContainsKey(productId);
        }
        public void AddProduct(Product product)
        {
            product.Id = catalogData.MaxId++;
            catalogData.ProductMap.Add(product.Id, product);
            WriteCatalogDataToFile();
        }
        public void DeleteProduct(int productId)
        {
            if (DoesProductExist(productId))
            {
                catalogData.ProductMap.Remove(productId);
                WriteCatalogDataToFile();
            }
            else
            {
                throw new ArgumentException();
            }
        }
        public void UpdateProduct(Product product)
        {
            if (DoesProductExist(product.Id))
            {
                catalogData.ProductMap.Remove(product.Id);
                catalogData.ProductMap.Add(product.Id, product);
                WriteCatalogDataToFile();
            }
            else
            {
                throw new ArgumentException();
            }
        }
        public Product GetProduct(int productId)
        {
            if (DoesProductExist(productId))
            {
                return catalogData.ProductMap[productId];
            }
            else
            {
                throw new ArgumentException();
            }
        }
        public Product GetProduct(string productType)
        {
            if (productType.Equals("BOOK", StringComparison.OrdinalIgnoreCase))
            {
                return new Book();
            }
            else if (productType.Equals("TOY", StringComparison.OrdinalIgnoreCase))
            {
                return new Toy();
            }
            else if (productType.Equals("APPLIANCES", StringComparison.OrdinalIgnoreCase))
            {
                return new Appliances();
            }
            else
            {
                throw new ArgumentException();
            }
        }
        public ProductValidator GetProductValidator()
        {
            return productValidator;
        }
        public string[] GetProductTypeNames()
        {
            return catalogData.ProductTypeNames;
        }
        public IList<Product> GetProducts(string productTypeToFilterBy = null)
        {
            Type type = null;
            if (productTypeToFilterBy != null)
            {
                try
                {
                    type = GetProduct(productTypeToFilterBy).GetType();
                }
                catch (ArgumentException)
                {
                    throw new ArgumentException();
                }
            }
            List<Product> result = new List<Product>();
            foreach (var p in catalogData.ProductMap.Values)
            {
                if (type== null || p.GetType().Equals(type))
                {
                    result.Add(p);
                }
            }
            return result;
        }
        public void AddProductCount(int productId, int countToAdd)
        {
            if (DoesProductExist(productId))
            {
                catalogData.ProductMap[productId].Count += countToAdd;
                WriteCatalogDataToFile();
            }
            else
            {
                throw new ArgumentException();
            }
        }
        public void SubstractProductCount(int productId, int countToSubstract)
        {
            if (DoesProductExist(productId))
            {
                Product product = catalogData.ProductMap[productId];
                if (product.Count - countToSubstract >= 0)
                {
                    product.Count -= countToSubstract;
                    WriteCatalogDataToFile();
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                throw new ArgumentException();
            }
            
        }

        public void ClearListCatalog()
        {
            bool success = File.Exists(catalogDataFileName);
            if (success)
            {
                File.Delete(this.catalogDataFileName);
                CreateCatalogData(false);
            }
            else
            {
                CreateCatalogData(false);
            }
        }

        private void WriteCatalogDataToFile()
        {
            var fs = File.Open(catalogDataFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var formatter = new BinaryFormatter();
            formatter.Serialize(fs, catalogData);
            fs.Dispose();
        }
        
    }
}
