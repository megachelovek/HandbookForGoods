using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov.Model
{
    class HomeAppliances:Product
    {
        /// <summary>
        /// Тип товара (телефон\холодильник)
        /// </summary>
        private string type;

        /// <summary>
        /// Модель 
        /// </summary>
        private string model;

        /// <summary>
        /// Описание
        /// </summary>
        private string description;

        public string Type { get => type; set => type = value; }

        public string Model { get => model; set => model = value; }

        public string Description { get => description; set => description = value; }
    }
}
