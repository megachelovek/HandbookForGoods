using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov.Model
{
    class Book:Product
    {
        /// <summary>
        /// Название книги
        /// </summary>
        private string name;

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
    }
}
