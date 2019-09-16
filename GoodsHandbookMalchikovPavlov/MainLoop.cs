using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using GoodsHandbookMalchikovPavlov.Model;
using GoodsHandbookMalchikovPavlov.Validators;

namespace GoodsHandbookMalchikovPavlov
{
    internal sealed class ProductCachedStuff
    {
        public string ProductName { get; set; }

        public PropertyInfo[] PropertyInfoArray { get; set; }

        public string[] ProperPropertyNames { get; set; }
    }

    internal enum Command
    {
        None,
        Create,
        List,
        AddRange,
        DeleteRange
    }

    public sealed class MainLoop
    {
        private static readonly string USAGE_TEXT =
            "Program can be used to create, edit and view products information\n" +
            "Usage: This is an interactive kind of program. Type any command name listed below\n" +
            "and go through steps it takes to acomplish your task\n" +
            "create [product name] - to create a new record of a given product type\n" +
            "list [product name]   - to list product records, optionally filter by product type\n" +
            "add_range [product id] [product count]   - to add count of item to product with this ID\n" +
            "delete_range [product id] [product count]   - to delete count of item to product with this ID\n" +
            "help [command name]   - to get detailed information about a given command if applicable\n";

        private readonly Dictionary<string, Command> commandMap = new Dictionary<string, Command>
        {
            {"create", Command.Create},
            {"list", Command.List},
            {"add_range", Command.AddRange},
            {"delete_range", Command.DeleteRange}
        };

        private readonly Dictionary<string, Type> nameToProductTypeMap;
        private readonly Dictionary<Type, ProductCachedStuff> productMap;
        private readonly Dictionary<Type, ProductValidator> validatorMap;

        private readonly Dictionary<int, Product> storage = new Dictionary<int, Product>();

        private bool attemptingToExit;

        private Command activeCommand = Command.None;
        private string promptPrefix = ">";

        private readonly StringBuilder inputBuffer = new StringBuilder();
        private readonly StringBuilder outputBuffer = new StringBuilder();
        private bool outputAttention;

        private bool interruptedByCtrlCombination;
        private ConsoleKey keyPressedWithCtrl;

        private Type createProductType;
        private Product createProduct;
        private bool createInputRequested;
        private int createPropertyIndex;
        private ProductCachedStuff createProductStuff;
        private ProductValidator createValidator;
        private int createProductsById = 0;

        private Type listProductType;
        private bool listInputRequested;

        public MainLoop()
        {
            nameToProductTypeMap = new Dictionary<string, Type>();
            validatorMap = new Dictionary<Type, ProductValidator>();
            Type[] productTypes =
            {
                typeof(Toy),
                typeof(Book),
                typeof(HomeAppliances)
            };
            ProductValidator[] validators =
            {
                new ToyValidator(),
                new BookValidator(),
                new HomeAppliancesValidator()
            };
            for (var i = 0; i < productTypes.Length; i++)
            {
                var type = productTypes[i];
                nameToProductTypeMap.Add(ReflectionMisc.GetTypeName(type), type);
                validatorMap.Add(type, validators[i]);
            }

            productMap = CreateDictionaryOfCachedProductStuff(productTypes);

            storage.Add(0,
                new Toy
                {
                    Id = 0, Name = "Barby", Company = "Microsoft", Price = 0.07f, Count = 0, Unit = "things",
                    StartAge = 0, EndAge = 12, Sex = "Male", Type = "Doll"
                });
            storage.Add(1,
                new Book
                {
                    Id = 1, Name = "Tower", Company = "Ubisoft", Price = 6.37f, Count = 5, Unit = "things",
                    Author = "King", Genre = "Mystic", Year = 1999
                });
            storage.Add(2,
                new HomeAppliances
                {
                    Id = 2, Name = "TV", Company = "Amazon", Price = 600.07f, Count = 7, Unit = "things", Type = "TV",
                    Model = "GH_90", Description = "someshit"
                });
        }

        public void Begin()
        {
            outputBuffer.Append(USAGE_TEXT);
            OutputResponse();
            var isRunning = true;
            while (isRunning)
            {
                OutputPrompt();
                GatherUserInput();
                isRunning = !ProcessUserInput();
                OutputResponse();
            }
        }

