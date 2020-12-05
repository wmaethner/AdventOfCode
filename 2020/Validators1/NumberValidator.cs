using System;
namespace Validators
{
    public static class NumberValidator
    {

        public static bool IsNumeric(this string str) => double.TryParse(str, out _);
        public static bool IsInteger(this string str) => int.TryParse(str, out _);

        #region Converters
        public static double ToDouble(this string str)
        {
            double dbl;

            if (!double.TryParse(str, out dbl))
            {
                throw new ArgumentException($"String {str} is not castable to a double.");
            }

            return dbl;
        }
        public static int? ToInt(this string str)
        {
            int num;

            if (!int.TryParse(str, out num))
            {
                throw new ArgumentException($"String {str} is not castable to a int.");
            }

            return num;
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="min">inclusive</param>
        /// <param name="max">inclusive</param>
        /// <returns></returns>
        public static bool ValidNumberRange(this string str, int min, int max)
        {
            if (!str.IsNumeric())
            {
                return false;
            }

            double number = str.ToDouble();
            return ValidNumberRange(number, min, max);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="min">inclusive</param>
        /// <param name="max">inclusive</param>
        /// <returns></returns>
        public static bool ValidNumberRange(this int number, int min, int max)
        {
            return number >= min && number <= max;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="min">inclusive</param>
        /// <param name="max">inclusive</param>
        /// <returns></returns>
        public static bool ValidNumberRange(this double number, int min, int max)
        {
            return number >= min && number <= max;
        }
    }
}
