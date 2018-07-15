(function($) {
	"use strict"

	///////////////////////////
	// Preloader
	$(window).on('load', function() {
		$("#preloader").delay(600).fadeOut();
	});

	///////////////////////////
	// Scrollspy
	$('body').scrollspy({
		target: '#nav',
		offset: $(window).height() / 2
	});

	///////////////////////////
	// Smooth scroll
	$("#nav .main-nav a[href^='#']").on('click', function(e) {
		e.preventDefault();
		var hash = this.hash;
		$('html, body').animate({
			scrollTop: $(this.hash).offset().top
		}, 600);
	});

	$('#back-to-top').on('click', function(){
		$('body,html').animate({
			scrollTop: 0
		}, 600);
	});

	///////////////////////////
	// Btn nav collapse
	$('#nav .nav-collapse').on('click', function() {
		$('#nav').toggleClass('open');
	});

	///////////////////////////
	// Mobile dropdown
	$('.has-dropdown a').on('click', function() {
		$(this).parent().toggleClass('open-drop');
	});

	///////////////////////////
	// On Scroll
	$(window).on('scroll', function() {
		var wScroll = $(this).scrollTop();

		// Fixed nav
		wScroll > 1 ? $('#nav').addClass('fixed-nav') : $('#nav').removeClass('fixed-nav');

		// Back To Top Appear
		wScroll > 700 ? $('#back-to-top').fadeIn() : $('#back-to-top').fadeOut();
	});

	///////////////////////////
	// magnificPopup
	$('.work').magnificPopup({
		delegate: '.lightbox',
		type: 'image'
	});

	///////////////////////////
	// Owl Carousel
	$('#about-slider').owlCarousel({
		items:1,
		loop:true,
		margin:15,
		nav: true,
		navText : ['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
		dots : true,
		autoplay : true,
		animateOut: 'fadeOut'
	});

	$('#testimonial-slider').owlCarousel({
		loop:true,
		margin:15,
		dots : true,
		nav: false,
		autoplay : true,
		responsive:{
			0: {
				items:1
			},
			992:{
				items:2
			}
		}
	});

    $('#btnQuery').click(function () {
        var c = $("#StockNum").val();
        if (c.length !== 4) { 
            alert('請輸入正確的股票編號');
            return;
        }
        if (c === '') {
            return;
        }

        doQuery(c);
    });
    $(".StockStatus").hide();


    $("#WishDivi").change(function () {
        setStgPrice()
    });
    $("#btnStartNow").click(function () {
        $('a#alinkGoContact').click();
    });
    
    $('#btnReg').magnificPopup({
        items:[{
            src: '#frmReg', // CSS selector of an element on page that should be used as a popup
            type: 'iframe',
            iframe: {
                dailymotion: {
                    src: 'LineLoginAuth.aspx'
                }
            }
        }]
    });
    

    /*
    $('#btnReg').magnificPopup({
        type: 'iframe',
        iframe: {
            patterns: {
                dailymotion: {
                    index: 'dailymotion.com',
                    id: function (url) {
                        var m = url.match(/^.+dailymotion.com\/(video|hub)\/([^_]+)[^#]*(#video=([^_&]+))?/);
                        if (m !== null) {
                            if (m[4] !== undefined) {

                                return m[4];
                            }
                            return m[2];
                        }
                        return null;
                    },

                    src: 'LineLoginAuth.aspx'

                }
            }
        }


    });
    */

    $(document).on('click', '.popup-modal-dismiss', function (e) {
        e.preventDefault();
        $.magnificPopup.close();
    });

})(jQuery);


function setStgPrice() {
    if (!StockInfoData)
        return;
    var WishDivi = parseFloat($("#WishDivi").val());
    var CurrGoodPrice = Math.floor(parseFloat((parseFloat((StockInfoData.CurrFromEPS * StockInfoData.PrevDiviFrom3YearAvgByEPS) / WishDivi))) * 100) / 100;
    var FutureGoodPrice = Math.floor(parseFloat((parseFloat((StockInfoData.FutureFromEPS * StockInfoData.PrevDiviFrom3YearAvgByEPS) / WishDivi))) * 100) / 100;
    $("#CurrPrice").html(CurrGoodPrice);
    $("#FuturePrice").html(FutureGoodPrice);

    var CurrPrice = parseFloat(StockInfoData.Price);

    $("#CurrPerviewPriceBlock .panel").removeClass("panel-green").removeClass("panel-red").addClass("panel-red");
    $("#FuturePerviewPriceBlock .panel").removeClass("panel-green").removeClass("panel-red").addClass("panel-red");

    $("#CurrPriceMsg").html('不夠便宜');
    $("#FuturePriceMsg").html('不夠便宜');

    if (CurrPrice <= CurrGoodPrice) {
        $("#CurrPriceMsg").html('價格合理');
        $("#CurrPerviewPriceBlock .panel").removeClass("panel-red").addClass("panel-green");
    }
    if (CurrPrice <= FutureGoodPrice) {
        $("#FuturePriceMsg").html('價格合理');
        $("#FuturePerviewPriceBlock .panel").removeClass("panel-red").addClass("panel-green");
    }
    if (CurrGoodPrice <= 0) {
        $("#CurrPriceMsg").html('沒有投資價值');
        $("#CurrPrice").html('N/A');
    }
    if (FutureGoodPrice <= 0) {
        $("#CurrPriceMsg").html('沒有投資價值');
        $("#FuturePrice").html('N/A');
    }
}

function setRiskStatus() {
    $("#IsSelfStockMsg3").html("");
    $("#IsSelfStockMsg2").html("");
    $("#IsSelfStockMsg").html("");

    if (!StockInfoData)
        return;
        
    if (StockInfoData.Industry === '建材營造業') {
        $("#IsSelfStockMsg3").append('產業特殊');
    }

    if (!StockInfoData.EPS_Divi)
        return;
    
    $.each(StockInfoData.EPS_Divi, function (key, value) {
        if (value.TotalDivi)
            if (value.TotalDivi <= 0) {
                $("#IsSelfStockMsg2").html('獲利不穩');
            }

        if (value.TotalEPS)
            if (value.TotalEPS <= 0) {
                $("#IsSelfStockMsg").html('營收不穩');
            }
    });

    $("#IsSelfStockBlock").show();

    if ($("#IsSelfStockMsg3").html() == "")
        if ($("#IsSelfStockMsg2").html() == "")
            if ($("#IsSelfStockMsg").html() == "")
                $("#IsSelfStockBlock").hide();

    $("#IsSelfStockBlock .panel").addClass("panel-yellow");






}

var StockInfoData = null;
var doQuery = function (x) {
    var targetUrl = 'Data/GetStockInfo?StockNum=' + x + '&t=' + makeid();
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
            if (data.Code !==0) {
                setErrStatus("[" + data.Code + "]" + data.Message);
                return;
            }
            StockInfoData = data.Result;

            $("#StockInfoNum").html(StockInfoData.Num);
            $("#StockInfoName").html(StockInfoData.Name);
            $("#StockInfoIndustry").html(StockInfoData.Industry);
            $("#StockInfoPrice").html(StockInfoData.Price);
            //取得去年Divi
            var PrevYearDivi = parseFloat(StockInfoData.EPS_Divi[1].TotalDivi);
            var CurrPrice = parseFloat(StockInfoData.Price)
            var CurrDivi = new Number(PrevYearDivi / CurrPrice);
            CurrDivi = Math.floor(CurrDivi * 10000)/100;
            $("#StockInfoCurrDivi").html(CurrDivi).append('%');

            setStockStatus("IsPromisingEPS", StockInfoData.IsPromisingEPS);
            setStockStatus("IsGrowingUpEPS", StockInfoData.IsGrowingUpEPS );
            setStockStatus("IsAlwaysIncomeEPS", StockInfoData.IsAlwaysIncomeEPS);

            setStockStatus("IsAlwaysPayDivi", StockInfoData.IsAlwaysPayDivi);
            setStockStatus("IsOverDiffDivi", (StockInfoData.IsOverDiffDivi == 0 ? 1 : 0));

            setStockStatus("IsSafeDebt", StockInfoData.IsSafeDebt);
            setStockStatus("IsSafePB", StockInfoData.IsSafePB);
            setStockStatus("IsSafeInvestor", StockInfoData.IsSafeInvestor);
            setStockStatus("IsSafeValue", StockInfoData.IsSafeValue);

            

            setStgPrice();
            setRiskStatus();
            $(".StockStatus").show();

            $('.contact ').eq(0).html('<i class="fa fa-phone"></i><h3> 基本資料</h3>');
            $('.contact ').eq(1).html('<i class="fa fa-phone"></i><h3> 年份／股息</h3>');
            $('.contact ').eq(2).html('<i class="fa fa-phone"></i><h3> 年季／EPS／利息比</h3>');

            $('.contact ').eq(0).append('<P>股名: ' + StockInfoData.Name + "</P>");
            $('.contact ').eq(0).append('<P>股價: ' + StockInfoData.Price + "</P>");
            $('.contact ').eq(0).append('<P>產業: ' + StockInfoData.Industry + "</P>");
            $('.contact ').eq(0).append('<P>本益比: ' + StockInfoData.PERatio + "</P>");
            $('.contact ').eq(0).append('<P>大戶持股(%): ' + StockInfoData.InvestorRatio + "</P>");
            $('.contact ').eq(0).append('<P>負債比: ' + StockInfoData.DebtRatio + "%</P>");
            $('.contact ').eq(0).append('<P>淨值（億）: ' + StockInfoData.Value + "</P>");
            
            $.each(StockInfoData.EPS_Divi, function (key, value) {
                $('.contact ').eq(2).append(value.Year + value.LastQ + '／' + value.TotalEPS + '／' + value.EachDiviFromEPS + '<BR>');
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

function setErrStatus(msg) {
    $(".StockStatus").hide();
    var x = document.getElementById("ToastMsg");
    $("#ToastMsg").html(msg)
    $("#ToastMsg").show();
    x.className = "show";
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 5000);
}

function setStockStatus(id, IsOk) {

    $("#btn" + id).removeClass("btn-danger").removeClass("btn-success");
    $("#btn" + id).find("i").removeClass("fa-times").removeClass("fa-check");

    if (IsOk === 1) {
        $("#btn" + id).addClass("btn-success"); 
        $("#btn" + id).find("i").addClass("fa-check");
    } else {
        $("#btn" + id).addClass("btn-danger");
        $("#btn" + id).find("i").addClass("fa-times");
    }

}

function makeid() {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var i = 0; i < 5; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
}
