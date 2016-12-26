/*
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
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;

public partial class InsertLinkFrom : HtmlEditorUserControl {
    protected override ASPxEditBase[] GetChildDxEdits() {
        return new ASPxEditBase[] { 
			rblLinkType, 
			lblUrl, txbURL,
			lblEmailTo, txbEmailTo,
			lblSubject, txbSubject,
            lblLinkDisplay, lblText,
            txbText, lblToolTip,
            txbToolTip, ckbOpenInNewWindow
        };
    }
    protected override ASPxButton[] GetChildDxButtons() {
        return new ASPxButton[] { btnOk, btnCancel, btnChange };
    }
    protected override ASPxHtmlEditorRoundPanel GetChildDxHtmlEditorRoundPanel() {
        return rpInsertLink;
    }
}