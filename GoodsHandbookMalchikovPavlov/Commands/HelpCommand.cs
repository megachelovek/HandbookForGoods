using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsHandbookMalchikovPavlov
{
    sealed class HelpCommand : ICommand
    {
        private const string NAME = "help";

        private const string USAGE =
            "USAGE:\n" +
            "help [command name]\n";

        private readonly Dictionary<string, ICommand> commandMap;

        public HelpCommand(Dictionary<string, ICommand> commandMap)
        {
            this.commandMap = commandMap;
        }
       
        public bool ProcessInput(string input, out string output, out bool attention)
        {
            attention = false;
            List<StringPos> positions = Misc.GetWords(input);

            if (positions.Count == 2)
            {
                StringPos pos = positions[1];
                foreach (KeyValuePair<string, ICommand> pair in commandMap)
                {
                    string commandName = pair.Key;
                    if (Misc.StringsEqual(input, pos.begin, pos.end, commandName, 0, commandName.Length - 1))
                    {
                        output = pair.Value.GetCommandUsageText();
                        return true;
                    }
                }
            }
            output = GetCommandUsageText();

            return true;
        }
        public bool ProcessCtrlCombinations(ConsoleKeyInfo keyInfo, out string output, out bool attention)
        {
            attention = false;
            output = "";
            return true;
        }
        public string GetCommandName()
        {
            return NAME;
        }
        public string GetCommandUsageText()
        {
            StringBuilder buffer = new StringBuilder(USAGE.Length);
            buffer.Append(USAGE);
            buffer.Append("\nAvailable commands:\n");
            foreach (KeyValuePair<string, ICommand> pair in commandMap)
            {
                string commandName = pair.Key;
                buffer.Append(commandName);
                buffer.Append("\n");
            }
            return buffer.ToString();
        }

        public static string GetName()
        {
            return NAME;
        }
    }
}
