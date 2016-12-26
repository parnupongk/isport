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
using MobileLibrary;
using WebLibrary;
namespace isport_payment
{
    public partial class pay_dtac_cancel : mBasePage
    {
        public override void SetHeader()
        {
            
        }
        public override void SetContent()
        {
            string urlRedirect = ConfigurationManager.AppSettings["isportRoot"].ToString() + ConfigurationManager.AppSettings["defaultURLRedirect"].ToString();
            MobileUtilities mU = Utilities.getMISDN(Request);
            try
            {
                
                
                LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(),"Dtac_AOC_Cancel", mU.mobileNumber, " status:" + Request["stts"].ToString() + " ,cmd=" + Request["cmd"].ToString());
                // Send State to Crie
                string strContent = Request["cid"].ToString() == "" ? Request["cprefid"].ToString() : Request["cid"].ToString(); // 4511000010:S:L:142:0000:1:0:1
                bool isSubscribe = Request["cid"].ToString() == "" ? true : false;
                string strServiceId = "";
                if (strContent != "")
                {
                    string[] strContents = strContent.Split(':');
                    strServiceId = strContents[0];
                    bProperty_SIZE = strContents[1];
                    bProperty_LNG = strContents[2];
                    bProperty_SGID = strContents[3];
                    bProperty_MP = strContents[4];
                    bProperty_PRJ = strContents[5];
                    bProperty_SCSID = strContents[6];
                }
                strServiceId = isSubscribe ? (strServiceId == "1" ? "8451120500001" : "8451110500001") : strServiceId; 
                AppCode.SendStatetoCrie(strServiceId, strContent, mU.mobileNumber, "0");
                
            }
            catch(Exception ex)
            {
                LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(),"Dtac_AOC_Error", mU.mobileNumber+"Error status: " + ex.Message,"" );
            }
            Response.Redirect(urlRedirect + "lng=" + bProperty_LNG + "&size=" + bProperty_SIZE + "&mp=" + bProperty_MP
                    + "&prj=" + bProperty_PRJ + "&sg=" + bProperty_SGID, false);
        }
        public override void SetFooter()
        {
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}