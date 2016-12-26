<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="fbscoredetail.aspx.cs" Inherits="WS_BB.fbscoredetail" %>

<%@ Register Assembly="DevExpress.Web.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dxrp" %>
<%@ Register assembly="DevExpress.Web.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxPanel" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dxtc" %>
<%@ Register assembly="DevExpress.Web.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxClasses" tagprefix="dxw" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%"  
    BackColor="#DDECFE" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" 
    CssPostfix="Office2003_Blue" HeaderText="Score Detail" 
    ImageFolder="~/App_Themes/Office2003 Blue/{0}/" 
        EnableDefaultAppearance="False" HorizontalAlign="Center">
    <PanelCollection>
<dxp:PanelContent runat="server">

    <table style="width:100%;" cellpadding="2" cellspacing="2">
        <tr>
            <td>
                </td>
            <td>
                <asp:Label ID="lblLeague" runat="server" CssClass="fontHeaderHome"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="lblScore" runat="server" CssClass="fontSubHeader"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
                <dxtc:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="1" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                    ImageFolder="~/App_Themes/Glass/{0}/" TabSpacing="0px" Width="100%" 
                    >
                    <TabPages>
                        <dxtc:TabPage Text="เกาะติดเกม" >
                            <TabStyle HorizontalAlign="Center">
                            </TabStyle>
                            <ContentCollection>
                                <dxw:ContentControl runat="server">
                                    <dxwgv:ASPxGridView ID="gvGame" runat="server" AutoGenerateColumns="False" 
                                        CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" 
                                        CssPostfix="BlackGlass" OnHtmlRowPrepared="gvGame_HtmlRowPrepared" 
                                        Width="400px">
                                        <Columns>
                                            <dxwgv:GridViewDataTextColumn Caption="Minute" FieldName="MINUTE"   ToolTip="MINUTE"
                                                VisibleIndex="0" CellStyle-BorderRight-BorderStyle="None">
<CellStyle>
<BorderRight BorderStyle="None"></BorderRight>
</CellStyle>
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Even" ToolTip="Even"
                                                VisibleIndex="1" CellStyle-BorderRight-BorderStyle="None">
                                                <DataItemTemplate>
                                                    <asp:Image ID="img" runat="server" />
                                                </DataItemTemplate>
<CellStyle>
<BorderRight BorderStyle="None"></BorderRight>
</CellStyle>
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Player" FieldName="playerEN"  ToolTip="Player"
                                                VisibleIndex="2" CellStyle-BorderRight-BorderStyle="None">
