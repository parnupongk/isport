﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MobileLibrary;
using WebLibrary;
using isport_service;
using Microsoft.ApplicationBlocks.Data;

namespace isport
{
    public partial class football_result_detail : System.Web.UI.Page
    {

        #region Parameter
        private string imgControl = "";
        #endregion

        #region Property
        private string bPropertySGID;
        public string bProperty_SGID
        {
            get { return bPropertySGID == null ? "" : bPropertySGID; }
            set { bPropertySGID = value; }
        }
        private string bPropertyLNG;
        public string bProperty_LNG
        {
            get { return bPropertyLNG == null ? "L" : bPropertyLNG; }
            set { bPropertyLNG = value; }
        }
        private string bPropertySCSID;
        public string bProperty_SCSID
        {
            get { return bPropertySCSID == null ? "" : bPropertySCSID; }
            set { bPropertySCSID = value; }
        }
        public string bPropertySIZE;
        public string bProperty_SIZE
        {
            get { return bPropertySIZE == null ? "N" : bPropertySIZE; }
            set { bPropertySIZE = value; }
        }
        private string bPropertyMP;
        public string bProperty_MP
        {
            get { return bPropertyMP == null ? "0025" : bPropertyMP; }
            set { bPropertyMP = value; }
        }
        private string bPropertyMPCODE;
        public string bProperty_MPCODE
        {
            get { return bPropertyMPCODE == null ? "0025" : bPropertyMPCODE; }
            set { bPropertyMPCODE = value; }
        }

        private string bPropertyPRJ;
        public string bProperty_PRJ
        {
            get { return bPropertyPRJ == null ? "" : bPropertyPRJ; }
            set { bPropertyPRJ = value; }
        }
        private string bPropertyUSERAGENT;
        public string bProperty_USERAGENT
        {
            get { return bPropertyUSERAGENT == null ? "" : bPropertyUSERAGENT; }
            set { bPropertyUSERAGENT = value; }
        }
        public string MasterID
        {
            get
            {
                return Request["mid"] == null ? "0" : Request["mid"].ToString();
            }
        }
        public int Level
        {
            get
            {
                return Request["Level"] == null ? 0 : int.Parse(Request["Level"].ToString());
            }
        }

        public string OperatorType
        {
            get
            {
                return ViewState["OperatorType"] == null || ViewState["OperatorType"] == "" ? AppCode.OperatorType.All.ToString() : ViewState["OperatorType"].ToString();
            }
        }
        public string ProjectType
        {
            get
            {
                return Request["p"] == null ? AppCode.ProjectType.bb.ToString() : Request["p"];
            }
        }
        #endregion

