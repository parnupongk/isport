<%@ Page Title="" Language="C#" MasterPageFile="~/admin/adminMaster.Master" AutoEventWireup="true" CodeBehind="newpage.aspx.cs" Inherits="isport.admin.newpage"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v8.1, Version=8.1.4.0, Culture=neutral, PublicKeyToken=9b171c9fd64da1d1" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
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
                <img src="images/Loading.gif" />
                Please Wait...
         </div>
    </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
    <asp:PostBackTrigger ControlID="btnSubmit"/>
    <asp:PostBackTrigger ControlID="gvData" />
    <asp:PostBackTrigger ControlID="btnEdtSubmit" />
    <asp:AsyncPostBackTrigger ControlID="ddlSubName" EventName="SelectedIndexChanged" />
    <asp:AsyncPostBackTrigger ControlID="ddlContentType" EventName="SelectedIndexChanged" />
    </Triggers>
    <ContentTemplate>


    <table style="width: 100%;" cellpadding="2" bgcolor="#0066FF" cellspacing="2">
        <tr>
            <td bgcolor="White" align="right">
                &nbsp; Service :</td>
            <td align="left" bgcolor="White">
                <asp:DropDownList ID="ddlSubName" runat="server" AutoPostBack="True" 
                    Height="20px" onselectedindexchanged="ddlSubName_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;
                <asp:TextBox ID="txtName" runat="server" Height="20px" Width="190px" 
                    Enabled="False"></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td bgcolor="White" rowspan="2">
                &nbsp;</td>
            <td align="left" bgcolor="White">
                <asp:CheckBox ID="chkIsWapPay" runat="server" Text="Check Wap Payment" />
                &nbsp;<asp:CheckBox ID="chkIsSmsPay" runat="server" Text="Check SMS Payment" />
            </td>
        </tr>
        <tr>
            <td align="left" bgcolor="White">
                
                    <asp:Panel ID="pnlSMS" runat="server" Visible="true">
                <table style="width:100%;"  cellpadding="3" cellspacing="3">
                    <tr>
                        <td align="right">
                            Send Now :</td>
                        <td>
                            <asp:CheckBox ID="chkSendNow" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Content Type :
                        </td>
                        <td>
                           
                            <div style="float:left;vertical-align:top"> &nbsp;
                            <asp:DropDownList ID="ddlContentType" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="ddlContentType_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="">++Selected++</asp:ListItem>
                                <asp:ListItem Value="news">news</asp:ListItem>
                                <asp:ListItem Value="gallery">gallery</asp:ListItem>
                                <asp:ListItem Value="clip">clip</asp:ListItem>
                                <asp:ListItem Value="pdf">pdf</asp:ListItem>
                            </asp:DropDownList>
                            
                            <asp:FileUpload ID="filUpload" runat="server"  />
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            SMS Text :</td>
                        <td>
                            &nbsp;<asp:TextBox ID="txtSMS" runat="server" Height="54px" MaxLength="150" 
                                TextMode="MultiLine" Width="423px"></asp:TextBox>
                            &nbsp;ไม่เกิน 150 ตัวอักษร</td>
                    </tr>
                    <tr>
                        <td align="right">
                            SMS Choose :</td>
                        <td>
                            &nbsp;<asp:TextBox ID="txtChoose" runat="server" Height="54px" 
                                TextMode="MultiLine" Width="423px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Sms Answer : 
                        </td>
                        <td>
                            &nbsp;<asp:TextBox ID="txtAnswer" runat="server" Width="423px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Display Date :</td>
                        <td>
                            &nbsp;<asp:TextBox ID="txtDisplayDate" runat="server" Height="20px" 
                                Width="139px"></asp:TextBox>
                            <asp:CalendarExtender
                                ID="calExten" runat="server" Format="yyyyMMdd" TargetControlID="txtDisplayDate">
                            </asp:CalendarExtender>
  
                        &nbsp;Format &quot; YYYYMMDD &quot;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <dxe:ASPxButton ID="btnSubmit" runat="server" Height="50px" 
                                onclick="btnSubmit_Click" Text="Insert New Page" Width="150px">
                            </dxe:ASPxButton>
                        </td>
                    </tr>
                    </table>
                    </asp:Panel>
                    
                    <asp:Panel ID="pnlProgram" runat="server" Visible="false" 
                    HorizontalAlign="Center">
                    <div style="text-align:center">
                        Program Date : <asp:TextBox ID="txtProgramDate" runat="server" Height="20px" 
                                Width="139px"></asp:TextBox>
                            <asp:CalendarExtender
                                ID="CalendarExtender1" runat="server" Format="yyyyMMdd" TargetControlID="txtProgramDate">
                            </asp:CalendarExtender>
                        &nbsp;<asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click" 
                            Text="Search" />
                    </div>
                        <dxwgv:ASPxGridView ID="gvProgram"  runat="server" AutoGenerateColumns="False"
                            onpageindexchanged="gvProgram_PageIndexChanged" 
                            onhtmlrowprepared="gvProgram_HtmlRowPrepared">
                            <Settings ShowVerticalScrollBar="true" VerticalScrollableHeight="300" />
                            <SettingsPager Mode="ShowAllRecords">
                            </SettingsPager>
                            <Columns>
                                <dxwgv:GridViewDataComboBoxColumn VisibleIndex="0" Width="300px">
                                    <PropertiesComboBox ValueType="System.String" >
                                    </PropertiesComboBox>
                                    <DataItemTemplate>
                                        <table style="width:300px;">
                                            <tr >
                                                <td style="width:100px;">
                                                    <asp:CheckBox ID="rdoTeam1" runat="server" GroupName="team" 
                                                        Text='<%# Bind("teamName1_en") %>' />
                                                </td>
                                                <td>
                                                    VS</td>
                                                <td style="width:100px;">
                                                    <asp:CheckBox ID="rdoTeam2" runat="server" GroupName="team" 
                                                        Text='<%# Bind("teamName2_en") %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn Caption="team name1 th" FieldName="teamName1_th" 
                                    Name="teamName1_th" VisibleIndex="1">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="team name2 th" FieldName="teamName2_th" 
                                    Name="teamName2_th" VisibleIndex="2">
                                </dxwgv:GridViewDataTextColumn>
                                
                                <dxwgv:GridViewDataTextColumn Caption="team code1" FieldName="team_code1" 
                                    Name="team_code1" VisibleIndex="3">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="tema code2" FieldName="team_code2" 
                                    Name="team_code2" VisibleIndex="4">
                                </dxwgv:GridViewDataTextColumn>

                                
                                <dxwgv:GridViewDataTextColumn Caption="match date" FieldName="match_datetime" 
                                    Name="match_datetime" VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="match time" FieldName="match_time" 
                                    Name="match_time" VisibleIndex="6">
                                </dxwgv:GridViewDataTextColumn>
                                
                                <dxwgv:GridViewDataTextColumn Caption="msch id" FieldName="msch_id" 
                                    Name="msch_id" VisibleIndex="7">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    
                        <br />
                        <asp:Button ID="btnProgram" runat="server" onclick="Button1_Click" 
                            Text="Insert " Height="50px" Width="150px" />
                    
                    </asp:Panel>
                    
                    <asp:Panel ID="pnlEDT"  runat="server" Visible="false"  HorizontalAlign="Center" BackColor="#EEEEEE">
                     <div style="height:30px;padding:10px 5px 5px 5px;">Upload Excel( *.csv) :&nbsp; 
                         <asp:FileUpload ID="fileEDTUpload" runat="server" />
                         <asp:Button id="btnEdtSubmit" runat="server" Text="Submit" 
                             onclick="btnEdtSubmit_Click"/>
                        </div>
                     <div style="height:30px;padding:10px 5px 5px 5px;"> <asp:Label ID="lblSum" runat="server" Font-Bold="True" Font-Size="X-Large" 
                             ForeColor="#FF9933"></asp:Label> </div>
                     <div style="text-align:center;">
                         <asp:GridView ID="gvEdt" runat="server" BackColor="White" BorderColor="#CCCCCC" 
                             BorderStyle="None" BorderWidth="1px" CellPadding="3" Width="100%" >
                             <RowStyle ForeColor="#000066" />
                             <FooterStyle BackColor="White" ForeColor="#000066" />
                             <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                             <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                             <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                             
                         </asp:GridView>
                     </div>
                            </asp:Panel>
                    
            </td>
        </tr>
        <tr>
            <td colspan="2" bgcolor="White" align="center">
                &nbsp;
                &nbsp;
                <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="10pt" 
                    ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" bgcolor="White" align="center">
                &nbsp;
                &nbsp;
                <dxwgv:ASPxGridView ID="gvData" runat="server" 
                    onpageindexchanged="gvData_PageIndexChanged" Width="100%" 
                    onhtmlrowprepared="gvData_HtmlRowPrepared" AutoGenerateColumns="False" 
                    KeyFieldName="ui_projecttype" onrowcommand="gvData_RowCommand">
                     <SettingsBehavior ConfirmDelete="True" AllowSort="False" />
                    <SettingsPager PageSize="20">
                    </SettingsPager>
                    <Columns>
                                <dxwgv:GridViewDataButtonEditColumn Caption="Delete" EditFormSettings-Visible="False" VisibleIndex="0">
