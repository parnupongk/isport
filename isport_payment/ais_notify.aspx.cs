using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using WebLibrary;
namespace isport_payment
{
    public partial class ais_notify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["cmd"] != null && Request["cmd"] != "" && Request["cmd"] == "token")
            {
                try
                {
                    AppCode.TokenLogsInsert(Request["token"], Request["cmd"], Request["sid"], Request["sn"], Request["spsid"], Request["msisdn"], Request.ServerVariables["REMOTE_ADDR"]);
                }
                catch (Exception ex) { ExceptionManager.WriteError(ex.Message); }

                //Response.ContentType = "text/xml";
                //string status = File.ReadAllText(Server.MapPath("wap_success.xml"));
                Response.Write("success");
                //Response.Redirect("wap_success.xml", false);
            }
        }
    }
}
