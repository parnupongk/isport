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
    /// Summary description for feedais
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class feedais : System.Web.Services.WebService
    {

        //===========================================================================
        //
        //                                  ใช้สำหรับ Feed ข้อมูล Euro 2012 ให้ AIS เท่านั้น  < ContentGroupId = 807 >
        //
        //===========================================================================
        [WebMethod(Description = "Get LiveScore Euro 2012")]
        public XmlElement Isport_GetLiveScore(string lang, string msisdn, string type)
        {
            string contestGroupId = "807";
            contestGroupId = "";
            string timeRef = DateTime.Now.ToString("HH:mm");
            TimeSpan diff24 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 00, 00);
            TimeSpan diff18 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 00, 00);
            decimal addTime = (diff18.Hours < 0 && diff24.Hours >= 0) ? 0 : +1;

            new AppCode_Logs().Logs_Insert("FeedAis_LiveScore", contestGroupId, "", type, msisdn,AppCode_Base.AppName.FeedAis.ToString(),"","","","","",AppCode_Base.GETIP(),"");
            XmlDocument xmlDoc = new XmlDocument();
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                         new XAttribute("header", "")
                         , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                         ));
            rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, "", lang, contestGroupId, AppCode_LiveScore.MatchType.inprogress, addTime, "N");
            xmlDoc.Load(rtnXML.CreateReader());
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Score ")]
        public XmlElement Isport_GetScore( string lang, string msisdn, string type)
        {
            string scoreDate =  DateTime.Now.ToString("yyyyMMdd") ;
            string contestGroupId = "807";
            contestGroupId = "";
            TimeSpan diffDate = new DateTime(int.Parse(scoreDate.Substring(0, 4)), int.Parse(scoreDate.Substring(4, 2)), int.Parse(scoreDate.Substring(6, 2))) - DateTime.Now;

            new AppCode_Logs().Logs_Insert("FeedAis_GetScore", contestGroupId, "", type, msisdn, AppCode_Base.AppName.FeedAis.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            XmlDocument xmlDoc = new XmlDocument();
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                         new XAttribute("header", "")
                         , new XAttribute("date", AppCode_LiveScore.DateText(scoreDate))
                         ));
            rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, "", lang, contestGroupId, AppCode_LiveScore.MatchType.Finished, diffDate.Days, "N");
            xmlDoc.Load(rtnXML.CreateReader());
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Score Detail")]
        public XmlElement Isport_GetScoreDetail(string matchId, string mschId, string lang, string msisdn, string type)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_LiveScore().CommandGetScoreDetail(matchId, mschId, lang).CreateReader());
            //new AppCode_Logs().Logs_Insert_IP();
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Program Euro 2012")]
        public XmlElement Isport_GetFootballProgram( string lang, string msisdn, string type)
        {
            string contestGroupId = "807";
            contestGroupId = "";
            new AppCode_Logs().Logs_Insert("FeedAis_Program", contestGroupId, "", type, msisdn, AppCode_Base.AppName.FeedAis.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_FootballProgram().CommandGetProgramByLeague(contestGroupId, "", lang).CreateReader());
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get News euro 2012")]
        public XmlElement Isport_GetSportNews( string rowCount ,string lang, string msisdn, string type)
        {
            // contestGroupId => ข่าว by league
            // countryId => ข่าว by ประเทศ
            string contestGroupId = "807";
            //contestGroupId = "697";
            rowCount = rowCount == "" ? "5" : rowCount;
            //new AppCode_Logs().Logs_Insert("StarSoccer", contestGroupId, countryId, type, Context.Request.ServerVariables["REMOTE_ADDR"].ToString());
            new AppCode_Logs().Logs_Insert("FeedAis_News", contestGroupId, "", type, msisdn, AppCode_Base.AppName.FeedAis.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            XmlDocument xmlDoc = new XmlDocument();
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", "")
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                , new XAttribute("smsMessage", ConfigurationManager.AppSettings["wordingSMS"])
                ));
            rtnXML = new AppCode_News().Command_GetNewsFeedAis(rtnXML, contestGroupId, "00001", rowCount, lang, "", type);
            xmlDoc.Load(rtnXML.CreateReader());

            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Football Analysis Euro 2012")]
        public XmlElement Isport_GetFootballAnalyse(string lang, string msisdn, string type)
        {
            string contestGroupId = "807";
            contestGroupId = "";
            new AppCode_Logs().Logs_Insert("FeedAis_FootballAnalyse", contestGroupId, "", type, msisdn, AppCode_Base.AppName.FeedAis.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_FootballProgram().CommandGetAnalysisByLeagueFeedAis(contestGroupId, "", lang).CreateReader());
            return xmlDoc.DocumentElement;
        }

    }
}
