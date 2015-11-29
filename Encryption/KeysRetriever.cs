using System;
using System.IO;
using System.Numerics;

namespace Encryption
{
    public static class KeysRetriever
    {
        public static BigInteger[] RetrievePublicKeys(string path)
        {
            var publicKeys = new BigInteger[2];
            var file = File.ReadAllText(@"" + path);
            var keys = file.Split(',');

            publicKeys[0] = BigInteger.Parse(keys[0]);
            publicKeys[1] = BigInteger.Parse(keys[1]);

            Console.WriteLine("E: " + publicKeys[0]);
            Console.WriteLine("N: " + publicKeys[1]);

            return publicKeys;
        }

        public static BigInteger RetrievePrivateKey(string path)
        {
            var bytes = File.ReadAllBytes(@"" + path);
            var privateKey = new BigInteger(bytes);
            Console.WriteLine("D: " + privateKey);
            return privateKey;
        }
    }
}