        #region Check Parameter
        private void CheckParameter()
        {

            bProperty_LNG = Request["lng"] == null ? "L" : Request["lng"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
            bProperty_SGID = Request["sg"] == null ? "" : Request["sg"];
            if (Request["scs_id"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["scs_id"], @"^[a-zA-Z'.\s]$"))
            {
                bProperty_SCSID = Request["scs_id"].Replace(@"^[a-zA-Z'.\s]$", "");
            }
            else
            {
                bProperty_SCSID = Request["scs_id"] != null && Request.QueryString["scs_id"].Length > 1 ? Request.QueryString.GetValues("scs_id")[0] : Request["scs_id"];//Request["scs_id"];
            }
            bProperty_SIZE = Request["size"] == null ? "" : Request["size"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
            //bProperty_MP = Request["mp"] == null ? "0025" : Request["mp"].Replace(@"^[a-zA-Z'.\s]$", ""); ไม่ใช่ mp ให้ไปใช้ mp_code
            bProperty_MPCODE = Request["mp_code"] == null ? "0025" : Request["mp_code"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
            bProperty_PRJ = Request["prj"] == null ? "1" : Request["prj"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
        }
        #endregion

        #region Header
        private void GenMenuHeader(string vStrName, string opt)
        {
            try
            {

                if (vStrName.ToLower().IndexOf("symbian") > 0 || Request.Browser.ScreenPixelsWidth < 320)
                {
                    divMenuHeader.Attributes.Add("style", "display:none");
                }
                else
                {
                    divMenuHeader.Attributes.Add("style", "display:block");
                }
                string projectType = Request["p1"] == null ? (Request["p"] == null ? "" : Request["p"]) : Request["p1"]; // p1 มาจาก confirm.aspx เพื่อ show banner ของแต่ละ pack
                lblHeaderLow.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, opt, projectType, "", "football_static.aspx", "0"));
                //else 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Content
        private void GenContent(string opt, string projectType)
        {
            string link = "football_static.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
        + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
        + "&sg=" + bProperty_SGID + "&scs_id=" + bProperty_SCSID;
            link += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";
            link += (Request["msch_id"] != null && link.IndexOf("msch_id") < 0) ? "&msch_id=" + Request["msch_id"] : "";

            string pageMaster = "indexl.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
        + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
        + "&sg=" + bProperty_SGID + "&scs_id=" + bProperty_SCSID;
            pageMaster += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";

            // get match info
            DataSet ds = SqlHelper.ExecuteDataset(AppMain.strConn, CommandType.StoredProcedure, "usp_BB_football_livescore_selectbymschid"
                , new SqlParameter[] { new SqlParameter("@msch_id", Request["msch_id"]) });
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                DataRow dr = ds.Tables[0].Rows[0];
                string str = "<div class='rowcenter'><div class='col-xs-12 col-sm-12 col-lg-12'>" + dr["result_time"] +" " + dr["teamcode1th"].ToString() + " "  + dr["result_team_code1"] + "-" + dr["result_team_code2"] + " " + dr["teamcode2th"].ToString() + "</div></div>";
                frmMain.Controls.Add(new LiteralControl(str));

                // สรุปผล
                frmMain.Controls.Add(new LiteralControl("<hr class=featurette-divider><div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'>รายละเอียด</div></div>"));
                DataSet ds5Last = SqlHelper.ExecuteDataset(AppMain.strConn, CommandType.StoredProcedure, "usp_wapui_selectmatchdetail"
                    , new SqlParameter[] { new SqlParameter("@msch_id", Request["msch_id"]) });
                if (ds5Last.Tables.Count > 0 && ds5Last.Tables[0].Rows.Count > 0)
                {
                    int index = 1; string cssName = "rowprogramdetail";
                    foreach (DataRow drLast in ds5Last.Tables[0].Rows)
                    {
                        cssName = (index % 2) == 0 ? "rowprogramdetail_" : "rowprogramdetail";
                        frmMain.Controls.Add(new LiteralControl("<div class='"+ cssName + "'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1></div><div class=col-xs-11 col-sm-11 col-lg-11 col-md-11>" + drLast["match_result_goal"] + "</div></a></div>"));
                        index++;
                    }
                }
                else frmMain.Controls.Add(new LiteralControl("<div class='rowcenter'><div class='col-xs-12 col-sm-12 col-lg-12'>ขออภัยค่ะ ไม่พบข้อมูลค่ะ!!</div></div>"));

            }
            else frmMain.Controls.Add(new LiteralControl("<div class='rowcenter'><div class='col-xs-12 col-sm-12 col-lg-12'>ขออภัยค่ะ ไม่พบข้อมูลค่ะ!!</div></div>"));
        }
        #endregion

        #region Footer
        private void GenFooter(string opt, string projectType)
        {
            try
            {
                frmMain.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooter(AppMain.strConn, opt, projectType, "", "football_static.aspx", "0", bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));
            }
            catch (Exception ex)
            {
                throw new Exception("GenFooter >>" + ex.Message);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    MobileUtilities mU = Utilities.getMISDN(Request);
                    ViewState["OperatorType"] = mU.mobileOPT;
                    string vStrName = Request.ServerVariables["HTTP_USER_AGENT"];
                    CheckParameter();
                    GenMenuHeader(vStrName, mU.mobileOPT);
                    if (Request["pssv_id"] != null && Request["pssv_id"] != "")
                    {
                        string check = ServiceSMS.MemberSms_CheckActive(ConfigurationManager.ConnectionStrings["IsportPackConnectionString"].ToString(), Request["pssv_id"], mU.mobileOPT, mU.mobileNumber);
                        if (check == "Y")
                        {
                            GenContent(mU.mobileOPT, Request["p"] == null ? "" : Request["p"]);
                        }
                        else
                        {
                            frmMain.Controls.Add(ServiceWapUI_GenControls.genText("", "ขออภัยค่ะ กรุณาสมัครบริการก่อนเข้าใช้งานค่ะ", "", "0", Request["p"].ToString(), false, ""));
                            GenContent(mU.mobileOPT, "sms");
                        }
                    }
                    else GenContent(mU.mobileOPT, Request["p"] == null ? "" : Request["p"]);

                    GenFooter(mU.mobileOPT, Request["p"] == null ? "" : Request["p"]);

                    new AppCodeOracle().Subscribe_portal_log(mU.mobileNumber, bProperty_USERAGENT, bProperty_SGID
                         , mU.mobileOPT, bProperty_PRJ, bProperty_MPCODE, bProperty_SCSID);
                }
                catch (Exception ex)
                {
                    ExceptionManager.WriteError("Error >>" + ex.Message);
                }
            }
        }

    }
}
