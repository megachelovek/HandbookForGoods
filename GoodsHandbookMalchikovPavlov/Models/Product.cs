using System;
namespace GoodsHandbookMalchikovPavlov.Models
{
    [Serializable]
    public abstract class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public float Price { get; set; }
        public int Count { get; set; }
    }
}
