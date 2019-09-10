using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov
{
    [AttributeUsage(AttributeTargets.Property)]
    class ProductAutoIdAttribute : Attribute
    {
        public ProductAutoIdAttribute() { }
    }
}
