using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using GoodsHandbookMalchikovPavlov.Models;
using GoodsHandbookMalchikovPavlov.Validators;

[assembly: InternalsVisibleTo("ProductCatalogTests")]
[assembly: InternalsVisibleTo("ProductValidatorTests")]

namespace GoodsHandbookMalchikovPavlov
{
    [Serializable]
    internal class ProductCatalogData
    {
        public string[] ProductTypeNames =
        {
            "Appliances",
            "Book",
            "Toy"
        };

        public ProductCatalogData()
        {
            ProductPropertyValidValues.Add("BookGenre", new[]
            {
                "Fairy Tale",
                "Mystic",
                "Fantasy",
                "Detective",
                "Psychology",
                "Popular Science",
                "Educational",
                "Sentimental Novel",
                "Teenage Prose"
            });
            ProductPropertyValidValues.Add("ToyCategory", new[]
            {
                "Educational",
                "Video Game",
                "Doll",
                "Electronic"
            });
            ProductPropertyValidValues.Add("ToySex", new[]
            {
                "Male",
                "Female",
                "Any"
            });
            ProductPropertyValidValues.Add("AppliancesCategory", new[]
            {
                "Refrigerator",
                "Stove",
                "Teapot"
            });
        }

        public int MaxId { get; set; }
        public Dictionary<int, Product> ProductMap { get; set; } = new Dictionary<int, Product>();

        public Dictionary<string, string[]> ProductPropertyValidValues { get; set; } =
            new Dictionary<string, string[]>();
    }

    internal sealed class ProductCatalog : IProductCatalog
    {
        private readonly string catalogDataFileName;
        private ProductCatalogData catalogData;
        private readonly ProductValidatorManager productValidatorManager;

        public ProductCatalog(string catalogDataFileName)
        {
            this.catalogDataFileName = catalogDataFileName;
            var success = File.Exists(catalogDataFileName);
            CreateCatalogData(success);
            productValidatorManager = new ProductValidatorManager(this);
        }

        public bool IsExist(int productId)
        {
            return catalogData.ProductMap.ContainsKey(productId);
        }

        public void Add(Product product)
        {
            product.Id = catalogData.MaxId++;
            catalogData.ProductMap.Add(product.Id, product);
            WriteCatalogDataToFile();
        }

        public void Delete(int productId)
        {
            if (IsExist(productId))
            {
                catalogData.ProductMap.Remove(productId);
                WriteCatalogDataToFile();
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public void Update(Product product)
        {
            if (IsExist(product.Id))
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

        public Product Get(int productId)
        {
            if (IsExist(productId))
                return catalogData.ProductMap[productId];
            throw new ArgumentException();
        }

        public Product Get(string productType)
        {
            if (productType.Equals("BOOK", StringComparison.OrdinalIgnoreCase))
                return new Book();
            if (productType.Equals("TOY", StringComparison.OrdinalIgnoreCase))
                return new Toy();
            if (productType.Equals("APPLIANCES", StringComparison.OrdinalIgnoreCase))
                return new Appliances();
            throw new ArgumentException();
        }

        public ProductValidatorManager GetValidator()
        {
            return productValidatorManager;
        }

        public IEnumerable<string> GetTypeNames()
        {
            return catalogData.ProductTypeNames;
        }

        public IEnumerable<string> GetPropertyValidValues(PropertyInfo propertyInfo)
        {
            var key = propertyInfo.DeclaringType.Name + propertyInfo.Name;
            if (catalogData.ProductPropertyValidValues.ContainsKey(key))
                return catalogData.ProductPropertyValidValues[key];
            throw new ArgumentException();
        }

        public IList<Product> GetProducts(string productTypeToFilterBy = null)
        {
            Type type = null;
            if (productTypeToFilterBy != null)
                try
                {
                    type = Get(productTypeToFilterBy).GetType();
                }
                catch (ArgumentException)
                {
                    throw new ArgumentException();
                }

            var result = new List<Product>();
            foreach (var p in catalogData.ProductMap.Values)
                if (type == null || p.GetType().Equals(type))
                    result.Add(p);
            return result;
        }

        public void AddCount(int productId, int countToAdd)
        {
            if (IsExist(productId))
            {
                catalogData.ProductMap[productId].Count += countToAdd;
                WriteCatalogDataToFile();
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public void SubstractCount(int productId, int countToSubstract)
        {
            if (IsExist(productId))
            {
                var product = catalogData.ProductMap[productId];
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

        public Product GetItem(int productId)
        {
            if (IsExist(productId))
            {
                var product = catalogData.ProductMap[productId];
                return product;
            }

            return null;
        }

        public void ClearListCatalog()
        {
            var success = File.Exists(catalogDataFileName);
            if (success)
            {
                File.Delete(catalogDataFileName);
                CreateCatalogData(false);
            }
            else
            {
                CreateCatalogData(false);
            }
        }

        private void CreateCatalogData(bool success)
        {
            if (success)
            {
                var fs = File.Open(catalogDataFileName, FileMode.Open, FileAccess.Read);
                success = fs.Length != 0;
                if (success)
                {
                    var formatter = new BinaryFormatter();
                    catalogData = (ProductCatalogData) formatter.Deserialize(fs);
                }

                fs.Dispose();
            }
            else
            {
                catalogData = new ProductCatalogData();
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