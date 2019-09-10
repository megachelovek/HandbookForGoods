using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov
{
    internal struct StringPos
    {
        public int begin;
        public int end;
    }
    static class Misc
    {
        public static bool StringsEqual(string first, int firstBegin, int firstEnd, string second, int secondBegin, int secondEnd)
        {
            if (!((firstBegin >= 0 && firstBegin < first.Length))) return false;
            if (!((firstEnd < first.Length))) return false;
            if (!((secondBegin >= 0 && secondBegin < second.Length))) return false;
            if (!((secondEnd < second.Length))) return false;

            int firstLength = firstEnd - firstBegin + 1;
            int secondLength = secondEnd - secondBegin + 1;

            if (firstLength > 0 && (firstLength == secondLength))
            {
                for (int i = 0; i < firstLength; i++)
                {
                    if (first[firstBegin + i] != second[secondBegin + i])
                        return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int FindString(string str, int begin, int end, string[] strings)
        {
            for (int i = 0; i < strings.Length; i++)
            {

                if (Misc.StringsEqual(str, begin, end, strings[i], 0, str.Length - 1))
                {
                    return i;
                }
            }
            return -1;
        }
        public static StringPos GetWordPos(string str, int begin, int end)
        {
            StringPos result = new StringPos() { begin = -1, end = -1 };
            int length = end - begin + 1;
            for (int i = 0; i < length; i++)
            {
                if (result.begin == -1 && str[i] != ' ')
                {
                    result.begin = i;
                }
                if (result.begin > -1 && str[i] == ' ')
                {
                    result.end = i - 1;
                    return result;
                }
            }
            if (result.begin > -1)
            {
                result.end = length - 1;
                return result;
            }
            return result;
        }

        public static List<StringPos> GetWords(string str)
        {
            List<StringPos> result = new List<StringPos>();
            int wordBegin = -1;
            for (int i = 0; i < str.Length; i++)
            {
                if (wordBegin == -1 && str[i] != ' ')
                {
                    wordBegin = i;
                }
                if (wordBegin > -1 && str[i] == ' ')
                {
                    StringPos pos = new StringPos() { begin = wordBegin, end = i - 1 };
                    result.Add(pos);
                    wordBegin = -1;
                }
            }
            if (wordBegin > -1)
            {
                StringPos pos = new StringPos() { begin = wordBegin, end = str.Length - 1 };
                result.Add(pos);
            }
            return result;
        }
    }

}
