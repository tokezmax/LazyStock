﻿namespace LazyStock.Web.Models
{
    public class EPSDataModel
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