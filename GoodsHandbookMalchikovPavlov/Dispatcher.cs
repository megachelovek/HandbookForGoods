using System;
using System.Collections.Generic;
using System.Text;
using GoodsHandbookMalchikovPavlov.Commands;
using GoodsHandbookMalchikovPavlov.Model;
using GoodsHandbookMalchikovPavlov.Validators;

namespace GoodsHandbookMalchikovPavlov
{
    internal sealed class Dispatcher
    {
        private static readonly string USAGE =
            "Program can be used to create, edit and view products information\n" +
            "Usage: This is an interactive kind of program. Type any command name listed below\n" +
            "and go through steps it takes to acomplish your task\n" +
            "create              - to create a new record of a given product type\n" +
            "list                - to list product records\n" +
            "help [command name] - to get detailed information about a given command if applicable\n";

        private readonly Dictionary<string, ICommand> commandMap;
        private readonly Dictionary<string, Type> nameToProductMap;
        private readonly Dictionary<Type, ProductValidator> productToValidatorMap;
        private ICommand activeCommand;

        private bool attemptingToExit;
        private int prefixOffset;

        private readonly List<Product> storage;

        public Dispatcher()
        {
            storage = new List<Product>();
            nameToProductMap = new Dictionary<string, Type>();
            productToValidatorMap = new Dictionary<Type, ProductValidator>();

            var productType = typeof(Toy);
            var productName = ReflectionMisc.GetTypeName(productType);
            nameToProductMap.Add(productName, productType);
            productToValidatorMap.Add(productType, new ToyValidator());

            productType = typeof(Book);
            productName = ReflectionMisc.GetTypeName(productType);
            nameToProductMap.Add(productName, productType);
            productToValidatorMap.Add(productType, new BookValidator());

            productType = typeof(HomeAppliances);
            productName = ReflectionMisc.GetTypeName(productType);
            nameToProductMap.Add(productName, productType);
            productToValidatorMap.Add(productType, new HomeAppliancesValidator());

            commandMap = new Dictionary<string, ICommand>();
            commandMap.Add(ListCommand.GetName(), new ListCommand(storage));
            commandMap.Add(HelpCommand.GetName(), new HelpCommand(commandMap));
            commandMap.Add(CreateCommand.GetName(),
                new CreateCommand(storage, nameToProductMap, productToValidatorMap));
        }

        public void Start()
        {
            Console.TreatControlCAsInput = true;

            var buffer = new StringBuilder();
            var bufferIndex = 0;
            var attention = false;
            PrintResponse(USAGE, false);
            PrintPrompt();
            while (true)
            {
                var keyInfo = Console.ReadKey(true);

                var printable = !char.IsControl(keyInfo.KeyChar);

                if (keyInfo.Modifiers == ConsoleModifiers.Control)
                {
                    string output;
                    if (ProcessCtrlCombinations(keyInfo, out output, out attention)) break;
                    PrintResponse(output, attention);
                    PrintPrompt();
                }

                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        string output;
                        if (ProcessInput(buffer.ToString(), out output, out attention)) break;
                        PrintResponse(output, attention);
                        PrintPrompt();
                        buffer.Length = 0;
                        bufferIndex = 0;
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace)
                    {
                        if (bufferIndex > 0)
                        {
                            ClearPromptLine(buffer.Length);

                            bufferIndex--;
                            buffer.Remove(bufferIndex, 1);

                            Console.Write(buffer.ToString());
                            Console.SetCursorPosition(prefixOffset + bufferIndex, Console.CursorTop);
                        }
                    }
                    else if (keyInfo.Key == ConsoleKey.LeftArrow)
                    {
                        if (bufferIndex > 0)
                        {
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            bufferIndex--;
                        }
                    }
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        if (bufferIndex < buffer.Length)
                        {
                            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                            bufferIndex++;
                        }
                    }
                    else if (printable)
                    {
                        buffer.Insert(bufferIndex++, keyInfo.KeyChar);
                        ClearPromptLine(buffer.Length);

                        Console.Write(buffer.ToString());
                        Console.SetCursorPosition(prefixOffset + bufferIndex, Console.CursorTop);
                    }
                }
            }
        }

        public bool ProcessInput(string input, out string output, out bool attention)
        {
            attention = false;
            output = "";
            if (attemptingToExit) attemptingToExit = false;
            if (activeCommand == null) activeCommand = ParseCommandName(input);
            if (activeCommand != null)
                if (activeCommand.ProcessInput(input, out output, out attention))
                    activeCommand = null;
            return false;
        }

        public bool ProcessCtrlCombinations(ConsoleKeyInfo keyInfo, out string output, out bool attention)
        {
            attention = false;
            output = "";

            if (activeCommand != null)
            {
                if (activeCommand.ProcessCtrlCombinations(keyInfo, out output, out attention)) activeCommand = null;
            }
            else
            {
                if (keyInfo.Key == ConsoleKey.Q)
                {
                    if (attemptingToExit) return true;
                    output = "Exit? Press CTRL+Q again to exit";
                    attention = true;
                    attemptingToExit = true;
                }
                else if (attemptingToExit)
                {
                    attemptingToExit = false;
                }
            }

            return false;
        }

        private void ClearPromptLine(int lineLength)
        {
            Console.SetCursorPosition(prefixOffset, Console.CursorTop);
            for (var i = 0; i < lineLength; i++)
                Console.Write(" ");
            Console.SetCursorPosition(prefixOffset, Console.CursorTop);
        }

        private void PrintResponse(string response, bool attention)
        {
            if (response.Length > 0)
            {
                if (attention) Console.BackgroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(0, Console.CursorTop + 1);
                Console.Write(response);
                if (attention) Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        private void PrintPrompt()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            var prefix = "";
            if (activeCommand != null) prefix = activeCommand.GetCommandName();
            prefix += ">";
            Console.Write(prefix);
            prefixOffset = prefix.Length;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private ICommand ParseCommandName(string input)
        {
            var pos = Misc.GetWordPos(input, 0, input.Length - 1);

            foreach (var pair in commandMap)
            {
                var commandName = pair.Key;
                if (Misc.StringsEqual(input, pos.begin, pos.end, commandName, 0, commandName.Length - 1))
                    return pair.Value;
            }

            return null;
        }
    }
}