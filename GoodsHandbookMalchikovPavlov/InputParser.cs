namespace GoodsHandbookMalchikovPavlov
{
    internal struct StringPos
    {
        private int begin;
        private int end;
        public StringPos(int begin, int end)
        {
            this.begin = begin;
            this.end = end;
        }
        // Эти две проперти не используются - зачем они?
        public int Begin { get { return begin; } }
        public int End { get { return end; } }
    }
    // Следите за абстракцией класса. Некоторые методы зачем-то торчат наружу, но не используются извне.
    static class InputParser
    {
        // Этот метод не используется и вполне может быть заменен на string.Split() +
        // выбор первого элемента массива, если он существует
        public static StringPos GetFirstWord(string str)
        {
            int wordBegin = -1;
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                if(wordBegin == -1 && (ch != ' ' && ch != '\t'))
                {
                    wordBegin = i;
                }
                if(wordBegin != -1 && (ch == ' ' || ch == '\t' ))
                {
                    return new StringPos(wordBegin, i - 1);
                    
                } 
            }
            if (wordBegin != -1)
            {
                return new StringPos(wordBegin, str.Length - 1);
                
            }
            return new StringPos(0, -1);
        }
        
        // Чем не угодил string.Split() с последующим подсчетом слов в массиве?
        public static int GetWordsCount(string str)
        {
            int count = 0;
            int wordBegin = -1;
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                if (wordBegin == -1 && (ch != ' ' && ch != '\t'))
                {
                    wordBegin = i;
                }
                if (wordBegin != -1 && (ch == ' ' || ch == '\t'))
                {
                    count++;
                    wordBegin = -1;
                }
            }
            if (wordBegin != -1)
            {
                count++;
            }
            return count;
        }
        // string.Split(). Ребят, вы хотите написать свою FCL? Поверьте, разработчики MS сделают это лучше нас с вами
        public static string[] GetWords(string str)
        {
            int count = GetWordsCount(str);
            if (count > 0)
            {
                string[] result = new string[count];
                int index = 0;
                int wordBegin = -1;
                for (int i = 0; i < str.Length; i++)
                {
                    char ch = str[i];
                    if (wordBegin == -1 && (ch != ' ' && ch != '\t'))
                    {
                        wordBegin = i;
                    }
                    if (wordBegin != -1 && (ch == ' ' || ch == '\t'))
                    {
                        result[index++] = str.Substring(wordBegin, i - wordBegin);
                        wordBegin = -1;
                    }
                }
                if (wordBegin != -1)
                {
                    result[index++] = str.Substring(wordBegin, str.Length - wordBegin);
                }
                return result;
            }
            return new string[0];
        }
        // Не используется
        public static bool ContainsOnlyLetters(string str)
        {
            // Если уж так нужно - есть прекрасный метод IEnumerable<T>.All() в Linq
            foreach (var ch in str)
            {
                if (!char.IsLetter(ch))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
