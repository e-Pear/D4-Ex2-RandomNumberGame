using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D4_Ex2_RandomNumberGame
{
    public static class DecimalExtensions
    {
        public static string ToUsString(this decimal value)
        {
            return value.ToString(CultureInfo.GetCultureInfo("en-US"));
        }

        public static int[] SplitToCharacteristicAndMantissa(this decimal value)
        {

            string buffer = value.ToUsString();
            string[] bufferArr = buffer.Split('.');

            int characteristic = 0;
            int mantissa = 0;

            Int32.TryParse(bufferArr[0], out characteristic);

            if (bufferArr.Length == 2)
            {
                if(bufferArr[1].Length > 1) Int32.TryParse(bufferArr[1], out mantissa);
                else
                {
                    while (bufferArr[1].Length < 2)
                    {
                        bufferArr[1] += "0";
                    }
                    Int32.TryParse(bufferArr[1], out mantissa);
                }
            }

            return new int[2] { characteristic, mantissa };
        }
    }
}
