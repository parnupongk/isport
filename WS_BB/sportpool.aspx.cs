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
    public partial class sportpool : feedMain
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public override string CheckPageName(string pn, string appName)
        {
            string rtn = "";
            //optCode = muMobile.mobileOPT == null || muMobile.mobileOPT == "" ? AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"])) : muMobile.mobileOPT;
            //optCode = optCode == "" ? muMobile.mobileOPT : optCode;
            try
            {
                if (pn == "bannerclick")
                {
                    new AppCode_Logs().Logs_Insert("isportsportpool_bannerclick", "", "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.SportPool.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "Y");
                    //Response.Redirect("http://wap.isport.co.th/isportui/indexl.aspx?p=d02campaignh&mp_code=0096", false);
                    string prjName = (Request["p"] == null) ? "bb" : Request["p"];
                    Response.Redirect("http://wap.isport.co.th/isportui/indexl.aspx?p=" + prjName + "&mp_code=0103", false);
                }
                else if (pn == "notification")
                {
                    rtn = Isport_Notification();
                }
                else if (pn == "program")
                {
                    rtn = GetProgram(Request["date"], Request["contestgroupid"], Request["countryid"], Request["lang"], Request["type"]);
                }
                else if (pn == "programanalyse")
                {
                    rtn = isport_GetAnalysis(Request["contestgroupid"], Request["sporttype"], Request["lang"], Request["type"]);
                }
                else if (pn == "footballanalysedetail")
                {
                    rtn = Isport_GetAnalyseDetail(Request["contestgroupid"], Request["teamcode1"], Request["teamcode2"], Request["matchid"], Request["lang"], Request["type"]);
                }
                else if (pn == "leaguetable")
                {
                    rtn = GetLeagueTable(appName, Request["contestgroupid"], Request["sporttype"], Request["lang"]);
                }
                else if (pn == "playerbyteam")
                {
                    rtn = Isport_GetPlayerByTeam(Request["teamid"], Request["lang"], Request["type"]);
                }
                else if (pn == "programbyteam")
                {
                    rtn = Isport_GetProgramByTeam(Request["teamid"], Request["lang"], Request["type"]);
                }
                else if (pn == "livescore")
                {
                    rtn = Isport_GetLiveScore(Request["contestgroupid"], Request["countryid"], Request["lang"], Request["type"]);
                }
                else if (pn == "getscore")
                {
                    rtn = GetResult(appName, Request["contestgroupid"], Request["date"],Request["sporttype"],  Request["lang"],false);
                }
                else if (pn == "scoredetail")
                {
                    rtn = GetScoreDetail(appName,Request["matchid"], "", Request["lang"]);
                }
                else if (pn == "listmenu")
                {
                    rtn = Isport_GetListMenu(Request["lang"], Request["type"]);
                }
                else if (pn == "getabout")
                {
                    //rtn = Isport_GetAbout(AppCode_Base.AppName.SportArena.ToString(), Request["lang"], Request["type"]);
                }
                else if (pn == "getsmsservice")
                {
                    rtn = GenSMSService(Request["lang"], Request["type"], Request["sporttype"]);
                }
                else if (pn == "matchlike")
                {
                    rtn = Isport_GenMatchLike(Request["matchid"], Request["team1"], Request["team2"], Request["star"], AppCode_Base.GETIP(), Request["imei"], Request["imsi"], muMobile.mobileNumber,optCode );
                    //new AppCode_Logs().Logs_Insert("isportsportpool_matchlike", "", "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.SportPool.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "N");
                }
                else if (pn == "tded")
                {
                    rtn = GetTdedSportPool(Request["lang"]);
                    //new AppCode_Logs().Logs_Insert("isportsportpool_tded", "", "", Request["type"], muMobile.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.SportPool.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], optCode, AppCode_Base.GETIP(), "N");
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private string Isport_Notification()
        {
            try
            {
                rtnXML = new AppCode_Utility().CommandGetNotification_SportPool(rtnXML, "", 1);
                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("Isport_Notification >> " + ex.Message);
            }
        }

        #region Match Like , Match Star
        /// <summary>
        /// Isport_GenMatchLike 
        /// </summary>
        /// <param name="matchId"> Isport Feed Match ID</param>
        /// <param name="team1"></param>
        /// <param name="team2"></param>
        /// <param name="star"></param>
        /// <param name="ip"></param>
        /// <param name="imei"></param>
        /// <param name="imsi"></param>
        /// <param name="phoneNo"></param>
        /// <param name="optCode"></param>
        /// <returns></returns>
        private string Isport_GenMatchLike(string matchId, string team1
            ,string team2,string star,string ip
            ,string imei,string imsi,string phoneNo,string optCode)
        {
            try
            {
                rtnXML = new AppCode_MatchLike().CommandMatchLike(rtnXML, matchId, team1, team2, star, ip, imei, imsi, phoneNo, optCode);

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;

            }
            catch (Exception ex)
            {
                throw new Exception("Isport_GenMatchLike>>" + ex.Message);
            }
        }
        #endregion

        #region Program
        private string GetProgram(string date, string contentGroupId, string sportType, string lang, string type)
        {
            try
            {
                // GetProgram
                date = (date == null || date == "") ? DateTime.Now.ToString("yyyyMMdd") : date;
                int year = (date != null && date.Length > 4) ? int.Parse(date.Substring(0, 4)) : DateTime.Now.Year;
                year = (year > 2550) ? year - 543 : year;
                date = year.ToString() + date.Substring(4);
                date = (date == null || date == "") ? DateTime.Now.ToString("yyyyMMdd") : date;
                contentGroupId = (contentGroupId == null || contentGroupId == "") ? "" : contentGroupId;

                rtnXML.Element("SportApp").SetAttributeValue("date", AppCode_LiveScore.DateText(date));
                rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));

                if (type != "genfile" && date == DateTime.Now.ToString("yyyyMMdd"))
                {
                    try
                    {
                        rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_Program_Sportpool"].ToString()));
                    }
                    catch { rtnXML = new AppCode_FootballProgram().CommandGetProgramByDate_SportPool(rtnXML, date, contentGroupId, "", lang); }
                }
                else
                {
                    rtnXML = new AppCode_FootballProgram().CommandGetProgramByDate_SportPool(rtnXML, date, contentGroupId, "", lang);
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

        #region Get List Menu
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
        private string GetOperatorBanner()
        {
            try
            {
                string urlIcon = ConfigurationManager.AppSettings["Application_IsportSportPool_URLIBanner_Default"];
                if (optCode == "01") urlIcon = ConfigurationManager.AppSettings["Application_IsportSportPool_URLIBanner_AIS"];
                else if (optCode == "02") urlIcon = ConfigurationManager.AppSettings["Application_IsportSportPool_URLIBanner_Dtac"];
                else if (optCode == "03" || optCode == "04") urlIcon = ConfigurationManager.AppSettings["Application_IsportSportPool_URLIBanner_True"];
                else if (optCode == "05") urlIcon = ConfigurationManager.AppSettings["Application_IsportSportPool_URLIBanner_3GX"];
                return urlIcon;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private string Isport_GetListMenu(string lang, string type)
        {
            try
            {

                rtnXML.Element("SportApp").Add(new XAttribute("version", ConfigurationManager.AppSettings["Application_SportArena_Version"].ToString()));
                rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));
                rtnXML.Element("SportApp").Add(new XAttribute("adview", (DateTime.Now.Hour > 0 && DateTime.Now.Hour < 6) ? "true" : ConfigurationManager.AppSettings["Application_IsportSportPool_adView"].ToString()));
                rtnXML.Element("SportApp").Add(new XAttribute("urlicon", GetOperatorIcon()));
                rtnXML.Element("SportApp").Add(new XAttribute("urlbanner", GetOperatorBanner()));
                rtnXML.Element("SportApp").Add(new XAttribute("msisdn", (muMobile.mobileOPT == "03" || muMobile.mobileOPT == "04") ? "" : muMobile.mobileNumber));// ลูกค้า true จะให้กด *4511 ในการสมัครบริการเท่านั้น

                string checkActive = CheckActiveandPromotion(AppCode_Base.AppName.SportPool,Request["imsi"],Request["imei"]
                    , ConfigurationManager.AppSettings["Application_SportPool_Day_Promotion"], bool.Parse(ConfigurationManager.AppSettings["Application_SportPool_IsCheck_Promotion"]), ConfigurationManager.AppSettings["Application_SportPool_PSSVID"]);


                //if (AppCode_Subscribe.CheckIsOTPWait(Request["imsi"], Request["imei"], AppCode_Base.AppName.SportPool.ToString()) == "N")

                //
                //============ Sportpool ไม่มี OTP นะจ๊ะ =======================
                //
                // E : คือ charg mt ไม่ได้เกิน 7 วันติดต่อกัน  // update 20/02/2015 พี่เจน confirm ให้รอไปใช้งานรวมกับ MO
                if (checkActive == "N" || checkActive == "E")
                {
                    string header = "", detail = "", footer = "", footer1 = "";
                    if (muMobile.mobileNumber == "")
                    {
                        // บังคับให้ปิด wifi
                        header = ConfigurationManager.AppSettings["Application_SportPool_Header"];
                        detail = ConfigurationManager.AppSettings["Application_SportPool_Footer"];//ConfigurationManager.AppSettings["Application_SportPool_Detail"];
                        footer = "";//ConfigurationManager.AppSettings["Application_SportPool_Footer"];
                        footer1 = "";//ConfigurationManager.AppSettings["Application_SportPool_Footer1"];
                    }
                    else //if( checkActive== "N" )
                    {
                        header = ConfigurationManager.AppSettings["Application_SportPool_Header"];
                        detail = ConfigurationManager.AppSettings["Application_SportPool_Detail"];
                        footer = "";//ConfigurationManager.AppSettings["Application_SportPool_Footer"];
                        footer1 = "";//ConfigurationManager.AppSettings["Application_SportPool_Footer1"];
                    }

                    // ลูกค้า true จะให้กด *4511 ในการสมัครบริการเท่านั้น
                    detail = (muMobile.mobileOPT == "03" || muMobile.mobileOPT == "04") ? ConfigurationManager.AppSettings["Application_SportPool_Detail_true"] : detail;

                    rtnXML.Element("SportApp").Add(new XElement("Active",
                                                                       new XAttribute("isactive", checkActive)
                                                                    , new XAttribute("otp_status", "new")
                                                                    , new XAttribute("header", header)
                                                                    , new XAttribute("detail", detail)
                                                                    , new XAttribute("footer", footer)
                                                                    , new XAttribute("footer1", footer1)));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("Active",
                                                                       new XAttribute("isactive", checkActive)
                                                                    , new XAttribute("otp_status", "active") // wait OTP
                                                                    , new XAttribute("header", "สมัครสมาชิก ")
                                                                    , new XAttribute("detail", "กรุณายืนยันรหัสผ่าน ")
                                                                    , new XAttribute("footer", "กรุณารอรับ sms password")));
                }

                #region Menu
                rtnXML.Element("SportApp").Add(new XElement("left_menu",
                     new XElement("menu", new XAttribute("name", "ทีมโปรด"), new XAttribute("name_en", "Team"), new XAttribute("type", "menu"), new XAttribute("url", ""))
                     , new XElement("menu", new XAttribute("name", "ผลสด"), new XAttribute("name_en", "Livescore"), new XAttribute("type", "menu"), new XAttribute("url", ""))
                     , new XElement("menu", new XAttribute("name", "สรุปผล"), new XAttribute("name_en", "Result"), new XAttribute("type", "menu"), new XAttribute("url", ""))
                    , new XElement("menu", new XAttribute("name", "วิเคราะห์-วิจารณ์"), new XAttribute("name_en", "Analyse"), new XAttribute("type", "menu"), new XAttribute("url", ""))
                    , new XElement("menu", new XAttribute("name", "บอลชุดน่าเชียร์"), new XAttribute("name_en", "Tded"), new XAttribute("type", "menu"), new XAttribute("url", ""))
                    , new XElement("menu", new XAttribute("name", "โปรแกรม"), new XAttribute("name_en", "Program"), new XAttribute("type", "menu"), new XAttribute("url", ""))
                    , new XElement("menu", new XAttribute("name", "ตารางคะแนน"), new XAttribute("name_en", "Leaguetable"), new XAttribute("type", "menu"), new XAttribute("url", ""))
                    , new XElement("menu", new XAttribute("name", "บริการ SMS"), new XAttribute("name_en", "ServiceSMS"), new XAttribute("type", "menu"), new XAttribute("url", ""))
                    ));

                #endregion

                rtnXML = new AppCode_FootballList().Command_GetFootballLeague_IsportStarSoccer(rtnXML, lang, type);


                xmlDoc.Load(rtnXML.CreateReader());
                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("SportPool_GetListMenu >> " + ex.Message);
            }
        }
        #endregion

        #region Get Player of Team
        public string Isport_GetPlayerByTeam(string teamId, string lang, string type)
        {
            try
            {
                rtnXML = new AppCode_FootballAnalysis().CommandGetPlayerTeam(rtnXML, teamId, lang);
                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("SportPool_GetPlayerByTeam>>" + ex.Message);
            }
        }
        #endregion

        #region Get Analyse

        private string isport_GetAnalysis(string contestGroupId, string sportType, string lang, string type)
        {
            try
            {
                // GetAnalysis
                contestGroupId = (contestGroupId == null || contestGroupId == "") ? "" : contestGroupId;

                if (type != "genfile" && contestGroupId == "")
                {
                    rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_Analyse_Sportpool"].ToString()));
                }
                else
                {
                    rtnXML = new AppCode_FootballProgram().CommandGetAnalysisByLeague_SportPool(rtnXML, contestGroupId, "", lang);
                }
                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("isport_GetAnalysis >>" + ex.Message);
            }
        }
        #endregion

        #region Get Analyse Detail
        private string Isport_GetAnalyseDetail(string contestGroupId,string teamCode1,string teamCode2,string matchId,string lang,string type)
        {
            try
            {
                xmlDoc.Load(new AppCode_FootballAnalysis().CommandGetSportpoolAnalysisAllLeague(rtnXML,contestGroupId, teamCode1, teamCode2, matchId, lang, type).CreateReader());
                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("Isport_GetAnalyseDetail >> " + ex.Message);
            }
        }
        #endregion

        #region Program by Team
        public string Isport_GetProgramByTeam(string teamId, string lang, string type)
        {
            try
            {
                string scoreDate =  DateTime.Now.AddDays(-7).ToString("yyyyMMdd") ;
                TimeSpan diffDate = new DateTime(int.Parse(scoreDate.Substring(0, 4)), int.Parse(scoreDate.Substring(4, 2)), int.Parse(scoreDate.Substring(6, 2))) - DateTime.Now;
                // สรุปผลทีมโปรด
                rtnXML = new AppCode_LiveScore().CommandGetScoreByTeamId(rtnXML, teamId, lang, AppCode_LiveScore.MatchType.Finished, diffDate.Days, "N");

                // โปรแกรมทีมโปรด
                xmlDoc.Load(new AppCode_FootballProgram().CommandGetProgramByTeam_SportPool(rtnXML, teamId, lang).CreateReader());
                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("SportPool_GetSportpoolAnalyse>>" + ex.Message);
            }
        }
        #endregion

        #region LiveScore
        private string Isport_GetLiveScore(string contestGroupId, string countryId, string lang, string type)
        {
            countryId = countryId == null ? "" : countryId;
            string timeRef = DateTime.Now.ToString("HH:mm");
            TimeSpan diff24 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 00, 00);
            TimeSpan diff18 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 00, 00);
            decimal addTime = (diff18.Hours < 0 && diff24.Hours >= 0) ? 0 : +1; // เวลาปัจจุบันเลย 18.00 ถ้าเลย Day + 1 
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                         new XAttribute("header", "")
                         , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                         ));
            if (type != "genfile" && contestGroupId == "")
            {
                try
                {
                    xmlDoc.LoadXml(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_LiveScoreNew"].ToString()));
                }
                catch {
                    rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, countryId, lang, contestGroupId, AppCode_LiveScore.MatchType.inprogress, addTime, "N");
                }
            }
            else
            {

                rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, countryId, lang, contestGroupId, AppCode_LiveScore.MatchType.inprogress, addTime, "N");

                if (rtnXML.Element("SportApp").Element("League") == null)
                {
                    rtnXML.Element("SportApp").Element("status").Remove();
                    rtnXML.Element("SportApp").Element("message").Remove();
                    rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, "", lang, contestGroupId, AppCode_LiveScore.MatchType.Finished, 0, "N");
                }


                xmlDoc.Load(rtnXML.CreateReader());

            }

            //new AppCode_Logs().Logs_Insert("LiveScore", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP());
            return xmlDoc.InnerXml;
        }
        #endregion

        #region Get SMS Service
        private string GenSMSService(string lang , string type,string sportType)
        {
            try
            {

                string optCode = muMobile.mobileOPT;// AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"]));

                rtnXML.Element("SportApp").Add(new XAttribute("wording1", "เช็คทีเด็ดก่อนเชียร์บอล ไม่ผิดหวัง!!"));
                rtnXML.Element("SportApp").Add(new XAttribute("wording2", ""));
                rtnXML.Element("SportApp").Add(new XAttribute("banner", "http://wap.isport.co.th/isportws/banner/isportsportpool_banner.html"));
                rtnXML.Element("SportApp").Add(new XAttribute("adview", (DateTime.Now.Hour > 0 && DateTime.Now.Hour < 6) ? "true" : ConfigurationManager.AppSettings["Application_IsportStarSoccer_adView"].ToString()));
                
                rtnXML = new AppCode_Utility().CommangGetPrivilege_IsportSportPool(rtnXML, AppCode_Base.AppName.SportPool.ToString(), lang, type, optCode);

                // news free
                rtnXML = new AppCode_News().Command_GetNewsByDate(rtnXML, DateTime.Now.ToString("yyyyMMdd"), sportType, "1", lang, "", type);

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
    }
}
