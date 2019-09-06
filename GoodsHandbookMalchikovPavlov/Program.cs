using System;
namespace GoodsHandbookMalchikovPavlov
{
    class Program
    {
         public static void Main()
        {
            Dispatcher dispatcher = new Dispatcher();
            StringBuilder buffer = new StringBuilder();
            while (true)
            {
                if (Console.In.Peek() == -1)
                {
                    buffer.Length -= Environment.NewLine.Length;
                    dispatcher.ProcessInput(buffer.ToString());
                    buffer.Length = 0;
                }
                int read = Console.In.Read();
                if (read == -1)
                {
                    break;
                }
                else
                {
                    buffer.Append((char)read);
                }
            }

        }
    }
}
