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
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="html5shiv.js"></script>
      <script src="respond.min.js"></script>
    <![endif]-->
</head>

<body>

    <div id="divMenuHeader" runat="server" class="navbar navbar-default">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span>
                </button>
                <a id="lnkMain" runat="server" class="navbar-brand" href="#">SPORT</a>
            </div>
            <div class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                    <li><a href="http://wap.isport.co.th/isportui/index.aspx?p=t03campaign">ชิงทอง!</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/?p=news&class_id=">ข่าวเด่น</a></li>
                    <li><a href="http://wap.isport.co.th/sport_center/isport/football_livescore.aspx?lng=L">ผลสดฟุตบอล</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/?p=analyse">ทรรศนะวันนี้</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/?p=tdedsportpool">ทีเด็ดสปอร์ตพูล</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/?p=tdedstartscore">ทีเด็ดสตาร์ซอคเก้อร์</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/?p=tdedfootball">ทีเด็ดจัดหนัก</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/?p=program&lng=L&class_id=">โปรแกรม</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/?p=tableMini&class_id=1">ตารางคะแนน</a></li>

                </ul>
            </div>
            <!--/.nav-collapse -->
        </div>
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
