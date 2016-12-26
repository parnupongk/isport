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
    /// Summary description for isportphone
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class isportphone : System.Web.Services.WebService
    {

        //===================================================================================================================
        //
        //
        //                      ส่งให้กับ app content ccafe
        //
        //
        //
        //===================================================================================================================
        XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", "Isport")
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));

        public bool CheckKeyAccess(string key)
        {
            

            return (key == ConfigurationManager.AppSettings["CCAFEKEYACCESS"].ToString()) ?true:false;

        }

        private void SetErrorKeyAccess()
        {
            rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", "please request key access."));
        }

        [WebMethod(Description = "Get League Menu")]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void Isport_GetLeagueMenu(string lang, string code, string type,string key)
        {
            //new AppCode_Logs().Logs_Insert("ListMenu", "", "", type, code);
            XmlDocument xmlDoc = new XmlDocument();

            if (CheckKeyAccess(key))
            {
                xmlDoc.Load(new AppCode_FootballList().Command_GetFootballLeagueMain(lang, type).CreateReader());
            }
            else
            {

                SetErrorKeyAccess();
                xmlDoc.Load(rtnXML.CreateReader());
            }
            Context.Response.ContentType = "application/json";
            Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //ContentType = "application/json" ResponseEncoding = "UTF-8"
            Context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(xmlDoc, Newtonsoft.Json.Formatting.Indented));//xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get ข่าว สยามกีฬา")]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void Isport_GetSportNews(string contestGroupId, string sportType, string newsRowCount, string lang, string code, string type, string key)
        {
            XmlDocument xmlDoc = new XmlDocument();

            if (CheckKeyAccess(key))
            {
                
                newsRowCount = newsRowCount == "" ? "5" : newsRowCount;
                //contestGroupId = contestGroupId =="" ? "2612" : contestGroupId ;
                if (sportType == "00001")
                {
                    if (contestGroupId == "2612")
                        rtnXML = new AppCode_News().Command_GetNewsFootballThai(rtnXML, contestGroupId, int.Parse(sportType).ToString(), newsRowCount, lang, "", type);
                    else
                    {
                        rtnXML = new AppCode_News().Command_GetNews(rtnXML, contestGroupId, sportType, newsRowCount, lang, "", type);
                    }
                }
                else
                {
                    // ไม่ส่ง contentGroupId ไปเพราะว่ากีฬาอื่นๆ siamsport ไม่ได้แยกย่อยลงไป 
                    rtnXML = new AppCode_News().Command_GetNewsBySportType_forSportPhone(sportType, lang, type);
                }
            }
            else
            {
                SetErrorKeyAccess();
                xmlDoc.Load(rtnXML.CreateReader());
            }

            new AppCode_Logs().Logs_Insert("SportPhone_GetNews", contestGroupId, "", type, code, AppCode_Base.AppName.SportPhone.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
            xmlDoc.Load(rtnXML.CreateReader());
            //string json = AppCode_XmlToJson.XmlToJSON(xmlDoc);
            // return json;
            Context.Response.ContentType = "application/json";
            Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //ContentType = "application/json" ResponseEncoding = "UTF-8"
            Context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(xmlDoc, Newtonsoft.Json.Formatting.Indented));//xmlDoc.DocumentElement;

        }

        

        [WebMethod(Description = "Get ข้อมูล ตารางคะแนน,อันดับโลก")]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void Isport_GetSportTableandRanking(string contestGroupId, string sportType, string lang, string code, string type, string key)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (CheckKeyAccess(key))
            {
                if (sportType == "00001")
                {
                    if (contestGroupId == "2612")
                        // ตารางคะแนน
                        rtnXML.Element("SportApp").Add(new AppCode_FootballAnalysis().CommandGetFootballAnalysislevelByscsIdXML_IStore(contestGroupId, lang), "", "", "", "", "", AppCode_Base.GETIP());
                    else
                    {
                        rtnXML = new AppCode_FootballAnalysis().CommandGetLeagueTable_IStore(contestGroupId, lang);
                    }

                }
                else
                {
                    // อันดับโลก
                    rtnXML.Element("SportApp").Add(new AppCode_News().Command_GetSportContent_Ranking("SportRanking", sportType, lang));
                }

                rtnXML.Element("SportApp").Add(
                      new XElement("status", "success")
                      , new XElement("message", ""));
            }
            else
            {
                SetErrorKeyAccess();
                xmlDoc.Load(rtnXML.CreateReader());
            }

            new AppCode_Logs().Logs_Insert("SportPhone_GetRanking", contestGroupId, "", type, code, AppCode_Base.AppName.SportPhone.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
            xmlDoc.Load(rtnXML.CreateReader());
            // string json = AppCode_XmlToJson.XmlToJSON(xmlDoc);
            // return json;
            Context.Response.ContentType = "application/json";
            Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //ContentType = "application/json" ResponseEncoding = "UTF-8"
            Context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(xmlDoc, Newtonsoft.Json.Formatting.Indented));//xmlDoc.DocumentElement;
        }

        [WebMethod(Description = "Get ข้อมูล โปรแกรม")]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void Isport_GetSportProgram(string contestGroupId, string sportType, string lang, string code, string type, string key)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (CheckKeyAccess(key))
            {
                if (sportType == "00001")
                {
                    if (contestGroupId == "2612")
                        // โปรแกรมการแข่งขัน
                        //xml.Element("SportApp").Add(new AppCode_FootballProgram().CommandGetFootballProgram("FootballThaiProgram", contestGroupId, lang));
                        rtnXML = new AppCode_FootballProgram().CommandGetFootballProgram_iStore(rtnXML, contestGroupId, lang);

                    else
                        rtnXML = new AppCode_FootballProgram().CommandGetProgramByLeague_iStore(rtnXML, contestGroupId, "", lang);
                }
                else
                {
                    // โปรแกรมการแข่งขัน
                    rtnXML = new AppCode_News().Command_GetSportContent(rtnXML, "00006", sportType, lang);
                }

                if (contestGroupId == "2612")
                    rtnXML.Element("SportApp").Add(
                      new XElement("status", "success")
                        , new XElement("message", ""));
            }
            else
            {
                SetErrorKeyAccess();
                xmlDoc.Load(rtnXML.CreateReader());
            }
            new AppCode_Logs().Logs_Insert("SportPhone_GetProgram", contestGroupId, "", type, code, AppCode_Base.AppName.SportPhone.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
            xmlDoc.Load(rtnXML.CreateReader());
            Context.Response.ContentType = "application/json";
            Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //ContentType = "application/json" ResponseEncoding = "UTF-8"
            Context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(xmlDoc, Newtonsoft.Json.Formatting.Indented));//xmlDoc.DocumentElement;
        }


        [WebMethod(Description = "Get ข้อมูล ผล")]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void Isport_GetSportResult(string contestGroupId, string sportType, string lang, string code, string type, string key)
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (CheckKeyAccess(key))
            {
                if (sportType == "00001")
                {
                    if (contestGroupId == "2612")
                        // ผลการแข่งขัน
                        rtnXML = new AppCode_LiveScore().CommandLiveScore_IStore(rtnXML, contestGroupId, sportType, true, lang);
                    else
                    {
                        rtnXML = new AppCode_LiveScore().CommandGetScore_IStore(rtnXML, contestGroupId, lang);
                    }
                }
                else
                {
                    // ผลการแข่งขัน
                    rtnXML = new AppCode_News().Command_GetSportContent(rtnXML, "00007", sportType, lang);
                }

                if (contestGroupId == "2612")
                    rtnXML.Element("SportApp").Add(
                      new XElement("status", "success")
                      , new XElement("message", ""));
            }
            else
            {
                SetErrorKeyAccess();
                xmlDoc.Load(rtnXML.CreateReader());
            }
            new AppCode_Logs().Logs_Insert("SportPhone_GetResult", contestGroupId, "", type, code, AppCode_Base.AppName.SportPhone.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");
            xmlDoc.Load(rtnXML.CreateReader());

            Context.Response.ContentType = "application/json";
            Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //ContentType = "application/json" ResponseEncoding = "UTF-8"
            Context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(xmlDoc, Newtonsoft.Json.Formatting.Indented));//xmlDoc.DocumentElement;
        }

        [WebMethod(Description = " รายละเอียดผลบอล ")]
        public XmlElement Isport_GetSportResultDetail(string mschId, string code, string key)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new AppCode_LiveScore().CommandScoreResult(mschId).CreateReader());
            //string json = AppCode_XmlToJson.XmlToJSON(xmlDoc);
            //return json;
            return  xmlDoc.DocumentElement;
            //return new AppCode_LiveScore().CommandLiveScoreResult(mschId);
        }

        [WebMethod(Description = " Get Service Subscribe ")]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void Isport_GetSubscribeService(string optCode, string code, string type, string key,string appname)
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            if (CheckKeyAccess(key))
            {
                xmlDoc.Load(new AppCode_Subscribe().CommandGetSubscribeService(optCode, appname).CreateReader());
            }
            else
            {
                SetErrorKeyAccess();
                xmlDoc.Load(rtnXML.CreateReader());
            }
            new AppCode_Logs().Logs_Insert("SportPhone_GetServiceSub", optCode, "", type, code, appname, "", "", "", "", "", AppCode_Base.GETIP(), "");
            Context.Response.ContentType = "application/json";
            Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //ContentType = "application/json" ResponseEncoding = "UTF-8"
            Context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(xmlDoc, Newtonsoft.Json.Formatting.Indented));//xmlDoc.DocumentElement;

        }

        [WebMethod(Description = " Get Header Footer ")]
        public XmlElement Isport_GetFooter(string lang,string code, string type)
        {
            new AppCode_Logs().Logs_Insert("SportPhone_GetFooter", "", "", type, code, AppCode_Base.AppName.SportPhone.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            XmlDocument xmlDoc = new XmlDocument();
            XDocument xml = new XDocument(new XElement("SportApp") );
           xmlDoc.Load(new AppCode_Utility().CommandGetHeaderandFooter(xml, lang, type).CreateReader());
           WebLibrary.ExceptionManager.WriteError("Isport_GetTabMenu  Logs Get mobile number : " + code);
            return xmlDoc.DocumentElement;
        }

        [WebMethod(Description = " Get Tab ")]
        public XmlElement Isport_GetTabMenu(string sportType,string lang, string code, string type)
        {
            new AppCode_Logs().Logs_Insert("SportPhone_GetMenu", "", "", type, code, AppCode_Base.AppName.SportPhone.ToString(), "", "", "", "", "", AppCode_Base.GETIP(),"");
            XmlDocument xmlDoc = new XmlDocument();
            XDocument xml = new XDocument(new XElement("SportApp"));
            xmlDoc.Load(new AppCode_Utility().CommandGetTab(xml,sportType, lang, type).CreateReader());
            

            return xmlDoc.DocumentElement;
        }

    }
}
