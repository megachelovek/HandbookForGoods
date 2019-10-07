using System;

namespace GoodsHandbookMalchikovPavlov
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    internal class NameAttribute : Attribute
    {
        public NameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}