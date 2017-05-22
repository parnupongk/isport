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
    public partial class index : System.Web.UI.Page
    {
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

            bProperty_LNG = Request["lng"] == null ? "L" : Request["lng"] ;
            bProperty_SGID = Request["sg"] == null ? "" : Request["sg"];
            if (Request["scs_id"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["scs_id"], @"^[a-zA-Z'.\s]$"))
            {
                bProperty_SCSID = Request["scs_id"].Replace(@"^[a-zA-Z'.\s]$", "");
            }
            else
            {
                bProperty_SCSID = Request["scs_id"] != null && Request.QueryString["scs_id"].Length > 1 ? Request.QueryString.GetValues("scs_id")[0] : Request["scs_id"];//Request["scs_id"];
            }
            bProperty_SIZE = Request["size"] == null ? "" : Request["size"] ;
            //bProperty_MP = Request["mp"] == null ? "0025" : Request["mp"].Replace(@"^[a-zA-Z'.\s]$", ""); ไม่ใช่ mp ให้ไปใช้ mp_code
            bProperty_MPCODE = Request["mp_code"] == null ? "0025" : (Request["mp_code"].Split(',').Length > 0) ? Request["mp_code"].Split(',')[0].ToString() : Request["mp_code"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
            bProperty_PRJ = Request["prj"] == null ? "1" : Request["prj"];
        }
        #endregion

        #region page load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if( Request["p1"] != null && ( Request["p1"] == "step3_soccer" || Request["p1"]== "step3_love" || Request["p1"] == "step3_edtguide"))
                    {
                        Response.Redirect("http://wap.isport.co.th/isportui/landing/dtac/"+Request["p1"]+".html",false);
                    }

                    lnkMain.HRef = "index.aspx?p=bb&" + Request.QueryString.ToString().Replace("p=","p1=");
                    mU = Utilities.getMISDN(Request);
                    ViewState["OperatorType"] = mU.mobileOPT;
                    bProperty_USERAGENT = Request.ServerVariables["HTTP_USER_AGENT"];
                    CheckParameter();
                    if (mU.mobileOPT == "04" || mU.mobileOPT == "03" || mU.mobileOPT == "02") divClose.Attributes.Add("style", "display:;");
                    string projectType = (Request["s"] != null && Request["s"] != "") ? Request["s"] : Request["p"] == null ? "bb" : Request["p"]; // request["s"] = service game football
                    bProperty_SGID = (Request["s"] != null && Request["s"] != "") ? "269" : bProperty_SGID;

                    GenHeader(mU.mobileOPT, Request["p1"] == null ? (Request["p"] == null ? "" : Request["p"]) : Request["p1"]); // p1 มาจาก confirm.aspx เพื่อ show banner ของแต่ละ pack
                    if(Request["p"] == null || Request["p"] == "bb"  )GenNews();
                    else
                    {
                        divNews.Attributes.Add("style", "display:none");
                    }

                    

                    PreGenContent(mU.mobileOPT, projectType);
                    GenFooter(mU.mobileOPT, projectType);

                    new AppCodeOracle().Subscribe_portal_log(mU.mobileNumber, Request["p"]+" | "+bProperty_USERAGENT, bProperty_SGID
                         , mU.mobileOPT, bProperty_PRJ, bProperty_MPCODE, bProperty_SCSID);
                }
                catch (Exception ex)
                {
                    ExceptionManager.WriteError("Error >>" + ex.Message);
                }
            }
        }

        #endregion

        #region Header
        private void GenNews()
        {
            try
            {
                string url = "", classId = "", headNews = "", detailNews = "";
                classId = (classId == null || classId == "" ? "42" : classId);
                string[] str = classId.Split(',');
                classId = str.Length > 0 ? str[0] : classId;
                DataTable dt = new isport_service.ServiceNews().News_SelectOracle(AppMain.strConnOracle, "6", classId);
                for (int index = 0; index < dt.Rows.Count; index++)
                {
                    DataRow drContent = dt.Rows[index];
                    url = "detail.aspx?" + "pcnt_id=" + drContent["news_id"].ToString() + ((HttpContext.Current.Request.QueryString.ToString() == "") ? "" : "&" + HttpContext.Current.Request.QueryString);//"&sg=" + dr["ui_sg_id"].ToString()
                    // + "&class_id=" + classId;//+ "&scs_id=" + bProperty_SCSID + "&mp_code=" + bProperty_MPCODE + "&prj=" + bProperty_PRJ + "&p=" + Request["p"];

                    if (index == 0)
                    {
                        headNews = "<div class='thumbnail'><a href='" + url + "'>"
                            + "<img class='img-full' src='http://sms-gw.samartbug.com/isportimage/images/500x300/" + drContent["news_images_600"].ToString() + "'>"
                            + "<div class='caption'><h4 class='media-heading'>" + drContent["news_header_th"].ToString() + "</h4></a>"
                            + "<p>" + drContent["news_title_th"].ToString() + "</p>"
                            + "<small-gray><i class='fa fa-clock-o fa-1' aria-hidden='true'></i>" + DateTime.ParseExact(drContent["news_createdate"].ToString(), "M/d/yyyy h:mm:ss tt", null).ToString("d/MMM/yy H:s") + "</small-gray></div></div>";



                    }
                    else
                    {
                        detailNews += "<div class='media'><a class='pull-left' href='" + url + "'>"
                            + "<img src='http://sms-gw.samartbug.com/isportimage/images/120x75/" + drContent["news_images_190"].ToString() + "'></a>"
                            + "<div class='media-body'><h4 small>" + drContent["news_header_th"].ToString() + "</h4 small>"
                            + "<div class='media-button'><small-gray><i class='fa fa-clock-o fa-1' aria-hidden='true'></i>" + DateTime.ParseExact(drContent["news_createdate"].ToString(), "M/d/yyyy h:mm:ss tt", null).ToString("d/MMM/yy H:s") + "</small-gray></div></div></div>";

                    }


                }

                //lblNews.Controls.AddAt(lblNews.Controls.Count, new LiteralControl("<div class='row'><div class='page-header'><h1><img src='images/news-icon.png' alt='isport'/>ข่าว</h1></div>"));
                lblNews.Controls.AddAt(lblNews.Controls.Count, new LiteralControl("<div class='col-md-5'>" + headNews + "</div>"));
                lblNews.Controls.AddAt(lblNews.Controls.Count, new LiteralControl("<div class='col-md-4'>" + detailNews + "</div>"));
                //lblNews.Controls.AddAt(lblNews.Controls.Count, new LiteralControl("<div class='col-md-3'><div class='fb - page' data-href='https://www.facebook.com/isportclub' data-small-header='false' data-adapt-container-width='true' data-hide-cover='false' data-show-facepile='true' data-show-posts='false'><div class='fb-xfbml-parse-ignore'><blockquote cite='https://www.facebook.com/isportclub'><a href='https://www.facebook.com/isportclub'>Isportclub</a></blockquote></div></div>  </div>"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void GenHeader(string opt,string projectType)
        {
            try
            {
                lblAds.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, opt, "isport_ads", "", "index.aspx", "0"));
                lblHeader.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, opt, projectType, "", "index.aspx", "0"));

            }
            catch (Exception ex)
            {
                throw new Exception("GenHeader >> " + ex.Message);
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
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["ui_index"].ToString() == "0")
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (dr["ui_ispaymentwap"] != null && dr["ui_ispaymentwap"].ToString()  != "" && (bool)dr["ui_ispaymentwap"])
                    {

                        // wap
                        string[] ipCurrents = Request.ServerVariables["REMOTE_ADDR"].ToString().Split('.');
                        string ipCurrent = (ipCurrents.Length > 3) ? ipCurrents[0] + "." + ipCurrents[1] + "." + ipCurrents[2] : Request.ServerVariables["REMOTE_ADDR"].ToString();
                        string chargeCode50B = (bProperty_SGID == "95" || bProperty_SGID == "255") ? ConfigurationManager.AppSettings["DtacPID_ChageCode7Day"] : ConfigurationManager.AppSettings["DtacPID_ChageCode50Day"];
                        ServiceWap.WapCheckAllService(AppMain.strConnOracle, Response, chargeCode50B, ConfigurationManager.AppSettings["isportAllowIP"]
                            , ipCurrent, opt, mU.mobileNumber, bProperty_USERAGENT, bProperty_SGID, bProperty_SCSID, bProperty_MPCODE, bProperty_PRJ);

                        GenContent(mU.mobileOPT, projectType);

                    }
                    else if (dr["ui_ispaymentsms"] != null && dr["ui_ispaymentsms"].ToString() != "" && (bool)dr["ui_ispaymentsms"])
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
                                if (Request["p"] != null && (projectType == "sportbuffet_news" || projectType == "sportbuffet_program" || projectType == "sportbuffet_result"))
                                {
                                    GenContent(mU.mobileOPT, "sportbuffet_sub");
                                }
                                else GenContent(mU.mobileOPT, "sms");
                            }
                        }
                    }
                    else GenContent(mU.mobileOPT, projectType);
                }
                else if (projectType == "aiserr_payment")
                {
                    GenContent(mU.mobileOPT, projectType);
                    frmMain.Controls.Add(ServiceWapUI_GenControls.genText("", Request["mess"], "", "0", projectType, false, ""));
                    
                }
                else GenContent(mU.mobileOPT, projectType);

            }
            catch (Exception ex)
            {
                throw new Exception(" PreGenContent >> " + ex.Message);
            }
        }

        private void GenContent(string opt, string projectType)
        {
            try
            {
                string link = "redirect.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
                                    + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                    + "&scs_id=" + bProperty_SCSID;
                         link += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";

                string pageMaster = "index.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
                                        + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                        + "&sg=" + bProperty_SGID + "&scs_id=" + bProperty_SCSID;
                pageMaster += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";

                if (projectType == "edtgame")
                {
                    DataSet ds = SqlHelper.ExecuteDataset(AppCode.strConnPack, CommandType.StoredProcedure, "usp_answer_EDTquiz"
                        , new SqlParameter[] {new SqlParameter("@cur_date", DateTime.Now.ToString("yyyyMMdd")) });
                    if( ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 )
                    {
                        foreach(DataRow dr in ds.Tables[0].Rows)
                        {
                            frmMain.Controls.Add(isport_service.ServiceWapUI_GenControls.genImagesHeader("EDT Quiz", dr["pic_path_70"].ToString(), "img-full", "0", projectType, false,""));
                            frmMain.Controls.Add(isport_service.ServiceWapUI_GenControls.genTextHeader("", dr["pcnt_detail_local"].ToString(), "EDT Quiz", "0", projectType, false, ""));
                        }
                    }
                }
                else
                {
                    if( Request["pcat"] != null && Request["pcat"] == "301" )
                    {
                        frmMain.Controls.Add(isport_service.ServiceWapUI_GenControls.genTextHeader("", "รักเมืองไทยเที่ยวทั่วไทย", "", "0", projectType, false, ""));
                    }

                    frmMain.Controls.Add(new isport_service.ServiceWapUI_Content().GenContent(AppMain.strConn
                         , AppMain.strConnOracle
                        , Request["mid"] == null ? "0" : Request["mid"]
                        , Request["level"] == null ? "0" : Request["level"]
                        , opt, projectType, link, pageMaster, Request["class_id"]));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GenContent >> " + ex.Message);
            }
        }
        #endregion

        #region Footer
        private void GenFooter(string opt,string projectType)
        {
            try
            {

                lblFooter.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooter(AppMain.strConn, opt, projectType, "", "index.aspx", "0", bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));

                

            }
            catch (Exception ex)
            {
                throw new Exception("GenFooter >> " + ex.Message);
            }
        }
        #endregion
    }
}
