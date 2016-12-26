using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.Script.Services;

namespace WS_BB
{
    /// <summary>
    /// Summary description for isporttpl
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class isporttpl : System.Web.Services.WebService
    {

        [WebMethod]
        public XmlElement  Isport_GetScoreResult(string teamCode,string lang,string mobileNumber,string model,string type,string projectCode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XDocument xDoc = new XDocument(new XElement("SportApp", new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                                                                                                        ,new XAttribute("header","") ));
            xDoc = new AppCode_LiveScore().CommandgetMatchScheduleByResultType(xDoc, "SportApp", "Result", teamCode);
            xmlDoc.Load(xDoc.CreateReader());
            new AppCode_Logs().Logs_Insert("MobileLS_GetScoreResult", "", projectCode, model, mobileNumber,AppCode_Base.AppName.MobileLifeStyle.ToString(),"","","","","",AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod]
        public XmlElement Isport_GetProgram(string teamCode, string lang, string mobileNumber, string model, string type, string projectCode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XDocument xDoc = new XDocument(new XElement("SportApp", new XAttribute("date", AppCode_LiveScore.DateTimeText(DateTime.Now.ToString("yyyyMMddHHmm")))
                                                                                                        , new XAttribute("header", "")));
            xDoc = new AppCode_LiveScore().CommandgetMatchScheduleByResultType(xDoc, "SportApp", "Program", teamCode);
            xmlDoc.Load(xDoc.CreateReader());
            new AppCode_Logs().Logs_Insert("MobileLS_GetProgram", "", projectCode, model, mobileNumber, AppCode_Base.AppName.MobileLifeStyle.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get Football News")]
        public XmlElement Isport_GetFootballNews(string teamCode,string lang,string mobileNumber,string model,string type,string projectCode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XDocument xDoc = new XDocument(new XElement("SportApp", new XAttribute("date", AppCode_LiveScore.DateTimeText(DateTime.Now.ToString("yyyyMMddHHmm")))
                                                                                                        , new XAttribute("header", "ไทยพรีเมียร์ลีก")
                                                                                                        ,new XAttribute("type","ข่าว")));
            xDoc = new AppCode_News().Command_GetNewsFootballThai(xDoc, "2612", int.Parse("00001").ToString(), "10", lang, "", type);
            xmlDoc.Load(xDoc.CreateReader());
            new AppCode_Logs().Logs_Insert("MobileLS_GetNews", "", projectCode, model, mobileNumber, AppCode_Base.AppName.MobileLifeStyle.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;    
        }

        [WebMethod(Description = "Get League Table")]
        public XmlElement Isport_GetLeagueTable(string leagueId ,string lang,string mobileNumber,string model,string type,string projectCode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XDocument xDoc = new XDocument(new XElement("SportApp", new XAttribute("date", AppCode_LiveScore.DateTimeText(DateTime.Now.ToString("yyyyMMddHHmm")))
                                                                                                        , new XAttribute("header", "")));
            xDoc.Element("SportApp").Add(new AppCode_FootballAnalysis().CommandGetFootballAnalysislevelByscsIdXML_IStore(leagueId, lang));

            xDoc.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", ""));

            xmlDoc.Load(xDoc.CreateReader());
            new AppCode_Logs().Logs_Insert("MobileLS_GetLeagueTable", "", projectCode, model, mobileNumber, AppCode_Base.AppName.MobileLifeStyle.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description="Get football statistic ")]
        public XmlElement Isport_GetFootballStatistic(string teamCode, string lang, string mobileNumber, string model, string type, string projectCode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XDocument xDoc = new XDocument(new XElement("SportApp", new XAttribute("date", AppCode_LiveScore.DateTimeText(DateTime.Now.ToString("yyyyMMddHHmm")))
                                                                                                        , new XAttribute("header", "")));
            xDoc = new AppCode_FootballAnalysis().CommandGetFootballStatistic(xDoc,"SportApp",teamCode, lang);
            xmlDoc.Load(xDoc.CreateReader());
            new AppCode_Logs().Logs_Insert("MobileLS_GetFootballStatistic", "", projectCode, model, mobileNumber, AppCode_Base.AppName.MobileLifeStyle.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get football Clip ")]
        public XmlElement Isport_GetFootballClip(string teamCode,string pageNumber, string lang, string mobileNumber, string model, string type, string projectCode)
        {
            pageNumber = pageNumber == "" ? "1" : pageNumber;
            XmlDocument xmlDoc = new XmlDocument();
            XDocument xDoc = new XDocument(new XElement("SportApp", new XAttribute("date", AppCode_LiveScore.DateTimeText(DateTime.Now.ToString("yyyyMMddHHmm")))
                                                                                                        , new XAttribute("header", "")
                                                                                                        , new XAttribute("URL", "http://wap.isport.co.th/isportclip/index.aspx?mp_code=0023&projectCode=LSP&teamCode=" + teamCode + "&lang=th")));
            xDoc = new AppCode_News().CommandGetContentClipbyTeamCode(xDoc, pageNumber ,"SportApp", lang, teamCode);
            xmlDoc.Load(xDoc.CreateReader());
            new AppCode_Logs().Logs_Insert("MobileLS_GetFootballClip", "", projectCode, model, mobileNumber, AppCode_Base.AppName.MobileLifeStyle.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get xml wap ")]
        public XmlElement Isport_GetXmlWap(string lang, string mobileNumber, string model, string type, string projectCode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XDocument xDoc = new XDocument(new XElement("SportApp", new XAttribute("date", AppCode_LiveScore.DateTimeText(DateTime.Now.ToString("yyyyMMddHHmm")))
                                                                                                        , new XAttribute("header", "")));

            xDoc.Element("SportApp").Add(new XElement("service", new XAttribute("is", "520"), new XAttribute("description", "สมัคร sms สรุปผลบอลลีกดัง"), new XAttribute("url", "http://wap.isport.co.th/?s=520")));
            xDoc.Element("SportApp").Add(new XElement("service", new XAttribute("is", "519"), new XAttribute("description", "สมัคร sms สปอร์ตพูล"), new XAttribute("url", "http://wap.isport.co.th/?s=519")));
            xDoc.Element("SportApp").Add(new XElement("service", new XAttribute("is", "521"), new XAttribute("description", "สมัคร sms สตาร์ซอคเก้อร์"), new XAttribute("url", "http://wap.isport.co.th/?s=521")));

            xDoc.Element("SportApp").Add(new XElement("status", "success")
                   , new XElement("message", ""));

            xmlDoc.Load(xDoc.CreateReader());
            new AppCode_Logs().Logs_Insert("MobileLS_GetXMLWap", "", projectCode, model, mobileNumber, AppCode_Base.AppName.MobileLifeStyle.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get mobile ",EnableSession=true)]
        public string Isport_GetMobile()
        {
            MobileLibrary.MobileUtilities mU =  new MobileLibrary.MobileUtilities();
            return mU.mobileNumber;

        }

    }
}
