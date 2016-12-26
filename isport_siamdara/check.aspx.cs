using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace isport_siamdara
{
    public partial class check : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(Request.UserAgent +"  " + Request.Browser.JavaScript + " " + Request.Browser.MobileDeviceModel + " " + Request.Browser.Platform + " " + Request.Browser.ScreenPixelsWidth);
        }
    }
}
