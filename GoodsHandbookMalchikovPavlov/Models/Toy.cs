using System;
namespace GoodsHandbookMalchikovPavlov.Models
{
    /// <summary>
    /// Игрушка
    /// </summary>
    [Serializable]
    public sealed class Toy : Product
    {
        public string Category { get; set; }
        [Name("Minimum Age")]
        public int MinAge { get; set; }
        [Name("Maximum Age")]
        public int MaxAge { get; set; }
        public string Sex { get; set; }
    }
}
