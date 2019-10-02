using System;
namespace GoodsHandbookMalchikovPavlov.Models
{
    /// <summary>
    /// Книги
    /// </summary>
    [Serializable]
    public sealed class Book : Product
    {
        public string Author { get; set; }
        public string Genre { get; set; }
    }
}
