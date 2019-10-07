using GoodsHandbookMalchikovPavlov;
using GoodsHandbookMalchikovPavlov.Models;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        private IProductCatalog productTestCatalog;

        [SetUp]
        public void Setup()
        {
            productTestCatalog = new ProductCatalog("testDataCatalog");
            productTestCatalog.ClearListCatalog();
        }

        [Test]
        public void GetProductCatalogTests()
        {
            Product product = new Toy();
            product.Name = "Barbie1";
            product.Count = 10;
            product.Price = 9999;
            product.Unit = "��";
            productTestCatalog.Add(product);
            
            ///�������� Add
            Assert.AreEqual(productTestCatalog.GetProducts().Count, 1);
        }

        [Test]
        public void AddProductCatalogTests()
        {
            Product product = new Toy();
            product.Name = "Barbie2";
            product.Count = 10;
            product.Price = 9999;
            product.Unit = "��";
            productTestCatalog.Add(product);

            ///�������� Get
            var product2 = productTestCatalog.Get(0);
            Assert.AreEqual(product2.Name, "Barbie2");
        }

        [Test]
        public void DoesProductExistCatalogTests()
        {
            Product product = new Toy();
            product.Name = "Barbie3";
            product.Count = 10;
            product.Price = 9999;
            product.Unit = "��";
            productTestCatalog.Add(product);

            bool isExist;
            isExist = productTestCatalog.IsExist(0);

            ///�������� IsExist
            Assert.True(isExist);
        }

        [Test]
        public void UpdateProductCatalogTests()
        {
            Product product = new Toy();
            product.Name = "Barbie1";
            product.Count = 10;
            product.Price = 9999;
            product.Unit = "��";
            productTestCatalog.Add(product);

            Product productNew2 = new Toy();
            productNew2.Name = "Barbie v2.0";
            productNew2.Count = 10;
            productNew2.Price = 9999;
            productNew2.Unit = "��";
            productNew2.Id = 0;
            productTestCatalog.Update(productNew2);

            ///�������� Update
            Assert.AreEqual(productTestCatalog.Get(0).Name, "Barbie v2.0");
        }

        [Test]
        public void ApplianceProductCatalogTests()
        {
            Product product = new Toy();
            product.Name = "Barbie1";
            product.Count = 10;
            product.Price = 9999;
            product.Unit = "��";
            productTestCatalog.Add(product);

            Product productNew3 = new Appliances();
            productNew3.Name = "Samsung";
            productNew3.Count = 20;
            productNew3.Price = 30000;
            productNew3.Unit = "��";
            productTestCatalog.Add(productNew3);

            ///�������� GetProducts
            var products = productTestCatalog.GetProducts();
            Assert.AreEqual(products.Count, 2);
            Assert.AreEqual(products[0].Name, "Barbie1");
        }

        [Test]
        public void AddProductCountCatalogTests()
        {
            Product product = new Toy();
            product.Name = "Barbie1";
            product.Count = 10;
            product.Price = 9999;
            product.Unit = "��";
            productTestCatalog.Add(product);

            ///�������� AddCount
            productTestCatalog.AddCount(0, 90);
            Assert.AreEqual(productTestCatalog.Get(0).Count, 100);
        }

        [Test]
        public void SubstractProductCountCatalogTests()
        {
            Product product = new Toy();
            product.Name = "Barbie1";
            product.Count = 20;
            product.Price = 9999;
            product.Unit = "��";
            productTestCatalog.Add(product);

            ///�������� SubstractCount
            productTestCatalog.SubstractCount(0, 19);
            Assert.AreEqual(productTestCatalog.Get(0).Count, 1);
        }
        
    }
}