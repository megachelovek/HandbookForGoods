using System;
namespace GoodsHandbookMalchikovPavlov.Models
{
    [Serializable]
    public sealed class Appliances : Product
    {
        public string Category { get; set; }
        public string Model { get; set; }
        public string Company { get; set; }
    }
}
