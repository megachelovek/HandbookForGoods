using System;
using System.Collections.Generic;
using NUnit.Framework;
using GoodsHandbookMalchikovPavlov;
using GoodsHandbookMalchikovPavlov.Models;

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
        public void ProductCatalogTests()
        {
            Product product = new Toy();
            product.Name = "Barbie";
            product.Count = 10;
            product.Price = 9999;
            product.Unit = "Шт";
            productTestCatalog.AddProduct(product);

            ///Проверка GetProduct
            Product product2 = productTestCatalog.GetProduct(0);
            Assert.AreEqual(product2.Name, "Barbie");

            ///Проверка AddProduct
            Assert.AreEqual(productTestCatalog.GetProducts().Count,1);

            bool isExist;
            isExist = productTestCatalog.DoesProductExist(0);

            ///Проверка DoesProductExist
            Assert.True(isExist);

            Product productNew2 = new Toy();
            productNew2.Name = "Barbie v2.0";
            productNew2.Count = 10;
            productNew2.Price = 9999;
            productNew2.Unit = "Шт";
            productNew2.Id = 0;
            productTestCatalog.UpdateProduct(productNew2);

            ///Проверка UpdateProduct
            Assert.AreEqual(productTestCatalog.GetProduct(0).Name, "Barbie v2.0");

            Product productNew3 = new Appliances();
            productNew3.Name = "Samsung";
            productNew3.Count = 20;
            productNew3.Price = 30000;
            productNew3.Unit = "Шт";
            productTestCatalog.AddProduct(productNew3);

            ///Проверка GetProducts
            IList<Product> products = productTestCatalog.GetProducts();
            Assert.AreEqual(products.Count, 2);
            Assert.AreEqual(products[0].Name, "Barbie v2.0");

            ///Проверка AddProductCount
            productTestCatalog.AddProductCount(0,90);
            Assert.AreEqual(productTestCatalog.GetProduct(0).Count, 100);

            ///Проверка SubstractProductCount
            productTestCatalog.SubstractProductCount(1, 19);
            Assert.AreEqual(productTestCatalog.GetProduct(1).Count, 1);

        }

    }
}