<%@ Page Language="C#" AutoEventWireup="true" Inherits="isport_payment.pay_ais" Codebehind="pay_ais.aspx.cs" ResponseEncoding="utf-8" %>
<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<body>
    <mobile:Form id="frmMain" runat="server">
    <mobile:Panel ID="pnlHeader" Runat="server">
    
    </mobile:Panel>
        <mobile:Label ID="lbl5Bath" runat="server" BreakAfter="true"  Text=""></mobile:Label>
    <mobile:Link ID="lnk5Bath" BreakAfter="true"  runat="server" >[OK]</mobile:Link>

    <mobile:Label ID="lbl50Bath" BreakAfter="true" runat="server"  Text=""></mobile:Label>
    <mobile:Link ID="lnk50Bath" BreakAfter="true" runat="server">[OK]</mobile:Link>
    
    <mobile:Panel ID="pnlFooter" Runat="server">
    
    </mobile:Panel>
    </mobile:Form>
</body>
</html>
