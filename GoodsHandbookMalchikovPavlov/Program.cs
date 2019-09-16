using System;

namespace GoodsHandbookMalchikovPavlov
{
    public static class Program
    {
        private const string _intro =
            "\n░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░\r\n░███╔╗╔╗█████████╔╗╔╗███████╔╗█████░\r\n░███║╚╝║╔═╗█╔═╦╗╔╝║║╚╗╔═╗╔═╗║╠╗████░\r\n░███║╔╗║║╬╚╗║║║║║╬║║╬║║╬║║╬║║═╣████░\r\n░███╚╝╚╝╚══╝╚╩═╝╚═╝╚═╝╚═╝╚═╝╚╩╝████░\r\n░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░\r\n\r";

        public static void Main()
        {
            Console.WriteLine("Press any button to start\n");
            Console.ReadKey();
            Console.WriteLine(_intro);
            var mainLoop = new MainLoop();
            mainLoop.Begin();
        }
    }
}