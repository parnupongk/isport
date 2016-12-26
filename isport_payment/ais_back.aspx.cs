using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace isport_payment
{
    public partial class ais_back : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("http://wap.isport.co.th/isportui/index.aspx?p=bb&" + Request.QueryString);
        }
    }
}
