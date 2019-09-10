using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov
{
    interface ICommand
    {
        bool ProcessInput(string input, out string output, out bool attention);
        bool ProcessCtrlCombinations(ConsoleKeyInfo keyInfo, out string output, out bool attention);

        string GetCommandName();
        string GetCommandUsageText();

    }

}
