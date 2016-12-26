using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using WebLibrary;
using System.Configuration;
using DevExpress.Web.ASPxGridView;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using OracleDataAccress;
using System.Data.OracleClient;
using Tamir.SharpSsh;

namespace isport.admin
{
    public partial class newpage : System.Web.UI.Page
    {

        #region page logs

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


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                if (!IsPostBack)
                {
                    txtProgramDate.Text = DateTime.Now.ToString("yyyyMMdd");
                    txtDisplayDate.Text = DateTime.Now.AddDays(2).ToString("yyyyMMdd");
                    
                    //DataBindProjectType();
                    DataBindSubService();
                    if (Request["pcatid"] != null && Request["pcatid"] == "269")
                    {
                        pnlProgram.Visible = true;
                        pnlSMS.Visible = false;
                        GetMatchQuize();
                        DataBindProgram();
                        ddlSubName.Items.FindByValue(Request["pcatid"]).Selected = true;
                        txtName.Text = ddlSubName.SelectedItem.Text.Substring(0, 2) + new Random().Next(99999);
                        chkIsSmsPay.Text = WebLibrary.MitTool.GetCookie(Request, "admin");
                    }
                    //txtName.Text = ddlSubName.SelectedItem.Value.Substring(0, 2) + new Random().Next(9999);
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(" newpage page_load >> " + ex.Message);
            }
        }
        #endregion

        #region Data Management

        private void DataBindPanel()
        {
            if (ddlSubName.SelectedIndex > 0)
            {
                chkSendNow.Checked = false;
                txtName.Text = ddlSubName.SelectedItem.Text.Substring(0, 2) + new Random().Next(999999);
                pnlEDTGame.Visible = false;
                if (ddlSubName.SelectedValue == "269")
                {
                    pnlEDT.Visible = false;
                    pnlProgram.Visible = true;
                    pnlSMS.Visible = false;
                    GetMatchQuize();
                    DataBindProgram();

                }
                else if (ddlSubName.SelectedValue == "279")
                {
                    pnlSMS.Visible = false;
                    pnlProgram.Visible = false;
                    pnlEDT.Visible = true;

                }
                else if (ddlSubName.SelectedValue == "301")
                {
                    pnlEDTGame.Visible = true;
                    //pnlSMS.Visible = false;
                }
                else
                {
                    pnlSMS.Visible = true;
                    pnlProgram.Visible = false;
                    pnlEDT.Visible = false;

                    if( ddlSubName.SelectedValue == "300")//|| ddlSubName.SelectedValue == "298" 
                    {
                        chkSendNow.Checked = true;
                        txtDisplayDate.Text = DateTime.Now.ToString("yyyyMMdd");
                        //txtSMS.Text = (ddlSubName.SelectedValue == "300")? "วิเคราะห์ฟุตบอลวันนี้ " : "" ;
                    }
                }
            }
            else txtName.Text = "";
        }
        private void DataBindSubService()
        {
            // set fillter service by user login ถ้าไม่ set จะเห็น service ทั้งหมด
            string pcatId = ConfigurationManager.AppSettings["IsportAllowServiceTo" + WebLibrary.MitTool.GetCookie(Request, "isportwapadmin")] == null ? "" : ConfigurationManager.AppSettings["IsportAllowServiceTo" + WebLibrary.MitTool.GetCookie(Request, "isportwapadmin")] ;
            System.Data.DataTable dt = new AppCode().Select_SipSubService(pcatId);

            ddlSubName.DataTextField = "pssv_desc";
            ddlSubName.DataValueField = "pcat_id";
            ddlSubName.DataSource = dt;
            ddlSubName.DataBind();
            ddlSubName.Items.Insert(0,new ListItem("++Selected++", "0", true));

            if( Session["DDLSUBSERVICE"] != null )
            {
                ddlSubName.ClearSelection();
                ddlSubName.Items.FindByValue(Session["DDLSUBSERVICE"].ToString()).Selected = true;
                FillterDataProjectType();
                DataBindPanel();
                //Session["DDLSUBSERVICE"] = ddlSubName.SelectedValue;
            }
        }
        private void DataBindProjectType()
        {
            try
            {
                System.Data.DataSet ds = new AppCode().SelectUIGroupProjectTypeNotNull();

                Session["DataProjectType"] = ds.Tables[0];
                gvData.DataSource = ds.Tables[0];
                gvData.DataBind();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("DataBindProjectType " + ex.Message);
                lblError.Text = ex.Message;
            }
        }

