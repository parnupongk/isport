<%@ Page Language="C#" Buffer="false" EnableViewState="false" AutoEventWireup="true" Inherits="isport_payment.pay_dtac" Codebehind="pay_dtac.aspx.cs" ResponseEncoding="utf-8" %>
<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <meta HTTP-EQUIV="REFRESH" content="1; url=http://wap.isport.co.th/sport_center/isport/index.aspx">
</head>
<body>
<Mobile:StyleSheet ID="StyleSheet1" runat="server">
  <Style name="title" Font-size="Small" Font-Bold="true" />
  <Style name="messageError" ForeColor="red" />
</Mobile:StyleSheet>



    <mobile:Form id="frmMain"  runat="server">
<mobile:Panel ID="pnlHeader" Runat="server">
    
    </mobile:Panel>
    <mobile:Panel ID="pnlContent" Runat="server">
    
    </mobile:Panel>
    <mobile:Panel ID="pnlFooter" Runat="server">
    
    </mobile:Panel>
    </mobile:Form>
    
    <mobile:Form id="frmUcl"  runat="server">
    
    <mobile:Label ID="lbl5Bath" runat="server"  Text=""></mobile:Label>
    <mobile:Link ID="lnk5Bath"  runat="server" >[OK]</mobile:Link>

    <mobile:Label ID="lbl10Bath" runat="server"  Text=""></mobile:Label>
    <mobile:Link ID="lnk10Bath" runat="server">[OK]</mobile:Link>

    <mobile:Label ID="lbl50Bath" runat="server"  Text=""></mobile:Label>
    <mobile:Link ID="lnk50Bath" runat="server">[OK]</mobile:Link>
    </mobile:Form>
    
</body>
</html>
