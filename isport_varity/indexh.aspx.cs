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

namespace isport_varity
{
    public partial class indexh : System.Web.UI.Page
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
            //if (Request["lng"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["lng"], @"^[a-zA-Z'.\s]{1,40}$"))
            //{
            // bProperty_LNG = "L";
            //}
            //else
            //{
            bProperty_LNG = Request["lng"] == null ? "L" : Request["lng"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
            //}

            //if (Request["sg"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["sg"], @"^[a-zA-Z'.\s]$"))
            //{
            bProperty_SGID = Request["sg"] == null ? "255" : Request["sg"].Replace(@"^[a-zA-Z'.\s]$", "");
            // }
            // else
            // {
            // bProperty_SGID = Request["sg"];
            //}

            if (Request["scs_id"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["scs_id"], @"^[a-zA-Z'.\s]$"))
            {
                bProperty_SCSID = Request["scs_id"].Replace(@"^[a-zA-Z'.\s]$", "");
            }
            else
            {
                bProperty_SCSID = Request["scs_id"] != null && Request.QueryString["scs_id"].Length > 1 ? Request.QueryString.GetValues("scs_id")[0] : Request["scs_id"];//Request["scs_id"];
            }

            //if (Request["size"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["size"], @"^[a-zA-Z'.\s]{1,40}$"))
            //{
            //bProperty_SIZE = "";
            //}
            //else
            //{
            bProperty_SIZE = Request["size"] == null ? "" : Request["size"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
            //}

            // if (Request["mp"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["mp"], @"^[a-zA-Z'.\s]$"))
            // {
            // bProperty_MP = "";
            //}
            // else
            //{
            bProperty_MP = Request["mp"] == null ? "0025" : Request["mp"].Replace(@"^[a-zA-Z'.\s]$", "");
            //}

            //if (Request["mp_code"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["mp_code"], @"^[a-zA-Z'.\s]{1,40}$"))
            //{
            //bProperty_MPCODE = "";
            //}
            //else
            //{
            bProperty_MPCODE = Request["mp_code"] == null ? "0025" : Request["mp_code"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
            //}

            //if (Request["prj"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["prj"], @"^[a-zA-Z'.\s]{1,40}$"))
            //{
            // bProperty_PRJ = "";
            // }
            // else
            //{
            bProperty_PRJ = Request["prj"] == null ? "50" : Request["prj"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
            //}
        }
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request["m"] != null) Response.AppendCookie(new HttpCookie("User-Identity-Forward-msisdn", Request["m"]));
                    MobileUtilities mU = Utilities.getMISDN(Request);

                   if (mU.mobileNumber == "" && Request["o"] == null && Request["m"] == null) Response.Redirect(ConfigurationManager.AppSettings["URLGetMSISDN"] + Request.Url.Query, false);

                    ViewState["OperatorType"] = mU.mobileOPT;
                    bProperty_USERAGENT = Request.ServerVariables["HTTP_USER_AGENT"];
                    CheckParameter();
                    GenMenuHeader(bProperty_USERAGENT, mU.mobileOPT);

                    string sexType = Request["stype"] == null ? "4" : Request["stype"];
                    GenContentHeader();
                    GenContent("176", sexType);
                    GenContentFooter(mU.mobileOPT);

                    try
                    {
                        isport_service.ServiceWap_Logs.Subscribe_portal_log(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString(), mU.mobileNumber, bProperty_USERAGENT
                            , "255", mU.mobileOPT, "1", "0025", "");
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.WriteError(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.WriteError("Error >>" + ex.Message);
                }
            }
        }
        #endregion

        #region Content
        private void GenContentHeader()
        {
            try
            {
                string link  =  "indexh.aspx?" + "lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
                                + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                + "&sg=" + bProperty_SGID + "&scs_id=" + bProperty_SCSID
                                + "&stype=";
                frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(@"<div class='row'>"
                + "<div class='col-xs-3 col-sm-3 col-md-3'><div class='alert alert-danger'><strong><a href='" + link + "4'>เรื่องจากทางบ้าน</a></strong></div></div>"
                + "<div class='col-xs-3 col-sm-3 col-md-3'><div class='alert alert-danger'><a href='" + link + "1'><strong>ประสบการณ์ใหม่</a></strong></div></div>"
                + "<div class='col-xs-3 col-sm-3 col-md-3'><div class='alert alert-danger'><strong><a href='" + link + "2'>เรื่องเด็ดมาแรง</a></strong></div></div>"
                + "<div class='col-xs-3 col-sm-3 col-md-3'><div class='alert alert-danger'><strong><a href='" + link + "3'>เรื่องรักเร้าใจ</a></strong></div></div>"
                + "</div>"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GenContentFooter(string opt)
        {
            try
            {
                string link = "indexh.aspx?" + "lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
                                + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                + "&sg=" + bProperty_SGID + "&scs_id=" + bProperty_SCSID
                                + "&stype=";
                frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(@"<div class='row'>
                <div class='col-xs-12 col-sm-12 col-md-12'><div class='alert alert-danger'><a href='" + link + "0'><strong>อ่านทั้งหมด..</a></strong></div></div></div>"));

                lblFooter.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooter(ConfigurationManager.ConnectionStrings["IsporttdedConnectionString"].ToString(), opt, "varity", "", "detail.aspx", "0", bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void GenContent(string pcatId,string sexType)
        {

            DataTable dt = (ViewState["VarityData"] == null) ? new AppCode().GetSipContentBypCatId(pcatId, sexType) : (DataTable)ViewState["VarityData"];
            ViewState["VarityData"] = dt;
            if (dt.Rows.Count > 0)
            {
                string link = "",strPaging = "";
                int start =0, end = 0;
                
                start = (Request["page"] ==null )? start : start + ((int.Parse(Request["page"])-1) * 10);
                end = (Request["page"] == null) ? 10 : end + ((int.Parse(Request["page"]) ) * 10);

                for (int i = start; i < end && i < dt.Rows.Count ; i++ )
                {
                    DataRow dr = dt.Rows[i];
                    link = "detail.aspx?" + "lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
                                + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                + "&sg=" + bProperty_SGID + "&scs_id=" + bProperty_SCSID;
                    link += "&pcnt_id=" + dr["pcnt_id"].ToString() + "&pcat=" + pcatId;
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row featurette'><img src='images/sexgroup" + sexType + ".png' class='img'><a class='img' href='" + link + "'>" + dr["name"].ToString() + "</a></div>"));
                }

                for (int i = 1; i < (dt.Rows.Count / 10); i++)
                {
                    link = "indexh.aspx?" + "lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
                                + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                + "&sg=" + bProperty_SGID + "&scs_id=" + bProperty_SCSID;
                    link += "&page=" + i + "&stype=" + sexType;
                    // paging
                    strPaging += (Request["page"] == i.ToString()) ? "<div class='col-xs-2 col-sm-2 col-md-2'><div class='alert alert-danger'><strong>" + i + "</strong></div></div>" 
                        : "<div class='col-xs-2 col-sm-2 col-md-2'><div class='alert alert-danger'><a href='" + link + "'><strong>" + i + "</a></strong></div></div>";
                    
                }
                if (strPaging != "") frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(@"<div class='rowheader'>"+strPaging+"</div>"));
            }
            else
            {
                frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(@"<div class='row'><div class='col-xs-12 col-sm-12 col-md-12'><strong>ไม่พบข้อมูลที่ต้องการ กรุณาเข้าใช้งานใหม่ภายหลังค่ะ..</strong></div></div>"));
            }
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
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
