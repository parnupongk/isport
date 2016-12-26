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
    public partial class sportarena : System.Web.UI.Page
    {

        XDocument rtnXML = new XDocument(new XElement("SportApp",
                             new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                             , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))));
        XmlDocument xmlDoc = new XmlDocument();
        private string strErr = "", strStatus = "";
        private string code = "",imsi="",imei="",model="";
        MobileUtilities muMobile = null;

        private StatusWS CheckErrorParameter(HttpRequest Request)
        {
            StatusWS ws = new StatusWS();
            if (Request["pn"] != "notification")
            {
                //if (Request["ap"] == "" || Request["ap"] == null) ws.strErrMess += "pls. input app name,";
                if (Request["imei"] == "" || Request["imei"] == null) ws.strErrMess += "pls. input IMEI,";
                if (Request["imsi"] == "" || Request["imsi"] == null) ws.strErrMess += "pls. input IMSI,";
                //if (Request["ano"] == "" || Request["ano"] == null) strErr += "pls. input ano";
                if (ws.strErrMess != "") ws.strStatus = "Error";
            }
            return ws;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            muMobile = Utilities.getMISDN(Request);
            
            StatusWS ws = CheckErrorParameter(Request);
            strErr = ws.strErrMess;
            strStatus = ws.strStatus;
            if (!IsPostBack && (strErr == null || strErr == ""))
            {
                code = muMobile.mobileNumber + "|" + muMobile.mobileOPT + "|" + Request["imsi"] + "|" + Request["imei"];
                Response.Write(CheckPageName(Request["pn"], Request["ap"]));
            }
            else if (strErr != null && strErr != "")
            {
                rtnXML = AppCode_PageStatus.GenStatus(rtnXML,strStatus,strErr);
                xmlDoc.Load(rtnXML.CreateReader());
                Response.Write(xmlDoc.InnerXml);

                new AppCode_Logs().Logs_Insert("sportarena_error", "", "", "", muMobile.mobileOPT + "|" + muMobile.mobileNumber + "|" + Request["imei"] + "|" + Request["imsi"]
                    , AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(),"");
            }
        }

        #region CheckPageName
        private string CheckPageName(string pn, string ap)
        {
            string rtn ="";
            try
            {
                if (pn == "sportclip")
                {
                    rtn = Isport_GetSportClip(Request["sporttype"], Request["rowcount"], Request["lang"], Request["type"]);
                }
                else if (pn == "notification")
                {
                    rtn = Isport_GetNotification(Request["countryid"], Request["minute"], Request["land"], Request["type"]);
                }
                else if (pn == "program")
                {
                    rtn = Isport_GetFootballProgram(Request["date"], Request["contestgroupid"], Request["countryid"], Request["lang"], Request["type"]);
                }
                else if (pn == "programbymonth")
                {
                    rtn = Isport_GetFootballProgramByMonth(Request["contestgroupid"], Request["year"], Request["month"], Request["lang"], Request["type"]);
                }
                else if (pn == "footballanalyse")
                {
                    rtn = Isport_GetFootballAnalyse(Request["contestgroupid"], Request["countryid"], Request["lang"], Request["type"]);
                }
                else if (pn == "footballanalysedetail")
                {
                    rtn = Isport_GetFootballAnalyseDetail(Request["contestgroupid"], Request["teamcode1"], Request["teamcode2"], Request["matchid"], Request["lang"], Request["type"]);
                }
                else if (pn == "leaguetable")
                {
                    rtn = Isport_GetLeagueTable(Request["contestgroupid"], Request["lang"], Request["type"]);
                }
                else if (pn == "gettopleague")
                {
                    rtn = Isport_GetTopLeague(Request["lang"], Request["type"]);
                }
                else if (pn == "getteam")
                {
                    rtn = Isport_GetTeam(Request["contestgroupid"], Request["lang"], Request["type"]);
                }
                else if (pn == "tded")
                {
                    rtn = Isport_GetTded(Request["lang"], Request["type"]);
                }
                else if (pn == "funtong")
                {
                    rtn = Isport_GetFuntong(Request["contestgroupid"], Request["countryid"], Request["lang"], Request["type"]);
                }
                else if (pn == "livescore")
                {
                    rtn = Isport_GetLiveScore(Request["contestgroupid"], Request["countryid"], Request["lang"], Request["type"]);
                }
                else if (pn == "getscore")
                {
                    rtn = Isport_GetScore(Request["date"], Request["contestgroupid"], Request["countryid"], Request["lang"], Request["type"]);
                }
                else if (pn == "scoredetail")
                {
                    rtn = Isport_GetScoreDetail(Request["matchid"], "", Request["lang"], Request["type"]);
                }
                else if (pn == "getnews")
                {
                    rtn = Isport_GetSportNews(Request["date"], Request["contestgroupid"], Request["sporttype"], Request["rowcount"], Request["countryid"], Request["lang"], Request["type"]);
                }
                else if (pn == "getgallery")
                {
                    rtn = Isport_GetSportImages(Request["contestgroupid"], Request["sporttype"], Request["rowcount"], Request["countryid"], Request["lang"], Request["type"]);
                }
                else if (pn == "mainpage")
                {
                    rtn = Isport_GetDataMainPage(Request["contestgroupid"], Request["sporttype"], Request["rowcount"], Request["countryid"], Request["lang"], Request["type"]);
                }
                else if (pn == "listmenu")
                {
                    rtn = Isport_GetListMenu(Request["lang"], Request["type"]);
                }
                else if (pn == "getabout")
                {
                    rtn = Isport_GetAbout(AppCode_Base.AppName.SportArena.ToString(), Request["lang"], Request["type"]);
                }
                else if (pn == "getprivilege")
                {
                    rtn = Isport_GetPrivilege(AppCode_Base.AppName.SportArena.ToString(), Request["lang"], Request["type"]);
                }
                else if (pn == "getbanner")
                {
                    rtn = Isport_GetBanner(AppCode_Base.AppName.SportArena.ToString(), Request["lang"], Request["type"]);
                }
                else if (pn == "playerscore")
                {
                    rtn = Isport_GetPlayerTopScore(Request["contestgroupid"], Request["lang"], Request["type"]);
                }
                else if (pn == "othersport")
                {
                    rtn = Isport_GetSiamSportAllTag(Request["contestgroupid"], Request["sporttype"], Request["countryid"], Request["lang"], Request["type"]);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Other Sport ( result,program )
        private  string Isport_GetSiamSportAllTag(string contestGroupId, string sportType, string countryId, string lang, string type)
        {
            // contestGroupId == ScsId (isport.dbo.sport_class_season)

            rtnXML.Element("SportApp").Add( new XAttribute("iconFileName", "http://wap.isport.co.th/isportws/images/sportarena/"+ sportType + ".png"));
            if (sportType == "00001")
            {
                contestGroupId = contestGroupId == "" ? "2612" : contestGroupId;
                // ผลการแข่งขัน
                rtnXML.Element("SportApp").Add(new AppCode_LiveScore().CommandLiveScore("FootballThaiScore", contestGroupId, sportType, true, lang));

                // โปรแกรมการแข่งขัน
                rtnXML.Element("SportApp").Add(new AppCode_FootballProgram().CommandGetFootballProgram("FootballThaiProgram", contestGroupId, lang));

                // ตารางคะแนน
                rtnXML.Element("SportApp").Add(new AppCode_FootballAnalysis().CommandGetFootballAnalysislevelByscsIdXML("FootballThaiTable", contestGroupId, lang));

            }
            else
            {
                //contestGroupId = contestGroupId == "" && sportType == "00002" ? "2824" : contestGroupId;
                //contestGroupId = contestGroupId == "" && sportType == "00003" ? "2803" : contestGroupId;
                // ผลการแข่งขัน
                rtnXML.Element("SportApp").Add(new AppCode_News().Command_GetSportContent("OtherSportScore", "00007", contestGroupId, lang, sportType));

                // โปรแกรมการแข่งขัน
                rtnXML.Element("SportApp").Add(new AppCode_News().Command_GetSportContent("OtherSportProgram", "00006", contestGroupId, lang, sportType));

            }

            rtnXML.Element("SportApp").Add(
                  new XElement("status", "success")
                  , new XElement("message", ""));

            xmlDoc.Load(rtnXML.CreateReader());
            new AppCode_Logs().Logs_Insert("SiamSportAllTag", contestGroupId, "", type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        #endregion

        #region Player top score
        private string Isport_GetPlayerTopScore(string contestGroupId, string lang, string type)
        {
            //new AppCode_Logs().Logs_Insert("PlayerTopScore", contestGroupId, "", type, code);
            contestGroupId = contestGroupId == "" ? "21" : contestGroupId;
            xmlDoc.Load(new AppCode_FootballAnalysis().CommandGetPlayerTopScore(contestGroupId, lang).CreateReader());
            return xmlDoc.InnerXml;
        }
        #endregion

        #region Get Banner
        private string Isport_GetBanner(string projectCode, string lang, string type)
        {
            rtnXML.Element("SportApp").Add(
                    new AppCode_Utility().GetImageElement("Banner", "http://wap.isport.co.th/isportws/images/privilege/ipad/banner.gif"
                    , ""
                    , ""
                    , "Sport Arena  FHM"
                    , "025020442"
                    , "http://wap.isport.co.th/isportws/banner/isportsportarena_banner.html")
                 , new XElement("status", "success")
                , new XElement("message", ""));

            xmlDoc.Load(rtnXML.CreateReader());
            new AppCode_Logs().Logs_Insert("Banner", projectCode, "", type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        #endregion

        #region get privilege
        private string Isport_GetPrivilege(string projectCode, string lang, string type)
        {
            //rtnXML = new AppCode_Utility().CommangGetPrivilege(rtnXML, projectCode, lang, type);

            string optCode = muMobile.mobileOPT;//AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"]));

            rtnXML = new AppCode_Utility().CommangGetPrivilege_IsportStarsoccer(rtnXML, projectCode, lang, type, optCode);

            xmlDoc.Load(rtnXML.CreateReader());

            new AppCode_Logs().Logs_Insert("Privilege", projectCode, "", type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        #endregion

        #region About
        private string Isport_GetAbout(string projectCode, string lang, string type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_Utility().CommangGetAbout(projectCode, lang).CreateReader());
            return xmlDoc.InnerXml;
        }
        #endregion

        #region Get List Menu
        private  string Isport_GetListMenu(string lang, string type)
        {

            XmlDocument xmlDoc = new XmlDocument();
            rtnXML.Element("SportApp").Add(new XAttribute("version", ConfigurationManager.AppSettings["Application_SportArena_Version"].ToString()));
            rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));
            rtnXML.Element("SportApp").Add(new XAttribute("adview", ConfigurationManager.AppSettings["Application_SportArena_adView"].ToString()));

            string checkActive = CheckActiveandPromotion();
            if (AppCode_Subscribe.CheckIsOTPWait(Request["imsi"], Request["imei"], AppCode_Base.AppName.SportArena.ToString()) == "N")
            {
                rtnXML.Element("SportApp").Add(new XElement("Active",
                                                                   new XAttribute("isactive", checkActive)
                                                                , new XAttribute("otp_status", "new")
                                                                , new XAttribute("header", "รับสิทธิ์ใช้ฟรี 7 วัน")
                                                                , new XAttribute("detail", "เกาะติดฟุตบอลทั่วโลก สด! ตลอด 24 ชั่วโมง   พร้อมวิเคราะห์ฟันธงบอลเด็ดน่าเชียร์ประจำวัน มั่นใจได้ไม่ผิดหวัง!")
                                                                , new XAttribute("footer", "กรุณากรอกหมายเลขโทรศัพท์มือถือ (08xxxxxxxxx)")
                                                                , new XAttribute("footer1", "ค่าบริการหลังหมดโปรโมชั่นเพียงสัปดาห์ละ 10 บาทเท่านั้น")));
            }
            else
            {
                rtnXML.Element("SportApp").Add(new XElement("Active",
                                                                   new XAttribute("isactive", checkActive)
                                                                , new XAttribute("otp_status", "wotp") // wait OTP
                                                                , new XAttribute("header", "สมัครสมาชิก ")
                                                                , new XAttribute("detail", "กรุณายืนยันรหัสผ่าน ")
                                                                , new XAttribute("footer", "กรุณารอรับ sms password")));
            }

            rtnXML.Element("SportApp").Add(new XElement("left_menu",
                new XElement("menu", new XAttribute("name", "ฟุตบอล"), new XAttribute("type", "header"), new XAttribute("url", ""))
                , new XElement("menu", new XAttribute("name", "ผลสด"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=livescore&"))
                , new XElement("menu", new XAttribute("name", "สรุปผล"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=getscore&"))
                , new XElement("menu", new XAttribute("name", "ข่าว"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=getnews&"))
                , new XElement("menu", new XAttribute("name", "โปรแกรม"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=program&"))
                , new XElement("menu", new XAttribute("name", "วิเคราะห์"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=footballanalyse&"))
                , new XElement("menu", new XAttribute("name", "ทีเด็ด"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=tded&"))
                , new XElement("menu", new XAttribute("name", "ตารางคะแนน"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=leaguetable&"))
                , new XElement("menu", new XAttribute("name", "กีฬาอื่นๆ"), new XAttribute("type", "header"), new XAttribute("url", ""))
                , new XElement("menu", new XAttribute("name", "ผลการแข่งขัน"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=othersport&"))
                , new XElement("menu", new XAttribute("name", "ข่าว"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=getnews&"))
                , new XElement("menu", new XAttribute("name", "โปรแกรม"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=othersport&"))
                , new XElement("menu", new XAttribute("name", "MORE"), new XAttribute("type", "header"), new XAttribute("url", ""))
                //, new XElement("menu", new XAttribute("name", "GAME"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=getnews&"))
                , new XElement("menu", new XAttribute("name", "HOT!"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=getprivilege&"))
                //, new XElement("menu", new XAttribute("name", "HOT MODEL"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=getgallery&"))
                , new XElement("menu", new XAttribute("name", "คลิปไทยลีก"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=sportclip&"))
                //, new XElement("menu", new XAttribute("name", "วิทยุ 96.0"), new XAttribute("type", "menu"), new XAttribute("url", "http://202.162.77.38:8000/SportRadio"))
                ));
            if (type == "android")
            {
                rtnXML.Element("SportApp").Element("left_menu").Add(new XElement("menu", new XAttribute("name", "SETTINGS"), new XAttribute("type", "header"), new XAttribute("url", ""))
               , new XElement("menu", new XAttribute("name", "Privacy Settings"), new XAttribute("type", "menu"), new XAttribute("url", "")));
            }

            rtnXML.Element("SportApp").Add(new XElement("button_menu",
                 new XElement("menu", new XAttribute("name", "HOT!"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=getprivilege&"), new XAttribute("format", "f1"), new XAttribute("button_act", "http://wap.isport.co.th/isportws/sportarena/images/button_teededpng_act.png"), new XAttribute("button_inact", "http://wap.isport.co.th/isportws/sportarena/images/button_teeded_inact.png"))
                , new XElement("menu", new XAttribute("name", "HOT MODEL"), new XAttribute("type", "menu"), new XAttribute("url", "http://wap.isport.co.th/isportws/sportarena.aspx?pn=getgallery&"), new XAttribute("format", "f2"), new XAttribute("button_act", "http://wap.isport.co.th/isportws/sportarena/images/button_sexy_model_act.png"), new XAttribute("button_inact", "http://wap.isport.co.th/isportws/sportarena/images/button_sexy_model_inact.png"))
                ));
            xmlDoc.Load(new AppCode_FootballList().Command_GetFootballLeague_Sportarena(rtnXML, lang, type).CreateReader());
            new AppCode_Logs().Logs_Insert("getmenulist", "", "", type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        #endregion

        #region Get Top League (favorite team)
        private string Isport_GetTopLeague(string lang, string type)
        {

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(new AppCode_FootballList().Command_GetFootballTopLeague(rtnXML, lang, type).CreateReader());
            new AppCode_Logs().Logs_Insert("GetTopLeague", "", "", type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
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
            if (bool.Parse(ConfigurationManager.AppSettings["Application_SportArena_IsCheck_Promotion"]))
            {

                string optCode = (muMobile.mobileOPT == null || muMobile.mobileOPT == "") ? AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"])) : muMobile.mobileOPT;
                if (!(optCode == "04"))
                {
                    optCode = (muMobile.mobileOPT == null || muMobile.mobileOPT == "") ? optCode : muMobile.mobileOPT;
                }
                if (optCode == "05")
                {
                    // 3GX
                    isActive = "Y";
                }
                else
                {
                    // เช็คว่าเกินวันที่ให้ฟรีหรือยัง
                    SubscribeMember sm = AppCode_Subscribe.IsportAppCheckExpirePromotion(muMobile.mobileNumber, Request["imsi"], Request["imei"], muMobile.mobileOPT
                        , AppCode_Base.AppName.SportArena.ToString(), ConfigurationManager.AppSettings["Application_SportArena_Day_Promotion"]);
                    if (sm.status == "Y")
                    {
                        // Y = เกินวันที่ให้ฟรี
                        isActive = "N";
                        // Check Dup
                        //string msisdn = muMobile.mobileNumber == "" || muMobile.mobileNumber == null ? AppCode_Subscribe.CheckActive_AppMember(Request["imsi"], Request["imei"], AppCode_Base.AppName.StarSoccer.ToString()).msisdn : muMobile.mobileNumber;
                       //isActive = AppCode_Subscribe.CheckActive_IsportStarSoccer_AppMember(optCode, msisdn, Request["imsi"], Request["imei"], AppCode_Base.AppName.StarSoccer.ToString());

                    }
                }

            }
            return isActive;
        }
        #endregion

        #region Get Main Page
        private string Isport_GetDataMainPage(string contestGroupId, string sportType, string rowCount, string countryId, string lang, string type)
        {
            if (type != "genfile" && contestGroupId == "")
            {

                xmlDoc.LoadXml(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_DataMainPageNew"].ToString()));
            }
            else
            {
                XDocument rtnXML = new XDocument(new XElement("SportApp",
                    new XAttribute("header", "")
                    , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                    , new XAttribute("smsMessage", ConfigurationManager.AppSettings["wordingSMS"])
                    ));
                rowCount = rowCount == "" ? "5" : rowCount;
                string timeRef = DateTime.Now.ToString("HH:mm");
                TimeSpan diff24 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 00, 00);
                TimeSpan diff18 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 00, 00);
                decimal addTime = (diff18.Hours < 0 && diff24.Hours >= 0) ? 0 : +1;

                rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, "", lang, contestGroupId, AppCode_LiveScore.MatchType.inprogress, 0, "N");
                if (rtnXML.Element("SportApp").Element("League") == null)
                {
                    rtnXML.Element("SportApp").Element("status").Remove();
                    rtnXML.Element("SportApp").Element("message").Remove();
                    rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, "", lang, contestGroupId, AppCode_LiveScore.MatchType.Finished, 0, "N");
                }

                //rtnXML = new AppCode_News().Command_GetNews_AIS(rtnXML, contestGroupId, sportType, rowCount, lang, countryId, type);
                rtnXML = new AppCode_News().Command_GetNews_Test(rtnXML, contestGroupId, sportType, rowCount, lang, countryId, type);

                //rtnXML = new AppCode_News().Command_GetNewsImages_AIS(rtnXML, contestGroupId, "00008", rowCount, lang, countryId, type, "HotModel");

                rtnXML.Element("SportApp").Add(new XElement("HotModel"
                              , new XAttribute("news_id", "1")
                              , new XAttribute("contestgroupid", "1")
                              , new XAttribute("news_images_190", "http://ads.samartmedia.com/www/delivery/avw.php?zoneid=600&amp;cb=INSERT_RANDOM_NUMBER_HERE&amp;n=a8bb8fe4")
                              , new XAttribute("news_images_1000", "http://ads.samartmedia.com/www/delivery/avw.php?zoneid=600&amp;cb=INSERT_RANDOM_NUMBER_HERE&amp;n=a8bb8fe4")
                              , new XAttribute("news_images_600", "http://ads.samartmedia.com/www/delivery/avw.php?zoneid=599&amp;cb=INSERT_RANDOM_NUMBER_HERE&amp;n=a64c0bfe")
                              , new XAttribute("news_images_400", "http://ads.samartmedia.com/www/delivery/avw.php?zoneid=600&amp;cb=INSERT_RANDOM_NUMBER_HERE&amp;n=a8bb8fe4")
                              , new XAttribute("news_images_350", "http://ads.samartmedia.com/www/delivery/avw.php?zoneid=600&amp;cb=INSERT_RANDOM_NUMBER_HERE&amp;n=a8bb8fe4")
                              , new XAttribute("news_images_description", "SMS Sport Buffet"))
                              );

                xmlDoc.Load(rtnXML.CreateReader());

            }
            new AppCode_Logs().Logs_Insert("GetDataMainPage", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        #endregion

        #region Get Sport Image
        private string Isport_GetSportImages(string contestGroupId, string sportType, string rowCount, string countryId, string lang, string type)
        {
            // contestGroupId => ข่าว by league
            // countryId => ข่าว by ประเทศ

            rowCount = rowCount == "" ? "20" : rowCount;
            //new AppCode_Logs().Logs_Insert("StarSoccer", contestGroupId, countryId, type, Context.Request.ServerVariables["REMOTE_ADDR"].ToString());

            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", "")
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                , new XAttribute("smsMessage", ConfigurationManager.AppSettings["wordingSMS"])
                ));
            rtnXML = new AppCode_News().Command_GetNewsImages_AIS(rtnXML, contestGroupId, sportType, rowCount, lang, countryId, type,"News");
            xmlDoc.Load(rtnXML.CreateReader());

            // Insert Logs
            new AppCode_Logs().Logs_Insert("StarSoccer", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        #endregion

        #region Get News
        private string Isport_GetSportNews(string date, string contestGroupId, string sportType, string rowCount, string countryId, string lang, string type)
        {
            // contestGroupId => ข่าว by league
            // countryId => ข่าว by ประเทศ
            contestGroupId = (contestGroupId == "") ? ConfigurationManager.AppSettings["contestGroupDefault"].ToString() : contestGroupId;
            
            //new AppCode_Logs().Logs_Insert("StarSoccer", contestGroupId, countryId, type, Context.Request.ServerVariables["REMOTE_ADDR"].ToString());
            string newsDate = (date == null || date == "") ? DateTime.Now.ToString("yyyyMMdd") : date;

            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", "")
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                , new XAttribute("smsMessage", ConfigurationManager.AppSettings["wordingSMS"])
                ));
            if (sportType == "00001")
            {
                rowCount = rowCount == "" ? "1" : rowCount;
                rtnXML = new AppCode_News().Command_GetNewsByDate(rtnXML, newsDate, sportType, rowCount, lang, "", type);
            }
            else
            {
                rowCount = rowCount == "" ? "20" : rowCount;
                rtnXML = new AppCode_News().Command_GetNews(rtnXML, contestGroupId, sportType, rowCount, lang, countryId, type);
            }
            //rtnXML = new AppCode_News().Command_GetNews_AIS(rtnXML, contestGroupId, sportType, rowCount, lang, countryId, type);
            xmlDoc.Load(rtnXML.CreateReader());
            // Insert Logs
            new AppCode_Logs().Logs_Insert("StarSoccer", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        #endregion

        #region Get Score Detail
        private string Isport_GetScoreDetail(string matchId, string mschId, string lang, string type)
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
            return xmlDoc.InnerXml;
        }
        #endregion

        #region Get Socre
        private string Isport_GetScore(string scoreDate, string contestGroupId, string countryId, string lang, string type)
        {
            countryId = countryId == null ? "" : countryId;
            if (type != "genfile" && contestGroupId == "" && scoreDate == DateTime.Now.ToString("yyyyMMdd"))
            {
                xmlDoc.LoadXml(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_ScoreNew"].ToString()));
            }
            else
            {
                scoreDate = scoreDate == "" ? DateTime.Now.ToString("yyyyMMdd") : scoreDate;
                int year = (scoreDate != null && scoreDate.Length > 4) ? int.Parse(scoreDate.Substring(0, 4)) : DateTime.Now.Year;
                year = (year > 2550) ? year - 543 : year;
                scoreDate = year.ToString() + scoreDate.Substring(4);

                AppCode_LiveScore ls = new AppCode_LiveScore();
                scoreDate = scoreDate == "" ? DateTime.Now.ToString("yyyyMMdd") : scoreDate;
                TimeSpan diffDate = new DateTime(int.Parse(scoreDate.Substring(0, 4)), int.Parse(scoreDate.Substring(4, 2)), int.Parse(scoreDate.Substring(6, 2))) - DateTime.Now;

                XDocument rtnXML = new XDocument(new XElement("SportApp",
                             new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                             , new XAttribute("date", AppCode_LiveScore.DateText(scoreDate))
                             ));
                rtnXML = ls.CommandGetScoreResult(rtnXML, "3405", scoreDate, lang);// บอลไทย
                rtnXML = ls.CommandGetScore(rtnXML, countryId, lang, contestGroupId, AppCode_LiveScore.MatchType.Finished, diffDate.Days, "N");
                xmlDoc.Load(rtnXML.CreateReader());

            }

            new AppCode_Logs().Logs_Insert("GetScore", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        #endregion

        #region LiveScore
        private string Isport_GetLiveScore(string contestGroupId, string countryId, string lang,  string type)
        {
            countryId = countryId == null ? "" : countryId;
            if (type != "genfile" && contestGroupId == "")
            {
                xmlDoc.LoadXml(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_LiveScoreNew"].ToString()));
            }
            else
            {
                string timeRef = DateTime.Now.ToString("HH:mm");
                TimeSpan diff24 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 00, 00);
                TimeSpan diff18 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 00, 00);
                decimal addTime = (diff18.Hours < 0 && diff24.Hours >= 0) ? 0 : +1; // เวลาปัจจุบันเลย 18.00 ถ้าเลย Day + 1 
                XDocument rtnXML = new XDocument(new XElement("SportApp",
                             new XAttribute("header", "")
                             , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                             ));
                rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, countryId, lang, contestGroupId, AppCode_LiveScore.MatchType.inprogress, addTime, "N");

                if (rtnXML.Element("SportApp").Element("League") == null)
                {
                    rtnXML.Element("SportApp").Element("status").Remove();
                    rtnXML.Element("SportApp").Element("message").Remove();
                    rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, "", lang, contestGroupId, AppCode_LiveScore.MatchType.Finished, 0, "N");
                }

                rtnXML.Element("SportApp").Add(new XAttribute("radio", "http://202.162.77.38:8000/SportRadio"));
                xmlDoc.Load(rtnXML.CreateReader());

            }

            new AppCode_Logs().Logs_Insert("LiveScore", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        #endregion

        #region Funtong
        private string Isport_GetFuntong(string contestGroupId, string countryId, string lang, string type)
        {

            xmlDoc.Load(new AppCode_Tded().CommangGetFuntong(contestGroupId, countryId, lang).CreateReader());
            new AppCode_Logs().Logs_Insert("มองอย่างเซียน", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        #endregion

        #region Tded
        public string Isport_GetTded(string lang, string type)
        {

            try
            {

                string optCode = muMobile.mobileOPT;//AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"]));

                rtnXML.Element("SportApp").Add(new XAttribute("wording1", "เช็คทีเด็ดก่อนเชียร์บอล ไม่ผิดหวัง!!"));
                rtnXML.Element("SportApp").Add(new XAttribute("wording2", ""));
                rtnXML = new AppCode_Subscribe().CommandGetSMSService_IsportStarSoccer(rtnXML, optCode, ConfigurationManager.AppSettings["MpCode_AISSPORTARENA"].ToString());

                if (rtnXML.Element("SportApp").Element("status") == null)
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", ""));
                }
                xmlDoc.Load(rtnXML.CreateReader());
                new AppCode_Logs().Logs_Insert("Tded", "", "", type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GenSMSService >>" + ex.Message);
            }
        }
        #endregion

        #region List Team By LeagueId
        private string Isport_GetTeam(string contestGroupId, string lang, string type)
        {

            contestGroupId = contestGroupId == "" ? "21" : contestGroupId;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_FootballAnalysis().CommandGetTeam(contestGroupId, lang).CreateReader());
            new AppCode_Logs().Logs_Insert("GetTeam", contestGroupId, "", type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        #endregion

        #region League Table
        private string Isport_GetLeagueTable(string contestGroupId, string lang, string type)
        {

            contestGroupId = contestGroupId == "" ? "21" : contestGroupId;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_FootballAnalysis().CommandGetLeagueTable(contestGroupId, lang).CreateReader());
            new AppCode_Logs().Logs_Insert("LeagueTable", contestGroupId, "", type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        #endregion

        #region Football Analyse
        private string Isport_GetFootballAnalyseDetail(string contestGroupId, string teamCode1, string teamCode2, string matchId, string lang, string type)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_FootballAnalysis().CommandGetSportpoolAnalysis(contestGroupId, teamCode1, teamCode2, matchId, lang, type).CreateReader());
            new AppCode_Logs().Logs_Insert("SportpoolAnalyse", contestGroupId, "", type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        private string Isport_GetFootballAnalyse(string contestGroupId, string countryId, string lang, string type)
        {

            
           rtnXML =  new AppCode_FootballProgram().CommandGetAnalysisByLeague_test(rtnXML,contestGroupId, countryId, lang);
            rtnXML.Element("SportApp").Add(new XAttribute("ivr_call", "025020455"));
            xmlDoc.Load(rtnXML.CreateReader());
            new AppCode_Logs().Logs_Insert("FootballAnalyse", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        #endregion

        #region Program
        private string Isport_GetFootballProgramByMonth(string contestGroupId, string year, string month, string lang, string type)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_FootballProgram().Command_GetProgramByMonth(contestGroupId, year, month, lang).CreateReader());
            new AppCode_Logs().Logs_Insert("ProgramByMonth", contestGroupId, "", type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }

        private string Isport_GetFootballProgram(string date,string contestGroupId, string countryId, string lang, string type)
        {

            date = (date == "" || date == null) ? DateTime.Now.ToString("yyyyMMdd") : date ;
            rtnXML.Element("SportApp").Attribute("date").Value = AppCode_LiveScore.DateText(date);
            xmlDoc.Load(new AppCode_FootballProgram().CommandGetProgramByDate(rtnXML, date, contestGroupId, countryId, lang).CreateReader());
            new AppCode_Logs().Logs_Insert("Program", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;

        }
        #endregion

        #region Notification
        private string Isport_GetNotification(string countryId, string minute, string lang,  string type)
        {
            //Context.Request.UserHostName

            if (type != "genfile" && countryId == "")
            {
                xmlDoc.LoadXml(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_Notification"].ToString()));
            }
            else
            {
                int iMinute = minute == "" ? int.Parse(ConfigurationManager.AppSettings["ApplicationnotificationMinute"].ToString()) : int.Parse(minute);
                xmlDoc.Load(new AppCode_Utility().CommandGetNotification(countryId, iMinute, lang).CreateReader());

            }
            new AppCode_Logs().Logs_Insert("Notification", "", countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;
        }
        #endregion

        #region Sport Clip
        private string Isport_GetSportClip(string sportType, string rowCount, string lang, string type)
        {

            rowCount = rowCount == "" ? "20" : rowCount;
            xmlDoc.Load(new AppCode_News().Command_GetSportClip(sportType, rowCount, lang, type).CreateReader());
            new AppCode_Logs().Logs_Insert("SportClip", "", sportType, type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
            return xmlDoc.InnerXml;

        }
        #endregion
    }
}
