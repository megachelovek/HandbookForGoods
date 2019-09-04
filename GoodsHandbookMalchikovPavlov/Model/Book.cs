using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov.Model
{
    class Book:Product
    {
        /// <summary>
        /// Автор
        /// </summary>
        private string author;

        /// <summary>
        /// Год издания
        /// </summary>
        private int year;
        
        /// <summary>
        /// Жанр
        /// </summary>
        private string genre;

        public string Author { get => author; set => author = value; }

        public int Year { get => year; set => year = value; }

        public string Genre { get => genre; set => genre = value; }
    }
}
