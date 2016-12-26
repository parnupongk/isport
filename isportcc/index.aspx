<%@ Page Language="C#" AutoEventWireup="true" Inherits="isportcc.index" Codebehind="index.aspx.cs" %>
<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>


<%@ Register src="uHeader.ascx" tagname="uHeader" tagprefix="uc1" %>
<%@ Register src="uContent.ascx" tagname="uContent" tagprefix="uc2" %>
<%@ Register src="uFooter.ascx" tagname="uFooter" tagprefix="uc3" %>


<html xmlns="http://www.w3.org/1999/xhtml" >
<body>

    <mobile:Form id="frmMain"   runat="server">
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
<uc1:uHeader ID="uHeader1" runat="server" />
    <uc2:uContent ID="uContent1" runat="server" />
    <uc3:uFooter ID="uFooter1" runat="server" />
    
    </mobile:Form>
    

    

    

</body>
</html>
