<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="isport.index" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="Keywords" content="ข่าวกีฬา, ฟุตบอล, ข่าวบอล, ผลบอล, ผลบอลสด,  ผลการแข่งขัน, ผลฟุตบอล, วิเคราะห์บอล, ผลบอลพรีเมียร์ลีก, ผลบอลไทยพรีเมียร์ลีก, ยูโร 2012 , ผลบอลล่าสุด, พรีเมียร์ลีก, กัลโช่, บุนเดสลีกา, ลา ลีกา, ยูฟ่า แชมเปี้ยนส์ ลีก, ยูโรปา ลีก, เทนนิส, กอล์ฟ, มวย, สยามกีฬาทีวี, NBT19, ฟุตบอลสยามทีวี, สตาร์ซอคเก้อร์ทีวี, สยามกีฬาTV, FootballsiamTV, StarsoccerTV, ไฮไลท์ฟุตบอล, ไทยพรีเมียร์ลีก, ดิวิชั่น1, ลีกภูมิภาค, ไทยคม เอฟเอคัพ, โตโยต้าลีกคัพ">
    <meta name="Description" content="SMS LiveScore ผลสด อัพเดททุกการทำประตู อัตราต่อรองพรีเมียร์ลีก โปรแกรมการแข่งขันและถ่ายทอดสด วิเคราะห์ทีมที่ใช้ เพิ่มความมั้นใจ ก่อนการเชียร์บอล">
    <meta name="author" content="">
    <!--link rel="shortcut icon" href="http://www.isport.co.th/dist/img/logo.png"-->
    <title>I-Sport Co., Ltd.</title>
    <!--link href="bootstrap.css" rel="stylesheet"-->
     <link href="http://www.isport.co.th/dist/css/bootstrap.min.css" rel="stylesheet">

    <link href="customCSS.css" rel="stylesheet">
        <link rel="stylesheet" href="http://www.isport.co.th/dist/layerslider/css/layerslider.css" type="text/css">
    <link href="http://www.isport.co.th/dist/css/theme-2017.css?v34" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css?family=Kanit" rel="stylesheet">
    <!-- Bootstrap core CSS -->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="html5shiv.js"></script>
      <script src="respond.min.js"></script>
    <![endif]-->
    <link rel="stylesheet" href="http://www.isport.co.th/dist/layerslider/skins/v5/skin.css" type="text/css">
