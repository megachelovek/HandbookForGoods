using System;

namespace GoodsHandbookMalchikovPavlov.Models
{
    /// <summary>
    ///     Книги
    /// </summary>
    [Serializable]
    public sealed class Book : Product
    {
        [Name("Author First Name")] public string AuthorFirstName { get; set; }

        [Name("Author Last Name")] public string AuthorLastName { get; set; }

        [Name("Author Middle Name")] public string AuthorMiddleName { get; set; }

        public string Genre { get; set; }
    }
}