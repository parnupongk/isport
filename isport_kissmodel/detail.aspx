<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="detail.aspx.cs" Inherits="isport_kissmodel.detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<asp:Literal runat="server" ID="litMeta" />
    <meta property="fb:app_id" content="833529233349089">
    <meta property="og:type" content="article" />
    <meta property="og:site_name" content="isport" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <asp:Label ID="lblContent" runat="server"></asp:Label>
    <div id="social" class="row">
        <asp:Label ID="lblFooter" runat="server"></asp:Label>
    </div>

</asp:Content>
