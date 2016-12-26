<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="isport_edt.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>EDT.guru</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <link rel="shortcut icon" href="../images/logo.ico" />
    <link href="../dist/css/bootstrap.css" rel="stylesheet" />
    <link href="../dist/css/customCSS.css" rel="Stylesheet" />
    <link href="../navbar-fixed-top.css" rel="stylesheet">
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="html5shiv.js"></script>
      <script src="respond.min.js"></script>
    <![endif]-->

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
                    <img src="../images/logo.png" /></a>
            </div>
            <div class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                    <li class="active"><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamdaraclip_sub">คลิปลับดารา</a></li>
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
            <div class="row featurette">
                <div class="col-md-12">
                    <div class="panel">
                        <div class="panel-body">
                            <asp:Label ID="lblContentSub" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row featurette">
                <div class="page-header">
                    <asp:Label ID="lblTitle" runat="server"></asp:Label>
                </div>
                <div class="col-md-8">

                    <asp:Label ID="lblContent" runat="server"></asp:Label>
                </div>


                <div class="col-md-4">

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
            </div>

            <hr class="featurette-divider" />
            <div class="col-md-12">
                <asp:Label ID="lblFooter" runat="server"></asp:Label>
            </div>
        </form>



    </div>

    <footer>

      
        <p class="pull-right"><a href="#">Back to top</a></p>
        <p>&copy; 2013 Company, Isport. &middot;</p>
     </footer>

    <script src="../assets/js/jquery.js"></script>
    <script src="../dist/js/bootstrap.min.js"></script>
    <script src="../assets/js/holder.js"></script>
</body>
</html>

