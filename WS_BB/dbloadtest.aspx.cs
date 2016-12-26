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

namespace WS_BB
{
    public partial class dbloadtest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime scoreDate = new DateTime();
                //TimeSpan diffDate = new DateTime(int.Parse(scoreDate.Substring(0,4) ), int.Parse(scoreDate.Substring(4,2)), int.Parse(scoreDate.Substring(6,2))) - DateTime.Now;

                //TimeSpan diff24 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 00, 00);
                //TimeSpan diff18 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 00, 00);
                //decimal addTime = (diff18.Hours <= 0 && diff24.Hours >= 0) ? 0 : +1; // เวลาปัจจุบันเลย 18.00 ถ้าเลย Day + 1
                string str = "";
                XmlDocument xmlDoc = new XmlDocument();
                XDocument rtnXML = new XDocument(new XElement("SportApp",
                             new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                             , new XAttribute("date", "")
                             ));


                //if (Request["db"] == "1")
                //    // DB Isport
                //    rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, "", "th", "", AppCode_LiveScore.MatchType.Finished, 0, "N");
                //else
                    // DB Sport CC
                    //rtnXML.Element("SportApp").Add( new isportapp().Isport_GetScore("", "", "", "th", "", ""));

                //xmlDoc.Load(rtnXML.CreateReader());
                str = new isportapp().Isport_GetScore("", "", "", "th", "", "").ToString();
                Response.Write(str);
            }
        }
    }
}
