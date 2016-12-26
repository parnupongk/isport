using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mobile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WebLibrary;
namespace isport
{
    public partial class reqcheck : System.Web.UI.MobileControls.MobilePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write(new AppCode_CheckActive().DtacCheckactive("8451110500001", "66851446178", "14200001", "imo"));
            string vStrName = Request.ServerVariables["HTTP_USER_AGENT"];
            Response.Write(vStrName.ToLower().IndexOf("opera"));

        }
    }
}