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
        var c = $("#StockNum").val().split(' ')[0];
        if (c.length !== 4) { 
            alert('請輸入正確的股票編號');
            return;
        }
        if (c === '') {
            return;
        }

        doQuery(c);
    });

    $('#btnSlot').click(function () {
        doQueryGetNum();
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
                    src: 'LazySlot.aspx'
                }
            }
        }]
    });

    


    $(document).on('click', '.popup-modal-dismiss', function (e) {
        e.preventDefault();
        $.magnificPopup.close();
    });
    
    $(document).ready(function () {
        AuthControll();
        (IsMobile() ? $("#btnSlot").hide() : $("#btnSlot").show());
        $("#StockNum").autocomplete({source: $.fn.Setting.StockNumMapping});
    });
})(jQuery);


//var LazySlotWin;
window.closeLazySlot = function (StockNum) {
  //  if (LazySlotWin) {
        //LazySlotWin.close();
    //}
    
    if (StockNum) {
        $('a#alinkGoContact').click();
        $("#StockNum").val(StockNum);
        $('#btnQuery').click();
    }
    
    $.magnificPopup.close();
}

function IsMobile() {
    return (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent));
}

function openLazySlot(StockNum,StockName) {
    $.magnificPopup.open({
        items: {
            src: 'LazySlot.aspx?Num=' + StockNum + '&StockName=' + encodeURIComponent(StockName)
        },
        type: 'iframe',
        closeOnBgClick: false,
        showCloseBtn: false
    });
}


var CommonHelper = new Common();
var StockInfoData = null;

function setStgPrice() {
    $("#StockStgBlock").hide();
    $("#PleaseLoginBlock").show();
    if (_u) { 
        if (_u.Permission == "1") {
            $("#StockStgBlock").show();
            $("#PleaseLoginBlock").hide();
        } else {
            if (!StockInfoData.CurrFromEPS) { 
                logout();
                return;
            }
        }
    }
    
    if (!StockInfoData)
        return;
    var WishRatio = $("#WishDivi").val();
    var CurrGoodPrice = CommonHelper.RoundX(StockInfoData["EstimateStablePrice" + WishRatio],2);
    var FutureGoodPrice = CommonHelper.RoundX(StockInfoData["EstimateUnstablePrice" + WishRatio], 2);

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
    });

    if (StockInfoData.IsUnstableEPS)
        $("#IsSelfStockMsg").html('EPS不穩定');

    $("#IsSelfStockBlock").show();

    if ($("#IsSelfStockMsg3").html() == "")
        if ($("#IsSelfStockMsg2").html() == "")
            if ($("#IsSelfStockMsg").html() == "")
                $("#IsSelfStockBlock").hide();

    $("#IsSelfStockBlock .panel").addClass("panel-yellow");
}

