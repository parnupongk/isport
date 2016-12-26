<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rep_channel.aspx.cs" Inherits="IsportApp_Report.rep_channel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- Bootstrap core CSS -->
    <link href="dist/css/bootstrap.css" rel="stylesheet">

    <!-- IE10 viewport hack for Surface/desktop Windows 8 bug -->
    <link href="assets/css/ie10-viewport-bug-workaround.css" rel="stylesheet">

    <!-- Just for debugging purposes. Don't actually copy these 2 lines! -->
    <!--[if lt IE 9]><script src="../../assets/js/ie8-responsive-file-warning.js"></script><![endif]-->
    <script src="assets/js/ie-emulation-modes-warning.js"></script>

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <style>
       .ajax__myTab .ajax__tab_header {  
    font-family: verdana,tahoma,helvetica;  
    font-size: 11px;  
    border-bottom: solid 1px #999999;  
    margin:20px;
}  
  
.ajax__myTab .ajax__tab_outer {  
    padding-right: 4px;  
    height: 21px;  
    background-color: #C0C0C0;  
    margin-right: 2px;  
    border-right: solid 1px #666666;  
    border-top: solid 1px #aaaaaa;  
}  
  
.ajax__myTab .ajax__tab_inner {  
    padding-left: 3px;  
    background-color: #C0C0C0;  
}  
  
.ajax__myTab .ajax__tab_tab {  
    height: 20px;  
    padding: 4px;  
    margin: 0;  
}  
  
.ajax__myTab .ajax__tab_hover .ajax__tab_outer {  
    background-color: #cccccc;  
}  
  
.ajax__myTab .ajax__tab_hover .ajax__tab_inner {  
    background-color: #cccccc;  
}  
  
.ajax__myTab .ajax__tab_hover .ajax__tab_tab {  
}  
  
.ajax__myTab .ajax__tab_active .ajax__tab_outer {  
    background-color: #fff;  
    border-left: solid 1px #999999;  
}  
  
.ajax__myTab .ajax__tab_active .ajax__tab_inner {  
    background-color: #fff;  
}  
  
.ajax__myTab .ajax__tab_active .ajax__tab_tab {  
}  
  
.ajax__myTab .ajax__tab_body {  
    font-family: verdana,tahoma,helvetica;  
    font-size: 10pt;  
    border: 1px solid #999999;  
    border-top: 0;  
    padding: 8px;  
    background-color: #ffffff;  
}  
    </style>
