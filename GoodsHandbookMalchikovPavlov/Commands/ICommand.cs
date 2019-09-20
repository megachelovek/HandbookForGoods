namespace GoodsHandbookMalchikovPavlov.Commands
{
    public enum CommandReturnCode
    {
        Done, Undone
    }
    public interface ICommand
    {
        CommandReturnCode Process(string input);
        string GetLastResponse();
    }
}
