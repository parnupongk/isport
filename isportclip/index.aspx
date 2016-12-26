<%@ Page Language="C#" AutoEventWireup="true" Inherits="isportclip.index" Codebehind="index.aspx.cs" %>
<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<body>
    <mobile:Form id="frmMain" runat="server">
    <mobile:DeviceSpecific ID="DeviceSpecific1" Runat="server">
<Choice Filter="isHTML32">
<ScriptTemplate>
<link rel="stylesheet" href="mobileStyle.css" type="text/css" />

</ScriptTemplate> </Choice>

<Choice Filter="isWML11">
<ScriptTemplate>
<link rel="stylesheet" href="mobileStyle.css" type="text/css" />
</ScriptTemplate> </Choice>

</mobile:DeviceSpecific>
    </mobile:Form>
</body>
</html>