        private void GatherUserInput()
        {
            Console.TreatControlCAsInput = true;
            inputBuffer.Length = 0;
            var inputBufferIndex = 0;
            var promptPrefixLength = promptPrefix.Length;
            interruptedByCtrlCombination = false;

            while (true)
            {
                var keyInfo = Console.ReadKey(true);

                var printable = !char.IsControl(keyInfo.KeyChar);

                if (keyInfo.Modifiers == ConsoleModifiers.Control)
                {
                    interruptedByCtrlCombination = true;
                    keyPressedWithCtrl = keyInfo.Key;
                    break;
                }

                if (keyInfo.Key == ConsoleKey.Enter) break;

                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (inputBufferIndex > 0)
                    {
                        ClearPromptLine(inputBuffer.Length, promptPrefixLength);

                        inputBufferIndex--;
                        inputBuffer.Remove(inputBufferIndex, 1);

                        Console.Write(inputBuffer.ToString());
                        Console.SetCursorPosition(promptPrefixLength + inputBufferIndex, Console.CursorTop);
                    }
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    if (inputBufferIndex > 0)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        inputBufferIndex--;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    if (inputBufferIndex < inputBuffer.Length)
                    {
                        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                        inputBufferIndex++;
                    }
                }
                else if (printable)
                {
                    inputBuffer.Insert(inputBufferIndex++, keyInfo.KeyChar);
                    ClearPromptLine(inputBuffer.Length, promptPrefixLength);

                    Console.Write(inputBuffer.ToString());
                    Console.SetCursorPosition(promptPrefixLength + inputBufferIndex, Console.CursorTop);
                }
            }
        }

        private void OutputPrompt()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            Console.Write(promptPrefix);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void OutputResponse()
        {
            var response = outputBuffer.ToString();
            if (response.Length > 0)
            {
                var stockColor = Console.BackgroundColor;
                if (outputAttention) Console.BackgroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(0, Console.CursorTop + 1);
                Console.Write(response);
                if (outputAttention) Console.BackgroundColor = stockColor;
            }
        }

        private bool ProcessUserInput()
        {
            outputBuffer.Length = 0;
            outputAttention = false;
            if (!interruptedByCtrlCombination)
                return ProcessInput();
            return ProcessCtrlCombinations();
        }

        private bool ProcessInput()
        {
            if (attemptingToExit) attemptingToExit = false;
            if (activeCommand == Command.None) ParseCommandAndArguments();
            switch (activeCommand)
            {
                case Command.None:
                {
                    outputBuffer.Append(USAGE_TEXT);
                }
                    break;
                case Command.Create:
                {
                    if (ProcessCreateCommand()) activeCommand = Command.None;
                }
                    break;
                case Command.List:
                {
                    if (ProcessListCommand()) activeCommand = Command.None;
                }
                    break;
                case Command.AddRange:
                {
                    if (ProccessAddRange()) activeCommand = Command.None;
                }
                    break;
                case Command.DeleteRange:
                {
                    if (ProccessDeleteRange()) activeCommand = Command.None;
                }
                    break;
            }

            UpdatePromptPrefix();
            return false;
        }

        private bool ProcessCtrlCombinations()
        {
            if (keyPressedWithCtrl == ConsoleKey.Q)
            {
                if (attemptingToExit) return true;
                outputBuffer.Append("Exit? Press CTRL+Q again to exit\n");
                outputAttention = true;
                attemptingToExit = true;
            }
            else if (attemptingToExit)
            {
                attemptingToExit = false;
            }

            return false;
        }

        private void ParseCommandAndArguments()
        {
            var args = inputBuffer.ToString().Split(' ', '\t');
            if (args.Length > 0)
                if (commandMap.ContainsKey(args[0]))
                {
                    activeCommand = commandMap[args[0]];
                    switch (activeCommand)
                    {
                        case Command.None:
                        {
                            return;
                        }
                        case Command.Create:
                        {
                            if (args.Length == 1)
                            {
                            }
                            else if (args.Length == 2)
                            {
                                if (nameToProductTypeMap.ContainsKey(args[1]))
                                {
                                    var productType = nameToProductTypeMap[args[1]];
                                    InitCreateCommand(productType);
                                }
                            }
                            else if (args.Length > 2)
                            {
                                Debug.Assert(false);
                            }
                        }
                            break;
                        case Command.List:
                        {
                            if (args.Length == 1)
                            {
                            }
                            else if (args.Length == 2)
                            {
                                if (nameToProductTypeMap.ContainsKey(args[1]))
                                    listProductType = nameToProductTypeMap[args[1]];
                            }
                            else if (args.Length > 2)
                            {
                                Debug.Assert(false);
                            }
                        }
                            break;
                    }
                }
        }

