namespace GoodsHandbookMalchikovPavlov.Model
{
    internal class Book : Product
    {
        /// <summary>
        ///     Автор
        /// </summary>
        private string author;

        /// <summary>
        ///     Жанр
        /// </summary>
        private string genre;

        /// <summary>
        ///     Год издания
        /// </summary>
        private int year;

        public string Author
        {
            get => author;
            set => author = value;
        }

        public int Year
        {
            get => year;
            set => year = value;
        }

        public string Genre
        {
            get => genre;
            set => genre = value;
        }
    }
}