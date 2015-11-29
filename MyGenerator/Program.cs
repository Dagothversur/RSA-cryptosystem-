using System;

namespace MyGenerator
{
    internal class Program
    {
        private const int Tests = 10;

        private static void Main(string[] args)
        {
            var info = new string[2];
            Console.WriteLine("Welcome in Prime Numbers Generator!");

            A1:
            Console.WriteLine("Plase enter how many numbers u want to generate");
            info[0] = Console.ReadLine();

            Console.WriteLine("Please enter how many bits should this numbers contain");
            info[1] = Console.ReadLine();
            var results = InputChecker.CheckInput(info); //info[0]- k  info[1]-d
            if (results == null)
            {
                Console.WriteLine("Incorrect numbers. Please try again! ");
                goto A1;
            }

            /***************************************Input_Loaded*********************************************/

            Console.WriteLine("Random Prime Numbers creation started!");
            var gen = new Generator();
            gen.PrepeareThreads(results[1], Tests, results[0]);
            gen.StartThreads();
            Console.ReadKey();
        }
    }
}
