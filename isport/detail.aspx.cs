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
using System.Text;
using WebLibrary;
using isport_service;
using Microsoft.ApplicationBlocks.Data;
namespace isport
{
    public partial class detailh : System.Web.UI.Page
    {

        #region Parameter
        private string imgControl = "";
        MobileUtilities mU = null;
        string vStrName = "";
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

            bProperty_LNG = Request["lng"] == null ? "L" : Request["lng"];
            bProperty_SGID = Request["sg"] == null ? "" : Request["sg"];
            if (Request["scs_id"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["scs_id"], @"^[a-zA-Z'.\s]$"))
            {
                bProperty_SCSID = Request["scs_id"].Replace(@"^[a-zA-Z'.\s]$", "");
            }
            else
            {
                bProperty_SCSID = Request["scs_id"] != null && Request.QueryString["scs_id"].Length > 1 ? Request.QueryString.GetValues("scs_id")[0] : Request["scs_id"];//Request["scs_id"];
            }
            bProperty_SIZE = Request["size"] == null ? "" : Request["size"];
            //bProperty_MP = Request["mp"] == null ? "0025" : Request["mp"].Replace(@"^[a-zA-Z'.\s]$", ""); ไม่ใช่ mp ให้ไปใช้ mp_code
            bProperty_MPCODE = Request["mp_code"] == null ? "0025" : Request["mp_code"];
            bProperty_PRJ = Request["prj"] == null ? "53" : Request["prj"];
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mU = Utilities.getMISDN(Request);
                vStrName  = Request.ServerVariables["HTTP_USER_AGENT"];
                CheckParameter();
                GenMenuHeader(vStrName,mU.mobileOPT);
                GenHeader(mU.mobileOPT, Request["p"] == null ? "" : Request["p"]);
                if( Request["_id"] != null && Request["_id"] != "" )
                {
                    GenNewsDetail_FromSql(Request["_id"]);
                }
                else if (Request["pcnt_id"] != null && Request["pcnt_id"] != "")
                {
                    GenNewsDetail(Request["pcnt_id"], mU.mobileOPT, Request["p"] == null ? "" : Request["p"]);
                }
                
