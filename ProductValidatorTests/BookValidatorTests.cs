using GoodsHandbookMalchikovPavlov;
using GoodsHandbookMalchikovPavlov.Models;
using GoodsHandbookMalchikovPavlov.Validators;
using NUnit.Framework;

namespace YooperTests
{
    public class BookPartValidatorTests
    {
        private ProductValidatorManager productValidatorManager;

        [SetUp]
        public void Setup()
        {
            IProductCatalog productCatalog = new ProductCatalog("product_data");
            productValidatorManager = productCatalog.GetValidator();
        }

        /// ValidateAuthor
        [TestCase("aaa1")]
        [TestCase("1aaa")]
        [TestCase("a1aa")]
        public void TestValidateAuthor_ContainsDigits_ReturnFalse(string value)
        {
            Assume.That(value.Length, Is.GreaterThanOrEqualTo(BookValidator.MinAuthorLength));
            Assume.That(value.Length, Is.LessThanOrEqualTo(BookValidator.MaxAuthorLength));
            var result =
                productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("AuthorFirstName"), value);
            Assert.That(!result);
        }

        [TestCase("a-aa")]
        [TestCase("a-a-a")]
        public void TestValidateAuthor_ContainsHyphen_ReturnTrue(string value)
        {
            Assume.That(value.Length, Is.GreaterThanOrEqualTo(BookValidator.MinAuthorLength));
            Assume.That(value.Length, Is.LessThanOrEqualTo(BookValidator.MaxAuthorLength));
            var result =
                productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("AuthorFirstName"), value);
            Assert.That(result);
        }

        [TestCase("a--aa")]
        public void TestValidateAuthor_ContainsMoreThanOneAdjacentHyphen_ReturnFalse(string value)
        {
            Assume.That(value.Length, Is.GreaterThanOrEqualTo(BookValidator.MinAuthorLength));
            Assume.That(value.Length, Is.LessThanOrEqualTo(BookValidator.MaxAuthorLength));
            var result =
                productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("AuthorFirstName"), value);
            Assert.That(!result);
        }

        [TestCase("")]
        public void TestValidateAuthor_Empty_ReturnFalse(string value)
        {
            var result =
                productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("AuthorFirstName"), value);
            Assert.That(!result);
        }

        [TestCase("")]
        public void TestValidateAuthorMiddleName_Empty_ReturnTrue(string value)
        {
            var result =
                productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("AuthorMiddleName"), value);
            Assert.That(result);
        }
    }
}