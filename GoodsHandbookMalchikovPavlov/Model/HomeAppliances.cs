using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov.Model
{
    [Name("Home Appliances")]
    class HomeAppliances :Product
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

        [Name("Home Appliance Type")]
        public string Type { get => type; set => type = value; }

        [Name("Home Appliance Model")]
        public string Model { get => model; set => model = value; }

        [Name("Home Appliance Description")]
        public string Description { get => description; set => description = value; }
    }
}
