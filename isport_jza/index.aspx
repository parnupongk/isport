<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true"
    CodeBehind="index.aspx.cs" Inherits="isport_jza.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <link href="dist/css/carousel.css" rel="stylesheet">

    <script src="assets/js/jquery.js"></script>
    <script src="dist/js/bootstrap.min.js"></script>
    <script src="assets/js/holder.js"></script>
    <!-- Carousel
    ================================================== -->
    <div id="myCarousel" class="carousel slide">
      <!-- Indicators -->
      <ol class="carousel-indicators">
        <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
        <li data-target="#myCarousel" data-slide-to="1"></li>
      </ol>
      <div class="carousel-inner">
        <div class="item active">
          <!--img src="images/banner2.gif" data-src="images/banner2.gif" alt="Second slide"-->
          <!-- ads siamdara -->
          <a href='http://ads.samartmedia.com/www/delivery/ck.php?n=abf49e94&amp;cb=INSERT_RANDOM_NUMBER_HERE' target='_blank'>
          <img src='http://ads.samartmedia.com/www/delivery/avw.php?zoneid=780&amp;cb=INSERT_RANDOM_NUMBER_HERE&amp;n=abf49e94' border='0' alt='' /></a>
        <!--img src='http://ads.samartmedia.com/www/delivery/avw.php?zoneid=780&amp;cb=INSERT_RANDOM_NUMBER_HERE&amp;n=a7fb349b' border='0' alt='' /-->

          <div class="container">
            <div class="carousel-caption">
            </div>
          </div>
        </div>
        <div class="item">
        <!-- ads Isport -->
        <a href='http://ads.samartmedia.com/www/delivery/ck.php?n=a34f282c&amp;cb=INSERT_RANDOM_NUMBER_HERE' target='_blank'>
        <img src='http://ads.samartmedia.com/www/delivery/avw.php?zoneid=779&amp;cb=INSERT_RANDOM_NUMBER_HERE&amp;n=a34f282c' border='0' alt='' /></a>
        <!--img src='http://ads.samartmedia.com/www/delivery/avw.php?zoneid=779&amp;cb=INSERT_RANDOM_NUMBER_HERE&amp;n=ad036d52' border='0' alt='' /-->

          <div class="container">
            <div class="carousel-caption">

            
            </div>
          </div>
        </div>

      </div>
      <a class="left carousel-control" href="#myCarousel" data-slide="prev"><span class="glyphicon glyphicon-chevron-left"></span></a>
      <a class="right carousel-control" href="#myCarousel" data-slide="next"><span class="glyphicon glyphicon-chevron-right"></span></a>
    </div><!-- /.carousel -->

        <asp:Label ID="lblContent" runat="server"></asp:Label>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">


    
    <div id="social" class="banner-top">
        <asp:Label ID="lblFooter" runat="server"></asp:Label> 
    </div>
</asp:Content>
