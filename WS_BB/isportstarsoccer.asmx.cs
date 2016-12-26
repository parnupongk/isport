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
    /// Summary description for isportstarsoccer
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class isportstarsoccer : System.Web.Services.WebService
    {

        //========================================================================================================
        //========================================================================================================
        //
        //
        //                                      service นี้ใช้กับ App Starsoccer version เก่า ที่จะบังคับให้ลูกค้า Download Version ใหม่ ( เอา Data ออกทั้งหมด )
        //
        //
        //========================================================================================================
        //========================================================================================================

        [WebMethod(Description = "Get Notification")]
        public XmlElement Isport_GetNotification(string countryId, string minute, string lang, string code, string type)
        {
            //Context.Request.UserHostName
            XmlDocument xmlDoc = new XmlDocument();
            //if (countryId == "")
            //{
                //xmlDoc.LoadXml(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_Notification"]));
            //}
            //else
            //{
                countryId = countryId == "" ? "21,22,135,116,85,19,13,14" : countryId;
                int iMinute = minute == "" ? int.Parse(ConfigurationManager.AppSettings["ApplicationnotificationMinute"].ToString()) : int.Parse(minute);
                XDocument rtnXML = new XDocument(new XElement("SportApp"));
                rtnXML.Element("SportApp").Add(new XElement("notification", "none"));

                xmlDoc.Load(rtnXML.CreateReader());
                
           // }
            //new AppCode_Logs().Logs_Insert("StarSoccer_Notification", "", countryId, type, code, AppCode_Base.AppName.StarSoccer3GX.ToString(), "", "", "", "", "", AppCode_Base.GETIP());
            return xmlDoc.DocumentElement;
        }


        [WebMethod(Description = "Get โปรแกรมทั้งฤดูกาล ")]
        public XmlElement Isport_GetFootballProgramByMonth(string contestGroupId, string year, string month, string lang, string code, string type)
        {
            new AppCode_Logs().Logs_Insert("StarSoccer_ProgramByMonth", contestGroupId, "", type, code, AppCode_Base.AppName.StarSoccer3GX.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_FootballProgram().Command_GetProgramByMonth(contestGroupId, year, month, lang).CreateReader());
            return xmlDoc.DocumentElement;
        }


        [WebMethod(Description = "Get ข้อมูลหน้าหลัก (ข่าว + ผลสด)")]
        public XmlElement Isport_GetDataMainPage(string contestGroupId, string sportType, string rowCount, string countryId, string lang, string code, string type)
        {
            // contestGroupId => ข่าว by league
            // countryId => ข่าว by ประเทศ
            //contestGroupId = (contestGroupId == "") ? ConfigurationManager.AppSettings["contestGroupDefault"].ToString() : contestGroupId;
            XmlDocument xmlDoc = new XmlDocument();
            //if (type != "genfile" && contestGroupId == "")
            //{
                //xmlDoc.LoadXml(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_DataMainPage_StarSoccer"]));
           // }
            //else
            //{
                XDocument rtnXML = new XDocument(new XElement("SportApp",
                    new XAttribute("header", "")
                    , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                    , new XAttribute("smsMessage", ConfigurationManager.AppSettings["wordingSMS"])
                    ));

                rowCount = rowCount == "" ? "8" : rowCount;
                string timeRef = DateTime.Now.ToString("HH:mm");
                TimeSpan diff24 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 00, 00);
                TimeSpan diff18 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 00, 00);
                decimal addTime = (diff18.Hours < 0 && diff24.Hours >= 0) ? 0 : +1;
                //rtnXML = new AppCode_News().Command_GetNews(rtnXML, contestGroupId, sportType, rowCount, lang, countryId, type);
                //rtnXML = new AppCode_LiveScore().CommandGetTextScore(rtnXML, countryId, lang, contestGroupId, AppCode_LiveScore.MatchType.inprogress, addTime, "N");

                rtnXML.Element("SportApp").Add(new XElement("News"
                               , new XAttribute("news_id", "")
                               , new XAttribute("news_header", "Free Update Version")
                               , new XAttribute("news_title", "กรุณา Update Version ฟรี!!!")
                               , new XAttribute("news_detail", "<br/>วิธี Download<br/><br/>  เข้าไปที่ play store>>ค้นหา 'Starsoccer'>> Click Install<br/><br/> หรือ <br/><br/><a href='market://details?id=com.isport_starsoccer' target='_blank'>Click Download</a>")
                               , new XAttribute("news_images_190", "http://wap.isport.co.th/isportws/images/ic_launcher.png")
                               , new XAttribute("news_images_1000", "http://wap.isport.co.th/isportws/images/ic_launcher.png")
                               , new XAttribute("news_images_600", "http://wap.isport.co.th/isportws/images/ic_launcher.png")
                               , new XAttribute("news_images_400", "http://wap.isport.co.th/isportws/images/ic_launcher.png")
                               , new XAttribute("news_images_350", "http://wap.isport.co.th/isportws/images/ic_launcher.png")
                               , new XAttribute("news_images_description", "")
                               , new XAttribute("news_url_fb", "")
                               ));
                rtnXML.Element("SportApp").Add(
                      new XElement("status", "success")
                      , new XElement("message", ""));
                rtnXML.Element("SportApp").Add(
                          new XElement("Score_score", "success")
                          , new XElement("message_score", ""));

                rtnXML.Element("SportApp").Add(new XElement("Score_score", "กรุณา Update Version."));

                xmlDoc.Load(rtnXML.CreateReader());
            //}
            return xmlDoc.DocumentElement;
        }


        [WebMethod(Description = "Get คู่ที่มีการวิเคราะห์ (ถ้าไม่ส่ง contestGroupId,countryId จะ get ทั้งหมดของวันนั้น)")]
        public XmlElement Isport_GetFootballAnalyse(string contestGroupId, string countryId, string lang, string code, string type)
        {
            new AppCode_Logs().Logs_Insert("StarSoccer_FootballAnalyse", contestGroupId, countryId, type, code, AppCode_Base.AppName.StarSoccer3GX.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
            XmlDocument xmlDoc = new XmlDocument();
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingHeader"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));


            rtnXML.Element("SportApp").Add(new XElement("League",
                                 new XAttribute("contestGroupName", "กรุณา Update Version")
                                , new XAttribute("contestGroupId", "")
                                , new XAttribute("contestURLImages", "http://wap.isport.co.th/isportws/images/ic_launcher.png")
                                , new XAttribute("tmName", "กรุณา Update Version")
                                , new XAttribute("tmSystem", "กรุณา Update Version")));

            //rtnXML.Element("SportApp").Element("League").Add(
            //                new XElement("Match",
            //                new XAttribute("mschId", "")
            //                , new XAttribute("matchId", "")
            //                , new XAttribute("teamCode1", "") // id isportfeed
            //                , new XAttribute("teamCode2", "") // id isportfeed
            //                , new XAttribute("teamName1", "-")
            //                , new XAttribute("teamName2", "-")
            //                , new XAttribute("liveChannel", "")
            //                , new XAttribute("matchDate", "")
            //                , new XAttribute("matchTime", "")
            //                , new XAttribute("isDetail", "false")
            //                ));
            rtnXML.Element("SportApp").Add(
                  new XElement("status", "success")
                  , new XElement("message", ""));
            xmlDoc.Load(new AppCode_FootballProgram().CommandGetAnalysisByLeague(contestGroupId, countryId, lang).CreateReader());
            xmlDoc.Load(rtnXML.CreateReader());
            return xmlDoc.DocumentElement;
        }


        [WebMethod(Description = "Get ข้อมูล ผล,โปรแกรม,ตารางคะแนน หน้าสยามกีฬา")]
        public XmlElement Isport_GetSiamSportAllTag(string contestGroupId, string sportType, string countryId, string lang, string code, string type)
        {
            // contestGroupId == ScsId (isport.dbo.sport_class_season)
            new AppCode_Logs().Logs_Insert("StarSoccer_SiamSportAllTag", contestGroupId, "", type, code, AppCode_Base.AppName.StarSoccer3GX.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
            XmlDocument xmlDoc = new XmlDocument();
            XDocument xml = new XDocument(new XElement("SportApp",
                new XAttribute("header", "")
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                , new XAttribute("iconFileName", sportType + ".png")
                ));
            if (sportType == "00001")
            {
                contestGroupId = contestGroupId == "" ? "2612" : contestGroupId;
                // ผลการแข่งขัน
                xml.Element("SportApp").Add(new AppCode_LiveScore().CommandLiveScore("FootballThaiScore", contestGroupId, sportType, true, lang));

                // โปรแกรมการแข่งขัน
                xml.Element("SportApp").Add(new AppCode_FootballProgram().CommandGetFootballProgram("FootballThaiProgram", contestGroupId, lang));

                // ตารางคะแนน
                xml.Element("SportApp").Add(new AppCode_FootballAnalysis().CommandGetFootballAnalysislevelByscsIdXML("FootballThaiTable", contestGroupId, lang));

            }
            else
            {
                contestGroupId = contestGroupId == "" && sportType == "00002" ? "2824" : contestGroupId;
                contestGroupId = contestGroupId == "" && sportType == "00003" ? "2803" : contestGroupId;
                // ผลการแข่งขัน
                xml.Element("SportApp").Add(new AppCode_News().Command_GetSportContent("OtherSportScore", "00007", contestGroupId, lang, sportType));

                // โปรแกรมการแข่งขัน
                xml.Element("SportApp").Add(new AppCode_News().Command_GetSportContent("OtherSportProgram", "00006", contestGroupId, lang, sportType));

            }

            xml.Element("SportApp").Add(
                  new XElement("status", "success")
                  , new XElement("message", ""));

            xmlDoc.Load(xml.CreateReader());
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get ข่าว สยามกีฬา")]
        public XmlElement Isport_GetSiamSportNews(string contestGroupId, string sportType, string newsRowCount, string lang, string code, string type)
        {
            // contestGroupId == ScsId (isport.dbo.sport_class_season)
            new AppCode_Logs().Logs_Insert("StarSoccer_SiamSportNews", contestGroupId, "", type, code, AppCode_Base.AppName.StarSoccer3GX.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
            XmlDocument xmlDoc = new XmlDocument();
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                         new XAttribute("header", "")
                         , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                         , new XAttribute("smsMessage", ConfigurationManager.AppSettings["wordingSMS"])
                         ));
            newsRowCount = newsRowCount == "" ? "10" : newsRowCount;
            contestGroupId = contestGroupId == "" ? "2612" : contestGroupId;
            if (sportType == "00001")
            {
                rtnXML = new AppCode_News().Command_GetNewsFootballThai(rtnXML, contestGroupId, int.Parse(sportType).ToString(), newsRowCount, lang, "", type);
            }
            else
            {
                // ไม่ส่ง contentGroupId ไปเพราะว่ากีฬาอื่นๆ siamsport ไม่ได้แยกย่อยลงไป 
                rtnXML = new AppCode_News().Command_GetNewsBySportType(sportType, newsRowCount, lang, type);
            }
            xmlDoc.Load(rtnXML.CreateReader());
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get ข่าว (By contestGroupId or countryId)")]
        public XmlElement Isport_GetSportNews(string newsDate, string sportType, string rowCount, string countryId, string lang, string code, string type)
        {
            // contestGroupId => ข่าว by league
            // countryId => ข่าว by ประเทศ


            rowCount = rowCount == "" ? "1" : rowCount;
            new AppCode_Logs().Logs_Insert("StarSoccer_News", newsDate, countryId, type, code, AppCode_Base.AppName.StarSoccer3GX.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
            XmlDocument xmlDoc = new XmlDocument();
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", "")
                , new XAttribute("date", AppCode_LiveScore.DateText(newsDate))
                , new XAttribute("smsMessage", ConfigurationManager.AppSettings["wordingSMS"])
                ));
            //rtnXML = new AppCode_News().Command_GetNewsByDate(rtnXML, newsDate, sportType, rowCount, lang, countryId, type);
            rtnXML.Element("SportApp").Add(new XElement("News"
                               , new XAttribute("news_id", "")
                               , new XAttribute("news_header", "Free Update Version")
                               , new XAttribute("news_title", "กรุณา Update Version ฟรี!!!")
                               , new XAttribute("news_detail", "<br/>วิธี Download<br/><br/>  เข้าไปที่ play store>>ค้นหา 'Starsoccer'>> Click Install<br/><br/> หรือ <br/><br/><a href='market://details?id=com.isport_starsoccer' target='_blank'>Click Download</a>")
                               , new XAttribute("news_images_190", "http://wap.isport.co.th/isportws/images/ic_launcher.png")
                               , new XAttribute("news_images_1000", "http://wap.isport.co.th/isportws/images/ic_launcher.png")
                               , new XAttribute("news_images_600", "http://wap.isport.co.th/isportws/images/ic_launcher.png")
                               , new XAttribute("news_images_400", "http://wap.isport.co.th/isportws/images/ic_launcher.png")
                               , new XAttribute("news_images_350", "http://wap.isport.co.th/isportws/images/ic_launcher.png")
                               , new XAttribute("news_images_description", "")
                               , new XAttribute("news_url_fb", "")
                               ));
            rtnXML.Element("SportApp").Add(
                  new XElement("status", "success")
                  , new XElement("message", ""));
            xmlDoc.Load(rtnXML.CreateReader());

            // Insert Logs

            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get ตารางคะแนน (By contestGroupId )")]
        public XmlElement Isport_GetLeagueTable(string contestGroupId, string lang, string code, string type)
        {
            new AppCode_Logs().Logs_Insert("StarSoccer_LeagueTable", contestGroupId, "", type, code, AppCode_Base.AppName.StarSoccer3GX.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
            contestGroupId = contestGroupId == "" ? "21" : contestGroupId;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_FootballAnalysis().CommandGetLeagueTable(contestGroupId, lang).CreateReader());
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Score (By contestGroupId or countryId)")]
        public XmlElement Isport_GetScore(string scoreDate, string contestGroupId, string countryId, string lang, string code, string type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //if (contestGroupId == "" && scoreDate == DateTime.Now.ToString("yyyyMMdd"))
            //{
                //xmlDoc.LoadXml(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_Score"]));
            //}
            //else
            //{
                scoreDate = scoreDate == "" ? DateTime.Now.ToString("yyyyMMdd") : scoreDate;
                TimeSpan diffDate = new DateTime(int.Parse(scoreDate.Substring(0, 4)), int.Parse(scoreDate.Substring(4, 2)), int.Parse(scoreDate.Substring(6, 2))) - DateTime.Now;

                XDocument rtnXML = new XDocument(new XElement("SportApp",
                             new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                             , new XAttribute("date", AppCode_LiveScore.DateText(scoreDate))
                             ));
               // rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, countryId, lang, contestGroupId, AppCode_LiveScore.MatchType.Finished, diffDate.Days, "N");
                rtnXML.Element("SportApp").Add( new XElement("League",
                                 new XAttribute("contestGroupName", "กรุณา Update Version")
                                , new XAttribute("contestGroupId", "")
                                , new XAttribute("contestURLImages", "http://wap.isport.co.th/isportws/images/ic_launcher.png")
                                , new XAttribute("tmName", "กรุณา Update Version")
                                , new XAttribute("tmSystem", "กรุณา Update Version")));
                rtnXML.Element("SportApp").Element("League").Add(new XElement("Score"
                               , new XAttribute("teamCode1", "")
                               , new XAttribute("teamCode2", "")
                               , new XAttribute("teamName1", "")
                               , new XAttribute("teamName2", "")
                               , new XAttribute("status", "")
                               , new XAttribute("minutes", "")
                               , new XAttribute("curent_period", "")
                               , new XAttribute("score_home", "")
                               , new XAttribute("score_away", "")
                               , new XAttribute("score_home_ht", "")
                               , new XAttribute("score_away_ht","")
                               , new XAttribute("matchId", "")
                               , new XAttribute("mschId", "")
                               , new XAttribute("matchDate", "")
                               , new XAttribute("contestGroupId", "")
                               , new XAttribute("isDetail", "false")
                               ));
                rtnXML.Element("SportApp").Add(new XElement("status", "success")
                            , new XElement("message", ""));

                xmlDoc.Load(rtnXML.CreateReader());
           // }
                new AppCode_Logs().Logs_Insert("StarSoccer_GetScore", contestGroupId, countryId, type, code, AppCode_Base.AppName.StarSoccer3GX.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
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

        [WebMethod(Description = "Get วิเคราะห์,เอเชียนแฮนดิแคป,สถิติเชิงลึก,ผลที่คาด")]
        public XmlElement Isport_GetSportpoolAnalyse(string contestGroupId, string teamCode1, string teamCode2, string matchId, string lang, string code, string type)
        {
            new AppCode_Logs().Logs_Insert("StarSoccer_SportpoolAnalyse", contestGroupId, "", type, code, AppCode_Base.AppName.StarSoccer3GX.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_FootballAnalysis().CommandGetSportpoolAnalysis(contestGroupId, teamCode1, teamCode2, matchId, lang, type).CreateReader());
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get โปรแกรม (ถ้าไม่ส่ง contestGroupId,countryId จะ get ทั้งหมดของวันนั้น)")]
        public XmlElement Isport_GetFootballProgram(string contestGroupId, string countryId, string lang, string code, string type)
        {
            new AppCode_Logs().Logs_Insert("StarSoccer_Program", contestGroupId, countryId, type, code, AppCode_Base.AppName.StarSoccer3GX.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
            //xmlDoc.Load(new AppCode_FootballProgram().CommandGetProgramByLeague(contestGroupId, countryId, lang).CreateReader());
            XmlDocument xmlDoc = new XmlDocument();
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingHeader"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));

            rtnXML.Element("SportApp").Add(new XElement("League",
                     new XAttribute("contestGroupName", "กรุณา Update Version")
                    , new XAttribute("contestGroupId", "")
                    , new XAttribute("contestURLImages", "http://wap.isport.co.th/isportws/images/ic_launcher.png")
                    , new XAttribute("tmName", "กรุณา Update Version")
                    , new XAttribute("tmSystem", "กรุณา Update Version")));

            rtnXML.Element("SportApp").Add(
      new XElement("status", "success")
      , new XElement("message", ""));

            xmlDoc.Load(rtnXML.CreateReader());
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Sport Clip")]
        public XmlElement Isport_GetSportClip(string sportType, string rowCount, string code, string lang, string type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            rowCount = rowCount == "" ? "15" : rowCount;
            xmlDoc.Load(new AppCode_News().Command_GetSportClip(sportType, rowCount, lang, type).CreateReader());
            new AppCode_Logs().Logs_Insert("StarSoccer_SportClip", "", sportType, type, code, AppCode_Base.AppName.StarSoccer3GX.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get LiveScore (By contestGroupId or countryId) ")]
        public XmlElement Isport_GetLiveScore(string contestGroupId, string countryId, string lang, string code, string type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //if (contestGroupId == "")
            //{
                //xmlDoc.LoadXml(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_LiveScore"]));
           // }
           // else
            //{
                string timeRef = DateTime.Now.ToString("HH:mm");
                TimeSpan diff24 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 00, 00);
                TimeSpan diff18 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 00, 00);
                decimal addTime = (diff18.Hours < 0 && diff24.Hours >= 0) ? 0 : +1;
                

                XDocument rtnXML = new XDocument(new XElement("SportApp",
                             new XAttribute("header", "")
                             , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                             ));
                //rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, countryId, lang, contestGroupId, AppCode_LiveScore.MatchType.inprogress, addTime, "N");
                rtnXML.Element("SportApp").Add(new XElement("League",
                                 new XAttribute("contestGroupName", "กรุณา Update Version")
                                , new XAttribute("contestGroupId", "")
                                , new XAttribute("contestURLImages", "http://wap.isport.co.th/isportws/images/ic_launcher.png")
                                , new XAttribute("tmName", "กรุณา Update Version")
                                , new XAttribute("tmSystem", "กรุณา Update Version")));
                rtnXML.Element("SportApp").Element("League").Add(new XElement("Score"
                               , new XAttribute("teamCode1", "")
                               , new XAttribute("teamCode2", "")
                               , new XAttribute("teamName1", "")
                               , new XAttribute("teamName2", "")
                               , new XAttribute("status", "")
                               , new XAttribute("minutes", "")
                               , new XAttribute("curent_period", "")
                               , new XAttribute("score_home", "")
                               , new XAttribute("score_away", "")
                               , new XAttribute("score_home_ht", "")
                               , new XAttribute("score_away_ht", "")
                               , new XAttribute("matchId", "")
                               , new XAttribute("mschId", "")
                               , new XAttribute("matchDate", "")
                               , new XAttribute("contestGroupId", "")
                               , new XAttribute("isDetail", "false")
                               ));
                rtnXML.Element("SportApp").Add(new XElement("status", "success")
                            , new XElement("message", ""));
                xmlDoc.Load(rtnXML.CreateReader());
            //}
                new AppCode_Logs().Logs_Insert("StarSoccer_LiveScore", contestGroupId, countryId, type, code, AppCode_Base.AppName.StarSoccer3GX.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get ListMenu StarSoccer")]
        public XmlElement Isport_GetListMenu(string lang, string code, string type)
        {
            new AppCode_Logs().Logs_Insert("StarSoccre_ListMenu", "", "", type, code, AppCode_Base.AppName.StarSoccer3GX.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_FootballList().Command_GetFootballLeague_StarSoccer(lang, type).CreateReader());
            return xmlDoc.DocumentElement;
        }

    }
}
