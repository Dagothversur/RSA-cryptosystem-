using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MyGenerator;

namespace CRT
{
    public static class Crt
    {
        private const int TestPrecision = 10;
        private static BigInteger _n;
        private static BigInteger _eulerFunction;
        private static BigInteger _e, _d;
        private static string _path;

        //CRT//
        private static BigInteger _dP;
        private static BigInteger _p;
        private static BigInteger _q;
        private static BigInteger _dQ;
        private static BigInteger _qInv;

        public static void SetPath(string path)
        {
            _path = @"" + path;
        }

        private static void WriteToFile()
        {
            var publicFile = new StreamWriter(_path + @"\publicKey2.txt");
            var message = _e + "," + _n;
            var hidden = _p + "," + _q + "," + _dP + "," + _dQ + "," + _qInv;
            byte[] h = System.Text.Encoding.ASCII.GetBytes(hidden);

            publicFile.WriteLine(message);
            File.WriteAllBytes(_path + @"\privateKey2.txt", h);
            publicFile.Close();
        }

        //*******************RSA_ALGORITHM*********************//

        public static void CRT(Generator gen, int bufferSize)
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
            _p = gen.ReturnPrime(bufferSize, TestPrecision);
            _q = gen.ReturnPrime(bufferSize, TestPrecision);

            if (BigInteger.Compare(_p, _q) == BigInteger.Zero)
                goto AGAIN;

            _n = BigInteger.Multiply(_p, _q);
            Fi(_p, _q);
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
            _d = BigInteger.Divide(BigInteger.One, _e);
            _dP = BigInteger.ModPow(_d, BigInteger.One, BigInteger.Subtract(_p, BigInteger.One));
            _dQ = BigInteger.ModPow(_d, BigInteger.One, BigInteger.Subtract(_q, BigInteger.One));
            _qInv = BigInteger.ModPow( (1/_q),BigInteger.One,  _p);
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

        public static void DecryptMessage(string path, List<BigInteger> d, BigInteger n)
        {
            var bytes = File.ReadAllBytes(path);
            var cipherText = new BigInteger(bytes);
            Decrypt(cipherText, _qInv, _dP, _dQ, _p, _q);

        }


        private static void Decrypt(BigInteger c, BigInteger inv, BigInteger dP, BigInteger dQ, BigInteger p,BigInteger q)
        {
            var m1 = BigInteger.ModPow(c, dP, p);
            var m2 = BigInteger.ModPow(c, dQ, q);
            var h = BigInteger.Multiply(inv, BigInteger.ModPow(m1 - m2, BigInteger.One, p));
            var m = m2 + BigInteger.Multiply(h, q);



            var decryptedMessage = m.ToByteArray();
            var S = Encoding.ASCII.GetString(decryptedMessage);
            Console.WriteLine("Decrypted message: " + S);
        }

        //****************************ADDITIONAL_FUNCTIONS***************//

        public static List<BigInteger> GetEncryptionKeys()
        {
            var eKeys = new List<BigInteger> { _e, _n };
            return eKeys;
        }

        public static List<BigInteger> GetDecryptionKeys()
        {
            var eKeys = new List<BigInteger> { _d, _n };
            return eKeys;
        }
    }
}

