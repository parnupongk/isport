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
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ImagePropertiesForm.ascx.cs" Inherits="ImagePropertiesForm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<table cellpadding="0" cellspacing="0" id="imagePropertiesForm">
    <tr>
        <td>
            <dxe:ASPxLabel ID="lblSize" runat="server" Text="Size:"></dxe:ASPxLabel>
            <div class="captionIndent"></div>            
            <dxe:ASPxComboBox ID="cmbSize" runat="server" SelectedIndex="0" ValueType="System.String" ClientInstanceName="_dxeCmbSize">
                <Items>
                    <dxe:ListEditItem Text="Original image size" Value="original" />
                    <dxe:ListEditItem Text="Custom size" Value="custom" />
                </Items>
                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCmbSizeSelectedIndexChanged(s); }" />                
            </dxe:ASPxComboBox>
        </td>
    </tr>
    <tr id="_dxeSizePropertiesRow" style="display:none;">
        <td class="imageSizeEditorsCell">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td class="captionCell">
                        <dxe:ASPxLabel ID="lblWidth" runat="server" Text="Width:"></dxe:ASPxLabel></td>
                    <td>
                        <dxe:ASPxSpinEdit ID="spnWidth" ClientInstanceName="_dxeSpnWidth" runat="server" Height="21px" Number="1" Width="52px" AllowNull="False" Increment="0" LargeIncrement="0" NumberType="Integer" MaxValue="10000" MinValue="1">
                            <SpinButtons ShowIncrementButtons="False">
                            </SpinButtons>
                        </dxe:ASPxSpinEdit>                        
                    </td>
                    <td class="pixelSizeCell"><dxe:ASPxLabel ID="lblPixelWidth" runat="server" Text="pixels"></dxe:ASPxLabel></td>
                </tr>
                <tr>
                    <td colspan="3" class="separatorCell"></td>
                </tr>
                <tr>
                    <td class="captionCell"><dxe:ASPxLabel ID="lblHeight" runat="server" Text="Height:"></dxe:ASPxLabel></td>
                    <td>
                        <dxe:ASPxSpinEdit ID="spnHeight" ClientInstanceName="_dxeSpnHeight" runat="server" Height="21px" Number="1" Width="52px" AllowNull="False" Increment="0" LargeIncrement="0" MinValue="1" NumberType="Integer" MaxValue="10000">
                            <SpinButtons ShowIncrementButtons="False">
                            </SpinButtons>
                        </dxe:ASPxSpinEdit>                    
                    </td>
                    <td class="pixelSizeCell"><dxe:ASPxLabel ID="lblPixelHeight" runat="server" Text="pixels"></dxe:ASPxLabel></td> 
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <div class="fieldSeparator"></div>        
        </td>
    </tr>
    <tr>
        <td>
            <dxe:ASPxLabel ID="lblImagePosition" runat="server" Text="Position:"></dxe:ASPxLabel>        
            <div class="captionIndent"></div>                        
            <dxe:ASPxComboBox ID="cmbImagePosition" ClientInstanceName="_dxeCmbImagePosition" runat="server" SelectedIndex="0" ValueType="System.String">
                <Items>
                    <dxe:ListEditItem Text="Left-aligned" Value="left" />
                    <dxe:ListEditItem Text="Center" Value="center" />
                    <dxe:ListEditItem Text="Right-aligned" Value="right" />
                </Items>
            </dxe:ASPxComboBox>            
        </td>
    </tr>
    <tr>
        <td>
            <div class="fieldSeparator"></div>        
        </td>
    </tr>    
    <tr>
        <td>
            <dxe:ASPxLabel ID="lblImageDescription" runat="server" Text="Description:"></dxe:ASPxLabel>
            <div class="captionIndent"></div>
            <dxe:ASPxTextBox ID="txbDescription" ClientInstanceName="_dxeTxbDescription" runat="server" Width="170px" AutoCompleteType="Disabled">
            </dxe:ASPxTextBox>
        </td>
    </tr>    
</table>

<script type="text/javascript" id="dxss_ImagePropertiesForm">
    function OnCmbSizeSelectedIndexChanged(cmb) {
        var isShow = cmb.GetSelectedIndex() == 1;
        _aspxSetElementDisplay(_aspxGetElementById("_dxeSizePropertiesRow"), isShow);
        if (isShow)
            aspxAdjustControlsSizeInDialogWindow();
    }
</script>