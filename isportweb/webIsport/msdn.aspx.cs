using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobileLibrary;
namespace webIsport
{
    public partial class msdn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MobileUtilities mu = Utilities.getMISDN(Request);
            Response.Write(mu.mobileOPT + "|" + mu.mobileNumber);
        }
    }
}