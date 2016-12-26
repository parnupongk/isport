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
    /// Summary description for feeddtac
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class feeddtac : System.Web.Services.WebService
    {
        [WebMethod(Description = "Get ข่าว (By contestGroupId or countryId)")]
        public XmlElement Isport_GetSportNews(string sportType,string contestGroupId, string rowCount, string lang, string msisdn, string type)
        {
            // contestGroupId => ข่าว by league
            // countryId => ข่าว by ประเทศ

            rowCount = rowCount == "" ? "20" : rowCount;
            XmlDocument xmlDoc = new XmlDocument();
            XDocument rtnXML = new XDocument(new XElement("channel",
                new XAttribute("title", "ข่าวสยามกีฬา")
                , new XAttribute("description", "ข่าวสยามกีฬา")
                , new XAttribute("language","th")
                //,new XAttribute("link","http://wap.isport.co.th/?s=feeddtac")
                , new XAttribute("link", "http://wap.isport.co.th/isportui/indexl.aspx?p=d02SSC")
                , new XAttribute("pubDate", DateTime.Now.ToString("ddd, dd MMM yyyy HH:MM:ss") + " +0700")
                ,new XAttribute("generator","isport")
                ,new XAttribute("copyright","Isport Co., Ltd.")
                ,new XAttribute("ttl",rowCount)
                ));
            rtnXML.Element("channel").Add(new XElement("image"
                ,new XAttribute("title","ข่าวสยามกีฬา")
                //,new XAttribute("link","http://wap.isport.co.th/?s=feeddtac")
                , new XAttribute("link", "http://wap.isport.co.th/isportui/indexl.aspx?p=d02SSC")
                ,new XAttribute("url","http://wap.isport.co.th/isportws/images/feeddtac/logo.jpg")
                ,new XAttribute("width","280")
                ,new XAttribute("height","101")
                ,new XAttribute("description","ข่าวสยามกีฬาออนไลน์")
                ));
            if ((type == "mobile" && msisdn != "" && msisdn.Length == 11) || (type == "web"))
            {
                #region Get Data
                if (sportType == "00001")
                {
                    // Football Sport
                    if (contestGroupId == "2612")
                    {

                        // Football Thai
                        rtnXML = new AppCode_News().Command_GetNewsFootballThai_FeedDtac(rtnXML, contestGroupId, int.Parse(sportType).ToString(), rowCount, lang, "", type);

                    }
                    else
                    {

                        // Football Inter
                        rtnXML = new AppCode_News().Command_GetNews_FeedDtac(rtnXML, contestGroupId, sportType, rowCount, lang, "", type);


                    }

                }
                else
                {

                    // Other Sport
                    // ไม่ส่ง contentGroupId ไปเพราะว่ากีฬาอื่นๆ siamsport ไม่ได้แยกย่อยลงไป 
                    rtnXML = new AppCode_News().Command_GetNewsBySportType_FeedDtac(rtnXML, sportType, rowCount, lang, type);

                }
                #endregion
            }
            else
            {
                rtnXML.Element("channel").Add(new XElement("status", "error")
                       , new XElement("message", "insert msisdn!!"));
            }
            xmlDoc.Load(rtnXML.CreateReader());
            // Insert Logs
            new AppCode_Logs().Logs_Insert("FeedDtac_GetNews", "", "", type, msisdn,AppCode_Base.AppName.FeedDtac.ToString(),"","","","","",AppCode_Base.GETIP(),"");
            return xmlDoc.DocumentElement;
        }

    }
}
