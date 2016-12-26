<%@ Control Language="C#" AutoEventWireup="true" Inherits="isport.userControl.uFootballTable" Codebehind="uFootballTable.ascx.cs" %>
<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>

<mobile:ObjectList ID="objTable" Runat="server"  CommandStyle-StyleReference="title"
				LabelStyle-StyleReference="title">
    <DeviceSpecific ID="DeviceSpecific1">
        <Choice Filter="">
            <HeaderTemplate>
            <%#GetHeaderThaiPoint()%>
                <table columns="4" >
<tr ><td   >
    <%#GetTextNo() %></td><td  >
    <%#GetTextCuntry() %></td><td  >
    <%#GetTextMatch()%></td><td  >
    <%#GetTextScore() %></td>
    <td  >
        <%#GetTextLoss() %></td>
</tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
            <td   >
                <mobile:Label ID="lblNo" Alignment="center" Text='<%# Eval(PLACE) %>' Runat="server"></mobile:Label></td>
            <td  >
               <mobile:Label ID="lblContry" Alignment="center" Text='<%# Eval(TEAM_NAME) %>' Runat="server"></mobile:Label> </td>
            <td  >
              <mobile:Label ID="lblRace" Alignment="center" Text='<%# Eval(TOTAL_PLAY) %>' Runat="server"></mobile:Label> </td>
            <td  >
                <mobile:Label Alignment="center" ID="Label2" Text='<%# Eval(TOTAL_POINT) %>' Runat="server"></mobile:Label></td>
            <td  >
               <mobile:Label ID="lblScore" Alignment="center" Text='<%# Eval(TOTAL_DIFF) %>' Runat="server"></mobile:Label> </td>
            </tr>
            </ItemTemplate>
            <FooterTemplate>
            
            </table>
            
</FooterTemplate>
        </Choice>
    </DeviceSpecific>
</mobile:ObjectList> 