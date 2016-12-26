<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wap_report.aspx.cs" Inherits="IsportApp_Report.wap_report" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>


<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<%@ Register assembly="DevExpress.Web.ASPxScheduler.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxScheduler" tagprefix="dxwschs" %>

<%@ Register assembly="DevExpress.Web.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxTabControl" tagprefix="dxtc" %>
<%@ Register assembly="DevExpress.Web.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxClasses" tagprefix="dxw" %>

<%@ Register assembly="DevExpress.Web.ASPxGridView.v8.1.Export, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dxwgv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    </head>
<body>
    <form id="form1" runat="server">
        <div style="text-align:center;">
            
    <table style="width: 100%; text-align:center;" bgcolor="#004080" >
        <tr bgcolor="#DBE8E8">
            <td align="center">
                            <table style="width:400px">
                                <tr>
                                    <td style="width:40%;">
                                        serviceGroup:</td>
                                    <td align="left" colspan="3">
                                        <asp:DropDownList ID="ddlSg" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:40%;">
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
                                    <td align="center">
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
                            <dxtc:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
                                Width="100%" CssFilePath="~/App_Themes/Plastic Blue/{0}/styles.css" 
                                CssPostfix="PlasticBlue" ImageFolder="~/App_Themes/Plastic Blue/{0}/">
                                <TabPages>
                                    <dxtc:TabPage Name="Active" Text="Active">
                                        <ContentCollection>
                                            <dxw:ContentControl ID="ContentControl1" runat="server">
                                                <asp:ImageButton ID="btnReportExport" runat="server" ImageUrl="~/btnExcel.gif" 
                                                    OnClick="btnReportExport_Click" />
                                                <dxwgv:ASPxGridViewExporter ID="gvReportExport" runat="server" 
                                                    GridViewID="gvReport"> 
                                                </dxwgv:ASPxGridViewExporter>
                                                
                                                
                                                <dxwgv:ASPxGridView ID="gvReport" runat="server" AutoGenerateColumns="False" 
                                                    CssFilePath="~/App_Themes/Plastic Blue/{0}/styles.css" CssPostfix="PlasticBlue" 
                                                    OnPageIndexChanged="gvReport_PageIndexChanged" Width="100%" 
                                                    OnHtmlRowPrepared="gvReport_HtmlRowPrepared">

                                                    <TotalSummary>
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="OPT01_VIEW" 
                                                            ShowInColumn="AIS View" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="OPT01_ANUMBER" 
                                                            ShowInColumn="AIS Unique" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="OPT02_VIEW" 
                                                            ShowInColumn="Dtac View" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="OPT02_ANUMBER" 
                                                            ShowInColumn="Dtac Unique" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="OPT03_VIEW" 
                                                            ShowInColumn="True View" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="OPT03_ANUMBER" 
                                                            ShowInColumn="True Unique" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="OPT04_VIEW" 
                                                            ShowInColumn="TrueH View" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="OPT04_ANUMBER" 
                                                            ShowInColumn="TrueH Unique" SummaryType="Sum" />
                                                    </TotalSummary>

                                                    <Columns>
                                                        <dxwgv:GridViewDataTextColumn Caption="Date" FieldName="REP_DATE" 
                                                            VisibleIndex="0">
                                                            <PropertiesTextEdit DisplayFormatString="d">
                                                            </PropertiesTextEdit>
                                                            <FooterTemplate>
                                                                Sum =
                                                            </FooterTemplate>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="AIS View" FieldName="OPT01_VIEW" 
                                                            VisibleIndex="1">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#FFCCFF">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="AIS Unique" FieldName="OPT01_ANUMBER" 
                                                            Name="ais anumber" VisibleIndex="2">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#FFCCFF">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Dtac View" FieldName="OPT02_VIEW" 
                                                            VisibleIndex="3">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#CCFF99">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Dtac Unique" FieldName="OPT02_ANUMBER" 
                                                            VisibleIndex="4">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#E0E0E0">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="True View" FieldName="OPT03_VIEW" 
                                                            VisibleIndex="5">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#E0E0E0">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="True Unique" FieldName="OPT03_ANUMBER" 
                                                            VisibleIndex="6">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#FFFFCC">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="TrueH View" FieldName="OPT04_VIEW" 
                                                            VisibleIndex="7">
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="TrueH Unique" FieldName="OPT04_ANUMBER" 
                                                            VisibleIndex="8">
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

