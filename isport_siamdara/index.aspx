<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="isport_siamdara.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>siamdaranews.com</title>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta name="description" content=""/>
    <meta name="author" content=""/>
    <link rel="shortcut icon" href="logo.ico" />
    <link href="bootstrap.css" rel="stylesheet"/>
    <link href="carousel.css" rel="Stylesheet"/>
    <link href="customCSS.css" rel="stylesheet"/>
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="html5shiv.js"></script>
      <script src="respond.min.js"></script>
    <![endif]-->
    
</head>

<body>
 <div id="divMenuHeader" runat="server" class="navbar navbar-default" style="display:none;">
      <div class="container">
        <div class="navbar-header">
          <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
          <a class="navbar-brand" href="#"><img src="images/logosiamdara.jpg"  /></a>
        </div>
        <div class="navbar-collapse collapse">
          <ul class="nav navbar-nav">
            <li class="active"><a href="http://wap.isport.co.th/isportui/indexh.aspx?p=siamclip">คลิปลับดารา</a></li>
            <li><a href="http://kissmodel.net">คลิปเด็ดสาวบิกินี่</a></li>
          </ul>
          <ul class="nav navbar-nav navbar-right">
            <li><a href="http://wap.isport.co.th/isportui/indexh.aspx?p=siamvarity">ประสบการณ์รักพริตตี้</a></li>
            <li><a href="http://wap.isport.co.th/isportui/indexh.aspx">Isport</a></li>
          </ul>
        </div><!--/.nav-collapse -->
      </div>
    </div>
   
    <div class="marketing">
    <div id="divMenuHeader_low" runat="server" style="display:none;">
        <asp:Label ID="lblHeaderLow" runat="server"></asp:Label>
        </div>
    </div>
    
    
    
    <form id="frmMain" runat="server">


    <div>
    
    </div>
    </form>
    
    <script src="jquery.js"></script>
    <script src="bootstrap.min.js"></script>
</body>
</html>
