<%@ Page Title="" Language="C#" MasterPageFile="~/admin/adminMaster.Master" AutoEventWireup="true" CodeBehind="adminedtreloadapi.aspx.cs" Inherits="isport.admin.adminedtreloadapi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentDetail" runat="server">
     <div> EDT ID :&nbsp; <asp:TextBox ID="txtEdtId" runat="server" Height="16px" Width="340px"></asp:TextBox> &nbsp;<asp:Button ID="btnsubmit" runat="server" Text="Submit" OnClick="btnsubmit_Click" />&nbsp;*** ex. id,id,id....</div>
    <div style="text-align:center">  <asp:Label ID="lblResult" runat="server" ForeColor="#FF5050"></asp:Label></div>
</asp:Content>