</head>
<body bgcolor="#DBE8E8">
        <script type="text/javascript">
        function alertMessage(str) {
            alert(str);

        }

    </script>
    <form id="form1" runat="server">
        <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajax:ToolkitScriptManager>



        <ajax:TabContainer runat="server" ID="Tabs" Height="190px" ActiveTabIndex="0" Width="100%" CssClass="ajax__myTab" >
            <ajax:TabPanel runat="server" ID="Panel1" HeaderText="Report By Service Sub."  >
                <ContentTemplate>
                    <div align="center">

                        <table style="width: 600px;" cellpadding="4" cellspacing="4">
                            <tr style="padding-top:10px;">
                                <td>Service No :</td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlSipService" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="padding-top:10px;">
                                <td>Start Date</td>
                                <td style="padding-top:10px;">
                                    <asp:TextBox ID="txtServiceStartDate" CssClass="form-control" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="CalendarExtender3" PopupButtonID="txtServiceStartDate" runat="server" PopupPosition="Right" TargetControlID="txtServiceStartDate" Enabled="True"></ajax:CalendarExtender>

                                </td>
                                <td>to</td>
                                <td style="padding-top:10px;">
                                    <asp:TextBox ID="txtServiceEndDate" CssClass="form-control" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="CalendarExtender4" PopupButtonID="txtServiceEndDate" runat="server" PopupPosition="Right" TargetControlID="txtServiceEndDate" Enabled="True">
                                    </ajax:CalendarExtender>
                                </td>
                            </tr>
                            <tr style="padding-top:10px;">
                                <td colspan="4" align="center" style="padding-top:10px;">
                                    <asp:Button ID="btnServiceSubmit" runat="server" CssClass="btn" Text="Submit" OnClick="btnServiceSubmit_Click" />
                                </td>
                            </tr>
                        </table>

                    </div>



                </ContentTemplate>

            </ajax:TabPanel>
            <ajax:TabPanel runat="server" ID="Panel2" HeaderText="Report By Channel Sub."  >
                <ContentTemplate>
                    <div align="center">

                        <table style="width: 800px;" cellpadding="4" cellspacing="4">
                            <tr style="padding-top:10px;">
                                <td>Channel Subscribe :</td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlMpCode" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </td>

                            </tr>
                            <tr style="padding-top:10px;">
                                <td>Service No :</td>
                                <td colspan="3" style="padding-top:10px;">
                                    <asp:DropDownList ID="ddlMpCodeService" runat="server" CssClass="form-control">
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr style="padding-top:10px;">
                                <td>Start Date</td>
                                <td style="padding-top:10px;">
                                    <div>
                                    <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="CalendarExtender1" PopupButtonID="txtStartDate"  PopupPosition="Right" runat="server" TargetControlID="txtStartDate" Enabled="True">
                                    </ajax:CalendarExtender>
                                        </div>

                                </td>
                                <td>to</td>
                                <td style="padding-top:10px;">
                                    <div>
                                    <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="CalendarExtender2" PopupButtonID="txtEndDate" PopupPosition="Right" runat="server" TargetControlID="txtEndDate" Enabled="True">
                                    </ajax:CalendarExtender>
                                        </div>
                                </td>
                            </tr>
                            <tr style="padding-top:10px;">
                                <td colspan="4" align="center" style="padding-top:10px;">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn" OnClick="btnSubmit_Click1"
                                        Text="Submit" />
                                </td>
                            </tr>
                        </table>

                    </div>



                </ContentTemplate>

            </ajax:TabPanel>
        </ajax:TabContainer>

        <div id="divChart" align="center">
            <ajax:LineChart ID="LineChart1" runat="server"
                ChartWidth="1000" ChartHeight="250" ChartType="Basic"
                ChartTitle="NewSub Subscribe"
                ChartTitleColor="#0E426C" >
                <Series>
                    <ajax:LineChartSeries Name="AIS" LineColor="#00FF99"></ajax:LineChartSeries>
                    <ajax:LineChartSeries Name="Dtac" LineColor="#3399FF"></ajax:LineChartSeries>
                    <ajax:LineChartSeries Name="TrueH" LineColor="#FF0000"></ajax:LineChartSeries>
                    <ajax:LineChartSeries Name="Sum" LineColor="#000000"></ajax:LineChartSeries>
                </Series>
            </ajax:LineChart>

        </div>

        <div align="center">

            <asp:GridView ID="gvData" runat="server" CellPadding="3" AutoGenerateColumns="False" Width="80%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" ShowFooter="True" OnRowDataBound="gvData_RowDataBound">
                <RowStyle ForeColor="#000066" />
                <FooterStyle BackColor="#F7A3A7" ForeColor="#000066" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" Height="50px"  />
                <Columns>
                    <asp:BoundField DataField="rep_date" HeaderText="Date">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="pssv_id" HeaderText="Service ID">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Service Name">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("pssv_name") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("pssv_name") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="Label1" runat="server" Text='Sum = '></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AIS">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ais") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAis" runat="server" Text='<%# Bind("ais") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblSumAis" runat="server" Text=''></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AIS Cancel">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("total_cancel_01") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCancelAis" runat="server" Text='<%# Bind("total_cancel_01") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblSumCancelAis" runat="server" Text=''></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" Width="20px" />
                        <HeaderStyle HorizontalAlign="Center"  Width="20px" />
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dtac">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("dtac") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDtac" runat="server" Text='<%# Bind("dtac") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblSumDtac" runat="server" Text=''></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dtac Cancel">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("total_cancel_02") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCancelDtac" runat="server" Text='<%# Bind("total_cancel_02") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblSumCancelDtac" runat="server" Text=''></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" Width="20px" />
                        <HeaderStyle HorizontalAlign="Center"  Width="20px" />
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="True-H">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("truemoveH") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblTrue" runat="server" Text='<%# Bind("truemoveH") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblSumTrue" runat="server" Text=''></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TrueH Cancel">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("total_cancel_04") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCancelTrueH" runat="server" Text='<%# Bind("total_cancel_04") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblSumCancelTrueH" runat="server" Text=''></asp:Label>
                        </FooterTemplate>
                       <FooterStyle HorizontalAlign="Center" Width="20px" />
                        <HeaderStyle HorizontalAlign="Center"  Width="20px" />
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="3GX">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("_3gx") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl3Gx" runat="server" Text='<%# Bind("_3gx") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblSum3Gx" runat="server" Text=''></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="3Gx Cancel" ControlStyle-Width="20px">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("total_cancel_05") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCancel3gx" runat="server" Text='<%# Bind("total_cancel_05") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblSumCancel3Gx" runat="server" Text=''></asp:Label>
                        </FooterTemplate>

                        <FooterStyle HorizontalAlign="Center" Width="20px" />
                        <HeaderStyle HorizontalAlign="Center"  Width="20px" />
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#F7A3A7" Font-Bold="True" ForeColor="#000066" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

        </div>

        <div align="center">

            <asp:GridView ID="gvServiceNo" runat="server" CellPadding="3" AutoGenerateColumns="False" Width="80%" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" OnRowDataBound="gvServiceNo_RowDataBound" ShowFooter="True">
                <RowStyle ForeColor="#000066" />
                <FooterStyle BackColor="#FF9999" ForeColor="#000066" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" Height="50px" />
                <Columns>
                    <asp:BoundField DataField="rep_date" HeaderText="Date" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="mp_desc" HeaderText="Channel Des." HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="opt_code" HeaderText="OPT" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Subscribe">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("new_sub") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblSubNo" runat="server" Text='<%# Bind("new_sub") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblSumServiceNo" runat="server" Text=''></asp:Label>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cancel">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("total_cancel_01") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCancelSubNo" runat="server" Text='<%# Bind("total_cancel_01") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblSumCancelServiceNo" runat="server" Text=''></asp:Label>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </div>


    </form>

    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="assets/js/vendor/jquery.min.js"><\/script>')</script>
    <script src="dist/js/bootstrap.min.js"></script>
    <!-- IE10 viewport hack for Surface/desktop Windows 8 bug -->
    <script src="assets/js/ie10-viewport-bug-workaround.js"></script>


</body>
</html>
