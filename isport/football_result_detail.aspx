<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="football_result_detail.aspx.cs" Inherits="isport.football_result_detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="logo.ico">

    <title>wap.isport.co.th</title>



    <!-- Custom styles for this template -->
    <!-- Bootstrap core CSS -->
    <link href="bootstrap.css" rel="stylesheet">
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

    <div id="divMenuHeader_low" runat="server" class="row">
        <asp:Label ID="lblHeaderLow" runat="server"></asp:Label>
    </div>


    <form id="frmMain" runat="server">
    </form>

    <div id="social" class="row">
        <asp:Label ID="lblFooter" CssClass="transparent" runat="server"></asp:Label>
    </div>

    <hr class="featurette-divider">
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

</body>
</html>
