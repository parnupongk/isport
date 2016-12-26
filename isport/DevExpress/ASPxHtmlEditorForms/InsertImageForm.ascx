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
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InsertImageForm.ascx.cs" Inherits="InsertImageForm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v8.1" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dxhe" %>
<%@ Register Assembly="DevExpress.Web.v8.1" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>
<%@ Register Src="ImagePropertiesForm.ascx" TagName="ImagePropertiesForm" TagPrefix="ucip" %>

<dxp:ASPxPanel ID="Panel1" runat="server" Width="100%" DefaultButton="btnInsertImage">
    <PanelCollection>
        <dxp:PanelContent runat="server">
    <table cellpadding="0" cellspacing="0" id="insertImageForm">
        <tr>
            <td class="contentCell">    
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="typeRadionButtonListCell">
                            <table cellpadding="0" cellspacing="0" class="radioButtonTable">
                                <tr>
                                    <td>
                                        <dxe:ASPxRadioButton ID="rblFromTheWeb" runat="server" GroupName="InsertImageFormGroup" Checked="True" Text="From the web (URL)" TextWrap="False" ClientInstanceName="_dxeRblImageFromTheWeb">
                                            <ClientSideEvents CheckedChanged="function(s, e) { OnImageFromTypeChanged__InsertImageForm(s); }" />
                                        </dxe:ASPxRadioButton>                                    
                                    </td>
                                    <td>
                                        <dxe:ASPxRadioButton ID="rblFromThisComputer" runat="server" GroupName="InsertImageFormGroup" Text="From this computer" TextWrap="False" ClientInstanceName="_dxeRblImageFromThisComputer">
                                            <ClientSideEvents CheckedChanged="function(s, e) { OnImageFromTypeChanged__InsertImageForm(s); }" />
                                        </dxe:ASPxRadioButton>                                    
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>        
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" style="width:100%;">
                                <tr>
                                    <td style="vertical-align:top">
                                        <dxhe:ASPxHtmlEditorRoundPanel ID="rpInsertImage" runat="server" Width="315px">
                                            <PanelCollection>
                                                <dxhe:HtmlEditorRoundPanelContent runat="server">
                                                    <!-- FromTheWeb -->
                                                    <table cellpadding="0" cellspacing="0" id="_dxeFromTheWeb" class="fromTheWeb" >
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="lblImageUrl" runat="server" Text="Enter image web address:" AssociatedControlID="txbInsertImageUrl">
                                                                </dxe:ASPxLabel>                            
                                                                <div class="captionIndent"></div>
                                                                <dxe:ASPxTextBox ID="txbInsertImageUrl" ClientInstanceName="_dxeTbxInsertImageUrl" runat="server" Width="274px" AutoCompleteType="Disabled">
                                                                    <ClientSideEvents TextChanged="function(s, e) { aspxInsertImageSrcValueChanged(s.GetText()); }" />
                                                                    <ValidationSettings ErrorDisplayMode="Text" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidateOnLeave="False">
                                                                        <RequiredField IsRequired="True" ErrorText="This field is required" />                                                                    
                                                                        <ErrorFrameStyle Font-Size="10px">
                                                                        </ErrorFrameStyle>
                                                                    </ValidationSettings>
                                                                </dxe:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="saveToServerCheckBoxCell">
                                                                <dxe:ASPxCheckBox ID="ckbSaveToServer" ClientInstanceName="_dxeCkbSaveToServer"
                                                                    runat="server" Text="Save file to this server">
                                                                </dxe:ASPxCheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="imagePreview"> 
                                                                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                                                    <tr>
                                                                        <td class="imagePreviewCell">
                                                                            <span id="dxInsertImagePreviewText">Image preview</span>
                                                                            <img src="" id="dxInsertImagePreviewImage" style="display:none;" width="180" height="100" alt="Preview Image"/>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <!-- FromThisComputer -->                    
                                                    <table cellpadding="0" cellspacing="0" id="_dxeFromThisComputer" style="display: none;">
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="lblBrowse" runat="server" Text="Browse your computer for the image file to upload:" AssociatedControlID="uplImage">
                                                                </dxe:ASPxLabel>
                                                                <div class="captionIndent"></div>                                                                                
                                                                <dxhe:ASPxHtmlEditorUploadControl ID="uplImage" runat="server" ClientInstanceName="_dxeUplImage"
                                                                    OnFileUploadComplete="uplImage_FileUploadComplete">
                                                                    <ClientSideEvents FileUploadComplete="function(s, e) { aspxImageUploadComplete(e) }"
                                                                    />
                                                                </dxhe:ASPxHtmlEditorUploadControl>                                                                    
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </dxhe:HtmlEditorRoundPanelContent>
                                            </PanelCollection>
                                        </dxhe:ASPxHtmlEditorRoundPanel>                                        
                                    </td>
                                    <%-- Image Properties --%>
                                    <td id="_dxeMoreImagePropertiesRow" class="imagePropertiesCell" style="display:none">
                                        <ucip:ImagePropertiesForm ID="ucImagePropertiesForm" runat="server" />                            
                                    </td>                            
                                </tr>
                            </table>                                                
                        </td>
                    </tr>
                    <tr>
                        <td class="moreOptionsCell">
                            <dxe:ASPxCheckBox ID="ckbMoreImageOptions" ClientInstanceName="CkbMoreImageOptions" runat="server" Text="More options">
                                <ClientSideEvents CheckedChanged="function(s, e) { OnMoreImageOptionsCheckedChanged(s.GetChecked()) }" />                    
                            </dxe:ASPxCheckBox>            
                        </td>
                    </tr>
                </table>                            
            </td>        
        </tr>
        <tr>
            <%-- Button Cells --%>
            <td class="buttonsCell" align="right">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <dxe:ASPxButton ID="btnInsertImage" runat="server" Height="26px" Text="Insert" Width="82px"
                                Autopostback="false" ClientInstanceName="_dxeBtnInsertImage">
                                <clientsideevents click="function(s, e) { OnOkButtonClick_InsertImageForm(); }" />
                            </dxe:ASPxButton>
                            <dxe:ASPxButton ID="btnChangeImage" runat="server" Text="Change" AutoPostBack="False" Height="23px"
                                Width="74px" ClientInstanceName="_dxeBtnChangeImage" ClientVisible="False">
                                <clientsideevents click="function(s, e) { OnOkButtonClick_InsertImageForm(); }" />
                            </dxe:ASPxButton>
                        </td>
                        <td class="cancelButton">
                            <dxe:ASPxButton ID="btnInsertImageCancel" runat="server" Height="26px" Text="Cancel"
                                Width="82px" Autopostback="false">
                                <clientsideevents click="function(s, e) { OnCancelButtonClick_InsertImageForm(); }" />
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
    function GetDialogData_InsertImageForm() {
        var res = new Object();
        res.width = null;
        res.height = null;
        
        res.src = _dxeTbxInsertImageUrl.GetText();
        var isOriginal = _dxeCmbSize.GetValue().toLowerCase() == "original";
        if (!isOriginal)
            res.width = _dxeSpnWidth.GetValue();
        if (!isOriginal)
            res.height = _dxeSpnHeight.GetValue();
        res.align = _dxeCmbImagePosition.GetValue();
        res.alt = _dxeTxbDescription.GetText();
        return res;
    }
    function IsValidFields_InsertImageForm() {
        var ret = true;    
        if (_dxeTbxInsertImageUrl.IsVisible()) {
            _dxeTbxInsertImageUrl.Validate();
            ret = ret && _dxeTbxInsertImageUrl.GetIsValid();
        }
        else
            ret = ret && (_dxeUplImage.GetText() != "");
        return ret;
    }
    function OnOkButtonClick_InsertImageForm() {
        if (IsValidFields_InsertImageForm())        
            aspxDialogComplete(1, GetDialogData_InsertImageForm());
    }
    function OnCancelButtonClick_InsertImageForm() {
        aspxDialogComplete(0, GetDialogData_InsertImageForm());
    }
    function OnMoreImageOptionsCheckedChanged(isChecked) {
        _aspxSetElementDisplay(_aspxGetElementById("_dxeMoreImagePropertiesRow"), isChecked);
        if (isChecked)
            aspxAdjustControlsSizeInDialogWindow();
    }        
    function OnImageFromTypeChanged__InsertImageForm(radioButtonList) {
        var value = radioButtonList.GetValue();
        var fromWebArea = _aspxGetElementById("_dxeFromTheWeb");
        var fromThisComputerArea = _aspxGetElementById("_dxeFromThisComputer");
        _aspxSetElementDisplay(fromWebArea, _dxeRblImageFromTheWeb.GetChecked());
        _aspxSetElementDisplay(fromThisComputerArea, _dxeRblImageFromThisComputer.GetChecked());
    }
</script>