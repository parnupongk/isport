using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Data;
using MobileLibrary;
using WebLibrary;
namespace WS_BB
{
    public partial class starsoccer : System.Web.UI.Page
    {
        XDocument rtnXML = new XDocument(new XElement("SportApp",
                             new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                             , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")) )));
        XmlDocument xmlDoc = new XmlDocument();
        private string strErr = "", strStatus = "";
        MobileUtilities muMobile = null;
        string optCode = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                muMobile = Utilities.getMISDN(Request);
                StatusWS ws =  AppCode_PageStatus.CheckErrorParameter(Request);
                strErr = ws.strErrMess;
                strStatus = ws.strStatus;
                if (!IsPostBack && (strErr == null || strErr == ""))
                {
                    Response.Write(CheckPageName(muMobile,Request["ap"], Request["pn"]));
                }
                else if (strErr != null && strErr != "")
                {

                    rtnXML = AppCode_PageStatus.GenStatus(rtnXML,strStatus,strErr);
                    xmlDoc.Load(rtnXML.CreateReader());
                    Response.Write(xmlDoc.InnerXml);

                    new AppCode_Logs().Logs_Insert("isportstartsoccer_error", strErr, Request.Url.ToString(), "android", muMobile.mobileOPT + "|" + muMobile.mobileNumber + "|" + Request["imei"] + "|" + Request["imsi"]
                        , AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(),"");
                }
            }
            catch(Exception ex)
            {
                strStatus = "Error";
                strErr = ex.Message;
                ExceptionManager.WriteError("Main StartSoccer>>"+ex.Message);

                rtnXML = AppCode_PageStatus.GenStatus(rtnXML, strStatus, strErr);
                xmlDoc.Load(rtnXML.CreateReader());
                Response.Write(xmlDoc.InnerXml);
            }

        }