var doQuery = function (x) {
    var targetUrl = 'Data/GetStockInfo?StockNum=' + x+ '&t=' + CommonHelper.GenRandom(5);
    $.ajax({
        type: "post",
        url: targetUrl,
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        beforeSend: function (req, opts) {
            if (_u && _u.HashCode) 
                req.setRequestHeader("_UserInfo", (new Base64()).encode(JSON.stringify(_u)));
            
            //req.abort();
            $("#preloader").show();
            $(".StockStatus").hide();
        },
        success: function (data) {
            StockInfoData = null;
            data = $.fn.ApiResponse.JsonFormatCheck(data);
            if (!data) {
                setErrStatus("通訊異常!");
                return;
            }
            if (data.Code !==0) {
                setErrStatus("[" + data.Code + "]" + data.Message);
                return;
            }
            StockInfoData = data.Result;



            $("#RevenueGrowthRatio").attr("title", "["+StockInfoData.RevenueYYYYMM + "]累計:" + StockInfoData.RevenueGrowthRatio+"%");
            $("#PriceModifyDate").attr("title","最後更新時間:"+StockInfoData.PriceModifyDate);
            $("#StockInfoNum").html(StockInfoData.StockNum);
            $("#StockInfoName").html(StockInfoData.StockName);
            $("#StockInfoIndustry").html(StockInfoData.Industry);

            StockInfoData.Price = CommonHelper.RoundX(parseFloat(StockInfoData.Price), 2);
            $("#StockInfoPrice").html(StockInfoData.Price);
            
            setStockStatus("IsGrowingUpRevenue", StockInfoData.IsGrowingUpRevenue);

            setStockStatus("IsPromisingEPS", StockInfoData.IsPromisingEPS);
            setStockStatus("IsGrowingUpEPS", StockInfoData.IsGrowingUpEPS );
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

            var prevYear = (new Date()).getFullYear() - 1;
            var PrevYearDivi = 0;
            $.each(StockInfoData.EPS_Divi, function (key, value) {
                if (value.Year == prevYear)
                    PrevYearDivi = CommonHelper.RoundX(value.TotalEPS, 2);

                $('.contact ').eq(2).append(value.Year + value.LastQ + '／' + CommonHelper.RoundX(value.TotalEPS, 2) + '／' + (value.EachDiviFromEPS == null ? 0 : +value.EachDiviFromEPS) +'<BR>');
                if (value.LastQ === 'Q4')
                    $('.contact ').eq(1).append(value.Year + value.LastQ + '／' + value.TotalDivi + "<BR>");
            });

            var CurrDivi = new Number(PrevYearDivi / StockInfoData.Price);
            CurrDivi = Math.floor(CurrDivi * 10000) / 100;
            $("#StockInfoCurrDivi").html(CurrDivi).append('%');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('通訊異常');
        },
        complete: function () {
            $("#preloader").delay(100).fadeOut();
        }
    });
}


var doQueryGetNum = function (x) {
    var targetUrl = 'LazySlot/GetNum?t=' + CommonHelper.GenRandom(5);
    $.ajax({
        type: "post",
        url: targetUrl,
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        beforeSend: function (req, opts) {
            if (_u && _u.HashCode)
                req.setRequestHeader("_UserInfo", (new Base64()).encode(JSON.stringify(_u)));
            else { 
                setErrStatus("請先登錄Line~");
                req.abort();
                return;
            }
            $("#preloader").show();
        },
        success: function (data) {
            data = $.fn.ApiResponse.JsonFormatCheck(data);
            if (!data) {
                setErrStatus("通訊異常!");
                return;
            }
            if (data.Code !== 0) {
                setErrStatus("[" + data.Code + "]" + data.Message);
                return;
            }
            openLazySlot(data.Result.StockNum, data.Result.StockName)
        },
        error: function (xhr, ajaxOptions, thrownError) {
            setErrStatus("通訊異常");
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


var _u;
var _ckHelper = new CookieHelper();

function AuthControll() {
    var urlParams = new URLSearchParams(window.location.search);
    if (urlParams.has('v')) {
        if (typeof (Storage) !== "undefined") {
            var userObj = (new Base64()).decode((new URLSearchParams(window.location.search)).get('v').replace(/\ /g, '+').replace(/\n/g, ''));
            _ckHelper.setCookie("_UserInfo", userObj)
            //window.localStorage.setItem("_UserInfo", userObj);
            window.location = "/";
            return;
        }
    }
    
    //if (typeof (Storage) !== "undefined")
    _u = _ckHelper.getCookie("_UserInfo")
    
    if (!_u) return;
    if (_u==="null") return;
    _u = JSON.parse(_u);
    $("#LineAuth >img").attr("src", _u.PicUrl + "?ad=" + CommonHelper.GenRandom(4)).css({ "height": "25px"});
    $("#LineAuth").append("&nbsp; Hi~" + decodeURIComponent(_u.UserName));
    $("#LineAuth").attr("href", "javascript:logout();")
}


function logout() {
    _ckHelper.delCookie("_UserInfo");
    //if (_ckHelper.getCookie("_UserInfo"))
        //$.cookie('_UserInfo', null);
    /*
    if (typeof (Storage) !== "undefined") {
        if (localStorage._UserInfo)
            localStorage.removeItem('_UserInfo');
    }
    */
    location.reload()
}