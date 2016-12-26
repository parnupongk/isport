<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="indexl.aspx.cs" Inherits="isport.indexl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="Keywords" content="ข่าวกีฬา, ฟุตบอล, ข่าวบอล, ผลบอล, ผลบอลสด,  ผลการแข่งขัน, ผลฟุตบอล, วิเคราะห์บอล, ผลบอลพรีเมียร์ลีก, ผลบอลไทยพรีเมียร์ลีก, ยูโร 2012 , ผลบอลล่าสุด, พรีเมียร์ลีก, กัลโช่, บุนเดสลีกา, ลา ลีกา, ยูฟ่า แชมเปี้ยนส์ ลีก, ยูโรปา ลีก, เทนนิส, กอล์ฟ, มวย, สยามกีฬาทีวี, NBT19, ฟุตบอลสยามทีวี, สตาร์ซอคเก้อร์ทีวี, สยามกีฬาTV, FootballsiamTV, StarsoccerTV, ไฮไลท์ฟุตบอล, ไทยพรีเมียร์ลีก, ดิวิชั่น1, ลีกภูมิภาค, ไทยคม เอฟเอคัพ, โตโยต้าลีกคัพ">
    <meta name="Description" content="อัพเดทข่าวสารวงการกีฬา ฟุตบอล ผลบอล ผลฟุตบอลทั่วโลก พรีเมียร์ลีก ไทยพรีเมียร์ลีก ยูโร 2012 ยูฟ่าแชมเปี้ยนส์ลีก พร้อมทั้งวิเคราะห์บอลจาก สยามกีฬา สตาร์ซอคเก้อร์ สปอร์ตพูล สปอร์ตแมน คลิปรายการจาก สยามกีฬาทีวี ฟุตบอลสยามทีวี สตาร์ซอคเก้อร์ทีวี และ โฟโต้แกลเลอรี่">
    <meta name="author" content="">
    <link rel="shortcut icon" href="logo.ico">
    <title>wap.isport.co.th</title>
    <!-- Custom styles for this template -->
    <!-- Bootstrap core CSS -->
    <link href="bootstrap.css" rel="stylesheet">
    <link href="customCSS.css" rel="stylesheet">
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="html5shiv.js"></script>
      <script src="respond.min.js"></script>
    <![endif]-->
