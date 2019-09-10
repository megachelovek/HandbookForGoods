using System;
using System.IO;
using System.Globalization;
using System.Text;
using System.Reflection;
namespace GoodsHandbookMalchikovPavlov
{

    class Foo
    {
        private string dick;
        public string Dick { get { return dick; } set { dick = value; } }
    }

    class Bar : Foo
    {
        private string cock;
        public string Cock { get { return cock; } set { cock = value; } }

    }

    public static class Program
    {

        public static void Main()
        {
           
            Dispatcher dispatcher = new Dispatcher();
            dispatcher.Start();

        }

    }
}


