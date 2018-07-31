using System;

namespace Common.Tools
{
    public static class TokenGenerator
    {
        /// <summary>
        /// 取得token值
        /// </summary>
        /// <param name="x">碼數</param>
        /// <returns>字串</returns>
        public static string GetRandom(int x)
        {
            String Token = "";
            Random Counter = new Random(Guid.NewGuid().GetHashCode());
            while (Token.Length < x)
            {
                Token += Counter.Next().ToString();
            }
            return Token.Substring(0, x);
        }

        /// <summary>
        /// 取得TimeStamp
        /// </summary>
        /// <param name="x"></param>
        /// <returns>yyyyMMddHHmmssfff + X碼亂數 +"客製化字串"</returns>
        public static string GetTimeStamp(int x = 0, String CustomerString = "")
        {
            String ReturnToken = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            if (x > 0)
                ReturnToken += GetRandom(x);
            if (!String.IsNullOrWhiteSpace(CustomerString))
                ReturnToken += CustomerString;
            return ReturnToken;
        }
    }
}