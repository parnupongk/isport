using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Data;

namespace WS_BB
{
    /// <summary>
    /// Summary description for isportapp
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class isportapp : System.Web.Services.WebService
    {

        [WebMethod(Description = "Get Sport Clip")]
        public XmlElement Isport_GetSportClip(string sportType,string rowCount, string code,string lang, string type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            rowCount = rowCount == "" ? "20" : rowCount;
            xmlDoc.Load(new AppCode_News().Command_GetSportClip(sportType, rowCount, lang, type).CreateReader());
            new AppCode_Logs().Logs_Insert("SportClip", "", sportType, type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Setting")]
        public DataSet Isport_GetSetting(string type)
        {
            return new AppCode_Utility().CommandGetSetting();
        }

        [WebMethod(Description = "Save Setting")]
        public XmlElement Isport_SaveSetting(string setting, string tokenid, string code, string type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_Utility().CommandSaveSetting(setting, tokenid, code, type).CreateReader());
            new AppCode_Logs().Logs_Insert("SaveSetting", "", setting, type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Notification")]
        public DataSet Isport_GetDataNotification(string minute)
        {
            //Context.Request.UserHostName
            int iMinute = minute == "" ? int.Parse(ConfigurationManager.AppSettings["ApplicationnotificationMinute"].ToString()) : int.Parse(minute);
            return new AppCode_Utility().GetNotification(iMinute);
        }

        [WebMethod(Description = "Get Notification")]
        public XmlElement Isport_GetNotification(string countryId,string minute, string lang, string code, string type)
        {
            //Context.Request.UserHostName
            XmlDocument xmlDoc = new XmlDocument();
            if (type != "genfile" && countryId =="" )
            {
                xmlDoc.LoadXml(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_Notification"].ToString()));
            }
            else
            {
                int iMinute = minute == "" ? int.Parse(ConfigurationManager.AppSettings["ApplicationnotificationMinute"].ToString()) : int.Parse(minute);
                xmlDoc.Load(new AppCode_Utility().CommandGetNotification(countryId, iMinute, lang).CreateReader());
                
            }
            //new AppCode_Logs().Logs_Insert("Notification", "", countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP());
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get โปรแกรม (ถ้าไม่ส่ง contestGroupId,countryId จะ get ทั้งหมดของวันนั้น)")]
        public XmlElement Isport_GetFootballProgram(string contestGroupId, string countryId,string lang, string code, string type)
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            countryId = ""; // มีการส่ง countryid เข้าไปเยอะทำให้ logs insert ไม่ได้
            xmlDoc.Load(new AppCode_FootballProgram().CommandGetProgramByLeague(contestGroupId, countryId,lang).CreateReader());
            new AppCode_Logs().Logs_Insert("Program", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get โปรแกรมทั้งฤดูกาล ")]
        public XmlElement Isport_GetFootballProgramByMonth(string contestGroupId, string year, string month, string lang,string code, string type)
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_FootballProgram().Command_GetProgramByMonth(contestGroupId, year,month, lang).CreateReader());
            new AppCode_Logs().Logs_Insert("ProgramByMonth", contestGroupId, "", type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get คู่ที่มีการวิเคราะห์ (ถ้าไม่ส่ง contestGroupId,countryId จะ get ทั้งหมดของวันนั้น)")]
        public XmlElement Isport_GetFootballAnalyse(string contestGroupId, string countryId, string lang, string code, string type)
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            countryId = ""; // มีการส่ง countryid เข้าไปเยอะทำให้ logs insert ไม่ได้
            xmlDoc.Load(new AppCode_FootballProgram().CommandGetAnalysisByLeague(contestGroupId, countryId, lang).CreateReader());
            new AppCode_Logs().Logs_Insert("FootballAnalyse", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get วิเคราะห์,เอเชียนแฮนดิแคป,สถิติเชิงลึก,ผลที่คาด")]
        public XmlElement Isport_GetSportpoolAnalyse(string contestGroupId,string teamCode1,string teamCode2,string matchId,string lang, string code, string type)
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_FootballAnalysis().CommandGetSportpoolAnalysis(contestGroupId, teamCode1, teamCode2, matchId, lang,type).CreateReader());
            new AppCode_Logs().Logs_Insert("SportpoolAnalyse", contestGroupId, "", type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get ตารางคะแนน (By contestGroupId )")]
        public XmlElement Isport_GetLeagueTable(string contestGroupId, string lang,string code, string type)
        {
            
            contestGroupId = contestGroupId == "" ? "21" : contestGroupId;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_FootballAnalysis().CommandGetLeagueTable(contestGroupId,lang).CreateReader());
            new AppCode_Logs().Logs_Insert("LeagueTable", contestGroupId, "", type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get ทีเด็ด")]
        public XmlElement Isport_GetTded(string lang,string code, string type)
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_Tded().CommangGetTded(lang).CreateReader());
            new AppCode_Logs().Logs_Insert("Tded", "", "", type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get มองอย่างเซียน")]
        public XmlElement Isport_GetFuntong(string contestGroupId, string countryId, string lang, string code, string type)
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_Tded().CommangGetFuntong(contestGroupId,countryId, lang).CreateReader());
            new AppCode_Logs().Logs_Insert("มองอย่างเซียน", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get LiveScore (By contestGroupId or countryId) ")]
        public XmlElement Isport_GetLiveScore(string contestGroupId, string countryId, string lang, string code, string type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            countryId = ""; // มีการส่ง countryid เข้าไปเยอะทำให้ logs insert ไม่ได้
            if (type != "genfile" && contestGroupId == "")
            {
                xmlDoc.LoadXml(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_LiveScore"].ToString()));
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
                //rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, countryId, lang, contestGroupId, AppCode_LiveScore.MatchType.inprogress, addTime, "N");
                rtnXML = new AppCode_LiveScore().CommandLiveScore_(rtnXML, "", "00001", false, lang);
                if (rtnXML.Element("SportApp").Element("League") == null)
                {
                    rtnXML.Element("SportApp").Element("status").Remove();
                    rtnXML.Element("SportApp").Element("message").Remove();
                    rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, "", lang, contestGroupId, AppCode_LiveScore.MatchType.Finished, 0, "N");
                }

                xmlDoc.Load(rtnXML.CreateReader());
                
            }
            new AppCode_Logs().Logs_Insert("LiveScore", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Score (By contestGroupId or countryId)")]
        public XmlElement Isport_GetScore(string scoreDate,string contestGroupId,string countryId, string lang, string code, string type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (type != "genfile" && contestGroupId == "" && scoreDate ==DateTime.Now.ToString("yyyyMMdd"))
            {
                xmlDoc.LoadXml(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_Score"].ToString()));
            }
            else
            {
                AppCode_LiveScore ls = new AppCode_LiveScore();
                scoreDate = scoreDate == "" ? DateTime.Now.ToString("yyyyMMdd") : scoreDate;
                TimeSpan diffDate = new DateTime(int.Parse(scoreDate.Substring(0, 4)), int.Parse(scoreDate.Substring(4, 2)), int.Parse(scoreDate.Substring(6, 2))) - DateTime.Now;

                XDocument rtnXML = new XDocument(new XElement("SportApp",
                             new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                             , new XAttribute("date", AppCode_LiveScore.DateText(scoreDate))
                             ));
                rtnXML = ls.CommandGetScore(rtnXML, countryId, lang, contestGroupId, AppCode_LiveScore.MatchType.Finished, diffDate.Days, "N");
                if (rtnXML.Element("SportApp").Element("League") == null)
                {
                    rtnXML.Element("SportApp").Element("status").Remove();
                    rtnXML.Element("SportApp").Element("message").Remove();
                    rtnXML = ls.CommandGetScoreResult(rtnXML, "3405", scoreDate, lang);// บอลไทย
                    rtnXML = ls.CommandGetScore(rtnXML, "", lang, contestGroupId, AppCode_LiveScore.MatchType.Finished, 0, "N");
                }
                xmlDoc.Load(rtnXML.CreateReader());
                
            }
            new AppCode_Logs().Logs_Insert("GetScore", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Score Detail")]
        public XmlElement Isport_GetScoreDetail(string matchId, string mschId, string lang, string code, string type)
        {

            XmlDocument xmlDoc = new XmlDocument();

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
            return xmlDoc.DocumentElement;
        }
        [WebMethod(Description = "Get Score Detail")]
        public string Isport_GetFileScoreDetail(string matchId, string mschId, string lang, string code, string type)
        {

            XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(new AppCode_LiveScore().CommandGetScoreDetail(matchId, mschId, lang).CreateReader());
               // new AppCode_Logs().Logs_Insert_IP();

            return xmlDoc.InnerXml;
        }


        [WebMethod(Description = "Get ข่าว (By contestGroupId or countryId)")]
        public XmlElement Isport_GetSportNews(string contestGroupId, string sportType,string rowCount,string countryId,string lang, string code, string type)
        {
            // contestGroupId => ข่าว by league
            // countryId => ข่าว by ประเทศ
            contestGroupId  = (contestGroupId == "" )? ConfigurationManager.AppSettings["contestGroupDefault"].ToString() : contestGroupId ;
            rowCount = rowCount == "" ? "20" : rowCount;
            //new AppCode_Logs().Logs_Insert("StarSoccer", contestGroupId, countryId, type, Context.Request.ServerVariables["REMOTE_ADDR"].ToString());
            
            XmlDocument xmlDoc = new XmlDocument();
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", "")
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                , new XAttribute("smsMessage", ConfigurationManager.AppSettings["wordingSMS"])
                ));
            rtnXML = new AppCode_News().Command_GetNews_AIS(rtnXML,contestGroupId, sportType, rowCount, lang, countryId,type);
            xmlDoc.Load(rtnXML.CreateReader());
            // Insert Logs
            new AppCode_Logs().Logs_Insert("StarSoccer", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get ข่าว (Get images By contestGroupId or countryId)")]
        public XmlElement Isport_GetSportImages(string contestGroupId, string sportType, string rowCount, string countryId, string lang, string code, string type)
        {
            // contestGroupId => ข่าว by league
            // countryId => ข่าว by ประเทศ

            rowCount = rowCount == "" ? "20" : rowCount;
            //new AppCode_Logs().Logs_Insert("StarSoccer", contestGroupId, countryId, type, Context.Request.ServerVariables["REMOTE_ADDR"].ToString());
            XmlDocument xmlDoc = new XmlDocument();
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", "")
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                , new XAttribute("smsMessage", ConfigurationManager.AppSettings["wordingSMS"])
                ));
            rtnXML = new AppCode_News().Command_GetNewsImages_AIS(rtnXML, contestGroupId, sportType, rowCount, lang, countryId, type,"News");
            xmlDoc.Load(rtnXML.CreateReader());

            // Insert Logs
            new AppCode_Logs().Logs_Insert("StarSoccer", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get ข้อมูลหน้าหลัก (ข่าว + ผลสด)")]
        public XmlElement Isport_GetDataMainPage(string contestGroupId, string sportType, string rowCount, string countryId, string lang, string code, string type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (type != "genfile" && contestGroupId == "")
            {
   
                xmlDoc.LoadXml(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_DataMainPage"].ToString()));
            }
            else
            {
                XDocument rtnXML = new XDocument(new XElement("SportApp",
                    new XAttribute("header", "")
                    , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                    , new XAttribute("smsMessage", ConfigurationManager.AppSettings["wordingSMS"])
                    ));

                // if ( type =="iphone" && !AppCode_Utility.CheckAisCustomer(Context.Request.ServerVariables["REMOTE_ADDR"].ToString()))
                //{
                // rtnXML.Element("SportApp").Add(new XElement("status", "reject")
                //   , new XElement("message", "Exclusive for Ais Customer"));
                //}
                //else
                //{
                rowCount = rowCount == "" ? "8" : rowCount;
                string timeRef = DateTime.Now.ToString("HH:mm");
                TimeSpan diff24 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 00, 00);
                TimeSpan diff18 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 00, 00);
                decimal addTime = (diff18.Hours < 0 && diff24.Hours >= 0) ? 0 : +1;
                rtnXML = new AppCode_News().Command_GetNews_AIS(rtnXML, contestGroupId, sportType, rowCount, lang, countryId, type);
                rtnXML = new AppCode_LiveScore().CommandGetTextScore(rtnXML, countryId, lang, contestGroupId, AppCode_LiveScore.MatchType.inprogress, addTime, "N");
                //}

                //if (type == "iphone")
                //{
                //    WebLibrary.ExceptionManager.WriteError(" iphone IP >> " + Context.Request.ServerVariables["REMOTE_ADDR"].ToString());
                //}
                xmlDoc.Load(rtnXML.CreateReader());
                
            }
            new AppCode_Logs().Logs_Insert("GetDataMainPage", contestGroupId, countryId, type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get ListMenu StarSoccer")]
        public XmlElement Isport_GetListMenu(string lang, string code, string type)
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                             new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                             , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))));
            xmlDoc.Load(new AppCode_FootballList().Command_GetFootballLeague(rtnXML,lang, type).CreateReader());
            new AppCode_Logs().Logs_Insert("ListMenu", "", "", type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get ListMenu StarSoccer")]
        public XmlElement Isport_GetSettingMenu(string lang, string code, string type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("menuSetting.xml"));
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get About ")]
        public XmlElement Isport_GetAbout(string projectCode, string lang, string code, string type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_Utility().CommangGetAbout(projectCode,lang).CreateReader());
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Privilege ")]
        public XmlElement Isport_GetPrivilege(string projectCode, string lang, string code, string type)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", "Privilege ")
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));

                
                rtnXML = new AppCode_Utility().CommangGetPrivilege(rtnXML, projectCode, lang, type);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(rtnXML.CreateReader());
            new AppCode_Logs().Logs_Insert("Privilege", projectCode, "", type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Banner ")]
        public XmlElement Isport_GetBanner(string projectCode, string lang, string code, string type)
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_Utility().CommangGetBanner(projectCode, lang).CreateReader());
            new AppCode_Logs().Logs_Insert("Banner", projectCode, "", type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Banner News ")]
        public XmlElement Isport_GetBannerNews(string projectCode, string lang, string code, string type)
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_Utility().CommangGetBannerNews(projectCode, lang).CreateReader());
            new AppCode_Logs().Logs_Insert("Banner", projectCode, "", type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Player Top Score ")]
        public XmlElement Isport_GetPlayerTopScore(string contestGroupId, string lang, string code, string type)
        {
            //new AppCode_Logs().Logs_Insert("PlayerTopScore", contestGroupId, "", type, code);
            XmlDocument xmlDoc = new XmlDocument();
            contestGroupId = contestGroupId == "" ? "21" : contestGroupId;
            xmlDoc.Load(new AppCode_FootballAnalysis().CommandGetPlayerTopScore(contestGroupId, lang).CreateReader());
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "สำหรับ windows service( gen file score detail )")]
        public DataSet Isport_GetLiveScoreWS()
        {
            return new AppCode_LiveScore().GetScore();
        }

        #region สยามกีฬา

        [WebMethod(Description = "Get ข่าว สยามกีฬา")]
        public XmlElement Isport_GetSiamSportNews(string contestGroupId, string sportType, string newsRowCount, string lang, string code, string type)
        {
            // contestGroupId == ScsId (isport.dbo.sport_class_season)
            
            XmlDocument xmlDoc = new XmlDocument();
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                         new XAttribute("header", "")
                         , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                         , new XAttribute("smsMessage", ConfigurationManager.AppSettings["wordingSMS"])
                         ));
            newsRowCount = newsRowCount == "" ? "5" : newsRowCount;
            contestGroupId = contestGroupId == "" ? "3405" : contestGroupId;
            if (sportType == "00001")
            {
                //rtnXML = new AppCode_News().Command_GetNewsFootballThai(rtnXML, contestGroupId, int.Parse(sportType).ToString(), newsRowCount, lang, "",type);
                rtnXML = new AppCode_News().Command_GetNews_AIS(rtnXML, contestGroupId, sportType, newsRowCount, lang, "", type);
            }
            else
            {
                // ไม่ส่ง contentGroupId ไปเพราะว่ากีฬาอื่นๆ siamsport ไม่ได้แยกย่อยลงไป 
                rtnXML = new AppCode_News().Command_GetNewsBySportType_AIS(sportType, newsRowCount, lang,type);
            }
            xmlDoc.Load(rtnXML.CreateReader());
            new AppCode_Logs().Logs_Insert("SiamSportNews", contestGroupId, "", type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }


        [WebMethod(Description = "Get ข้อมูล ผล,โปรแกรม,ตารางคะแนน หน้าสยามกีฬา")]
        public XmlElement Isport_GetSiamSportAllTag(string contestGroupId,string sportType, string countryId, string lang, string code, string type)
        {
            // contestGroupId == ScsId (isport.dbo.sport_class_season)
            
            XmlDocument xmlDoc = new XmlDocument();
            XDocument xml = new XDocument(new XElement("SportApp",
                new XAttribute("header", "")
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                , new XAttribute("iconFileName",sportType + ".png" )
                ));
            if (sportType == "00001")
            {
                contestGroupId = contestGroupId == "" ? "3405" : contestGroupId;
                // ผลการแข่งขัน
                xml.Element("SportApp").Add(new AppCode_LiveScore().CommandLiveScore("FootballThaiScore",contestGroupId, sportType, true, lang));

                // โปรแกรมการแข่งขัน
                xml.Element("SportApp").Add(new AppCode_FootballProgram().CommandGetFootballProgram("FootballThaiProgram",contestGroupId, lang));

                // ตารางคะแนน
                xml.Element("SportApp").Add(new AppCode_FootballAnalysis().CommandGetFootballAnalysislevelByscsIdXML("FootballThaiTable", contestGroupId, lang));

            }
            else
            {
                //contestGroupId = contestGroupId == "" && sportType =="00002" ? "2824" : contestGroupId;
                //contestGroupId = contestGroupId == "" && sportType == "00003" ? "2803" : contestGroupId;
                // ผลการแข่งขัน
                xml.Element("SportApp").Add( new AppCode_News().Command_GetSportContent("OtherSportScore","00007", contestGroupId, lang,sportType));

                // โปรแกรมการแข่งขัน
                xml.Element("SportApp").Add( new AppCode_News().Command_GetSportContent("OtherSportProgram","00006", contestGroupId, lang,sportType));
                
            }

            xml.Element("SportApp").Add(
                  new XElement("status", "success")
                  , new XElement("message", ""));

            xmlDoc.Load(xml.CreateReader());
            new AppCode_Logs().Logs_Insert("SiamSportAllTag", contestGroupId, "", type, code, AppCode_Base.AppName.SportArena.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        #endregion

    }
}
