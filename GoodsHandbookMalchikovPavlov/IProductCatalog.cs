using System.Collections.Generic;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;
using GoodsHandbookMalchikovPavlov.Validators;
namespace GoodsHandbookMalchikovPavlov
{
    /// <summary>
    /// 1. returns в саммари нужно или заполнять, или удалять
    /// 2. IsProductExist / ProductExists
    /// 3. Слово Product из всех CRUD-методов можно убрать, мы и так работаем в контексте каталога продуктов
    /// 4. В методах интерфейса лучше не завязываться на массивы в возвращаемом значении, заменив их на IEnumerable<T> 
    /// </summary>
    public interface IProductCatalog
    {
        /// <summary>
        /// Проверка, существует ли продукт с таким идентификатором.
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        /// <returns></returns>
        bool DoesProductExist(int productId);

        /// <summary>
        /// Добавление продукта в список.
        /// </summary>
        /// <param name="product">Продукт</param>
        void AddProduct(Product product);

        /// <summary>
        /// Удаление продукта по его идентификатору.
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        void DeleteProduct(int productId);

        /// <summary>
        /// Обновление информации о продукте.
        /// </summary>
        /// <param name="product">Новый продукт</param>
        void UpdateProduct(Product product);

        /// <summary>
        /// Получить продукт по его идентификатору
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        /// <returns>Продукт</returns>
        Product GetProduct(int productId);

        /// <summary>
        /// Получение продукта по его типу.
        /// </summary>
        /// <param name="productType">Тип продукта</param>
        /// <returns>Продукт</returns>
        Product GetProduct(string productType);

        /// <summary>
        /// Получение валидатора продукта.
        /// </summary>
        /// <returns>Валидатор</returns>
        ProductValidatorManager GetProductValidator();

        /// <summary>
        /// Получение списка типов.
        /// </summary>
        /// <returns></returns>
        string[] GetProductTypeNames();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        string[] GetProductPropertyValidValues(PropertyInfo propertyInfo);

        /// <summary>
        /// Получение списка уже существующих продуктов.
        /// </summary>
        /// <param name="productTypeToFilterBy"></param>
        /// <returns></returns>
        IList<Product> GetProducts(string productTypeToFilterBy = null);

        /// <summary>
        /// Добавить к текущему количеству продукта значение.
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        /// <param name="countToAdd">Значение для добавления</param>
        void AddProductCount(int productId, int countToAdd);

        /// <summary>
        /// Вычесть значение из количества продукта.
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        /// <param name="countToSubstract">Значение для вычитания</param>
        void SubstractProductCount(int productId, int countToSubstract);

        /// <summary>
        /// Очистить лист от продуктов.
        /// </summary>
        void ClearListCatalog();
    }
}
