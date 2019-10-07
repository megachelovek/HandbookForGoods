using System.Collections.Generic;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;
using GoodsHandbookMalchikovPavlov.Validators;

namespace GoodsHandbookMalchikovPavlov
{
    public interface IProductCatalog
    {
        /// <summary>
        ///     Проверка, существует ли продукт с таким идентификатором.
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        /// <returns></returns>
        bool IsExist(int productId);

        /// <summary>
        ///     Добавление продукта в список.
        /// </summary>
        /// <param name="product">Продукт</param>
        void Add(Product product);

        /// <summary>
        ///     Удаление продукта по его идентификатору.
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        void Delete(int productId);

        /// <summary>
        ///     Обновление информации о продукте.
        /// </summary>
        /// <param name="product">Новый продукт</param>
        void Update(Product product);

        /// <summary>
        ///     Получить продукт по его идентификатору
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        /// <returns>Продукт</returns>
        Product Get(int productId);

        /// <summary>
        ///     Получение продукта по его типу.
        /// </summary>
        /// <param name="productType">Тип продукта</param>
        /// <returns>Продукт</returns>
        Product Get(string productType);

        /// <summary>
        ///     Получение валидатора продукта.
        /// </summary>
        /// <returns>Валидатор</returns>
        ProductValidatorManager GetValidator();

        /// <summary>
        ///     Получение списка типов.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetTypeNames();

        /// <summary>
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        IEnumerable<string> GetPropertyValidValues(PropertyInfo propertyInfo);

        /// <summary>
        ///     Получение списка уже существующих продуктов.
        /// </summary>
        /// <param name="productTypeToFilterBy"></param>
        /// <returns></returns>
        IList<Product> GetProducts(string productTypeToFilterBy = null);

        /// <summary>
        ///     Добавить к текущему количеству продукта значение.
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        /// <param name="countToAdd">Значение для добавления</param>
        void AddCount(int productId, int countToAdd);

        /// <summary>
        ///     Вычесть значение из количества продукта.
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        /// <param name="countToSubstract">Значение для вычитания</param>
        void SubstractCount(int productId, int countToSubstract);

        /// <summary>
        ///     Получить информацию о продукте.
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        Product GetItem(int productId);

        /// <summary>
        ///     Очистить лист от продуктов.
        /// </summary>
        void ClearListCatalog();
    }
}