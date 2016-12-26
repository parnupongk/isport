<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="detail.aspx.cs" Inherits="isport_edt.detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>EDT.guru</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <link rel="shortcut icon" href="images/logo.ico" />
    <link href="dist/css/bootstrap.css" rel="stylesheet" />
    <link href="dist/css/customCSS.css" rel="Stylesheet" />
    <link href="navbar-fixed-top.css" rel="stylesheet">
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="html5shiv.js"></script>
      <script src="respond.min.js"></script>
    <![endif]-->
    <script src="http://maps.googleapis.com/maps/api/js"></script>
    <script>
        var lat = "<%= Lat %>";
        var lng = "<%= Lng %>";
        function initialize() {
            var mapProp = {
                center: new google.maps.LatLng(lat, lng),
                zoom: 12,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);

            var marker = new google.maps.Marker({
                // The below line is equivalent to writing:
                // position: new google.maps.LatLng(-34.397, 150.644)
                position: new google.maps.LatLng(lat, lng), //{ lat: lat, lng: lng },
                map: map
            });

            // You can use a LatLng literal in place of a google.maps.LatLng object when
            // creating the Marker object. Once the Marker object is instantiated, its
            // position will be available as a google.maps.LatLng object. In this case,
            // we retrieve the marker's position using the
            // google.maps.LatLng.getPosition() method.
            var infowindow = new google.maps.InfoWindow({
                content: '<p>Marker Location:' + marker.getPosition() + '</p>'
            });

            google.maps.event.addListener(marker, 'click', function () {
                infowindow.open(map, marker);
            });
        }
        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
</head>

<body>
    <div id="divMenuHeader" runat="server" class="navbar navbar-default navbar-fixed-top" role="navigation">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="#">
                    <img src="images/logo.png" /></a>
            </div>
            <div class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                    <li class="active"><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamclip">คลิปลับดารา</a></li>
                    <!-- li><a href="http://wap.isport.co.th/isportui/indexh.aspx?p=siamlucky">เลขเด็ดเศรษฐี</a></li>
            <li><a href="http://wap.isport.co.th/isportui/indexh.aspx?p=siamhoro">เช็คดวงวันนี้</a></li -->
                </ul>
                <ul class="nav navbar-nav navbar-right">
                    <li><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamvarity">ประสบการณ์รักพริตตี้</a></li>
                    <li><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=bb">Isport</a></li>
                </ul>
            </div>
            <!--/.nav-collapse -->
        </div>
    </div>

    <div class="container">

        <form id="frmMain" runat="server">


            <div class="page-header">

                <h1><small>
                    <asp:Label ID="lblTitle" runat="server"></asp:Label></small></h1>


            </div>
            <div class="col-xs-12 col-md-8">
                <div class="media">


                    <asp:Label ID="lblImgCover" runat="server"></asp:Label>
                    <div class="row_Gray_Center">

                        <asp:Label ID="lblImgConverThumb" runat="server"></asp:Label>

                        <!-- nav>
                                  <ul class="pager">
                                    <li class="next"><a href="#">ภาพทั้งหมด <span aria-hidden="true">&rarr;</span></a></li>
                                  </ul>
                                </nav -->

                    </div>

                    <div class="media-body">
                        <h4 class="media-heading">
                            <asp:Label ID="lblBodyHeader" runat="server"></asp:Label></h4>
                        <p>
                            <asp:Label ID="lblBodyDetail" runat="server"></asp:Label></p>
                    </div>


                    <div class="panel panel-success">
                        <div class="panel-heading">
                            <h4>รายละเอียด</h4>
                        </div>
                        <div class="panel-body">
                            <div class="list-group">
                                <asp:Label ID="lblContentDetail" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-success">
                        <div class="panel-heading">
                            <h4>ข้อมูลน่าสนใจ</h4>
                        </div>
                        <div class="panel-body">
                            <div class="list-group">
                                <asp:Label ID="lblOtherDetail" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>

                </div>

                <asp:Label ID="lblNext" runat="server"></asp:Label>

            </div>


            <div class="col-xs-12 col-md-4">
                <div class="col-md-12">

                    <div class="panel">
                        <div class="panel-body">
                            <script>(function (e) { var t = "DIV_YNG_" + e + "_" + parseInt(Math.random() * 1e3); document.write('<div id="' + t + '" class="yengo-block yengo-block-' + e + '"></div>'); if ("undefined" === typeof loaded_blocks_yengo) { loaded_blocks_yengo = []; function n() { var e = loaded_blocks_yengo.shift(); var t = e.adp_id; var r = e.div; var i = document.createElement("script"); i.type = "text/javascript"; i.async = true; i.charset = "windows-1251"; i.src = "//www.yengo.com/data/" + t + ".js?async=1&div=" + r + "&t=" + Math.random(); var s = document.getElementsByTagName("head")[0] || document.getElementsByTagName("body")[0]; s.appendChild(i); var o = setInterval(function () { if (document.getElementById(r).innerHTML && loaded_blocks_yengo.length) { n(); clearInterval(o) } }, 50) } setTimeout(n) } loaded_blocks_yengo.push({ adp_id: e, div: t }) })(131640)</script>
                        </div>
                    </div>

                    <div class="panel">
                        <div class="panel-body">
                            <asp:Label ID="lblbanner" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div class="panel">
                        <div class="panel-body">
                            <div id="googleMap" style="width: 100%; height: 200px;"><a onclick="javascript:window.localtion=http://maps.google.com/?ll=lat,lng;" href="#"></a></div>
                            <!--img src="https://maps.googleapis.com/maps/api/staticmap?center=40.714728,-73.998672&zoom=14&size=400x400" alt="" /-->
                        </div>
                    </div>

                </div>

            </div>




            <hr class="featurette-divider">
        </form>
        <div class="col-md-12">
            <asp:Label ID="lblFooter" runat="server"></asp:Label>
        </div>
    </div>

    <footer>
      

        <p class="pull-right"><a href="#top">Back to top</a></p>
        <p>&copy; 2013 Company, Isport. &middot;</p>

     </footer>

    <script src="assets/js/jquery.js"></script>
    <script src="dist/js/bootstrap.min.js"></script>
    <script src="assets/js/holder.js"></script>
</body>
</html>

