using System;
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
    public partial class indexl : System.Web.UI.Page
    {
        //====================================================================================
        //
        //  ************************ ให้ใช่ index.aspx  ****************
        //
        //=====================================================================================

        //=====================================================================================================
        //1. ใช้ mp_code
        //=====================================================================================================
        #region Parameter
        MobileUtilities mU = null;
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
            get { return bPropertyPRJ == null ? "1" : bPropertyPRJ; }
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
            bProperty_MPCODE = Request["mp_code"] == null ? "0025" : (Request["mp_code"].Split(',').Length > 0) ? Request["mp_code"].Split(',')[0].ToString() : Request["mp_code"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
            bProperty_PRJ = Request["prj"] == null ? "1" : Request["prj"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
        }
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx?" + Request.QueryString);

            if (!IsPostBack)
            {
                try
                {
                    /*
                     * 
                     *          **** ที่ project type index=0 จะเป็นตัวบอกว่าหน้านั้นต้องทำการ check active หรือไม่ และ check active sms หรือ wap ****
                     * 
                     * 
                     */
                    mU = Utilities.getMISDN(Request);
                    ViewState["OperatorType"] = mU.mobileOPT;
                    bProperty_USERAGENT = Request.ServerVariables["HTTP_USER_AGENT"];
                    CheckParameter();
                    //GenHeader();
                    GenMenuHeader(bProperty_USERAGENT, mU.mobileOPT);
                    //GenNews();
                    string projectType = (Request["s"] != null && Request["s"] != "")? Request["s"] : Request["p"] == null ? "bb" : Request["p"]; // request["s"] = service game football
                    bProperty_SGID = (Request["s"] != null && Request["s"] != "") ? "269" : bProperty_SGID;
                    PreGenContent(mU.mobileOPT, projectType);

                    //GenContent(mU.mobileOPT,Request["p"]);
                    GenFooter(mU.mobileOPT, projectType);

                    bProperty_MPCODE = Request["p"].IndexOf("partner") > -1 ? "99" + Request["p"].Substring(7, 2) : bProperty_MPCODE;

                    new AppCodeOracle().Subscribe_portal_log( mU.mobileNumber, bProperty_USERAGENT, bProperty_SGID
                         , mU.mobileOPT, bProperty_PRJ, bProperty_MPCODE, bProperty_SCSID);
                }
                catch (Exception ex)
                {
                    ExceptionManager.WriteError("Error >>" + ex.Message);
                }
            }
        }
        #endregion

        #region Content
        private void PreGenContent(string opt, string projectType)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppMain.strConn, CommandType.StoredProcedure, "usp_wapisport_uiselectbyprojecttype"
                    , new SqlParameter[] { new SqlParameter("@projecttype", projectType) });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["ui_index"].ToString() == "0" )
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (dr["ui_ispaymentwap"] != null && dr["ui_ispaymentwap"].ToString() != "" && (bool)dr["ui_ispaymentwap"])
                    {

                        // wap
                        string[] ipCurrents = Request.ServerVariables["REMOTE_ADDR"].ToString().Split('.');
                        string ipCurrent = (ipCurrents.Length > 3 ) ? ipCurrents[0] +"."+ipCurrents[1] +"."+ipCurrents[2] : Request.ServerVariables["REMOTE_ADDR"].ToString() ;
                        string chargeCode50B = (bProperty_SGID == "95" || bProperty_SGID == "255") ? ConfigurationManager.AppSettings["DtacPID_ChageCode7Day"] : ConfigurationManager.AppSettings["DtacPID_ChageCode50Day"];
                        ServiceWap.WapCheckAllService(AppMain.strConnOracle, Response, chargeCode50B, ConfigurationManager.AppSettings["isportAllowIP"]
                            , ipCurrent, opt, mU.mobileNumber, bProperty_USERAGENT, bProperty_SGID, bProperty_SCSID, bProperty_MPCODE, bProperty_PRJ);

                        GenContent(mU.mobileOPT, projectType);

                    }
                    else if (dr["ui_ispaymentsms"] != null && dr["ui_ispaymentsms"].ToString() != ""  && (bool)dr["ui_ispaymentsms"])
                    {
                        // sms
                        if (Request["pssv_id"] != null && Request["pssv_id"] != "")
                        {
                            string check = ServiceSMS.MemberSms_CheckActive(ConfigurationManager.ConnectionStrings["IsportPackConnectionString"].ToString(), Request["pssv_id"], mU.mobileOPT, mU.mobileNumber);
                            if (check == "Y")
                            {
                                GenContent(mU.mobileOPT, projectType);
                            }
                            else
                            {
                                frmMain.Controls.Add(ServiceWapUI_GenControls.genText("", "ขออภัยค่ะ กรุณาสมัครบริการก่อนเข้าใช้งานค่ะ", "", "0", Request["p"].ToString(), false, ""));
                                if (projectType != null && (projectType == "sportbuffet_news" || projectType == "sportbuffet_program" || projectType == "sportbuffet_result"))
                                {
                                    GenContent(mU.mobileOPT, "sportbuffet_sub");
                                }
                                else GenContent(mU.mobileOPT, "sms");
                            }
                        }
                        else GenContent(mU.mobileOPT, "sms");
                    }
                    else if (projectType == "aiserr_payment")
                    {
                        GenContent(mU.mobileOPT, projectType);
                        frmMain.Controls.Add(ServiceWapUI_GenControls.genText("", Request["mess"], "", "0", projectType, false, ""));

                    }
                    else GenContent(mU.mobileOPT, projectType);
                }
                else GenContent(mU.mobileOPT, projectType);

            }
            catch (Exception ex)
            {
                throw new Exception(" PreGenContent >> "+ex.Message);
            }
        }

        private void GenContent(string opt,string projectType)
        {
            // sg ต้องใส่ที่ link เลย
            string link = "redirect.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
        + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
        + "&scs_id=" + bProperty_SCSID;
            link += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";

            string pageMaster = "indexl.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
        + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
        + "&sg=" + bProperty_SGID + "&scs_id=" + bProperty_SCSID;
            pageMaster += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";

            frmMain.Controls.Add(new isport_service.ServiceWapUI_Content().GenContent(AppMain.strConn
                , AppMain.strConnOracle
                , Request["mid"] == null ? "0" : Request["mid"]
                , Request["level"] == null ? "0" : Request["level"]
                , opt, projectType, link, pageMaster, Request["class_id"]));
        
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

                    //divMenuHeader_low.Attributes.Add("style", "display:block");
                }
                else
                {
                    divMenuHeader.Attributes.Add("style", "display:block");
                    //divMenuHeader_low.Attributes.Add("style", "display:none");

                }
                string projectType = Request["p1"] == null ? (Request["p"] == null ? "" : Request["p"]) : Request["p1"]; // p1 มาจาก confirm.aspx เพื่อ show banner ของแต่ละ pack
                lblHeaderLow.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, opt, projectType, "", "indexl.aspx", "0"));
                //else 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       

        #endregion

        #region Footer
        private void GenFooter(string opt, string projectType)
        {
            try
            {
                lblFooter.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooter(AppMain.strConn, opt, projectType, "", "indexl.aspx", "0", bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));
            }
            catch (Exception ex)
            {
                throw new Exception("GenFooter >>" + ex.Message);
            }
        }
        #endregion

    }
}
