using System;

namespace Common.Extensions
{
    public static class DecimalExtension
    {
        /// <summary>
        /// 四捨五入至某小數位數
        /// </summary>
        /// <param name="value">數值</param>
        /// <param name="precision">小數位數</param>
        /// <returns></returns>
        public static decimal ToRound(this decimal value, int precision = 0)
        {
            return Math.Round(value, precision, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 無條件捨去至某小數位數
        /// </summary>
        /// <param name="value">數值</param>
        /// <param name="precision">小數位數</param>
        /// <returns></returns>
        public static decimal ToFloor(this decimal value, int precision = 0)
        {
            return Round(value, precision, Math.Floor);
        }

        /// <summary>
        /// 無條件進位至某小數位數
        /// </summary>
        /// <param name="value">數值</param>
        /// <param name="precision">小數位數</param>
        /// <returns></returns>
        public static decimal ToCeiling(this decimal value, int precision = 0)
        {
            return Round(value, precision, Math.Ceiling);
        }

        private static decimal Round(decimal value, int precision, Func<decimal, decimal> roundingFunction)
        {
            value *= (decimal)Math.Pow(10, precision);
            value = roundingFunction(value);
            return value * (decimal)Math.Pow(10, -1 * precision);
        }
    }
}
