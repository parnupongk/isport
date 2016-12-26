using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Configuration;
using MobileLibrary;
using WebLibrary;
using OracleDataAccress;
namespace isport_varity
{
    public partial class detail : System.Web.UI.Page
    {
        #region Property
        private string bPropertySGID;
        public string bProperty_SGID
        {
            get { return bPropertySGID == null ? "255" : bPropertySGID; }
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
            get { return bPropertyUSERAGENT == null ? "" : bPropertyUSERAGENT; }
            set { bPropertyUSERAGENT = value; }
        }
        #endregion

        #region Check Parameter
        private void CheckParameter()
        {
            try
            {
                bProperty_LNG = Request["lng"] == null ? "L" : Request["lng"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
                bProperty_SGID = Request["sg"] == null || Request["sg"]=="" ? "255" : Request["sg"];

                if (Request["scs_id"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["scs_id"], @"^[a-zA-Z'.\s]$"))
                {
                    bProperty_SCSID = Request["scs_id"].Replace(@"^[a-zA-Z'.\s]$", "");
                }
                else
                {
                    bProperty_SCSID = Request["scs_id"] != null && Request.QueryString["scs_id"].Length > 1 ? Request.QueryString.GetValues("scs_id")[0] : Request["scs_id"];//Request["scs_id"];
                }
                bProperty_SIZE = Request["size"] == null ? "" : Request["size"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
                bProperty_MP = Request["mp"] == null ? "0025" : Request["mp"].Replace(@"^[a-zA-Z'.\s]$", "");
                bProperty_MPCODE = Request["mp_code"] == null ? "0025" : Request["mp_code"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
                bProperty_PRJ = Request["prj"] == null ? "50" : Request["prj"].Replace(@"^[a-zA-Z'.\s]$", "");
            }
            catch (Exception ex)
            {
                throw new Exception("CheckParameter >> " + ex.Message);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                if (Request["m"] != null) Response.AppendCookie(new HttpCookie("User-Identity-Forward-msisdn", Request["m"]));
                MobileUtilities mU = Utilities.getMISDN(Request);
                try
                {
                    
                    if (mU.mobileNumber == "" && Request["o"] == null && Request["m"] == null) Response.Redirect(ConfigurationManager.AppSettings["URLGetMSISDNDetail"] + Request.Url.Query, false);
                    //ViewState["OperatorType"] = mU.mobileOPT;
                    //frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(@"<div class='rowheader'>" + mU.mobileNumber + "</div>"));
                    string vStrName = Request.ServerVariables["HTTP_USER_AGENT"];
                    CheckParameter();
                    GenMenuHeader(vStrName, mU.mobileOPT);
                    GenContent(mU.mobileOPT, mU.mobileNumber, vStrName);

                }
                catch (Exception ex)
                {
                    ExceptionManager.WriteError(ex.Message);
                }
                finally
                {
                    GenFooter(mU.mobileOPT);
                }
            }
        }

        #region Header
        private void GenMenuHeader(string vStrName, string opt)
        {
            try
            {
                if (vStrName.ToLower().IndexOf("symbian") > 0 || Request.Browser.ScreenPixelsWidth < 320)
                {
                    divMenuHeader.Attributes.Add("style", "display:none");
                    //myCarousel.Attributes.Add("style", "display:none");
                    divMenuHeader_low.Attributes.Add("style", "display:block");
                    lblHeaderLow.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(ConfigurationManager.ConnectionStrings["IsporttdedConnectionString"].ToString(), opt, "varity", "", "indexh.aspx", "0"));
                }
                else
                {
                    divMenuHeader.Attributes.Add("style", "display:block");
                    divMenuHeader_low.Attributes.Add("style", "display:none");
                    //myCarousel.Attributes.Add("style", "display:block");
                    frmMain.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(ConfigurationManager.ConnectionStrings["IsporttdedConnectionString"].ToString(), opt, "varity", "", "indexh.aspx", "0"));
                }
                //else 
            }
            catch (Exception ex)
            {
                throw new Exception("GenMenuHeader >> " + ex.Message);
            }
        }
        #endregion

        #region Content
        private void GenContent(string opt,string msisdn,string userAgent)
        {
            try
            {
                DataSet ds = OrclHelper.Fill(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString(), CommandType.StoredProcedure, "Wap_UI.SIP_Content_Varitybypcnt", "i_sip_content"
                    , new OracleParameter[] {OrclHelper.GetOracleParameter("p_pcnt",Request["pcnt_id"],OracleType.VarChar,ParameterDirection.Input)
                                                          ,OrclHelper.GetOracleParameter("o_content","",OracleType.Cursor,ParameterDirection.Output)});
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string[] ipCurrents = Request.ServerVariables["REMOTE_ADDR"].ToString().Split('.');
                    string ipCurrent = (ipCurrents.Length > 3 ) ? ipCurrents[0] +"."+ipCurrents[1] +"."+ipCurrents[2] : Request.ServerVariables["REMOTE_ADDR"].ToString() ;

                    isport_service.ServiceWap.WapCheckAllService(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString(), Response, "45110531001"
                        , ConfigurationManager.AppSettings["IsportAllowIP"], ipCurrent, opt, msisdn, userAgent, bProperty_SGID, Request["pcnt_id"], bProperty_MPCODE, bProperty_PRJ);

                    //isport_service.ServiceWap.WapCheckAllService(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString(), Response, "45110531001"
                     //   , ConfigurationManager.AppSettings["IsportAllowIP"], ipCurrent, opt, msisdn, userAgent, "260", Request["pcnt_id"], bProperty_MPCODE, bProperty_PRJ);

                    // Show Content
                    frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", ds.Tables[0].Rows[0]["TITLE_LOCAL"].ToString(), "detail.aspx", "0", "varity", false, ""));
                    frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genText("", ds.Tables[0].Rows[0]["DETAIL_LOCAL"].ToString(), "detail.aspx", "0", "varity", false, ""));

                    // Paging
                    string topic = ds.Tables[0].Rows[0]["TOPIC"].ToString().Substring(0, ds.Tables[0].Rows[0]["TOPIC"].ToString().IndexOf("ตอนที่")).Trim() + "%";
                    ds = OrclHelper.Fill(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString(), CommandType.StoredProcedure, "Wap_UI.SIP_Content_VaritybyCatId", "i_sip_content"
                        , new OracleParameter[] { OrclHelper.GetOracleParameter("p_catid", Request["pcat"] == null ? "176": Request["pcat"] , OracleType.VarChar, ParameterDirection.Input) 
                                                            , OrclHelper.GetOracleParameter("p_pcnttitle", topic, OracleType.VarChar, ParameterDirection.Input) 
                                                            , OrclHelper.GetOracleParameter("o_Content", "", OracleType.Cursor, ParameterDirection.Output)});

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string link = "", strPaging="";
                        for (int i = 0 ; i < ds.Tables[0].Rows.Count ; i++)
                        {

                            link = "detail.aspx?" + "lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
                                + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                + "&sg=" + bProperty_SGID + "&scs_id=" + bProperty_SCSID
                                + "&pcnt_id=" + ds.Tables[0].Rows[i]["pcnt_id"].ToString() + "&pcat=";
                            link +=  (Request["pcat"] == null ? "176" : Request["pcat"]);

                            //frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(@"<div class='rowheader'>" + link + "</div>"));
                            // paging
                            strPaging += (Request["pcnt_id"] == ds.Tables[0].Rows[i]["pcnt_id"].ToString()) ? "<div class='col-xs-2 col-sm-2 col-md-2'><div class='alert alert-danger'><strong>" + (i + 1) + "</strong></div></div>"
                                : "<div class='col-xs-2 col-sm-2 col-md-2'><div class='alert alert-danger'><a href='" + link + "'><strong>" + (i+1) + "</a></strong></div></div>";

                        }
                        if (strPaging != "") frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(@"<div class='rowheader'>" + strPaging + "</div>"));
                    }
                }

                //frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(@"<div class='rowheader'>" + strPaging + "</div>"));
            }
            catch (Exception ex)
            {
                throw new Exception("GenContent >> " + ex.Message);
            }
        }
        #endregion


        #region Footer
        private void GenFooter(string opt)
        {
            try
            {
                string link = "detail.aspx?" + "lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
                                + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                + "&sg=" + bProperty_SGID + "&scs_id=" + bProperty_SCSID
                                + "&pcat=";
                link += (Request["pcat"] == null ? "176" : Request["pcat"]);
                frmMain.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooterVarity(AppCode.strIsportPackConn, bProperty_SGID, "", "", link, Request["pcat"], "4", 3, true));
                lblFooter.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooter(AppCode.strIsportConn, opt, "varity", "", "detail.aspx", "0", bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));
            }
            catch (Exception ex)
            {
                throw new Exception("GenFooter >> " + ex.Message);
            }
        }
        #endregion

    }
}
