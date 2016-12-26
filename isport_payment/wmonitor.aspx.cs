using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebLibrary;
namespace isport_payment
{
    public partial class wmonitor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // เป็นหน้า monitor ของพี่โก๋
                int ss = AppCode.CheckSessionPayment("66818593778", "01", "142", "0", "02");
                Response.Write("payment session :" + ss);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("wMonitor Error >>" + ex.Message);   
                Response.Write(ex.Message);
            }
        }
    }
}
