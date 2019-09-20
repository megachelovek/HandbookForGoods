using System;
namespace GoodsHandbookMalchikovPavlov.Models
{
    [Serializable]
    public sealed class Book : Product
    {
        public string Author { get; set; }
        public string Genre { get; set; }
    }
}
