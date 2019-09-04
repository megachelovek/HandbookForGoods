using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov.Model
{
    class Toy:Product
    {
        /// <summary>
        /// Название игрушки
        /// </summary>
        private string name;

        /// <summary>
        /// Возраст от
        /// </summary>
        private int startAge;

        /// <summary>
        /// Возраст до
        /// </summary>
        private int endAge;

        /// <summary>
        /// Для девочек/мальчиков (f/m)
        /// </summary>
        private char sex;

        /// <summary>
        /// Тип (мягкая, конструктор, образовательная)
        /// </summary>
        private string type;
    }
}
