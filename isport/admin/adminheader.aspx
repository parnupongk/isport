<%@ Page Title="" Language="C#" MasterPageFile="~/admin/adminMaster.Master" AutoEventWireup="true" CodeBehind="adminheader.aspx.cs" Inherits="isport.admin.adminheader" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .style1
        {
            color: #FF0066;
        }
        img
        {
            max-height:200px;
        }
        .style2
        {
            width: 120px;
        }
        .progress
            {
            	position: absolute;
                background-color: #FAFAFA;
                z-index: 2147483647 !important;
                opacity: 0.8;
                overflow: hidden;
                text-align: center; top: 0; left: 0;
                height: 100%;
                width: 100%;
                padding-top:20%;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentDetail" runat="server">

   <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" >
    <ProgressTemplate>
      <div class="progress">
                <img src="images/Loading.gif" alt="img"/>
                Please Wait...
         </div>
    </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
    <asp:PostBackTrigger ControlID="gvContent" />
    </Triggers>
    <ContentTemplate>
    
    <table style="width: 100%;">
        <tr>
            <td align="center">
                &nbsp; &nbsp; Project Type : &nbsp;<asp:DropDownList ID="ddlProject" runat="server" 
                    AutoPostBack="True" onselectedindexchanged="ddlProject_SelectedIndexChanged">
                </asp:DropDownList>
&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                Project Type : &nbsp;&nbsp;
                <asp:TextBox ID="txtProjectTpye" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
    <dxwgv:ASPxGridView ID="gvContent" runat="server" AutoGenerateColumns="False"  EnableCallBacks="False" 
    onrowcommand="gvContent_RowCommand" ClientInstanceName="gvContent" 
        KeyFieldName="header_id" Width="100%" onpageindexchanged="gvContent_PageIndexChanged">
        <SettingsBehavior ConfirmDelete="True" />
        
        <Settings ShowFooter="True" />
        
    <Columns>
                                <dxwgv:GridViewDataButtonEditColumn Caption="Delete" EditFormSettings-Visible="False" VisibleIndex="0">
<EditFormSettings Visible="False"></EditFormSettings>
                                <DataItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" 
                                        ImageUrl="images/icon_trashcan.gif" 
                                        onclientclick="return window.confirm(&quot;Do you want to delete the row&quot;);" CausesValidation="False" PostBackUrl="~/admin/index.aspx" />
                                </DataItemTemplate>
                            </dxwgv:GridViewDataButtonEditColumn>
<dxwgv:GridViewDataTextColumn Caption="insert" VisibleIndex="1">
    <DataItemTemplate>
        <asp:LinkButton ID="btnInsert" runat="server" CommandName="insert">insert</asp:LinkButton>
    </DataItemTemplate>
    <FooterTemplate>
        <asp:Button ID="btnInsert" runat="server" onclick="btnInsert_Click" 
            Text="Insert" />
    </FooterTemplate>
</dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn Caption="Edit" VisibleIndex="2">
    <DataItemTemplate>
        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit">Edit</asp:LinkButton>
    </DataItemTemplate>
                                </dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn Caption="index" VisibleIndex="3" FieldName="header_index">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="start date" FieldName="header_startdate" 
                                    VisibleIndex="4">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="end date" FieldName="header_enddate" 
                                    VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Icon" FieldName="content_icon" 
                                    VisibleIndex="6">
                                    <dataitemtemplate>
                                        <asp:Image ID="Image2" runat="server" ImageUrl='<%# CheckImagePath(Eval("content_icon").ToString()) %>' />
                                    </dataitemtemplate>
                                </dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn FieldName="content_image" Caption="image" VisibleIndex="7">
    <DataItemTemplate>
        <a id="aLink1" runat="server" href='<%# Bind("content_link") %>' 
            target="_blank">
        <asp:Image ID="Image1" runat="server" ImageUrl='<%# CheckImagePath(Eval("content_image").ToString()) %>' />
        </a>
    </DataItemTemplate>
</dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="text" FieldName="content_text" 
                                    VisibleIndex="8">
                                    <DataItemTemplate>
                                    <a id="aLink2" target="_blank" href='<%# Bind("content_link") %>' runat="server" >
                                        <asp:Label ID="lbl" runat="server" Text='<%# Bind("content_text") %>'></asp:Label>
                                        </a>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="link" FieldName="content_link" 
                                    VisibleIndex="9">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="align" FieldName="content_align" 
                                    VisibleIndex="10">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="is payment" FieldName="header_ispayment" 
                                    VisibleIndex="12">
                                </dxwgv:GridViewDataTextColumn>
                                
                                <dxwgv:GridViewDataTextColumn Caption="id" FieldName="header_id" Visible="False" 
                                    VisibleIndex="12">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="operator" FieldName="header_operator" 
                                    VisibleIndex="13">
                                </dxwgv:GridViewDataTextColumn>
                                
                                <dxwgv:GridViewDataTextColumn Caption="is default" Name="header_isdefault" FieldName="header_isdefault"
                                    VisibleIndex="14">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="content_breakafter" Name="content_breakafter" FieldName="content_breakafter"
                                    VisibleIndex="15" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Random" Name="header_random" FieldName="header_random"
                                    VisibleIndex="16" Visible="true">
                                </dxwgv:GridViewDataTextColumn>
                                
    </Columns>

    </dxwgv:ASPxGridView>
    
    
            </td>
        </tr>
    </table>
                   </ContentTemplate>
    </asp:UpdatePanel>
    
    <dxpc:ASPxPopupControl ID="selectProductsPopUp" runat="server" AllowDragging="True" 
        CloseAction="CloseButton" EnableClientSideAPI="True" 
        ClientInstanceName="selectProductsPopUp" 
    PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" 
        >
        <ContentCollection>
            <dxpc:PopupControlContentControl runat="server">
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    <Windows>
       <dxpc:PopupWindow HeaderText="Content" Modal="True" Name="ProductPopUp">
           <ContentTemplate>
               <div style="text-align:center;" >
                   <table style="width: 450px; height: 91px;">
                       <tr>
                           <td align="right">
                               Icon :</td>
                           <td align="left">
                               <asp:TextBox ID="txtIcon" runat="server" Width="100px"></asp:TextBox>
                               <asp:ImageButton ID="btnSearech" runat="server" 
                                   ImageUrl="~/admin/images/bt-search.gif" onclick="btnSearech_Click" />
                           </td>
                           <td align="right">
                               SG ID :</td>
                           <td align="left">
                               <asp:TextBox ID="txtSgId" runat="server" Width="100px"></asp:TextBox>
                           </td>
                       </tr>
                       <tr>
                           <td align="right">
                               index&nbsp; :</td>
                           <td align="left">
                               <asp:TextBox ID="txtIndex" runat="server" Width="100px"></asp:TextBox>
                           </td>
                           <td align="right">
                               Operator :</td>
                           <td>
                               <asp:DropDownList ID="ddlOperator" runat="server" Width="100px">
                                   <asp:ListItem Value="All">All</asp:ListItem>
                                   <asp:ListItem Value="01">Ais</asp:ListItem>
                                   <asp:ListItem Value="02">Dtac</asp:ListItem>
                                   <asp:ListItem Value="03">Ture</asp:ListItem>
                               </asp:DropDownList>
                           </td>
                       </tr>
                       <tr>
                           <td align="right">
                               Start date :</td>
                           <td align="left" colspan="3">
                               <dxe:ASPxDateEdit ID="dateStart" runat="server" Width="200px" 
                                   EditFormat="DateTime">
                               </dxe:ASPxDateEdit>
                           </td>
                       </tr>
                       <tr>
                           <td align="right">
                               End date :</td>
                           <td align="left" colspan="3">
                               <dxe:ASPxDateEdit ID="dateEnd" runat="server" EditFormat="DateTime" 
                                   Width="200px">
                               </dxe:ASPxDateEdit>
                           </td>
                       </tr>
                       <tr>
                           <td align="right">
                               image :</td>
                           <td align="left">
                               <asp:FileUpload ID="filUpload" runat="server" />
                           </td>
                           <td align="left" colspan="2">
                               <asp:CheckBox ID="chkDelete" runat="server" Text="Delete" />
                               <asp:Image ID="img" runat="server" />
                           </td>
                       </tr>
                       <tr>
                           <td align="right">
                               link :</td>
                           <td align="left" colspan="3">
                               <asp:TextBox ID="txtLink" runat="server" Width="300px"></asp:TextBox>
                           </td>
                       </tr>
                       <tr>
                           <td align="right">
                               text :</td>
                           <td align="left" colspan="3">
                               <asp:TextBox ID="txtText" runat="server" Width="300px"></asp:TextBox>
                           </td>
                       </tr>
                       <tr>
                           <td align="right">
                               align :</td>
                           <td align="left" colspan="3">
                               <asp:DropDownList ID="ddlAlign" runat="server" Width="100px">
                                   <asp:ListItem>Left</asp:ListItem>
                                   <asp:ListItem>Center</asp:ListItem>
                                   <asp:ListItem>Right</asp:ListItem>
                               </asp:DropDownList>
                           </td>
                       </tr>
                       <tr>
                           <td align="right">
                               is payment :</td>
                           <td align="left" colspan="3">
                               <asp:CheckBox ID="chkPay" runat="server" />
                               &nbsp; is default :<asp:CheckBox ID="chkDefault" runat="server" />
                               &nbsp;Break After :<asp:CheckBox ID="chkBreakAfter" runat="server" />
                           </td>
                       </tr>
                       <tr>
                           <td align="right">
                               Random :
                           </td>
                           <td align="left" colspan="3">
                               <asp:DropDownList ID="ddlRandom" runat="server">
                                   <asp:ListItem Value="">--Selected--</asp:ListItem>
                                   <asp:ListItem Value="start">start</asp:ListItem>
                                   <asp:ListItem Value="end">end</asp:ListItem>
                               </asp:DropDownList>
                           </td>
                       </tr>
                       <tr>
                           <td align="center" colspan="4">
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
        <dxpc:PopupWindow HeaderText="Images" Name="imagesPopup">
            <ContentTemplate>
                <dxwgv:ASPxGridView ID="gvImages" runat="server" AutoGenerateColumns="False" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                    onrowcommand="gvImages_RowCommand" Width="100%" KeyFieldName="images_id" 
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
                        <dxwgv:GridViewDataButtonEditColumn Caption="Select" 
                            EditFormSettings-Visible="False" VisibleIndex="0" Width="20px">
                            <EditFormSettings Visible="False" />
                            <DataItemTemplate>
                                <asp:LinkButton ID="lnkSelect" runat="server" CommandArgument='<%# Bind("images_path") %>'
                                    CommandName="Select">Select</asp:LinkButton>
                            </DataItemTemplate>
                        </dxwgv:GridViewDataButtonEditColumn>
                        <dxwgv:GridViewDataTextColumn Caption="images" FieldName="images_path" 
                            VisibleIndex="1">
                            <DataItemTemplate>
                                <asp:Image ID="Image3" runat="server" ImageUrl='<%# Bind("images_path") %>' />
                            </DataItemTemplate>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="path" FieldName="images_path" 
                            VisibleIndex="2">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="id" FieldName="images_id" 
                            Visible="False" VisibleIndex="4">
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </ContentTemplate>
            <ContentCollection>
                <dxpc:PopupControlContentControl runat="server">
                </dxpc:PopupControlContentControl>
            </ContentCollection>
        </dxpc:PopupWindow>
    </Windows>
</dxpc:ASPxPopupControl>
    
    
</asp:Content>
