using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using MobileLibrary;
using WebLibrary;
using isport_service;
namespace isport_siamdara
{
    public partial class index : System.Web.UI.Page
    {

        MobileUtilities mU = null;

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
            get { return bPropertyPRJ == null ? "57" : bPropertyPRJ; }
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
            bProperty_SGID = Request["sg"] == null ? ConfigurationManager.AppSettings["SiamDaraClipSG"] : Request["sg"];
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
            bProperty_PRJ = Request["prj"] == null ? "57" : Request["prj"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
        }
        #endregion

        #region page load
        /// <summary>
        ///  Request[d] : siamdara clip Redirect to clip ( เอาลงเมื่อ  siamdara clip edit ขึ้น)
        ///  Request[p] : siamdata quiz game
        ///  Request[s] : siamdara clip edit ( 22/7/2014 ) เปลี่ยนเป็น ให้เข้ามาดูหน้า wap , เพิ่มบริการ ดาราแซบ โดนใช้ content เดิม
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Siamdata News
                if (Request["n"] != null) Response.Redirect("news.aspx?" + Request.QueryString,false);

                if (!IsPostBack)
                {
                    CheckParameter();
                    if (Request["m"] != null) Response.AppendCookie(new HttpCookie("User-Identity-Forward-msisdn", Request["m"]));
                    if (Request["o"] != null && Request["o"] != "") Response.AppendCookie(new HttpCookie("Plain-User-Identity-Forward-msisdn", Request["o"]));
                    mU = Utilities.getMISDN(Request);
                    string vStrName = Request.ServerVariables["HTTP_USER_AGENT"];
                    GenMenuHeader(vStrName, mU.mobileOPT);

                    string ipAllow = ConfigurationManager.AppSettings["IsportAllowIP"];
                    string[] ipCurrents = Request.ServerVariables["REMOTE_ADDR"].ToString().Split('.');
                    string ipCurrent = (ipCurrents.Length > 3) ? ipCurrents[0] + "." + ipCurrents[1] + "." + ipCurrents[2] : Request.ServerVariables["REMOTE_ADDR"].ToString();

                    bool isAllow = ipAllow.IndexOf(ipCurrent) > -1 ? true : false;

                    if (mU.mobileNumber != "" || isAllow)
                    {

                        if (Request["d"] != null)
                        {
                            // siamdara clip ตั้งแต่ 01/08/2014 จะไม่ใช่แบบนี้แล้ว
                            bProperty_SGID = ConfigurationManager.AppSettings["SiamDaraClipSG"];
                            GenDaraClip(mU.mobileNumber, mU.mobileOPT, vStrName);
                        }
                        else if( Request["p"] != null || Request["s"] != null )
                        {
                            bProperty_SGID = (Request["p"] != null) ? ConfigurationManager.AppSettings["SiamDaraGameSG"] :  ConfigurationManager.AppSettings["SiamDaraClipSG"];
                            string[] para = (Request["p"] != null )? Request["p"].ToString().Split('_') : Request["s"].ToString().Split('_'); // Request["s"] ไม่มีส่งเป็น _ มานะ

                            string pssvId = (Request["p"] == null ) ? "" : "796" ; // siamdara quiz
                            pssvId = (pssvId == "" && Request["s"] != null) ?  "747"  : pssvId; // siamdara clip
                            pssvId = (Request["s"] != null && para.Length > 1 ) ? "799" : pssvId;

                            if (para.Length > 0 )
                            {

                                // siamdara quiz game
                                PreGenContent(mU.mobileOPT, para[0], para.Length > 1 ? para[1] : "", pssvId, isAllow);
                            }

                        }

                       GenFooter(mU.mobileOPT); 

                        try
                        {
                            isport_service.ServiceWap_Logs.Subscribe_portal_log(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString(), mU.mobileNumber, vStrName
                                , bProperty_SGID, mU.mobileOPT, "57", "0025", "");
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.WriteError(ex.Message);
                        }


                    }
                    else if (Request["o"] == null && Request["m"] == null) Response.Redirect(ConfigurationManager.AppSettings["URLGetMSISDN"] + Request.Url.Query, false);
                    else GenMessage("ขออภัยค่ะ กรุณาใช้ Internet จากเครื่องโทรศัพท์" + mU.mobileNumber, mU.mobileOPT);

                }
            }
            catch (Exception ex)
            {
                GenMessage("ขออภัยค่ะ ระบบเกิดข้อผิดพลาด กรุณาเข้าใช้งานใหม่ " + ex.Message, "");
                ExceptionManager.WriteError(ex.Message);
            }
        }

        #endregion

        #region Header
        private void GenMenuHeader(string vStrName, string opt)
        {
            try
            {
                if (vStrName.ToLower().IndexOf("symbian") > 0)
                {
                    divMenuHeader.Attributes.Add("style", "display:none");
                    divMenuHeader_low.Attributes.Add("style", "display:block");
                    lblHeaderLow.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString(), opt, "siamdara", "", "index.aspx", "0"));
                }
                else
                {
                    divMenuHeader.Attributes.Add("style", "display:block");
                    divMenuHeader_low.Attributes.Add("style", "display:none");
                    frmMain.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString(), opt, "siamdara", "", "news.aspx", "0"));
                }
                //else 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Content
        private void PreGenContent(string opt, string projectType,string pcntId,string pssvId,bool isAllow)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString(), CommandType.StoredProcedure, "usp_wapisport_uiselectbyprojecttype"
                    , new SqlParameter[] { new SqlParameter("@projecttype", projectType) });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["ui_index"].ToString() == "0")
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    // sms

                    // siam dara quiz game
                    string check = ServiceSMS.MemberSms_CheckActive(ConfigurationManager.ConnectionStrings["IsportConnectionString"].ToString(), pssvId, mU.mobileOPT, mU.mobileNumber);
                    if (check == "Y"  || isAllow)//|| Request["s"] !=null
                    {
                        if( Request["p"] != null  ) GenGameContent(mU.mobileOPT, projectType,pcntId);
                        else if (Request["s"] != null) GenContent(mU.mobileOPT, projectType);

                    }
                    else
                    {
                        frmMain.Controls.Add(ServiceWapUI_GenControls.genText("", "ขออภัยค่ะ กรุณาสมัครบริการก่อนเข้าใช้งานค่ะ", "", "0", Request["p"], false, ""));
                        if (Request["p"] != null)
                        {
                            GenContent(mU.mobileOPT, "siamquiz");
                        }
                        else if (Request["s"] != null)
                        {
                            GenContent(mU.mobileOPT, "siamdaraclip_sub");
                        }
                        else GenContent(mU.mobileOPT, "sms");
                    }


                    // else GenContent(mU.mobileOPT, projectType);
                }
                else GenContent(mU.mobileOPT, Request["p"]);

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
            pageMaster += "&p=" + projectType;

            frmMain.Controls.Add(new isport_service.ServiceWapUI_Content().GenContent(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString()
                , ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString()
                , Request["mid"] == null ? "0" : Request["mid"]
                , Request["level"] == null ? "0" : Request["level"]
                , opt, projectType, link, pageMaster, Request["class_id"]));
        }

        private void GenGameContent(string opt, string projectType,string pcntId)
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

            /*frmMain.Controls.Add(new isport_service.ServiceWapUI_Content().GenContent(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString()
                , ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString()
                , Request["mid"] == null ? "0" : Request["mid"]
                , Request["level"] == null ? "0" : Request["level"]
                , opt, projectType, link, pageMaster, Request["class_id"]));*/


            DataSet ds = new isport_service.AppCode().SelectUIByLevel_Wap(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString(), "0", "0", opt, projectType);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string classImg = "img-center";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                    {

                            if (dr["content_align"].ToString() == "Center")
                            {
                                classImg = "img-center";
                            }
                            else if (dr["content_align"].ToString() == "Right")
                            {
                                classImg = "img-right";
                            }
                            if ( !(bool)dr["ui_ispayment"])
                            {
                                // รูปคำถาม
                                frmMain.Controls.AddAt(frmMain.Controls.Count, ServiceWapUI_GenControls.genImagesHeader(dr, link, "0", projectType, (bool)dr["ui_ismaster"], classImg));
                            }
                            else if (pcntId != null && pcntId != "" && (bool)dr["ui_ispayment"])
                            {
                                // รูปคำตอบ
                                frmMain.Controls.AddAt(frmMain.Controls.Count, ServiceWapUI_GenControls.genImagesHeader(dr, link, "0", projectType, (bool)dr["ui_ismaster"], classImg));
                            }

                    }
                    else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                    {

                        frmMain.Controls.AddAt(frmMain.Controls.Count, ServiceWapUI_GenControls.genText(dr, link, "0", projectType, (bool)dr["ui_ismaster"], (bool)dr["ui_ispayment"]));
                    }
                }
            }


        }


        private void GenDaraClip(string msisdn, string opt, string vStrName)
        {
            try
            {
                string strURL = "";
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["IsportConnectionString"].ToString(), CommandType.StoredProcedure, "usp_getsip_content_bypcntid"
                        , new SqlParameter[] { new SqlParameter("@pcnt_id", Request["d"]) });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    string pssv_id = ((opt == "03" || opt == "04") && dr["pssv_id"].ToString() == "762") ? "747" : dr["pssv_id"].ToString(); // 762 = Dtac , 747 = True , True H
                    string chk = AppCode.CheckSmsSubScribe(opt, msisdn, pssv_id);
                    chk = (chk == "N" && chk == "03") ? AppCode.CheckSmsSubScribe("04", msisdn, pssv_id) : chk;

                    if (msisdn != "" && chk == "Y")
                    {

                            // Siamdara clip
                            if (Request.Browser.ScreenPixelsWidth > int.Parse(ConfigurationManager.AppSettings["ModeScreenSize"]))
                            {

                                #region Phone hight
                                if (vStrName.ToLower().IndexOf("iphone") > 0)
                                {
                                    // iphone
                                    strURL = ds.Tables[0].Rows[0]["pcnt_detail_local"].ToString();
                                }
                                else
                                {
                                    // android and other
                                    strURL = ds.Tables[0].Rows[0]["pcnt_detail"].ToString();
                                }
                                #endregion
                            }
                            else
                            {
                                // feature phone
                                strURL = ds.Tables[0].Rows[0]["pcnt_title"].ToString();
                            }

                            if (strURL != "") Response.Redirect(strURL, false);

                    }
                    else if( chk == "N" )
                    {
                        // CheckActive = N  or msisdn = ""
                        GenMessage("ขออภัยค่ะ กรุณาสมัครบริการก่อนเข้าใช้งานค่ะ" , opt);
                    }
                    else GenMessage("ขออภัยค่ะ กรุณาใช้ Internet จากเครื่องโทรศัพท์ " , opt);
                }
                else
                {
                    GenMessage("ขออภัยค่ะ ไม่พบข้อมูลที่ต้องการกรุณาเข้าใช้งานใหม่" , opt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GenDaraClip >>" + ex.Message);
            }
        }
        private void GenMessage(string message,string opt)
        {
            try
            {
                if (Request.Browser.ScreenPixelsWidth > int.Parse(ConfigurationManager.AppSettings["ModeScreenSize"]))
                {
                    frmMain.Controls.Add(new LiteralControl("<div class='row'><div class='col-xs-12 col-sm-12 col-lg-12'><p>" + message + "</p></div></div>"));
                }
                else
                {
                    
                    //var div = Page.FindControl("id1").vis;
                    //div.Visibility = true;
                    frmMain.Controls.Add(new LiteralControl(message));
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception("GenMessage>> "+ex.Message);
            }
        }

        #endregion

        #region Footer
        private void GenFooter(string opt)
        {
            try
            {
                frmMain.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooter(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString(), opt, "siamdara", "", "index.aspx", "0", "0025", "57", "", ""));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}
