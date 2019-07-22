using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class RandomUtil
    {
        protected static readonly RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        protected static byte[] randomNumber = new byte[4];

        public static int NextByte(int value)
        {
            if (value == 0)
                throw new DivideByZeroException();
            rngCsp.GetBytes(randomNumber);
            return Math.Abs(randomNumber[0] % value);
        }

        public static int NextInt(int value)
        {
            if (value == 0)
                throw new DivideByZeroException();
            rngCsp.GetBytes(randomNumber);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(randomNumber);
            return Math.Abs(BitConverter.ToInt32(randomNumber, 0) % value);
        }

        public static int NextInt(int value, int value1)
        {
            if (value == 0)
                throw new DivideByZeroException();
            rngCsp.GetBytes(randomNumber);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(randomNumber);
            if (value1 - value == 0)
                return value;

            return value + Math.Abs(BitConverter.ToInt32(randomNumber, 0) % (value1 - value));
        }
    }
}