        private bool ProcessCreateCommand()
        {
            if (createInputRequested)
            {
                if (createProductType == null)
                {
                    var arg = inputBuffer.ToString();
                    if (nameToProductTypeMap.ContainsKey(arg))
                    {
                        var productType = nameToProductTypeMap[arg];
                        InitCreateCommand(productType);
                        var info = createProductStuff.PropertyInfoArray[createPropertyIndex];

                        if (Attribute.IsDefined(info, typeof(AutoIdAttribute)))
                        {
                            var productId = GetFreeProductId(storage);
                            info.SetValue(createProduct, productId);
                            createPropertyIndex++;
                        }

                        OutputProductPropertyValueRequest(createProductType, createPropertyIndex);
                    }
                    else
                    {
                        OutputProductNameRequest();
                    }
                }
                else
                {
                    var info = createProductStuff.PropertyInfoArray[createPropertyIndex];

                    var validateSuccess = createValidator.Validate(createProductType, info, inputBuffer.ToString());
                    if (validateSuccess)
                    {
                        info.SetValue(createProduct, createValidator.GetLastProperty());


                        if (createPropertyIndex + 1 < createProductStuff.PropertyInfoArray.Length)
                        {
                            createPropertyIndex++;
                        }
                        else
                        {
                            storage.Add(createProduct.Id, createProduct);
                            createProductType = null;
                            createProduct = null;
                            createProductStuff = null;
                            createValidator = null;
                            createPropertyIndex = 0;
                            createInputRequested = false;
                            outputBuffer.Append("Product has been successfully created");
                            return true;
                        }
                    }
                    else
                    {
                        outputBuffer.Append(createValidator.GetLastError());
                        outputAttention = true;
                    }

                    info = createProductStuff.PropertyInfoArray[createPropertyIndex];
                    if (Attribute.IsDefined(info, typeof(AutoIdAttribute)))
                    {
                        var productId = GetFreeProductId(storage);
                        info.SetValue(createProduct, productId);
                        createPropertyIndex++;
                    }

                    OutputProductPropertyValueRequest(createProductType, createPropertyIndex);
                }
            }
            else
            {
                if (createProductType == null)
                    OutputProductNameRequest();
                else
                    OutputProductPropertyValueRequest(createProductType, createPropertyIndex);

                createInputRequested = true;
            }

            return false;
        }

        private bool ProcessListCommand()
        {
            if (listProductType == null)
            {
                if (listInputRequested)
                {
                    var arg = inputBuffer.ToString();
                    if (nameToProductTypeMap.ContainsKey(arg))
                    {
                        listProductType = nameToProductTypeMap[arg];
                        MakeProductRecordsListString(storage, productMap, listProductType, outputBuffer);
                        listProductType = null;
                        listInputRequested = false;
                        return true;
                    }

                    outputBuffer.Append("Records of all Product types will be listed\n");
                    MakeProductRecordsListString(storage, productMap, listProductType, outputBuffer);
                    listProductType = null;
                    listInputRequested = false;
                    return true;
                }

                outputBuffer.Append("You can filter records by Product type name\n");
                OutputProductNameRequest();
                listInputRequested = true;
                return false;
            }

            MakeProductRecordsListString(storage, productMap, listProductType, outputBuffer);
            listProductType = null;
            listInputRequested = false;
            return true;
        }

        private bool ProccessAddRange()
        {
            var arg = inputBuffer.ToString().Split(' ');
            if (arg.Length == 2)
            {
                var currentItem = storage[Convert.ToInt32(arg[0])];
                currentItem.Count += Convert.ToInt32(arg[1]);
                outputBuffer.Append("Product has been successfully updated\n");
                outputBuffer.Append(
                    $"Product {currentItem.Name} (ID={currentItem.Id}) current amount is {currentItem.Count} \n");
                return true;
            }

            OutputProductAddRangeRequest();

            return false;
        }

        private bool ProccessDeleteRange()
        {
            var arg = inputBuffer.ToString().Split(' ');
            if (arg.Length == 2)
            {
                var currentItem = storage[Convert.ToInt32(arg[0])];
                if (currentItem.Count < Convert.ToInt32(arg[1]))
                {
                    OutputProductDeleteRangeRequest(currentItem.Count);
                }
                else
                {
                    currentItem.Count = currentItem.Count - Convert.ToInt32(arg[1]);
                    outputBuffer.Append("Product has been successfully updated\n");
                    outputBuffer.Append(
                        $"Product {currentItem.Name} (ID={currentItem.Id}) current amount is {currentItem.Count} \n");
                    return true;
                }
            }
            else
            {
                OutputProductDeleteRangeRequest();
            }

            return false;
        }

        private void InitCreateCommand(Type productType)
        {
            createProductType = productType;
            createProduct = (Product) Activator.CreateInstance(createProductType);
            createProductStuff = productMap[createProductType];
            createValidator = validatorMap[createProductType];
            createPropertyIndex = 0;
        }

        private void OutputProductNameRequest()
        {
            outputBuffer.Append("List of product type names:\n");
            foreach (var pair in nameToProductTypeMap)
            {
                outputBuffer.Append("- ");
                outputBuffer.Append(pair.Key);
                outputBuffer.Append("\n");
            }

            outputBuffer.Append("Enter product type name:");
        }