<style type="text/css">
body { font-family: 'Kanit', sans-serif; color: #333; }
.ud-ribbon { position: fixed; top: 0; left: 0; z-index: 9999; }
.bgo { background: transparent; }
.hfix { height: 500px; background-color: #FFF; border-right: 5px solid #f7f5f5; }
.button-download { line-height: 25px; }
.row_Header_Left { color: #F90; }
</style>

</head>
<body>

    <div id="fb-root">
    </div>
    <script>                (function (d, s, id) {
                    var js, fjs = d.getElementsByTagName(s)[0];
                    if (d.getElementById(id)) return;
                    js = d.createElement(s); js.id = id;
                    js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&version=v2.4&appId=1452449645079652";
                    fjs.parentNode.insertBefore(js, fjs);
                }(document, 'script', 'facebook-jssdk'));</script>

    <div id="divMenuHeader" runat="server" class="navbar navbar-default">
        <div id="header" class="navbar navbar-fixed-top" role="navigation">
      <div class="container">
    <div class="navbar-header">
          <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse"> <span class="sr-only">Toggle navigation</span> <span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span> </button>
          <a href="index.aspx" id="ctl00_lnkMain" class="navbar-brand wow bounceInRight" style="visibility: visible; animation-name: bounceInRight;"><img alt="isport" src="http://www.isport.co.th/dist/img/logo.png"></a> </div>
    <div class="navbar-collapse collapse">
          <ul class="nav navbar-nav">
        <li><a href="dist/incs/sub.aspx" title="I-SPORT">HOME</a></li>
        <li class="dropdown show"> <a data-toggle="dropdown" class="dropdown-toggle" href="http://www.isport.co.th/sub.aspx?#footer" title="SERVICES">SERVICES <i class="fa fa-angle-down" aria-hidden="true"></i> </a>
              <ul class="dropdown-menu" id="menu1">
            <li class="dropdown"><a href="dist/incs/pr.aspx" title="ชิงรางวัล">ชิงรางวัล <i class="fa fa-angle-down" aria-hidden="true"></i></a>
                  <ul class="dropdown-menu">
                <li><a href="http://wap.isport.co.th/isportui/?p=smstaijai" title="ชิงรางวัลใหญ่">ชิงรางวัลใหญ่</a></li>
                <li><a href="http://wap.isport.co.th/isportui/index.aspx?p=menupromo" title="ชิงทอง">ชิงทอง!</a></li>
              </ul>
                </li>
            <li><a href="http://wap.isport.co.th/isportui/?p=news&amp;class_id=" title="ข่าวเด่น">ข่าวเด่น</a></li>
            <li class="dropdown"><a href="#" title="ผลฟุตบอล">ผลฟุตบอล <i class="fa fa-angle-down" aria-hidden="true"></i></a>
                  <ul class="dropdown-menu">
                <li><a href="http://wap.isport.co.th/sport_center/isport/football_livescore.aspx?lng=L" title="ผลฟุตบอลสด">ผลฟุตบอลสด</a></li>
                <li><a href="http://wap.isport.co.th/isportui/?p=program&amp;lng=L&amp;class_id=" title="โปรแกรมการแข่งขัน">โปรแกรมการแข่งขัน</a></li>
              </ul>
                </li>
            <li class="dropdown right"><a href="#" title="วิเคราะห์ฟุตบอล">วิเคราะห์ฟุตบอล <i class="fa fa-angle-down" aria-hidden="true"></i></a>
                  <ul class="dropdown-menu">
                <li><a href="http://wap.isport.co.th/isportui/?p=analyse" title="ทรรศนะวันนี้">ทรรศนะวันนี้</a></li>
                <li><a href="http://wap.isport.co.th/isportui/?p=tdedsportpool" title="ทีเด็ดสปอร์ตพูล">ทีเด็ดสปอร์ตพูล</a></li>
                <li><a href="http://wap.isport.co.th/isportui/?p=tdedstartscore" title="ทีเด็ดสตาร์ซอคเก้อร์">ทีเด็ดสตาร์ซอคเก้อร์</a></li>
                <li><a href="http://wap.isport.co.th/isportui/?p=tdedfootball" title="ทีเด็ดจัดหนัก">ทีเด็ดจัดหนัก</a></li>
              </ul>
                </li>
            <li><a href="http://wap.isport.co.th/isportui/?p=othersport" title="กีฬาอื่นๆ">กีฬาอื่นๆ</a></li>
          </ul>
            </li>
        <li><a href="http://www.isport.co.th/sub.aspx?#service" title="SUBSCRIBE">SUBSCRIBE</a></li>
        <li><a href="http://www.isportmart.com" target="_blank" title="SHOP">SHOP</a></li>
        <!--<li><a href="#event" title="EVENT">EVENT</a></li>
                    <li><a href="#tour" title="TOUR">TOUR</a></li>
                    <li><a href="#talent" title="TALENT">TALENT</a></li>-->
        <li><a href="http://www.isport.co.th/pr.aspx" title="NEWS">NEWS</a></li>
        <!--<li><a href="#about" title="ABOUT">ABOUT</a></li>
                    <li><a href="#contact" title="CONTACT">CONTACT</a></li>-->
        <li><a href="http://career.isport.co.th" title="JOBS">JOBS</a></li>
        <!--<li><a href="#" title="INVESTOR">INVESTOR</a></li>-->
      </ul>
        </div>
    <!--/.nav-collapse --> 
  </div>
    </div>

        <!-- div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span>
                </button>
                <a id="lnkMain" runat="server" class="navbar-brand" href="#">
                    <img alt="isport" src="images/logo-isport.png" /></a>
            </div>
            <div class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                    <li><a href="http://wap.isport.co.th/isportui/?p=smstaijai">ชิงรางวัลใหญ่</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/index.aspx?p=menupromo">ชิงทอง!</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/?p=news&class_id=">ข่าวเด่น</a></li>
                    <li><a href="http://wap.isport.co.th/sport_center/isport/football_livescore.aspx?lng=L">ผลสดฟุตบอล</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/?p=analyse">ทรรศนะวันนี้</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/?p=tdedsportpool">ทีเด็ดสปอร์ตพูล</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/?p=tdedstartscore">ทีเด็ดสตาร์ซอคเก้อร์</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/?p=tdedfootball">ทีเด็ดจัดหนัก</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/?p=program&lng=L&class_id=">โปรแกรม</a></li>

                    <li><a href="http://wap.isport.co.th/isportui/?p=othersport">กีฬาอื่นๆ</a></li>

                </ul>
            </div>
            <!--/.nav-collapse -->
        </!-->
    </div>
     <script type="text/javascript">
        function close_window() {
            if (confirm("Close Window?")) {
                top.close();
                windos.open(location, '_self').close();
            }
        }
    </script>
    <!-- 
    <div class="fixed" id="divClose" style="display:none;" runat="server"  >
                    <img  src="images/close_btn.png" onclick="javascript:close_window();"/>
        </div> -->
    <div id="wrap" class="container">
        <asp:Label ID="lblHeader" runat="server"></asp:Label>
        <div id="divNews" runat="server" class="row" style="display: block">
            <div class='row'>
                <div class='row_Header_Left'>
                    <img src='images/news-icon.png' alt='isport' />ข่าว
                </div>
                <asp:Label ID="lblNews" runat="server"></asp:Label>
                <div class="col-md-3">


                    <asp:Label ID="lblAds" runat="server"></asp:Label>
                    <!-- yengo ads>
                    <script>(function (e) { var t = "DIV_YNG_" + e + "_" + parseInt(Math.random() * 1e3); document.write('<div id="' + t + '" class="yengo-block yengo-block-' + e + '"></div>'); if ("undefined" === typeof loaded_blocks_yengo) { loaded_blocks_yengo = []; function n() { var e = loaded_blocks_yengo.shift(); var t = e.adp_id; var r = e.div; var i = document.createElement("script"); i.type = "text/javascript"; i.async = true; i.charset = "windows-1251"; i.src = "//www.yengo.com/data/" + t + ".js?async=1&div=" + r + "&t=" + Math.random(); var s = document.getElementsByTagName("head")[0] || document.getElementsByTagName("body")[0]; s.appendChild(i); var o = setInterval(function () { if (document.getElementById(r).innerHTML && loaded_blocks_yengo.length) { n(); clearInterval(o) } }, 50) } setTimeout(n) } loaded_blocks_yengo.push({ adp_id: e, div: t }) })(128104)</script-->



                    <div class="fb-page" data-href="https://www.facebook.com/isportclub" data-small-header="false" data-adapt-container-width="true" data-hide-cover="false" data-show-facepile="true" data-show-posts="false">
                        <div class="fb-xfbml-parse-ignore">
                            <blockquote cite="https://www.facebook.com/isportclub">
                                <a href="https://www.facebook.com/isportclub">Isportclub</a>
                            </blockquote>
                        </div>
                    </div>

                </div>

    </div>

    </div>

        <form id="frmMain" runat="server">
        </form>

    <hr class="featurette-divider">

    <!-- div class="row">
        <div class="col-xs-12 col-md-4 col-sm-6">
            <!-- yengo ads >
            <script>(function (e) { var t = "DIV_YNG_" + e + "_" + parseInt(Math.random() * 1e3); document.write('<div id="' + t + '" class="yengo-block yengo-block-' + e + '"></div>'); if ("undefined" === typeof loaded_blocks_yengo) { loaded_blocks_yengo = []; function n() { var e = loaded_blocks_yengo.shift(); var t = e.adp_id; var r = e.div; var i = document.createElement("script"); i.type = "text/javascript"; i.async = true; i.charset = "windows-1251"; i.src = "//www.yengo.com/data/" + t + ".js?async=1&div=" + r + "&t=" + Math.random(); var s = document.getElementsByTagName("head")[0] || document.getElementsByTagName("body")[0]; s.appendChild(i); var o = setInterval(function () { if (document.getElementById(r).innerHTML && loaded_blocks_yengo.length) { n(); clearInterval(o) } }, 50) } setTimeout(n) } loaded_blocks_yengo.push({ adp_id: e, div: t }) })(128100)</script>
        </div>
    <div class="col-xs-12 col-md-4 col-sm-6">
                <!-- yengo ads>
                <script>(function (e) { var t = "DIV_YNG_" + e + "_" + parseInt(Math.random() * 1e3); document.write('<div id="' + t + '" class="yengo-block yengo-block-' + e + '"></div>'); if ("undefined" === typeof loaded_blocks_yengo) { loaded_blocks_yengo = []; function n() { var e = loaded_blocks_yengo.shift(); var t = e.adp_id; var r = e.div; var i = document.createElement("script"); i.type = "text/javascript"; i.async = true; i.charset = "windows-1251"; i.src = "//www.yengo.com/data/" + t + ".js?async=1&div=" + r + "&t=" + Math.random(); var s = document.getElementsByTagName("head")[0] || document.getElementsByTagName("body")[0]; s.appendChild(i); var o = setInterval(function () { if (document.getElementById(r).innerHTML && loaded_blocks_yengo.length) { n(); clearInterval(o) } }, 50) } setTimeout(n) } loaded_blocks_yengo.push({ adp_id: e, div: t }) })(129843)</script>

                            <!-- google adsense >
                    <script async src="//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>
<!-- wap.isport.co.th >
<ins class="adsbygoogle"
     style="display:block"
     data-ad-client="ca-pub-2796964969269378"
     data-ad-slot="4836932643"
     data-ad-format="auto"></ins>
<script>
    (adsbygoogle = window.adsbygoogle || []).push({});
</script>

            </div>

        </!-->

        <hr class="featurette-divider" />

        <div id="social" class="row">
            <asp:Label ID="lblFooter" CssClass="transparent" runat="server"></asp:Label>
        </div>
        <!-- Site footer -->

        <div class="footer">
            <!--p class="pull-right"><a href="#">Back to top</a></p>
        <p>&copy; 2013 Company, Isport. &middot;</p-->
            <div class="container">
                <div class="col-md-8 col-sm-6 col-xs-12 text-muted pull-left">
                    Copyright &copy; 2014 iSport | All Rights Reserved
                </div>
                <div class="col-md-4 col-xs-12">
                    <div class="pull-right">
                        <!--a href="#" class="text-muted"><i class=" fa fa-pencil "></i> Tearms </a> |  
					 <a href="#" class="text-muted"><i class=" fa fa-puzzle-piece "></i> Privacy </a> | -->
                        <a href="#">Back to top</a>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="jquery.js"></script>
    <script src="bootstrap.min.js"></script>
    <script>
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
  m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

        ga('create', 'UA-59344651-5', 'auto');
        ga('send', 'pageview');

    </script>

    <script>        BootstrapDialog.show({
            message: $('<div></div>').load('remote.html')
        });</script>
</body>
</html>