        #region Check page name
        private string  CheckPageName(MobileUtilities muMobile,string ap,string pn)
        {
            try
            {
                string rtn = "";


                optCode = muMobile.mobileOPT == null || muMobile.mobileOPT == "" ? AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"])) : muMobile.mobileOPT;
                if (ap == AppCode_Base.AppName.StarSoccer.ToString()) // App MTUTD
                {
                    if ( pn == "livescore") // gen sms service
                    {
                        rtn = GetLiveScore(Request["contentgroupid"], Request["sportType"], Request["lang"]);
                        new AppCode_Logs().Logs_Insert("isportstartsoccer_getlivescore", Request["contentgroupid"], "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "");
                    }
                    else if (pn == "getnews")
                    {
                        rtn = GetNews("", Request["date"], Request["rowcount"], Request["type"], Request["sportType"], Request["lang"]);
                        new AppCode_Logs().Logs_Insert("isportstartsoccer_getnews", "", "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "");
                    }
                    else if (pn == "scoredetail")
                    {
                        rtn = GetScoreDetail(Request["matchId"], Request["mschId"], Request["lang"]);
                        new AppCode_Logs().Logs_Insert("isportstartsoccer_getscoredetail", "", "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "");
                    }
                    else if (pn == "bannerclick")
                    {
                        new AppCode_Logs().Logs_Insert("isportstartsoccer_bannerclick", "", "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "Y");
                        //Response.Redirect("http://wap.isport.co.th/isportui/indexl.aspx?p=d02campaignh&mp_code=0096", false);
                        string prjName = ( Request["p"] == null ) ? "bb" : Request["p"] ;
                        Response.Redirect("http://wap.isport.co.th/isportui/indexl.aspx?p=" + prjName + "&mp_code=0096", false);
                    }
                    else if (pn == "banner")
                    {
                        rtn = GetBanner();
                        new AppCode_Logs().Logs_Insert("isportstartsoccer_getbanner", "", "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "");
                    }
                    else if (pn == "radio")
                    {

                    }
                    else if (pn == "result")
                    {
                        rtn = GetResult(Request["contentgroupid"], Request["date"], Request["sportType"], Request["lang"]);
                        new AppCode_Logs().Logs_Insert("isportstartsoccer_getresult", Request["contentgroupid"], "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "");
                    }
                    else if (pn == "program")
                    {
                        rtn = GetProgram(Request["date"], Request["contentgroupid"], Request["sportType"], Request["lang"], Request["type"]);
                        new AppCode_Logs().Logs_Insert("isportstartsoccer_getprogram", Request["contentgroupid"], "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "");
                    }
                    else if (pn == "analysis")
                    {
                        rtn = GetAnalysis(Request["contentgroupid"], Request["sportType"], Request["lang"], Request["type"]);
                        new AppCode_Logs().Logs_Insert("isportstartsoccer_getanalysis", Request["contentgroupid"], "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "");
                    }
                    else if (pn == "menulist")
                    {
                        rtn = GetMenuList(Request["sportType"], Request["lang"]);
                        new AppCode_Logs().Logs_Insert("isportstartsoccer_getmenulist", "", "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "");
                    }
                    else if (pn == "leaguetable")
                    {
                        rtn = GetLeagueTable(Request["contentgroupid"], Request["sportType"], Request["lang"]);
                        new AppCode_Logs().Logs_Insert("isportstartsoccer_getleaguetable", Request["contentgroupid"], "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "");
                    }
                    else if (pn == "clip")
                    {
                        rtn = GetClip(Request["contentgroupid"], Request["sportType"], Request["lang"], Request["type"]);
                        new AppCode_Logs().Logs_Insert("isportstartsoccer_getClip", Request["contentgroupid"], "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "");
                    }
                    else if (pn == "smsservice")
                    {
                        rtn = GenSMSService();
                        new AppCode_Logs().Logs_Insert("isportstartsoccer_smsservice", "", "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "");
                    }
                    else if (pn == "hot")
                    {
                        rtn = GenHot(AppCode_Base.AppName.StarSoccer.ToString(), Request["lang"], Request["type"]);
                        new AppCode_Logs().Logs_Insert("isportstartsoccer_hot", "", "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "");
                    }
                    else if (pn == "event")
                    {
                        rtn = GenEvent(AppCode_Base.AppName.StarSoccer.ToString(), Request["lang"], Request["type"]);
                        new AppCode_Logs().Logs_Insert("isportstartsoccer_event", "", "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "");
                    }
                    else if (pn == "notification")
                    {
                        rtn = GetNotification(Request["contentgroupid"], Request["lang"], Request["min"], Request["type"]);
                        //new AppCode_Logs().Logs_Insert("isportstartsoccer_notification", Request["contentgroupid"], "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.StarSoccer.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "");
                    }
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("CheckPageName>>" + ex.Message);
            }
        }
        #endregion

        #region Get Notification
        public string GetNotification(string contentGroupId ,string lang,string minute,string type)
        {
            try
            {

                if ( type != "genfile" && contentGroupId == "")
                {
                    xmlDoc.LoadXml(new push().SendGet(ConfigurationManager.AppSettings["IsportStarSoccer_GetNotification"]));
                }
                else
                {
                    contentGroupId = contentGroupId == "" ? ConfigurationManager.AppSettings["Application_IsportStarSoccer_DefaultNotify"] : contentGroupId;
                    int iMinute = minute == "" ? int.Parse(ConfigurationManager.AppSettings["ApplicationnotificationMinute"].ToString()) : int.Parse(minute);

                    rtnXML = new AppCode_Utility().CommandGetNotification_BallThai(rtnXML, "", iMinute, AppCode_Utility.Country.th.ToString());
                    if (rtnXML.Element("SportApp").Element("notification").Value == "")
                    {
                        rtnXML = new AppCode_Utility().CommandGetNotificationByContestGroupId(contentGroupId, iMinute, lang);
                    }
                    xmlDoc.Load(rtnXML.CreateReader());

                }

            }
            catch (Exception ex)
            {
                XDocument rtnXML = new XDocument(new XElement("SportApp"));
                rtnXML.Element("SportApp").Add(new XElement("notification", "none"));
                xmlDoc.Load(rtnXML.CreateReader());
                //throw new Exception("GetNotification >>"+ex.Message);
            }

            return xmlDoc.InnerXml;
        }
        #endregion

        #region Get Event
        private string GenEvent(string projectCode, string lang, string type)
        {
            try
            {
                rtnXML.Element("SportApp").Add(new XAttribute("version", ConfigurationManager.AppSettings["Application_IsportStarSoccer_Version"].ToString()));

                if (bool.Parse(ConfigurationManager.AppSettings["Application_IsportStarSoccer_ShowEvent"]))
                {
                    rtnXML = new AppCode_Utility().CommangGetEvent(rtnXML, projectCode, lang, type);
                }

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GenHot >>" + ex.Message);
            }
        }
        #endregion

        #region Get Hot
        private string GenHot(string projectCode, string lang, string type)
        {
            try
            {

                string optCode = muMobile.mobileOPT; //AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"]));

                rtnXML = new AppCode_Utility().CommangGetPrivilege_IsportStarsoccer(rtnXML, projectCode, lang, type, optCode);

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GenHot >>" + ex.Message);
            }
        }
        #endregion

        #region Get SMS Service
        private string GenSMSService()
        {
            try
            {

                string optCode = muMobile.mobileOPT == null || muMobile.mobileOPT == "" ? AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"])) : muMobile.mobileOPT ;//;

                rtnXML.Element("SportApp").Add(new XAttribute("wording1", "เช็คทีเด็ดก่อนเชียร์บอล ไม่ผิดหวัง!!"));
                rtnXML.Element("SportApp").Add(new XAttribute("wording2", ""));
                rtnXML = new AppCode_Subscribe().CommandGetSMSService_IsportStarSoccer(rtnXML, optCode, ConfigurationManager.AppSettings["MpCode_ISPORTSTARSOCCER"].ToString());

                if (rtnXML.Element("SportApp").Element("status") == null)
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", ""));
                }
                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GenSMSService >>" + ex.Message);
            }
        }
        #endregion

        #region Get Clip
        private string GetClip(string contentGroupId, string sportType, string lang,string type)
        {
            try
            {

                rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));
                contentGroupId = contentGroupId == "" ? ConfigurationManager.AppSettings["contestGroupDefault"] : contentGroupId;
                string rowCount =  "15" ;
                xmlDoc.Load(new AppCode_News().Command_GetSportClip(rtnXML,sportType, rowCount, lang, type).CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GetMenuList>>" + ex.Message);
            }
        }
        #endregion

        #region League Table
        private string GetLeagueTable(string contentGroupId,string sportType, string lang)
        {
            try
            {

                rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));
                contentGroupId = contentGroupId == "" ? ConfigurationManager.AppSettings["contestGroupDefault"] : contentGroupId;
                rtnXML = new AppCode_FootballAnalysis().CommandGetLeagueTable_IsportStarSoccer(rtnXML, contentGroupId, lang);

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GetMenuList>>" + ex.Message);
            }
        }
        #endregion

        #region Menu List
        private string GetOperatorIcon()
        {
            try
            {
                string urlIcon = ConfigurationManager.AppSettings["Application_IsportStarSoccer_URLICon_Default"];
                if (optCode == "01") urlIcon = ConfigurationManager.AppSettings["Application_IsportStarSoccer_URLICon_AIS"];
                else if (optCode == "02") urlIcon = ConfigurationManager.AppSettings["Application_IsportStarSoccer_URLICon_Dtac"];
                else if (optCode == "03" || optCode == "04") urlIcon = ConfigurationManager.AppSettings["Application_IsportStarSoccer_URLICon_True"];
                else if (optCode == "05") urlIcon = ConfigurationManager.AppSettings["Application_IsportStarSoccer_URLICon_3GX"];
                return urlIcon;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private string GetOperatorIVR()
        {
            try
            {
                string strIVR = ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_AIS"];
                if (optCode == "01") strIVR = ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_AIS"];
                else if (optCode == "02") strIVR = ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_Dtac"];
                else if (optCode == "03" || optCode == "04") strIVR = ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_True"];
                return strIVR;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private string GetOperatorBanner()
        {
            try
            {
                string urlIcon = ConfigurationManager.AppSettings["Application_IsportStarSoccer_URLIBanner_AIS"];
                if (optCode == "02") urlIcon = ConfigurationManager.AppSettings["Application_IsportStarSoccer_URLIBanner_Dtac"];
                else if (optCode == "03" || optCode == "04") urlIcon = ConfigurationManager.AppSettings["Application_IsportStarSoccer_URLIBanner_True"];
                else if (optCode == "05") urlIcon = ConfigurationManager.AppSettings["Application_IsportStarSoccer_URLIBanner_3GX"];
                return urlIcon;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GetMenuList(string sportType, string lang)
        {
            try
            {
                rtnXML.Element("SportApp").Add(new XAttribute("version", ConfigurationManager.AppSettings["Application_IsportStarSoccer_Version"].ToString()));
                rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th")); // && Request["imei"] != "359092056674736" imei p' jane
                // adview : true= random admob กับ ads isport ; false = ads isport
                rtnXML.Element("SportApp").Add(new XAttribute("adview", (DateTime.Now.Hour > 0 && DateTime.Now.Hour < 6 ) ? "true" : ConfigurationManager.AppSettings["Application_IsportStarSoccer_adView"].ToString()));
                rtnXML.Element("SportApp").Add(new XAttribute("urlicon", GetOperatorIcon()));
                rtnXML.Element("SportApp").Add(new XAttribute("urlbanner", GetOperatorBanner()));
                rtnXML.Element("SportApp").Add(new XAttribute("IVR", GetOperatorIVR()));
                rtnXML.Element("SportApp").Add(new XAttribute("OPTCode", optCode));

                string checkActive = CheckActiveandPromotion();
                if (AppCode_Subscribe.CheckIsOTPWait(Request["imsi"], Request["imei"], AppCode_Base.AppName.StarSoccer.ToString()) == "N")
                {
                    rtnXML.Element("SportApp").Add(new XElement("Active",
                                                                       new XAttribute("isactive", checkActive)
                                                                    , new XAttribute("otp_status", "new")
                                                                    , new XAttribute("header", "รับสิทธิ์ใช้ฟรี 7 วัน")
                                                                    , new XAttribute("detail", "เกาะติดฟุตบอลทั่วโลก สด! ตลอด 24 ชั่วโมง   พร้อมวิเคราะห์ฟันธงบอลเด็ดน่าเชียร์ประจำวัน มั่นใจได้ไม่ผิดหวัง!")
                                                                    , new XAttribute("footer", "กรุณากรอกหมายเลขโทรศัพท์มือถือ (08xxxxxxxxx)")
                                                                    , new XAttribute("footer1", ConfigurationManager.AppSettings["Application_StarSoccer_Footer"] )));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("Active",
                                                                       new XAttribute("isactive", checkActive)
                                                                    , new XAttribute("otp_status", "wotp") // wait OTP
                                                                    , new XAttribute("header", "สมัครสมาชิก ")
                                                                    , new XAttribute("detail", "กรุณากรอก รหัสเข้าแอพสตาร์ซอคเกอร์ (ที่ได้รับจาก sms)")
                                                                    , new XAttribute("footer", "ระบบ Dtac กรุณาติดต่อ 02-502-6767")));
                }

                rtnXML = new AppCode_FootballList().Command_GetFootballLeague_IsportStarSoccer(rtnXML, lang,sportType );

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GetMenuList>>" + ex.Message);
            }
        }
        
        #endregion

        #region Check Active
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Y= Active , N = Not Active </returns>
        private string CheckActiveandPromotion()
        {
            string isActive = "Y";
            try
            {
                if (bool.Parse(ConfigurationManager.AppSettings["Application_IsCheck_Promotion"]))
                {

                    //string optCode = AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"]));
                    //if (!(optCode == "04"))
                    //{
                        //optCode = (muMobile.mobileOPT == null || muMobile.mobileOPT == "") ? optCode : muMobile.mobileOPT;
                    //}

                    // insert frist access
                    SubscribeMember sm = AppCode_Subscribe.IsportAppCheckExpirePromotion(muMobile.mobileNumber, Request["imsi"], Request["imei"], muMobile.mobileOPT
                            , AppCode_Base.AppName.StarSoccer.ToString(), ConfigurationManager.AppSettings["Application_Day_Promotion"]);

                    if (optCode == "05")
                    {
                        // 3GX
                        isActive = "Y";
                    }
                    else
                    {
                        // เช็คว่าเกินวันที่ให้ฟรีหรือยัง
                        if (sm.status == "Y")
                        {
                            // Y = เกินวันที่ให้ฟรี

                            // Check Dup
                            string msisdn = muMobile.mobileNumber == "" || muMobile.mobileNumber == null ? AppCode_Subscribe.CheckActive_AppMember(Request["imsi"], Request["imei"], AppCode_Base.AppName.StarSoccer.ToString()).msisdn : muMobile.mobileNumber;
                            //ExceptionManager.WriteError("CheckActiveandPromotion>>" + msisdn + "optCode:" + optCode);
                            isActive = AppCode_Subscribe.CheckActive_IsportStarSoccer_AppMember(optCode, msisdn, Request["imsi"], Request["imei"], AppCode_Base.AppName.StarSoccer.ToString());

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CheckActiveandPromotion>>" + ex.Message);
            }
            return isActive;
        }
        #endregion

        #region Analysis
        private string GetAnalysis(string contentGroupId, string sportType, string lang,string type)
        {
            try
            {
                // GetAnalysis
                contentGroupId = (contentGroupId == null || contentGroupId == "") ? "" : contentGroupId;

                if ( type != "genfile" && contentGroupId == "")
                {
                    try
                    {
                        rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_Analyse"].ToString()));
                    }
                    catch
                    {
                        //rtnXML = new AppCode_FootballProgram().CommandGetAnalysisByLeague(rtnXML, contentGroupId, "", lang);
                    }
                }
                else
                {
                    rtnXML = new AppCode_FootballProgram().CommandGetAnalysisByLeague(rtnXML, contentGroupId, "", lang);
                }
                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GetMainPage>>" + ex.Message);
            }
        }
        #endregion

        #region Program
        private string GetProgram(string date,string contentGroupId, string sportType, string lang,string type)
        {
            try
            {
                // GetProgram
                date = (date == null || date == "") ? DateTime.Now.ToString("yyyyMMdd") : date; 
                int year = (date != null && date.Length > 4) ? int.Parse(date.Substring(0, 4)) : DateTime.Now.Year;
                year = (year > 2550) ? year - 543 : year;
                date = year.ToString() + date.Substring(4);
                date = ( date ==null || date == "" )? DateTime.Now.ToString("yyyyMMdd") : date ;
                contentGroupId = (contentGroupId == null || contentGroupId == "") ? "" : contentGroupId;
                
                rtnXML.Element("SportApp").SetAttributeValue("date", AppCode_LiveScore.DateText(date));
                rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));

                if (type != "genfile" && date == DateTime.Now.ToString("yyyyMMdd"))
                {
                    try
                    {
                        rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_Program"].ToString()));
                    }
                    catch
                    {
                        rtnXML = new AppCode_FootballProgram().CommandGetProgramByDate(rtnXML, date, contentGroupId, "", lang);
                    }
                }
                else
                {
                    rtnXML = new AppCode_FootballProgram().CommandGetProgramByDate(rtnXML, date, contentGroupId, "", lang);
                }

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GetMainPage>>" + ex.Message);
            }
        }
        #endregion

        #region Banner
        private string GetBanner()
        {
            try
            {

                rtnXML.Element("SportApp").Add(new XElement("Banner", "http://wap.isport.co.th/isportws/banner/isportstarsoccer_banner.html")
                    ,new XElement("Status", strStatus)
                    , new XElement("Status_Message", strErr));

                xmlDoc.Load(rtnXML.CreateReader());
                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GetBanner>>" + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Score
        private string GetResult(string contentGroupId,string scoreDate, string sportType, string lang)
        {
            try
            {
                // GetResult
                AppCode_LiveScore ls = new AppCode_LiveScore();

                int year = (scoreDate != null && scoreDate.Length > 4) ? int.Parse(scoreDate.Substring(0, 4)) : DateTime.Now.Year;
                year = (year > 2550) ? year - 543 : year;
                scoreDate = year.ToString() + scoreDate.Substring(4);

                scoreDate = scoreDate == "" ? DateTime.Now.ToString("yyyyMMdd") : scoreDate;
                TimeSpan diffDate = new DateTime(int.Parse(scoreDate.Substring(0, 4)), int.Parse(scoreDate.Substring(4, 2)), int.Parse(scoreDate.Substring(6, 2))) - DateTime.Now;

                if (contentGroupId == "" && scoreDate == DateTime.Now.ToString("yyyyMMdd"))
                {
                    try
                    {
                        rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_ScoreNew"].ToString()));
                    }
                    catch { }
                    /*if (rtnXML.Element("SportApp").Element("status") == null)
                    {
                        rtnXML = ls.CommandGetScoreResult(rtnXML, "3405", scoreDate, lang);// บอลไทย
                        rtnXML = ls.CommandGetScore(rtnXML, "", lang, contentGroupId, AppCode_LiveScore.MatchType.Finished, diffDate.Days, "N");
                    }*/
                }
                else
                {
                    //rtnXML = ls.CommandGetScoreResult(rtnXML, "3405", scoreDate, lang);// บอลไทย
                    rtnXML = ls.CommandGetScore(rtnXML, "", lang, contentGroupId, AppCode_LiveScore.MatchType.Finished, diffDate.Days, "N");
                }
                
                rtnXML.Element("SportApp").SetAttributeValue("date", AppCode_LiveScore.DateText(scoreDate));
                rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GetResult>>" + ex.Message);
            }
        }
        private string GetLiveScore(string contentGroupId, string sportType, string lang)
        {
            try
            {
                // LiveScore
                AppCode_LiveScore ls = new AppCode_LiveScore();

                if (contentGroupId == "")
                {
                    try
                    {
                        rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_LiveScoreNew"].ToString()));
                    }
                    catch { }
                    if (rtnXML.Element("SportApp").Element("status") == null)
                    {
                        //rtnXML = ls.CommandGetScoreResult(rtnXML, "3405", DateTime.Now.ToString("yyyyMMdd"), lang);// บอลไทย
                        rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, "", lang, contentGroupId, AppCode_LiveScore.MatchType.inprogress, 0, "N");
                        
                    }
                    
                }
                else
                {
                    rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, "", lang, contentGroupId, AppCode_LiveScore.MatchType.inprogress, 0, "N");
                }

                if (rtnXML.Element("SportApp").Element("url") == null ) rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));
                if ( rtnXML.Element("SportApp").Element("League") == null)
                {
                    rtnXML.Element("SportApp").Element("status").Remove();
                    rtnXML.Element("SportApp").Element("message").Remove();
                    //rtnXML = ls.CommandGetScoreResult(rtnXML, "3405", DateTime.Now.ToString("yyyyMMdd"), lang);// บอลไทย
                    rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, "", lang, contentGroupId, AppCode_LiveScore.MatchType.Finished, 0, "N");
                    
                }
                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GetLiveScore>>" + ex.Message);
            }
        }
        private string GetScoreDetail(string matchId,string mschId,string lang)
        {
            try
            {
                try
                {
                    // get score detail ต้องไปดึงที่ xml file ก่อน (ถ้าไม่เจอถึงไปที่ database)
                    string urlScore = ConfigurationManager.AppSettings["IsportGenFile_ScoreDetail"].ToString() + matchId + ".xml";
                    xmlDoc.LoadXml(new push().SendGet(urlScore));
                }
                catch { }

                if (xmlDoc.ChildNodes.Count == 0)
                {
                    xmlDoc.Load(new AppCode_LiveScore().CommandGetScoreDetail(matchId, mschId, lang).CreateReader());
                    //new AppCode_Logs().Logs_Insert_IP();
                }

                //rtnXML = new AppCode_LiveScore().CommandGetScoreDetail(rtnXML,matchId, mschId, lang);
                //xmlDoc.Load(rtnXML.CreateReader());
                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GetScoreDetail>>"+ex.Message);
            }
        }
        #endregion

        #region News
        private string GetNews(string contentGroupId ,string date,string rowCount ,string type ,string sportType,string lang)
        {
            try
            {

                date = date == "" || date == null ? DateTime.Now.ToString("yyyyMMdd") : date;

                int year = (date != null && date.Length > 4) ? int.Parse(date.Substring(0, 4)) : DateTime.Now.Year;
                year = (year > 2550) ? year - 543 : year;
                date = year.ToString() + date.Substring(4);

                //string newsDate = (date == null || date == "") ? DateTime.Now.ToString("yyyyMMdd") : date;
                rowCount = (rowCount == null || rowCount == "") ? "1" : rowCount;
                // News

                if (type != "genfile" && date == DateTime.Now.ToString("yyyyMMdd"))
                {
                    try
                    {
                        rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_GetNews"].ToString()));
                    }
                    catch
                    {
                        rtnXML = new AppCode_News().Command_GetNewsByDate(rtnXML, date, sportType, rowCount, lang, "", type);
                    }
                }
                else
                {
                    rtnXML = new AppCode_News().Command_GetNewsByDate(rtnXML, date, sportType, rowCount, lang, "", type);
                }

                xmlDoc.Load(rtnXML.CreateReader());
                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GetNews>>" + ex.Message);
            }
        }
        #endregion

    }
}
