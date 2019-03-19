using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppUI
{
    class Program
    {
        private static string[] colors = { "MAGENTA", "RED", "WHITE", "BLUE", "CYAN" };       
        private static string[] removeColors = { "RED", "WHITE", "BLUE" };



        static void Main(string[] args)
        {
            
            foreach (var item in colors)
            {
                Console.WriteLine("Color: {0}", item);
            }
            Console.ReadKey();
        }
    }
}
