<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LazySlot.aspx.cs" Inherits="LazyStock.Web.LazySlot" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Lazy Slot</title>
    
    <style>
    html,body{margin:0;padding:0;overflow:hidden;}
    body{background:url(img/LazySlot/body_bg.jpg) 0px 0px repeat-x #000;}
    .main_bg{background:url(img/LazySlot/main_bg.jpg) top center no-repeat;height:500px;}
    .main{width:500px;height:500px;position:relative;margin:0 auto;}
    .num_mask{background:url(img/LazySlot/num_mask.png) 0px 0px no-repeat;height:92px;width:370px;position:absolute;left:50%;top:170px;margin-left:-185px;z-index:9;}
    .num_box{height:225px;width:380px;position:absolute;left:50%;top:170px;margin-left:-185px;z-index:8;overflow:hidden;text-align:center;}
    .num{background:url(img/LazySlot/num.png) top center repeat-y;width:91px;height:132px;float:left;margin-right:0px;}
     #res {text-align:center;color:#fff;padding-top:80px;font-size:50px;font-family: 'verdana', 'Times New Roman','微軟正黑體','新細明體'}
    .button {display:inline-block;text-align: center; vertical-align: middle;margin-top: 30px;padding: 16px 57px;border: 1px solid #2a2e3c;border-radius: 8px;background: #4e5670;background: -webkit-gradient(linear, left top, left bottom, from(#4e5670), to(#2a2e3c));background: -moz-linear-gradient(top, #4e5670, #2a2e3c);background: linear-gradient(to bottom, #4e5670, #2a2e3c);text-shadow: #191b23 1px 1px 1px;font: normal normal bold 20px arial;color: #ffffff;text-decoration: none;}
    .button:hover,
    .button:focus {border: 1px solid ##313646;background: #5e6786;background: -webkit-gradient(linear, left top, left bottom, from(#5e6786), to(#323748));background: -moz-linear-gradient(top, #5e6786, #323748);background: linear-gradient(to bottom, #5e6786, #323748);color: #ffffff;text-decoration: none;}
    .button:active {background: #2a2e3c;background: -webkit-gradient(linear, left top, left bottom, from(#2a2e3c), to(#2a2e3c));background: -moz-linear-gradient(top, #2a2e3c, #2a2e3c);background: linear-gradient(to bottom, #2a2e3c, #2a2e3c);}
    </style>
</head>
<body>
  <div class="main_bg">
      <div class="main">
        <div id="res">~要保密喲~</div>
        <div class="num_mask"></div>
        <div class="num_box">
          <div class="num"></div>
          <div class="num"></div>
          <div class="num"></div>
          <div class="num"></div>
          <a id="BtnGoback" class="button" href="javascript:GoBack();">查看健檢報告</a>
        </div>
  </div>
</div>
<script type="text/javascript" src="js/jquery.min.js"></script>
<script type="text/javascript" src="js/easing.js"></script>
<script type="text/javascript" src="js/LazySlot.js?t=<%=DateTime.Now.ToString("yyyyMMddHHmmssfff") %>"></script>
<script>
$(document).ready(function () {
    $("#BtnGoback").hide();
    var Num;
    try {
        Num= parseInt($("#Num").val(),10);
    } catch (e) {
        
    }

    if (Num && Num > 1000 && Num <= 9999)
        Go();
    else {
        $('#res').text("無效連結~");
        $("#BtnGoback").show();
    }
    });

function GoBack() {
    if (parent) {
        parent.window.closeLazySlot($("#Num").val());
    }
}

var u = 133.3;
var isBegin = false;
function Go() {
    if (isBegin) return false;
    isBegin = true;
    if (!$("#Num")) {
        if (!$("#Num").val()) {
            alert('無效的連結!');
            if (parent) {
                parent.closeLazySlot();
            }
        }
    }

    $(".num").css('backgroundPositionY', 0);
    var result = $("#Num").val();
    
    var num_arr = (result + '').split('');
    $(".num").each(function (index) {
        var _num = $(this);
        setTimeout(function () {
            _num.animate({
                backgroundPositionY: (u * 60) - (u * num_arr[index])
            }, {
                    duration: 6000 + index * 3000,
                    easing: "easeInOutCirc",
                    complete: function () {
                        if (index == 3) {
                            isBegin = false;
                            $("#BtnGoback").show();
                            if ($("#StockName") && $("#StockName").val()) { 
                                $('#res').text($("#StockName").val());
                            }
                        }
                    }
                });
        }, index * 300);
    });
}
</script>
</body>
</html>