        private void FillterDataProjectType()
        {
            try
            {
                DataSet ds = null;
                if (ddlSubName.SelectedValue == "279")
                {
                    ds = new AppCode().Select_SipContentBuCatID(ddlSubName.SelectedValue);
                }
                else
                {
                    ds = new AppCode().SelectUIGroupProjectTypeByPcatId(ddlSubName.SelectedValue);
                }
                ViewState["DataProjectType"] = ds;
                //DataTable dt = (DataTable)Session["DataProjectType"];
                //DataView dv = dt.DefaultView;
                //System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
                //str.Append("pcat_id = " + ddlSubName.SelectedValue);
                //dv.RowFilter = str.ToString();

                gvData.DataSource = ds;
                gvData.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ViewStateProgram()
        {
            try
            {
                if (ViewState["DataProgram"] == null) DataBindProgram();
                DataSet ds = (DataSet)ViewState["DataProgram"];
                gvProgram.DataSource = ds.Tables[0];
                gvProgram.DataBind();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("ViewStateProgram>>" + ex.Message);
                lblError.Text = ex.Message;
            }
        }
        private void DataBindProgram()
        {
            try
            {
                DataSet ds = new isport_service.ServiceProgram().FootballProgramByDate(AppMain.strConn, "", "L", DateTime.ParseExact(txtProgramDate.Text, "yyyyMMdd", null).ToString("yyyyMMdd"));


                ViewState["DataProgram"] = ds;
                gvProgram.DataSource = ds.Tables[0];
                gvProgram.DataBind();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("DataBindProgram>>" + ex.Message);
                lblError.Text = ex.Message;
            }
        }



        private string GetLink(string pcatId)
        {
            string link = "";
            if (ConfigurationManager.AppSettings["Isport_SERVICE_NOURL"].IndexOf(pcatId) > -1)
            {
                link =  "";
            }
            else if (ConfigurationManager.AppSettings["Isport_URLSMS" + pcatId] == null)
            {
                link =  AppCode.GetShortURL( ConfigurationManager.AppSettings["Isport_URLSMS"] + txtName.Text);
            }
            else
            {
                // ถ้า service siamdara clip (256) และ content type = news จะต้อง fix link เพราะ wap จะแสดงเป็น list ข่าว
                if (pcatId == "256" && ddlContentType.SelectedValue == "news")
                {
                    link = ConfigurationManager.AppSettings["Isport_URLSMS256NEWS"] + txtName.Text;
                }
                else link =  ConfigurationManager.AppSettings["Isport_URLSMS" + pcatId] + txtName.Text;
            }

            return link;//AppCode.GenGoogleAPI_ShoterURL(link);
        }
        #endregion

        #region Submit
        protected void Button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvProgram.VisibleRowCount; i++)
            {
                try
                {
                    GridViewDataColumn col1 = gvProgram.Columns[0] as GridViewDataColumn;
                    //GridViewDataColumn col2 = gvProgram.Columns[8] as GridViewDataColumn;
                    CheckBox rdoTeam1 = gvProgram.FindRowCellTemplateControl(i, col1, "rdoTeam1") as CheckBox;
                    CheckBox rdoTeam2 = gvProgram.FindRowCellTemplateControl(i, col1, "rdoTeam2") as CheckBox;
                    //ASPxCheckBox chkIsVal = gvVndInvoiceDevExp.FindRowCellTemplateControl(i, col2, "chkIsVal") as ASPxCheckBox;

                    if (rdoTeam1 != null && rdoTeam2 != null && (rdoTeam1.Checked || rdoTeam2.Checked))
                    {
                        string team = "";
                        //DataRow dr = gvProgram.GetDataRow(i);
                        object[] obj = (object[])gvProgram.GetRowValues(i
                , new string[] { "team_code1", "team_code2", "msch_id", "match_datetime" });

                        if (rdoTeam1.Checked) team = obj[0].ToString();
                        else if (rdoTeam2.Checked) team = obj[1].ToString();

                        // service 269 ไม่ insert ที่ oracle
                        // Insert
                        object o = SqlHelper.ExecuteScalar(AppMain.strConn, CommandType.StoredProcedure, "usp_insert_match_quiz"
                           , new SqlParameter[] { new SqlParameter("@msch_id",obj[2].ToString())
                                                        , new SqlParameter("@team_code",team)
                                                        , new SqlParameter("@display_date",DateTime.ParseExact(obj[3].ToString(), "yyyyMMdd", null))
                                                        , new SqlParameter("@create_by",WebLibrary.MitTool.GetCookie(Request, "isportwapadmin"))
                        });

                        if (o != null && o != "")
                        {
                            string pcntId = o.ToString();
                            string uiID = Guid.NewGuid().ToString();
                            new AppCode().InsertUI(GetUIRow(uiID, pcntId), GetContentRow(uiID));
                            txtName.Text = ddlSubName.SelectedItem.Text.Substring(0, 2) + new Random().Next(99999);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.WriteError("Submit Program Game >>" + ex.Message);
                    new AppSendEmail().SendEmail(ConfigurationManager.AppSettings["Isport_MailErr_To"], ConfigurationManager.AppSettings["Isport_MailErr_Subject"]
                        , String.Format(ConfigurationManager.AppSettings["Isport_MailErr_Body"], WebLibrary.MitTool.GetCookie(Request, "isportwapadmin"), ddlSubName.SelectedItem.Text + " " + txtName.Text
                        , "newpage.aspx", ex.Message));
                    lblError.Text = ex.Message;
                }
            }
            ClearPage();

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataBindProgram();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text != "")
                {
                    AppCode appCode = new AppCode();

                    if (GetUIID == "")
                    {
                        // Insert
                        string pcntId = "";
                        if (txtSMS.Text != "")
                        {
                            pcntId = appCode.Insert_SipContent(ddlSubName.SelectedValue.ToString()
                                    , GetLink(ddlSubName.SelectedValue.ToString())
                                    , txtSMS.Text.Replace("\r", "").Replace("\n", "").Length > 150 ? txtSMS.Text.Replace("\r", "").Replace("\n", "").Substring(0, 150) : txtSMS.Text.Replace("\r", "").Replace("\n", "")
                                    , txtChoose.Text.Replace("\r", "").Replace("\n", "").Length > 150 ? txtChoose.Text.Replace("\r", "").Replace("\n", "").Substring(0, 150) : txtChoose.Text.Replace("\r", "").Replace("\n", "")
                                    , ddlContentType.SelectedIndex > 0 ? ddlContentType.SelectedValue : txtAnswer.Text.Replace("\r", "").Replace("\n", "")
                                    , txtDisplayDate.Text
                                    , WebLibrary.MitTool.GetCookie(Request, "isportwapadmin")
                                    , chkSendNow.Checked ? "Y" : "N");
                        }
                        string uiID = Guid.NewGuid().ToString();
                        appCode.InsertUI(GetUIRow(uiID, pcntId), GetContentRow(uiID));
                    }
                    else
                    {
                        // Update 
                        string txt = txtSMS.Text.Replace("\r", "").Replace("\n", "");
                        appCode.Update_SipContent(GetUIID, txt
                                                , txtChoose.Text.Replace("\r", "").Replace("\n", "")
                                                , ddlContentType.SelectedIndex > 0 ? ddlContentType.SelectedValue : txtAnswer.Text.Replace("\r", "").Replace("\n", "")
                                                , DateTime.ParseExact(txtDisplayDate.Text, "yyyyMMdd", null)
                                                , WebLibrary.MitTool.GetCookie(Request, "isportwapadmin"));

                        if (ddlSubName.SelectedValue.ToString() == "265")
                            appCode.UpdateContentTitle(txtName.Text, txt);
                    }

                    ClearPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("SubmitInsert " + ex.Message);
                lblError.Text = ex.Message;
            }
        }


        private void GetMatchQuize()
        {
            DataSet ds = SqlHelper.ExecuteDataset(AppMain.strConn, CommandType.StoredProcedure, "usp_match_quiz_select");
            ViewState["DataMatchQuize"] = ds;
        }

        private void ClearPage()
        {
            GetUIID = "";
            txtName.Text = ddlSubName.SelectedItem.Text.Substring(0, 2) + new Random().Next(99999);
            txtName.Enabled = true;
            txtSMS.Text = "";
            txtChoose.Text = "";
            txtAnswer.Text = "";
            txtDisplayDate.Text = DateTime.Now.AddDays(+2).ToString("yyyyMMdd");
            //ddlSubName.ClearSelection();
            FillterDataProjectType();
            if (ddlSubName.SelectedValue == "269")
            {
                pnlProgram.Visible = true;
                pnlSMS.Visible = false;
                GetMatchQuize();
                DataBindProgram();
            }
            else
            {
                pnlSMS.Visible = true;
                pnlProgram.Visible = false;
            }
        }

        private isportDS.wapisport_uiRow GetUIRow(string uiID,string pcntId)
        {
            try
            {
                string sgConfigName = "SG_PCAT" + ddlSubName.SelectedValue;
                isportDS.wapisport_uiRow drUi = new isportDS.wapisport_uiDataTable().Newwapisport_uiRow();
                ViewState["lastId"] = txtName.Text;
                drUi.ui_id = uiID;
                drUi.ui_master_id = pcntId; // เวลา Edite จะได้ไปแก้ไขที่ Sip_content ได้
                drUi.ui_projecttype = txtName.Text;//ddlProject.SelectedValue;//txtProject.Text;//ConfigurationManager.AppSettings["Isport_ProjectType"].ToString();
                drUi.ui_level =  0 ;
                drUi.ui_index = 0;
                drUi.ui_operator = "All";
                drUi.ui_startdate = DateTime.Now;
                drUi.ui_createdate = DateTime.ParseExact(txtDisplayDate.Text, "yyyyMMdd", null);
                drUi.ui_updatedate = DateTime.Now;
                drUi.ui_createuser = "";
                drUi.ui_updateuser = "";
                drUi.ui_createip = Request.UserHostAddress;
                drUi.ui_updateip = Request.UserHostAddress;
                drUi.ui_ismaster = false;
                drUi.ui_ispayment = false;
                drUi.ui_sg_id = ConfigurationManager.AppSettings[sgConfigName] == null || ConfigurationManager.AppSettings[sgConfigName] == "" ? "" : ConfigurationManager.AppSettings[sgConfigName];  // service pcatid= 256 content new ต้องใส่ sg ไปด้วยเพราะ การแสดงหน้าข่าวต้องแสดงเป็น list ข่าวทั้งหมด
                drUi.ui_isnews = false;
                drUi.ui_isnews_top = "";
                drUi.ui_contentname = "";
                drUi.ui_ispaymentwap = chkIsWapPay.Checked;
                drUi.ui_ispaymentsms = chkIsSmsPay.Checked;
                return drUi;
            }
            catch (Exception ex)
            {
                throw new Exception("GetUIRow>>" + ex.Message);
            }
        }
        private isportDS.wapisport_contentRow GetContentRow(string masterID)
        {
            try
            {
                isportDS.wapisport_contentRow drContent = new isportDS.wapisport_contentDataTable().Newwapisport_contentRow();
                drContent.content_id = Guid.NewGuid().ToString();
                drContent.master_id = masterID;
                drContent.content_icon = "";
                drContent.content_createdate = DateTime.Now;
                drContent.content_link = "";
                drContent.content_text = txtSMS.Text;
                drContent.content_align = "Center";
                drContent.content_breakafter = true;
                drContent.content_color = "Black";
                drContent.content_bold = "true";
                drContent.content_isredirect = false;
                drContent.content_bgcolor = "white";
                drContent.content_txtsize = "medium";
                drContent.content_isgallery = false;


                #region Single File
                if (filUpload.PostedFile.ContentLength > 0)
                {
                    string fileType = filUpload.FileName.Split('.').Length > 1 ? filUpload.FileName.Split('.')[1] : "";
                    string fileName = DateTime.Now.ToString("yyyMMddHHss") + "." + fileType;// +filUpload.FileName;
                    fileName = "zero" + fileName;
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
                            //if (Request["pcat"] == "280")
                            //{
                            ExceptionManager.WriteError(ConfigurationManager.AppSettings["Isport_PathStreamimgZero"] + fileName);
                            sshCp.Put(fullPath, ConfigurationManager.AppSettings["Isport_PathStreamimgZero"] + fileName);
                            //}
                            //else
                            //{
                            //ExceptionManager.WriteError(ConfigurationManager.AppSettings["Isport_PathStreamimg"] + fileName);
                            //sshCp.Put(fullPath, ConfigurationManager.AppSettings["Isport_PathStreamimg"] + fileName);

                            //}
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
                        drContent.content_image = "";//((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIcon")).Text;
                        drContent.content_icon = "";
                    }
                }
                /*
                if (filUpload != null && filUpload.PostedFile !=null && filUpload.PostedFile.ContentLength > 0)
                {
                    string fileType = filUpload.FileName.Split('.').Length > 1 ? filUpload.FileName.Split('.')[1] : "";
                    string fileName = DateTime.Now.ToString("yyyMMddHHss") + "." + fileType;// +filUpload.FileName;
                    string fullPath = Server.MapPath(ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileName);
                    filUpload.PostedFile.SaveAs(fullPath);
                    drContent.content_image = ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileName;
                    //ExceptionManager.WriteError(filUpload.PostedFile.ContentType);

                }*/

                #endregion


                return drContent;
            }
            catch (Exception ex)
            {
                throw new Exception("GetContentRow>>" + ex.Message);
            }
        }

