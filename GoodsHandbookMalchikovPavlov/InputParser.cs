using System;

namespace GoodsHandbookMalchikovPavlov
{
    internal static class InputParser
    {
        public static string[] GetWords(string str)
        {
            return str.Split(' ');
        }
    }
}