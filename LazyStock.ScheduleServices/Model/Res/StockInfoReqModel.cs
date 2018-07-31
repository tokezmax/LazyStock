using System;
using System.Collections.Generic;

namespace LazyStock.ScheduleServices.Model
{
    public class StockInfoResModel
    {
        #region 基礎資訊

        public string StockNum { get; set; }
        public string StockName { get; set; }
        public Nullable<double> Value { get; set; }
        public string Industry { get; set; }
        public Nullable<double> DebtRatio { get; set; }
        public Nullable<double> InvestorRatio { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> PERatio { get; set; }
        public string StockBasicModifyDate { get; set; }
        public string PriceModifyDate { get; set; }
        public string RevenueYYYYMM { get; set; }
        public Nullable<double> RevenueGrowthRatio { get; set; }
        public Nullable<int> IsPromisingEPS { get; set; }
        public Nullable<int> IsGrowingUpEPS { get; set; }
        public Nullable<int> IsAlwaysIncomeEPS { get; set; }
        public Nullable<int> IsAlwaysPayDivi { get; set; }
        public Nullable<int> IsStableDivi { get; set; }
        public Nullable<int> IsGrowingUpRevenue { get; set; }
        public Nullable<int> IsBlock { get; set; }
        public Nullable<int> IsPocket { get; set; }
        public Nullable<int> IsSafeValue { get; set; }
        public Nullable<int> IsSafePB { get; set; }
        public Nullable<int> IsSafeInvestor { get; set; }
        public Nullable<int> IsSafeDebt { get; set; }
        public Nullable<int> IsUnstableEPS { get; set; }
        public Nullable<double> FutureFromEPS { get; set; }
        public Nullable<double> CurrFromEPS { get; set; }
        public Nullable<double> PrevDiviFrom3YearAvgByEPS { get; set; }
        public Nullable<double> EstimateStableDivi { get; set; }
        public Nullable<double> EstimateUnstableDivi { get; set; }
        public Nullable<double> EstimateStablePrice5 { get; set; }
        public Nullable<double> EstimateUnstablePrice5 { get; set; }
        public Nullable<double> EstimateStablePrice7 { get; set; }
        public Nullable<double> EstimateUnstablePrice7 { get; set; }
        public Nullable<double> EstimateStablePrice10 { get; set; }
        public Nullable<double> EstimateUnstablePrice10 { get; set; }

        #endregion 基礎資訊

        #region 詳細資訊

        public List<EPS_DiviDataModel> EPS_Divi { get; set; }

        #endregion 詳細資訊
    }
}