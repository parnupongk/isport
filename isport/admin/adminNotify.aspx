<%@ Page Title="" Language="C#" MasterPageFile="~/admin/adminMaster.Master" AutoEventWireup="true" CodeBehind="adminNotify.aspx.cs" Inherits="isport.admin.adminNotify" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentDetail" runat="server">
    <asp:TextBox ID="txtNotify" TextMode="MultiLine" runat="server" Height="69px" Width="369px"></asp:TextBox>


    <asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" Text="Send" />
&nbsp;


</asp:Content>
