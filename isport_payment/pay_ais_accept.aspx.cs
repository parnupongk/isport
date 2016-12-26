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
using System.Configuration;
using WebLibrary;
namespace isport_payment
{
    public partial class pay_ais_accept : mBasePage
    {
        /*=================================================================
         * 
         * 
         *         เปลี่ยนไปใช้ API ใหม่หน้านี้ไม่ใช่แล้วนะครับ
         * 
         * 
         * =================================================================*/
        public override void SetHeader()
        {
            
        }
        public override void SetContent()
        {

            lblMessage.Text = "";
            string tID = Request["tID"];
            string sad = Request["sad"];
            string sn = Request["SN"];
            string dad = Request["dad"];
            string sID = Request["sID"];
            string cpSessionID = Request["CPsessionID"];
            try
            {
                if (cpSessionID != null && cpSessionID != "")
                {
                    string url = "?cmd=dl&spName=511&dad=" + dad + "&tID=" + tID + "&SN=" + sn + "&sID=" + sID
                        + "&cURL=" + ConfigurationManager.AppSettings["aisRedirectPathURL"]
                        + "pay_ais_redirect.aspx?CPsessionID=" + cpSessionID + "&cct=10";

                    string logName = sn == "4511115" ? "AisSubScribeCDG_AcceptSession" : "AisSessionCDG_AcceptSession";
                    LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), logName, "Request URL :" + Request.QueryString, "Response URL :" + url);

                    Response.Redirect(ConfigurationManager.AppSettings["aisSubScribeURL"] + url, false);
                }
                else Response.Redirect(ConfigurationManager.AppSettings["isportRoot"], false);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("sn=" + sn + ",sad=" + sad + ",tID=" + tID + ",dad=" + dad + ",sID=" + sID
                    + " Error Message:" + ex.Message);
                lblMessage.Text = ConfigurationManager.AppSettings["subScribeErrorMessage"];
            }

        }
        public override void SetFooter()
        {
            Setfooter(pnlFooter);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}