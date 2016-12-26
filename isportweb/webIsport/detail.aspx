<%@ Page Title="" Language="C#" MasterPageFile="~/master_isport_sub.Master" AutoEventWireup="true"
    CodeBehind="detail.aspx.cs" Inherits="webIsport.detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:Literal runat="server" ID="litMeta" />
    <meta property="fb:app_id" content="1452449645079652">
    <meta property="og:type" content="article" />
    <meta property="og:site_name" content="isport" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        window.fbAsyncInit = function () {
            FB.init({
                appId: '1452449645079652',
                xfbml: true,
                version: 'v2.4'
            });
        };

        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) { return; }
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/en_US/sdk.js";
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));
    </script>
    <div class="row">
        <div class='page-header'>
            <h2>
                <asp:Label ID="lblHeader" runat="server"></asp:Label></h2>
        </div>
        <div class="col-md-8">
            <asp:Label ID="lblNews" runat="server"></asp:Label>
        </div>
        <div class="col-md-4">
            <!--div class='thumbnail'>
                <asp:Label ID="lblAds" runat="server" CssClass="row"></asp:Label>
                               
            </div-->

            <div id="fb-root">
            </div>
            <script>                (function (d, s, id) {
                    var js, fjs = d.getElementsByTagName(s)[0];
                    if (d.getElementById(id)) return;
                    js = d.createElement(s); js.id = id;
                    js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&version=v2.4&appId=234540040014586";
                    fjs.parentNode.insertBefore(js, fjs);
                }(document, 'script', 'facebook-jssdk'));</script>
            <div class="fb-page" data-href="https://www.facebook.com/isportclub" data-small-header="true"
                data-adapt-container-width="true" data-hide-cover="true" data-show-facepile="true"
                data-show-posts="true">
                <div class="fb-xfbml-parse-ignore">
                    <blockquote cite="https://www.facebook.com/isportclub">
                        <a href="https://www.facebook.com/isportclub">Isportclub</a>
                    </blockquote>
                </div>
            </div>
            <p></p>
            <div class="row">
                <!-- yengo ads-->
                <script>(function (e) { var t = "DIV_YNG_" + e + "_" + parseInt(Math.random() * 1e3); document.write('<div id="' + t + '" class="yengo-block yengo-block-' + e + '"></div>'); if ("undefined" === typeof loaded_blocks_yengo) { loaded_blocks_yengo = []; function n() { var e = loaded_blocks_yengo.shift(); var t = e.adp_id; var r = e.div; var i = document.createElement("script"); i.type = "text/javascript"; i.async = true; i.charset = "windows-1251"; i.src = "//www.yengo.com/data/" + t + ".js?async=1&div=" + r + "&t=" + Math.random(); var s = document.getElementsByTagName("head")[0] || document.getElementsByTagName("body")[0]; s.appendChild(i); var o = setInterval(function () { if (document.getElementById(r).innerHTML && loaded_blocks_yengo.length) { n(); clearInterval(o) } }, 50) } setTimeout(n) } loaded_blocks_yengo.push({ adp_id: e, div: t }) })(128107)</script>
            </div>

        </div>
    </div>
    <div class="row">
        <div class='fb-like' data-share='true' data-width='450' data-show-faces='true'>
        </div>


        <!-- Place this tag in your head or just before your close body tag. -->
        <script src="https://apis.google.com/js/platform.js" async defer></script>

        <!-- Place this tag where you want the share button to render. -->
        <div class="g-plus" data-action="share"></div>

    </div>
</asp:Content>
