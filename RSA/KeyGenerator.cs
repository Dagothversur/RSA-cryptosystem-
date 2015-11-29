using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using MyGenerator;

namespace RSA
{
    public static class KeyGenerator
    {
        private const int TestPrecision = 10;
        private static BigInteger _n;
        private static BigInteger _eulerFunction;
        private static BigInteger _e, _d;
        private static string _path;

        public static void SetPath(string path)
        {
            _path = @"" + path;
        }

        private static void WriteToFile()
        {
            var publicFile = new StreamWriter(_path + @"\publicKey.txt");
            var message = _e + "," + _n;
            var hidden = _d.ToByteArray();

            publicFile.WriteLine(message);
            File.WriteAllBytes(_path + @"\privateKey.txt", hidden);
            publicFile.Close();
        }

        //*******************RSA_ALGORITHM*********************//

        public static void Rsa(Generator gen, int bufferSize)
        {
            GenerateN(gen, bufferSize);
            GENERATE:
            GenerateE(gen, bufferSize);
            try
            {
                GenerateD(gen, bufferSize);
            }
            catch (Exception)
            {
                goto GENERATE;
            }

            WriteToFile();
        }

        private static void GenerateN(Generator gen, int bufferSize)
        {
            AGAIN:
            var p = gen.ReturnPrime(bufferSize, TestPrecision);
            var q = gen.ReturnPrime(bufferSize, TestPrecision);

            if (BigInteger.Compare(p, q) == BigInteger.Zero)
                goto AGAIN;

            _n = BigInteger.Multiply(p, q);
            Fi(p, q);
        }

        private static void Fi(BigInteger p, BigInteger q)
        {
            var component1 = (BigInteger.Subtract(BigInteger.One, p));
            var component2 = (BigInteger.Subtract(BigInteger.One, q));
            _eulerFunction = BigInteger.Multiply(component1, component2);
            Console.WriteLine("FI: " + _eulerFunction);
        }

        private static void GenerateE(Generator gen, int bufferSize)
        {
            while (true)
            {
                var e = gen.ReturnPrime(bufferSize, TestPrecision);
                if (BigInteger.Compare(e, _eulerFunction) < 0)
                {
                    _e = e;
                    return;
                }
            }
        }

        private static void GenerateD(Generator gen, int bufferSize)
        {
            _d = EuclidianAlgorithm(_e, _eulerFunction);
        }

        private static BigInteger EuclidianAlgorithm(BigInteger e, BigInteger eulerFunction)
        {
            var col1 = new BigInteger[3];
            var col2 = new BigInteger[3];
            col1[0] = eulerFunction;
            col2[0] = eulerFunction;
            col1[1] = e;
            col2[1] = BigInteger.One;

            col1[2] = BigInteger.Zero;
            col2[2] = BigInteger.Zero;

            try
            {
                while (true)
                {
                    var division = BigInteger.Divide(col1[0], col1[1]);
                    var result1 = BigInteger.Multiply(col1[1], division);
                    var result2 = BigInteger.Multiply(col2[1], division);


                    col1[2] = BigInteger.Subtract(col1[0], result1);
                    col2[2] = BigInteger.Subtract(col2[0], result2);

                    if (col1[2] < BigInteger.Zero)
                    {
                        var helper = BigInteger.ModPow(col1[2], BigInteger.One, eulerFunction);
                        col1[2] = BigInteger.Add(eulerFunction, helper);
                    }

                    if (col2[2] < BigInteger.Zero)
                    {
                        var helper = BigInteger.ModPow(col2[2], BigInteger.One, eulerFunction);
                        col2[2] = BigInteger.Add(eulerFunction, helper);
                    }

                    if (col1[2] == BigInteger.One)
                    {
                        return col2[2];
                    }
                    //ROW removal

                    col1[0] = col1[1];
                    col1[1] = col1[2];
                    col1[2] = BigInteger.Zero;

                    col2[0] = col2[1];
                    col2[1] = col2[2];
                    col2[2] = BigInteger.Zero;
                }
            }
            catch (DivideByZeroException ex)
            {
                throw;
            }
        }


        //**************************ENCRYPTION***************************//

        public static void EncryptMessage(string message, BigInteger e, BigInteger n)
        {
            var ascii = Encoding.ASCII.GetBytes(message);
            var m = new BigInteger(ascii);

            var c = Encrypt(m, e, n);
            var cipherText = c.ToByteArray();
            File.WriteAllBytes(_path + @"\cipher.txt", cipherText);
        }

        private static BigInteger Encrypt(BigInteger m, BigInteger e, BigInteger n)
        {
            return BigInteger.ModPow(m, e, n);
        }

        //*************************DECRYPTION****************************//

        public static void DecryptMessage(string path, BigInteger d, BigInteger n)
        {
            var bytes = File.ReadAllBytes(path);
            var cipherText = new BigInteger(bytes);
            Decrypt(cipherText, d, n);
        }


        private static void Decrypt(BigInteger c, BigInteger d, BigInteger n)
        {
            var decryptedNumber = BigInteger.ModPow(c, d, n);
            var decryptedMessage = decryptedNumber.ToByteArray();
            var S = Encoding.ASCII.GetString(decryptedMessage);
            Console.WriteLine("Decrypted message: " + S);
        }

        //****************************ADDITIONAL_FUNCTIONS***************//

        public static List<BigInteger> GetEncryptionKeys()
        {
            var eKeys = new List<BigInteger> {_e, _n};
            return eKeys;
        }

        public static List<BigInteger> GetDecryptionKeys()
        {
            var eKeys = new List<BigInteger> {_d, _n};
            return eKeys;
        }
    }
}
