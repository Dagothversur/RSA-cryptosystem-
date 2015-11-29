using System;
using MyGenerator;

namespace RSA
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("RSA Keys Generator");
            Console.WriteLine("Author: Szymon Sutek");

            A1:
            Console.WriteLine("Please enter bits number for keys size!");
            var buffer = InputChecker.CheckInput(Console.ReadLine());
            if (buffer == -1)
            {
                Console.WriteLine("Incorrect numbers. Please try again! ");
                goto A1;
            }

            //************************EXECUTING_RSA_ALGORITHM****************************//

            var gen = new Generator();
            KeyGenerator.SetPath(@"C: \Users\Simon\Desktop\Kryptografia\");
            KeyGenerator.Rsa(gen, buffer);

            //************************KEY_PAIRS_CREATED**********************************//
            var list1 = KeyGenerator.GetEncryptionKeys();
            var list2 = KeyGenerator.GetDecryptionKeys();

            Console.WriteLine("\nPrinting keys: ");
            Console.WriteLine("E is equalto: " + list1[0]);
            Console.WriteLine("D is equalto: " + list2[0]);
            Console.WriteLine("N is equalto: " + list1[1]);
            Console.ReadKey();
        }
    }
}
