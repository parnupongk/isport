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
    public partial class pay_ais_session : mBasePage
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
            string cpSessionID = Request["CPsessionID"] ;
            try
            {
                if (cpSessionID != null && cpSessionID != "")
                {
                    cpSessionID += ":" + sn;
                    MobileLibrary.MobileUtilities mU = MobileLibrary.Utilities.getMISDN(Request);
                    if (AppCode.CheckSessionPayment(mU.mobileNumber, mU.mobileOPT, bProperty_SGID, bProperty_SCSID
                        , sn == "4511115" ? AppCode.GetPTypeCode.pTypeCode_50Bath : AppCode.GetPTypeCode.pTypeCode_5Bath) == 1)
                    {
                        lblMessage.Text = ConfigurationManager.AppSettings["subScribeDupMessage"];
                    }
                    else
                    {
                        string url = "?cmd=session&tID=" + tID + "&sad=" + sad + "&SN=" + sn
                            + "&spName=511&cURL=" + ConfigurationManager.AppSettings["aisRedirectPathURL"]
                            + "pay_ais_accept.aspx?CPsessionID=" + cpSessionID;

                        string logName = sn == "4511115" ? "AisSubScribeCDG_CreateSession" : "AisSessionCDG_CreateSession";
                        LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), logName, "Request URL :" + Request.QueryString, "Response URL :" + url);

                        Response.Redirect(ConfigurationManager.AppSettings["aisSubScribeURL"] + url, false);
                    }
                }
                else Response.Redirect(ConfigurationManager.AppSettings["isportRoot"], false);

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("sn=" + sn + ",sad=" + sad + ",tID=" + tID + " Error Message:" + ex.Message);
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