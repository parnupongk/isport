<%--
{************************************************************************************}
{                                                                                    }
{   DO NOT MODIFY THIS FILE!                                                         }
{                                                                                    }
{   It will be overwritten without prompting when a new version becomes              }
{   available. All your changes will be lost.                                        }
{                                                                                    }
{   This file contains the default template and is required for the form             }
{   rendering. Improper modifications may result in incorrect behavior of            }
{   dialog forms.                                                                    }
{                                                                                    }
{************************************************************************************}
--%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InsertLinkForm.ascx.cs" Inherits="InsertLinkFrom" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v8.1" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dxhe" %>
<%@ Register Assembly="DevExpress.Web.v8.1" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>

<dxp:ASPxPanel ID="MainPanel" runat="server" Width="100%" DefaultButton="btnOk">
    <PanelCollection>
        <dxp:PanelContent runat="server">
    <table cellpadding="0" cellspacing="0" id="insertLinkForm">
        <tr>
            <td class="contentCell">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="typeRadionButtonListCell">
                            <dxe:ASPxRadioButtonList ID="rblLinkType" runat="server" ItemSpacing="22px" RepeatDirection="Horizontal"
                                SelectedIndex="0" ClientInstanceName="_dxeRblLinkType">
                                <clientsideevents selectedindexchanged="function(s, e) { OnTypeLinkChanged__InsertLinkForm(s); }" />
                                <items>
                                    <dxe:ListEditItem Text="URL" Value="URL" />
                                    <dxe:ListEditItem Text="E-mail address" Value="Email" />
                                </items>
                                <border borderstyle="None" />
                                <paddings paddingleft="0px" paddingtop="0px" paddingbottom="0px" />
                            </dxe:ASPxRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dxhe:ASPxHtmlEditorRoundPanel ID="rpInsertLink" runat="server">
                                <panelcollection>
                                    <dxhe:HtmlEditorRoundPanelContent runat="server">
                                        <!-- URL -->
                                        <table cellpadding="0" cellspacing="0" id="_dxeURLArea">
                                            <tr>
                                                <td class="captionCell">                        
                                                    <dxe:ASPxLabel ID="lblUrl" runat="server" Text="URL:" AssociatedControlID="txbURL">
                                                    </dxe:ASPxLabel>                                
                                                </td>
                                                <td class="inputCell">                        
                                                    <dxe:ASPxTextBox ID="txbURL" ClientInstanceName="_dxeTxbURL" runat="server" Width="274px" AutoCompleteType="Disabled">
                                                        <ValidationSettings ErrorDisplayMode="Text" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidateOnLeave="False">
                                                            <RequiredField IsRequired="True" ErrorText="This field is required" />                                                                    
                                                            <ErrorFrameStyle Font-Size="10px">
                                                            </ErrorFrameStyle>
                                                        </ValidationSettings>
                                                    </dxe:ASPxTextBox>                                
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0" id="_dxeEmailArea" style="display: none;">
                                            <tr>
                                                <td class="captionCell">                        
                                                    <dxe:ASPxLabel ID="lblEmailTo" runat="server" Text="E-mail to:" AssociatedControlID="txbEmailTo">
                                                    </dxe:ASPxLabel>                                
                                                </td>
                                                <td class="inputCell">                        
                                                    <dxe:ASPxTextBox ID="txbEmailTo" ClientInstanceName="_dxeTxbEmailTo" runat="server" Width="250px" AutoCompleteType="Disabled">
                                                        <ValidationSettings ErrorDisplayMode="Text" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidateOnLeave="False">
                                                            <RegularExpression ErrorText="Ivalid e-mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                            <ErrorFrameStyle Font-Size="10px">
                                                            </ErrorFrameStyle>
                                                            <RequiredField IsRequired="True" ErrorText="This field is required" />
                                                        </ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                </td>                        
                                            </tr>
                                            <tr>
                                                <td class="captionCell">                        
                                                    <dxe:ASPxLabel ID="lblSubject" runat="server" Text="Subject:" AssociatedControlID="txbSubject">
                                                    </dxe:ASPxLabel>                                
                                                </td>
                                                <td class="inputCell">                        
                                                    <dxe:ASPxTextBox ID="txbSubject" ClientInstanceName="_dxeTxbSubject" runat="server" Width="250px" AutoCompleteType="Disabled">
                                                    </dxe:ASPxTextBox>                                
                                                </td>                        
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td colspan="2" class="displayPropertiesCell">
                                                    <dxe:ASPxLabel ID="lblLinkDisplay" runat="server" Text="Display Properties:">
                                                    </dxe:ASPxLabel>                                                            
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="captionCell">                        
                                                    <dxe:ASPxLabel ID="lblText" ClientInstanceName="_dxeLblText" runat="server" Text="Text:" AssociatedControlID="txbText">
                                                    </dxe:ASPxLabel>
                                                </td>
                                                <td class="inputCell">                        
                                                    <dxe:ASPxTextBox ID="txbText" ClientInstanceName="_dxeTxbText" runat="server" Width="258px" AutoCompleteType="Disabled">
                                                    </dxe:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" class="separatorCell"></td>
                                            </tr>
                                            <tr>
                                                <td class="captionCell">                        
                                                    <dxe:ASPxLabel ID="lblToolTip" runat="server" Text="ToolTip:" AssociatedControlID="txbToolTip">
                                                    </dxe:ASPxLabel>
                                                </td>
                                                <td class="inputCell">
                                                    <dxe:ASPxTextBox ID="txbToolTip" ClientInstanceName="_dxeTxbToolTip" runat="server" Width="258px" AutoCompleteType="Disabled">
                                                    </dxe:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </dxhe:HtmlEditorRoundPanelContent>
                                </panelcollection>
                            </dxhe:ASPxHtmlEditorRoundPanel>
                        </td>
                    </tr>
                    <tr>
                        <td id="_dxeTargetArea" class="targetCheckBoxCell">
                            <dxe:ASPxCheckBox ID="ckbOpenInNewWindow" ClientInstanceName="_dxeCkbOpenInNewWindow"
                                runat="server" Text="Open in new window">
                            </dxe:ASPxCheckBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="buttonsCell" align="right">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <dxe:ASPxButton ID="btnOk" runat="server" Text="OK" AutoPostBack="False" Height="23px"
                                Width="74px" ClientInstanceName="_dxeBtnOk">
                                <clientsideevents click="function(s, e) {OnOkButtonClick_InsertLinkForm();}" />
                            </dxe:ASPxButton>
                            <dxe:ASPxButton ID="btnChange" runat="server" Text="Change" AutoPostBack="False"
                                Height="23px" Width="74px" ClientInstanceName="_dxeBtnChange" ClientVisible="False">
                                <clientsideevents click="function(s, e) { OnOkButtonClick_InsertLinkForm(); }" />
                            </dxe:ASPxButton>
                        </td>
                        <td class="cancelButton">
                            <dxe:ASPxButton ID="btnCancel" runat="server" Text="Cancel" AutoPostBack="False"
                                Height="23px" Width="74px">
                                <clientsideevents click="function(s, e) {OnCancelButtonClick_InsertLinkForm();}" />
                            </dxe:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
         </dxp:PanelContent>
    </PanelCollection>
