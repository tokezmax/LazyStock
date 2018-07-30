var AjaxHelper = function (o) {
    "user strict";
    var opts = $.extend({
        type: "post",
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        beforeSend: function (xhr, opts) {},
        success: function (data) {},
        error: function (xhr, ajaxOptions, thrownError) {},
        complete: function () {}
    }, $.fn.BC.Setting, arguments[0] || { CalenderElement: $(this) });
    
    /* Private Propertites */
    // Create self constant in class
    var self = this;

    // initialize options parameter for definition
    var opts = options || {};

 
  
 
    var targetUrl = 'Data/GetStockInfo?StockNum=' + x + '&t=' + CommonHelper.GenRandom(5);
    $.ajax({
        type: "post",
        url: targetUrl,
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        beforeSend: function (xhr, opts) {
            //xhr.abort();
            $("#preloader").show();
            $(".StockStatus").hide();
        },
        success: function (data) {
            StockInfoData = null;
            if (!data) {
                setErrStatus("通訊異常!");
                return;
            }
            if (data.Code !== 0) {
                setErrStatus("[" + data.Code + "]" + data.Message);
                return;
            }
            StockInfoData = data.Result;

            $("#RevenueGrowthRatio").attr("title", "[" + StockInfoData.RevenueYYYYMM + "]累計:" + StockInfoData.RevenueGrowthRatio + "%");
            $("#PriceModifyDate").attr("title", "最後更新時間:" + StockInfoData.PriceModifyDate);
            $("#StockInfoNum").html(StockInfoData.StockNum);
            $("#StockInfoName").html(StockInfoData.StockName);
            $("#StockInfoIndustry").html(StockInfoData.Industry);
            $("#StockInfoPrice").html(StockInfoData.Price);
            //取得去年Divi
            var PrevYearDivi = parseFloat(StockInfoData.EPS_Divi[1].TotalDivi);
            var CurrPrice = parseFloat(StockInfoData.Price)
            var CurrDivi = new Number(PrevYearDivi / CurrPrice);
            CurrDivi = Math.floor(CurrDivi * 10000) / 100;
            $("#StockInfoCurrDivi").html(CurrDivi).append('%');


            setStockStatus("IsGrowingUpRevenue", StockInfoData.IsGrowingUpRevenue);

            setStockStatus("IsPromisingEPS", StockInfoData.IsPromisingEPS);
            setStockStatus("IsGrowingUpEPS", StockInfoData.IsGrowingUpEPS);
            setStockStatus("IsAlwaysIncomeEPS", StockInfoData.IsAlwaysIncomeEPS);

            setStockStatus("IsAlwaysPayDivi", StockInfoData.IsAlwaysPayDivi);
            setStockStatus("IsStableDivi", StockInfoData.IsStableDivi);

            setStockStatus("IsSafeDebt", StockInfoData.IsSafeDebt);
            setStockStatus("IsSafePB", StockInfoData.IsSafePB);
            setStockStatus("IsSafeInvestor", StockInfoData.IsSafeInvestor);
            setStockStatus("IsSafeValue", StockInfoData.IsSafeValue);


            $(".StockStatus").show();
            setStgPrice();
            setRiskStatus();


            $('.contact ').eq(0).html('<i class="fa fa-phone"></i><h3> 基本資料</h3>');
            $('.contact ').eq(1).html('<i class="fa fa-phone"></i><h3> 年份／股息</h3>');
            $('.contact ').eq(2).html('<i class="fa fa-phone"></i><h3> 年季／EPS／利息比</h3>');

            $('.contact ').eq(0).append('<P>股名: ' + StockInfoData.StockName + "</P>");
            $('.contact ').eq(0).append('<P>股價: ' + StockInfoData.Price + "</P>");
            $('.contact ').eq(0).append('<P>產業: ' + StockInfoData.Industry + "</P>");
            $('.contact ').eq(0).append('<P>本益比: ' + StockInfoData.PERatio + "</P>");
            $('.contact ').eq(0).append('<P>大戶持股(%): ' + StockInfoData.InvestorRatio + "</P>");
            $('.contact ').eq(0).append('<P>負債比: ' + StockInfoData.DebtRatio + "%</P>");
            $('.contact ').eq(0).append('<P>淨值（億）: ' + StockInfoData.Value + "</P>");

            $.each(StockInfoData.EPS_Divi, function (key, value) {
                $('.contact ').eq(2).append(value.Year + value.LastQ + '／' + CommonHelper.RoundX(value.TotalEPS, 2) + '／' + (value.EachDiviFromEPS == null ? 0 : +value.EachDiviFromEPS) + '<BR>');
                if (value.LastQ === 'Q4')
                    $('.contact ').eq(1).append(value.Year + value.LastQ + '／' + value.TotalDivi + "<BR>");
            });

        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('通訊異常');
        },
        complete: function () {
            $("#preloader").delay(100).fadeOut();
        }
    });
}
var Ajax = function (options) {
    "user strict";
    /* Private Propertites */
    // Create self constant in class
    var self = this;

    // initialize options parameter for definition
    var o = options || {};
    // Flag for loadData()
    var isLoading = false;

    /* Public Propertites */
    // Tree data
    self.data = o.data || null;
    // AJAX URL for loadData()
    self.ajaxUrl = o.ajaxUrl || '/org3layermenu/ajax';

    /* Constructor */
    var __constructor = function () {

        // Construct code
    }

    /* <public method> Initialize with callback function */
    self.init = function (callback) {

        loadData(callback);
    }

    /* <public method> Render */
    self.render = function (options) {
        // Load data if no data 
        if (self.data === null) {
            loadData(function () {
                self.render(options)
            });
            return;
        }
        console.log(self.data);
        // Render code
    }

    /* <private method> Load data from AJAX */
    var loadData = function (callback) {

        // Check if is loading
        if (isLoading) {
            // Retry later
            setTimeout(function () {
                callback();
            }, 500);
            return;
        }

        isLoading = true;
        // Load data by AJAX
        
        $.get(self.ajaxUrl, function (data) {
            self.data = data;
            isLoading = false;
            callback();
        });
    }

    // Activate constructor
    __constructor();
}