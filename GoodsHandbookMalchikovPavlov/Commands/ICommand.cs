using System;

namespace GoodsHandbookMalchikovPavlov
{
    internal interface ICommand
    {
        bool ProcessInput(string input, out string output, out bool attention);
        bool ProcessCtrlCombinations(ConsoleKeyInfo keyInfo, out string output, out bool attention);

        string GetCommandName();
        string GetCommandUsageText();
    }
}