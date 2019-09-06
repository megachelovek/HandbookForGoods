using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov
{
    interface IValidator
    {
        bool Validate(string fieldName, string fieldValue);
    }

    
    static class Common
    {
        public static bool StringsEqual(string first, int firstBegin, int firstEnd, string second, int secondBegin, int secondEnd)
        {
            Debug.Assert(firstBegin >= 0 && firstBegin < first.Length);
            Debug.Assert(firstEnd < first.Length);
            Debug.Assert(secondBegin >= 0 && secondBegin < second.Length);
            Debug.Assert(secondEnd < second.Length);
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
        public static bool FindString(string toFind, int begin, int end, IEnumerable<string> strings)
        {
            foreach (string str in  strings)
            {

                if (Common.StringsEqual(toFind, begin, end, str, 0, str.Length - 1))
                {
                    return true;
                }
            }
            return false;
        }
    }

    internal struct TokenInfo
    {
        public int begin;
        public int end;
    }

    

    abstract class Product
    {
        protected string name;
        protected string company;
    }
 
    internal enum ToyCategory
    {
        Educational, VideoGame
    }
    sealed class Toy : Product
    {
        private ToyCategory category;
        private int age;
        public string Name { get { return name; } set { name = value; } }
        public string Company { get { return company; } set { company = value; } }
        public ToyCategory Category { get { return category; } set { category = value; } }
        public int Age { get { return age; } set { age = value; } }
    }

    static class ToyInfo
    {
        public static readonly Type TYPE = new Toy().GetType();
        public static string[] TOY_FIELD_NAMES = { "Category", "Age" };
        public static readonly string[] TOY_CATEGORY_NAMES = { "Educational", "Video Game", "Math", "Plastic" };
        public const int MIN_AGE = 0;
        public const int MAX_AGE = 18;
    }
    class ProductValidator : IValidator
    {
        public bool Validate(string fieldName, string fieldValue)
        {
            switch (fieldName)
            {
                case "Name":
                    {

                    }
                    break;
                case "Company":
                    {

                    }
                    break;
            }
            return true;
        }
    }
    sealed class ToyValidator : IValidator
    {
        public bool Validate(string fieldName, string fieldValue)
        {
            switch (fieldName)
            {
                case "Category":
                    {
                        return Common.FindString(fieldValue, 0, fieldValue.Length - 1, ToyInfo.TOY_CATEGORY_NAMES);
                    }
                case "Age":
                    {
                        int age;
                        if (Int32.TryParse(fieldValue, out age))
                        {
                            return (age >= ToyInfo.MIN_AGE && age <= ToyInfo.MAX_AGE);
                        }
                    }
                    break;
            }
            return false;
        }
    }
    internal enum DispatcherCommand
    { None, Create, List }
    sealed class Dispatcher
    {
        private static readonly Dictionary<string, DispatcherCommand> COMMAND_MAP = new Dictionary<string, DispatcherCommand>
        {
            { "create", DispatcherCommand.Create},
            { "list", DispatcherCommand.List}
        };

        private static readonly Dictionary<string, Type> PRODUCT_MAP = new Dictionary<string, Type>
        {
            { "toy", ToyInfo.TYPE },
            { "food", ToyInfo.TYPE },
            { "device", ToyInfo.TYPE }

        };

        public static string[] PRODUCT_FIELD_NAMES = { "Name", "Company"};

        


        private int currentField;
        private DispatcherCommand activeCommand = DispatcherCommand.None;
        private Type typeBeingCreated = null;
        public void ProcessInput(string input)
        {

            string proccessedInput;
            List<TokenInfo> tokens = ParseInput(input, out proccessedInput);
            if (tokens.Count > 0)
            {
                if (activeCommand == DispatcherCommand.None)
                {
                    activeCommand = ParseCommandName(proccessedInput, tokens[0]);
                    if (activeCommand != DispatcherCommand.None)
                    {
                        tokens.RemoveAt(0);
                    }
                }

                switch (activeCommand)
                {
                    case DispatcherCommand.Create:
                        {
                            if (ProcessCreateCommand(proccessedInput, tokens))
                            {
                                activeCommand = DispatcherCommand.None;
                            }
                        }
                        break;

                }

#if false
                Console.WriteLine(proccessedInput);
                foreach(TokenInfo token in tokens)
                {
                    Console.WriteLine("begin: {0}, end: {1}, length: {2}", token.begin, token.end, token.end - token.begin + 1);
                }
#endif
            }
        }

        private List<TokenInfo> ParseInput(string input, out string proccessedInput)
        {
            StringBuilder buffer = new StringBuilder(input.Length);
            List<TokenInfo> tokens = new List<TokenInfo>();
            int state = 0;
            int count = 0;
            int tokenBegin = -1;
            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];
                switch (state)
                {
                    case 0:
                        {
                            if (ch == ' ')
                            {

                            }
                            else if (ch == '"')
                            {
                                state = 1;
                            }
                            else if (ch == '\\')
                            {
                                state = 3;
                            }
                            else
                            {
                                state = 2;
                                tokenBegin = buffer.Length;
                                buffer.Append(ch);
                                count++;
                            }
                        }break;
                    case 1:
                        {
                            if (ch == '\\')
                            {
                                state = 4;
                            }
                            else if (ch == '"')
                            {
                                state = 5;
                            }
                            else
                            {
                                if (tokenBegin == -1)
                                {
                                    tokenBegin = buffer.Length;
                                    buffer.Append(ch);
                                    count++;
                                }
                                else
                                {
                                    buffer.Append(ch);
                                }
                            }
                        }
                        break;
                    case 2:
                        {
                            if (ch == ' ')
                            {
                                tokens.Add(new TokenInfo { begin = tokenBegin, end = buffer.Length - 1 });
                                buffer.Append(ch);
                                state = 0;
                                tokenBegin = -1;
                            }
                            else if (ch == '\\')
                            {
                                state = 3;
                            }
                            else if (ch == '"')
                            {
                                state = 6;
                            }
                            else
                            {
                                if (tokenBegin == -1)
                                {
                                    tokenBegin = buffer.Length;
                                    buffer.Append(ch);
                                    count++;
                                }
                                else
                                {
                                    buffer.Append(ch);
                                }
                            }

                        }
                        break;
                    case 3:
                        {
                            if (ch == '\\' || ch == '"')
                            {
                                state = 2;
                                if (tokenBegin == -1)
                                {
                                    tokenBegin = buffer.Length;
                                    buffer.Append(ch);
                                    count++;
                                }
                                else
                                {
                                    buffer.Append(ch);
                                }
                            }
                            else
                            {
                                state = 6;
                            }
                        }
                        break;
                    case 4:
                        {
                            if (ch == '\\' || ch == '"')
                            {
                                state = 1;
                                buffer.Append(ch);

                            }
                            else
                            {
                                state = 6;
                            }
                        }
                        break;
                    case 5:
                        {
                            if (ch == ' ')
                            {
                                tokens.Add(new TokenInfo { begin = tokenBegin, end = buffer.Length - 1 });
                                buffer.Append(ch);
                                state = 0;
                                tokenBegin = -1;
                            }
                            else
                            {
                                state = 6;
                            }
                        }
                        break;
                    case 6:
                        {
                            count = 0;
                            proccessedInput = null;
                            return new List<TokenInfo>();
                        }
                }   
            }
            switch (state)
            {
                case 2:
                    {
                        tokens.Add(new TokenInfo { begin = tokenBegin, end = buffer.Length - 1 });
                    }
                    break;
                case 5:
                    {
                        tokens.Add(new TokenInfo { begin = tokenBegin, end = buffer.Length - 1 });
                    }
                    break;
                case 1:
                case 3:
                case 4:
                case 6:
                    {
                        proccessedInput = null;
                        return new List<TokenInfo>();
                    };
            }
            proccessedInput = buffer.ToString();
            return tokens;
        }

        private DispatcherCommand ParseCommandName(string commandString, TokenInfo token)
        {
            foreach (KeyValuePair<string, DispatcherCommand> pair in COMMAND_MAP)
            {
                string commandName = pair.Key;
                if (Common.StringsEqual(commandString, token.begin, token.end, commandName, 0, commandName.Length - 1))
                {
                    return pair.Value;
                }
            }

            return DispatcherCommand.None;
        }

        private bool ProcessCreateCommand(string argsString, List<TokenInfo> tokens)
        {
            string product = argsString.Substring(tokens[0].begin, tokens[0].end + 1);
            if (PRODUCT_MAP.ContainsKey(product))
            {
                typeBeingCreated = PRODUCT_MAP[product];
            }

            return true;
          
        }
    }
}
