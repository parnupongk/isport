<%@ Page Title="" Language="C#" MasterPageFile="~/admin/adminMaster.Master" AutoEventWireup="true" CodeBehind="adminshortener.aspx.cs" Inherits="isport.admin.adminshortener" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentDetail" runat="server">
    <table style="width:800px;" cellpadding="3" cellspacing="3">
        <tr>
            <td align="right">
                Operator&nbsp; :</td>
            <td align="left">
                <asp:DropDownList ID="cmbOptCode" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="cmbOptCode_SelectedIndexChanged">
                    <asp:ListItem Selected="True">++Selected++</asp:ListItem>
                    <asp:ListItem Value="99">Other</asp:ListItem>
                    <asp:ListItem Value="01">AIS</asp:ListItem>
                    <asp:ListItem Value="02">DTAC</asp:ListItem>
                    <asp:ListItem Value="03">TRUE</asp:ListItem>
                    <asp:ListItem Value="05">3Gx</asp:ListItem>
                    
                </asp:DropDownList>
                &nbsp;
            </td>
            <td align="left">
                &nbsp;</td>
            <td align="left">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right">
                Start Date :</td>
            <td align="left">
                <asp:TextBox ID="txtStartDate" runat="server" Height="20px" 
                                Width="139px"></asp:TextBox>
                            <asp:CalendarExtender
                                ID="CalendarExtender1" runat="server" Format="yyyyMMdd" TargetControlID="txtStartDate">
                            </asp:CalendarExtender></td>
            <td align="right">
                End Date :</td>
            <td align="left">
                <asp:TextBox ID="txtEndDate" runat="server" Height="20px" 
                                Width="139px"></asp:TextBox>
                            <asp:CalendarExtender
                                ID="CalendarExtender2" runat="server" Format="yyyyMMdd" TargetControlID="txtEndDate">
                            </asp:CalendarExtender></td></td>
        </tr>
        <tr>
            <td align="right">
                Parameter :</td>
            <td align="left">
                <asp:TextBox ID="txtParameter" runat="server" Width="300px">s</asp:TextBox>
            </td>
            <td align="left">
                &nbsp;
                &nbsp;
                Parameter Value :</td>
            <td align="left">
                <asp:TextBox ID="txtParameterValue" runat="server" Width="300px"></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td align="right">
                URL Link :</td>
            <td align="left" colspan="3">
                <asp:TextBox ID="txtLink" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                Description :</td>
            <td align="left" colspan="3">
                <asp:TextBox ID="txtDesc" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                Service Id</td>
            <td align="left" colspan="3">
                <asp:TextBox ID="txtPssvId" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                Service Name :</td>
            <td align="left" colspan="3">
                <asp:TextBox ID="txtPssvName" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <asp:Button ID="btnInsert" runat="server" Text="Submit" 
                    onclick="btnInsert_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" DataKeyNames="URL_ID" ForeColor="#333333" GridLines="None" 
        onrowdeleting="gvData_RowDeleting" Width="90%" EnableModelValidation="True" OnRowCancelingEdit="gvData_RowCancelingEdit" OnRowEditing="gvData_RowEditing" OnRowUpdating="gvData_RowUpdating">
        <RowStyle BackColor="#EFF3FB" />
        <Columns>
            <asp:TemplateField HeaderText="Delete" ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                        CommandArgument="ID" CommandName="Delete" 
                        ImageUrl="~/admin/images/icon_trashcan.gif" 
                        onclientclick="return confirm('Are you sure you want to delete this record?');" 
                        Text="Delete" />
                </ItemTemplate>
                <ItemStyle Width="50px" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="True" />
            <asp:BoundField DataField="URL_ID" HeaderText="ID Code" Visible="false" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="OPT Code">
                <EditItemTemplate>
                    <asp:TextBox ID="txtOptCode" runat="server" Text='<%# Bind("OPT_CODE") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("OPT_CODE") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="PARAMETER" HeaderText="Parameter" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Parameter value">
                <EditItemTemplate>
                    <asp:TextBox ID="txtParaValue" runat="server" Text='<%# Bind("PARAMETER_VALUE") %>' Width="131px"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("PARAMETER_VALUE") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
                        <asp:TemplateField HeaderText="Service ID">
                <EditItemTemplate>
                    <asp:TextBox ID="txtPssvId" runat="server" Text='<%# Bind("PSSV_ID") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblPssvId" runat="server" Text='<%# Bind("PSSV_ID") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Service Name">
                <EditItemTemplate>
                    <asp:TextBox ID="txtPssvName" runat="server" Text='<%# Bind("PSSV_NAME") %>' Width="320px"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblPssvName" runat="server" Text='<%# Bind("PSSV_NAME") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="URL">
                <EditItemTemplate>
                    <asp:TextBox ID="txtURL" runat="server" Text='<%# Bind("URL") %>' Width="395px"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("URL") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description">
                <EditItemTemplate>
                    <asp:TextBox ID="txtDesc" runat="server" Text='<%# Bind("description") %>' Width="320px"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("description") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>

                        <asp:BoundField DataField="START_DATE" HeaderText="Start Date" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="END_DATE" HeaderText="End Date">
            <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
        </Columns>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
    <br/>
    
</asp:Content>
