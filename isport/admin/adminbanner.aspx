<%@ Page Title="" Language="C#" MasterPageFile="~/admin/adminMaster.Master" AutoEventWireup="true" CodeBehind="adminbanner.aspx.cs" Inherits="isport.admin.adminbanner" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        img
        {
            max-height:200px;
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
<div>
    <table style="600: ;" cellpadding="3" cellspacing="3">
        <tr>
            <td align="right">
                &nbsp;
                App Name :</td>
            <td>
                <asp:DropDownList ID="cmbAppName" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="cmbAppName_SelectedIndexChanged">
                <asp:ListItem Value="">++Select++</asp:ListItem>
                <asp:ListItem Value="SportPool">SportPool</asp:ListItem>
                <asp:ListItem Value="SportArena">SportArena</asp:ListItem>
                <asp:ListItem Value="StarSoccer3GX">StarSoccer3GX</asp:ListItem>
                <asp:ListItem Value="StarSoccer">StarSoccer</asp:ListItem>
                <asp:ListItem Value="FeedNewToAis">FeedNewToAis</asp:ListItem>
                <asp:ListItem Value="FeedAis">FeedAis</asp:ListItem>
                <asp:ListItem Value="MobileLifeStyle">MobileLifeStyle</asp:ListItem>
                <asp:ListItem Value="SportPhone">SportPhone</asp:ListItem>
                <asp:ListItem Value="SportPhoneEntertainment">SportPhoneEntertainment</asp:ListItem>
                <asp:ListItem Value="FeedDtac">FeedDtac</asp:ListItem>
                <asp:ListItem Value="MTUTD">MTUTD</asp:ListItem>
                <asp:ListItem Value="KissModel">KissModel</asp:ListItem>
                </asp:DropDownList>
                &nbsp;
            </td>
            <td align="right">
                &nbsp;Operator :
            </td>
            <td>
                <asp:DropDownList ID="cmbOptCode" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="cmbOptCode_SelectedIndexChanged">
                    <asp:ListItem Value="all">all</asp:ListItem>
                    <asp:ListItem Value="01">AIS</asp:ListItem>
                    <asp:ListItem Value="02">DTAC</asp:ListItem>
                    <asp:ListItem Value="03">TRUE</asp:ListItem>
                    <asp:ListItem Value="05">3Gx</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                Model Type :</td>
            <td>
                <asp:DropDownList ID="cmbModel" runat="server">
                    <asp:ListItem>android</asp:ListItem>
                    <asp:ListItem>ipad</asp:ListItem>
                    <asp:ListItem>iphone</asp:ListItem>
                    <asp:ListItem>bb</asp:ListItem>
                    <asp:ListItem>window</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td align="right">
                Banner Type :</td>
            <td>
                <asp:DropDownList ID="cmbBannerType" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="cmbOptCode_SelectedIndexChanged">
                    <asp:ListItem>detail</asp:ListItem>
                    <asp:ListItem>footer</asp:ListItem>
                    <asp:ListItem>header</asp:ListItem>
                    <asp:ListItem>popup</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                Call Phone :</td>
            <td>
                <asp:TextBox ID="txtPhone" runat="server" Width="300px"></asp:TextBox>
            </td>
            <td align="right">
                Title :</td>
            <td>
                <asp:TextBox ID="txtTitle" runat="server" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
                &nbsp;
                Detail :</td>
            <td>
                <asp:TextBox ID="txtDetail" runat="server" Width="300px"></asp:TextBox>
            </td>
            <td align="right">
                &nbsp;
                Footer :</td>
            <td>
                <asp:TextBox ID="txtFooter" runat="server" Width="300px"></asp:TextBox>
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
                Image Size Big URL :</td>
            <td colspan="3">
                <asp:TextBox ID="txtBigURL" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                Image Size Medium URL :
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtMediumURL" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                Image Size Small URL :
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtSmallURL" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Button ID="btnInsert" runat="server" Text="Submit" 
                    onclick="btnInsert_Click" />
            </td>
        </tr>
    </table>
</div>
<div>
    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" ForeColor="#333333" GridLines="None" Width="90%" DataKeyNames="id" 
        onrowdeleting="gvData_RowDeleting" EnableModelValidation="True" OnRowUpdating="gvData_RowUpdating" OnRowCancelingEdit="gvData_RowCancelingEdit" OnRowEditing="gvData_RowEditing">
        <RowStyle BackColor="#EFF3FB" />
        <Columns>
            <asp:TemplateField HeaderText="Delete" ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                        CommandName="Delete" ImageUrl="~/admin/images/icon_trashcan.gif"  CommandArgument="ID"
                        onclientclick="return confirm('Are you sure you want to delete this record?');" 
                        Text="Delete" />
                </ItemTemplate>
                <ItemStyle Width="50px" />
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="True" />
            <asp:BoundField DataField="ID" HeaderText="ID Code" Visible="false" />
            <asp:TemplateField HeaderText="OPT Code">
                <EditItemTemplate>
                    <asp:TextBox ID="txtOptCode" runat="server" Text='<%# Bind("OPT_CODE") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("OPT_CODE") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="App Name">
                <EditItemTemplate>
                    <asp:TextBox ID="txtAppName" runat="server" Text='<%# Bind("app_name") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("app_name") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Banner Type">
                <EditItemTemplate>
                    <asp:TextBox ID="txtBannerType" runat="server" Text='<%# Bind("BANNER_TYPE") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("BANNER_TYPE") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="model_type" HeaderText="Model Type" />
            <asp:TemplateField HeaderText="Call">
                <EditItemTemplate>
                    <asp:TextBox ID="txtPhoneNo" runat="server" Text='<%# Bind("phone_no") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("phone_no") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Link">
                <EditItemTemplate>
                    <asp:TextBox ID="txtLinkEdit" runat="server" Text='<%# Bind("link") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("link") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="150px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Title">
                <EditItemTemplate>
                    <asp:TextBox ID="txtTitleEdit" runat="server" Text='<%# Bind("title") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("title") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle Width="100px" />
                <ItemStyle Width="100px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Detail">
                <EditItemTemplate>
                    <asp:TextBox ID="txtDetailEdit" runat="server" Text='<%# Bind("detail") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("detail") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle Width="100px" />
                <ItemStyle Width="100px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Big Image">
                <EditItemTemplate>
                    <asp:TextBox ID="txtBigImg" runat="server" Text='<%# Bind("big_img") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Image runat="server" ID="myImgL" ImageUrl='<%# Bind("big_img") %>' Width="100px"/>
                </ItemTemplate>
                <ItemStyle Width="100px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Medium Image">
                <EditItemTemplate>
                    <asp:TextBox ID="txtMediumImg" runat="server" Text='<%# Bind("medium_img") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                   <asp:Image runat="server" ID="myImgM" ImageUrl='<%# Bind("medium_img") %>' Width="100px"/>
                </ItemTemplate>
                <ItemStyle Width="100px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Small Image">
                <EditItemTemplate>
                    <asp:TextBox ID="txtSmallImg" runat="server" Text='<%# Bind("small_img") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Image runat="server" ID="myImgS" ImageUrl='<%# Bind("small_img") %>' Width="100px" />
                </ItemTemplate>
                <ItemStyle Width="100px" />
            </asp:TemplateField>
            <asp:BoundField DataField="create_date" HeaderText="Create date" />
        </Columns>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
    </div>
</asp:Content>
