using GoodsHandbookMalchikovPavlov;
using GoodsHandbookMalchikovPavlov.Models;
using GoodsHandbookMalchikovPavlov.Validators;
using NUnit.Framework;

namespace YooperTests
{
    public class ProductValidatorTests
    {
        private ProductValidatorManager productValidatorManager;

        [SetUp]
        public void Setup()
        {
            IProductCatalog productCatalog = new ProductCatalog("product_data");
            productValidatorManager = productCatalog.GetValidator();
        }

        /// Product name
        [TestCase("aaa")]
        public void TestValidateName_ShorterThanMinLength_ReturnFalse(string value)
        {
            Assume.That(value.Length, Is.LessThan(ProductValidator.MinNameLength));
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Name"), value);
            Assert.That(!result);
        }

        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void ValidateName_LongerThanMaxLength_ReturnFalse(string value)
        {
            Assume.That(value.Length, Is.GreaterThan(ProductValidator.MaxNameLength));
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Name"), value);
            Assert.That(!result);
        }

        [TestCase("aaaa")]
        [TestCase("aaaaa")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void ValidateName_LengthInBetweenMinAndMaxLengths_ReturnTrue(string value)
        {
            Assume.That(value.Length, Is.GreaterThanOrEqualTo(ProductValidator.MinNameLength));
            Assume.That(value.Length, Is.LessThanOrEqualTo(ProductValidator.MaxNameLength));
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Name"), value);
            Assert.That(result);
        }

        [TestCase("aaa bbb ccc dddd eeee ffff")]
        public void TestValidateName_ContainsMoreThanFiveWords_ReturnFalse(string value)
        {
            Assume.That(value.Length, Is.GreaterThanOrEqualTo(ProductValidator.MinNameLength));
            Assume.That(value.Length, Is.LessThanOrEqualTo(ProductValidator.MaxNameLength));
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Name"), value);
            Assert.That(!result);
        }

        [TestCase("aaa'''''")]
        public void TestValidateName_MoreThanFourAdjacentPunctuationMarks_ReturnFalse(string value)
        {
            Assume.That(value.Length, Is.GreaterThanOrEqualTo(ProductValidator.MinNameLength));
            Assume.That(value.Length, Is.LessThanOrEqualTo(ProductValidator.MaxNameLength));
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Name"), value);
            Assert.That(!result);
        }

        [TestCase("aaa  aa")]
        public void TestValidateName_ContainsDoubleWhiteSpace_ReturnFalse(string value)
        {
            Assume.That(value.Length, Is.GreaterThanOrEqualTo(ProductValidator.MinNameLength));
            Assume.That(value.Length, Is.LessThanOrEqualTo(ProductValidator.MaxNameLength));
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Name"), value);
            Assert.That(!result);
        }

        [TestCase("----")]
        public void TestValidateName_ContainsPunctuationMarksOnly_ReturnFalse(string value)
        {
            Assume.That(value.Length, Is.GreaterThanOrEqualTo(ProductValidator.MinNameLength));
            Assume.That(value.Length, Is.LessThanOrEqualTo(ProductValidator.MaxNameLength));
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Name"), value);
            Assert.That(!result);
        }


        /// Validate Unit
        [TestCase("aa aa")]
        public void ValidateUnit_ContainsMoreThenOneWord_ReturnFalse(string value)
        {
            Assume.That(value.Length, Is.GreaterThanOrEqualTo(ProductValidator.MinUnitLength));
            Assume.That(value.Length, Is.LessThanOrEqualTo(ProductValidator.MaxUnitLength));
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Unit"), value);
            Assert.That(!result);
        }

        [TestCase("aa55")]
        [TestCase("a--a")]
        public void ValidateUnit_ContainsNotLetters_ReturnFalse(string value)
        {
            Assume.That(value.Length, Is.GreaterThanOrEqualTo(ProductValidator.MinUnitLength));
            Assume.That(value.Length, Is.LessThanOrEqualTo(ProductValidator.MaxUnitLength));
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Unit"), value);
            Assert.That(!result);
        }

        /// ValidatePrice
        [TestCase("aaa")]
        [TestCase("aa1a1")]
        public void TestValidatePrice_ContainsNotNumber_ReturnFalse(string value)
        {
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Price"), value);
            Assert.That(!result);
        }

        [TestCase("10,999")]
        [TestCase("10,000")]
        public void TestValidatePrice_ContainsMoreFractionDigitsThanTwo_ReturnFalse(string value)
        {
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Price"), value);
            Assert.That(!result);
        }

        [TestCase("10,9")]
        [TestCase("10,0")]
        [TestCase("10")]
        public void TestValidatePrice_ContainsLessFractionDigitsThanTwo_ReturnFalse(string value)
        {
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Price"), value);
            Assert.That(!result);
        }

        [TestCase("-10,99")]
        public void TestValidatePrice_ContainsNegativeNumber_ReturnFalse(string value)
        {
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Price"), value);
            Assert.That(!result);
        }

        [TestCase("10,99")]
        [TestCase("10,00")]
        public void TestValidatePrice_ContainsTwoFractionDigits_ReturnTrue(string value)
        {
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Price"), value);
            Assert.That(result);
        }

        /// ValidateCount
        [TestCase("aaa")]
        [TestCase("aa1a1")]
        public void TestValidateCount_ContainsNotNumber_ReturnFalse(string value)
        {
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Count"), value);
            Assert.That(!result);
        }

        [TestCase("-1")]
        public void TestValidateCount_ContainsNegativeNumber_ReturnFalse(string value)
        {
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Count"), value);
            Assert.That(!result);
        }

        [TestCase("100")]
        public void TestValidateCount_NormalCount_ReturnTrue(string value)
        {
            var result = productValidatorManager.Validate(new Book(), typeof(Book).GetProperty("Count"), value);
            Assert.That(result);
        }
    }
}