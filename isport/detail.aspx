<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="detail.aspx.cs" Inherits="isport.detailh" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="SMS LiveScore ผลสด อัพเดททุกการทำประตู อัตราต่อรองพรีเมียร์ลีก โปรแกรมการแข่งขันและถ่ายทอดสด วิเคราะห์ทีมที่ใช้ เพิ่มความมั้นใจ ก่อนการเชียร์บอล">
    <meta name="author" content="">
    <link rel="shortcut icon" href="logo.ico" >

    <title>wap.isport.co.th</title>
    <!-- Bootstrap core CSS -->
    <link href="bootstrap.css" rel="stylesheet"/>
   <link href="customCSS.css" rel="Stylesheet" />
   <link href="https://fonts.googleapis.com/css?family=Kanit" rel="stylesheet">

    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="html5shiv.js"></script>
      <script src="respond.min.js"></script>
    <![endif]-->
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
 
 <div id="divMenuHeader" runat="server" class="navbar navbar-default" style="display:none;">
      <div class="container">
        <div class="navbar-header">
          <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
         <a id="lnkMain" runat="server" class="navbar-brand" href="http://wap.isport.co.th/isportui/index.aspx?p=bb"><img alt="isport" src="images/logo-isport.png" /></a>
        </div>
                <div class="navbar-collapse collapse">
          <ul class="nav navbar-nav">
            <li class="active"><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamclip">คลิปลับดารา</a></li>
            <li><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamlucky">เลขเด็ดเศรษฐี</a></li>
            <li><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamhoro">เช็คดวงวันนี้</a></li >
          </ul>
          <ul class="nav navbar-nav navbar-right">
            <li><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamvarity">ประสบการณ์รักพริตตี้</a></li>
            <li><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=bb">ผลฟุตบอลวันนี้</a></li>
          </ul>
        </div><!--/.nav-collapse -->
      </div>
    </div>  
 

 
<form id="frmMain" runat="server">

</form>

    <hr class="featurette-divider">

    <!-- yengo ads -->
    <script>(function(e){var t="DIV_YNG_"+e+"_"+parseInt(Math.random()*1e3); document.write('<div id="'+t+'" class="yengo-block yengo-block-'+e+'"></div>'); if("undefined"===typeof loaded_blocks_yengo){loaded_blocks_yengo=[]; function n(){var e=loaded_blocks_yengo.shift(); var t=e.adp_id; var r=e.div; var i=document.createElement("script"); i.type="text/javascript"; i.async=true; i.charset="windows-1251"; i.src="//www.yengo.com/data/"+t+".js?async=1&div="+r+"&t="+Math.random(); var s=document.getElementsByTagName("head")[0]||document.getElementsByTagName("body")[0]; s.appendChild(i); var o=setInterval(function(){if(document.getElementById(r).innerHTML&&loaded_blocks_yengo.length){n(); clearInterval(o)}},50)} setTimeout(n)}loaded_blocks_yengo.push({adp_id:e,div:t})})(128105)</script>


    <hr class="featurette-divider">
      <!-- Site footer -->
      
      <asp:Label ID="lblFooter" runat="server"></asp:Label>
      
        <footer class="footer">
      
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
                      <a href="#" > Back to top</a>
			  </div> 
			</div>

          </div>
      </footer>



    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="jquery.js"></script>
    <script src="bootstrap.min.js"></script>
    <script>$('#modal').modal('show')</script>
</body>
</html>
