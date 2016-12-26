<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="football_analyse.aspx.cs" Inherits="isport.football_analyse" ResponseEncoding="utf-8" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="logo.ico" >

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
 
<div id="divMenuHeader" runat="server" class="navbar navbar-default">
      <div class="container">
        <div class="navbar-header">
          <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
          <a class="navbar-brand" href="#">WAP.ISPORT.CO.TH</a>
        </div>
        <div class="navbar-collapse collapse">
          <ul class="nav navbar-nav">
            <li class="active"><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamclip">คลิปลับดารา</a></li>
            <li><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamlucky">เลขเด็ดเศรษฐี</a></li>
            <li><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamhoro">เช็คดวงวันนี้</a></li >
          </ul>
          <ul class="nav navbar-nav navbar-right">
            <li><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamvarity">ประสบการณ์รักพริตตี้</a></li>
            <li><a href="http://wap.isport.co.th/isportui/indexl.aspx">ผลฟุตบอลวันนี้</a></li>
          </ul>
        </div><!--/.nav-collapse -->
      </div>
    </div>
 

<form id="frmMain" runat="server">

</form>

    <hr class="featurette-divider">
      <!-- Site footer -->
      <footer>
      <asp:Label ID="lblFooter" CssClass="transparent" runat="server"></asp:Label>
        <p class="pull-right"><a href="#">Back to top</a></p>
        <p>&copy; 2013 Company, Isport. &middot;</p>
      </footer>




    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="jquery.js"></script>
    <script src="bootstrap.min.js"></script>
    
</body>
</html>