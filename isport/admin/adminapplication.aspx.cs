using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using OracleDataAccress;
using System.Data.OracleClient;
using isport_service;
using WebLibrary;
namespace isport.admin
{
    public partial class adminapplication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                DataModelsBinding();
                DataContentBinding();
                DataMeidaBinding();
            }
        }
        private void DataModelsBinding()
        {
            try
            {
                DataSet ds = OrclHelper.Fill(AppMain.strConnOracle, CommandType.StoredProcedure, "ISPORT_TEST.KissApp_SelectAll_Models", "Kiss_Models"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("o_Content", "", OracleType.Cursor, ParameterDirection.Output) });

                gvModel.DataSource = ds;
                gvModel.DataBind();

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("DataModelsBinding >> "+ ex.Message);
            }
        }
        private void DataContentBinding()
        {
            try
            {
                DataSet ds = OrclHelper.Fill(AppMain.strConnOracle, CommandType.StoredProcedure, "ISPORT_TEST.KissApp_SelectAll_Content", "Kiss_Content"
                    , new OracleParameter[] {OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output )});

                ViewState["DataContent"] = ds;

                gvData.DataSource = ds;
                gvData.DataBind();

                //if (gvData.Rows.Count > -1)
                //{
                //    gvData.SelectedIndex = 0;
                //    Session["SelContentID"] = gvData.SelectedDataKey.Value.ToString();
                //}
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                ExceptionManager.WriteError("DataContentBinding >> " + ex.Message);
            }
        }
        private void DataMeidaBinding()
        {
            try
            {
                if (Session["SelContentID"] != null)
                {
                    DataSet ds = OrclHelper.Fill(AppMain.strConnOracle, CommandType.StoredProcedure, "ISPORT_TEST.KissApp_Select_MediaByKCID", "KISS_MEDIA"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("P_KC_ID",Session["SelContentID"],OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("o_Content", "", OracleType.Cursor, ParameterDirection.Output) 
                                                        });

                    gvMedia.DataSource = ds;
                    gvMedia.DataBind();

                    //Session["SelContentID"] = null;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("DataMeidaBinding >> " + ex.Message);
            }
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                string[] sType = fileUpload.PostedFile.FileName.Split('.');
                if (fileUpload.PostedFile.ContentLength > 0 && (sType.Length > 0 && sType[sType.Length-1] == "xls"))
                {
                    string path = Server.MapPath(ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileUpload.PostedFile.FileName);
                    fileUpload.PostedFile.SaveAs(path);
                    DataTable dt = getData(path);
                    //gvData.DataSource = dt;
                    //gvData.DataBind();
                    DataContentBinding();
                }
                else
                {
                    lblSum.Text = " กรุณา Upload file XLS ครับ ";
                }
            }
            catch (Exception ex)
            {
                lblSum.Text += "Error : " + ex.Message;
                //new AppSendEmail().SendEmail(ConfigurationManager.AppSettings["Isport_MailErr_To"], ConfigurationManager.AppSettings["Isport_MailErr_Subject"]
                //        , String.Format(ConfigurationManager.AppSettings["Isport_MailErr_Body"], WebLibrary.MitTool.GetCookie(Request, "isportwapadmin"), "Kiss Model Application"
                //        , "newpage.aspx", ex.Message));
            }
        }

        private DataTable getData(string path)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("date"), new DataColumn("title"), new DataColumn("link"), new DataColumn("id") });
            //Response.Write(path);
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source= " + path + " ; Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(connectionString);
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();

            try
            {
                string sql = "";
                int countError = 0;
                string displayDate = "", errEDTID = "";
                OleDbCommand cmd;
                OleDbDataReader drRead;
                DataRow dr = null;

                #region Content

                try
                {
                    sql = "select * from [content$]";
                    cmd = new OleDbCommand(sql, conn);
                    drRead = cmd.ExecuteReader();
                    dr = null;

                    Random ran = new Random();

                    while (drRead.Read())
                    {
                        try
                        {

                            //string str = drRead["footer"].ToString().Replace("\r", "\\\\\\\\\\\\r");
                            dt.Rows.Add(drRead);
                            displayDate = (drRead["date"].ToString().Trim() == "") ? displayDate : DateTime.ParseExact(drRead["date"].ToString(), "d/M/yyyy", null).ToString("MM/dd/yyyy");

                            OrclHelper.ExecuteNonQuery(AppMain.strConnOracle, CommandType.StoredProcedure, "ISPORT_TEST.KissApp_Insert_Content"
                                , new OracleParameter[] {OrclHelper.GetOracleParameter("P_KC_ID", Guid.NewGuid().ToString() ,OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("P_TITLE",drRead["title"],OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("P_TITLE_DETAIL",drRead["detail"],OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("P_FOOTER",drRead["footer"],OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("P_CREATE_BY","",OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("P_FREE",drRead["free"],OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("P_DISPLAY_DATE",DateTime.Now,OracleType.DateTime,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("P_KM_ID",drRead["model_id"].ToString().Trim() == "" ? 0 : drRead["model_id"] ,OracleType.Int16,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("P_DISPLAY_TXT", displayDate.Replace("/",""),OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("P_TYPE", drRead["type"].ToString(),OracleType.VarChar,ParameterDirection.Input)
                                                                    ,OrclHelper.GetOracleParameter("P_DETAIL1",drRead["detail1"],OracleType.VarChar,ParameterDirection.Input)
                                                                    ,OrclHelper.GetOracleParameter("P_DETAIL2",drRead["detail2"],OracleType.VarChar,ParameterDirection.Input)
                                                                    });

                        }
                        catch (Exception ex)
                        {
                            errEDTID += ":" + ex.Message + ";<br/>";
                            countError++;
                        }
                    }

                }
                catch (Exception ex)
                {
                    errEDTID += ":" + ex.Message + ";<br/>";
                    countError++;
                }

                #endregion

                #region Model

                try
                {
                    sql = "select * from [model$]";
                    cmd = new OleDbCommand(sql, conn);
                    drRead = cmd.ExecuteReader();
                    while (drRead.Read())
                    {
                        OrclHelper.ExecuteNonQuery(AppMain.strConnOracle, CommandType.StoredProcedure, "ISPORT_TEST.KissApp_Insert_Models"
                                , new OracleParameter[] {OrclHelper.GetOracleParameter("p_km_id", drRead["id"] ,OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("p_fname",drRead["FNAME"],OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("p_lname",drRead["LNAME"],OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("p_nname",drRead["NNAME"],OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("p_shape",drRead["SHAPE"],OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("p_w",drRead["KM_W"],OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("p_h",drRead["KM_H"],OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("p_interview",drRead["INTERVIEW"],OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("p_create_by","",OracleType.VarChar,ParameterDirection.Input)
                                                                    });
                    }
                }
                catch (Exception ex)
                {
                    errEDTID += " Model :" + ex.Message + ";<br/>";
                    countError++;
                }


                #endregion

                lblSum.Text = "Error : " + countError + " ErrID : " + errEDTID ;



                new AppSendEmail().SendEmail(ConfigurationManager.AppSettings["Isport_MailErr_To"], ConfigurationManager.AppSettings["Isport_MailConfirm_Subject"]
                        , String.Format(ConfigurationManager.AppSettings["Isport_MailConfirm_Body"], WebLibrary.MitTool.GetCookie(Request, "isportwapadmin"), "Kiss Model Application"
                        , "newpage.aspx", lblSum.Text));

                conn.Close();
            }
            catch (Exception nex)
            {

                conn.Close();
                throw new Exception(" getdata >> " + nex.Message);

            }
            return dt;
        }

        private string InsertMedia(string strPic,string strClip,string contentId)
        {
            try
            {
                OrclHelper.ExecuteNonQuery(AppMain.strConnOracle, CommandType.StoredProcedure, "ISPORT_TEST.KissApp_Insert_Media"
                                , new OracleParameter[] {OrclHelper.GetOracleParameter("P_PIC", strPic,OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("P_CLIP",strClip,OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("P_CREATE_BY","",OracleType.VarChar,ParameterDirection.Input)
                                                                    , OrclHelper.GetOracleParameter("P_KC_ID",contentId,OracleType.VarChar,ParameterDirection.Input)
                                                                    });
                
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }

            return "";
        }

        protected void uploadMedia_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            //Session["SelContentID"] = "38bb5dd4-2e9c-4682-83d8-14cd99e54d90";
            if (Session["SelContentID"] != null && Session["SelContentID"] != "")
            {
                string strPic = "", strClip = "";
                string fileName = DateTime.Now.ToString("yyyMMddHHss") + new Random().Next(999) + e.ContentType;// +e.FileName;
                string fullPath = Server.MapPath(ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileName);
                uploadMedia.SaveAs(fullPath);

                if (e.ContentType.Contains("mp4") || e.ContentType.Contains("3gp") || e.ContentType == "application/octet-stream")
                {
                    //new upWowza().WowZaUpload(fullPath, fileName);
                    Upload(fullPath, fileName);
                    strClip =  fileName;
                }
                else
                {
                    strPic =  fileName; 
                }

                InsertMedia(strPic, strClip, Session["SelContentID"].ToString());

                Session["fileName"] += ":" + ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileName;
            }
        }

        void Upload(string fullPath,string fileName)
        {
            WSUpWowZa.upWowza proxy = new WSUpWowZa.upWowza();
            proxy.WowZaUploadCompleted += new WSUpWowZa.WowZaUploadCompletedEventHandler(proxy_WowZaUploadCompleted);
            proxy.WowZaUpload(fullPath, fileName);
        }

        void proxy_WowZaUploadCompleted(object sender, WSUpWowZa.WowZaUploadCompletedEventArgs e)
        {
            lblSum.Text += Session["fileName"].ToString();
        }

        protected void uploadMedia_UploadCompleteAll(object sender, AjaxControlToolkit.AjaxFileUploadCompleteAllEventArgs e)
        {
            lblSum.Text += Session["fileName"].ToString();
        }

        protected void gvData_SelectedIndexChanged(object sender, EventArgs e)
        {

            //lblSum.Text = gvData.SelectedDataKey.Value.ToString();
            Session["SelContentID"] = gvData.SelectedIndex == -1 ? "" : gvData.SelectedDataKey.Value;
            string str = Session["SelContentID"].ToString();
            DataMeidaBinding();



        }

        protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

                String id = gvData.DataKeys[e.RowIndex].Value.ToString();//e.Keys[e.RowIndex].ToString();
                OrclHelper.ExecuteNonQuery(AppMain.strConnOracle, CommandType.StoredProcedure, "ISPORT_TEST.KissApp_Delete_Content"
                                    , new OracleParameter[] {OrclHelper.GetOracleParameter("P_KC_ID", id,OracleType.VarChar,ParameterDirection.Input)
                                                                    });
                Session["SelContentID"] = -1;
                DataContentBinding();
                DataMeidaBinding();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("gvData_RowDeleting >> " + ex.Message);
            }
        }

        protected void gvMedia_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                String id = gvMedia.DataKeys[e.RowIndex].Value.ToString();//e.Keys[e.RowIndex].ToString();
                OrclHelper.ExecuteNonQuery(AppMain.strConnOracle, CommandType.StoredProcedure, "ISPORT_TEST.KissApp_Delete_Media"
                                    , new OracleParameter[] {OrclHelper.GetOracleParameter("P_KME_ID", id,OracleType.VarChar,ParameterDirection.Input)
                                                                    });

                DataMeidaBinding();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("gvMedia_RowDeleting >> " + ex.Message);
            }
        }

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex=e.NewPageIndex;
            Session["SelContentID"] = -1;
            DataContentBinding();
            DataMeidaBinding();
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = (DataSet)ViewState["DataContent"];
                DataView dv = ds.Tables[0].DefaultView;
                System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
                str.Append(" type='" + ddlType.SelectedValue +"'");
                dv.RowFilter = str.ToString();

                gvData.DataSource = dv;
                gvData.DataBind();

            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("ddlType_SelectedIndexChanged >> " + ex.Message);
            }

        }
    }
}