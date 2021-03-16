using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SellThat.Extensions
{
    public static class MathExtensions
    {
        public static int Clamp(this int value, int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            if(value < minValue)
            {
                return minValue;
            }
            if(value > maxValue)
            {
                return maxValue;
            }
            return value;
        }
    }
}
