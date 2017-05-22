<%@ Page Title="" Language="C#" MasterPageFile="~/admin/adminMaster.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="isport.admin.index" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="DevExpress.Web.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<%@ Register src="../userControl/uMenu.ascx" tagname="uMenu" tagprefix="uc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit.HTMLEditor" tagprefix="cc1" %>
<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dxhe" %>

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
        .style3
        {
            color: #FF3300;
        }
    </style>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentDetail" runat="server">

   <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" >
    <ProgressTemplate>
      <div class="progress">
                <img src="images/Loading.gif" />
                Please Wait...
         </div>
    </ProgressTemplate>
    </asp:UpdateProgress>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
    <asp:PostBackTrigger ControlID="gvContent" />
    <asp:PostBackTrigger ControlID="btnInsert"/>
    </Triggers>
    
    <ContentTemplate>

    <table style="width:40%;">
    <tr>
        <td>
            Project Type :</td>
        <td>
            <asp:DropDownList ID="ddlProject" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddlProject_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
        <tr>
            <td>
                Project Type(New) :</td>
            <td>
                <asp:TextBox ID="txtProjectType" runat="server"></asp:TextBox>
                &nbsp;<asp:HyperLink ID="lnkWap" runat="server" Target="_blank">[[  View  ]]</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                Master Level :</td>
            <td>
                <asp:LinkButton ID="lnkLevel" runat="server"></asp:LinkButton>
            </td>
        </tr>
    <tr>
        <td>
            Master Detail :</td>
        <td>
            <uc1:uMenu ID="uMenu1" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            <asp:Button ID="btnInsert" runat="server" onclick="btnInsert_Click" 
                Text="Insert" />
        &nbsp;<asp:Button ID="btnBack" runat="server" onclick="btnBack_Click" 
                Text="Back to main" />
        </td>
    </tr>
    
        <tr>
            <td class="style1" colspan="2">
                ** Row index = 0 จะบอกหน้านั้น Check Active หรือไม่ (wap or sms)</td>
        </tr>
    
</table>
    <dxwgv:ASPxGridView ID="gvContent" runat="server" AutoGenerateColumns="False" 
    onrowcommand="gvContent_RowCommand" ClientInstanceName="gvContent" 
        KeyFieldName="ui_id" 
        onpageindexchanged="gvContent_PageIndexChanged" Width="1500px"> 
        <SettingsBehavior ConfirmDelete="True" AllowSort="False" />
        
    <Columns>
                                <dxwgv:GridViewDataButtonEditColumn Caption="Delete" EditFormSettings-Visible="False" VisibleIndex="0">
<EditFormSettings Visible="False"></EditFormSettings>
                                <DataItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" 
                                        ImageUrl="images/icon_trashcan.gif" 
                                        onclientclick="return window.confirm(&quot;Do you want to delete the row&quot;);" CausesValidation="False" PostBackUrl="~/admin/index.aspx" />
                                </DataItemTemplate>
                            </dxwgv:GridViewDataButtonEditColumn>
<dxwgv:GridViewDataTextColumn Caption="Add Sub" VisibleIndex="1">
    <DataItemTemplate>
        <asp:LinkButton ID="btnAddSub" runat="server" CommandName="AddSub">Sub Menu</asp:LinkButton>
    </DataItemTemplate>
</dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn Caption="insert" VisibleIndex="2">
    <DataItemTemplate>
        <asp:LinkButton ID="btnInsert" runat="server" CommandName="insert">insert</asp:LinkButton>
    </DataItemTemplate>
                                </dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn Caption="Edit" VisibleIndex="3">
    <DataItemTemplate>
        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit">Edit</asp:LinkButton>
    </DataItemTemplate>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="index" FieldName="ui_index" 
                                    VisibleIndex="4">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="start date" FieldName="ui_startdate" 
                                    VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="end date" FieldName="ui_enddate" 
                                    VisibleIndex="6">
                                </dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn FieldName="content_icon" Caption="Icon" VisibleIndex="7">
    <DataItemTemplate>
        <asp:Image ID="Image2" runat="server" ImageUrl='<%# CheckImagePath( Eval("content_icon").ToString() ) %>' />
    </DataItemTemplate>
</dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="image" FieldName="content_image" 
                                    VisibleIndex="8" Width="100px">
                                    <DataItemTemplate>
                                    <a id="aLink1" target="_blank" href='<%# Bind("content_link") %>' runat="server" >
                                        <asp:Image ID="Image1" Width="100px" runat="server" ImageUrl='<%# CheckImagePath(Eval("content_image").ToString()) %>' />
                                        </a>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="text" FieldName="content_text" 
                                    VisibleIndex="9" Width="200px">
                                    <DataItemTemplate>
                                        <a ID="aLink2" runat="server" href='<%# Bind("content_link") %>' 
                                            target="_blank">
                                        <asp:Label ID="lbl" runat="server" Text='<%# Bind("content_text") %>'></asp:Label>
                                        </a>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="link" FieldName="content_link" 
                                    VisibleIndex="10" Width="100px">
                                </dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn FieldName="content_align" Caption="align" VisibleIndex="11"></dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="is master" FieldName="ui_ismaster" 
                                    VisibleIndex="12">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="is payment" FieldName="ui_ispayment" 
                                    VisibleIndex="13" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                
                                <dxwgv:GridViewDataTextColumn Caption="id" FieldName="ui_id" Visible="False" 
                                    VisibleIndex="14">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="ui_master_id" FieldName="ui_master_id" 
                                    Visible="False" VisibleIndex="15">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="ui_operator" FieldName="ui_operator" 
                                    Visible="False" VisibleIndex="16">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="SG ID" FieldName="ui_sg_id" 
                                    Visible="true" VisibleIndex="17">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="BreakAfter" FieldName="content_breakafter" 
                                    Visible="true" VisibleIndex="18">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="is News" FieldName="ui_isnews" 
                                    Visible="true" VisibleIndex="19">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="is News Top" FieldName="ui_isnews_top" 
                                    Visible="true" VisibleIndex="20">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Content" FieldName="ui_contentname" 
                                    Visible="true" VisibleIndex="21">
                                </dxwgv:GridViewDataTextColumn>
                                
                                <dxwgv:GridViewDataTextColumn Caption="content_color" FieldName="content_color" 
                                    Visible="False" VisibleIndex="18">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="content_bold" FieldName="content_bold" 
                                    Visible="False" VisibleIndex="19">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="content_isredirect" FieldName="content_isredirect" 
                                    Visible="True" VisibleIndex="20">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Check Wap" FieldName="ui_ispaymentwap" 
                                    Visible="false" VisibleIndex="21">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Check SMS" FieldName="ui_ispaymentsms" 
                                    Visible="false" VisibleIndex="22">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="bgColor" FieldName="content_bgcolor" 
                                    Visible="false" VisibleIndex="23">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="txtSize" FieldName="content_txtsize" 
                                    Visible="false" VisibleIndex="24">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="isGallert" FieldName="content_isgallery" 
                                    Visible="false" VisibleIndex="25">
                                </dxwgv:GridViewDataTextColumn>
                                
    </Columns>

    </dxwgv:ASPxGridView>
    
               </ContentTemplate>
    </asp:UpdatePanel>
    
    <dxpc:ASPxPopupControl ID="selectProductsPopUp" runat="server" AllowDragging="True" 
        CloseAction="CloseButton" EnableClientSideAPI="True" 
        ClientInstanceName="selectProductsPopUp" PopupVerticalAlign="WindowCenter" 
        PopupHorizontalAlign="WindowCenter" >
        <ContentCollection>
            <dxpc:PopupControlContentControl runat="server">
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    <Windows>
       <dxpc:PopupWindow HeaderText="Content" Modal="True"  Name="ProductPopUp">
           <ContentTemplate>
               <div style="text-align:center;overflow:scroll;max-height:500px;" >
                   <table style="border: 2px solid #9999FF; " 
                       cellpadding="3" cellspacing="3" bgcolor="#FF9933" width="100%">
                       <tr>
                           <td align="right" bgcolor="White" class="style2" width="80px">
                               Icon :</td>
                           <td align="left" bgcolor="White">
                               <asp:TextBox ID="txtIcon" runat="server" Width="100px"></asp:TextBox>
                               <asp:ImageButton ID="btnSearech" runat="server" 
                                   ImageUrl="~/admin/images/bt-search.gif" onclick="btnSearech_Click" />
                           </td>
                           <td align="right" bgcolor="White">
                               SG ID :</td>
                           <td align="left" 
                               
                               style="border-style: none none solid none; border-bottom-width: 2px; border-bottom-color: #9999FF" 
                               bgcolor="White">
                               <asp:TextBox ID="txtSgId" runat="server" Width="100px"></asp:TextBox>
                           </td>
                       </tr>
                       <tr>
                           <td align="right" bgcolor="White" class="style2" width="80px">
                               index&nbsp; :</td>
                           <td align="left" bgcolor="White">
                               <asp:TextBox ID="txtIndex" runat="server" Width="100px"></asp:TextBox>
                           </td>
                           <td align="right" bgcolor="White">
                               Operator :</td>
                           <td style="border-style: none none solid none; border-bottom-width: 2px; border-bottom-color: #9999FF" 
                               bgcolor="White" align="left">
                               <asp:DropDownList ID="ddlOperator" runat="server" Width="100px">
                                   <asp:ListItem Value="All">All</asp:ListItem>
                                   <asp:ListItem Value="01">Ais</asp:ListItem>
                                   <asp:ListItem Value="02">Dtac</asp:ListItem>
                                   <asp:ListItem Value="03">Ture</asp:ListItem>
                               </asp:DropDownList>
                           </td>
                       </tr>
                       <tr>
                           <td align="right" bgcolor="White" class="style2" width="80px">
                               Start date :</td>
                           <td align="left" bgcolor="White">
                               <dxe:ASPxDateEdit ID="dateStart" runat="server" EditFormat="DateTime" 
                                   Width="200px">
                               </dxe:ASPxDateEdit>
                           </td>
                           <td align="right" bgcolor="White">
                               End date :</td>
                           <td bgcolor="White" 
                               
                               style="border-style: none none solid none; border-bottom-width: 2px; border-bottom-color: #9999FF" 
                               align="left">
                               <dxe:ASPxDateEdit ID="dateEnd" runat="server" EditFormat="DateTime" 
                                   Width="200px">
                               </dxe:ASPxDateEdit>
                           </td>
                       </tr>
                       <tr>
                           <td align="right" bgcolor="White" class="style2" width="80px">
                               Video or Images :</td>
                           <td align="left" bgcolor="White">
                              
                               <asp:FileUpload ID="filUpload" runat="server" />
                           </td>
                           <td align="right" bgcolor="White">
                               Image URL :</td>
                           <td align="left" bgcolor="White" 
                               style="border-style: none none solid none; border-bottom-width: 2px; border-bottom-color: #9999FF">
                               <asp:TextBox ID="txtImgURL" runat="server" Width="300px"></asp:TextBox>
                           </td> 
                       </tr>
                       <tr>
                           <td align="right" bgcolor="White" class="style2" width="80px">
                               &nbsp;</td>
                           <td align="left" bgcolor="White" colspan="3">
                              
                               &nbsp;</td>
                       </tr>
                       <tr>
                           <td align="right" rowspan="2" bgcolor="White" class="style2" width="80px">
                               Images :</td>
                           <td align="left" colspan="3" bgcolor="White">
                           <asp:AjaxFileUpload ID="AjaxFileUpload1" runat="server" Padding-Bottom="4"
            Padding-Left="2" Padding-Right="1" Padding-Top="4" ThrobberID="myThrobber"  MaximumNumberOfFiles="10"
            AllowedFileTypes="jpg,jpeg,png,gif" 
                 onuploadcomplete="AjaxFileUpload1_UploadComplete"   />
                 
                           </td>
                       </tr>
                       <tr>
                           <td align="left" colspan="3" bgcolor="White">
                               <asp:CheckBox ID="chkDelete" runat="server" Text="Delete" />
                               <asp:Image ID="img" runat="server" />
                           </td>
                       </tr>
                       <tr>
                           <td align="right" bgcolor="White" class="style2" width="80px">
                               link :</td>
                           <td align="left" colspan="3" bgcolor="White">
                               <asp:TextBox ID="txtLink" runat="server" Width="300px"></asp:TextBox> &nbsp;<span 
                                   class="style3">** fix &quot;?mp_code&quot; ¨Ð·Ó¡ÒÃ¹Ó request[&quot;mp_code&quot;] µÒÁµèÍãËé
                               </span>
                           </td>
                       </tr>
                       <tr>
                           <td align="right" bgcolor="White" class="style2" width="80px">
                               text :</td>
                           <td align="left" colspan="3" bgcolor="White">
                               <dxhe:ASPxHtmlEditor ID="txtText" runat="server" Height="80px">
                                   <settingsimageupload>
                                       <validationsettings allowedcontenttypes="image/jpeg,image/pjpeg,image/gif,image/png,image/x-png">
                                       </validationsettings>
                                   </settingsimageupload>
                                   <Settings AllowContextMenu="False" AllowDesignView="False" 
                                       AllowInsertDirectImageUrls="False" AllowPreview="False" />
                               </dxhe:ASPxHtmlEditor>
                           </td>
                       </tr>
                       <tr>
                           <td align="right" bgcolor="White" class="style2" width="80px">
                               text font :</td>
                           <td align="left" colspan="3" bgcolor="White">
                               <asp:DropDownList ID="ddlColor" runat="server">
                                   <asp:ListItem Selected=True>Black</asp:ListItem>
                                   <asp:ListItem>Red</asp:ListItem>
                                   <asp:ListItem>Yellow</asp:ListItem>
                                   <asp:ListItem>Blue</asp:ListItem>
                                   <asp:ListItem>Green</asp:ListItem>
                                   <asp:ListItem>white</asp:ListItem>
                               </asp:DropDownList>
                               &nbsp; Bold :
                               <asp:DropDownList ID="ddlBold" runat="server">
                                   <asp:ListItem>false</asp:ListItem>
                                   <asp:ListItem>true</asp:ListItem>
                               </asp:DropDownList>
                               &nbsp; Size :
                               <asp:DropDownList ID="ddlFontSize" runat="server">
                                   <asp:ListItem>high</asp:ListItem>
                                   <asp:ListItem Selected=True>medium</asp:ListItem>
                                   <asp:ListItem>small</asp:ListItem>
                               </asp:DropDownList>
                               &nbsp;&nbsp; Bg Color :
                               <asp:DropDownList ID="ddlBgColor" runat="server">
                                   <asp:ListItem Selected="true">white</asp:ListItem>
                                   <asp:ListItem>Black</asp:ListItem>
                                   <asp:ListItem>Red</asp:ListItem>
                                   <asp:ListItem>Yellow</asp:ListItem>
                                   <asp:ListItem>Blue</asp:ListItem>
                                   <asp:ListItem>Green</asp:ListItem>
                               </asp:DropDownList>
                               &nbsp;&nbsp; Align :
                               <asp:DropDownList ID="ddlAlign" runat="server" Width="100px">
                                   <asp:ListItem Selected="True">Center</asp:ListItem>
                                   <asp:ListItem>Left</asp:ListItem>
                                   <asp:ListItem>Right</asp:ListItem>
                               </asp:DropDownList>
                               &nbsp;&nbsp; Content :<asp:DropDownList ID="ddlContent" runat="server">
                                   <asp:ListItem Value="">-- Selected --</asp:ListItem>
                                   <asp:ListItem Value="footballlivescore">football livescore</asp:ListItem>
                                   <asp:ListItem Value="footballprogram">football program</asp:ListItem>
                                   <asp:ListItem Value="footballtable">football table</asp:ListItem>
                                   <asp:ListItem Value="analyse">analyse</asp:ListItem>
                                   <asp:ListItem Value="topscore">topscore</asp:ListItem>
                                   <asp:ListItem Value="tded">tded</asp:ListItem>
                                   <asp:ListItem Value="video">video</asp:ListItem>
                                   <asp:ListItem Value="subscribe">subscribe-ais</asp:ListItem>
                                   <asp:ListItem Value="subscribe">subscribe-dtac</asp:ListItem>
                                   <asp:ListItem Value="subscribe">subscribe-true</asp:ListItem>
                               </asp:DropDownList>
                           </td>
                       </tr>
                       <tr>
                           <td align="right" bgcolor="White" class="style2" width="80px">
                               is Header (Answer) :</td>
                           <td align="left" colspan="3" bgcolor="White">
                               <asp:CheckBox ID="chkPay" runat="server" />
                               &nbsp; Break After :<asp:CheckBox ID="chkBreakAfter" runat="server" />
                               &nbsp; Is Gallery :
                               <asp:CheckBox ID="chkIsGallery" runat="server" />
                           </td>
                       </tr>
                       <tr>
                           <td align="right" bgcolor="White" class="style2" width="80px">
                               is News :</td>
                           <td align="left" colspan="3" bgcolor="White">
                               <asp:CheckBox ID="chkNews" runat="server" />
                               &nbsp;Top:<asp:TextBox ID="txtNewsTop" runat="server" MaxLength="2" Width="100px"></asp:TextBox>
                           </td>
                       </tr>
                       <tr>
                           <td align="right" bgcolor="White" class="style2" width="80px">
                               is Redirect</td>
                           <td align="left" colspan="3" bgcolor="White">
                               <asp:CheckBox ID="chkIsRedirect" runat="server" />
                               &nbsp;is Payment Wap <asp:CheckBox ID="chkIsPaymentWap" runat="server" />
                               &nbsp;is Payment SMS <asp:CheckBox ID="chkIsPaymentSMS" runat="server" />
                            </td>
                       </tr>
                       <tr>
                           <td align="center" bgcolor="White" colspan="4">
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
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# Bind("images_path") %>' />
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
