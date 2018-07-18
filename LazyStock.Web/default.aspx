<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="LazyStock.Web._default" %>
<%@ Import Namespace="System.Web.Optimization" %>

<!DOCTYPE html>
<html lang="en">

<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1">
	<!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->

	<title>LazyStock</title>

	<!-- Google font -->
	<link href="https://fonts.googleapis.com/css?family=Montserrat:400,700%7CVarela+Round" rel="stylesheet">
    <%--
	<!-- Bootstrap -->
	<link type="text/css" rel="stylesheet" href="css/bootstrap.min.css" />

	<!-- Owl Carousel -->
	<link type="text/css" rel="stylesheet" href="css/owl.carousel.css" />
	<link type="text/css" rel="stylesheet" href="css/owl.theme.default.css" />

	<!-- Magnific Popup -->
	<link type="text/css" rel="stylesheet" href="css/magnific-popup.css" />

	<!-- Font Awesome Icon -->
	<link rel="stylesheet" href="css/font-awesome.min.css">

	<!-- Custom stlylesheet -->
	<link type="text/css" rel="stylesheet" href="css/style.css?t=<%=DateTime.Now.ToString("yyyyMMddHHmmssfff") %>" />
    <link type="text/css" rel="stylesheet"  href="css/toast.css"/>
     --%>

    <%:Styles.Render("~/bundles/css") %>
	<!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
	<!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
	<!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
    <script async src="//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>
    <script>
      (adsbygoogle = window.adsbygoogle || []).push({
        google_ad_client: "ca-pub-4646927894355266",
        enable_page_level_ads: true
      });
    </script>

    
</head>

