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
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;
using MobileLibrary;
namespace isport_payment
{
    public partial class pay_ais_redirect : mBasePage
    {
        public override void SetHeader()
        {
            
        }
        public override void SetContent()
        {
            string strSn = "";
            string strAppId = Request["appID"];
            string strCPsessionID = Request["CPsessionID"];
            int subScribeStatus = -1;
            MobileUtilities mU = Utilities.getMISDN(Request);
            try
            {

                    #region redirect sub and session
                    string[] strcpSessionIDs = strCPsessionID.Split(':');
                    mU.mobileNumber = mU.mobileNumber == "" ? Request["msisdn"] : mU.mobileNumber;
                    if (strcpSessionIDs.Length > 0)
                    {
                        //bProperty_SIZE = strcpSessionIDs[0];
                        //bProperty_LNG = strcpSessionIDs[1];
                        bProperty_SGID = strcpSessionIDs[0];
                        bProperty_MP = strcpSessionIDs[1];
                        bProperty_PRJ = strcpSessionIDs[2];
                        bProperty_SCSID = strcpSessionIDs[3];
                        strSn = strcpSessionIDs[4];


                        bProperty_SCSID = bProperty_SCSID == "" ? "0" : bProperty_SCSID;
                        if (Request["status"] != null && Request["status"].ToString() == "success")
                        {
                            if (strAppId == "12993511" || strAppId == "27155511" || strSn == "4511115")
                            {
                                // Subscribe
                                #region Subscribe
                                int ss = AppCode.CheckSessionPayment(mU.mobileNumber, mU.mobileOPT, bProperty_SGID
                                    , bProperty_SCSID, AppCode.GetPTypeCode.pTypeCode_50Bath);
                                if (ss == 0)
                                {
                                    strAppId = (strAppId == null || strAppId == "") ? strSn : strAppId;
                                    subScribeStatus = SubScribeInsert(mU, AppCode.GetPTypeCode.pTypeCode_50Bath, strAppId);
                                    try
                                    {
                                        string strRequestHTML = AIS_Send_Regis(mU.mobileOPT, mU.mobileNumber);
                                        LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "AisSubScribeCDG_Regis_SMS", "msisdn:" + mU.mobileNumber + "status =" + strRequestHTML, "");
                                    }
                                    catch (Exception ex)
                                    {
                                        ExceptionManager.WriteError("AIS Regis Error : " + ex.Message);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                // Session
                                #region Session
                                int ss = AppCode.CheckSessionPayment(mU.mobileNumber, mU.mobileOPT, bProperty_SGID
                                    , bProperty_SCSID, AppCode.GetPTypeCode.pTypeCode_5Bath);

                                // string checkActive =  new isport.AppCode_CheckActive().CheckActive(mU.mobileNumber, mU.mobileOPT, bProperty_SGID, bProperty_MP, bProperty_PRJ);
                                //LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "AisSubscribe_AcceptRedirect_test", "msisdn:" + mU.mobileNumber + " Request URL:" + ss, "");
                                if (ss == 0)
                                {
                                    strSn = (bProperty_SGID == "108" || bProperty_SGID == "125" || bProperty_SGID == "208") ? "00683511" : "00685511";
                                    subScribeStatus = SubScribeInsert(mU, AppCode.GetPTypeCode.pTypeCode_5Bath, strSn);
                                    if (bProperty_SGID == "95" || bProperty_SGID == "208" || bProperty_SGID == "255")
                                    {
                                        ContentUsageLogsInsert(mU);
                                    }
                                }
                                #endregion
                            }



                            InsertRedirectLogs(mU);
                            string urlRedirect = (bProperty_SGID == "255") ? ConfigurationManager.AppSettings["tdedloveRoot"] : ConfigurationManager.AppSettings["isportRoot"];
                            if (bProperty_SGID == "256" || bProperty_SGID == "257") urlRedirect = ConfigurationManager.AppSettings["isportUIRoot"].ToString();
                            urlRedirect += AppCode.ServiceGroup_GetURLRedirect(bProperty_SGID, bProperty_SCSID);
                            Response.Redirect(urlRedirect + "lng=" + bProperty_LNG + "&size=" + bProperty_SIZE + "&mp=" + bProperty_MP
                                + "&prj=" + bProperty_PRJ + "&sg=" + bProperty_SGID, false);

                            //AIS_Send_Notify(strSn);

                        }// status success
                        else
                        {
                            LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "AisPayment_Error", mU.mobileNumber + " Query:" + Request.QueryString.ToString(), "");
                            Response.Redirect("http://wap.isport.co.th/isportui/check.aspx?p=aiserr_payment&mess=" + Request["reason"], false);
                        }
                    } // can not get CPsessionID
                    else
                    {
                        LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "AisPayment_Error", mU.mobileNumber + " Query:can not get CPsessionID", "");
                        Response.Redirect("http://wap.isport.co.th/isportui/check.aspx?p=aiserr_payment&mess=can not get CPsessionID", false);
                    }
                    #endregion

            }
            catch (Exception ex)
            {
                LogsManager.WriteLogs( "AisSubscribe_Error", mU.mobileNumber + " Error Subscribe status:" + subScribeStatus + " AppId=" + strAppId + " Error:" + ex.Message, "");
                lblMessage.Text = subScribeStatus + ConfigurationManager.AppSettings["subScribeSystemErrorMessage"].ToString();
            }
        
        }
        public override void SetFooter()
        {
            Setfooter(pnlFooter);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) return;
        }

        #region Insert Data
        private string AIS_Send_Regis(string opt,string msisdn)
        {
            try
            {
                string strRequestHTML = "";
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["IsportPackConnectionString"].ToString(), CommandType.Text, "select * from dbo.ufn_chk_dupsubc('" + opt + "','*451105023','" + msisdn + "','" + DateTime.Now + "')");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["dupsubc_flag"].ToString() =="N")
                {
                    System.Net.WebClient v = new System.Net.WebClient();
                    Byte[] byteRquestHTML = v.DownloadData("http://203.149.62.180/cgi/sms/ais/infopack/cdg/cdg_hybrid.aspx?service=468&command=1&msisdn=" + msisdn + "&telco=1&tel=0025");
                    strRequestHTML = System.Text.UTF8Encoding.UTF8.GetString(byteRquestHTML);
                }
                return strRequestHTML;
            }
            catch (Exception ex)
            {
                throw new Exception("AIS_Send_Regis>>" + ex.Message);
            }
        }

        private void AIS_Send_Notify(string strSn)
        {
            try
            {
                System.Net.WebClient v = new System.Net.WebClient();
                Byte[] byteRquestHTML = v.DownloadData(ConfigurationManager.AppSettings["aisSubScribeURL"] + "?cmd=notify&SN=" + strSn + "&spName=511&tID=" + Request["tid"] + "&status=success&reason=complete_download");
                System.Text.UTF8Encoding.UTF8.GetString(byteRquestHTML);
            }
            catch(Exception ex) {
                LogsManager.WriteLogs("AisSubscribe_Error_Noti", " Error:" + ex.Message, "");
            }
        }

        private void InsertRedirectLogs(MobileUtilities mU)
        {
            try
            {
                LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "AisSubscribe_AcceptRedirect", "msisdn:" + mU.mobileNumber + " Request URL:" + Request.QueryString.ToString(), "");
                UsageSubscribeProtalLogs_Insert(mU);
            }
            catch (Exception ex)
            {
                LogsManager.WriteLogs("AisSubscribe_Error", "InsertRedirectLogs>> " + ex.Message, "");
            }
        }
        private int SubScribeInsert(MobileUtilities mU, string pTypeCode, string strAppId)
        {
            string str = "";
            try
            {
                int subScribestatus = AppCode.SubscribeInsert(mU.mobileNumber, pTypeCode, strAppId, mU.mobileOPT
                                    , bProperty_SGID, bProperty_MP, bProperty_PRJ, "", bProperty_SCSID);

                PaymentLogs_Insert(mU, pTypeCode);
                if (pTypeCode == AppCode.GetPTypeCode.pTypeCode_50Bath.ToString())
                {
                    str = "msisdn:" + mU.mobileNumber + "status =" + subScribestatus + " appId=" + strAppId + " sgId=" + bProperty_SGID + " Query:" + Request.QueryString.ToString();
                    LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "AisSubScribeCDG_InsertStatus",str , "" );
                }
                else
                {
                    str = "msisdn:" + mU.mobileNumber + "status =" + subScribestatus + " appId=" + strAppId + " sgId=" + bProperty_SGID + " Query:" + Request.QueryString.ToString();
                    LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "AisSessionCDG_InsertStatus",str , "");
                }
                
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
                dr.SG_ID = bProperty_SGID == "" ? 0 :int.Parse(bProperty_SGID);
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
    }
}