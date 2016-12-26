using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
namespace isport_jza
{
    public partial class master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if( !IsPostBack )
            {
                if( DateTime.Now.Hour < 6 )
                {
                    lnkBanner.ImageUrl = "images/banner.gif";
                    lnkBanner.NavigateUrl = ConfigurationManager.AppSettings["URLSubscribeJza"];
                }
                else
                {
                    lnkBanner.ImageUrl = "images/banner1.gif";
                    lnkBanner.NavigateUrl = ConfigurationManager.AppSettings["URLPopUpBanner"];
                }
            }
        }
    }
}