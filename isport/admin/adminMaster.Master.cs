using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace isport.admin
{
    public partial class adminMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //XmlDSTopMenu.DataFile = "../setting/MenuTopUser.xml";


                string user = WebLibrary.MitTool.GetCookie(Request, "isportwapadmin");
                string bcUser = WebLibrary.MitTool.GetCookie(Request, "admin");
                //Request.Cookies.Get("").
                if ((bcUser == null || bcUser == ""))
                {
                    if ((user == null || user == ""))
                    {
                        Response.Redirect("login.aspx", false);
                    }
                }
                else if (bcUser != null)
                {
                    //WebLibrary.MitTool.CreateCookie(
                }
            }
        }
    }
}
