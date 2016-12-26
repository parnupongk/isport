<%@ Page Title="" Language="C#" MasterPageFile="~/admin/adminMaster.Master" AutoEventWireup="true" CodeBehind="adminimages.aspx.cs" Inherits="isport.admin.adminimages" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentDetail" runat="server">
    <asp:Button ID="btnInsert" runat="server" Text="insert" 
        onclick="btnInsert_Click" />
<dxwgv:ASPxGridView ID="gvImages" runat="server" 
    AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
    CssPostfix="Glass" Width="100%" onrowcommand="gvImages_RowCommand" 
        onpageindexchanged="gvImages_PageIndexChanged">
    <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
        <Header ImageSpacing="5px" SortingImageSpacing="5px">
        </Header>
    </Styles>
    <Images ImageFolder="~/App_Themes/Glass/{0}/">
        <CollapsedButton Height="12px" Width="11px" />
        <DetailCollapsedButton Height="9px" Width="9px" />
        <PopupEditFormWindowClose Height="17px" Width="17px" />
    </Images>
    <Columns>
<dxwgv:GridViewDataButtonEditColumn Caption="Delete" Width="20px" EditFormSettings-Visible="False" VisibleIndex="0">
<EditFormSettings Visible="False"></EditFormSettings>
                                <DataItemTemplate  >
                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" 
                                        ImageUrl="images/icon_trashcan.gif" 
                                        onclientclick="return window.confirm(&quot;Do you want to delete the row&quot;);" CausesValidation="False" PostBackUrl="~/admin/index.aspx" />
                                </DataItemTemplate>
                            </dxwgv:GridViewDataButtonEditColumn>
        <dxwgv:GridViewDataTextColumn Caption="Create Date" FieldName="images_date" 
            VisibleIndex="1">
        </dxwgv:GridViewDataTextColumn>
        <dxwgv:GridViewDataTextColumn Caption="images" FieldName="images_path" 
            VisibleIndex="2">
            <DataItemTemplate>
                <asp:Image ID="Image1" ImageUrl='<%# Bind("images_path") %>' runat="server" />
            </DataItemTemplate>
        </dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn FieldName="images_path" Caption="path" VisibleIndex="3"></dxwgv:GridViewDataTextColumn>
        <dxwgv:GridViewDataTextColumn Caption="Type" FieldName="images_type" 
            VisibleIndex="4">
        </dxwgv:GridViewDataTextColumn>
        <dxwgv:GridViewDataTextColumn Caption="id" FieldName="images_id" 
            Visible="False" VisibleIndex="4">
        </dxwgv:GridViewDataTextColumn>
    </Columns>
</dxwgv:ASPxGridView>
    
    
    <dxpc:ASPxPopupControl ID="selectProductsPopUp" runat="server" AllowDragging="True" 
        CloseAction="CloseButton" EnableClientSideAPI="True" 
        ClientInstanceName="selectProductsPopUp" Width="500px" Height="160px" 
        CssFilePath="~/App_Themes/Blue/{0}/styles.css" CssPostfix="Blue" 
    ImageFolder="~/App_Themes/Blue/{0}/" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" 
        >
        <SizeGripImage Height="16px" Url="~/App_Themes/Blue/Web/pcSizeGrip.gif" 
            Width="16px" />
        <ContentCollection>
            <dxpc:PopupControlContentControl runat="server">
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <CloseButtonImage Height="16px" Width="15px" />
    <Windows>
       <dxpc:PopupWindow HeaderText="Content" Modal="True" Name="ProductPopUp">
           <ContentTemplate>
               <div style="text-align:center;" >
                   <table style="width: 275px; height: 4px;">
                       <tr>
                           <td align="right">
                               image :</td>
                           <td align="left">
                               <asp:FileUpload ID="filUpload" runat="server" />
                           </td>
                       </tr>
                       <tr>
                           <td align="right">
                               &nbsp;</td>
                           <td align="left">
                               <asp:ImageButton ID="btnSubmit" runat="server" 
                                   ImageUrl="~/admin/images/submit2.gif" onclick="btnSubmit_Click" />
                           </td>
                       </tr>
                   </table>
               </div>
           </ContentTemplate>
<ContentCollection>
<dxpc:PopupControlContentControl ID="PopupControlContentControl1" runat="server"></dxpc:PopupControlContentControl>
</ContentCollection>
       
       </dxpc:PopupWindow>
    </Windows>
</dxpc:ASPxPopupControl>
    
</asp:Content>
