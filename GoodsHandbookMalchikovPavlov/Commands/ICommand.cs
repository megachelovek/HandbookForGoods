namespace GoodsHandbookMalchikovPavlov.Commands
{
    public enum CommandReturnCode
    {
        Done,
        Undone
    }

    public interface ICommand
    {
        /// <summary>
        ///     Процесс выполнения команды с аргументами
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        CommandReturnCode Process(string[] input);

        /// <summary>
        ///     Получение последнего ответа
        /// </summary>
        /// <returns></returns>
        string GetLastResponse();
    }
}