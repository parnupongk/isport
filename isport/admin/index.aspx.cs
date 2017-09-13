using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebLibrary;
using System.Configuration;
using Tamir.SharpSsh;
using System.Data;
using DevExpress.Web;
namespace isport.admin
{
    public partial class index : System.Web.UI.Page
    {

        #region Proprety
        /// <summary>
        /// Status row
        /// </summary>
        private string GetUIID
        {
            get
            {
                return ViewState["UIID"] == null ? "" : ViewState["UIID"].ToString();
            }
            set
            {
                ViewState["UIID"] = value;
            }
        }
        private string GetMasterID
        {
            get
            {
                return ViewState["mid"] == null ? "0" : ViewState["mid"].ToString();
            }
            set
            {
                ViewState["mid"] = value;
            }
        }
        private int GetLevel
        {
            get
            {
                return ViewState["l"] == null ? 0 : int.Parse(ViewState["l"].ToString());
            }
            set
            {
                ViewState["l"] = value;
            }
        }
        private int RowCount
        {
            get{
                return ViewState["rowcount"] == null ? 0 : int.Parse(ViewState["rowcount"].ToString());
            }
            set
            {
                ViewState["rowcount"] = value;
            }
        }
#endregion

        #region pageload & databind
        protected void Page_Load(object sender, EventArgs e)
        {
           if (!IsPostBack)
            {
                try
                {
                    DataBindProjectType();
                    txtProjectType.Text = Request["p"] == null ? "bb" : Request["p"].ToString();
                    lnkWap.NavigateUrl = ConfigurationManager.AppSettings["Isport_URLSMS"] + txtProjectType.Text;
                    if (ddlProject.Items.FindByValue(txtProjectType.Text)  != null)
                    {
                        ddlProject.ClearSelection();
                        ddlProject.Items.FindByValue(txtProjectType.Text).Selected = true;
                    }

                    DataBindContentBySession();
                }
                catch (Exception ex)
                {
                    ExceptionManager.WriteError(ex.Message);
                }
            }
        }
        private void DataBindProjectTypeBySession()
        {
            if (Session["DataBindProjectType"] == null) DataBindProjectType();
            else
            {
                DataSet ds = (DataSet)Session["DataBindProjectType"];
                ddlProject.DataTextField = "ui_projecttype";
                ddlProject.DataValueField = "ui_projecttype";
                ddlProject.DataSource = ds;
                ddlProject.DataBind();
            }
        }
        private void DataBindProjectType()
        {
            try
            {
                System.Data.DataSet ds = new AppCode().SelectUIGroupProjectType();
                ddlProject.DataTextField = "ui_projecttype";
                ddlProject.DataValueField = "ui_projecttype";
                ddlProject.DataSource = ds;
                ddlProject.DataBind();
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("DataBindProjectType " + ex.Message);
            }
        }
        private void DataBindContentBySession()
        {
            if (ViewState["DataContent"] == null) DataBindContent();
            else
            {
               DataSet ds = (DataSet)ViewState["DataContent"];

                DataView dv = null;
                dv = ds.Tables[0].DefaultView;
                dv.Sort = (ddlProject.SelectedValue == "newsmain") ? "ui_startdate desc" : "";

                gvContent.DataSource = dv;
                gvContent.DataBind();
            }
        }
        private void DataBindContent()
        {
            try
            {
                //if (ddlProject.SelectedIndex > 0)
                //{
                lnkLevel.Text = GetLevel.ToString();

                uMenu1.MenuID = GetMasterID;
                uMenu1.uGenMenu();
                
                System.Data.DataSet ds = new AppCode().SelectUIByLevel(GetMasterID, GetLevel, (ddlProject.SelectedIndex > 0 ? ddlProject.SelectedValue : Request["p"]));

                System.Data.DataView dv = null;
                dv = ds.Tables[0].DefaultView;           
                dv.Sort = (ddlProject.SelectedValue == "newsmain") ? "ui_startdate desc" : "" ;
                

                ViewState["DataContent"] = ds;
                RowCount = dv.Count;
                gvContent.DataSource = dv;
                gvContent.DataBind();
                //}
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("DataBindContent:" + ex.Message);
            }
        }
        #endregion

