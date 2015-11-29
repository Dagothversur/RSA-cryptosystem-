using System;
using System.Collections.Generic;
using System.Numerics;

namespace MyGenerator
{
    public static class PrimeNumberTest
    {
        private const int Max = 8;

        public static BigInteger CreateBigNumber(byte[] bytes)
        {
            return new BigInteger(bytes);
        }

        public static bool PseudoPrime(BigInteger number, int k)
        {
            if (!FermatTheorem(number)) return false;

            if (!MillerRabinTest(number, k)) return false;

            return true;
        }

        public static bool FermatTheorem(BigInteger number)
        {
            var rnd = new Random();
            var a = rnd.Next(Max) + 1;

            var leftside = BigInteger.ModPow(a, number, number);
            var rightside = BigInteger.ModPow(a, BigInteger.One, number);

            return (leftside == rightside);
        }

        public static bool MillerRabinTest(BigInteger number, int k)
        {
            //STEP 1
            BigInteger d;
            var n = number - 1;
            var s = FindK(n, out d);


            //STEP 2
            //iterate on Agenerator AGenerator//passing Zr=[0,s-1] a=[2,n-1]-wybierany losowo
            foreach (var a in GetA(number, k))
            {
                var y = BigInteger.ModPow(a, d, number);
                if (y != BigInteger.One && y != n)
                {
                    for (var r = 1; r <= s - 1; r++)
                    {
                        y = BigInteger.ModPow(y, 2, number);
                        if (y == 1)
                            return false;
                    }
                    if (y != n)
                        return false;
                }
            }
            //Step3
            return true; //it is probably prime
        }

        private static BigInteger FindK(BigInteger n, out BigInteger m)
        {
            BigInteger k = 1;
            var kFound = false;
            m = 0;

            while (!kFound)
            {
                var divisior = Power(2, k);
                var remainder = BigInteger.Remainder(n, divisior);

                if (remainder != BigInteger.Zero)
                {
                    kFound = true;
                }
                else
                {
                    m = BigInteger.Divide(n, divisior);
                    k++;
                }
            }
            k--;

            return k;
        }

        private static BigInteger Power(BigInteger n, BigInteger exponent)
        {
            var result = n;

            for (var i = 1; i < exponent; i++)
            {
                //result = result*n;
                result = BigInteger.Multiply(result, n);
            }

            return result;
        }

        private static IEnumerable<BigInteger> GetA(BigInteger limit, int k)
        {
            BigInteger a = 2;
            for (var i = 1; i <= k; i++) //iterate through tests
            {
                if (a < limit)
                {
                    yield return a;
                    a++;
                }
                else yield break;
            }
        }
    }
}
