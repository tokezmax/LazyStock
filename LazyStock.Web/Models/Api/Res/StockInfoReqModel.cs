using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace LazyStock.Web.Models
{
    public class StockInfoResModel
    {
        public StockInfoResModel()
        {
            this.Ver = Common.Tools.TokenGenerator.GetTimeStamp(3);
        }
        #region 基礎資訊
        /// <summary>
        /// 股票號碼
        /// </summary>
        public String Ver { get; set; }
        /// <summary>
        /// 股票號碼
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 股票名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 股票現價
        /// </summary>
        public float Price { get; set; }

        /// <summary>
        /// 產業別
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// 本益比
        /// </summary>
        public float PERatio { get; set; }
        /// <summary>
        /// 大戶持股比
        /// </summary>
        public float InvestorRatio { get; set; }
        /// <summary>
        /// 負債比
        /// </summary>
        public float DebtRatio { get; set; }
        /// <summary>
        /// 市值(億)
        /// </summary>
        public int Value { get; set; }


        /// <summary>
        /// 資料修改日期
        /// </summary>
        public string ModifyDate { get; set; }
        #endregion

        #region 決策分析數據
        /// <summary>
        /// 今年是否大於前年
        /// </summary>
        public int IsPromisingEPS { get; set; }
        /// <summary>
        /// EPS是否持續成長
        /// </summary>
        public int IsGrowingUpEPS { get; set; }
        /// <summary>
        /// 近三年，是否eps都是正的
        /// </summary>
        public int IsAlwaysIncomeEPS { get; set; }

        /// <summary>
        /// 近三年，是否都有配息
        /// </summary>
        public int IsAlwaysPayDivi { get; set; }
        /// <summary>
        /// 配息穩定性是否小於0.2
        /// </summary>
        public int IsOverDiffDivi { get; set; }

        /// <summary>
        /// 市值是否大於30億
        /// </summary>
        public int IsSafeValue { get; set; }
        /// <summary>
        /// 本益比是否小於20
        /// </summary>
        public int IsSafePB { get; set; }
        /// <summary>
        /// 大戶比重是否大於25%
        /// </summary>
        public int IsSafeInvestor { get; set; }

        /// <summary>
        /// 負債比是否小於55%
        /// </summary>
        public int IsSafeDebt { get; set; }


        /// <summary>
        /// 推測未來EPS
        /// </summary>
        public float FutureFromEPS { get; set; }

        /// <summary>
        /// 每點EPS配多少股息股利(參考過去三年)
        /// </summary>
        public float PrevDiviFrom3YearAvgByEPS { get; set; }

        /// <summary>
        /// 當前EPS
        /// </summary>
        public float CurrFromEPS { get; set; }
        #endregion

        #region 詳細資訊
        /// <summary>
        /// 股息股利(年)
        /// </summary>
        public List<EPS_Divi> EPS_Divi { get; set; }
        /// <summary>
        /// 股息資訊
        /// </summary>
        public List<Dividend> Dividends { get; set; }

        /// <summary>
        /// 季EPS
        /// </summary>
        public List<EP> EPS { get; set; }
        #endregion
    }

    public class EPS_Divi
    {
        public int StockNum { get; set; }
        public string Year { get; set; }
        public string LastQ { get; set; }
        public float TotalEPS { get; set; }
        public float TotalDivi { get; set; }
        public float EachDiviFromEPS { get; set; }
    }

    public class Dividend
    {
        public int Num { get; set; }
        public string Year { get; set; }
        public float CashDivi { get; set; }
        public float Divi { get; set; }
        public float AllDivi { get; set; }
    }

    public class EP
    {
        public string Index { get; set; }
        public int Num { get; set; }
        public int Year { get; set; }
        public string Q { get; set; }
        public float EPS { get; set; }
        public float KeepEPS { get; set; }
        public float Quarter3EPSAvg { get; set; }
    }
}