<CellStyle>
<BorderRight BorderStyle="None"></BorderRight>
</CellStyle>
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Team" FieldName="teamNameEN" 
                                                VisibleIndex="3">
                                            </dxwgv:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsPager Visible="False">
                                        </SettingsPager>
                                        <Images ImageFolder="~/App_Themes/BlackGlass/{0}/">
                                        </Images>
                                        <Styles CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" 
                                            CssPostfix="BlackGlass">
                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                            </Header>
                                        </Styles>
                                        <Border BorderStyle="None" />
                                    </dxwgv:ASPxGridView>
                                </dxw:ContentControl>
                            </ContentCollection>
                        </dxtc:TabPage>
                        <dxtc:TabPage Text="รายชื่อผู้เล่น">
                            <ContentCollection>
                                <dxw:ContentControl runat="server">
                                    <table cellpadding="3" cellspacing="3" style="width:100%;">
                                        <tr bgcolor="#EDF3F4">
                                            <td >
                                                <asp:Label ID="lblTeam1" runat="server" CssClass="fontSubHeader"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTeam2" runat="server" CssClass="fontSubHeader"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr bgcolor="#EDF3F4" >
                                            <td valign="top">
                                                <dxwgv:ASPxGridView ID="gvPlayer1" runat="server" AutoGenerateColumns="False" 
                                                    CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass" 
                                                     Width="100%">
                                                    <Columns>
                                                        <dxwgv:GridViewDataTextColumn Caption="Player" FieldName="PLAYER_NAME"  VisibleIndex="0">
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Position" FieldName="POSITION"  VisibleIndex="1">
                                                        </dxwgv:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsPager Visible="False">
                                                    </SettingsPager>
                                                    <Images ImageFolder="~/App_Themes/BlackGlass/{0}/">
                                                    </Images>
                                                    <Styles CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" 
                                                        CssPostfix="BlackGlass">
                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                        </Header>
                                                    </Styles>
                                                    <Border BorderStyle="NotSet" />
                                                </dxwgv:ASPxGridView>
                                            </td>
                                            <td valign="top"> 
                                                <dxwgv:ASPxGridView ID="gvPlayer2" runat="server" AutoGenerateColumns="False" 
                                                    CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass" 
                                                     Width="100%">
                                                    <Columns>
                                                        <dxwgv:GridViewDataTextColumn Caption="Player" FieldName="PLAYER_NAME" 
                                                            ToolTip="Player" VisibleIndex="0">
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Position" FieldName="POSITION"  VisibleIndex="1">
                                                        </dxwgv:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsPager Visible="False">
                                                    </SettingsPager>
                                                    <Images ImageFolder="~/App_Themes/BlackGlass/{0}/">
                                                    </Images>
                                                    <Styles CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" 
                                                        CssPostfix="BlackGlass">
                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                        </Header>
                                                    </Styles>
                                                    <Border BorderStyle="NotSet" />
                                                </dxwgv:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                </dxw:ContentControl>
                            </ContentCollection>
                        </dxtc:TabPage>
                    </TabPages>
                    <TabImage Url="~/images/footer_menu_live.png" />
                    <Paddings PaddingLeft="0px" />
                    <TabStyle HorizontalAlign="Center">
                    </TabStyle>
                    <ContentStyle>
                        <Border BorderColor="#4986A2" />
                    </ContentStyle>
                    
                </dxtc:ASPxPageControl>
            </td>
        </tr>
    </table>
        </dxp:PanelContent>
</PanelCollection>
    <TopRightCorner Height="9px" Url="~/App_Themes/Office2003 Blue/Web/rpTopRightCorner.png" 
        Width="9px" />
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
        <BackgroundImage ImageUrl="~/App_Themes/Office2003 Blue/Web/rpHeader.png" 
            HorizontalPosition="left" Repeat="RepeatX" VerticalPosition="top" />
    </HeaderLeftEdge>
        <HeaderContent>
            <BackgroundImage HorizontalPosition="left" 
                ImageUrl="~/App_Themes/Office2003 Blue/Web/rpHeader.png" Repeat="RepeatX" 
                VerticalPosition="top" />
        </HeaderContent>
    <TopLeftCorner Height="9px" Url="~/App_Themes/Office2003 Blue/Web/rpTopLeftCorner.png" 
        Width="9px" />
    <ContentPaddings PaddingBottom="10px" 
        PaddingTop="10px" Padding="2px" />
    <TopEdge>
        <BackgroundImage ImageUrl="~/App_Themes/Office2003 Blue/Web/rpTopEdge.png" 
            HorizontalPosition="left" Repeat="RepeatX" VerticalPosition="top" />
    </TopEdge>
    <NoHeaderTopLeftCorner Height="9px" 
        Url="~/App_Themes/Office2003 Blue/Web/rpNoHeaderTopLeftCorner.png" 
            Width="9px" />
    <HeaderRightEdge>
        <BackgroundImage ImageUrl="~/App_Themes/Office2003 Blue/Web/rpHeader.png" 
            HorizontalPosition="left" Repeat="RepeatX" VerticalPosition="top" />
    </HeaderRightEdge>
    <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
    <HeaderStyle BackColor="#7BA4E0">
        <Paddings Padding="0px" PaddingBottom="7px" PaddingLeft="2px" 
            PaddingRight="2px" />
        <BorderBottom BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
    </HeaderStyle>
    </dxrp:ASPxRoundPanel>
</asp:Content>
