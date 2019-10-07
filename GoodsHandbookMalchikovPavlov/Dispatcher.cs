using System;
using System.Linq;
using System.Resources;
using GoodsHandbookMalchikovPavlov.Commands;
using GoodsHandbookMalchikovPavlov.Properties;

namespace GoodsHandbookMalchikovPavlov
{
    /// <summary>
    ///     Диспетчер, который прослушивает команды, вводимые пользователем
    ///     и передает параметры командам, которые их выполняют
    /// </summary>
    public class Dispatcher
    {
        private readonly IProductCatalog productCatalog;
        private ICommand activeCommand;
        private string activeCommandName;

        private readonly string[] commands = {"create", "list", "delete", "add-count", "sub-count","get-item"};
        private string[] currentArgs;
        private readonly string help;

        private readonly ResourceManager resourceManager = new ResourceManager(typeof(Resources));
        private string response;

        public Dispatcher()
        {
            productCatalog = new ProductCatalog("product_data");
            help = resourceManager.GetString("HELP");
        }

        /// <summary>
        ///     Запуск основного цикла программы, выход по quit
        /// </summary>
        public void Start()
        {
            activeCommand = null;
            activeCommandName = "";
            response = help;
            OutputResponse();
            while (true)
            {
                OutputPrompt();
                var input = GatherInput();
                var done = ProcessInput(input);
                OutputResponse();
                if (done) break;
            }
        }

        /// <summary>
        ///     Ввод
        /// </summary>
        /// <returns></returns>
        private string GatherInput()
        {
            return Console.ReadLine();
        }

        /// <summary>
        ///     Процесс ввода аргументов
        /// </summary>
        /// <param name="input">новый аргумент</param>
        /// <returns>Завершен</returns>
        private bool ProcessInput(string input)
        {
            response = null;
            if (activeCommand != null)
            {
                var done = activeCommand.Process(InputParser.GetWords(input)) == CommandReturnCode.Done;
                response = activeCommand.GetLastResponse();
                if (done)
                {
                    activeCommand = null;
                    activeCommandName = "";
                }
            }
            else
            {
                var currentArgs = InputParser.GetWords(input);
                if (currentArgs.Length > 0)
                {
                    if (currentArgs[0].Equals("quit", StringComparison.OrdinalIgnoreCase)) return true;

                    if (currentArgs[0].Equals("help", StringComparison.OrdinalIgnoreCase))
                    {
                        response = help;
                    }
                    else if (commands.Contains(currentArgs[0]))
                    {
                        activeCommand = CallCommand(currentArgs[0]);
                        activeCommandName = currentArgs[0];
                        var done = activeCommand.Process(currentArgs) == CommandReturnCode.Done;
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

        /// <summary>
        ///     Кастомизированный вывод
        /// </summary>
        private void OutputPrompt()
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(activeCommandName);
            Console.Write(">>>");
            Console.ForegroundColor = temp;
        }

        /// <summary>
        ///     Вывод строк
        /// </summary>
        private void OutputResponse()
        {
            if (response != null && response.Length > 0)
            {
                while (response.EndsWith(Environment.NewLine) && response.Length > 0)
                    response = response.Substring(0, response.Length - 1);
                if (response.Length > 0) Console.WriteLine(response);
            }
        }

        /// <summary>
        ///     Создание и вызов нужной команды
        /// </summary>
        /// <param name="command">название команды</param>
        /// <returns>Команда</returns>
        private ICommand CallCommand(string command)
        {
            ICommand newCommand = null;
            switch (command)
            {
                case "create":
                    newCommand = new CreateCommand(productCatalog, currentArgs);
                    break;
                case "list":
                    newCommand = new ListCommand(productCatalog, currentArgs);
                    break;
                case "delete":
                    newCommand = new DeleteCommand(productCatalog, currentArgs);
                    break;
                case "add-count":
                    newCommand = new AddCountCommand(productCatalog, currentArgs);
                    break;
                case "sub-count":
                    newCommand = new SubstractCountCommand(productCatalog, currentArgs);
                    break;
                case "get-item":
                    newCommand = new GetItemCommand(productCatalog, currentArgs);
                    break;
            }

            return newCommand;
        }
    }
}