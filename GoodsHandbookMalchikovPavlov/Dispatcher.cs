using System;
using System.Collections.Generic;
using GoodsHandbookMalchikovPavlov.Commands;
namespace GoodsHandbookMalchikovPavlov
{
    /// <remarks>
    /// 1. Не хватает комментариев. Пожалейте людей, которые будут работать с вашим кодом. Какой бы прозрачной ни была
    /// задумка - ОБЯЗАТЕЛЬНО пишите саммари для а) класса, б) всех методов публичного API и с) наиболее важных
    /// сервисных методов, даже если они private. Для вашей реализации это вдвойне важная рекомендация, потому что вы
    /// выбрали state-реализацию, где активно в разных местах программы меняются поля экземпляра.
    /// 2. Почему боимся использовать var?
    /// 3. Форматирование. Между блоками кода, обрамленными в фигурные скобки, не должно быть пустых строк. Методы
    /// должны разделяться пустой строкой, коду нужен "воздух". Поля от первого метода (или конструктора), следующего за
    /// ними, также должны отделяться пустой строкой.
    /// </remarks>>
    public class Dispatcher
    {
        private readonly IProductCatalog productCatalog;
        private readonly Dictionary<string, ICommand> commandMap;
        private readonly string help =
            "Yooper is a product catalog program" + Environment.NewLine +
            "Available commands:" + Environment.NewLine +
            "create [product type] - creates new product and adds it to a catalog" + Environment.NewLine +
            "                        \"product type\" can be specified inline" + Environment.NewLine +
            "                        with command via optional parameter" + Environment.NewLine +
            "list [product type]   - lists products presented in catalog" + Environment.NewLine +
            "     [-full]            to filter output by product type" + Environment.NewLine +
            "                        use optional parameter  \"product type\"" + Environment.NewLine +
            "                        use option \"-full\" to get detailed output" + Environment.NewLine +
            "delete \"product id\"     deletes product from catalog by id" + Environment.NewLine +
            "add-count             - increases count of product" + Environment.NewLine +
            "         \"product id\"" + Environment.NewLine +
            "         \"count\"" + Environment.NewLine +
            "sub-count             - decreases count of product" + Environment.NewLine +
            "         \"product id\"" + Environment.NewLine +
            "         \"count\"" + Environment.NewLine +
            "quit                  - quites the program or, if used during execution" + Environment.NewLine +
            "                        of interactive command, exits from that command";
        private ICommand activeCommand;
        private string activeCommandName;
        private string response;
        private string[] currentArgs;
        public Dispatcher()
        {
            productCatalog = new ProductCatalog("product_data");
            commandMap = new Dictionary<string, ICommand>()
            {
                {"create",  new CreateCommand(productCatalog,currentArgs)},
                {"list",  new ListCommand(productCatalog,currentArgs)},
                {"delete",  new DeleteCommand(productCatalog,currentArgs)},
                {"add-count",  new AddCountCommand(productCatalog,currentArgs)},
                {"sub-count",  new SubstractCountCommand(productCatalog,currentArgs)}
            };
        }
        public void Start()
        {
            activeCommand = null;
            activeCommandName = "";
            response = help;
            OutputResponse();
            while (true)
            {
                OutputPrompt();
                string input = GatherInput();
                bool done = ProcessInput(input);
                OutputResponse();
                if (done)
                {
                    break;
                }
            }
        }
        private string GatherInput()
        {
            return Console.ReadLine();
        }
        // Загнав себя в рамки state-like-реализации, вы заодно усложнили себе жизнь в части тестирования самого важного
        // метода вашей утилиты - процессинга ввода.
        private bool ProcessInput(string input)
        {
            response = null;
            if (activeCommand != null)
            {
                bool done = activeCommand.Process(input) == CommandReturnCode.Done;
                response = activeCommand.GetLastResponse();
                if (done)
                {
                    activeCommand = null;
                    activeCommandName = "";
                }
            }
            else
            {
                string[] args = InputParser.GetWords(input);
                if (args.Length > 0)
                {
                    if (args[0].Equals("quit", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    else if ((args[0].Equals("help", StringComparison.OrdinalIgnoreCase)))
                    {
                        response = help;
                    }
                    else if (commandMap.ContainsKey(args[0]))
                    {
                        activeCommand = commandMap[args[0]];
                        activeCommandName = args[0];
                        bool done = activeCommand.Process(input) == CommandReturnCode.Done;
                        response = activeCommand.GetLastResponse();
                        if (done)
                        {
                            activeCommand = null;
                            activeCommandName = "";
                        }
                    }
                    else
                    { 
                        response = help;
                    }
                }
            }
            return false;
        }
        private void OutputPrompt()
        {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(activeCommandName);
            Console.Write(">>>");
            Console.ForegroundColor = temp;
        }
        private void OutputResponse()
        {
            if (response != null && response.Length > 0)
            {
                while(response.EndsWith(Environment.NewLine) && (response.Length > 0))
                {
                    response = response.Substring(0, response.Length - 1);
                }
                if (response.Length > 0)
                {
                    Console.WriteLine(response);
                }
            }

        }
    }
}
