using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov.Model
{
    class Product
    {
        /// <summary>
        /// Название товара
        /// </summary>
        private string name;

        /// <summary>
        ///  Идентификатор
        /// </summary>
        private int id;

        /// <summary>
        /// Количество
        /// </summary>
        private int count;

        /// <summary>
        /// Цена
        /// </summary>
        private float price;

        /// <summary>
        /// Единица измерения (кг, шт, м)
        /// /// </summary>
        private string unit;

        /// <summary>
        /// Компания производитель или издатель
        /// </summary>
        private string company;
    }
}
