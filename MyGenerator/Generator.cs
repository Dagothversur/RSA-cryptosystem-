using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading;

namespace MyGenerator
{
    public class Generator
    {
        private static readonly RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        private readonly List<Thread> _threadList;
        private int _size;

        /************************GENERATOR_FUNCTIONS*****************************/

        public Generator()
        {
            _threadList = new List<Thread>();
        }

        private static BigInteger GeneratePositiveNumber(byte[] buffer)
        {
            var result = BigInteger.Zero;
            var isNegative = true;

            while (isNegative)
            {
                rngCsp.GetBytes(buffer); //we fill up our array with random bits
                result = PrimeNumberTest.CreateBigNumber(buffer);
                if (result > BigInteger.Zero && !result.IsEven) isNegative = false;
            }

            return result;
        }

        private BigInteger GenerateRandomPrimeNumber(int k)
        {
            NotFound:
            var buffer = new byte[_size];

            var possiblePrime = GeneratePositiveNumber(buffer);

            if (!PrimeNumberTest.FermatTheorem(possiblePrime))
                goto NotFound;

            if (PrimeNumberTest.MillerRabinTest(possiblePrime, k))
            {
                Console.WriteLine("PrimeNumber: {0}", possiblePrime);
                return possiblePrime;
            }

            goto NotFound;
        }

        /*********************ADDITIONAL FUNCTIONS******************************/

        public BigInteger ReturnPrime(int d, int k)
        {
            _size = d/8;
            return GenerateRandomPrimeNumber(d);
        }

        public void PrepeareThreads(int d, int k, int threads)
        {
            _size = d/8;
            for (var i = 0; i < threads; i++)
            {
                _threadList.Add(new Thread(() => GenerateRandomPrimeNumber(k)));
            }
        }

        public void StartThreads()
        {
            foreach (var t in _threadList)
            {
                t.Start();
            }
        }
    }
}
