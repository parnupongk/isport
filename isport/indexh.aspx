<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="indexh.aspx.cs" Inherits="isport.indexh" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="logo.ico" >

    <title>wap.isport.co.th</title>



    <!-- Custom styles for this template -->
    <link href="navbar-fixed-top.css" rel="stylesheet">
    <link href="carousel.css" rel="stylesheet">
    <!-- Bootstrap core CSS -->
    <link href="bootstrap.css" rel="stylesheet">
    
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="html5shiv.js"></script>
      <script src="respond.min.js"></script>
    <![endif]-->
  </head>
 
<body>
 <!--script>
     //alert(window.innerWidth);
 </script -->   
 
 <div class="navbar navbar-default navbar-fixed-top">
      <div class="container">
        <div class="navbar-header">
          <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
          <a class="navbar-brand" href="#">Wap.Isport.co.th</a>
        </div>
        <div class="navbar-collapse collapse">
          <ul class="nav navbar-nav">
            <li class="active"><a href="">Home</a></li>
            <li><a href="#about">สปอร์ตแมน</a></li>
            <li><a href="#contact">สตาร์ซอคเก้อร์</a></li>
          </ul>
          <ul class="nav navbar-nav navbar-right">
            <li><a href="../navbar/">Default</a></li>
            <li><a href="../navbar-static-top/">Static top</a></li>
          </ul>
        </div><!--/.nav-collapse -->
      </div>
    </div>
 
 
 <!-- Carousel
    ================================================== -->
    <div id="myCarousel" class="carousel slide">
      <!-- Indicators -->
      <ol class="carousel-indicators">
        <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
        <li data-target="#myCarousel" data-slide-to="1"></li>
        <li data-target="#myCarousel" data-slide-to="2"></li>
      </ol>
      <div class="carousel-inner">
        <div class="item">
        <img src="http://wap.isport.co.th/isportui/upload/201406121511t-ded_starsoccer_320x200.gif" data-src="http://wap.isport.co.th/isportui/upload/201406121511t-ded_starsoccer_320x200.gif" >
            <div class="container">
                <div class="carousel-caption">
                  <!-- h4>:: ทีเด็ดบอลชุดสปอร์ตพูล::</h4>
                  <p>ท่านจะได้รับ SMS ทีเด็ดบอลชุด เฉพาะวันที่มีบอลชุดเด็ดวันละ 1-2 ชุด</p>
                  <p><a class="btn btn-large btn-primary" href="http://wap.isport.co.th/sport_center/sms/confirm.aspx?pssv_id=269&command=S&pro=D">สมัคร บริการ</a></p -->
                </div>
          </div>
        </div>
        <div class="item">
        <img src="http://wap.isport.co.th/isportui/upload/201406121511t-ded_starsoccer_320x200.gif" data-src="http://wap.isport.co.th/isportui/upload/201406121511t-ded_starsoccer_320x200.gif" >
          <div class="container">
            <div class="carousel-caption">
              <!--h4>:: ทีเด็ดสปอร์ตแมน ::</h4>
              <p>ท่านจะได้รับ SMS ทีเด็ดสปอร์ตแมน ทุกวันที่มีบอลคู่เด็ด วันละ 1-2 ข้อความ</p>
              <p><a class="btn btn-large btn-primary" href="http://wap.isport.co.th/sport_center/sms/confirm.aspx?pssv_id=447&command=S&pro=D">สมัคร บริการ</a></p -->
            </div>
          </div>
        </div>
        <div class="item active">
        <img src="images/test.jpg" data-src="images/test.jpg" alt="First slide">
          <div class="container">
            <div class="carousel-caption">
              <!-- h4>:: ทีเด็ดสตาร์ซอคเก้อร์::</h4>
              <p>ท่านจะได้รับ SMS ทีเด็ดสตาร์ซอคเก้อร์ ฟันธงคู่เด็ดวันละ 1 ตัว เฉพาะวันที่มีบอลเด็ดๆ เท่านั้น</p>
              <p><a class="btn btn-large btn-primary" href="http://wap.isport.co.th/sport_center/sms/confirm.aspx?pssv_id=312&command=S&pro=D">สมัคร บริการ</a></p -->
            </div>
          </div>
        </div>
      </div>
      <a class="left carousel-control" href="#myCarousel" data-slide="prev"><span class="glyphicon glyphicon-chevron-left"></span></a>
      <a class="right carousel-control" href="#myCarousel" data-slide="next"><span class="glyphicon glyphicon-chevron-right"></span></a>
    </div><!-- /.carousel -->
 
 
     <div class="container">
<form id="frmMain" runat="server">

</form>

    <hr class="featurette-divider">
      <!-- Site footer -->
      <footer>
      <asp:Label ID="lblFooter" runat="server"></asp:Label>
        <p class="pull-right"><a href="#">Back to top</a></p>
        <p>&copy; 2013 Company, Isport. &middot;</p>
      </footer>

    </div> <!-- /container -->


    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="jquery.js"></script>
    <script src="bootstrap.min.js"></script>
    <script src="holder.js"></script>
    
</body>
</html>
