using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    class NameAttribute : Attribute
    {
        private string name;
        public NameAttribute(string name)
        {
            this.name = name;
        }
        public string Name { get { return name; } }
    }
}