        #endregion

        #region Event
        protected void ddlSubName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillterDataProjectType();
            Session["DDLSUBSERVICE"] = ddlSubName.SelectedValue;
            DataBindPanel();
        }

        protected void ddlContentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlContentType.SelectedValue == "news")
            {
                filUpload.Visible = true;
            }
            //else filUpload.Visible = false;
        }
        protected void gvData_PageIndexChanged(object sender, EventArgs e)
        {
            //DataBindProjectType();
            //FillterDataProjectType();
            DataSet dt = (DataSet)ViewState["DataProjectType"];

            gvData.DataSource = dt;
            gvData.DataBind();
        }
        protected void gvProgram_PageIndexChanged(object sender, EventArgs e)
        {
            ViewStateProgram();
        }
        protected void gvProgram_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == DevExpress.Web.ASPxGridView.GridViewRowType.Data)
            {
                DataSet ds = (DataSet)ViewState["DataMatchQuize"];
                TableRow dr = e.Row;
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 )
                {
                    DataView dv = ds.Tables[0].DefaultView;
                    System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
                    str.Append("msch_id = " + e.GetValue("msch_id"));
                    dv.RowFilter = str.ToString();
                    if ( dv.ToTable().Rows.Count > 0  )
                    {
                        e.Row.BackColor = System.Drawing.Color.Cyan;
                        if (dv.ToTable().Rows[0]["team_code_quiz"].ToString() == e.GetValue("team_code1").ToString())
                        {
                            CheckBox chk = gvProgram.FindRowCellTemplateControl(e.VisibleIndex, null, "rdoTeam1") as CheckBox;
                            chk.Checked = true;
                        }
                        else if (dv.ToTable().Rows[0]["team_code_quiz"].ToString() == e.GetValue("team_code2").ToString())
                        {
                            CheckBox chk = gvProgram.FindRowCellTemplateControl(e.VisibleIndex, null, "rdoTeam2") as CheckBox;
                            chk.Checked = true;
                        }
                    }

                }
            }
        }
        protected void gvData_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {

            if (e.RowType == DevExpress.Web.ASPxGridView.GridViewRowType.Data )
            {

                TableRow dr =  e.Row;
                if (ViewState["lastId"] != null && ViewState["lastId"].ToString() == e.KeyValue.ToString())
                {
                    e.Row.BackColor = System.Drawing.Color.Cyan;
                }

                
                if (ddlContentType.SelectedValue != "279")
                {
                    if (e.GetValue("display_date") != null && e.GetValue("display_date").ToString() != "")
                    {

                        //if (DateTime.Parse(e.GetValue("display_date").ToString()) > DateTime.Now)
                        //{

                            LinkButton lnk = gvData.FindRowCellTemplateControl(e.VisibleIndex, null, "btnEdit") as LinkButton;
                            if (lnk != null) lnk.Visible = true;

                        //}

                    }
                }
            }
        }

        private void DeleteAll(string pcnt_id,string projectType,string pcatId)
        {
            try
            {
                AppCode appCode = new AppCode();
                appCode.Delete_SipContent(pcnt_id);
                if (pcatId == "269")
                {
                    // service 269 ไม่ insert ที่ oracle
                    appCode.MatchQuizDeleteByMschId(pcnt_id);
                    appCode.Delete_SipContent((int.Parse(pcnt_id)-1).ToString());
                }
                appCode.DeleteUIByProjectType(projectType);
                FillterDataProjectType();
                ClearPage();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("DeleteAll>> " + ex.Message);
            }
        }
        protected void gvData_RowCommand(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewRowCommandEventArgs e)
        {
            string comm = e.CommandArgs.CommandName;
            object[] obj = (object[])gvData.GetRowValues(e.VisibleIndex
                , new string[] { "ui_projecttype", "title_local", "create_date", "display_date", "ui_pcnt_id","detail","detail_local","pcat_id","sendnow" });
            if (comm == "Edit")
            {
                txtName.Enabled = false;
                txtName.Text = obj[0].ToString();
                txtSMS.Text = obj[1].ToString();
                txtChoose.Text = obj[5].ToString();
                txtDisplayDate.Text = DateTime.Parse( obj[3].ToString()).ToString("yyyyMMdd");
                GetUIID = obj[4].ToString();
                if (ddlContentType.Items.FindByValue(obj[7].ToString()) == null)
                    txtAnswer.Text = obj[6].ToString();
                else ddlContentType.Items.FindByValue(obj[7].ToString()).Selected = true;
                chkSendNow.Checked = obj[8].ToString() == "N" ? false : true;
            }
            else if (comm == "Delete")
            {
                DeleteAll(obj[4].ToString(), obj[0].ToString(),obj[7].ToString());
            }
            else if (comm == "Content")
            {
                Response.Redirect("index.aspx?p=" + obj[0].ToString()+"&pcat="+ obj[7].ToString(), false);
            }
        }
        #endregion

        #region EDT Upload Data
        protected void btnEdtSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string[] sType = fileEDTUpload.PostedFile.FileName.Split('.');
                if (fileEDTUpload.PostedFile.ContentLength > 0 && (sType.Length > 0 && sType[1] == "xls"))
                {
                    string path = Server.MapPath(ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileEDTUpload.PostedFile.FileName);
                    fileEDTUpload.PostedFile.SaveAs(path);
                    DataTable dt = getData(path);
                    gvEdt.DataSource = dt;
                    gvEdt.DataBind();

                    FillterDataProjectType();
                }
                else
                {
                    lblSum.Text = " กรุณา Upload file XLS ครับ ";
                }
            }
            catch (Exception ex)
            {
                lblSum.Text += "Error : " + ex.Message;
                new AppSendEmail().SendEmail(ConfigurationManager.AppSettings["Isport_MailErr_To"], ConfigurationManager.AppSettings["Isport_MailErr_Subject"]
                        , String.Format(ConfigurationManager.AppSettings["Isport_MailErr_Body"], WebLibrary.MitTool.GetCookie(Request, "isportwapadmin"), ddlSubName.SelectedItem.Text
                        , "newpage.aspx", ex.Message));
            }
        }

        private DataTable getData(string path)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {new DataColumn("date"),new DataColumn("title"),new DataColumn("link"),new DataColumn("id") });
            //Response.Write(path);
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source= " + path + " ; Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(connectionString);
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();

            try
            {
                string sql = "select * from [EDT$A:F]";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataReader drRead = cmd.ExecuteReader();
                DataRow dr = null;
                int countDate=0, countSMS = 0 , countError = 0;
                string  title = "", edtId = "", displayDate = "", pcntId , smsId="" , errEDTID = "",type="";
                string[] edtIds;
                Random ran = new Random();
                while (drRead.Read())
                {
                    try
                    {

                        if ( drRead[5].ToString().Trim() != "")
                        {
                            type = (string.IsNullOrEmpty(drRead[2].ToString())) ?type: drRead[2].ToString();
                            dr = dt.NewRow();
                            dr["date"] = (drRead[0].ToString().Trim() == "") ? dt.Rows[dt.Rows.Count - 1]["date"] : DateTime.ParseExact(drRead[0].ToString(), "d/M/yyyy", null).ToString("yyyyMMdd");
                            dr["title"] = drRead[3].ToString();
                            dr["link"] = drRead[5].ToString();
                            edtIds = dr["link"].ToString().Split('/');
                            edtId = edtIds[edtIds.Length - 1];

                            if (edtId.IndexOf('_') > -1) edtId = (edtId.Split('_'))[0];
                            else
                            {
                                // format ไม่ปกติ ex. travel.edtguide.com/386769 , eat.edtguide.com/387435/sand-sea-sun-restaurant
                                foreach ( string id in edtIds )
                                {
                                    try
                                    {
                                        int number;
                                        bool result = Int32.TryParse(id, out number);
                                        if (result)
                                        {
                                            edtId = id;
                                            break;
                                        }
                                    }
                                    catch { }
                                }
                            }
                            
                            

                            if ((drRead[3].ToString().Trim() != "") || (drRead[0].ToString().Trim() != ""))
                            {
                                // Insert SipContent
                                //smsId+ "2" ==> sms ที่ 2 ของวัน
                                dr["id"] = (drRead[0].ToString().Trim() == "") ? smsId + "2" : DateTime.ParseExact(drRead[0].ToString(), "d/M/yyyy", null).ToString("yyyyMMdd") + ran.Next(999).ToString();
                                displayDate = (drRead[0].ToString().Trim() == "") ? displayDate : DateTime.ParseExact(drRead[0].ToString(), "d/M/yyyy", null).ToString("MM/dd/yyyy");

                                txtName.Text = dr["id"].ToString(); // parameter gen link ex : edt.guru/index/dr[id]
                                countDate += (drRead[0].ToString().Trim() == "") ? 0 : 1;
                                countSMS += 1;

                                pcntId = new AppCode().Insert_SipContent_EDT(ddlSubName.SelectedValue.ToString()
                                            , GetLink(ddlSubName.SelectedValue.ToString())
                                            , dr["title"].ToString().Replace("\r", "").Replace("\n", "").Length > 180 ? dr["title"].ToString().Replace("\r", "").Replace("\n", "").Substring(0, 180) : dr["title"].ToString().Replace("\r", "").Replace("\n", "")
                                            , dr["id"].ToString()//txtChoose.Text.Replace("\r", "").Replace("\n", "").Length > 150 ? txtChoose.Text.Replace("\r", "").Replace("\n", "").Substring(0, 150) : txtChoose.Text.Replace("\r", "").Replace("\n", "")
                                            , edtId //=> bom update เปลี่ยนเป็น type content เพื่อ support บริการใหม่ //ddlContentType.SelectedIndex > 0 ? ddlContentType.SelectedValue : txtAnswer.Text.Replace("\r", "").Replace("\n", "")
                                            , dr["date"].ToString()//txtDisplayDate.Text
                                            , WebLibrary.MitTool.GetCookie(Request, "isportwapadmin")
                                            , chkSendNow.Checked ? "Y" : "N"
                                            ,type);

                                title = dr["title"].ToString();
                                smsId = dr["id"].ToString();//(drRead[0].ToString().Trim() == "") ? smsId : DateTime.ParseExact(drRead[0].ToString(), "d/M/yyyy", null).ToString("yyyyMMdd") + ran.Next(999).ToString();

                            }
                            // Write XML
                            try
                            {
                                GenXML_EDTData(string.Format(ConfigurationManager.AppSettings["Isport_EDT_GetDATA"], edtId));
                            }
                            catch (Exception ex)
                            {
                                errEDTID += edtId + " : Error Load XML EDT ID :" + edtId + ";<br/>";
                                countError++;
                            }
                            dt.Rows.Add(dr);

                            InsertEDTExcel(displayDate, drRead[1].ToString(), type, edtId, smsId);
                        }
                    }
                    catch (Exception ex)
                    {
                        errEDTID += edtId + ":" + ex.Message + ";<br/>";
                        countError++;
                    }
                }

                lblSum.Text = "Error : " + countError + " ErrID : "+ errEDTID +" ข้อมูลทั้งหมด " + countDate + " วัน / SMS ที่ส่งทั้งหมด " + countSMS + " SMS " ;

                new AppSendEmail().SendEmail(ConfigurationManager.AppSettings["Isport_MailErr_To"], ConfigurationManager.AppSettings["Isport_MailConfirm_Subject"]
                        , String.Format(ConfigurationManager.AppSettings["Isport_MailConfirm_Body"], WebLibrary.MitTool.GetCookie(Request, "isportwapadmin"), ddlSubName.SelectedItem.Text
                        , "newpage.aspx", lblSum.Text));

                conn.Close();
            }
            catch (Exception nex)
            {

                conn.Close();
                throw new Exception(" getdata >> "+nex.Message);

            }
            return dt;
        }
        private void InsertEDTExcel(string date , string title , string link , string edtId,string smsId)
        {
            try
            {
                OrclHelper.ExecuteNonQuery(AppCode.strConnOracle, CommandType.StoredProcedure, "Wap_UI.EDT_XML_INSERT"
    , new OracleParameter[] {OrclHelper.GetOracleParameter("p_ex_id",Guid.NewGuid().ToString() ,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_xml_id",edtId,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_title",title,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_link",link,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_display_date", date,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_sms_id", smsId,OracleType.VarChar,ParameterDirection.Input)});
            }
            catch (Exception ex)
            {
                throw new Exception(" InsertEDTExcel >>  " + ex.Message + " Display date : " + date);
            }
        }
        private void GenXML_EDTData(string url)
        {
            try
            {
                string xml = new SendService.sendpost().SendGet(url);
                //xml = xml.Replace("[","");
                //xml = xml.Replace("]","");
                //xml = "[{\"type\":\"eat\",\"entry_id\":\"407797\",\"name\":\"\\u0e23\\u0e49\\u0e32\\u0e19\\u0e2d\\u0e32\\u0e2b\\u0e32\\u0e23\\u0e0d\\u0e35\\u0e48\\u0e1b\\u0e38\\u0e48\\u0e19 \\u0e04\\u0e34\\u0e19\\u0e19\\u0e34\\u0e08\\u0e34\",\"detail\":\"Kinniji Japanese Restaurant \\u0e23\\u0e49\\u0e32\\u0e19\\u0e2d\\u0e32\\u0e2b\\u0e32\\u0e23\\u0e0d\\u0e35\\u0e48\\u0e1b\\u0e38\\u0e48\\u0e19\\u0e1a\\u0e23\\u0e23\\u0e22\\u0e32\\u0e01\\u0e32\\u0e28\\u0e2a\\u0e1a\\u0e32\\u0e22\\u0e46 \\u0e2a\\u0e44\\u0e15\\u0e25\\u0e4c\\u0e01\\u0e32\\u0e23\\u0e15\\u0e01\\u0e41\\u0e15\\u0e48\\u0e07\\u0e23\\u0e49\\u0e32\\u0e19\\u0e40\\u0e23\\u0e35\\u0e22\\u0e1a\\u0e07\\u0e48\\u0e32\\u0e22 \\u0e40\\u0e19\\u0e49\\u0e19\\u0e42\\u0e17\\u0e19\\u0e2a\\u0e35\\u0e40\\u0e2b\\u0e25\\u0e37\\u0e2d\\u0e07\\u0e19\\u0e27\\u0e25\\u0e2a\\u0e30\\u0e2d\\u0e32\\u0e14\\u0e15\\u0e32 \\u0e40\\u0e08\\u0e49\\u0e32\\u0e02\\u0e2d\\u0e07\\u0e23\\u0e49\\u0e32\\u0e19\\u0e04\\u0e37\\u0e2d \\u0e04\\u0e38\\u0e13\\u0e1e\\u0e35\\u0e17 \\u0e40\\u0e08\\u0e49\\u0e32\\u0e02\\u0e2d\\u0e07 Blocger \\u0e40\\u0e01\\u0e35\\u0e48\\u0e22\\u0e27\\u0e01\\u0e31\\u0e1a\\u0e23\\u0e49\\u0e32\\u0e19\\u0e2d\\u0e32\\u0e2b\\u0e32\\u0e23 &quot;\\u0e01\\u0e34\\u0e19\\u0e01\\u0e31\\u0e1a\\u0e1e\\u0e35\\u0e17&quot; \\u0e19\\u0e2d\\u0e01\\u0e08\\u0e32\\u0e01\\u0e40\\u0e21\\u0e19\\u0e39\\u0e1b\\u0e23\\u0e30\\u0e08\\u0e33\\u0e2d\\u0e22\\u0e48\\u0e32\\u0e07 \\u0e02\\u0e49\\u0e32\\u0e27\\u0e2b\\u0e19\\u0e49\\u0e32\\u0e41\\u0e0b\\u0e25\\u0e21\\u0e2d\\u0e19\\u0e23\\u0e21\\u0e04\\u0e27\\u0e31\\u0e19 \\u0e41\\u0e25\\u0e30\\u0e2a\\u0e25\\u0e31\\u0e14\\u0e15\\u0e48\\u0e32\\u0e07\\u0e46 \\u0e41\\u0e25\\u0e49\\u0e27 \\u0e17\\u0e32\\u0e07\\u0e23\\u0e49\\u0e32\\u0e19\\u0e08\\u0e30\\u0e21\\u0e35\\u0e01\\u0e32\\u0e23\\u0e04\\u0e34\\u0e14\\u0e04\\u0e49\\u0e19\\u0e40\\u0e21\\u0e19\\u0e39\\u0e43\\u0e2b\\u0e21\\u0e48\\u0e46 \\u0e2d\\u0e22\\u0e39\\u0e48\\u0e40\\u0e2a\\u0e21\\u0e2d \\u0e2d\\u0e32\\u0e17\\u0e34 \\u0e02\\u0e49\\u0e32\\u0e27\\u0e2b\\u0e19\\u0e49\\u0e32\\u0e41\\u0e0b\\u0e25\\u0e21\\u0e2d\\u0e19\\u0e23\\u0e21\\u0e04\\u0e27\\u0e31\\u0e19, \\u0e02\\u0e49\\u0e32\\u0e27\\u0e2b\\u0e19\\u0e49\\u0e32\\u0e04\\u0e34\\u0e19\\u0e19\\u0e34\\u0e08\\u0e34\\u0e14\\u0e49\\u0e07\\u0e08\\u0e32\\u0e19\\u0e22\\u0e31\\u0e01\\u0e29\\u0e4c 7 \\u0e2b\\u0e19\\u0e49\\u0e32, \\u0e02\\u0e49\\u0e32\\u0e27\\u0e2b\\u0e19\\u0e49\\u0e32\\u0e2b\\u0e21\\u0e39\\u0e0a\\u0e32\\u0e0a\\u0e39 \\u0e41\\u0e25\\u0e30\\u0e14\\u0e49\\u0e27\\u0e22\\u0e1b\\u0e23\\u0e30\\u0e2a\\u0e1a\\u0e01\\u0e32\\u0e23\\u0e13\\u0e4c\\u0e41\\u0e25\\u0e30\\u0e04\\u0e27\\u0e32\\u0e21\\u0e0a\\u0e2d\\u0e1a\\u0e43\\u0e19\\u0e01\\u0e32\\u0e23\\u0e17\\u0e32\\u0e19\\u0e2d\\u0e32\\u0e2b\\u0e32\\u0e23\\u0e02\\u0e2d\\u0e07\\u0e40\\u0e08\\u0e49\\u0e32\\u0e02\\u0e2d\\u0e07\\u0e23\\u0e49\\u0e32\\u0e19 \\u0e40\\u0e21\\u0e19\\u0e39\\u0e15\\u0e48\\u0e32\\u0e07\\u0e46 \\u0e08\\u0e36\\u0e07\\u0e15\\u0e2d\\u0e1a\\u0e42\\u0e08\\u0e17\\u0e22\\u0e4c\\u0e25\\u0e39\\u0e01\\u0e04\\u0e49\\u0e32\\u0e17\\u0e31\\u0e49\\u0e07\\u0e01\\u0e25\\u0e38\\u0e48\\u0e21\\u0e27\\u0e31\\u0e22\\u0e23\\u0e38\\u0e48\\u0e19 \\u0e04\\u0e19\\u0e17\\u0e33\\u0e07\\u0e32\\u0e19 \\u0e44\\u0e1b\\u0e08\\u0e19\\u0e16\\u0e36\\u0e07\\u0e04\\u0e23\\u0e2d\\u0e1a\\u0e04\\u0e23\\u0e31\\u0e27\",\"business\":\"\\u0e23\\u0e49\\u0e32\\u0e19\\u0e2d\\u0e32\\u0e2b\\u0e32\\u0e23\",\"address\":\"198 \\u0e0b\\u0e2d\\u0e22\\u0e08\\u0e38\\u0e2c\\u0e32 42 \\u0e2d\\u0e32\\u0e04\\u0e32\\u0e23 \\u0e42\\u0e04\\u0e23\\u0e07\\u0e01\\u0e32\\u0e23 U Center \\u0e16\\u0e19\\u0e19\\u0e1e\\u0e0d\\u0e32\\u0e44\\u0e17 \\u0e41\\u0e02\\u0e27\\u0e07\\u0e27\\u0e31\\u0e07\\u0e43\\u0e2b\\u0e21\\u0e48 \\u0e40\\u0e02\\u0e15\\u0e1b\\u0e17\\u0e38\\u0e21\\u0e27\\u0e31\\u0e19 \\u0e01\\u0e23\\u0e38\\u0e07\\u0e40\\u0e17\\u0e1e\\u0e2f 10330\",\"lat\":\"13.7349005522078\",\"lng\":\"100.528259962396\",\"tel\":\"0899276922\",\"web\":\"https:\\/\\/www.facebook.com\\/kinniji\",\"transport\":\"\\u0e08\\u0e32\\u0e01\\u0e1b\\u0e17\\u0e38\\u0e21\\u0e27\\u0e31\\u0e19 \\u0e21\\u0e38\\u0e48\\u0e07\\u0e2b\\u0e19\\u0e49\\u0e32\\u0e15\\u0e23\\u0e07\\u0e44\\u0e1b\\u0e22\\u0e31\\u0e07\\u0e16\\u0e19\\u0e19 \\u0e1e\\u0e0d\\u0e32\\u0e44\\u0e17 \\u0e41\\u0e25\\u0e49\\u0e27\\u0e40\\u0e25\\u0e35\\u0e49\\u0e22\\u0e27\\u0e40\\u0e02\\u0e49\\u0e32\\u0e08\\u0e38\\u0e2c\\u0e32 42 \\u0e1b\\u0e23\\u0e30\\u0e21\\u0e32\\u0e13 500 \\u0e40\\u0e21\\u0e15\\u0e23 \\u0e01\\u0e47\\u0e08\\u0e30\\u0e1e\\u0e1a\\u0e23\\u0e49\\u0e32\\u0e19\\u0e2d\\u0e32\\u0e2b\\u0e32\\u0e23\\u0e0d\\u0e35\\u0e48\\u0e1b\\u0e38\\u0e48\\u0e19 \\u0e04\\u0e34\\u0e19\\u0e19\\u0e34\\u0e08\\u0e34 \\u0e0b\\u0e36\\u0e48\\u0e07\\u0e15\\u0e31\\u0e49\\u0e07\\u0e2d\\u0e22\\u0e39\\u0e48\\u0e20\\u0e32\\u0e22\\u0e43\\u0e19\\u0e42\\u0e04\\u0e23\\u0e07\\u0e01\\u0e32\\u0e23 U Center \\u0e15\\u0e23\\u0e07\\u0e02\\u0e49\\u0e32\\u0e21\\u0e15\\u0e36\\u0e01\\u0e08\\u0e32\\u0e21\\u0e08\\u0e38\\u0e23\\u0e35\\u0e2a\\u0e41\\u0e04\\u0e27\\u0e23\\u0e4c\",\"location\":\"\",\"opentime\":\"\\u0e40\\u0e1b\\u0e34\\u0e14\\u0e17\\u0e38\\u0e01\\u0e27\\u0e31\\u0e19   \\u0e40\\u0e27\\u0e25\\u0e32 11.00 - 21.00 \\u0e19.\",\"image_cover_thumb\":\"http:\\/\\/ed.files-media.com\\/ud\\/images\\/1\\/136\\/407797\\/LUM_3562_Cover-100x70.jpg\",\"image_cover\":\"http:\\/\\/ed.files-media.com\\/ud\\/images\\/1\\/136\\/407797\\/LUM_3562_Cover-620x392.jpg\",\"gallery_thumb\":[\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3562_Cover-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3505-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3498-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3598-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3521-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3495-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3603-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3595-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3591-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3601-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3599-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3585-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3570-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3576-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3573-65x65.jpg\"],\"gallery\":[\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3562_Cover-620x392.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3505-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3498-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3598-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3521-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3495-400x599.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3603-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3595-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3591-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3601-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3599-400x599.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3585-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3570-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3576-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3573-599x400.jpg\"],\"menu_guide\":\"\\u0e02\\u0e49\\u0e32\\u0e27\\u0e2b\\u0e19\\u0e49\\u0e32\\u0e41\\u0e0b\\u0e25\\u0e21\\u0e2d\\u0e19\\u0e23\\u0e21\\u0e04\\u0e27\\u0e31\\u0e19, \\u0e2a\\u0e25\\u0e31\\u0e14 Mix Topping, \\u0e22\\u0e33\\u0e41\\u0e0b\\u0e25\\u0e21\\u0e2d\\u0e19\\u0e0b\\u0e32\\u0e0b\\u0e34\\u0e21\\u0e34\",\"type_of_food\":\"\",\"national_food\":\"\\u0e2d\\u0e32\\u0e2b\\u0e32\\u0e23\\u0e0d\\u0e35\\u0e48\\u0e1b\\u0e38\\u0e48\\u0e19\",\"other_services\":\"\",\"type_of_music\":\"\",\"no_of_table\":\"\\u0e19\\u0e49\\u0e2d\\u0e22\\u0e01\\u0e27\\u0e48\\u0e32 20 \\u0e42\\u0e15\\u0e4a\\u0e30\",\"alcohol\":\"no\",\"corkage_fee\":\"no\",\"payment\":\"\"}]";
                //EDT edt= JsonHelper.JsonDeserialize<EDT>(xml);
                List<AppCode_EDT> myDeserializedObjList = (List<AppCode_EDT>)Newtonsoft.Json.JsonConvert.DeserializeObject(xml, typeof(List<AppCode_EDT>));
                //AppCode_EDT edt = Newtonsoft.Json.JsonConvert.DeserializeObject<AppCode_EDT>(xml);

                foreach (AppCode_EDT edt in myDeserializedObjList)
                {
                    xml = Newtonsoft.Json.JsonConvert.SerializeObject(edt,Newtonsoft.Json.Formatting.Indented);
                    //SaveAsXmlToFile(xml, edt.entry_id+".json");
                    using (FileStream fs = File.Open(string.Format(ConfigurationManager.AppSettings["Isport_EDT_FileUpload"], new string[] { edt.entry_id + ".json" }), FileMode.Create))
                    using (StreamWriter sw = new StreamWriter(fs))
                    using (Newtonsoft.Json.JsonWriter jw = new Newtonsoft.Json.JsonTextWriter(sw))
                    {
                        jw.Formatting = Newtonsoft.Json.Formatting.Indented;
                        Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                        serializer.Serialize(jw, edt);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("GenXML_EDTData >> " + ex.Message);
            }
        }
        void SaveAsXmlToFile(object o, string fname)
        {
            StreamWriter wrt = new StreamWriter(string.Format(ConfigurationManager.AppSettings["Isport_EDT_FileUpload"], new string[] { fname }), false, Encoding.UTF8);
            try
            {
                wrt.Write(o);
                wrt.Flush();
                wrt.Close();
            }
            catch (Exception ex)
            {
                wrt.Flush();
                wrt.Close();
                throw new Exception("SaveAsXmlToFile" + ex.Message);
            }
        }

        #endregion

    }
}
