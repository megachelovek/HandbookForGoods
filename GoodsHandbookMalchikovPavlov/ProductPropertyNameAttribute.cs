using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov
{
    [AttributeUsage(AttributeTargets.Property)]
    class ProductPropertyNameAttribute : Attribute
    {
        private string name;
        public ProductPropertyNameAttribute(string name)
        {
            this.name = name;
        }

        public string Name { get { return name; } }
    }
}
