<%@ Page Title="" Language="C#" MasterPageFile="~/master_isport.Master" AutoEventWireup="true"
    CodeBehind="sub.aspx.cs" Inherits="webIsport.sub" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="fb-root">
    </div>
    <script>                (function (d, s, id) {
                    var js, fjs = d.getElementsByTagName(s)[0];
                    if (d.getElementById(id)) return;
                    js = d.createElement(s); js.id = id;
                    js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&version=v2.4&appId=1452449645079652";
                    fjs.parentNode.insertBefore(js, fjs);
                }(document, 'script', 'facebook-jssdk'));</script>

    <section id="news">
                <div id="divNews" runat="server" class="row">
                    <div class='row'><div class='page-header'><h1><img src='images/news-icon.png' alt='isport'/>ข่าว</h1></div>
                        <asp:Label ID="lblNews" runat="server"></asp:Label>
                         <div class="col-md-3">


                             <asp:Label ID="lblAds" runat="server"></asp:Label>
                             <!-- yengo ads
                             <script>(function(e){var t="DIV_YNG_"+e+"_"+parseInt(Math.random()*1e3); document.write('<div id="'+t+'" class="yengo-block yengo-block-'+e+'"></div>'); if("undefined"===typeof loaded_blocks_yengo){loaded_blocks_yengo=[]; function n(){var e=loaded_blocks_yengo.shift(); var t=e.adp_id; var r=e.div; var i=document.createElement("script"); i.type="text/javascript"; i.async=true; i.charset="windows-1251"; i.src="//www.yengo.com/data/"+t+".js?async=1&div="+r+"&t="+Math.random(); var s=document.getElementsByTagName("head")[0]||document.getElementsByTagName("body")[0]; s.appendChild(i); var o=setInterval(function(){if(document.getElementById(r).innerHTML&&loaded_blocks_yengo.length){n(); clearInterval(o)}},50)} setTimeout(n)}loaded_blocks_yengo.push({adp_id:e,div:t})})(128104)</script>

 -->
<div class="fb-page" data-href="https://www.facebook.com/isportclub/" data-tabs="timeline" data-small-header="false" data-adapt-container-width="true" data-hide-cover="false" data-show-facepile="true"><blockquote cite="https://www.facebook.com/isportclub/" class="fb-xfbml-parse-ignore"><a href="https://www.facebook.com/isportclub/">Isportclub</a></blockquote></div>

                         </div>
                        
                    </div>
                    <asp:Label ID="lblNewsFbThai" runat="server"></asp:Label>
                </div>

            </section>
    <section id="service">
                <div class="row">
                  <div class="page-header">
                        <h1>สมัครบริการ</h1>
                        <!--p class="lead blog-description">The official example template of creating a blog with Bootstrap.</p-->
                </div>

                <asp:Label ID="lblContent" runat="server"></asp:Label>

                </div>
            </section>

    <section id="program">
    <div class="row">
            <asp:Label ID="lblProgram" runat="server"></asp:Label>
                  <!--div class="page-header">
                        <h1><img src="images/ic_program.png" alt="isport program">โปรแกรม</h1>
                        
                </div-->
     </div>
    </section>
</asp:Content>
