using System;
using RSA;

namespace Encryption
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            KeyGenerator.SetPath(@"C:\Users\Simon\Desktop\Kryptografia");

            Console.WriteLine("Please enter where public keys are stored!");
            var publicKey = KeysRetriever.RetrievePublicKeys(Console.ReadLine());

            Console.WriteLine("Please enter where private keys are stored!");
            var privateKey = KeysRetriever.RetrievePrivateKey(Console.ReadLine());

            //********************************RSA_EXECUTION*********************************/

            A1:   Console.WriteLine("Please enter (E)-encryption or (D)-decryption");
            var key = Console.ReadLine();
            if (key == "E")
            {
                Console.WriteLine("Please enter message to be encrypted");
                var msg = Console.ReadLine();
                KeyGenerator.EncryptMessage(msg, publicKey[0], publicKey[1]);
                goto A1;
            }
            if (key == "D")
            {
                KeyGenerator.DecryptMessage(@"C:\Users\Simon\Desktop\Kryptografia\cipher.txt", privateKey, publicKey[1]);
                goto A1;
            }
            else
            {
                Console.WriteLine("Wrong key entered! Please enter any key to close the application.");
            }

            Console.ReadKey();
        }
    }
}
