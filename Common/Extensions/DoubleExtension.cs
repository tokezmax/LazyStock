using System;

namespace Common.Extensions
{
    public static class DoubleExtension
    {
        /// <summary>
        /// 四捨五入至某小數位數
        /// </summary>
        /// <param name="value">數值</param>
        /// <param name="precision">小數位數</param>
        /// <returns></returns>
        public static double ToRound(this double value, int precision = 0)
        {
            return Math.Round(value, precision, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 無條件捨去至某小數位數
        /// </summary>
        /// <param name="value">數值</param>
        /// <param name="precision">小數位數</param>
        /// <returns></returns>
        public static double ToFloor(this double value, int precision = 0)
        {
            return Round(value, precision, Math.Floor);
        }

        /// <summary>
        /// 無條件進位至某小數位數
        /// </summary>
        /// <param name="value">數值</param>
        /// <param name="precision">小數位數</param>
        /// <returns></returns>
        public static double ToCeiling(this double value, int precision = 0)
        {
            return Round(value, precision, Math.Ceiling);
        }

        private static double Round(double value, int precision, Func<double, double> roundingFunction)
        {
            value *= Math.Pow(10, precision);
            value = roundingFunction(value);
            return value * Math.Pow(10, -1 * precision);
        }
    }
}