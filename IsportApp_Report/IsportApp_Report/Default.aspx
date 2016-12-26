<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IsportApp_Report._Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<%@ Register assembly="DevExpress.Web.ASPxScheduler.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxScheduler" tagprefix="dxwschs" %>

<%@ Register assembly="DevExpress.Web.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dxtc" %>
<%@ Register assembly="DevExpress.Web.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxClasses" tagprefix="dxw" %>

<%@ Register assembly="DevExpress.Web.ASPxGridView.v8.1.Export, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxNavBar" tagprefix="dxnb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 46%;
        }
    </style>
    </head>
<body>
    <form id="form1" runat="server">
        <div style="text-align:center;">
    <table style="width: 100%; text-align:center;" bgcolor="#004080" >
        <tr bgcolor="#DBE8E8">
            <td align="center">
                            <table style="width:300px">
                                <tr>
                                    <td class="style1" >
                                        Application Name :</td>
                                    <td align="left" colspan="3">
                                        <dxe:ASPxComboBox ID="cmbAppName" runat="server" ValueType="System.String">
                                            <Items>
                                                <dxe:ListEditItem Text="FeedDtac" Value="FeedDtac" />
                                                <dxe:ListEditItem Text="FeedAis" Value="FeedAis" />
                                                <dxe:ListEditItem Text="MobileLifeStyle" Value="MobileLifeStyle" />
                                                <dxe:ListEditItem Text="SportArena" Value="SportArena" />
                                                <dxe:ListEditItem Text="StarSoccer3GX" Value="StarSoccer3GX" />
                                                <dxe:ListEditItem Text="StarSoccer" Value="StarSoccer" />
                                            </Items>
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1" >
                                        &nbsp;</td>
                                    <td style="width:40%;">
                                        &nbsp;</td>
                                    <td  align="left" colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="style1" >
                                        วันที่ :</td>
                                    <td style="width:40%;">
                        <dxe:ASPxDateEdit ID="dateStart" runat="server">
                    </dxe:ASPxDateEdit> 
                                    </td>
                                    <td  align="center" style="width:20%;">
                                        ถึง</td>
                                    <td style="width:40%;">
                        <dxe:ASPxDateEdit ID="dateEnd" runat="server">
                    </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="style1" >
                                        &nbsp;</td>
                                    <td align="center" colspan="3">
                        <dxe:ASPxButton ID="btnSubmit" runat="server" Text="Submit" 
                        onclick="btnSubmit_Click">
                    </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
            </td>
        </tr>
        <tr bgcolor="#DBE8E8">
            <td align="center">
                            <dxtc:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="3" 
                                Width="100%" CssFilePath="~/App_Themes/Plastic Blue/{0}/styles.css" 
                                CssPostfix="PlasticBlue" ImageFolder="~/App_Themes/Plastic Blue/{0}/">
                                <TabPages>
                                    <dxtc:TabPage Name="Active" Text="Active" Visible="false">
                                        <ContentCollection>
                                            <dxw:ContentControl runat="server">
                                                <asp:ImageButton ID="btnReportExport" runat="server" ImageUrl="~/btnExcel.gif" 
                                                    OnClick="btnReportExport_Click" />
                                                <dxwgv:ASPxGridViewExporter ID="gvReportExport" runat="server" 
                                                    GridViewID="gvReport">
                                                </dxwgv:ASPxGridViewExporter>
                                                
                                                <dxnb:ASPxNavBar ID="aspBar" runat="server" Width="100%" 
                                                    CssFilePath="~/App_Themes/Soft Orange/{0}/styles.css" CssPostfix="Soft_Orange" 
                                                    GroupSpacing="0px" ImageFolder="~/App_Themes/Soft Orange/{0}/" 
                                                    LoadingPanelText="">
                                                    <Groups>
                                                        <dxnb:NavBarGroup Expanded="False" Name="iChart" Text="กราฟ">
                                                            <ContentTemplate>
                                                                <asp:Chart ID="Chart" runat="server" BackColor="211, 223, 240" 
                                                                    BackGradientStyle="TopBottom" BackSecondaryColor="White" BorderColor="#1A3B69" 
                                                                    BorderDashStyle="Solid" BorderWidth="2px" Height="600px" 
                                                                    ImageLocation="~\TempImages\ChartPic_#SEQ(300,3)" SuppressExceptions="True" 
                                                                    ToolTip="DATE_REPORT" ViewStateContent="All" Width="1024px" 
                                                                    AntiAliasing="None" BackImageAlignment="Center" BackImageWrapMode="Scaled" 
                                                                    BorderlineWidth="3" Compression="100" RightToLeft="Yes">
                                                                    <Series>
                                                                        <asp:Series ChartArea="caAndroid" Legend="Default" Name="ssAndroid">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caAndroidTab" Legend="Default" Name="ssAndroidTab">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caBB" Legend="Default" Name="ssBB">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caiPhone" Legend="Default" Name="ssiPhone">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caiPad" Legend="Default" Name="ssiPad">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caJava" Legend="Default" Name="ssJava">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <MapAreas>
                                                                        <asp:MapArea Coordinates="0,0,0,0" />
                                                                        <asp:MapArea Coordinates="0,0,0,0" />
                                                                        <asp:MapArea Coordinates="0,0,0,0" />
                                                                    </MapAreas>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            Name="caAndroid" ShadowColor="Transparent">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#,k" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <axisy2 titlefont="Trebuchet MS, 8.25pt" enabled="True" title="Android">
										<labelstyle enabled="False" />
										<majorgrid enabled="False" />
										<majortickmark enabled="False" />
									</axisy2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            Name="caAndroidTab" ShadowColor="Transparent" 
                                                                            IsSameFontSizeForAllAxes="True">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <axisy2 titlefont="Trebuchet MS, 8.25pt" enabled="True" title="Android Tablet">
										<labelstyle enabled="False" />
										<majorgrid enabled="False" />
										<majortickmark enabled="False" />
									</axisy2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            Name="caBB" ShadowColor="Transparent" IsSameFontSizeForAllAxes="True">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#,k" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <axisy2 titlefont="Trebuchet MS, 8.25pt" enabled="True" title="BB">
										<labelstyle enabled="False" />
										<majorgrid enabled="False" />
										<majortickmark enabled="False" />
									</axisy2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            Name="caiPhone" ShadowColor="Transparent" IsSameFontSizeForAllAxes="True">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#,k" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <axisy2 titlefont="Trebuchet MS, 8.25pt" enabled="True" title="iPhone">
										<labelstyle enabled="False" />
										<majorgrid enabled="False" />
										<majortickmark enabled="False" />
									</axisy2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            Name="caiPad" ShadowColor="Transparent" IsSameFontSizeForAllAxes="True">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#,k" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False"></AxisX2>
                                                                            <axisy2 titlefont="Trebuchet MS, 8.25pt" enabled="True" title="iPad">
										<labelstyle enabled="False" />
										<majorgrid enabled="False" />
										<majortickmark enabled="False" />
									</axisy2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            Name="caJava" ShadowColor="Transparent" IsSameFontSizeForAllAxes="True">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#,k" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <axisy2 titlefont="Trebuchet MS, 8.25pt" enabled="True" title="Java">
										<labelstyle enabled="False" />
										<majorgrid enabled="False" />
										<majortickmark enabled="False" />
									</axisy2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                    <Legends>
                                                                        <asp:Legend BackColor="Transparent" 
                                                                            Font="Trebuchet MS, 8.25pt, style=Bold" IsTextAutoFit="False" 
                                                                            Name="Default" BackImageAlignment="Bottom">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                    <Titles>
                                                                        <asp:Title Font="Trebuchet MS, 14.25pt, style=Bold" ForeColor="26, 59, 105" 
                                                                            Name="Title1" ShadowColor="32, 0, 0, 0" ShadowOffset="3" 
                                                                            Text="Application Report">
                                                                        </asp:Title>
                                                                    </Titles>
                                                                    <BorderSkin SkinStyle="Emboss" />
                                                                </asp:Chart>
                                                            </ContentTemplate>
                                                        </dxnb:NavBarGroup>
                                                        <dxnb:NavBarGroup Name="iData" Text="ข้อมูล">
                                                            <ContentTemplate>
                                                                <dxwgv:ASPxGridView ID="gvReport" runat="server" AutoGenerateColumns="False" 
                                                                    CssFilePath="~/App_Themes/Plastic Blue/{0}/styles.css" CssPostfix="PlasticBlue" 
                                                                    OnPageIndexChanged="gvReport_PageIndexChanged" Width="100%">
                                                                    <TotalSummary>
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="ANDROID_ACTIVE" 
                                                                            ShowInColumn="Android" ShowInGroupFooterColumn="Android" SummaryType="Sum" 
                                                                            Tag="SUM" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="ANDROIDTABLET_ACTIVE" 
                                                                            ShowInColumn="Android Tablet" SummaryType="Sum" Tag="Sum" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="BB_ACTIVE" 
                                                                            ShowInColumn="BB" SummaryType="Sum" Tag="Sum" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="IPHONE_ACTIVE" 
                                                                            ShowInColumn="iPhone" SummaryType="Sum" Tag="Sum" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="IPAD_ACTIVE" 
                                                                            ShowInColumn="iPad" SummaryType="Sum" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="JAVA_ACTIVE" 
                                                                            ShowInColumn="Java" ShowInGroupFooterColumn="Java" SummaryType="Sum" />
                                                                    </TotalSummary>
                                                                    <Columns>
                                                                        <dxwgv:GridViewDataTextColumn Caption="Date" FieldName="DATE_REPORT" 
                                                                            VisibleIndex="0">
                                                                            <PropertiesTextEdit DisplayFormatString="d">
                                                                            </PropertiesTextEdit>
                                                                            <FooterTemplate>
                                                                                Sum =
                                                                            </FooterTemplate>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="Android" FieldName="ANDROID_ACTIVE" 
                                                                            Name="Android" VisibleIndex="1">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#FFCCFF">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="Android Tablet" 
                                                                            FieldName="ANDROIDTABLET_ACTIVE" VisibleIndex="2">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#FFCCFF">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="BB" FieldName="BB_ACTIVE" 
                                                                            VisibleIndex="3">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#CCFF99">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="iPhone" FieldName="IPHONE_ACTIVE" 
                                                                            VisibleIndex="4">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#E0E0E0">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="iPad" FieldName="IPAD_ACTIVE" 
                                                                            VisibleIndex="5">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#E0E0E0">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="Java" FieldName="JAVA_ACTIVE" 
                                                                            VisibleIndex="6">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#FFFFCC">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                    </Columns>
                                                                    <SettingsPager PageSize="31" ShowDefaultImages="False">
                                                                        <AllButton Text="All">
                                                                        </AllButton>
                                                                        <NextPageButton Text="Next &gt;">
                                                                        </NextPageButton>
                                                                        <PrevPageButton Text="&lt; Prev">
                                                                        </PrevPageButton>
                                                                    </SettingsPager>
                                                                    <Settings ShowFooter="True" />
                                                                    <Images ImageFolder="~/App_Themes/Plastic Blue/{0}/">
                                                                        <CollapsedButton Height="10px" 
                                                                            Url="~/App_Themes/Plastic Blue/GridView/gvCollapsedButton.png" Width="9px" />
                                                                        <ExpandedButton Height="9px" 
                                                                            Url="~/App_Themes/Plastic Blue/GridView/gvExpandedButton.png" Width="9px" />
                                                                        <HeaderFilter Height="15px" 
                                                                            Url="~/App_Themes/Plastic Blue/GridView/gvHeaderFilter.png" Width="13px" />
                                                                        <HeaderActiveFilter Height="15px" 
                                                                            Url="~/App_Themes/Plastic Blue/GridView/gvHeaderFilterActive.png" 
                                                                            Width="13px" />
                                                                        <HeaderSortDown Height="11px" 
                                                                            Url="~/App_Themes/Plastic Blue/GridView/gvHeaderSortDown.png" Width="11px" />
                                                                        <HeaderSortUp Height="11px" 
                                                                            Url="~/App_Themes/Plastic Blue/GridView/gvHeaderSortUp.png" Width="11px" />
                                                                    </Images>
                                                                    <Styles CssFilePath="~/App_Themes/Plastic Blue/{0}/styles.css" 
                                                                        CssPostfix="PlasticBlue">
                                                                        <Header ImageSpacing="10px" SortingImageSpacing="10px">
                                                                        </Header>
                                                                        <Footer Font-Bold="True" ForeColor="Black">
                                                                        </Footer>
                                                                    </Styles>
                                                                </dxwgv:ASPxGridView>
                                                            </ContentTemplate>
                                                        </dxnb:NavBarGroup>
                                                    </Groups>
                                                    <CollapseImage Height="18px" Width="18px" />
                                                    <ExpandImage Height="18px" Width="18px" />
                                                </dxnb:ASPxNavBar>
                                                &nbsp;
                                            </dxw:ContentControl>
                                        </ContentCollection>
                                    </dxtc:TabPage>
                                    <dxtc:TabPage Name="Active Notification IMSI IMEI" Text="Active Notification IMSI IMEI">
                                        <ContentCollection>
                                            <dxw:ContentControl runat="server">
                                                <asp:ImageButton ID="btnNotiExport" runat="server" ImageUrl="~/btnExcel.gif" 
                                                    OnClick="btnNotiExport_Click" />
                                                <dxwgv:ASPxGridViewExporter ID="gvNotiExport" runat="server" 
                                                    GridViewID="gvReport">
                                                </dxwgv:ASPxGridViewExporter>
                                                <dxwgv:ASPxGridView ID="gvNoti" runat="server" AutoGenerateColumns="False" 
                                                    OnPageIndexChanged="gvNoti_PageIndexChanged">
                                                    <TotalSummary>
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="ANDROID_ACTIVE_NOTIFY" 
                                                            ShowInColumn="Android" ShowInGroupFooterColumn="Android" SummaryType="Sum" 
                                                            Tag="SUM" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="ANDROID_IMSI" 
                                                            ShowInColumn="Android IMSI" SummaryType="Sum" Tag="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="ANDROID_IMEI" 
                                                            ShowInColumn="Android IMEI" SummaryType="Sum" Tag="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="IPHONE_ACTIVE" 
                                                            ShowInColumn="iPhone" SummaryType="Sum" Tag="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="IPAD_ACTIVE" 
                                                            ShowInColumn="iPad" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="JAVA_ACTIVE" 
                                                            ShowInColumn="Java" ShowInGroupFooterColumn="Java" SummaryType="Sum" />
                                                    </TotalSummary>
                                                    <Columns>
                                                        <dxwgv:GridViewDataTextColumn Caption="Date" FieldName="DATE_REPORT" 
                                                            VisibleIndex="0">
                                                            <PropertiesTextEdit DisplayFormatString="d">
                                                            </PropertiesTextEdit>
                                                            <FooterTemplate>
                                                                Sum =
                                                            </FooterTemplate>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Android Notify" FieldName="ANDROID_ACTIVE_NOTIFY" 
                                                            Name="Android" VisibleIndex="1">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#FFCCFF">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Android IMSI" 
                                                            FieldName="ANDROID_IMSI" VisibleIndex="2">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#FFCCFF">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Android IMEI" FieldName="ANDROID_IMEI" 
                                                            VisibleIndex="3">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#CCFF99">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="iPhone" FieldName="IPHONE_ACTIVE" 
                                                            VisibleIndex="4">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#E0E0E0">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="iPad" FieldName="IPAD_ACTIVE" 
                                                            VisibleIndex="5">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#E0E0E0">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Java" FieldName="JAVA_ACTIVE" 
                                                            VisibleIndex="6">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#FFFFCC">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsPager PageSize="31">
                                                        <AllButton Text="All">
                                                        </AllButton>
                                                        <NextPageButton Text="Next &gt;">
                                                        </NextPageButton>
                                                        <PrevPageButton Text="&lt; Prev">
                                                        </PrevPageButton>
                                                    </SettingsPager>
                                                    <Settings ShowFooter="True" />
                                                </dxwgv:ASPxGridView>
                                            </dxw:ContentControl>
                                        </ContentCollection>
                                    </dxtc:TabPage>
                                    <dxtc:TabPage Name="Unique" Text="Unique">
                                        <ContentCollection>
                                            <dxw:ContentControl runat="server">
                                                <asp:ImageButton ID="btnUniqueExport" runat="server" ImageUrl="~/btnExcel.gif" 
                                                    OnClick="btnUniqueExport_Click" />
                                                <dxnb:ASPxNavBar ID="barUnique" runat="server" Width="100%" 
                                                    CssFilePath="~/App_Themes/Soft Orange/{0}/styles.css" CssPostfix="Soft_Orange" 
                                                    GroupSpacing="0px" ImageFolder="~/App_Themes/Soft Orange/{0}/" 
                                                    LoadingPanelText="">
                                                    <Groups>
                                                        <dxnb:NavBarGroup Expanded="False">
                                                            <ContentTemplate>
                                                                <asp:Chart ID="chartUnique" runat="server" AntiAliasing="None" 
                                                                    BackColor="211, 223, 240" BackGradientStyle="TopBottom" 
                                                                    BackImageAlignment="Center" BackImageWrapMode="Scaled" 
                                                                    BackSecondaryColor="White" BorderColor="#1A3B69" BorderDashStyle="Solid" 
                                                                    BorderlineWidth="3" BorderWidth="2px" Compression="100" Height="600px" 
                                                                    ImageLocation="~\TempImages\ChartPic_#SEQ(300,3)" RightToLeft="Yes" 
                                                                    SuppressExceptions="True" ViewStateContent="All" Width="1024px">
                                                                    <Series>
                                                                        <asp:Series ChartArea="caAndroid" Legend="Default" Name="ssAndroid">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caAndroidTab" Legend="Default" Name="ssAndroidTab">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caBB" Legend="Default" Name="ssBB">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caiPhone" Legend="Default" Name="ssiPhone">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caiPad" Legend="Default" Name="ssiPad">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caJava" Legend="Default" Name="ssJava">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <MapAreas>
                                                                        <asp:MapArea Coordinates="0,0,0,0" />
                                                                        <asp:MapArea Coordinates="0,0,0,0" />
                                                                        <asp:MapArea Coordinates="0,0,0,0" />
                                                                    </MapAreas>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            Name="caAndroid" ShadowColor="Transparent">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <AxisY2 Enabled="True" Title="Android">
                                                                                <MajorGrid Enabled="False" />
                                                                                <MajorTickMark Enabled="False" />
                                                                                <LabelStyle Enabled="False" />
                                                                            </AxisY2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            IsSameFontSizeForAllAxes="True" Name="caAndroidTab" ShadowColor="Transparent">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <AxisY2 Enabled="True" Title="Android Tablet">
                                                                                <MajorGrid Enabled="False" />
                                                                                <MajorTickMark Enabled="False" />
                                                                                <LabelStyle Enabled="False" />
                                                                            </AxisY2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            IsSameFontSizeForAllAxes="True" Name="caBB" ShadowColor="Transparent">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <AxisY2 Enabled="True" Title="BB">
                                                                                <MajorGrid Enabled="False" />
                                                                                <MajorTickMark Enabled="False" />
                                                                                <LabelStyle Enabled="False" />
                                                                            </AxisY2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            IsSameFontSizeForAllAxes="True" Name="caiPhone" ShadowColor="Transparent">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <AxisY2 Enabled="True" Title="iPhone">
                                                                                <MajorGrid Enabled="False" />
                                                                                <MajorTickMark Enabled="False" />
                                                                                <LabelStyle Enabled="False" />
                                                                            </AxisY2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            IsSameFontSizeForAllAxes="True" Name="caiPad" ShadowColor="Transparent">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <AxisY2 Enabled="True" Title="iPad">
                                                                                <MajorGrid Enabled="False" />
                                                                                <MajorTickMark Enabled="False" />
                                                                                <LabelStyle Enabled="False" />
                                                                            </AxisY2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            IsSameFontSizeForAllAxes="True" Name="caJava" ShadowColor="Transparent">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <AxisY2 Enabled="True" Title="Java">
                                                                                <MajorGrid Enabled="False" />
                                                                                <MajorTickMark Enabled="False" />
                                                                                <LabelStyle Enabled="False" />
                                                                            </AxisY2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                    <Legends>
                                                                        <asp:Legend BackColor="Transparent" BackImageAlignment="Bottom" 
                                                                            Font="Trebuchet MS, 8.25pt, style=Bold" IsTextAutoFit="False" Name="Default">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                    <Titles>
                                                                        <asp:Title Font="Trebuchet MS, 14.25pt, style=Bold" ForeColor="26, 59, 105" 
                                                                            Name="Title1" ShadowColor="32, 0, 0, 0" ShadowOffset="3" 
                                                                            Text="Application Report">
                                                                        </asp:Title>
                                                                    </Titles>
                                                                    <BorderSkin SkinStyle="Emboss" />
                                                                </asp:Chart>
                                                            </ContentTemplate>
                                                        </dxnb:NavBarGroup>
                                                        <dxnb:NavBarGroup>
                                                            <ContentTemplate>
                                                                <dxwgv:ASPxGridView ID="gvUnique" runat="server" AutoGenerateColumns="False" 
                                                                    OnPageIndexChanged="gvUnique_PageIndexChanged">
                                                                    <TotalSummary>
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="ANDROID_UNIQUE" 
                                                                            SummaryType="Sum" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="ANDROIDTABLET_UNIQUE" 
                                                                            SummaryType="Sum" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="BB_UNIQUE" 
                                                                            ShowInColumn="BB" ShowInGroupFooterColumn="BB" SummaryType="Sum" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="IPHONE_UNIQUE" 
                                                                            ShowInColumn="iPhone" ShowInGroupFooterColumn="iPhone" SummaryType="Sum" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="IPAD_UNIQUE" 
                                                                            ShowInColumn="iPad" ShowInGroupFooterColumn="iPad" SummaryType="Sum" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="JAVA_UNIQUE" 
                                                                            ShowInColumn="Java" ShowInGroupFooterColumn="Java" SummaryType="Sum" />
                                                                    </TotalSummary>
                                                                    <Columns>
                                                                        <dxwgv:GridViewDataTextColumn Caption="Date" FieldName="DATE_REPORT" 
                                                                            VisibleIndex="0">
                                                                            <PropertiesTextEdit DisplayFormatString="d">
                                                                            </PropertiesTextEdit>
                                                                            <FooterTemplate>
                                                                                Sum =
                                                                            </FooterTemplate>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="Android" FieldName="ANDROID_UNIQUE" 
                                                                            Name="Android" VisibleIndex="1">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#FFCCFF">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="Android Tablet" 
                                                                            FieldName="ANDROIDTABLET_UNIQUE" VisibleIndex="2">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#FFCCFF">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="BB" FieldName="BB_UNIQUE" 
                                                                            VisibleIndex="3">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#CCFF99">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="iPhone" FieldName="IPHONE_UNIQUE" 
                                                                            VisibleIndex="4">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#E0E0E0">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="iPad" FieldName="IPAD_UNIQUE" 
                                                                            VisibleIndex="5">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#E0E0E0">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="Java" FieldName="JAVA_UNIQUE" 
                                                                            VisibleIndex="6">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#FFFFCC">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                    </Columns>
                                                                    <SettingsPager PageSize="31">
                                                                        <AllButton Text="All">
                                                                        </AllButton>
                                                                        <NextPageButton Text="Next &gt;">
                                                                        </NextPageButton>
                                                                        <PrevPageButton Text="&lt; Prev">
                                                                        </PrevPageButton>
                                                                    </SettingsPager>
                                                                    <Settings ShowFooter="True" />
                                                                </dxwgv:ASPxGridView>
                                                            </ContentTemplate>
                                                        </dxnb:NavBarGroup>
                                                    </Groups>
                                                    <CollapseImage Height="18px" Width="18px" />
                                                    <ExpandImage Height="18px" Width="18px" />
                                                </dxnb:ASPxNavBar>
                                                <dxwgv:ASPxGridViewExporter ID="gvUniqueExport" runat="server" 
                                                    GridViewID="gvUnique">
                                                </dxwgv:ASPxGridViewExporter>
                                            </dxw:ContentControl>
                                        </ContentCollection>
                                    </dxtc:TabPage>
                                    <dxtc:TabPage Name="View" Text="View">
                                        <ContentCollection>
                                            <dxw:ContentControl runat="server">
                                                <asp:ImageButton ID="btnViewExport" runat="server" ImageUrl="~/btnExcel.gif" 
                                                    OnClick="btnViewExport_Click" />
                                                <dxnb:ASPxNavBar ID="barView" runat="server" 
                                                    CssFilePath="~/App_Themes/Soft Orange/{0}/styles.css" CssPostfix="Soft_Orange" 
                                                    GroupSpacing="0px" ImageFolder="~/App_Themes/Soft Orange/{0}/" 
                                                    LoadingPanelText="" Width="100%">
                                                    <Groups>
                                                        <dxnb:NavBarGroup Expanded="False">
                                                            <ContentTemplate>
                                                                <asp:Chart ID="chartView" runat="server" AntiAliasing="None" 
                                                                    BackColor="211, 223, 240" BackGradientStyle="TopBottom" 
                                                                    BackImageAlignment="Center" BackImageWrapMode="Scaled" 
                                                                    BackSecondaryColor="White" BorderColor="#1A3B69" BorderDashStyle="Solid" 
                                                                    BorderlineWidth="3" BorderWidth="2px" Compression="100" Height="600px" 
                                                                    ImageLocation="~\TempImages\ChartPic_#SEQ(300,3)" RightToLeft="Yes" 
                                                                    SuppressExceptions="True" ViewStateContent="All" Width="1024px">
                                                                    <Series>
                                                                        <asp:Series ChartArea="caAndroid" Legend="Default" Name="ssAndroid">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caAndroidTab" Legend="Default" Name="ssAndroidTab">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caBB" Legend="Default" Name="ssBB">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caiPhone" Legend="Default" Name="ssiPhone">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caiPad" Legend="Default" Name="ssiPad">
                                                                        </asp:Series>
                                                                        <asp:Series ChartArea="caJava" Legend="Default" Name="ssJava">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <MapAreas>
                                                                        <asp:MapArea Coordinates="0,0,0,0" />
                                                                        <asp:MapArea Coordinates="0,0,0,0" />
                                                                        <asp:MapArea Coordinates="0,0,0,0" />
                                                                    </MapAreas>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            Name="caAndroid" ShadowColor="Transparent">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <AxisY2 Enabled="True" Title="Android">
                                                                                <MajorGrid Enabled="False" />
                                                                                <MajorTickMark Enabled="False" />
                                                                                <LabelStyle Enabled="False" />
                                                                            </AxisY2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            IsSameFontSizeForAllAxes="True" Name="caAndroidTab" ShadowColor="Transparent">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <AxisY2 Enabled="True" Title="Android Tablet">
                                                                                <MajorGrid Enabled="False" />
                                                                                <MajorTickMark Enabled="False" />
                                                                                <LabelStyle Enabled="False" />
                                                                            </AxisY2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            IsSameFontSizeForAllAxes="True" Name="caBB" ShadowColor="Transparent">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <AxisY2 Enabled="True" Title="BB">
                                                                                <MajorGrid Enabled="False" />
                                                                                <MajorTickMark Enabled="False" />
                                                                                <LabelStyle Enabled="False" />
                                                                            </AxisY2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            IsSameFontSizeForAllAxes="True" Name="caiPhone" ShadowColor="Transparent">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <AxisY2 Enabled="True" Title="iPhone">
                                                                                <MajorGrid Enabled="False" />
                                                                                <MajorTickMark Enabled="False" />
                                                                                <LabelStyle Enabled="False" />
                                                                            </AxisY2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            IsSameFontSizeForAllAxes="True" Name="caiPad" ShadowColor="Transparent">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <AxisY2 Enabled="True" Title="iPad">
                                                                                <MajorGrid Enabled="False" />
                                                                                <MajorTickMark Enabled="False" />
                                                                                <LabelStyle Enabled="False" />
                                                                            </AxisY2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                        <asp:ChartArea BackColor="64, 165, 191, 228" BackGradientStyle="TopBottom" 
                                                                            BackSecondaryColor="White" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" 
                                                                            IsSameFontSizeForAllAxes="True" Name="caJava" ShadowColor="Transparent">
                                                                            <AxisY ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="#" />
                                                                            </AxisY>
                                                                            <AxisX ArrowStyle="Triangle" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsStaggered="True" />
                                                                            </AxisX>
                                                                            <AxisX2 Enabled="False">
                                                                            </AxisX2>
                                                                            <AxisY2 Enabled="True" Title="Java">
                                                                                <MajorGrid Enabled="False" />
                                                                                <MajorTickMark Enabled="False" />
                                                                                <LabelStyle Enabled="False" />
                                                                            </AxisY2>
                                                                            <Area3DStyle Inclination="15" IsRightAngleAxes="False" Perspective="10" 
                                                                                Rotation="10" WallWidth="0" />
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                    <Legends>
                                                                        <asp:Legend BackColor="Transparent" BackImageAlignment="Bottom" 
                                                                            Font="Trebuchet MS, 8.25pt, style=Bold" IsTextAutoFit="False" Name="Default">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                    <Titles>
                                                                        <asp:Title Font="Trebuchet MS, 14.25pt, style=Bold" ForeColor="26, 59, 105" 
                                                                            Name="Title1" ShadowColor="32, 0, 0, 0" ShadowOffset="3" 
                                                                            Text="Application Report">
                                                                        </asp:Title>
                                                                    </Titles>
                                                                    <BorderSkin SkinStyle="Emboss" />
                                                                </asp:Chart>
                                                            </ContentTemplate>
                                                        </dxnb:NavBarGroup>
                                                        <dxnb:NavBarGroup>
                                                            <ContentTemplate>
                                                                <dxwgv:ASPxGridView ID="gvView" runat="server" AutoGenerateColumns="False" 
                                                                    OnPageIndexChanged="gvView_PageIndexChanged">
                                                                    <TotalSummary>
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="ANDROID_VIEW" 
                                                                            ShowInColumn="Android" ShowInGroupFooterColumn="Android" SummaryType="Sum" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="ANDROIDTABLET_VIEW" 
                                                                            ShowInColumn="Android Tablet" ShowInGroupFooterColumn="Android Tablet" 
                                                                            SummaryType="Sum" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="BB_VIEW" ShowInColumn="BB" 
                                                                            ShowInGroupFooterColumn="BB" SummaryType="Sum" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="IPHONE_VIEW" 
                                                                            ShowInColumn="iPhone" ShowInGroupFooterColumn="iPhone" SummaryType="Sum" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="IPAD_VIEW" 
                                                                            ShowInColumn="iPad" ShowInGroupFooterColumn="iPad" SummaryType="Sum" />
                                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="JAVA_VIEW" 
                                                                            ShowInColumn="Java" SummaryType="Sum" />
                                                                    </TotalSummary>
                                                                    <Columns>
                                                                        <dxwgv:GridViewDataTextColumn Caption="Date" FieldName="DATE_REPORT" 
                                                                            VisibleIndex="0">
                                                                            <PropertiesTextEdit DisplayFormatString="d">
                                                                            </PropertiesTextEdit>
                                                                            <FooterTemplate>
                                                                                Sum =
                                                                            </FooterTemplate>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="Android" FieldName="ANDROID_VIEW" 
                                                                            Name="Android" VisibleIndex="1">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#FFCCFF">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="Android Tablet" 
                                                                            FieldName="ANDROIDTABLET_VIEW" VisibleIndex="2">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#FFCCFF">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="BB" FieldName="BB_VIEW" VisibleIndex="3">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#CCFF99">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="iPhone" FieldName="IPHONE_VIEW" 
                                                                            VisibleIndex="4">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#E0E0E0">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="iPad" FieldName="IPAD_VIEW" 
                                                                            VisibleIndex="5">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#E0E0E0">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                        <dxwgv:GridViewDataTextColumn Caption="Java" FieldName="JAVA_VIEW" 
                                                                            VisibleIndex="6">
                                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                                            </PropertiesTextEdit>
                                                                            <CellStyle BackColor="#FFFFCC">
                                                                            </CellStyle>
                                                                        </dxwgv:GridViewDataTextColumn>
                                                                    </Columns>
                                                                    <SettingsPager PageSize="31">
                                                                        <AllButton Text="All">
                                                                        </AllButton>
                                                                        <NextPageButton Text="Next &gt;">
                                                                        </NextPageButton>
                                                                        <PrevPageButton Text="&lt; Prev">
                                                                        </PrevPageButton>
                                                                    </SettingsPager>
                                                                    <Settings ShowFooter="True" />
                                                                </dxwgv:ASPxGridView>
                                                            </ContentTemplate>
                                                        </dxnb:NavBarGroup>
                                                    </Groups>
                                                    <CollapseImage Height="18px" Width="18px" />
                                                    <ExpandImage Height="18px" Width="18px" />
                                                </dxnb:ASPxNavBar>
                                                <dxwgv:ASPxGridViewExporter ID="gvViewExport" runat="server" 
                                                    GridViewID="gvView">
                                                </dxwgv:ASPxGridViewExporter>
                                            </dxw:ContentControl>
                                        </ContentCollection>
                                    </dxtc:TabPage>
                                    <dxtc:TabPage Name="View By Page" Text="View By Page">
                                        <ContentCollection>
                                            <dxw:ContentControl runat="server">
                                            <asp:ImageButton ID="btnViewPage" runat="server" ImageUrl="~/btnExcel.gif" 
                                                    OnClick="btnViewPage_Click" />
                                            <dxwgv:ASPxGridViewExporter ID="gvViewPageExport" runat="server" 
                                                    GridViewID="gvViewPage">
                                                </dxwgv:ASPxGridViewExporter>
                                                <dxwgv:ASPxGridView ID="gvViewPage" runat="server" AutoGenerateColumns="False">
                                                    <TotalSummary>
                                                        <dxwgv:ASPxSummaryItem  FieldName="SUMVIEW" ShowInColumn="SUMVIEW"  SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem  FieldName="PAGE_NAME" ShowInColumn="PAGE_NAME" SummaryType="Count"  />
                                                    </TotalSummary>
                                                    <GroupSummary  >
                                                            <dxwgv:ASPxSummaryItem  FieldName="SUMVIEW" SummaryType="Sum"  ShowInGroupFooterColumn="SUMVIEW"   />
                                                            <dxwgv:ASPxSummaryItem  FieldName="PAGE_NAME" SummaryType="Count" ShowInGroupFooterColumn="PAGE_NAME"  />
                                                        </GroupSummary>
                                                    <Columns>
                                                        <dxwgv:GridViewDataTextColumn Caption="Model" FieldName="MODEL_TYPE" 
                                                            GroupIndex="0" Name="MODEL_TYPE" SortIndex="0" SortOrder="Ascending" 
                                                            VisibleIndex="0">
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="PAGE" FieldName="PAGE_NAME" 
                                                            Name="PAGE_NAME" VisibleIndex="1">
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="SUM" FieldName="SUMVIEW" Name="SUMVIEW" CellStyle-BackColor="#FFCCFF" UnboundType="Decimal"
                                                            VisibleIndex="2">
                                                            <PropertiesTextEdit DisplayFormatString="n"></PropertiesTextEdit>

<CellStyle BackColor="#FFCCFF"></CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsPager PageSize="40">
                                                        <AllButton Text="All">
                                                        </AllButton>
                                                        <NextPageButton Text="Next &gt;">
                                                        </NextPageButton>
                                                        <PrevPageButton Text="&lt; Prev">
                                                        </PrevPageButton>
                                                    </SettingsPager>
                                                    <Settings ShowGroupedColumns="true" ShowGroupFooter="VisibleAlways" ShowFooter="true" ShowGroupButtons="true" 
                                                        ShowGroupPanel="True" />
                                                        
                                                </dxwgv:ASPxGridView>
                                            </dxw:ContentControl>
                                        </ContentCollection>
                                    </dxtc:TabPage>
                                </TabPages>
                                <ContentStyle>
                                    <Border BorderWidth="0px" />
                                </ContentStyle>
                                <LoadingPanelImage Url="~/App_Themes/Plastic Blue/Web/tcLoading.gif" />
                            </dxtc:ASPxPageControl>
            </td>
        </tr>
        </table>
    </div>
    </form>
</body>
</html>
