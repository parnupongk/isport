<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="facebookshare.aspx.cs" Inherits="WS_BB.facebookshare" %>
<%@ Register assembly="DevExpress.Web.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxRoundPanel" tagprefix="dxrp" %>
<%@ Register assembly="DevExpress.Web.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxPanel" tagprefix="dxp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" 
    BackColor="#DDECFE" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" 
    CssPostfix="Office2003_Blue" EnableDefaultAppearance="False" HeaderText="News" 
    HorizontalAlign="Center" ImageFolder="~/App_Themes/Office2003 Blue/{0}/">
                    <PanelCollection>
<dxp:PanelContent runat="server">

    <table align="center" cellpadding="2" cellspacing="2" width="998px">
        <tr>
            <td align="left" colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="img" runat="server" />
            </td>
            <td align="left">
                <asp:Label ID="lblHeader" runat="server" CssClass="fontHeaderHome" 
                    Font-Bold="False" Font-Underline="False"></asp:Label>
                <br />
                <br />
                <asp:Label ID="lblTitle" runat="server" CssClass="fontSubHeader" 
                    Font-Bold="False"></asp:Label>
                <br />
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                <asp:Label ID="lblDetail" runat="server" CssClass="fontDetail1"></asp:Label>
            </td>
        </tr>
    </table>
                        </dxp:PanelContent>
</PanelCollection>
                    <TopRightCorner Height="9px" 
                        Url="~/App_Themes/Office2003 Blue/Web/rpTopRightCorner.png" Width="9px" />
                    <BottomRightCorner Height="9px" 
                        Url="~/App_Themes/Office2003 Blue/Web/rpBottomRightCorner.png" Width="9px" />
                    <NoHeaderTopRightCorner Height="9px" 
                        Url="~/App_Themes/Office2003 Blue/Web/rpNoHeaderTopRightCorner.png" 
                        Width="9px" />
                    <BottomLeftCorner Height="9px" 
                        Url="~/App_Themes/Office2003 Blue/Web/rpBottomLeftCorner.png" Width="9px" />
                    <NoHeaderTopEdge BackColor="#DDECFE">
                    </NoHeaderTopEdge>
                    <HeaderLeftEdge>
                        <BackgroundImage HorizontalPosition="left" 
                            ImageUrl="~/App_Themes/Office2003 Blue/Web/rpHeader.png" Repeat="RepeatX" 
                            VerticalPosition="top" />
                    </HeaderLeftEdge>
                    <HeaderContent>
                        <BackgroundImage HorizontalPosition="left" 
                            ImageUrl="~/App_Themes/Office2003 Blue/Web/rpHeader.png" Repeat="RepeatX" 
                            VerticalPosition="top" />
                    </HeaderContent>
                    <TopLeftCorner Height="9px" 
                        Url="~/App_Themes/Office2003 Blue/Web/rpTopLeftCorner.png" Width="9px" />
                    <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingTop="10px" />
                    <TopEdge>
                        <BackgroundImage HorizontalPosition="left" 
                            ImageUrl="~/App_Themes/Office2003 Blue/Web/rpTopEdge.png" Repeat="RepeatX" 
                            VerticalPosition="top" />
                    </TopEdge>
                    <NoHeaderTopLeftCorner Height="9px" 
                        Url="~/App_Themes/Office2003 Blue/Web/rpNoHeaderTopLeftCorner.png" 
                        Width="9px" />
                    <HeaderRightEdge>
                        <BackgroundImage HorizontalPosition="left" 
                            ImageUrl="~/App_Themes/Office2003 Blue/Web/rpHeader.png" Repeat="RepeatX" 
                            VerticalPosition="top" />
                    </HeaderRightEdge>
                    <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                    <HeaderStyle BackColor="#7BA4E0">
                    <Paddings Padding="0px" PaddingBottom="7px" PaddingLeft="2px" 
                        PaddingRight="2px" />
                    <BorderBottom BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                    </HeaderStyle>
                    </dxrp:ASPxRoundPanel>
</asp:Content>