        private void OutputProductPropertyValueRequest(Type productType, int index)
        {
            var name = productMap[productType].ProperPropertyNames[index];
            outputBuffer.Append(string.Format("Enter Value for \"{0}\"\n", name));
        }

        private void OutputProductAddRangeRequest()
        {
            outputBuffer.Append("Enter Id product and count");
        }

        private void OutputProductDeleteRangeRequest(object currentCount = null)
        {
            if (currentCount == null)
                outputBuffer.Append("Enter Id product and count to delete [123 10]");
            else
                outputBuffer.Append($"Count to delete should be less than {currentCount}");
        }

        private void UpdatePromptPrefix()
        {
            if (activeCommand != Command.None)
                promptPrefix = activeCommand + ">";
            else
                promptPrefix = ">";
        }

        private void ClearPromptLine(int lineLength, int prefixOffset)
        {
            Console.SetCursorPosition(prefixOffset, Console.CursorTop);
            for (var i = 0; i < lineLength; i++)
                Console.Write(" ");
            Console.SetCursorPosition(prefixOffset, Console.CursorTop);
        }

        private static ProductCachedStuff CacheProductStuff(Type productType)
        {
            var propertyInfoArray = productType.GetProperties();

            var properPropertyNames = new string[propertyInfoArray.Length];
            for (var i = 0; i < propertyInfoArray.Length; i++)
            {
                var info = propertyInfoArray[i];
                properPropertyNames[i] = ReflectionMisc.GetPropertyName(info);
            }

            var result = new ProductCachedStuff();
            result.ProductName = ReflectionMisc.GetTypeName(productType);
            result.PropertyInfoArray = propertyInfoArray;
            result.ProperPropertyNames = properPropertyNames;
            return result;
        }

        private static Dictionary<Type, ProductCachedStuff>
            CreateDictionaryOfCachedProductStuff(Type[] productTypes)
        {
            var result = new Dictionary<Type, ProductCachedStuff>();
            foreach (var type in productTypes) result.Add(type, CacheProductStuff(type));
            return result;
        }

        private static void MakeProductPropertyString(Product product, PropertyInfo[] infoArray, string[] properNames,
            int index, StringBuilder buffer)
        {
            var info = infoArray[index];
            var name = properNames[index];
            var value = info.GetValue(product).ToString();
            buffer.Append(string.Format("Name: \"{0}\" Value: {1}\n", name, value));
        }

        private static void MakeProductPropertyPromptString(PropertyInfo[] infoArray, int index, StringBuilder buffer)
        {
            var info = infoArray[index];
            var name = ReflectionMisc.GetPropertyName(info);
            buffer.Append(string.Format("Enter Value for \"{0}\"\n", name));
        }

        private static void MakeProductRecordsListString(Dictionary<int, Product> products,
            Dictionary<Type, ProductCachedStuff> productCachedStuffDict, Type filterProduct, StringBuilder buffer)
        {
            foreach (var pair in products)
            {
                var type = pair.Value.GetType();
                if (filterProduct != null)
                {
                    if (type.Equals(filterProduct))
                    {
                        Debug.Assert(productCachedStuffDict.ContainsKey(type));
                        var cache = productCachedStuffDict[type];
                        buffer.Append(string.Format("Product name: \"{0}\"\n", cache.ProductName));
                        for (var i = 0; i < cache.PropertyInfoArray.Length; i++)
                            MakeProductPropertyString(pair.Value, cache.PropertyInfoArray, cache.ProperPropertyNames, i,
                                buffer);
                    }
                }
                else
                {
                    Debug.Assert(productCachedStuffDict.ContainsKey(type));
                    var cache = productCachedStuffDict[type];
                    buffer.Append(string.Format("Product name: \"{0}\"\n", cache.ProductName));
                    for (var i = 0; i < cache.PropertyInfoArray.Length; i++)
                        MakeProductPropertyString(pair.Value, cache.PropertyInfoArray, cache.ProperPropertyNames, i,
                            buffer);
                }
            }
        }

        private static int GetFreeProductId(Dictionary<int, Product> products)
        {
            return products.Keys.Count;
        }

#if false
        private static bool TrySetPropertyOnProduct(Product product, int index, string value,
            Dictionary<Type, ProductCachedStuff> productCachedStuffDict, StringBuilder buffer)
        {
            Type type = product.GetType();
            Debug.Assert(productCachedStuffDict.ContainsKey(type));
            ProductCachedStuff cache = productCachedStuffDict[type];
            ProductValidator validator = productCachedStuffDict[type].Validator;
            if (validator.Validate(type, cache.PropertyInfoArray[index], value))
            {
                cache.PropertyInfoArray[index].SetValue(product, validator.GetLastProperty());
                return true;
            }
            else
            {
                buffer.Append(validator.GetLastError());
                return false;
            }
        }
#endif
    }
}