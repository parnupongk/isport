<%@ Page Language="C#" AutoEventWireup="true" Inherits="isport.sportcc_ileague" Codebehind="sportcc_ileague.aspx.cs"  ResponseEncoding="utf-8"%>
<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>

<%@ Register src="userControl/muHeader.ascx" tagname="muHeader" tagprefix="uc1" %>

<%@ Register src="userControl/muContent.ascx" tagname="muContent" tagprefix="uc2" %>
<%@ Register src="userControl/muFooter.ascx" tagname="muFooter" tagprefix="uc3" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<body>

    <mobile:Form id="frmMain" runat="server">
        <mobile:DeviceSpecific ID="DeviceSpecific1" Runat="server">
<Choice Filter="isHTML32">
<ScriptTemplate>
<link rel="stylesheet" href="css/isportcc.css" type="text/css" />

</ScriptTemplate> </Choice>

<Choice Filter="isWML11">
<ScriptTemplate>
<link rel="stylesheet" href="css/isportcc.css" type="text/css" />
</ScriptTemplate> </Choice>

</mobile:DeviceSpecific>
  <uc1:muHeader ID="muHeader" runat="server" />
  <uc2:muContent ID="muContent" runat="server" />
    <uc3:muFooter ID="muFooter" runat="server" />
    </mobile:Form>
  

    
  

</body>
</html>