<body>
	<!-- Header -->
	<header id="home">
		<!-- Background Image -->
		<div class="bg-img" style="background-image: url('./img/background1.jpg');">
			<div class="overlay"></div>
		</div>
		<!-- /Background Image -->

		<!-- Nav -->
		<nav id="nav" class="navbar nav-transparent">
			<div class="container">

				<div class="navbar-header">
					<!-- Logo -->
					<div class="navbar-brand">
                        <h3>
                            
						<a href="." class="white-text"><img src="img/LazyStockLogo5.png?t=<%=DateTime.Now.ToString("yyyyMMddHHmmssfff") %>" /></a>
                        </h3>
					</div>
					<!-- /Logo -->

					<!-- Collapse nav button -->
					<div class="nav-collapse">
						<span></span>
					</div>
					<!-- /Collapse nav button -->
				</div>

				<!--  Main navigation  -->
				<ul class="main-nav nav navbar-nav navbar-right">
					<li><a href="#home">首頁</a></li>
					<li><a href="#about" id="btnAbout">關於</a></li>
					<li><a href="#portfolio">案例</a></li>
					<li><a href="#service">服務</a></li>
                    <!--
                        <li><a href="#pricing">售價</a></li>
                        <li><a href="#team">團隊</a></li>
					    <li class="has-dropdown"><a href="#blog">Blog</a>
						<ul class="dropdown">
							<li><a href="blog-single.html">blog post</a></li>
						</ul>
					</li>
                    -->
					<li ><a href="#contact" id="alinkGoContact">點我試用</a></li>
                    <li ><a href="LineLoginAuth.aspx" id="LineAuth">
                        <img src="img/line/btn_login_base.png?ad=sdr4d" />
                        </a></li>
				</ul>
				<!-- /Main navigation -->

			</div>
		</nav>
		<!-- /Nav -->

		<!-- home wrapper -->
		<div class="home-wrapper">
			<div class="container">
				<div class="row">
					<!-- home content -->
					<div class="col-md-10 col-md-offset-1">
						<div class="home-content">
							<h1 class="white-text">Lazy Stock</h1>
							<p class="white-text">我們懶的操股，我們只想花最少的工，獲得最大的勝率<br />當你學會了慢，你獲得的不止是金錢，還有更多的時間</p>
							<button class="white-btn" id="btnStartNow">立刻使用</button>
                            <button class="main-btn" id="btnReg" style="display:none" >註冊</button>
						</div>
					</div>
					<!-- /home content -->
				</div>
			</div>
		</div>
		<!-- /home wrapper -->
	</header>
    
    <div  class="white-popup-block mfp-hide" id="frmReg">
        <p>輸入開通序號：<input value="adfsdf"/></p>
    </div>
	<!-- /Header -->

	<!-- About -->
	<div id="about" class="section md-padding">

		<!-- Container -->
		<div class="container">
			<!-- Row -->
			<div class="row">
				<!-- Section header -->
				<div class="section-header text-center">
					<h2 class="title">簡單．快速．穩健</h2>
                    巴菲特說再多，也沒報名牌給你<br />
				</div>
				<!-- /Section header -->
				<!-- about -->
				<div class="col-md-4">
					<div class="about">
						<i class="fa fa-cogs"></i>
						<h3>EPS</h3>
						<p>
                            比去年高<br />
                            持續成長<br />
                            連續賺錢<br />
						</p>
                        <!--<a href="#">Read more</a>-->
					</div>
				</div>
				<!-- /about -->

				<!-- about -->
				<div class="col-md-4">
					<div class="about">
						<i class="fa fa-magic"></i>
						<h3>股利回饋</h3>
						<p>連續發放<br />
                           配息穩定<br />
                           <br />

						</p>
						<!--<a href="#">Read more</a>-->
					</div>
				</div>
				<!-- /about -->

				<!-- about -->
				<div class="col-md-4">
					<div class="about">
						<i class="fa fa-mobile"></i>
						<h3>綜合考量</h3>
						<p>負債比<br />
                            本益比<br />
                            公司規模<br />
                            股東信心度<br />
                            信用不佳
						</p>
						<!--<a href="#">Read more</a>-->
					</div>
				</div>
				<!-- /about -->

			</div>
			<!-- /Row -->

		</div>
		<!-- /Container -->

	</div>
	<!-- /About -->


	<!-- Portfolio -->
	<div id="portfolio" class="section md-padding bg-grey">

		<!-- Container -->
		<div class="container">

			<!-- Row -->
			<div class="row">

				<!-- Section header -->
				<div class="section-header text-center">
					<h2 class="title">真實案例</h2>
                    我不盯盤，穩健獲利，你呢？
				</div>
				<!-- /Section header -->

				<!-- Work -->
				<div class="col-md-4 col-xs-6 work">
					<img class="img-responsive" src="./img/work6.png" alt="" style="height:132px">
					<div class="overlay"></div>
					<div class="work-content">
						<span>2017/12/03</span>
						<h3>艾訊(3088)</h3>
						<div class="work-link">
                            <!--<a href="#"><i class="fa fa-external-link"></i></a>-->
							<a class="lightbox" href="./img/work6.png"><i class="fa fa-search"></i></a>
						</div>
					</div>
				</div>
				<!-- /Work -->

				<!-- Work -->
				<div class="col-md-4 col-xs-6 work">
					<img class="img-responsive" src="./img/work7.png" alt="" style="height:132px">
					<div class="overlay"></div>
					<div class="work-content">
						<span>2017/12/06</span>
						<h3>中橡(2104)</h3>
						<div class="work-link">
							<!--<a href="#"><i class="fa fa-external-link"></i></a>-->
							<a class="lightbox" href="./img/work7.png"><i class="fa fa-search"></i></a>
						</div>
					</div>
				</div>
				<!-- /Work -->

				<!-- Work -->
				<div class="col-md-4 col-xs-6 work">
					<img class="img-responsive" src="./img/work8.png" alt="" style="height:132px">
					<div class="overlay"></div>
					<div class="work-content">
						<span>2017/12/03</span>
						<h3>中華(2204)</h3>
						<div class="work-link">
							<!--<a href="#"><i class="fa fa-external-link"></i></a>-->
							<a class="lightbox" href="./img/work8.png"><i class="fa fa-search"></i></a>
						</div>
					</div>
				</div>
				<!-- /Work -->
                <!-- Work -->
				<div class="col-md-4 col-xs-6 work">
					<img class="img-responsive" src="./img/work9.png" alt="">
					<div class="overlay"></div>
					<div class="work-content">
						<span>2017/12/03</span>
						<h3>微星(2377)</h3>
						<div class="work-link">
							<!--<a href="#"><i class="fa fa-external-link"></i></a>-->
							<a class="lightbox" href="./img/work9.png"><i class="fa fa-search"></i></a>
						</div>
					</div>
				</div>
				<!-- /Work -->

				<!-- Work -->
				<div class="col-md-4 col-xs-6 work">
					<img class="img-responsive" src="./img/work10.png" alt="">
					<div class="overlay"></div>
					<div class="work-content">
						<span>2017/12/10</span>
						<h3>崇友(4506)</h3>
						<div class="work-link">
							<!--<a href="#"><i class="fa fa-external-link"></i></a>-->
							<a class="lightbox" href="./img/work10.png"><i class="fa fa-search"></i></a>
						</div>
					</div>
				</div>
				<!-- /Work -->
			</div>
			<!-- /Row -->

		</div>
		<!-- /Container -->

	</div>
	<!-- /Portfolio -->

	<!-- Service -->
	<div id="service" class="section md-padding">

		<!-- Container -->
		<div class="container">

			<!-- Row -->
			<div class="row">

				<!-- Section header -->
				<div class="section-header text-center">
					<h2 class="title">為您提供以下服務</h2>
				</div>
				<!-- /Section header -->

				<!-- service -->
				<div class="col-md-4 col-sm-6">
					<div class="service">
						<i class="fa fa-diamond"></i>
						<h3>股票健檢(free)</h3>
						<p>只要一鍵就搞定財報分析，進出場有依據，買股票不用再膽憟心驚</p>
					</div>
				</div>
				<!-- /service -->

				<!-- service -->
				<div class="col-md-4 col-sm-6">
					<div class="service">
						<i class="fa fa-rocket"></i>
						<h3>優質推薦</h3>
						<p>精選優質股票，讓您優雅的投資，優雅的獲利</p>
					</div>
				</div>
				<!-- /service -->

				<!-- service -->
				<div class="col-md-4 col-sm-6">
					<div class="service">
						<i class="fa fa-cogs"></i>
						<h3>股價預測</h3>
						<p>運用EPS推測年度報酬率，讓您的定價更加準確</p>
					</div>
				</div>
				<!-- /service -->


				<!-- service -->
				<div class="col-md-4 col-sm-6">
					<div class="service">
						<i class="fa fa-cogs"></i>
						<h3>營收預測</h3>
						<p>若您是交易繁複的使用者，我們提供了季度部局預測模組</p>
					</div>
				</div>
				<!-- /service -->

				<!-- service -->
				<div class="col-md-4 col-sm-6">
					<div class="service">
						<i class="fa fa-pencil"></i>
						<h3>實體課程</h3>
						<p>每月舉辦不定期實體課程，讓你股海戰無不勝攻無不克</p>
					</div>
				</div>
				<!-- /service -->

				<!-- service--> 
				<div class="col-md-4 col-sm-6">
					<div class="service">
						<i class="fa fa-pencil"></i>
						<h3>黑名單警示</h3>
						<p>雞蛋水餃股、信用不良，由我們來把關</p>
					</div>
				</div>
				<!--  /service -->
			</div>
			<!-- /Row -->
		</div>
		<!-- /Container -->
	</div>
	<!-- /Service -->


	<!-- Why Choose Us -->
	<div id="features" class="section md-padding bg-grey">

		<!-- Container -->
		<div class="container">

			<!-- Row -->
			<div class="row">

				<!-- why choose us content -->
				<div class="col-md-6">
					<div class="section-header">
						<h2 class="title">為什麼選擇我們</h2>
					</div>
					<p>從賽局理論中我們知道，最大的贏家就是莊家，<BR>只有穩健的投資方法才是致勝之道</p>

					<div class="feature">
						<i class="fa fa-check"></i>
						<p>最專業的團隊為您把關</p>
					</div>
					<div class="feature">
						<i class="fa fa-check"></i>
						<p>運用大數據推演，勝率極高</p>
					</div>
					<div class="feature">
						<i class="fa fa-check"></i>
						<p>所有數據公開透明，經的起考驗</p>
					</div>
					<div class="feature">
						<i class="fa fa-check"></i>
						<p>當您了解市價後，買的安心賣的開心</p>
					</div>
					<div class="feature">
						<i class="fa fa-check"></i>
						<p>預計2030年，榮獲今週刊最佳理財商系統獎</p>
					</div>
                    
				</div>
				<!-- /why choose us content -->

				<!-- About slider -->

				<div class="col-md-6">
					<div id="about-slider" class="owl-carousel owl-theme">
						<img class="img-responsive" src="./img/about1.jpg" alt="">
						<img class="img-responsive" src="./img/about2.jpg" alt="">
					</div>
				</div>
				<!-- /About slider -->

			</div>
			<!-- /Row -->

		</div>
		<!-- /Container -->

	</div>
	<!-- /Why Choose Us -->


	<!-- Numbers -->
	<div id="numbers" class="section sm-padding">

		<!-- Background Image -->
		<div class="bg-img" style="background-image: url('./img/background2.jpg');">
			<div class="overlay"></div>
		</div>
		<!-- /Background Image -->

		<!-- Container -->
		<div class="container" style="display:none">

			<!-- Row -->
			<div class="row">

				<!-- number -->
				<div class="col-sm-3 col-xs-6">
					<div class="number">
						<i class="fa fa-users"></i>
						<h3 class="white-text"><span class="counter">101</span></h3>
						<span class="white-text">使用人數</span>
					</div>
				</div>
				<!-- /number -->

				<!-- number -->
				<div class="col-sm-3 col-xs-6">
					<div class="number">
						<i class="fa fa-trophy"></i>
						<h3 class="white-text"><span class="counter">10</span></h3>
						<span class="white-text">獎項</span>
					</div>
				</div>
				<!-- /number -->

				<!-- number -->
				<div class="col-sm-3 col-xs-6">
					<div class="number">
						<i class="fa fa-coffee"></i>
						<h3 class="white-text"><span class="counter">154</span>K</h3>
						<span class="white-text">咖啡杯數</span>
					</div>
				</div>
				<!-- /number -->

				<!-- number -->
				<div class="col-sm-3 col-xs-6">
					<div class="number">
						<i class="fa fa-file"></i>
						<h3 class="white-text"><span class="counter">45</span></h3>
						<span class="white-text">獲利次數</span>
					</div>
				</div>
				<!-- /number -->

			</div>
			<!-- /Row -->

		</div>
		<!-- /Container -->

	</div>
	<!-- /Numbers -->

	<!-- Pricing -->
	<div id="pricing" class="section md-padding" style="display:none">

		<!-- Container -->
		<div class="container">

			<!-- Row -->
			<div class="row">

				<!-- Section header -->
				<div class="section-header text-center">
					<h2 class="title">價目表</h2>
				</div>
				<!-- /Section header -->

				<!-- pricing -->
				<div class="col-sm-4">
					<div class="pricing">
						<div class="price-head">
							<span class="price-title">試用版</span>
							<div class="price">
								<h3>$0<span class="duration">/ 月</span></h3>
							</div>
						</div>
						<ul class="price-content">
							<li>
								<p>股票健檢</p>
							</li>
                            <li>
								<p>-</p>
							</li>
                            <li>
								<p>-</p>
							</li>
                            <li>
								<p>-</p>
							</li>
						</ul>
						<div class="price-btn">
							<button class="outline-btn" disabled>Free</button>
						</div>
					</div>
				</div>
				<!-- /pricing -->

				<!-- pricing -->
				<div class="col-sm-4">
					<div class="pricing">
						<div class="price-head">
							<span class="price-title">基本款</span>
							<div class="price">
								<h3>$299<span class="duration">/ month</span></h3>
							</div>
						</div>
						<ul class="price-content">
							<li>
								<p>股票健檢</p>
							</li>
							<li>
								<p>優質推薦</p>
							</li>
							<li>
								<p>股價預測</p>
							</li>
                            <li>
								<p>-</p>
							</li>

						</ul>
						<div class="price-btn">
							<button class="outline-btn" disabled>Coming soon</button>
						</div>
					</div>
				</div>
				<!-- /pricing -->

				<!-- pricing -->
				<div class="col-sm-4">
					<div class="pricing">
						<div class="price-head">
							<span class="price-title">專業版</span>
							<div class="price">
								<h3>$499<span class="duration">/ month</span></h3>
							</div>
						</div>
						<ul class="price-content">
							<li>
								<p>股票健檢</p>
							</li>
							<li>
								<p>優質推薦</p>
							</li>
							<li>
								<p>股價預測</p>
							</li>
							<li>
								<p>營收預測</p>
							</li>
						</ul>
						<div class="price-btn">
							<button class="outline-btn" disabled>Coming soon</button>
						</div>
					</div>
				</div>
				<!-- /pricing -->

			</div>
			<!-- Row -->

		</div>
		<!-- /Container -->

	</div>
	<!-- /Pricing -->


	<!-- Testimonial -->
	<div id="testimonial" class="section md-padding">

		<!-- Background Image -->
		<div class="bg-img" style="background-image: url('./img/background3.jpg');">
			<div class="overlay"></div>
		</div>
		<!-- /Background Image -->

		<!-- Container -->
		<div class="container">

			<!-- Row -->
			<div class="row">

				<!-- Testimonial slider -->
				<div class="col-md-10 col-md-offset-1">
					<div id="testimonial-slider" class="owl-carousel owl-theme">

						<!-- testimonial -->
						<div class="testimonial">
							<div class="testimonial-meta">
								<img src="./img/perso1.jpg" alt="">
								<h3 class="white-text">Leon Chen</h3>
								<span>創辦人</span>
							</div>
							<p class="white-text">
                                8年投資經驗，接觸過多種金融商品<br />
                                專長技術分析、價值型投資、自動化交易
						</div>
						<!-- /testimonial -->

						<!-- testimonial -->
						<div class="testimonial">
							<div class="testimonial-meta">
								<img src="./img/perso3.jpg" alt="">
								<h3 class="white-text">徵求合作伙伴</h3>
								<span>技術/產品/講師</span>
							</div>
							<p class="white-text">
                                有投資理財有熱情的伙伴，<br />
                                別害羞~找我聊聊吧~
							</p>
						</div>
						<!-- /testimonial -->
					</div>
				</div>
				<!-- /Testimonial slider -->

			</div>
			<!-- /Row -->

		</div>
		<!-- /Container -->

	</div>
	<!-- /Testimonial -->

	<!-- Team -->
	<div id="team" class="section md-padding" style="display:none">

		<!-- Container -->
		<div class="container">

			<!-- Row -->
			<div class="row">

				<!-- Section header -->
				<div class="section-header text-center">
					<h2 class="title">Our Team</h2>
				</div>
				<!-- /Section header -->

				<!-- team -->
				<div class="col-sm-4">
					<div class="team">
						<div class="team-img">
							<img class="img-responsive" src="./img/team1.jpg" alt="">
							<div class="overlay">
								<div class="team-social">
									<a href="#"><i class="fa fa-facebook"></i></a>
									<a href="#"><i class="fa fa-google-plus"></i></a>
									<a href="#"><i class="fa fa-twitter"></i></a>
								</div>
							</div>
						</div>
						<div class="team-content">
							<h3>John Doe</h3>
							<span>Web Designer</span>
						</div>
					</div>
				</div>
				<!-- /team -->

				<!-- team -->
				<div class="col-sm-4">
					<div class="team">
						<div class="team-img">
							<img class="img-responsive" src="./img/team2.jpg" alt="">
							<div class="overlay">
								<div class="team-social">
									<a href="#"><i class="fa fa-facebook"></i></a>
									<a href="#"><i class="fa fa-google-plus"></i></a>
									<a href="#"><i class="fa fa-twitter"></i></a>
								</div>
							</div>
						</div>
						<div class="team-content">
							<h3>John Doe</h3>
							<span>Web Designer</span>
						</div>
					</div>
				</div>
				<!-- /team -->

				<!-- team -->
				<div class="col-sm-4">
					<div class="team">
						<div class="team-img">
							<img class="img-responsive" src="./img/team3.jpg" alt="">
							<div class="overlay">
								<div class="team-social">
									<a href="#"><i class="fa fa-facebook"></i></a>
									<a href="#"><i class="fa fa-google-plus"></i></a>
									<a href="#"><i class="fa fa-twitter"></i></a>
								</div>
							</div>
						</div>
						<div class="team-content">
							<h3>John Doe</h3>
							<span>Web Designer</span>
						</div>
					</div>
				</div>
				<!-- /team -->

			</div>
			<!-- /Row -->

		</div>
		<!-- /Container -->

	</div>
	<!-- /Team -->


	<!-- Contact -->
	<div id="contact" class="section md-padding">
		<!-- Container -->
		<div class="container">
			<!-- 健檢查詢 -->
			<div class="row">
				<!-- Section-header -->
				<div class="section-header text-center">
					<h2 class="title">股票健檢</h2>

                    <div class="panel-body">
                        <form class="form-inline" role="form">
                            <div class="form-group">
                                <input  class="form-control" placeholder="股票代碼" id="StockNum" maxlength='4'>
                                <button class="main-btn btn-xs" id="btnQuery" onclick=" return false; ">查詢</button>
                            </div>
                        </form>
                   </div>
				</div>
				<!-- /Section-header -->
            </div>
			<!-- /健檢查詢 -->

            <!-- 基本資料 -->
			<div class="row StockStatus">
				<div class="panel panel-primary">
                    <div class="panel-heading">基本資料</div>
                    <!-- /.panel-heading -->
                    <div class="panel-body">
                        <table class="table table-bordered table-responsive">
                                <tr >
                                    <td class="info">證券代號</td>
                                    <td id="StockInfoNum"></td>
                                    <td class="info">證券名稱</td>
                                    <td id="StockInfoName"></td>
                                    <td class="info">產業別</td>
                                    <td id="StockInfoIndustry"></td>
                                </tr>
                                <tr >
                                    <td class="info">收盤價&nbsp;
                                        <button id="PriceModifyDate" type="button" class="btn btn-warning btn-xs" data-toggle="tooltip" data-placement="top" title="Tooltip on top" >!</button>
                                    </td>
                                    <td id="StockInfoPrice"></td>
                                    <td class="info">當年殖利率</td>
                                    <td id="StockInfoCurrDivi"></td>
                                </tr>
                                <tr >
                                    
                                </tr>
                        </table>
                    </div>
                    <!-- /.table-responsive -->
                </div>
                <!-- /.panel -->
			</div>
			<!-- /基本資料 -->

            <!-- 風檢指標 -->
            <div class="row StockStatus" id="StockStgBlock">
				<div class="panel panel-primary">
                    <div class="panel-heading">
                        <form class="form-inline" role="form">
                            <div class="form-group">
                                <label for="email">進場策略　(期望殖利率：</label>
                                <select id='WishDivi' class="form-control" >
                                    <option value="5">5%</option>
                                    <option value="7">7%</option>
                                    <option value="10">10%</option>
                                </select>
                                <label for="email">)</label>
                            </div>                        
                        </form>
                        


                    </div>
                    <!-- /.panel-heading -->
                    <div class="panel-body">
                        <div class="col-lg-3 col-md-3" id="IsSelfStockBlock">
                                <div class="panel panel-yellow" >
                                    <div class="panel-heading">
                                        <div class="row">
                                            <div class="col-xs-3">
                                                <i class="fa fa-info-circle fa-5x"></i>
                                            </div>
                                            <div class="col-xs-9 text-right">
                                                <div id="IsSelfStockMsg"></div>
                                                <div id="IsSelfStockMsg2"></div>
                                                <div id="IsSelfStockMsg3"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <a href="#" style="display:none">
                                        <div class="panel-footer">
                                            <span class="pull-left">詳細資訊</span>
                                            <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                            <div class="clearfix"></div>
                                        </div>
                                    </a>
                                </div>
                        </div>
                        <div class="col-lg-3 col-md-3" id="CurrPerviewPriceBlock">
                            <div class="panel panel-green">
                                <div class="panel-heading">
                                    <div class="row">
                                        <div class="col-xs-3">
                                            <i class="fa fa-shield fa-5x"></i>
                                        </div>
                                        <div class="col-xs-9 text-right">
                                            <div class="huge">保守型</div>
                                            <div id="CurrPriceMsg">建議進場價</div>
                                            <div id="CurrPrice"></div>
                                        </div>
                                    </div>
                                </div>
                                <a href="#" style="display:none">
                                    <div class="panel-footer">
                                        <span class="pull-left">詳細資訊</span>
                                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                        <div class="clearfix"></div>
                                    </div>
                                </a>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3" id="FuturePerviewPriceBlock">
                            <div class="panel panel-red">
                                <div class="panel-heading">
                                    <div class="row">
                                        <div class="col-xs-3">
                                            <i class="fa fa-eye fa-5x"></i>
                                        </div>
                                        <div class="col-xs-9 text-right">
                                            <div class="huge">預估型</div>
                                            <div id="FuturePriceMsg">建議進場價</div>
                                            <div id="FuturePrice"></div>
                                        </div>
                                    </div> 
                                </div>
                                <a href="#"  style="display:none">
                                    <div class="panel-footer">
                                        <span class="pull-left">View Details</span>
                                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                        <div class="clearfix"></div>
                                    </div>
                                </a>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4"></div>
                            </div>
                </div>
            </div>

            <div class="row StockStatus" id="PleaseLoginBlock">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <form class="form-inline" role="form">
                            <div class="form-group">
                                <label for="email">想看進場策略？</label>
                                <a href="LineLoginAuth.aspx" id="LineAuth2"><img src="img/line/44dp/btn_login_base.png" /></a>
                            </div>                        
                        </form>
                    </div>
                </div>
            </div>
            <!-- /風檢指標 -->            

            <!-- 健檢結果 -->
            <div class="row StockStatus">
                <!-- 獲利能力 -->
                <div class="col-sm-4">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                           獲利能力（EPS)
                        </div>
                        <!-- /.panel-heading -->
                        <div class="panel-body">
                            <table class="table table-bordered table-responsive">
                                    <tr >
                                        <td class="info">賺的比去年多</td>
                                        <td><button type="button" class="btn btn-success btn-circle StockStatusBtnGroup" id="btnIsPromisingEPS"><i class="fa fa-check"></i></button></td>
                                    </tr>

                                    <tr >
                                        <td class="info">獲利持續成長</td>
                                        <td><button type="button" class="btn btn-success btn-circle StockStatusBtnGroup" id="btnIsGrowingUpEPS"><i class="fa fa-check"></i></button></td>
                                    </tr>

                                    <tr >
                                        <td class="info">穩健獲利</td>
                                        <td><button type="button" class="btn btn-success btn-circle StockStatusBtnGroup" id="btnIsAlwaysIncomeEPS"><i class="fa fa-check"></i></button></td>
                                    </tr>
                            </table>
                        </div>
                        <!-- /.table-responsive -->
                    </div>
                    <!-- /.panel -->
                </div>
                <!-- /獲利能力 -->

                <!-- 股利政策 -->
                <div class="col-sm-4">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                           股利政策
                        </div>
                        <!-- /.panel-heading -->
                        <div class="panel-body">
                            <table class="table table-bordered table-responsive">
                                    <tr >
                                        <td class="info">連續配息</td>
                                        <td>
                                            <button type="button" class="btn btn-success btn-circle StockStatusBtnGroup" id="btnIsAlwaysPayDivi"><i class="fa fa-check"></i></button>
                                        </td>
                                    </tr>
                                    <tr >
                                        <td class="info">配息穩定性</td>
                                        <td>
                                            <button type="button" class="btn btn-success btn-circle StockStatusBtnGroup" id="btnIsStableDivi"><i class="fa fa-check"></i></button>
                                        </td>
                                    </tr>
                            </table>
                        </div>
                        <!-- /.table-responsive -->
                    </div>
                    <!-- /.panel -->
                </div>
                <!-- /股利政策 -->

                <!-- 參考項目 -->
                <div class="col-sm-4">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                           參考項目
                        </div>
                        <!-- /.panel-heading -->
                        <div class="panel-body">
                                <table class="table table-bordered table-responsive">

                                        <tr >
                                            <td class="info">公司市值(>30億）</td>
                                            <td>
                                                <button type="button" class="btn btn-success btn-circle StockStatusBtnGroup" id="btnIsSafeValue"><i class="fa fa-check"></i></button>
                                            </td>
                                        </tr>
                                        <tr >
                                            <td class="info">本益比(=<20）</td>
                                            <td>
                                                <button type="button" class="btn btn-success btn-circle StockStatusBtnGroup" id="btnIsSafePB"><i class="fa fa-check"></i></button>
                                            </td>
                                        </tr>
                                        <tr >
                                            <td class="info">大戶持股(>= 25%)</td>
                                            <td>
                                                <button type="button" class="btn btn-success btn-circle StockStatusBtnGroup" id="btnIsSafeInvestor"><i class="fa fa-check"></i></button>
                                            </td>
                                        </tr>
                                        <tr >
                                            <td class="info">負債比 (<= 55%)</td>
                                            <td>
                                                <button type="button" class="btn btn-success btn-circle StockStatusBtnGroup" id="btnIsSafeDebt"><i class="fa fa-check"></i></button>
                                            </td>
                                        </tr>
                                        <tr >
                                            <td class="info">營收成長比 &nbsp;
                                                <button id="RevenueGrowthRatio" type="button" class="btn btn-warning btn-xs" data-toggle="tooltip" data-placement="top" title="Tooltip on top" >i</button></td>
                                            <td>
                                                <button type="button" class="btn btn-success btn-circle StockStatusBtnGroup" id="btnIsGrowingUpRevenue"><i class="fa fa-check"></i></button>
                                            </td>
                                        </tr>
                                </table>
                            </div>
                            <!-- /.table-responsive -->
                    </div>
                    <!-- /.panel -->
                </div>
                <!-- /參考項目 -->
            </div>
            <!-- /健檢結果 -->

            <!-- 詳細資訊 -->
			<div class="row StockStatus" >
				<!-- contact -->
				<div class="col-sm-4 StockInfo">
					<div class="contact"　id="StockBasicInfo">
						<i class="fa fa-phone"></i>
						<h3>基本資料</h3>
					</div>


				</div>
				<!-- /contact -->

				<!-- contact -->
				<div class="col-sm-4 StockInfo">
					<div class="contact"　id="StockDivi">
						<i class="fa fa-envelope"></i>
						<h3>股息股利</h3>
					</div>
				</div>
				<!-- /contact -->

				<!-- contact -->
				<div class="col-sm-4 StockInfo">
					<div class="contact"　id="StockEPS">
						<i class="fa fa-map-marker"></i>
						<h3>EPS</h3>
					</div>
				</div>
				<!-- /contact -->

			</div>
			<!-- /詳細資訊 -->
		</div>
		<!-- /Container -->
    </div>
	<!-- /Contact -->


	<!-- Footer -->
	<footer id="footer" class="sm-padding bg-dark">
		<!-- Container -->
		<div class="container">

			<!-- Row -->
			<div class="row">

				<div class="col-md-12">

					<!-- footer logo -->
					<div class="footer-logo">
                        <h1><a href="." class="white-text">Lazy Stock</a></h1>
					</div>
					<!-- /footer logo -->

					<!-- footer follow -->
					<ul class="footer-follow">
                        <li>投資有賺有賠．策略工具並不保証100%獲利</li>
                        <!--
						<li><a href="#"><i class="fa fa-facebook"></i></a></li>
						<li><a href="#"><i class="fa fa-twitter"></i></a></li>
						<li><a href="#"><i class="fa fa-google-plus"></i></a></li>
						<li><a href="#"><i class="fa fa-instagram"></i></a></li>
						<li><a href="#"><i class="fa fa-linkedin"></i></a></li>
						<li><a href="#"><i class="fa fa-youtube"></i></a></li>
                        -->
					</ul>
					<!-- /footer follow -->

					<!-- footer copyright -->
					<div class="footer-copyright">
						<p>Copyright © 2017. All Rights Reserved. Designed by <a href="https://colorlib.com" target="_blank">LAZZZY</a></p>
					</div>
					<!-- /footer copyright -->

				</div>

			</div>
			<!-- /Row -->

		</div>
		<!-- /Container -->

	</footer>
	<!-- /Footer -->

	<!-- Back to top -->
	<div id="back-to-top"></div>
	<!-- /Back to top -->

	<!-- Preloader -->
	<div id="preloader">
		<div class="preloader">
			<span></span>
			<span></span>
			<span></span>
			<span></span>
		</div>
	</div>
	<!-- /Preloader -->
    

    <div id="ToastMsg" >Some text some message..</div>


	<!-- jQuery Plugins -->
    <%:Scripts.Render("~/bundles/js") %>
	<%--
    <script type="text/javascript" src="js/jquery.min.js"></script>
	<script type="text/javascript" src="js/bootstrap.min.js"></script>
	<script type="text/javascript" src="js/owl.carousel.min.js"></script>
	<script type="text/javascript" src="js/jquery.magnific-popup.js"></script>
    <script type="text/javascript" src="js/Common.js?t=<%=DateTime.Now.ToString("yyyyMMddHHmmssfff") %>"></script>
    <script type="text/javascript" src="js/main.js?t=<%=DateTime.Now.ToString("yyyyMMddHHmmssfff") %>"></script>
    --%>

    
    
    
    
</body>

</html>