</dxp:ASPxPanel>

<script type="text/javascript" id="dxss_InsertLinkForm">
    function OnOkButtonClick_InsertLinkForm() {
        if (IsValidFields_InsertLinkForm())
            aspxDialogComplete(1, GetDialogData_InsertLinkForm());
    }
    function OnCancelButtonClick_InsertLinkForm() {
        aspxDialogComplete(0, GetDialogData_InsertLinkForm());
    }
    
    function OnTypeLinkChanged__InsertLinkForm(radioButtonList) {
        var value = radioButtonList.GetValue();
        var urlArea = _aspxGetElementById("_dxeURLArea");
        var emailArea = _aspxGetElementById("_dxeEmailArea");
        var targetArea = _aspxGetElementById("_dxeTargetArea");
                
        _aspxSetElementDisplay(urlArea, value == "URL");
        _aspxSetElementDisplay(targetArea, value == "URL");
        
        _aspxSetElementDisplay(emailArea, value == "Email");        
    }
    function IsValidFields_InsertLinkForm() {
        var ret = true;
        if (_dxeTxbEmailTo.IsVisible()) {
            _dxeTxbEmailTo.Validate();
            ret = ret && _dxeTxbEmailTo.GetIsValid();
        }
            
        if (_dxeTxbURL.IsVisible()) {
            _dxeTxbURL.Validate();
            ret = ret && _dxeTxbURL.GetIsValid();
        }
        return ret;
    }
    function GetDialogData_InsertLinkForm() {
        var res = new Object();        
        res.subject = "";
        res.target = "";
        res.text = "";
        res.title = "";
        res.url = "";
        res.isCheckedOpenInNewWindow = false;
        
        res.isCheckedOpenInNewWindow = _dxeCkbOpenInNewWindow.GetValue();
        res.url = _dxeRblLinkType.GetValue() == "Email" ? _dxeTxbEmailTo.GetValue() : _dxeTxbURL.GetValue();
        res.subject = _dxeTxbSubject.GetValue();
        res.text = _dxeTxbText.GetText();
        res.title = _dxeTxbToolTip.GetText();
        
        return res;
    }
</script>