</head>
<body>

    <div id="divMenuHeader" runat="server" class="navbar navbar-default" style="display: none;">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="#">WAP.ISPORT.CO.TH</a>
            </div>
            <div class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                    <li class="active"><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamclip">คลิปลับดารา</a></li>
                    <li><a href="http://kissmodel.net">คลิปเด็ดสาวบิกินี่</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamhoro">เช็คดวงวันนี้</a></li>
                </ul>
                <ul class="nav navbar-nav navbar-right">
                    <li><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamvarity">ประสบการณ์รักพริตตี้</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/indexl.aspx">ผลฟุตบอลวันนี้</a></li>
                </ul>
            </div>
            <!--/.nav-collapse -->
        </div>
    </div>

    <div id="divMenuHeader_low" runat="server" class="row">
        <asp:Label ID="lblHeaderLow" runat="server"></asp:Label>
    </div>

    <div id="wrap">
        <div>
            <script>
                (function () {
                    var cx = '001977746206444305366:xs29x1o-ni4';
                    var gcse = document.createElement('script');
                    gcse.type = 'text/javascript';
                    gcse.async = true;
                    gcse.src = (document.location.protocol == 'https:' ? 'https:' : 'http:') +
                    '//cse.google.com/cse.js?cx=' + cx;
                    var s = document.getElementsByTagName('script')[0];
                    s.parentNode.insertBefore(gcse, s);
                })();
            </script>
            <gcse:search></gcse:search>
        </div>
        <asp:Label ID="lblHeader" runat="server"></asp:Label>
                                                                        <div id="divNews" runat="server" class="row">
                                        <div class='row'><!--div class='row_Header_Left'><img src='images/news-icon.png' alt='isport'/>ข่าว</!--div-->
                        <asp:Label ID="lblNews" runat="server"></asp:Label>
                         <div class="col-md-3">


                             <asp:Label ID="lblAds" runat="server"></asp:Label>
                             <!-- yengo ads-->
                             <!--script>(function(e){var t="DIV_YNG_"+e+"_"+parseInt(Math.random()*1e3); document.write('<div id="'+t+'" class="yengo-block yengo-block-'+e+'"></div>'); if("undefined"===typeof loaded_blocks_yengo){loaded_blocks_yengo=[]; function n(){var e=loaded_blocks_yengo.shift(); var t=e.adp_id; var r=e.div; var i=document.createElement("script"); i.type="text/javascript"; i.async=true; i.charset="windows-1251"; i.src="//www.yengo.com/data/"+t+".js?async=1&div="+r+"&t="+Math.random(); var s=document.getElementsByTagName("head")[0]||document.getElementsByTagName("body")[0]; s.appendChild(i); var o=setInterval(function(){if(document.getElementById(r).innerHTML&&loaded_blocks_yengo.length){n(); clearInterval(o)}},50)} setTimeout(n)}loaded_blocks_yengo.push({adp_id:e,div:t})})(128104)</!--script-->


                            <!--div class="fb-page" data-href="https://www.facebook.com/isportclub" data-small-header="false" data-adapt-container-width="true" data-hide-cover="false" data-show-facepile="true" data-show-posts="false">
                                <div class="fb-xfbml-parse-ignore">
                                    <blockquote cite="https://www.facebook.com/isportclub">
                                        <a href="https://www.facebook.com/isportclub">Isportclub</a></blockquote>
                                </div>
                            </!--div-->

                         </div>
                        
                    </div>
                </div>
        <form id="frmMain" runat="server">
        </form>
    </!--div>

    <hr class="featurette-divider">
    <div class="row">
    <div class="col-xs-12 col-md-4 col-sm-6">
        <!-- yengo ads -->
        <script>(function (e) { var t = "DIV_YNG_" + e + "_" + parseInt(Math.random() * 1e3); document.write('<div id="' + t + '" class="yengo-block yengo-block-' + e + '"></div>'); if ("undefined" === typeof loaded_blocks_yengo) { loaded_blocks_yengo = []; function n() { var e = loaded_blocks_yengo.shift(); var t = e.adp_id; var r = e.div; var i = document.createElement("script"); i.type = "text/javascript"; i.async = true; i.charset = "windows-1251"; i.src = "//www.yengo.com/data/" + t + ".js?async=1&div=" + r + "&t=" + Math.random(); var s = document.getElementsByTagName("head")[0] || document.getElementsByTagName("body")[0]; s.appendChild(i); var o = setInterval(function () { if (document.getElementById(r).innerHTML && loaded_blocks_yengo.length) { n(); clearInterval(o) } }, 50) } setTimeout(n) } loaded_blocks_yengo.push({ adp_id: e, div: t }) })(128100)</script>
    </div>
    <div class="col-xs-12 col-md-4 col-sm-6">
        <!-- yengo ads-->
        <script>(function (e) { var t = "DIV_YNG_" + e + "_" + parseInt(Math.random() * 1e3); document.write('<div id="' + t + '" class="yengo-block yengo-block-' + e + '"></div>'); if ("undefined" === typeof loaded_blocks_yengo) { loaded_blocks_yengo = []; function n() { var e = loaded_blocks_yengo.shift(); var t = e.adp_id; var r = e.div; var i = document.createElement("script"); i.type = "text/javascript"; i.async = true; i.charset = "windows-1251"; i.src = "//www.yengo.com/data/" + t + ".js?async=1&div=" + r + "&t=" + Math.random(); var s = document.getElementsByTagName("head")[0] || document.getElementsByTagName("body")[0]; s.appendChild(i); var o = setInterval(function () { if (document.getElementById(r).innerHTML && loaded_blocks_yengo.length) { n(); clearInterval(o) } }, 50) } setTimeout(n) } loaded_blocks_yengo.push({ adp_id: e, div: t }) })(129843)</script>
    </div>
        </div>

    <hr class="featurette-divider">

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
</body>
</html>
