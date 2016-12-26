using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using MobileLibrary;
using WebLibrary;
namespace webIsport
{
    public partial class check : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MobileUtilities mU = Utilities.getMISDN(Request);

                if (Request["r"] != null && Request["r"] != "")
                {
                    string url = Request["r"].ToString().Replace('|', '&');
                    Response.Redirect(url, false);
                }
                else if (mU.mobileNumber == "")
                {
                    Response.Redirect( ConfigurationManager.AppSettings["RootWeb"] + Request.QueryString,false );
                }
                else
                {
                    Response.Redirect(ConfigurationManager.AppSettings["RootWap"] + Request.QueryString, false);
                }
            }
                else return ;
        }
    }
}