<EditFormSettings Visible="False"></EditFormSettings>
                                <DataItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" 
                                        ImageUrl="images/icon_trashcan.gif" 
                                        
                                        onclientclick="return window.confirm(&quot;Do you want to delete the row&quot;);" 
                                        CausesValidation="False" PostBackUrl="~/admin/newpage.aspx" />
                                </DataItemTemplate>
                            </dxwgv:GridViewDataButtonEditColumn>
                                
                            <dxwgv:GridViewDataTextColumn Caption="Edit" VisibleIndex="1">
    <DataItemTemplate>
        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Visible="False">Edit</asp:LinkButton>
    </DataItemTemplate>
                                </dxwgv:GridViewDataTextColumn>
                                
                                <dxwgv:GridViewDataTextColumn Caption="Select" VisibleIndex="2">
    <DataItemTemplate>
        <asp:LinkButton ID="btnSelect" runat="server" CommandName="Content" 
            Visible="True">Add Content</asp:LinkButton>
    </DataItemTemplate>
                                </dxwgv:GridViewDataTextColumn>
                                
                        <dxwgv:GridViewDataTextColumn Caption="Project Name" FieldName="ui_projecttype" 
                            VisibleIndex="3">
                        </dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn FieldName="title_local" Caption="SMS Text" VisibleIndex="4"></dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn Caption="Choose" FieldName="detail" 
                                    VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Answer" FieldName="detail_local" 
                                    VisibleIndex="6">
                                </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Create Date" FieldName="create_date" 
                            SortIndex="0" SortOrder="Descending" VisibleIndex="7">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Display Date" FieldName="display_date" 
                            SortIndex="1" SortOrder="Ascending" VisibleIndex="8">
                        </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="pcnt_id" FieldName="ui_pcnt_id" 
                                                        VisibleIndex="9">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="pcat_id" FieldName="pcat_id" 
                                                        VisibleIndex="10">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="sendnow" FieldName="sendnow" 
                                                        VisibleIndex="11">
                                                    </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="link" FieldName="title" 
                                                        VisibleIndex="12">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    
                                
                    </Columns>
                </dxwgv:ASPxGridView>

                <br/>

                                <asp:Panel ID="pnlEDTGame"  runat="server" Visible="false"  HorizontalAlign="Center" BackColor="#EEEEEE">
                    <iframe src="http://wap.isport.co.th/isportumvc/EDTQuizGame/" frameborder="0"  style="width:100%;height:500px"></iframe>
                    </asp:Panel>

            </td>
        </tr>
    </table>
    
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>