                GenFooter();
                GenPopupBanner(mU.mobileOPT, "popup");
                InsertLogs();
            }


        }


        #region Logs
        private void InsertLogs()
        {
            try
            {

                isport_service.ServiceWap_Logs.Subscribe_portal_log(AppCode.strConnOracle, mU.mobileNumber, vStrName, "134", mU.mobileOPT, bProperty_PRJ, bProperty_MPCODE, bProperty_SCSID);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InsertLogs >> " + ex.Message);
            }
        }
        #endregion

        #region PopupBanner
        private void GenPopupBanner(string opt, string projectType)
        {
            try
            {
               // if (WebLibrary.MitTool.GetCookie(Request, "popupbanner") == "")
                //{

                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div id='modal' class='modal'><div class='modal-header'>"));
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<button type='button' class='close' data-dismiss='modal' aria-hidden='true'><img src='/images/close_btn.png'/></button></div><div class='modal-body'>"));
                    frmMain.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, opt, projectType, "", "detail.aspx", "0"));
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("</div></div>"));

                    WebLibrary.MitTool.CreateCookie("popupbanner", Response, "popupbanner", 2);
                //}
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GenPopupBanner >> " + ex.Message);
            }

        }
        #endregion

        #region MenuHeader
        private void GenMenuHeader(string vStrName, string opt)
        {
            try
            {
                if (vStrName.ToLower().IndexOf("symbian") > 0 || Request.Browser.ScreenPixelsWidth < 320)
                {
                    divMenuHeader.Attributes.Add("style", "display:none");

                    //divMenuHeader_low.Attributes.Add("style", "display:block");
                    //lblHeaderLow.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, opt, "isport", "", "indexh.aspx", "0"));
                }
                else
                {
                    divMenuHeader.Attributes.Add("style", "display:block");
                    //divMenuHeader_low.Attributes.Add("style", "display:none");

                }
                //else 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Header
        private void GenHeader(string opt, string projectType)
        {
            try
            {

                frmMain.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, opt, projectType, "", "detail.aspx", "0"));

            }
            catch (Exception ex)
            {
                throw new Exception("GenHeader >> " + ex.Message);
            }
        }
        #endregion

        #region Gen News Detail

        private void GenNewsDetail_FromSql(string _id)
        {
            DataSet ds =SqlHelper.ExecuteDataset(AppCode.strConn, CommandType.StoredProcedure, "usp_wapisport_uiselectbycontentid"
                , new SqlParameter[] {new SqlParameter("@content_id",_id) });
            if (ds.Tables[0].Rows.Count > 0)
            {
                string imgURL =  ds.Tables[0].Rows[0]["content_image"].ToString();
                imgURL = imgURL.IndexOf("http") > -1 ? imgURL : "http://wap.isport.co.th/isportui/" + imgURL.Replace("~/", "");

                string[] detail = ds.Tables[0].Rows[0]["content_text"].ToString().Split('|');

                frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'><h1>ข่าวบอลไทย</h1></div></div>"));

                genImages(imgURL, "img-responsive");
                frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowheadertext'><h3>" + ((detail.Length > 0) ? detail[0] : "") + "</h3></div>"));
                frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row'><p>" + ((detail.Length > 1) ? detail[1] : ds.Tables[0].Rows[0]["content_text"].ToString()) + "</p></div>"));
                //frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row'><p>" + ReplaceTagHTML(ds.Tables[0].Rows[0]["news_detail_th1"].ToString()) + "</p></div>"));
            }
        }
        private void GenNewsDetail(string pcntId,string opt, string projectType)
        {
            try
            {
                string pcnt = pcntId.Split(',').Length > 0 ? pcntId.Split(',')[0] : pcntId;
                DataTable dt = new AppCode().News_SelectByIdOracle(pcnt);
                if (dt.Rows.Count > 0)
                {
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'><h1>ข่าวบอลไทย</h1></div></div>"));

                    genImages("http://sms-gw.samartbug.com/isportimage/images/325x200/" + dt.Rows[0]["news_images_350"], "img-responsive");
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowheadertext'><h3>" + dt.Rows[0]["news_header_th"] + "</h3></div>"));
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row'><p>" + dt.Rows[0]["news_title_th"] + "</p></div>"));
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row'><p>" + ReplaceTagHTML(dt.Rows[0]["news_detail_th1"].ToString()) + "</p></div>"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GenNewsDetail>>" + ex.Message);
            }
        }
        #endregion

        #region Footer
        private void GenFooter()
        {
            try
            {
                DataSet ds = new AppCode().SelectFooterByoperator(OperatorType, ProjectType);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string link = "redirect.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
                                    + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                    + "&sg=" + dr["footer_sg_id"].ToString() + "&scs_id=" + bProperty_SCSID
                                    + "&r=" + dr["content_link"].ToString().Replace('&', '|');
                    link += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";

                    if (dr["content_link"] != null && dr["content_link"].ToString() != "")
                    {
                        lblFooter.Controls.AddAt(lblFooter.Controls.Count, genLink(dr, link));
                    }
                    else
                    {

                        if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                        {
                            genImagesLink(dr,false);

                        }
                        else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                        {
                            genText(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GenFooter >>" + ex.Message);
            }
        }
        #endregion

        #region Gen Content
        private void genText(DataRow dr)
        {
            try
            {
                if (dr["content_icon"] != null && dr["content_icon"].ToString() != "")
                {
                    if ((bool)dr["ui_ismaster"])
                    {
                        string urlMaster = "indexh.aspx?mid=" + dr["ui_id"].ToString() + "&level=" + (Level + 1) + "&p=" + ProjectType;
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowheader'><a href=" + urlMaster + "><p><img class='img' src='" + dr["content_icon"].ToString().Replace("~/", "") + "'>" + dr["content_text"].ToString() + "</p></a></div>"));
                    }
                    else
                    {
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row'><p><img class='img' src='" + dr["content_icon"].ToString().Replace("~/", "") + "'>" + dr["content_text"].ToString() + "</p></div>"));
                    }
                }
                else
                {
                    if ((bool)dr["ui_ismaster"])
                    {
                        string urlMaster = "indexh.aspx?mid=" + dr["ui_id"].ToString() + "&level=" + (Level + 1) + "&p=" + ProjectType;
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowheader'><a href=" + urlMaster + "><p>" + dr["content_text"].ToString() + "</p></a></div>"));
                    }
                    else
                    {
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row'><p>" + dr["content_text"].ToString() + "</p></div>"));
                    }
                }
            }
            catch (Exception ex)
            {
                new Exception("genText >> " + ex.Message);
            }
        }
        private void genImages(string imgSrc,string imgClass)
        {
            try
            {
                
                frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row'><img class='"+imgClass+"' src='" + imgSrc + "'></div>"));

            }
            catch (Exception ex)
            {
                throw new Exception("genImages >> " + ex.Message);
            }
        }
        private void genImagesLink(DataRow dr,bool isMaster)
        {
            try
            {
                if (isMaster)
                {
                    string urlMaster = "indexl.aspx?mid=" + dr["ui_id"].ToString() + "&level=" + (Level + 1) + "&p=" + ProjectType;
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowheader'><a href=" + urlMaster + "><img class='img-responsive' src='" + dr["content_image"].ToString().Replace("~/", "") + "'></a></div>"));
                }
                else
                {
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowheader'><h4>" + dr["content_text"].ToString() + "</h4></div>"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("genImages >> " + ex.Message);
            }
        }
        private Control genLink(DataRow dr, string link)
        {
            try
            {
                Control crt = new Control();
                // Gen Link
                if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                {


                    if ((bool)dr["content_breakafter"])
                    {
                        if (imgControl == "")
                        {
                            crt.Controls.AddAt(crt.Controls.Count, new LiteralControl("<div class='row featurette'><a href='" + link + "'><div class=col-xs-4 col-sm-4 col-lg-4 col-md-5><img class='img' src='" + dr["content_image"].ToString().Replace("~/", "") + "'></div><div class=col-xs-8 col-sm-8 col-lg-8 col-md-7>" + dr["content_text"] + "</div></a></div>"));
                            imgControl = "";
                        }
                        else
                        {
                            imgControl += "<a href='" + dr["content_link"].ToString() + "'><img class='img' src='" + dr["content_image"].ToString().Replace("~/", "") + "'></a>";
                            crt.Controls.AddAt(crt.Controls.Count, new LiteralControl("<div class='row'>" + imgControl + "</div>"));
                        }
                        imgControl = "";
                    }
                    else
                    {
                        imgControl += "<a href='" + dr["content_link"].ToString() + "'><img class='img' src='" + dr["content_image"].ToString().Replace("~/", "") + "'></a>";
                    }

                }
                else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                {
                    if (dr["content_icon"] != null && dr["content_icon"].ToString() != "")
                    {
                        crt.Controls.AddAt(crt.Controls.Count, new LiteralControl("<div class='row featurette'><img class='img' src='" + dr["content_icon"].ToString().Replace("~/", "") + "'><a class='img' href='" + link + "'>" + dr["content_text"].ToString() + "</a></div>"));
                    }
                    else
                    {
                        crt.Controls.AddAt(crt.Controls.Count, new LiteralControl("<div class='row featurette'><a  href='" + link + "'>" + dr["content_text"].ToString() + "</a></div>"));
                    }

                }
                return crt;
            }
            catch (Exception ex)
            {
                throw new Exception(" genLink>> " + ex.Message);
            }
        }
        private void genNews(DataRow dr)
        {
            bool isNews = dr["ui_isnews"].ToString() == "" ? false : (bool)dr["ui_isnews"];
            if (isNews)
            {
                // Gen News
                string url = "";
                string class_id = (Request["class_id"] == null ? "1" : Request["class_id"]);
                DataTable dt = new AppCode().News_SelectOracle(dr["ui_isnews_top"].ToString(), class_id);
                foreach (DataRow drContent in dt.Rows)
                {

                    url = "../football_news_detail.aspx?pcnt_id=" + drContent["news_id"].ToString() + "&sg=" + dr["ui_sg_id"].ToString()
                        + "&class_id=" + class_id + "&scs_id=" + bProperty_SCSID + "&mp_code=" + bProperty_MPCODE + "&prj=" + bProperty_PRJ + "&p=" + Request["p"];

                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row featurette'><a href='" + url + "'><div class=col-xs-4 col-sm-4 col-lg-4 col-md-5><img class='img' src='" + ConfigurationManager.AppSettings["isportPathNewsImages"] + drContent["news_images_190"].ToString() + "'></div><div class=col-xs-8 col-sm-8 col-lg-8 col-md-7>" + drContent["news_header_th"].ToString() + "</div></a></div>"));



                }
            }
        }
        public static string ReplaceTagHTML(string detail)
        {
            try
            {
                string rtn = detail;
                if (detail.IndexOf("<") > -2)
                {
                    if (detail.IndexOf("<") == 0)
                    {
                        rtn = rtn.Substring(detail.IndexOf(">") + 1);
                    }
                    else if (detail.IndexOf(">") > -1)
                    {
                        rtn = rtn.Substring(0, detail.IndexOf("<") - 1) + rtn.Substring(detail.IndexOf(">") + 1);
                    }
                    else
                    {
                        rtn = rtn.Substring(0, detail.IndexOf("<") - 1);
                    }
                    rtn = ReplaceTagHTML(rtn);
                }
                return rtn;
            }
            catch
            {
                return detail;
            }
        }
        #endregion
    }
}
