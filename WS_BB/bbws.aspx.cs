using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using WebLibrary;
namespace WS_BB
{
    public partial class bbws : System.Web.UI.Page
    {
        XDocument rtnXML = new XDocument(new XElement("my",
                             new XAttribute("header", "")
                             , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))));
        XmlDocument xmlDoc = new XmlDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack && Request["groupid"] != null)
            {
                rtnXML = XDocument.Parse(new push().SendGet("http://203.149.62.180/isportws/mb/group"+Request["groupid"]+".xml"));
            }
            else
            {
                if (Request["p"] != null && Request["p"] == "ll")
                {
                    rtnXML = GenLotto();
                }
                else if (Request["p"] != null && Request["p"] == "r")
                {
                    rtnXML = GenLottoNext();
                }
                else if (Request["p"] != null && Request["p"] == "live")
                {
                    //rtnXML = GenLiveScore();
                    //ExceptionManager.WriteError("bbws live");
                }
                else if (Request["p"] != null && Request["p"] == "livescore")
                {
                    rtnXML = GenLiveScore();
                }
                else rtnXML = XDocument.Parse(new push().SendGet("http://203.149.62.180/isportws/mb/group1.xml"));
            }

            xmlDoc.Load(rtnXML.CreateReader());
            Response.Write(xmlDoc.InnerXml);
        }

        private XDocument GenLotto()
        {

            string fileName = DateTime.Now.Date.ToString("yyyyMM");
                fileName += DateTime.Now.Date.Day < 16 ? "01" : "16";
            return    XDocument.Parse(new push().SendGet("http://203.149.62.180/isportws/mb/"+fileName+".xml"));
        }
        private XDocument GenLottoNext()
        {

            string fileName = "";
            fileName += DateTime.Now.Date.Day < 17 ? DateTime.Now.Date.ToString("yyyyMM") + "16" : DateTime.Now.AddMonths(1).ToString("yyyyMM") + "01" ;
            return XDocument.Parse(new push().SendGet("http://203.149.62.180/isportws/mb/R" + fileName + ".xml"));
        }

        private XDocument GenLiveScore()
        {
            rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_LiveScoreNew"].ToString()));
            return rtnXML;
        }
    }
}
