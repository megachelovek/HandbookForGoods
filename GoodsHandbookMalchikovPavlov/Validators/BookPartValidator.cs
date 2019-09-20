using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using GoodsHandbookMalchikovPavlov.Models;
using System.Linq;

namespace GoodsHandbookMalchikovPavlov.Validators
{
    internal static class BookPartValidator
    {
        private const int MinAuthorLength = 2;
        private const int MaxAuthorLength = 32;
        private static readonly string AuthorHasZeroLengthErrorMsg =
            "\"Author\" cannot have zero length" + Environment.NewLine;
        private static readonly string AuthorIsTooLongErrorMsg =
            "\"Author\" cannot have length more than " + MaxAuthorLength + " characters long" + Environment.NewLine;
        private static readonly string AuthorIsTooShortErrorMsg =
            "\"Author\" cannot have length less than " + MinAuthorLength + " characters long" + Environment.NewLine;
        private static readonly string AuthorConsistOfMoreThanOneWordErrorMsg =
           "\"Author\" cannot consist of more than one word" + Environment.NewLine;
        private static readonly string AuthorContainsWrongPatternErrorMsg =
          "\"Author\" can only contain letters and optionally \"-\" symbol in between" + Environment.NewLine;
        private static readonly string[] Genres = new string[]
            {
                "Fairy Tale",
                "Mystic",
                "Fantasy",
                "Detective",
                "Psychology",
                "Popular Science",
                "Educational",
                "Sentimental Novel",
                "Teenage Prose"
            };
        private static readonly string GenresDesc;
        static BookPartValidator()
        {
            StringBuilder buffer = new StringBuilder(128);
            buffer.Append("\"Genre\" must be one of the following:");
            buffer.Append(Environment.NewLine);
            foreach (var s in Genres)
            {
                buffer.Append(s);
                buffer.Append(Environment.NewLine);
            }
            GenresDesc = buffer.ToString();
        }
        public static bool Validate(Product product, PropertyInfo propertyInfo, string propertyValue,
          out string errorMsg, out object convertedValue)
        {
            errorMsg = null;
            convertedValue = propertyValue;
            switch (propertyInfo.Name)
            {
                case "Author":
                    {
                        return ValidateAuthor(product, propertyInfo, propertyValue, ref errorMsg, ref convertedValue);
                    }
                case "Genre":
                    {
                        return ValidateGenre(product, propertyInfo, propertyValue, ref errorMsg, ref convertedValue);
                    }
                default:
                    {
                        throw new ArgumentException();
                    }
            }
        }
        /// <summary>
        /// Возвращает тру если propertyValue - строка состоящая из букв и дефисa, дефис может находиться только
        /// в середине строки, дефисов может быть несколько, длина строки должна быть от 2 до 32 символов включительно
        /// </summary>
        private static bool ValidateAuthor(Product product, PropertyInfo propertyInfo, string propertyValue,
            ref string errorMsg, ref object convertedValue)
        {
            string[] words = InputParser.GetWords(propertyValue);
            if (words.Length == 1)
            {
                if (words[0].Length >= MinAuthorLength && words[0].Length <= MaxAuthorLength)
                {
                    bool check = words[0].All(c => Char.IsLetter(c) || c == '-');
                    Regex regex = new Regex(@"^\w(\w|-)*\w$");
                    if (check && regex.IsMatch(words[0]))
                    {
                        return true;
                    }
                    else
                    {
                        errorMsg = AuthorContainsWrongPatternErrorMsg;
                    }
                }
                else
                {
                    if (words[0].Length < MinAuthorLength)
                    {
                        errorMsg = AuthorIsTooShortErrorMsg;
                    }
                    else
                    {
                        errorMsg = AuthorIsTooLongErrorMsg;
                    }
                }
            }
            else if (words.Length == 0)
            {
                errorMsg = AuthorHasZeroLengthErrorMsg;
            }
            else if (words.Length > 1)
            {
                errorMsg = AuthorConsistOfMoreThanOneWordErrorMsg;
            }
            return false;
        }
        /// <summary>
        /// Возвращает тру если propertyValue равна одной из строк в Genres выше
        /// </summary>
        private static bool ValidateGenre(Product product, PropertyInfo propertyInfo, string propertyValue,
            ref string errorMsg, ref object convertedValue)
        {
            if (Misc.FindString(propertyValue, Genres) > -1)
            {
                convertedValue = propertyValue;
                return true;
            }
            else
            {
                errorMsg = GenresDesc;
            }
            return false;
        }
    }
}
