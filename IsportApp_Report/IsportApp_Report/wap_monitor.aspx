<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wap_monitor.aspx.cs" Inherits="IsportApp_Report.wap_monitor" %>
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
<head runat="server">
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
                        <dxe:ASPxDateEdit ID="dateStart" runat="server">
                    </dxe:ASPxDateEdit> 
                                    </td>
                                    <td  align="center" style="width:20%;">
                                        ถึง</td>
                                    <td style="width:40%;">
                                        &nbsp;</td>
                                </tr>
                                <tr>
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
                                            <dxw:ContentControl runat="server">
                                            </dxw:ContentControl>
                                        </ContentCollection>
                                    </dxtc:TabPage>
                                    <dxtc:TabPage Name="Unique" Text="Unique">
                                        <ContentCollection>
                                            <dxw:ContentControl runat="server">
                                            </dxw:ContentControl>
                                        </ContentCollection>
                                    </dxtc:TabPage>
                                    <dxtc:TabPage Name="View" Text="View">
                                        <ContentCollection>
                                            <dxw:ContentControl runat="server">
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
                                                <dxwgv:ASPxGridView ID="gvViewPage" runat="server" AutoGenerateColumns="False" 
                                                    CssFilePath="~/App_Themes/Plastic Blue/{0}/styles.css" CssPostfix="PlasticBlue" 
                                                    Width="400px" OnPageIndexChanged="gvViewPage_PageIndexChanged" 
                                                    OnCustomButtonCallback="gvViewPage_CustomButtonCallback" 
                                                    OnCustomCallback="gvViewPage_CustomCallback">
                                                    <Styles CssFilePath="~/App_Themes/Plastic Blue/{0}/styles.css" 
                                                        CssPostfix="PlasticBlue">
                                                        <Header ImageSpacing="10px" SortingImageSpacing="10px">
                                                        </Header>
                                                        <GroupFooter BackColor="Silver"></GroupFooter>
                                                    </Styles>
                                                    <Columns>
                                                        <dxwgv:GridViewDataTextColumn Caption="PFILE_NAME" FieldName="PFILE_NAME" Name="PFILE_NAME" 
                                                            VisibleIndex="0">
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="COUNT" FieldName="COUNT" 
                                                            Name="COUNT" VisibleIndex="1" UnboundType="Decimal">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#FFCCFF">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                    </Columns>
                                                    
                                                    <SettingsPager ShowDefaultImages="False" PageSize="40">
                                                        <AllButton Text="All">
                                                        </AllButton>
                                                        <NextPageButton Text="Next &gt;">
                                                        </NextPageButton>
                                                        <PrevPageButton Text="&lt; Prev">
                                                        </PrevPageButton>
                                                    </SettingsPager>
                                                    <Settings ShowGroupedColumns="true" ShowGroupFooter="VisibleAlways" ShowFooter="true" ShowGroupButtons="false" 
                                                        ShowGroupPanel="false" />
                                                        
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
