<%@ Page Title="" Language="C#" MasterPageFile="~/admin/adminMaster.Master" AutoEventWireup="true" CodeBehind="adminapplication.aspx.cs" Inherits="isport.admin.adminapplication" Async="true" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
       function onClientUploadCompleteAll(sender, e) {
           alert('success');
           //document.getElementById('lblSum').innerHTML = "Success";

           window.location.reload();
       }
       </script>
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
    <asp:PostBackTrigger ControlID="btnUpload" />
    <asp:PostBackTrigger ControlID="uploadMedia"/>
    <asp:PostBackTrigger ControlID="ddlType"/>
    </Triggers>
    
    <ContentTemplate>


<div style="background-color: #666699; text-align: center; line-height: 40px; font-weight: bold; font-size: 14px; color: #FFFFFF;"> Upload Model and Content </div>

            <div style="text-align:center;padding:15px;">Type : <asp:DropDownList runat="server" ID="ddlType" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                <asp:ListItem>++Selected++</asp:ListItem>
                <asp:ListItem>jza</asp:ListItem>
                <asp:ListItem>model</asp:ListItem>
                <asp:ListItem>sextip</asp:ListItem>
                <asp:ListItem>iSoccer</asp:ListItem>
                <asp:ListItem>CheerBallThai</asp:ListItem>
                </asp:DropDownList>
    </div>
    <div style="text-align:center;padding:15px;">Upload Excel : <asp:FileUpload ID="fileUpload" runat="server" />&nbsp;<asp:Button 
        ID="btnUpload" runat="server" onclick="btnUpload_Click" Text="Upload" />
    </div>
    <div>
        <asp:Label ID="lblSum" runat="server" Text=""></asp:Label>
    
    <div style=" background-color:#D1DDF1;">
        

        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1" 
            VerticalStripWidth="" Width="100%" Height="400px" ScrollBars="Auto" >
            <asp:TabPanel runat="server" HeaderText="Model" ID="TabPanel1">
                <ContentTemplate>
                
                    <asp:GridView ID="gvModel" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" EnableModelValidation="True" ForeColor="#333333" 
                        GridLines="None" Width="100%">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="km_id" HeaderText="id" />
                            <asp:BoundField DataField="fname" HeaderText="fname" />
                            <asp:BoundField DataField="lname" HeaderText="lname" />
                            <asp:BoundField DataField="nname" HeaderText="nname" />
                            <asp:BoundField DataField="shape" HeaderText="shape" />
                            <asp:BoundField DataField="km_w" HeaderText="width" />
                            <asp:BoundField DataField="km_h" HeaderText="height" />
                            <asp:BoundField DataField="interview" HeaderText="interview" />
                            <asp:BoundField DataField="create_date" HeaderText="create date" />
                            <asp:BoundField DataField="create_by" HeaderText="create by" />
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" HeaderText="Content" ID="TabPanel2">
                <ContentTemplate>
                    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" EnableModelValidation="True" ForeColor="#333333" 
            GridLines="None" Width="100%" AllowPaging="True" DataKeyNames="kc_id" 
                        onselectedindexchanged="gvData_SelectedIndexChanged" PageSize="12" 
                        onrowdeleting="gvData_RowDeleting" 
                        onpageindexchanging="gvData_PageIndexChanging">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                            CommandName="Delete" ImageUrl="~/admin/images/icon_trashcan.gif" 
                            onclientclick="return confirm('Do you want to delete the row?');" 
                            Text="Delete" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="display_txt" HeaderText="Date" />
                <asp:BoundField DataField="km_id" HeaderText="Model" />
                <asp:BoundField DataField="title" HeaderText="Title" >
                <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="title_detail" HeaderText="Detail" />
                <asp:BoundField DataField="title_detail1" HeaderText="Detail1" />
                <asp:BoundField DataField="title_detail2" HeaderText="Detail2" />
                <asp:BoundField DataField="Footer" HeaderText="Footer" />
                <asp:BoundField DataField="free" HeaderText="Is Free" />
                <asp:BoundField DataField="type" HeaderText="type" />
                <asp:BoundField DataField="kc_id" HeaderText="id" Visible="False" />
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
                </ContentTemplate>

            </asp:TabPanel>
        </asp:TabContainer>

    </div>
    
    <div style="background-color: #666699; text-align: center; line-height: 40px; font-weight: bold; font-size: 14px; color: #FFFFFF;"> Upload Media (images , video )</div>

    <div>
    <div style="width:400px;border:1px;border-bottom-color:Black;padding:15px;">
        <asp:AjaxFileUpload ID="uploadMedia" runat="server" Padding-Bottom="4" 
            Padding-Left="2" Padding-Right="1" Padding-Top="4" 
            ThrobberID="myThrobber"  MaximumNumberOfFiles="10"
            AllowedFileTypes="jpg,jpeg,png,gif,mp4,3gp" 
            onuploadcomplete="uploadMedia_UploadComplete" 
            onuploadcompleteall="uploadMedia_UploadCompleteAll" 
            OnClientUploadCompleteAll="onClientUploadCompleteAll">
            </asp:AjaxFileUpload>
        </div>
        <asp:GridView ID="gvMedia" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" EnableModelValidation="True" ForeColor="#333333" 
            GridLines="None" Width="80%" DataKeyNames="kme_id" 
            onrowdeleting="gvMedia_RowDeleting">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                            CommandName="Delete" ImageUrl="~/admin/images/icon_trashcan.gif" 
                            onclientclick="return confirm('Do you want to delete the row?');" 
                            Text="Delete" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="pic" HeaderText="Pic" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="clip" HeaderText="Clip" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="kc_id" HeaderText="Content ID" >
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="kme_id" HeaderText="kme_id" Visible="False" />
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            
        </asp:GridView>
    
    </div>
    </div>

    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
