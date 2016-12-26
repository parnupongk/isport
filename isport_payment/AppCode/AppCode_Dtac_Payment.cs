using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using MobileLibrary;
using WebLibrary;
using OracleDataAccress;
namespace isport_payment
{
    public class DtacPayment_Parameter
    {
        public string dtacStatus;
        public string dtacUrlRedirect;
        public string dtacContent;
        public string dtacDescription;
        public string dtacContentId;
        public string dtacCPRefId;
    }
    public class AppCode_Dtac_Payment
    {

        
        public string CreateTransaction_Dtac
        {
            get{
                return "4" + DateTime.Now.ToString("yyMMddhhmmss") + "894" + DateTime.Now.ToString("fff");
            }
        }

        public string CreateTransaction_Dtac_test
        {
            get
            {
                return "4" + DateTime.Now.ToString("yyMMddhhmm")  + DateTime.Now.ToString("fff");
            }
        }

        #region Dtac New Service
        /// <summary>
        /// Accept Sub Session(5,10) and sub Subscribe(50)
        /// </summary>
        /// <param name="strSessionId"></param>
        /// <param name="strTicket"></param>
        /// <param name="strCprefid"></param>
        /// <param name="strCmd"></param>
        /// <param name="strPhone"></param>
        /// <param name="typeCode"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        public DtacPayment_Parameter SubAcceptAOC(string strSessionId,string strTicket,string strCprefid,string strCmd
            ,string strPhone,string typeCode,string opt)
        {
            try
            {
                DtacPayment_Parameter para = new DtacPayment_Parameter();

                #region command
                string command = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "utf-8" + (char)34 + "?>";
                command += "<cpa-request-aoc>";
                command += "<authentication>";
                //command += "<user>TrOps</user>";
                //command += "<password>TqPZSp894</password>";
                command += "<user>Isport511</user>";
                command += "<password>Y466FJXit</password>";
                command += "</authentication>";
                command += "<delivery-confirm>";
                command += "<session>" + strSessionId + "</session>";
                command += "<ticket>" + strTicket + "</ticket>";
                command += "<cp-ref-id>" + strCprefid + "</cp-ref-id>";
                command += "<command>" + strCmd + "</command>";
                command += "<msisdn>" + strPhone + "</msisdn>";
                command += "<timestamp>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</timestamp>";
                command += "</delivery-confirm>";
                command += "</cpa-request-aoc>";
                #endregion

                string rtn = new sendpost().SendPost(ConfigurationManager.AppSettings["dtacAOCSubScribeServiceURL"], command);
                //string rtn = new sendpost().SendPost("http://202.91.21.248:8319/SAG/services/cpa/aoc/deliveryConfirm", command);
            
                string fileName = typeCode == "01" ? "Dtac_AOC_Confirm_Subscribe" : "Dtac_AOC_Confirm_Session";
                LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), fileName, "mobile:" + strPhone + " return xml:"+rtn, "");
                //LogsManager.WriteLogs("Test_SDP", rtn, command);

                para.dtacStatus = Utilities.GetDataXML("cpa-response-aoc", rtn, "status");
                para.dtacDescription = Utilities.GetDataXML("cpa-response-aoc", rtn, "status-desc");

                paymentDS1.I_PAYMENT_LOGRow drLogs = new paymentDS1().I_PAYMENT_LOG.NewI_PAYMENT_LOGRow();
                drLogs.OPT_CODE = opt;
                drLogs.ACCESS_CHANNEL = "W";
                drLogs.PHONE_NO = strPhone;
                drLogs.PTYPE_CODE = typeCode; //"01";
                drLogs.PAY_STATUS = para.dtacStatus;
                drLogs.PAY_STATUS_DETAIL = para.dtacDescription;
                drLogs.PAY_DATE = DateTime.Now;
                AppCode.PaymentLogsInsert(drLogs);