        #region GridView
        protected void gvImages_PageIndexChanged(object sender, EventArgs e)
        {
            DevExpress.Web.ASPxGridView.ASPxGridView grid =
(DevExpress.Web.ASPxGridView.ASPxGridView)selectProductsPopUp.Windows[1].FindControl("gvImages");

            grid.DataSource = new AppImages().ImagesSelect();
            grid.DataBind();
        }
        public string CheckImagePath(string pathImage)
        {
            return pathImage == null || pathImage == "" ? "~/images/no_image.jpg" : pathImage;
        }
        protected void gvContent_RowCommand(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewRowCommandEventArgs e)
        {
            string comm = e.CommandArgs.CommandName;
            object[] obj = (object[])gvContent.GetRowValues(e.VisibleIndex
                , new string[] { "ui_id", "ui_index","ui_operator"
                    ,"ui_startdate","ui_enddate","ui_ispayment"
                    ,"content_icon","content_link","content_text"
                    ,"content_align","ui_sg_id","content_breakafter"
                    ,"ui_isnews","ui_isnews_top","content_image"
                    ,"ui_contentname","content_color","content_bold"
                    ,"content_isredirect","ui_ispaymentwap","ui_ispaymentsms"
                    ,"content_bgcolor","content_txtsize","content_isgallery"});
            if (comm == "AddSub")
            {
                GetMasterID = obj[0].ToString();
                GetLevel += 1;
                DataBindContent();
            }
            else if (comm == "Edit")
            {
                // UI
                GetUIID = obj[0].ToString();
                ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIndex")).Text = obj[1].ToString();
                ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtSgId")).Text = obj[10].ToString();

                ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlOperator")).ClearSelection();
                ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlOperator")).Items.FindByValue(obj[2].ToString()).Selected = true;

                ((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateStart")).Date = (DateTime)obj[3];
                if (obj[4] !=null && obj[4].ToString() != "")
                {
                    ((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateEnd")).Date = (DateTime)obj[4];
                }
                ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkPay")).Checked = (bool)obj[5];
                ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkNews")).Checked = obj[12] == null || obj[12].ToString() == "" ? false : (bool)obj[12];
                ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtNewsTop")).Text = obj[13] == null ? "" : obj[13].ToString();

                // Content
                ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIcon")).Text = obj[6].ToString();
                ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtLink")).Text = obj[7].ToString();
                ((DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor)selectProductsPopUp.Windows[0].FindControl("txtText")).Html = obj[8].ToString();
                ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkBreakAfter")).Checked = (bool)obj[11];
                ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkDelete")).Checked = false;
                ((Image)selectProductsPopUp.Windows[0].FindControl("img")).ImageUrl 
                    = obj[14] == null || obj[14].ToString() == "" ? "~/images/no_image.jpg" : obj[14].ToString();
                ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlAlign")).ClearSelection();
                ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlAlign")).Items.FindByValue(obj[9].ToString()).Selected = true;
                ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlContent")).ClearSelection();
                ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlContent")).Items.FindByValue(obj[15] == null ? "" : obj[15].ToString()).Selected = true;

                ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlOperator")).ClearSelection();
                ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlOperator")).Items.FindByValue(obj[2].ToString()).Selected = true;

                if (obj[16] != null && obj[16].ToString() !="")
                {
                    ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlColor")).ClearSelection();
                    ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlColor")).Items.FindByValue(obj[16].ToString().Trim()).Selected = true;
                }

                if (obj[17] != null && obj[17].ToString() != "")
                {
                    ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlBold")).ClearSelection();
                    ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlBold")).Items.FindByValue(obj[17].ToString().Trim()).Selected = true;
                }

                if (obj[18] != null && obj[18].ToString() != "")
                {
                    ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkIsRedirect")).Checked = (bool)obj[18];
                }

                if (obj[19] != null && obj[19].ToString() != "")
                {
                    ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkIsPaymentWap")).Checked = (bool)obj[19];
                }

                if (obj[20] != null && obj[20].ToString() != "")
                {
                    ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkIsPaymentSMS")).Checked = (bool)obj[20];
                }
                if (obj[21] != null && obj[21].ToString() != "")
                {
                    ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlBgColor")).ClearSelection();
                    ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlBgColor")).Items.FindByValue(obj[21].ToString().Trim()).Selected = true;
                }
                if (obj[22] != null && obj[22].ToString() != "")
                {
                    ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlFontSize")).ClearSelection();
                    ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlFontSize")).Items.FindByValue(obj[22].ToString().Trim()).Selected = true;
                }
                if (obj[23] != null && obj[23].ToString() != "")
                {
                    ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkIsGallery")).Checked = (bool)obj[23];
                }
                selectProductsPopUp.Windows[0].ShowOnPageLoad = true;
            }
            else if (comm == "Delete")
            {
                // Delete
                new AppCode().DeleteUI(obj[0].ToString());
                DataBindContent();
            }
            else if (comm == "insert")
            {
                ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIndex")).Text = ((int)obj[1] - 1).ToString();
                selectProductsPopUp.Windows[0].ShowOnPageLoad = true;
            }

        }

        protected void gvImages_RowCommand(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewRowCommandEventArgs e)
        {
            try
            {
                if (e.CommandArgs.CommandName == "Select")
                {
                    ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIcon")).Text = e.CommandArgs.CommandArgument.ToString();
                }
                //((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIcon")).Text = 
                selectProductsPopUp.Windows[0].ShowOnPageLoad = true;
                selectProductsPopUp.Windows[1].ShowOnPageLoad = false;
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }
        }
        protected void gvContent_PageIndexChanged(object sender, EventArgs e)
        {
            DataBindContentBySession();
        }

        #endregion

        #region Submit
        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string uiID = "";
                if (GetUIID == "")
                {

                    #region Insert 
                    if (Session["fileName"] != null && Session["fileName"].ToString() != "")
                    {
                        string[] files = Session["fileName"].ToString().Split(':');
                        foreach (string strFile in files)
                        {
                            if (strFile != "")
                            {
                                // insert ตาม file upload
                                uiID = Guid.NewGuid().ToString();
                                new AppCode().InsertUI(GetUIRow(uiID), GetContentRow(uiID, strFile));
                            }

                        }
                        Session["fileName"] = "";
                    }
                    else
                    {
                        // insert ทีละ recode
                        uiID = Guid.NewGuid().ToString();
                        new AppCode().InsertUI(GetUIRow(uiID), GetContentRow(uiID,""));   
                    }
                    #endregion

                }
                else
                {

                    #region update
                    if (Session["fileName"] != null && Session["fileName"] != "")
                    {
                        string[] files = Session["fileName"].ToString().Split(':');
                        foreach (string strFile in files)
                        {
                            if (strFile != "")
                            {
                                // update ตาม file upload
                                uiID = GetUIID;
                                new AppCode().UpdateUI(GetUIRow(uiID), GetContentRow(uiID, strFile));
                            }
                        }
                    }
                    else
                    {
                        // update ทีละ recode
                        uiID = GetUIID;
                        new AppCode().UpdateUI(GetUIRow(uiID), GetContentRow(uiID, ""));
                    }
                    #endregion

                }

                Session["fileName"] = "";
                GetUIID = "";
                DataBindContent();
                selectProductsPopUp.Windows[0].ShowOnPageLoad = false;
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
                new AppSendEmail().SendEmail(ConfigurationManager.AppSettings["Isport_MailErr_To"], ConfigurationManager.AppSettings["Isport_MailErr_Subject"]
                        , String.Format(ConfigurationManager.AppSettings["Isport_MailErr_Body"], WebLibrary.MitTool.GetCookie(Request, "isportwapadmin"), txtProjectType.Text
                        , "index.aspx", ex.Message));
            }
        }
        private isportDS.wapisport_uiRow GetUIRow(string uiID)
        {
            try
            {
                DateTime startDate = ((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateStart")).Text == "" ? DateTime.Now : ((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateStart")).Date;
                isportDS.wapisport_uiRow drUi = new isportDS.wapisport_uiDataTable().Newwapisport_uiRow();
                drUi.ui_id = uiID;
                drUi.ui_master_id = GetMasterID;
                drUi.ui_projecttype = txtProjectType.Text;//ddlProject.SelectedValue;//txtProject.Text;//ConfigurationManager.AppSettings["Isport_ProjectType"].ToString();
                drUi.ui_level = ViewState["l"] == null ? 0 : int.Parse(ViewState["l"].ToString());
                drUi.ui_index = int.Parse(((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIndex")).Text);
                drUi.ui_operator = ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlOperator")).SelectedValue;
                drUi.ui_startdate = startDate;
                if (((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateEnd")).Text != "")
                {
                    drUi.ui_enddate = ((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateEnd")).Date;
                }
                drUi.ui_createdate = DateTime.Now;
                drUi.ui_updatedate = DateTime.Now;
                drUi.ui_createuser = "";
                drUi.ui_updateuser = "";
                drUi.ui_createip = Request.UserHostAddress;
                drUi.ui_updateip = Request.UserHostAddress;
                drUi.ui_ismaster = false;
                drUi.ui_ispayment = ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkPay")).Checked;
                drUi.ui_sg_id = ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtSgId")).Text;
                drUi.ui_isnews = ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkNews")).Checked;
                drUi.ui_isnews_top = ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtNewsTop")).Text;
                drUi.ui_contentname = ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlContent")).SelectedValue;
                drUi.ui_ispaymentwap = ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkIsPaymentWap")).Checked;
                drUi.ui_ispaymentsms = ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkIsPaymentSMS")).Checked;
                return drUi;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private isportDS.wapisport_contentRow GetContentRow(string masterID,string fileUploadName)
        {
            try
            {
                isportDS.wapisport_contentRow drContent = new isportDS.wapisport_contentDataTable().Newwapisport_contentRow();
                drContent.content_id = Guid.NewGuid().ToString();
                drContent.master_id = masterID;
                drContent.content_icon = ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIcon")).Text;
                drContent.content_createdate = DateTime.Now;
                drContent.content_link = ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtLink")).Text;
                drContent.content_text = ((DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor)selectProductsPopUp.Windows[0].FindControl("txtText")).Html;
                drContent.content_align = ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlAlign")).SelectedValue;
                drContent.content_breakafter = ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkBreakAfter")).Checked;
                drContent.content_color = ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlColor")).SelectedValue.Trim();
                drContent.content_bold = ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlBold")).SelectedValue.Trim();
                drContent.content_isredirect = ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkIsRedirect")).Checked;
                drContent.content_bgcolor = ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlBgColor")).SelectedValue.Trim();
                drContent.content_txtsize = ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlFontSize")).SelectedValue.Trim();
                drContent.content_isgallery = ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkIsGallery")).Checked;

                if (fileUploadName != "")
                {
                    // MultiFile
                    drContent.content_image = fileUploadName;
                }
                else
                {
                    #region Single File
                    TextBox txtImgURL = (TextBox)selectProductsPopUp.Windows[0].FindControl("txtImgURL");
                    FileUpload filUpload = (FileUpload)selectProductsPopUp.Windows[0].FindControl("filUpload");
                    if (filUpload.PostedFile.ContentLength > 0)
                    {
                        string fileType = filUpload.FileName.Split('.').Length > 1 ? filUpload.FileName.Split('.')[1] : "";
                        string fileName = DateTime.Now.ToString("yyyMMddHHss") + "." + fileType;// +filUpload.FileName;
                        fileName = Request["pcat"] == "280" ? "zero"+ fileName : fileName;
                        string fullPath = Server.MapPath(ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileName);
                        filUpload.PostedFile.SaveAs(fullPath);
                        drContent.content_image = ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileName;
                        ExceptionManager.WriteError(filUpload.PostedFile.ContentType);
                        if (filUpload.PostedFile.ContentType == "video/mp4" || filUpload.PostedFile.ContentType == "video/3gpp" || filUpload.PostedFile.ContentType == "application/octet-stream" || fileType == "3gp")
                        {
                            SshTransferProtocolBase sshCp;
                            sshCp = new Sftp(ConfigurationManager.AppSettings["Isport_SFTP_Host"], ConfigurationManager.AppSettings["Isport_SFTP_User"]);
                            sshCp.Password = ConfigurationManager.AppSettings["Isport_SFTP_Password"];
                            try
                            {
                                filUpload.PostedFile.SaveAs(fullPath);
                                sshCp.Connect();
                                //ExceptionManager.WriteError("conntect success");
                                if (Request["pcat"] == "280")
                                {


                                    ExceptionManager.WriteError(ConfigurationManager.AppSettings["Isport_PathStreamimgZero"] + fileName);
                                    sshCp.Put(fullPath, ConfigurationManager.AppSettings["Isport_PathStreamimgZero"] + fileName);
                                }
                                else
                                {
                                    ExceptionManager.WriteError(ConfigurationManager.AppSettings["Isport_PathStreamimg"] + fileName);
                                    sshCp.Put(fullPath, ConfigurationManager.AppSettings["Isport_PathStreamimg"] + fileName);

                                }
                                //ExceptionManager.WriteError("upload success");
                                sshCp.Close();
                                new System.IO.FileInfo(fullPath).Delete();
                            }
                            catch (Exception ex)
                            {
                                ExceptionManager.WriteError(ex.Message);
                                sshCp.Close();
                            }
                            drContent.content_link = fileName;
                            drContent.content_image = ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIcon")).Text;
                            drContent.content_icon = "";
                        }
                    }
                    else if (((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkDelete")).Checked) drContent.content_image = "";
                    else if (txtImgURL.Text != "") drContent.content_image = txtImgURL.Text;
                    #endregion
                }

                if (((TextBox)selectProductsPopUp.Windows[0].FindControl("txtLink")).Text.IndexOf(".mp4") > -1 || ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtLink")).Text.IndexOf(".3gp") > -1)
                {
                     //drContent.content_link = ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileName;
                    drContent.content_image = ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIcon")).Text;
                }

                return drContent;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region PageButton
        protected void btnSearech_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DevExpress.Web.ASPxGridView.ASPxGridView grid =
                    (DevExpress.Web.ASPxGridView.ASPxGridView)selectProductsPopUp.Windows[1].FindControl("gvImages");

                grid.DataSource = new AppImages().ImagesSelect();
                grid.DataBind();

                selectProductsPopUp.Windows[0].ShowOnPageLoad = false;
                selectProductsPopUp.Windows[1].ShowOnPageLoad = true;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }
        }
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            GetUIID = "";
            ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIndex")).Text = (RowCount + 1).ToString();
            ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtSgId")).Text = "";
            ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlOperator")).ClearSelection();
            ((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateStart")).Date = DateTime.Now;
            ((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateEnd")).Text = "";

            ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkPay")).Checked = false;
            ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkNews")).Checked = false;
            ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtNewsTop")).Text = "";

            // Content
            ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIcon")).Text = "";
            ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtLink")).Text = "";
            ((DevExpress.Web.ASPxHtmlEditor.ASPxHtmlEditor)selectProductsPopUp.Windows[0].FindControl("txtText")).Html = "";
            ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkBreakAfter")).Checked = true;
            ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkIsRedirect")).Checked = false;
            ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkDelete")).Checked = false;
            ((Image)selectProductsPopUp.Windows[0].FindControl("img")).ImageUrl = "~/images/no_image.jpg";
            ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlAlign")).ClearSelection();
            ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlBgColor")).ClearSelection();
            ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkIsGallery")).Checked = false;
            selectProductsPopUp.Windows[0].ShowOnPageLoad = true;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            GetMasterID = "0";
            GetLevel = 0;
            DataBindContent();
        }
        #endregion

        #region
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProjectType.Text = ddlProject.SelectedValue;
            lnkWap.NavigateUrl = ConfigurationManager.AppSettings["Isport_URLSMS"] + txtProjectType.Text;
            DataBindContent();
        }

        protected void btnMultiFile_Click(object sender, EventArgs e)
        {
            selectProductsPopUp.Windows[0].ShowOnPageLoad = false;
            selectProductsPopUp.Windows[2].ShowOnPageLoad = true;
        }

        protected void AjaxFileUpload1_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            string fileName = DateTime.Now.ToString("yyyMMddHHss") + new Random().Next(999) + e.ContentType;// +e.FileName;
            string fullPath = Server.MapPath(ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileName);
            ((AjaxControlToolkit.AjaxFileUpload)selectProductsPopUp.Windows[0].FindControl("AjaxFileUpload1")).SaveAs(fullPath);

            Session["fileName"] += ":" + ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileName;
        }

        #endregion
    }
}
