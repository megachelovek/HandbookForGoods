using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov.Model
{
    class Toy:Product
    {
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
        private string sex;

        /// <summary>
        /// Тип (мягкая, конструктор, образовательная)
        /// </summary>
        private string type;

        [Name("Start Age")]
        public int StartAge { get => startAge; set => startAge = value; }

        [Name("End Age")]
        public int EndAge { get => endAge; set => endAge = value; }

        public string Sex { get => sex; set => sex = value; }

        [Name("Toy Type")]
        public string Type { get => type; set => type = value; }

    }
}
