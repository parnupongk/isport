using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using OracleDataAccress;
using WebLibrary;

namespace isport_payment
{

    public class AppCode
    {

        public struct GetPTypeCode
        {
            public const string pTypeCode_5Bath = "02";
            public const string pTypeCode_50Bath = "01";
        }

        public static string GetIsportConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["IsportPaymentConnectionString"].ToString();
            }
        }
        public static string GetOracleConnectionString
        {
            get{
            return ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString();
            }
        }

        #region Check Session
        public static int CheckSessionPayment(string phone,string opt,string sgID,string scsId,string pTypeCode)
        {
            OracleConnection oConn = new OracleConnection(AppCode.GetOracleConnectionString);
            try
            {
                int rtn = 0;
                
                if(oConn.State == ConnectionState.Closed) oConn.Open();
                    object obj = OrclHelper.ExecuteScalar(oConn, CommandType.StoredProcedure
                    , "WAP_PAYMENT.CheckSessionPayment"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_phoneno",phone,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_sg_id",sgID,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_scs_id",scsId==""? "" :scsId,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_ptype_code",pTypeCode,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("o_checksession",0,OracleType.Int32,ParameterDirection.Output)});

                    rtn = obj == null ? 0 : int.Parse(obj.ToString());
                    oConn.Close();
                return rtn;
            }
            catch(Exception ex)
            {
                oConn.Close();
                throw new Exception("CheckSessionPayment>>" + ex.Message);
                
            }
        }
        #endregion

        #region Package
        public static int Package_GetValidDay(string strPTypeCode,string strOptCode,string strChargeCode,string strSgId)
        {
            try
            {
                object obj = null;
                using (OracleConnection oConn = new OracleConnection(AppCode.GetOracleConnectionString))
                {
                    obj = OrclHelper.ExecuteScalar(oConn, CommandType.StoredProcedure, "WAP_PAYMENT.Package_GetValidDay"
                        , new OracleParameter[] { OrclHelper.GetOracleParameter("p_ptype_code",strPTypeCode,OracleType.VarChar,ParameterDirection.Input)
                        ,OrclHelper.GetOracleParameter("p_opt_code",strOptCode,OracleType.VarChar,ParameterDirection.Input)
                        ,OrclHelper.GetOracleParameter("p_charge_code",strChargeCode,OracleType.VarChar,ParameterDirection.Input)
                        ,OrclHelper.GetOracleParameter("p_sg_id",strSgId,OracleType.VarChar,ParameterDirection.Input)
                        ,OrclHelper.GetOracleParameter("o_PackageBalidDay",0,OracleType.Int32,ParameterDirection.Output)});
                    return obj == null ? 0 : int.Parse(obj.ToString());
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Content Usage Logs
        public static int ContentUsageLogsInsert(paymentDS1.I_CONTENT_USAGE_LOGRow dr)
        {
            try
            {
                int rtn = 0;
                rtn = OrclHelper.ExecuteNonQuery(AppCode.GetOracleConnectionString, CommandType.StoredProcedure, "WAP_PAYMENT.ContentUsageLogsInsert"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_sg_id",dr.SG_ID,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_ccat_id",dr.CCAT_ID,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_phone_no",dr.PHONE_NO,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_access_channel",dr.ACCESS_CHANNEL,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_content_id",dr.CONTENT_ID,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_usage_type",dr.USAGE_TYPE,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_usage_date",dr.USAGE_DATE,OracleType.DateTime,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_ref_id",dr.REF_ID,OracleType.VarChar,ParameterDirection.Input) // PCNT_ID
                    });
                return rtn;
            }
            catch(Exception ex)
            {
                throw new Exception(" ContentUsageLogsInsert>> " + ex.Message);
            }
        }
        #endregion

        #region Subscribe_package , subscribe_customer
        public static int SubscribeInsert(string phone,string pTypeCode,string chargeCode
            ,string opt,string sg,string mp,string prj,string status,string scs_id)
        {
            string str="";
            OracleConnection oConn = new OracleConnection(AppCode.GetOracleConnectionString);
            try
            {
                str = "select WAP.INST_MEMBER('" + phone + "','" + pTypeCode + "','" + chargeCode + "','" + opt + "','"
                    + sg + "','" + mp + "','" + prj + "','" + status + "','" + scs_id + "') as status from dual";

                //WebLibrary.LogsManager.WriteLogs("Insert_Member_Dual", "", str);

                 if(oConn.State == ConnectionState.Closed) oConn.Open();
                     IDataReader dr = OrclHelper.ExecuteReader(oConn, CommandType.Text, str);
                     while(dr.Read())
                     {
                         str = dr["status"].ToString();
                     }
                 oConn.Close();
                 return str == "" ? -1 : int.Parse(str.ToString());

            }
            catch(Exception ex)
            {
                oConn.Close();
                WebLibrary.LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "Error_Insert_Member_Dual", str + " Error : "+ ex.Message, "");
                throw new Exception("SubscribeInsert>>" + ex.Message);
            }
        }
        #endregion

        #region Payment Logs and Token
        public static int TokenLogsInsert(string token,string cmd,string sid,string sn,string spsid,string msisdn,string ip)
        {
            try
            {
                return OrclHelper.ExecuteNonQuery(AppCode.GetOracleConnectionString, CommandType.StoredProcedure, "WAP_PAYMENT.INST_I_SUBSCRIBE_TOKEN"
                    , new OracleParameter[] {OrclHelper.GetOracleParameter("p_token",token,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_cmd",cmd,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_sid",sid,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_sn",sn,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_spsid",spsid,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_msisdn",msisdn,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_create_date",DateTime.Now,OracleType.DateTime,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_ip",ip,OracleType.VarChar,ParameterDirection.Input)
                    });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static int PaymentLogsInsert(paymentDS1.I_PAYMENT_LOGRow dr)
        {
            try
            {
                return OrclHelper.ExecuteNonQuery(AppCode.GetOracleConnectionString, CommandType.StoredProcedure, "WAP_PAYMENT.PaymentLogsInsert"
                    , new OracleParameter[] {OrclHelper.GetOracleParameter("p_opt",dr.OPT_CODE,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_channel",dr.ACCESS_CHANNEL,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_phone",dr.PHONE_NO,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_ptype_code",dr.PTYPE_CODE,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_status",dr.PAY_STATUS,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_status_detail",dr.PAY_STATUS_DETAIL,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_date",dr.PAY_DATE,OracleType.DateTime,ParameterDirection.Input)});
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Service Group
        public static string ServiceGroup_GetURLRedirect(string sgId,string scsId)
        {
            try
            {
                string url=ConfigurationManager.AppSettings["defaultURLRedirect"].ToString(), para = "";
                using(OracleConnection oConn = new OracleConnection(AppCode.GetOracleConnectionString))
                {
                    OracleParameter iPut = new OracleParameter();
                    iPut.ParameterName = "p_sg_id";
                    iPut.Value = sgId;
                    iPut.Direction = ParameterDirection.Input;
                    iPut.OracleType = OracleType.VarChar;

                    OracleParameter oPut = new OracleParameter();
                    oPut.ParameterName = "o_URLRedirect";
                    oPut.Direction = ParameterDirection.Output;
                    oPut.OracleType = OracleType.Cursor;

                    IDataReader dr = OrclHelper.ExecuteReader(oConn, CommandType.StoredProcedure, "WAP_PAYMENT.ServiceGroup_GetURLRedirect",
                        new OracleParameter[] {iPut,oPut });
                    while (dr.Read())
                    {
                        url = dr["SG_URL"] == null ? url : dr["SG_URL"].ToString();
                        para = dr["SG_PARENT_ID"] == null ? "" : dr["SG_PARENT_ID"].ToString();
                    }

                    //if (para != "")
                    //{
                        if (para == "106" || para == "101" || para == "118")
                        {
                            para = "cat_id=" + scsId + "&";
                        }
                        else if (sgId == "130" || sgId == "144" || sgId == "160")
                        {
                            para = "scs_id=" + scsId + "&";
                        }
                        else if (sgId == "95" || sgId == "208" || sgId == "255" || sgId == "260")
                        {
                            para = "pcnt_id=" + scsId + "&";
                        }
                        else if (sgId == "209")
                        {
                            para = "msch_id=" + scsId + "&";
                        }
                    //}
                }
                return url + para;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string GetServiceGroup(string sgID)
        {
            OracleConnection oConn = new OracleConnection(AppCode.GetOracleConnectionString);
            try
            {
                string rtn = "";
                if( oConn.State == ConnectionState.Closed ) oConn.Open();
                    OracleParameter oPut = new OracleParameter();
                    oPut.ParameterName = "Content_level";
                    oPut.OracleType = OracleType.Int32;
                    oPut.Direction = ParameterDirection.Output;

                    OracleParameter iPut = new OracleParameter("p_sg_id",sgID);
                    iPut.Direction = ParameterDirection.Input;
                    iPut.OracleType = OracleType.Int32;

                    object obj = OrclHelper.ExecuteScalar(oConn, CommandType.StoredProcedure
                    , "WAP_PAYMENT.ServiceGroup_GetContentLevel"
                    , new OracleParameter[] { iPut, oPut });

                    rtn = obj == null ? "" : obj.ToString();
                oConn.Close();
                return rtn;
            }
            catch(Exception ex)
            {
                oConn.Close();
                throw new Exception(ex.Message);
            }
        }
        
        #endregion

        #region Usage Subscribe Logs
        public static int UsageSubscribeLogs_Insert(paymentDS1.I_SUBSCRIBE_PORTAL_LOGRow dr)
        {
            try
            {
                return OrclHelper.ExecuteNonQuery(AppCode.GetOracleConnectionString, CommandType.StoredProcedure, "WAP_PAYMENT.SubScribeProtalLogs_Insert"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_phone",dr.PHONE_NO,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_usage_date",dr.USAGE_DATE,OracleType.DateTime,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_usage_agent",dr.USAGE_AGENT,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_sg_id",dr.SG_ID,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_opt",dr.OPT_CODE,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_prj",dr.PRJ_ID,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_mp",dr.MP_CODE,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_scs_id",dr.SCS_ID,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_usage_date_text",dr.USAGE_DATE_TXT,OracleType.VarChar,ParameterDirection.Input)});
            }
            catch(Exception ex)
            {
                throw new Exception("UsageSubscribeLogs_Insert>> " + ex.Message);
            }
        }
        #endregion

        #region SendState to Crie
        public static void SendStatetoCrie(string serviceId , string contentId,string msisdn,string action)
        {
            try
            {

                string urlState = String.Format(ConfigurationManager.AppSettings["dtacSendStatetoCrie"], msisdn, serviceId, contentId, action);
                string rtn = new sendpost().SendGet(urlState);
                WebLibrary.LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(),"SendState_Crie", rtn , "serviceID: " + serviceId + " msisdn: " + msisdn);

            }
            catch(Exception ex)
            {
                WebLibrary.LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(),"Error_SendState_Crie", ex.Message,"serviceID: " + serviceId + " msisdn: " + msisdn);
            }
        }
        #endregion

    }
}
