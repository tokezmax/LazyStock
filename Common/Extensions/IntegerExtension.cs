namespace Common.Extensions
{
    public static class IntegerExtension
    {
        /// <summary>
        /// 此整數是否為奇數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsOddNumber(this sbyte number)
        {
            return (number & 1) == 1;
        }

        /// <summary>
        /// 此整數是否為奇數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsOddNumber(this byte number)
        {
            return (number & 1) == 1;
        }

        /// <summary>
        /// 此整數是否為奇數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsOddNumber(this short number)
        {
            return (number & 1) == 1;
        }

        /// <summary>
        /// 此整數是否為奇數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsOddNumber(this ushort number)
        {
            return (number & 1) == 1;
        }

        /// <summary>
        /// 此整數是否為奇數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsOddNumber(this int number)
        {
            return (number & 1) == 1;
        }

        /// <summary>
        /// 此整數是否為奇數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsOddNumber(this uint number)
        {
            return (number & 1) == 1;
        }

        /// <summary>
        /// 此整數是否為奇數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsOddNumber(this long number)
        {
            return (number & 1) == 1;
        }

        /// <summary>
        /// 此整數是否為奇數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsOddNumber(this ulong number)
        {
            return (number & 1) == 1;
        }

        /// <summary>
        /// 此整數是否為偶數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsEvenNumber(this sbyte number)
        {
            return (number & 1) == 0;
        }

        /// <summary>
        /// 此整數是否為偶數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsEvenNumber(this byte number)
        {
            return (number & 1) == 0;
        }

        /// <summary>
        /// 此整數是否為偶數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsEvenNumber(this short number)
        {
            return (number & 1) == 0;
        }

        /// <summary>
        /// 此整數是否為偶數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsEvenNumber(this ushort number)
        {
            return (number & 1) == 0;
        }

        /// <summary>
        /// 此整數是否為偶數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsEvenNumber(this int number)
        {
            return (number & 1) == 0;
        }

        /// <summary>
        /// 此整數是否為偶數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsEvenNumber(this uint number)
        {
            return (number & 1) == 0;
        }

        /// <summary>
        /// 此整數是否為偶數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsEvenNumber(this long number)
        {
            return (number & 1) == 0;
        }

        /// <summary>
        /// 此整數是否為偶數
        /// </summary>
        /// <param name="number">要被判斷的整數</param>
        /// <returns></returns>
        public static bool IsEvenNumber(this ulong number)
        {
            return (number & 1) == 0;
        }
    }
}