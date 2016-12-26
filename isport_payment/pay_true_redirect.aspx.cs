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
using MobileLibrary;

namespace isport_payment
{
    public partial class pay_true_redirect : mBasePage
    {
        public override void SetHeader()
        {
            
        }
        public override void SetContent()
        {
            
        }
        public override void SetFooter()
        {
            try
            {
                LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(),"TrueSession_Accept", Request.QueryString.ToString(), "");
                string serviceID = Request["SERVICE_ID"];
                if (serviceID != null && serviceID != "")
                {
                    MobileUtilities mU = Utilities.getMISDN(Request);
                    UsageSubscribeProtalLogs_Insert(mU);
                    SubScribeInsert(mU, AppCode.GetPTypeCode.pTypeCode_5Bath, serviceID);
                    PaymentLogs_Insert(mU, AppCode.GetPTypeCode.pTypeCode_5Bath);
                    if (bProperty_SGID == "95" || bProperty_SGID == "208")
                    {
                        ContentUsageLogsInsert(mU);
                    }
                }
                //else Response.Redirect(ConfigurationManager.AppSettings["isportRoot"],false);

                string urlRedirect = ConfigurationManager.AppSettings["isportRoot"] + AppCode.ServiceGroup_GetURLRedirect(bProperty_SGID, bProperty_SCSID);
                Response.Redirect(urlRedirect + "lng=" + bProperty_LNG + "&size=" + bProperty_SIZE + "&mp=" + bProperty_MP
                    + "&prj=" + bProperty_PRJ + "&sg=" + bProperty_SGID, false);

            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }
        }

        #region Insert Data

        private int SubScribeInsert(MobileUtilities mU, string pTypeCode, string strAppId)
        {
            try
            {
                int subScribestatus = AppCode.SubscribeInsert(mU.mobileNumber, pTypeCode, strAppId, mU.mobileOPT
                                    , bProperty_SGID, bProperty_MP, bProperty_PRJ, "", bProperty_SCSID); // status 0=success , 1=not success
                LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(),"TrueInsert_SubscribeStatus", mU.mobileNumber, "status =" + subScribestatus);
                PaymentLogs_Insert(mU, pTypeCode);
                return subScribestatus;
            }
            catch (Exception ex)
            {
                throw new Exception("SubScribeInsert>>" + ex.Message);
            }
        }

        private void ContentUsageLogsInsert(MobileUtilities mU)
        {
            try
            {
                paymentDS1.I_CONTENT_USAGE_LOGRow dr = new paymentDS1().I_CONTENT_USAGE_LOG.NewI_CONTENT_USAGE_LOGRow();
                dr.SG_ID = int.Parse(bProperty_SGID);
                dr.CCAT_ID = 9;
                dr.PHONE_NO = mU.mobileNumber;
                dr.ACCESS_CHANNEL = "W";
                dr.CONTENT_ID = bProperty_SCSID;
                dr.USAGE_TYPE = "V";
                dr.USAGE_DATE = DateTime.Now;
                dr.REF_ID = "";
                AppCode.ContentUsageLogsInsert(dr);
            }
            catch (Exception ex)
            {
                throw new Exception(" ContentUsageLogsInsert>> " + ex.Message);
            }
        }
        private void PaymentLogs_Insert(MobileUtilities mU, string pTypeCode)
        {
            try
            {
                // Insert Log payment
                paymentDS1.I_PAYMENT_LOGRow drLogs = new paymentDS1().I_PAYMENT_LOG.NewI_PAYMENT_LOGRow();
                drLogs.OPT_CODE = mU.mobileOPT;
                drLogs.ACCESS_CHANNEL = "W";
                drLogs.PHONE_NO = mU.mobileNumber;
                drLogs.PTYPE_CODE = pTypeCode; //"01";
                drLogs.PAY_STATUS = "0";
                drLogs.PAY_STATUS_DETAIL = "<!--Successful-->";
                drLogs.PAY_DATE = DateTime.Now;
                AppCode.PaymentLogsInsert(drLogs);
            }
            catch (Exception ex)
            {
                throw new Exception("PaymentLogs_Insert>>" + ex.Message);
            }
        }
        private void UsageSubscribeProtalLogs_Insert(MobileUtilities mU)
        {
            try
            {
                paymentDS1.I_SUBSCRIBE_PORTAL_LOGRow dr = new paymentDS1().I_SUBSCRIBE_PORTAL_LOG.NewI_SUBSCRIBE_PORTAL_LOGRow();
                dr.PHONE_NO = mU.mobileNumber;
                dr.USAGE_DATE = DateTime.Now;
                dr.USAGE_AGENT = bProperty_USERAGENT;
                dr.SG_ID = int.Parse(bProperty_SGID);
                dr.OPT_CODE = mU.mobileOPT;
                dr.PRJ_ID = int.Parse(bProperty_PRJ);
                dr.MP_CODE = bProperty_MP;
                dr.SCS_ID = int.Parse(bProperty_SCSID == "" ? "0" : bProperty_SCSID);
                dr.USAGE_DATE_TXT = DateTime.Now.ToString("yyyyMMdd");
                AppCode.UsageSubscribeLogs_Insert(dr);
            }
            catch (Exception ex)
            {
                throw new Exception("UsageSubscribeProtalLogs_Insert>>" + ex.Message);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}