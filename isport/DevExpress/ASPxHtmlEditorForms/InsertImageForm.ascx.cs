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
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;

public partial class InsertImageForm : HtmlEditorUserControl {

    protected void Page_Load(object sender, EventArgs e) {
        ckbSaveToServer.ClientVisible = HtmlEditor.Settings.AllowInsertDirectImageUrls && !string.IsNullOrEmpty(HtmlEditor.SettingsImageUpload.UploadImageFolder);
        rblFromThisComputer.Enabled = !string.IsNullOrEmpty(HtmlEditor.SettingsImageUpload.UploadImageFolder);
    }
    protected override ASPxEditBase[] GetChildDxEdits() {
        return new ASPxEditBase[] {
			rblFromTheWeb, rblFromThisComputer,
			txbInsertImageUrl, 
            ckbSaveToServer, 
            ckbMoreImageOptions
        };
    }
    protected override ASPxButton[] GetChildDxButtons() {
        return new ASPxButton[] { btnInsertImage, btnChangeImage, btnInsertImage, btnInsertImageCancel };
    }
    protected override ASPxHtmlEditorRoundPanel GetChildDxHtmlEditorRoundPanel() {
        return rpInsertImage;
    }

    protected string SaveUploadFile() {
        string fileName = "";
        if (uplImage.HasFile) {
            string uploadFolder = HtmlEditor.SettingsImageUpload.UploadImageFolder;
            fileName = MapPath(uploadFolder) + uplImage.FileName;
            try {
                uplImage.SaveAs(fileName, false);
            } catch (IOException) {
                fileName = MapPath(uploadFolder) + uplImage.GetRandomFileName();
                uplImage.SaveAs(fileName);
            }
        }
        return Path.GetFileName(fileName);
    }
    protected void uplImage_FileUploadComplete(object sender, DevExpress.Web.ASPxUploadControl.FileUploadCompleteEventArgs args) {
        try {
            args.CallbackData = SaveUploadFile();
        } catch (Exception e) {
            args.IsValid = false;
            args.ErrorText = e.Message;
        }
    }
}