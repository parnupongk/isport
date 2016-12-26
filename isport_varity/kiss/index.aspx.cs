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

namespace isport_varity.kiss
{
    public partial class index : System.Web.UI.Page
    {
        //=====================================================================================================
        //1. ใช้ mp_code
        //
        //
        //=====================================================================================================
        #region Parameter
        MobileUtilities mU = null;
        private string imgControl = "";
        #endregion

        #region Property
        private string bPropertySGID;
        public string bProperty_SGID
        {
            get { return bPropertySGID == null ? "261" : bPropertySGID; }
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
            get { return bPropertyPRJ == null ? "50" : bPropertyPRJ; }
            set { bPropertyPRJ = value; }
        }
        private string bPropertyUSERAGENT;
        public string bProperty_USERAGENT
        {
            get { return bPropertyUSERAGENT == null ? Request.ServerVariables["HTTP_USER_AGENT"] : bPropertyUSERAGENT; }
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

        public string ProjectType
        {
            get
            {
                return Request["p"] == null ? "bb" : Request["p"];
            }
        }
        #endregion

        #region Check Parameter
        private void CheckParameter()
        {

            bProperty_LNG = Request["lng"] == null ? "L" : Request["lng"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
            bProperty_SGID = Request["sg"] == null ? "261" : Request["sg"];
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
            bProperty_PRJ = Request["prj"] == null ? "50" : Request["prj"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    /*
                     *          **** ที่ project type index=0 จะเป็นตัวบอกว่าหน้านั้นต้องทำการ check active หรือไม่ และ check active sms หรือ wap ****
                     */
                    

                    if (Request["m"] != null) Response.AppendCookie(new HttpCookie("User-Identity-Forward-msisdn", Request["m"]));
                    mU = Utilities.getMISDN(Request);

                    if (mU.mobileNumber == "" && Request["o"] == null && Request["m"] == null) Response.Redirect(ConfigurationManager.AppSettings["URLGetMSISDNKISS"] + Request.Url.Query, false);

                    ViewState["OperatorType"] = mU.mobileOPT;
                    bProperty_USERAGENT = Request.ServerVariables["HTTP_USER_AGENT"];

                    CheckParameter();
                    //GenHeader();
                    GenMenuHeader(bProperty_USERAGENT, mU.mobileOPT);
                    string[] projectType = Request["p"].Split('_');

                    PreGenContent(mU.mobileOPT, projectType[0],(projectType.Length > 1 ? projectType[1] : "" ));

                    //GenContent(mU.mobileOPT,Request["p"]);
                    GenFooter(mU.mobileOPT, projectType[0]);


                    bProperty_SGID = projectType.Length > 1 ? "278" : bProperty_SGID;

                    isport_service.ServiceWap_Logs.Subscribe_portal_log(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString(), mU.mobileNumber, bProperty_USERAGENT, bProperty_SGID
                         , mU.mobileOPT, bProperty_PRJ, bProperty_MPCODE, bProperty_SCSID);
                }
                catch (Exception ex)
                {
                    ExceptionManager.WriteError("Error >>" + ex.Message);
                }
            }
        }

        #region Content
        private void PreGenContent(string opt, string projectType,string kissOtherService)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["IsporttdedConnectionString"].ToString(), CommandType.StoredProcedure, "usp_wapisport_uiselectbyprojecttype"
                    , new SqlParameter[] { new SqlParameter("@projecttype", projectType) });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["ui_index"].ToString() == "0")
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                        // sms
                    string pssvId = ( kissOtherService == "" ) ? "795" : "797" ;
                    string check = ServiceSMS.MemberSms_CheckActive(ConfigurationManager.ConnectionStrings["IsporttdedPackConnection"].ToString(), pssvId, mU.mobileOPT, mU.mobileNumber); // Kiss Model 1
                    //check = (check == "Y") ? check : ServiceSMS.MemberSms_CheckActive(ConfigurationManager.ConnectionStrings["IsporttdedPackConnection"].ToString(), "797", mU.mobileOPT, mU.mobileNumber);  // Kiss Model 2
                            if (check == "Y")
                            {
                                GenContent(mU.mobileOPT, projectType);
                                GenContent(mU.mobileOPT, "kiss");
                            }
                            else
                            {
                                frmMain.Controls.Add(ServiceWapUI_GenControls.genText("", "ขออภัยค่ะ กรุณาสมัครบริการก่อนเข้าใช้งานค่ะ", "", "0", Request["p"].ToString(), false, ""));
                                if (Request["p"] != null )
                                {
                                    GenContent(mU.mobileOPT, "kiss_sub");
                                }
                                else GenContent(mU.mobileOPT, "sms");
                            }
                        

                   // else GenContent(mU.mobileOPT, projectType);
                }
                else GenContent(mU.mobileOPT, Request["p"]);//GenContent(mU.mobileOPT, projectType);

            }
            catch (Exception ex)
            {
                throw new Exception(" PreGenContent >> " + ex.Message);
            }
        }

        private void GenContent(string opt, string projectType)
        {
            // sg ต้องใส่ที่ link เลย
            string link = "http://wap.isport.co.th/isportui/redirect.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
        + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
        + "&scs_id=" + bProperty_SCSID;
            link += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";

            string pageMaster = "index.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
        + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
        + "&sg=" + bProperty_SGID + "&scs_id=" + bProperty_SCSID;
            pageMaster += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";

            frmMain.Controls.Add(new isport_service.ServiceWapUI_Content().GenContent(ConfigurationManager.ConnectionStrings["IsporttdedConnectionString"].ToString()
                , ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString()
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
                lblHeaderLow.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(ConfigurationManager.ConnectionStrings["IsporttdedConnectionString"].ToString(), opt, projectType, "", "index.aspx", "0"));
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
                string link = "../detail.aspx?" + "lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
                                + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                + "&sg=" + bProperty_SGID + "&scs_id=" + bProperty_SCSID
                                + "&pcat=";
                link += (Request["pcat"] == null ? "176" : Request["pcat"]);
                frmMain.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooterVarity(AppCode.strIsportPackConn, bProperty_SGID, "", "", link, Request["pcat"], "4", 3, true));
                frmMain.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooter(ConfigurationManager.ConnectionStrings["IsporttdedConnectionString"].ToString(), opt, "tdedlove", "", "index.aspx", "0", bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));
            }
            catch (Exception ex)
            {
                throw new Exception("GenFooter >>" + ex.Message);
            }
        }
        #endregion

    }
}
