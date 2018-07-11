using System;
using System.Collections;
using System.Data;

namespace Common.DataAccess
{
    /// <summary>
    /// 資料庫參數
    /// </summary>
    class DbParam
    {

        public enum LikeMode { Left, Right, Both };
        private static string LinkTemp = "And <COL1> Like <L> +@<COL2> +<R> ";

        /// <summary>
        /// 資料庫參數
        /// </summary>
        /// <param name="Value">值</param>
        /// <param name="ConditionsTemplete">篩選條件樣版</param>
        /// <returns></returns>
        public static String[] Get(String Value, String ConditionsTemplete)
        {
            return new String[] { CharFilter(Value), CharFilter(ConditionsTemplete) };
        }


        /// <summary>
        /// 產出link語法
        /// </summary>
        /// <param name="Value">值</param>
        /// <param name="Colname">欄位名稱</param>
        /// <param name="Like"></param>
        /// <returns></returns>
        public static String[] GetLike(String Value, String Colname, LikeMode Like)
        {
            string COL = CharFilter(Colname);
            string LikeString = LinkTemp.Replace("<COL1>", Colname)
                                            .Replace("<COL2>", Colname.Replace("[", "").Replace("]", ""));
            switch (Like)
            {
                case LikeMode.Right:
                    LikeString = LikeString.Replace("<R>", "'%'").Replace("<L>", "");
                    break;
                case LikeMode.Left:
                    LikeString = LikeString.Replace("<R>", "").Replace("<L>", "'%'");
                    break;
                case LikeMode.Both:
                    LikeString = LikeString.Replace("<R>", "'%'").Replace("<L>", "'%'");
                    break;
            }

            return new String[] { CharFilter(Value), LikeString };

        }

        /// <summary>
        /// 過瀘惡意字元
        /// </summary>
        /// <param name="Value">值</param>
        /// <returns>過瀘後之值</returns>
        private static string CharFilter(String Value)
        {
            return Value.Replace("'", "＇").Replace("--", "－－");
        }


    }
}
