using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobileLibrary;
namespace isport_siamdara
{
    public partial class msdn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            MobileUtilities mU = Utilities.getMISDN(Request);
            Response.Write(mU.mobileOPT + " " + mU.mobileNumber);
            
            string[] allCookies = Request.Cookies.AllKeys;
            foreach (string cookie in allCookies)
            {
                //s += "|" + cookie;
                Response.Write(cookie + "value : " + Request.Cookies[cookie].Value + "<br/>");
            }
            string[] allServer = Request.ServerVariables.AllKeys;
            foreach (string all in allServer)
            {

                Response.Write(all + " value : " + Request.ServerVariables[all] + "<br/>");
            } 
            
        }
    }
}
