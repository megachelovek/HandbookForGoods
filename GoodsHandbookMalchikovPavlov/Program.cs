using System;
using System.IO;
using System.Globalization;
using System.Text;
using System.Reflection;
namespace GoodsHandbookMalchikovPavlov
{

    public static class Program
    {

        public static void Main()
        {

            //Dispatcher dispatcher = new Dispatcher();
           //dispatcher.Start();

           MainLoop mainLoop = new MainLoop();
           mainLoop.Begin();

        }

    }
}


