using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov
{
    [AttributeUsage(AttributeTargets.Property)]
    class AutoIdAttribute : Attribute
    {
        public AutoIdAttribute() { }
    }
}
