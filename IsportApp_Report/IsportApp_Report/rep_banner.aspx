<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rep_banner.aspx.cs" Inherits="IsportApp_Report.rep_banner" %>

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
            background-color: #3366FF;
            height: 35px;
        }
        .style2
        {
            height: 41px;
            color: #FFFFFF;
            font-weight: bold;
            background-color: #3366FF;
        }
        .style3
        {
            width: 262px;
            height: 35px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align:center;">
    <table style="width: 100%; text-align:center;" bgcolor="#004080" >
        <tr bgcolor="#DBE8E8">
            <td align="center">
                            <table style="width:500px">
                                <tr>
                                    <td class="style2" colspan="4" align="center" >
                                        Report Ads Banner</td>
                                </tr>
                                <tr>
                                    <td class="style3" align="right" >
                                        Application&nbsp;&nbsp;Name :</td>
                                    <td align="left" colspan="3" >
                                        <dxe:ASPxComboBox ID="cmbAppName" runat="server" ValueType="System.String" 
                                            AutoPostBack="True" onselectedindexchanged="cmbAppName_SelectedIndexChanged" 
                                            SelectedIndex="5">
                                            <Items>
                                                <dxe:ListEditItem Text="FeedDtac" Value="FeedDtac" />
                                                <dxe:ListEditItem Text="FeedAis" Value="FeedAis" />
                                                <dxe:ListEditItem Text="MobileLifeStyle" Value="MobileLifeStyle" />
                                                <dxe:ListEditItem Text="SportArena" Value="SportArena" />
                                                <dxe:ListEditItem Text="StarSoccer3GX" Value="StarSoccer3GX" />
                                                <dxe:ListEditItem Text="StarSoccer" Value="StarSoccer"  />
                                                <dxe:ListEditItem Text="SportPool" Value="SportPool" />
                                            </Items>
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style3" align="right" >
                                        วันที่ :</td>
                                    <td >
                        <dxe:ASPxDateEdit ID="dateStart" runat="server">
                    </dxe:ASPxDateEdit> 
                                    </td>
                                    <td  align="center" >
                                        ถึง</td>
                                    <td >
                        <dxe:ASPxDateEdit ID="dateEnd" runat="server">
                    </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="style1" colspan="4" >
                        <dxe:ASPxButton ID="btnSubmit" runat="server" Text="Submit" 
                        onclick="btnSubmit_Click">
                    </dxe:ASPxButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="style1" colspan="4" >
                                     <iframe id="frmBanner" runat="server" src="" width="480px" height="60px"></iframe>   
                                        
                                     </td>
                                </tr>
                            </table>
            </td>
        </tr>
        <tr bgcolor="#DBE8E8">
            <td align="center">
                            <dxtc:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="1" 
                                Width="100%" CssFilePath="~/App_Themes/Plastic Blue/{0}/styles.css" 
                                CssPostfix="PlasticBlue" ImageFolder="~/App_Themes/Plastic Blue/{0}/">
                                <TabPages>
                                    <dxtc:TabPage Name="Ads by Operator" Text="Ads by Operator">
                                        <ContentCollection>
                                            <dxw:ContentControl runat="server">
                                                <asp:ImageButton ID="btnOPT" runat="server" ImageUrl="~/btnExcel.gif" 
                                                    OnClick="btnOPT_Click" />
                                                <dxwgv:ASPxGridViewExporter ID="gvOptExport" runat="server" 
                                                    GridViewID="gvOperator">
                                                </dxwgv:ASPxGridViewExporter>
                                                <dxwgv:ASPxGridView ID="gvOperator" runat="server" AutoGenerateColumns="False" 
                                                    CssFilePath="~/App_Themes/Plastic Blue/{0}/styles.css" 
                                                    CssPostfix="PlasticBlue" Width="100%" 
                                                    OnHtmlRowPrepared="gvOperator_HtmlRowPrepared" >
                                                    <Styles CssFilePath="~/App_Themes/Plastic Blue/{0}/styles.css" 
                                                        CssPostfix="PlasticBlue">
                                                        <Header ImageSpacing="10px" SortingImageSpacing="10px">
                                                        </Header>
                                                        <Footer Font-Bold="True" ForeColor="Black">
                                                        </Footer>
                                                    </Styles>
                                                    <TotalSummary>
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_01" 
                                                            ShowInColumn="Ais View" ShowInGroupFooterColumn="Ais View" SummaryType="Sum" 
                                                            Tag="SUM" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_UNIQUE_01" 
                                                            ShowInColumn="Ais Unique" SummaryType="Sum" Tag="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_02" 
                                                            ShowInColumn="Dtac View" SummaryType="Sum" Tag="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_UNIQUE_02" 
                                                            ShowInColumn="Dtac Unique" SummaryType="Sum" Tag="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_03" 
                                                            ShowInColumn="True View" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_UNIQUE_03" 
                                                            ShowInColumn="True Unique" ShowInGroupFooterColumn="True Unique" 
                                                            SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_04" 
                                                            ShowInColumn="True H View" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_UNIQUE_04" 
                                                            ShowInColumn="True H Unique" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_05" 
                                                            ShowInColumn="3GX View" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_UNIQUE_05" 
                                                            ShowInColumn="3GX Unique" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_OTHER" 
                                                            ShowInColumn="Other View" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_UNIQUE_OTHER" 
                                                            ShowInColumn="Other Unique" SummaryType="Sum" />
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
                                                        <dxwgv:GridViewDataTextColumn Caption="Ais View" 
                                                            FieldName="PAGE_VIEW_01" Name="Ais View" VisibleIndex="1">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#E6EEB5">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Ais Unique" FieldName="PAGE_VIEW_UNIQUE_01" 
                                                            VisibleIndex="2">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#E6EEB5">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Dtac View" FieldName="PAGE_VIEW_02" 
                                                            VisibleIndex="3">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#BFECFB">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Dtac Unique" FieldName="PAGE_VIEW_UNIQUE_02" 
                                                            VisibleIndex="4">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#BFECFB">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="True View" FieldName="PAGE_VIEW_03" 
                                                            VisibleIndex="5">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#FEF1F2">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="True Unique" FieldName="PAGE_VIEW_UNIQUE_03" 
                                                            VisibleIndex="6">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#FEF1F2">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="True H View" FieldName="PAGE_VIEW_04" 
                                                            VisibleIndex="7">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#FEF1F2">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="True H Unique" 
                                                            FieldName="PAGE_VIEW_UNIQUE_04" VisibleIndex="8">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#FEF1F2">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="3GX View" FieldName="PAGE_VIEW_05" 
                                                            VisibleIndex="9">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#BDDC9A">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="3GX Unique" 
                                                            FieldName="PAGE_VIEW_UNIQUE_05" VisibleIndex="10">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#BDDC9A">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Other View" FieldName="PAGE_VIEW_OTHER" 
                                                            VisibleIndex="11">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Other Unique" 
                                                            FieldName="PAGE_VIEW_UNIQUE_OTHER" VisibleIndex="12">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
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
                                                </dxwgv:ASPxGridView>
                                            </dxw:ContentControl>
                                        </ContentCollection>
                                    </dxtc:TabPage>
                                    <dxtc:TabPage Name="Ads Click" Text="Ads Click">
                                        <ContentCollection>
                                            <dxw:ContentControl runat="server">
                                                <asp:ImageButton ID="btnClick" runat="server" ImageUrl="~/btnExcel.gif" 
                                                    OnClick="btnClick_Click" style="height: 20px" />
                                                <dxwgv:ASPxGridViewExporter ID="gvClickExport" runat="server" 
                                                    GridViewID="gvClick">
                                                </dxwgv:ASPxGridViewExporter>
                                                <dxwgv:ASPxGridView ID="gvClick" runat="server" AutoGenerateColumns="False" 
                                                    CssFilePath="~/App_Themes/Plastic Blue/{0}/styles.css" CssPostfix="PlasticBlue" 
                                                    Width="100%" OnHtmlDataCellPrepared="gvClick_HtmlDataCellPrepared">
                                                    <Styles CssFilePath="~/App_Themes/Plastic Blue/{0}/styles.css" 
                                                        CssPostfix="PlasticBlue">
                                                        <Header ImageSpacing="10px" SortingImageSpacing="10px">
                                                        </Header>
                                                        <Footer Font-Bold="True" ForeColor="Black">
                                                        </Footer>
                                                    </Styles>
                                                    <TotalSummary>
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_01" 
                                                            ShowInColumn="Ais View" ShowInGroupFooterColumn="Ais View" SummaryType="Sum" 
                                                            Tag="SUM" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_02" 
                                                            ShowInColumn="Dtac Click" ShowInGroupFooterColumn="Dtac Click"  SummaryType="Sum" Tag="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_03" 
                                                            ShowInColumn="True Click" ShowInGroupFooterColumn="True Click" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_04" 
                                                            ShowInColumn="True H Click" ShowInGroupFooterColumn="True H Click" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_05" 
                                                            ShowInColumn="3GX Click" ShowInGroupFooterColumn="3GX Click" SummaryType="Sum" />
                                                        <dxwgv:ASPxSummaryItem DisplayFormat="n" FieldName="PAGE_VIEW_OTHER" 
                                                            ShowInColumn="Other Click" SummaryType="Sum" />
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
                                                        <dxwgv:GridViewDataTextColumn Caption="Ais Click" FieldName="PAGE_VIEW_01" 
                                                            Name="Ais View" VisibleIndex="1">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#E6EEB5">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Dtac Click" FieldName="PAGE_VIEW_02" 
                                                            VisibleIndex="2" Name="Dtac Click">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#BFECFB">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="True Click" FieldName="PAGE_VIEW_03" 
                                                            VisibleIndex="3" Name="True Click">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#FEF1F2">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="True H Click" FieldName="PAGE_VIEW_04" 
                                                            VisibleIndex="4" Name = "True H Click">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#FEF1F2">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="3GX Click" FieldName="PAGE_VIEW_05" 
                                                            VisibleIndex="5" Name = "3GX Click">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#BDDC9A">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Other Click" FieldName="PAGE_VIEW_OTHER" 
                                                            VisibleIndex="6">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
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
                                                </dxwgv:ASPxGridView>
                                            </dxw:ContentControl>
                                        </ContentCollection>
                                    </dxtc:TabPage>
                                    <dxtc:TabPage Text="Ads by Page">
                                        <ContentCollection>
                                            <dxw:ContentControl runat="server">
                                                <asp:ImageButton ID="btnPage" runat="server" ImageUrl="~/btnExcel.gif" 
                                                    OnClick="btnViewPage_Click" />
                                                <dxwgv:ASPxGridViewExporter ID="gvPageExport" runat="server" 
                                                    GridViewID="gvPage">
                                                </dxwgv:ASPxGridViewExporter>
                                                <dxwgv:ASPxGridView ID="gvPage" runat="server" AutoGenerateColumns="False" 
                                                    CssFilePath="~/App_Themes/Plastic Blue/{0}/styles.css" 
                                                    CssPostfix="PlasticBlue"  KeyFieldName="PAGE_NAME"
                                                    Width="100%" OnCustomCallback="gvPage_CustomCallback" 
                                                    OnHtmlDataCellPrepared="gvPage_HtmlDataCellPrepared">
                                                    <Columns>
                                                        <dxwgv:GridViewDataTextColumn Caption="Page Name" FieldName="PAGE_NAME" 
                                                            Name="Page Name" VisibleIndex="0" GroupIndex="0" SortIndex="0" SortOrder="Ascending" >
                                                            <PropertiesTextEdit DisplayFormatString="d">
                                                            </PropertiesTextEdit>
                                                            <FooterTemplate>
                                                                Sum =
                                                            </FooterTemplate>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Operator" FieldName="OPT_CODE" 
                                                            Name="Operator" VisibleIndex="1">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#B0C82C">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Unique" FieldName="SUM_UNIQUE" 
                                                            Name="Unique" VisibleIndex="2">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#34C3F2">
                                                            </CellStyle>
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="View" FieldName="SUM_VIEW" Name="View" 
                                                            VisibleIndex="3">
                                                            <PropertiesTextEdit DisplayFormatString="n">
                                                            </PropertiesTextEdit>
                                                            <CellStyle BackColor="#ED1B24">
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
                                                    <Settings ShowFooter="True" ShowGroupedColumns="True" ShowGroupPanel="True" />
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
    <div>
    
    </div>
    </form>
</body>
</html>
