<%@ Page Language="C#" AutoEventWireup="true" Inherits="isport_payment.pay_true" ResponseEncoding="utf-8" Codebehind="pay_true.aspx.cs" %>
<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<body>
    <mobile:Form id="Form1" runat="server">
        <mobile:Panel ID="pnlHeader" Runat="server">
        </mobile:Panel>
        <mobile:Label ID="lbl5Bath" runat="server" BreakAfter="true"  Text=""></mobile:Label>
        <mobile:Link ID="lnk5Bath" BreakAfter="true"  runat="server" >[OK]</mobile:Link>
        <mobile:Panel ID="pnlFooter" Runat="server">
        </mobile:Panel>
    </mobile:Form>
</body>
</html>