                return para;
            }
            catch (Exception ex)
            {
                throw new Exception("SubAcceptAOC >> " + ex.Message);
            }
        }

        public DataTable PaymentDtacSDPSelect_Parameter(string id)
        {
            try
            {

                DataSet ds = OrclHelper.Fill(AppCode.GetOracleConnectionString, CommandType.StoredProcedure, "WAP_PAYMENT.PaymentDtacSdpSelect", "I_PAYMENT_DTACSDP"
                    , new OracleParameter[] {OrclHelper.GetOracleParameter("P_ID",id,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("o_cursor","",OracleType.Cursor,ParameterDirection.Output)
                    });
                return ds.Tables.Count > 0 ? ds.Tables[0] : null ;
            }
            catch (Exception ex)
            {
                throw new Exception("PaymentDtacSDPSelect_Parameter>> " + ex.Message);
            }
        }
        public String PaymentDtacSDPInsert_Parameter(string id,string mobile,string sgId,string lng,string mpCode,string prj,string pcntId,string serviceId)
        {
            try
            {

                return OrclHelper.ExecuteNonQuery(AppCode.GetOracleConnectionString, CommandType.StoredProcedure, "WAP_PAYMENT.PaymentDtacSdpInsert"
                    , new OracleParameter[] {OrclHelper.GetOracleParameter("P_ID",id,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("P_MOBILE",mobile,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("P_SG_ID",sgId,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("P_LNG",lng,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("P_MP_CODE",mpCode,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("P_PRJ_ID",prj,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("P_PCNT_ID",pcntId,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("P_SERVICEID",serviceId,OracleType.VarChar,ParameterDirection.Input)
                    }).ToString();

            }
            catch (Exception ex)
            {
                throw new Exception("PaymentDtacSDPInsert_Parameter >> " + ex.Message);
            }
        }

        public DtacPayment_Parameter SubSession(string agent,string lng,string mp,string prj,string size
            ,string serviceID, string phone, string opt, string sgID, string scsId,string type)
        {
            try
            {
                string strSecond = DateTime.Now.Second.ToString().PadLeft(2, '0');
//                string strContentId = type +":"+size + ":" + lng + ":" + sgID + ":" + mp + ":" + prj + ":" + scsId + ":" + serviceID + ":" + strSecond;
                string strContentId =  size + ":" + lng + ":" + sgID + ":" + mp + ":" + prj + ":" + scsId + ":" + "1" + ":" + strSecond;
                DtacPayment_Parameter para = new DtacPayment_Parameter();

                #region command
                string command = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "utf-8" + (char)34 + "?>";
                command += "<cpa-request-aoc>";
                command += "<authentication>";
                command += "<user>TrOps</user>";
                command += "<password>TqPZSp894</password>";
                command += "</authentication>";
                command += "<download>";
                    command += "<service>" + serviceID + "</service>";
                    command += "<msisdn>" + phone + "</msisdn>";
                    command += "<content-id>" + strContentId + "</content-id>";
                    command += "<cp-ref-id>" + CreateTransaction_Dtac + "</cp-ref-id>";
                    command += "<mobile-model>" + agent + "</mobile-model>";
                    command += "<ok-url>http://wap.isport.co.th/isport_uat/sport_center/payment/pay_dtac_redirect.aspx?</ok-url>";
                    command += "<cancel-url>http://wap.isport.co.th/sport_center/isport/index.aspx?</cancel-url>";
                    command += "<locale>th</locale>";
                    command += "<timestamp>" + DateTime.Now.ToString("yyMMddHHmm") + strSecond + "</timestamp>";
                command += "</download>";
                command += "</cpa-request-aoc>";
                #endregion

                LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(),"1CPA_command", command,"");
                string rtn = new sendpost().SendPost(ConfigurationManager.AppSettings["dtacAOCSubScribeServiceURL"], command);

                LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(),"1CPA_Dtac", rtn, "mobile:" + phone);
                para.dtacStatus = Utilities.GetDataXML("cpa-response-aoc", rtn, "status");
                para.dtacUrlRedirect = Utilities.GetDataXML("cpa-response-aoc", rtn, "redirection");
                para.dtacDescription = Utilities.GetDataXML("cpa-response-aoc", rtn, "status-desc");

                return para;
            }
            catch (Exception ex)
            {
                throw new Exception("SubSession >> " + ex.Message);
            }
        }
        #endregion

        #region Dtac Old Service

        public string Subscribe(string serviceID, string phone, string opt, string sgID, string scsId)
        {
            try
            {
                string status = "";

                #region Command UnSub
                string command = "<cpa-wap>";
                command += "<authenticate>";
                command += "<user>iSpoPW</user>";
                command += "<password>DwaP894</password>";
                command += "</authenticate>";
                command += "<unregister-subscription>";
                command += "<transaction-id>" + CreateTransaction_Dtac + "</transaction-id>";
                command += "<msisdn>" + phone + "</msisdn>  ";
                command += "<wapgw-ip>203.155.200.133</wapgw-ip>";
                command += "<service-id>" + serviceID + "</service-id>";
                command += "</unregister-subscription>";
                command += "</cpa-wap>";
                #endregion

                string IDSessionSubscribe = Guid.NewGuid().ToString();
                string rtn = new sendpost().SendPost(ConfigurationManager.AppSettings["dtacSubScribeServiceURL"], command);
                LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(),"1DtacSubscribe_Unscribe", rtn, "SessionSubscribe:" + IDSessionSubscribe);
                status = Utilities.GetDataXML("cpa-wap", rtn, "status");

                if (status == "200" || status=="658")
                {
                    #region Command Subscribe
                    command = "<cpa-wap>";
                    command += "<authenticate>";
                    command += "<user>iSpoPW</user>";
                    command += "<password>DwaP894</password>";
                    command += "</authenticate>";
                    command += "<registration-subscription>";
                    command += "<transaction-id>" + CreateTransaction_Dtac + "</transaction-id> ";
                    command += "<msisdn>" + phone + "</msisdn>";
                    command += "<begin-dttm>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</begin-dttm>";
                    command += "<expire-dttm> " + DateTime.Now.AddDays(AppCode.Package_GetValidDay("01", opt, serviceID, sgID)).ToString("yyyyMMddHHmmss") + " </expire-dttm>";
                    command += "<access-url>none</access-url>";
                    command += "<access-text>none</access-text> ";
                    command += "<wapgw-ip>203.155.200.133</wapgw-ip>";
                    command += "<service-id>" + serviceID + "</service-id>";
                    command += "<sendto>none</sendto>";
                    command += "</registration-subscription>";
                    command += "</cpa-wap>";
                    #endregion

                    rtn = new sendpost().SendPost(ConfigurationManager.AppSettings["dtacSubScribeServiceURL"], command);
                    LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(),"1DtacSubscribe", rtn, "SessionSubscribe:" + IDSessionSubscribe);
                    status = Utilities.GetDataXML("cpa-wap", rtn, "status");
                    string strText = status == "200" ? "" : Utilities.GetDataXML("cpa-wap", rtn, "text");
                    // Insert Log payment
                    paymentDS1.I_PAYMENT_LOGRow drLogs = new paymentDS1().I_PAYMENT_LOG.NewI_PAYMENT_LOGRow();
                    drLogs.OPT_CODE = opt;
                    drLogs.ACCESS_CHANNEL = "W";
                    drLogs.PHONE_NO = phone;
                    drLogs.PTYPE_CODE = AppCode.GetPTypeCode.pTypeCode_50Bath; //"01";
                    drLogs.PAY_STATUS = status;
                    drLogs.PAY_STATUS_DETAIL = strText;
                    drLogs.PAY_DATE = DateTime.Now;
                    AppCode.PaymentLogsInsert(drLogs);

                }

                return status;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Session by HH,by content
        /// </summary>
        /// <param name="serviceID"></param>
        /// <param name="phone"></param>
        /// <param name="opt"></param>
        /// <param name="sgID"></param>
        /// <param name="scsId"></param>
        /// <returns></returns>
        public string SubscribeSession(string serviceID,string phone,string opt,string sgID,string scsId)
        {
            try
            {
                string status = "";
                if (AppCode.CheckSessionPayment(phone, opt, sgID, scsId,AppCode.GetPTypeCode.pTypeCode_5Bath.ToString()) == 0)
                {

                    #region Command
                    string command = "<cpa-wap>";
                    command += "<authenticate>";
                    command += "<user>iSpoPW</user>";
                    command += "<password>DwaP894</password>";
                    command += "</authenticate>";
                    command += "<request-url>";
                    command += "<transaction-id>" + CreateTransaction_Dtac + "</transaction-id>";
                    command += "<msisdn>" + phone + "</msisdn>";
                    command += "<access-url>none</access-url>";
                    command += "<access-text>none</access-text>";
                    command += "<wapgw-ip>203.155.200.133</wapgw-ip>";
                    command += "<service-id>" + serviceID + "</service-id>";
                    command += "<push-service>none</push-service>";
                    command += "<sendto>none</sendto>";
                    command += "</request-url>";
                    command += "</cpa-wap>";
                    #endregion

                    LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(),"DtacSession", "start Post", phone);
                    string rtn = new sendpost().SendPost(ConfigurationManager.AppSettings["dtacSubScribeServiceURL"], command);
                    // Save Session logs file
                    LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(),"DtacSession", rtn, "");

                    status = Utilities.GetDataXML("cpa-wap", rtn, "status");
                    string strText = status == "200" ? "" : Utilities.GetDataXML("cpa-wap", rtn, "text");
                    // Insert Log payment
                    paymentDS1.I_PAYMENT_LOGRow dr = new paymentDS1().I_PAYMENT_LOG.NewI_PAYMENT_LOGRow();
                    dr.OPT_CODE = opt;
                    dr.ACCESS_CHANNEL = "W";
                    dr.PHONE_NO = phone;
                    dr.PTYPE_CODE = AppCode.GetPTypeCode.pTypeCode_5Bath;
                    dr.PAY_STATUS = status;
                    dr.PAY_STATUS_DETAIL = strText;
                    dr.PAY_DATE = DateTime.Now;
                    AppCode.PaymentLogsInsert(dr);
                    
                }
                else
                {
                    status = "605";
                }
                return status;
            }
            catch(Exception ex)
            {
                throw new Exception("SubscribeSession>>" + ex.Message);
            }
        }

        #endregion

    }
}
