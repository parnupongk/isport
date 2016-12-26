using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WS_BB
{
    public partial class feedais1 : feedMain
    {
        public override string CheckPageName(string pageName,string appName)
        {
            string rtn = "";
            string contentGroupid = "";
            if (pageName == "getprogram")
            {
                rtn = GetProgram(appName, "", "", "00001", Request["lang"], Request["type"]);
            }
            else if (pageName == "getscore")
            {
                rtn = GetResult(appName, "", Request["date"], "00001", Request["lang"], false);
            }
            else if (pageName == "getnews")
            {
                rtn = GetNewsByContentGroup(appName, "", Request["row"], Request["lang"], muMobile.mobileNumber, Request["type"]);
            }
            else if (pageName == "getscoredetail")
            {
                rtn = GetScoreDetail(appName, Request["matchid"], Request["mschid"], Request["lang"]);
            }
            else if (pageName == "getleaguetable")
            {
                rtn = GetLeagueTable(appName, "807", "00001", Request["leag"]);
            }
            return rtn;
        }

        //private string 

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}
