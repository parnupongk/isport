<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="isport.admin.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link href=Styles_th.css rev=Stylesheet rel=Stylesheet  />
    <style type="text/css">
        .style1
        {
            height: 42px;
            color: #FFFFFF;
            font-size: large;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div >
    
        
    
        <table style="width:400px;" align="center" bgcolor="#3333CC" cellpadding="5" 
            cellspacing="5">
            <tr>
                <td align="center" class="style1" colspan="2">
                    Login</td>
            </tr>
            <tr>
                <td align="right" bgcolor="White">
                    User Name :</td>
                <td bgcolor="White">
                    <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" bgcolor="White">
                    Password :</td>
                <td bgcolor="White">
                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" bgcolor="White" colspan="2">
                    <asp:Button ID="btnSubmit" runat="server" onclick="btnSubmit_Click" 
                        Text="Submit" />
                </td>
            </tr>
        </table>
    
        
    
    </div>
    </form>
</body>
</htm>
