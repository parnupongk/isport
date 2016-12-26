using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;
using System.Xml.Linq;
using OracleDataAccress;
namespace WS_BB
{
    public class SubscribeMember
    {
        public string msisdn;
        public string status;
        public string imsi;
        public string imei;
    }

    public class AppCode_Subscribe : AppCode_Base
    {
        public struct Out_CheckActive
        {

            public string status;
            public string Mess;
            public string subTitle;
            public List<CheckActive> checkActive;
        }
        public class CheckActive
        {
            public string package_id;
            public string phone_no;
            public string opt_code;
            public string cust_id;
            public string start_date;
            public string end_date;
            public string ptype_code;
            public string package_price;

        }

        #region Gen OTP
        /// <summary>
        /// Check ว่า OTP ล่าสุดเกิน 30 นาทีหรือไม่  ( กรณีปิด app ไปดู sms แล้วเปิด app ใหม่ )
        /// </summary>
        /// <param name="imsi"></param>
        /// <param name="imei"></param>
        /// <param name="appName"></param>
        /// <returns></returns>
        public static string CheckIsOTPWait(string imsi, string imei, string appName)
        {
            string rtn = "N";
            try
            {
                DataSet ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.ISPORTAPP_Check_OTPWait", "Isportapp_active"
                     , new OracleParameter[] { OrclHelper.GetOracleParameter("p_imsi", imsi, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_imei", imei, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_app_name", appName, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("o_cursor", "", OracleType.Cursor, ParameterDirection.Output)
                                });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    rtn = "Y";
                }
                else
                {
                    rtn = "N";
                }
            }
            catch (Exception ex)
            {
                WebLibrary.ExceptionManager.WriteError("CheckIsOTPWait>> " + ex.Message);
            }
            return rtn;
        }
        public static string GenOTP(string msisdn, string optCode, string otp,string appName)
        {
            return SqlHelper.ExecuteNonQuery(strConnPack, CommandType.StoredProcedure, "usp_inst_queue_otp"
                , new SqlParameter[] { new SqlParameter("@phone_no", msisdn) 
                                                    ,new SqlParameter("@opt_code",optCode)
                                                    ,new SqlParameter("@otp",otp)
                                                    ,new SqlParameter("@app_name",appName)
                }).ToString();
        }
        #endregion

        #region Subscribe Member
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msisdn"></param>
        /// <param name="imsi"></param>
        /// <param name="imei"></param>
        /// <param name="optCode"></param>
        /// <param name="appName"></param>
        /// <param name="dayfree"></param>
        /// <returns>Y= Expire , N= not Expire</returns>
        public static SubscribeMember IsportAppCheckExpirePromotion(string msisdn, string imsi, string imei, string optCode, string appName, string dayfree)
        {
            try
            {
                SubscribeMember sm = new SubscribeMember();
                DataSet ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.ISPORTAPP_Insert_FristAccess", "Isportapp_fristaccess"
                     , new OracleParameter[] { OrclHelper.GetOracleParameter("p_phone_no", msisdn, OracleType.VarChar , ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_imsi", imsi, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_imei", imei, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_opt_code", optCode, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_app_name", appName, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_free_day", dayfree, OracleType.Number, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("o_cursor", "", OracleType.Cursor, ParameterDirection.Output)
                                });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sm.status =  "Y";
                    sm.msisdn = ds.Tables[0].Rows[0]["phone_no"].ToString();
                }
                else
                {
                    sm.status = "N";
                    sm.msisdn = "";
                }

                return sm;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static int InsertSubscribeMember(string msisdn, string imsi, string imei, string optCode,string otpCode, string appName)
                {
                    try
                    {
                        return OrclHelper.ExecuteNonQuery(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.ISPORTAPP_Insert_Activate"
                             , new OracleParameter[] { OrclHelper.GetOracleParameter("p_phone_no", msisdn, OracleType.VarChar , ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_imsi", imsi, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_imei", imei, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_opt_code", optCode, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_app_name", appName, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_otp_code", otpCode, OracleType.VarChar, ParameterDirection.Input)
                                });
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
        public static string UpdateSubscribeMember(string imsi, string imei, string optCode, string otpCode, string appName)
        {
            OracleConnection oConn = new OracleConnection(strConnOracle);
            try
            {
                //return OrclHelper.ExecuteNonQuery(strConnOracle, CommandType.StoredProcedure, "ISPORT_TEST.ISPORTAPP_Update_Activate"
                //     , new OracleParameter[] { OrclHelper.GetOracleParameter("p_imsi", imsi, OracleType.VarChar, ParameterDirection.Input)
                //                        ,OrclHelper.GetOracleParameter("p_imei", imei, OracleType.VarChar, ParameterDirection.Input)
                //                        ,OrclHelper.GetOracleParameter("p_opt_code", optCode, OracleType.VarChar, ParameterDirection.Input)
                //                        ,OrclHelper.GetOracleParameter("p_app_name", appName, OracleType.VarChar, ParameterDirection.Input)
                //                        ,OrclHelper.GetOracleParameter("p_otp_code", otpCode, OracleType.VarChar, ParameterDirection.Input)
                //                        ,OrclHelper.GetOracleParameter("p_out", 0, OracleType.Number, ParameterDirection.Output)
                //                });

                    if (oConn.State == System.Data.ConnectionState.Closed) oConn.Open();
                    OracleCommand cComm = new OracleCommand();
                    cComm.CommandType = CommandType.StoredProcedure;
                    cComm.CommandText = "ISPORT_APP.ISPORTAPP_Update_Activate";
                    cComm.Connection = oConn;
                    OracleParameter[] oracleParameters = new OracleParameter[] { OrclHelper.GetOracleParameter("p_imsi", imsi, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_imei", imei, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_opt_code", optCode, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_app_name", appName, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_otp_code", otpCode, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_out", 0, OracleType.Number, ParameterDirection.Output)};
                    foreach (OracleParameter iPara in oracleParameters)
                    {
                        cComm.Parameters.Add(iPara);
                    }
                    cComm.ExecuteNonQuery();
                    oConn.Close();

                    return cComm.Parameters["p_out"].Value.ToString();
            }
            catch (Exception ex)
            {
                oConn.Close();
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Check Recurring
        public static string CheckRecurring_IsportStarSoccer_AppMember(string opt, string msisdn, string imsi, string imei, string appName)
        {
            try
            {
                // CHECK ACTIVE ที่ SMS MEMBER
                //*451121600 true
                //*451141001 dtac
                //*451121601 ais
                string psnNumber = "";
                string rtn = "N";
                switch (opt)
                {
                    case "01": psnNumber = ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_AIS"]; break;
                    case "02": psnNumber = ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_Dtac"]; break;
                    case "03": psnNumber = ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_True"]; break;
                    case "04": psnNumber = ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_True"]; break;
                }
                DataSet ds = SqlHelper.ExecuteDataset(strConnPack, CommandType.Text, "select * from dbo.ufn_chk_wait_recur('" + opt + "','" + psnNumber + "','" + msisdn + "','" + DateTime.Now + "')");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["dupsubc_flag"].ToString() == "Y")
                {
                    rtn = "Y";
                }

                return rtn.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GetCheck Active
        public static string CheckActive_MT_Insub(string msisdn, string pssvId)
        {
            try
            {
                string rtn = "N";
                DataSet ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_TEST.CHK_ACTIVE_APP", "member_status_mt_app", 
                    new OracleParameter[] { OrclHelper.GetOracleParameter("v_phone_no",msisdn,OracleType.VarChar,ParameterDirection.Input)
                                                          ,OrclHelper.GetOracleParameter("v_pssv_id",pssvId,OracleType.VarChar,ParameterDirection.Input)
                                                          ,OrclHelper.GetOracleParameter("o_cursor","",OracleType.Cursor,ParameterDirection.Output)     
                                                       });

                rtn = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? rtn : ds.Tables[0].Rows[0]["active"].ToString();
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("CheckActive_MT_Insub >> " + ex.Message);
            }
        }
        public static string CheckActive_IsportStarSoccer_AppMember(string opt, string msisdn, string imsi, string imei, string appName,string pssvId)
        {
            try
            {
                string rtn = "N";
                DataSet ds = SqlHelper.ExecuteDataset(strConnPack, CommandType.Text, "select * from dbo.ufn_chk_dupsubc_new('" + opt + "'," + pssvId + ",'" + msisdn + "','" + DateTime.Now.AddDays(-1) + "')");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["dupsubc_flag"].ToString() == "Y")
                {
                    rtn = "Y";
                }

                return rtn.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string CheckActive_IsportStarSoccer_AppMember(string opt,string msisdn,string imsi, string imei, string appName)
        {
            try
            {
                // CHECK ACTIVE ที่ SMS MEMBER
                //*451121600 true
                //*451141001 dtac
                //*451121601 ais
                string psnNumber = "";
                string rtn = "N";
                switch (opt)
                {
                    case "01": psnNumber = ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_AIS"]; break;
                    case "02": psnNumber = ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_Dtac"]; break;
                    case "03": psnNumber = ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_True"]; break;
                    case "04": psnNumber = ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_True"]; break;
                }
                DataSet ds = SqlHelper.ExecuteDataset(strConnPack, CommandType.Text, "select * from dbo.ufn_chk_dupsubc('" + opt + "','" + psnNumber + "','" + msisdn + "','"+ DateTime.Now.AddDays(-1) +"')");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["dupsubc_flag"].ToString() == "Y")
                {
                    rtn = "Y";
                }

                return rtn.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static SubscribeMember CheckActive_AppMember(string imsi, string imei, string appName)
        {
            try
            {
                SubscribeMember sm = new SubscribeMember();
                DataSet ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.ISPORTAPP_Check_Activate", "isportapp_activate"
                     , new OracleParameter[] { 
                                OrclHelper.GetOracleParameter("p_imsi", imsi, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_imei", imei, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_app_name", appName, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_cursor", "", OracleType.Cursor, ParameterDirection.Output)
                        });

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sm.status = ds.Tables[0].Rows[0]["active"].ToString();
                    sm.msisdn = ds.Tables[0].Rows[0]["phone_no"].ToString();
                }
                else
                {
                    sm.status = "N";
                    sm.msisdn = "";
                }

                return sm;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Out_CheckActive CommandGetCheckActive(string codeType,string code, string packageId)
        {
            Out_CheckActive outCheckActive = new Out_CheckActive();
            List<CheckActive> checkActives = new List<CheckActive>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_sportapp_checkactive",
                    new SqlParameter[] { new SqlParameter("@code", code) 
                    ,new SqlParameter("@code_type",codeType)
                    ,new SqlParameter("@package_id",packageId)});
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CheckActive c = new CheckActive();
                    c.package_id = dr["package_id"].ToString();
                    c.phone_no = dr["phone_no"].ToString();
                    c.opt_code = dr["opt_code"].ToString();
                    c.cust_id = dr["cust_id"].ToString();
                    c.start_date = dr["start_date"].ToString();
                    c.end_date = dr["end_date"].ToString();
                    c.ptype_code = dr["ptype_code"].ToString();
                    c.package_price = dr["package_price"].ToString();
                    checkActives.Add(c);
                }

                outCheckActive.subTitle = (checkActives.Count > 0) ? "" : ConfigurationManager.AppSettings["subTitle"].ToString();
            }
            catch (Exception ex)
            {
                outCheckActive.status = "Error";
                outCheckActive.Mess = ex.Message;
                WebLibrary.ExceptionManager.WriteError("CommandGetCheckActive>> " + ex.Message);
            }
            
            outCheckActive.checkActive = checkActives;
            return outCheckActive;
        }
        #endregion

        #region Get Check Member

        public Out_CheckActive CommandGetCheckMember(string codeType, string code)
        {
            Out_CheckActive outCheckActive = new Out_CheckActive();
            List<CheckActive> checkActives = new List<CheckActive>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_sportapp_checkmember",
                    new SqlParameter[] { new SqlParameter("@code", code) 
                    ,new SqlParameter("@code_type",codeType)});
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CheckActive c = new CheckActive();
                    c.phone_no = dr["phone_no"].ToString();
                    c.opt_code = dr["opt_code"].ToString();
                    //c.cust_id = dr["cust_id"].ToString();
                    checkActives.Add(c);
                }
            }
            catch (Exception ex)
            {
                outCheckActive.status = "Error";
                outCheckActive.Mess = ex.Message;
                WebLibrary.ExceptionManager.WriteError("CommandGetCheckMember >> " + ex.Message);
            }
            outCheckActive.checkActive = checkActives;
            return outCheckActive;
        }
        #endregion

        #region Subscribe Package
        public Out_CheckActive CommandSubscribePackage(string strMSISDN,string strOpt,string strPssvID
            ,string strAction,string strPromo,string strMpCode)
        {
            Out_CheckActive outCheckActive = new Out_CheckActive();
            try
            {
                string urlSMS = "", serviceCode = "", sNo = "" ;
                // Select PSN_NUMBER & PSN_CODE
                string psnNumber = Select_SipService_Number(strOpt, strPssvID, strAction);
                
                switch (strOpt)
                {
                    case "01": 
                        // ais
                        serviceCode = psnNumber + strMpCode;
                        urlSMS=string.Format(ConfigurationManager.AppSettings["subscribeServiceAIS"], new string[] { strMSISDN, serviceCode, strAction, strPromo });
                        break;
                    case "02": 
                        // dtac
                        serviceCode = psnNumber.Substring(psnNumber.Length-3) + strMpCode;
                        sNo = psnNumber.Substring(6, 2);
                        urlSMS=string.Format(ConfigurationManager.AppSettings["subscribeServiceDtac"], new string[] { serviceCode, strMSISDN, sNo, strAction, strPromo });
                        break;
                    case "03": 
                        //true
                        string psnServiceId = "0101312050";
                        string psnCode = "4511000";
                        serviceCode = psnNumber + strMpCode;
                        urlSMS=string.Format(ConfigurationManager.AppSettings["subscribeServiceTrue"], new string[] { psnCode, strMSISDN, serviceCode, psnServiceId, strAction, strPromo });
                        break;
                    default: 
                        outCheckActive.status = "Fail";
                        outCheckActive.Mess = "Opt not Find"; 
                        break;
                }
                if (urlSMS != "")
                {
                    WebLibrary.ExceptionManager.WriteError(urlSMS);
                    string rtn = new push().SendGet(urlSMS);
                    if (MobileLibrary.Utilities.GetDataXML("XML", rtn, "STATUS") == "0")
                    {
                        outCheckActive.status = "Success";
                        outCheckActive.Mess = ConfigurationManager.AppSettings["smsSubscribeSuccess"];
                    }
                    else
                    {
                        outCheckActive.status = "Error";
                        outCheckActive.Mess = ConfigurationManager.AppSettings["smsSubscribeError"];
                    }

                    // INSERT BAK_SIP_SEND_TRANS_SPORTAPP
                    Insert_Sipsendtrans_SportApp(strMSISDN, strOpt, strPssvID, strAction,urlSMS,rtn);

                    
                }
            }
            catch ( Exception ex)
            {
                outCheckActive.status = "Error";
                outCheckActive.Mess = ex.Message;
                WebLibrary.ExceptionManager.WriteError("CommandSubscribePackage >> " + ex.Message);
            }
            return outCheckActive;
        }

        private string Select_SipService_Number(string strOpt, string strPssvID, string strAction)
        {
            try
            {
                string psnNumber="";
                SqlDataReader dr =  SqlHelper.ExecuteReader(strConnPack, CommandType.StoredProcedure, "usp_BB_select_sip_service_number",
                    new SqlParameter[] { new SqlParameter("@PSSV_ID",strPssvID)
                    ,new SqlParameter("@PSN_ACTION",strAction)
                    ,new SqlParameter("@OPT_CODE",strOpt)});
                while(dr.Read())
                {
                    psnNumber = dr["PSN_NUMBER2"].ToString();
                }
                return psnNumber;

            }
            catch (Exception ex)
            {
                throw new Exception("Select_SipService_Number>> " + ex.Message);
            }
        }
        public string Insert_Sipsendtrans_SportApp(string strMSISDN, string strOpt, string strPssvID, string strAction
            ,string reqURL,string result)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(strConnPack, CommandType.StoredProcedure, "usp_BB_insert_sip_send_trans_sportapp",
                    new SqlParameter[] { new SqlParameter("@CREATE_DATE",DateTime.Now)
                    ,new SqlParameter("@PSSV_ID",strPssvID)
                    ,new SqlParameter("@COMMAND",strAction)
                    ,new SqlParameter("@PHONE_NO",strMSISDN)
                    ,new SqlParameter("@OPT_CODE",strOpt)
                    ,new SqlParameter("@REQ_URL",reqURL)
                    ,new SqlParameter("@RESULT",result)}).ToString();
            }
            catch(Exception ex)
            {
                throw new Exception("Insert_Sipsendtrans_SportApp>> " + ex.Message);
            }
        }
        #endregion

        #region Active Service
        public Out_CheckActive CommandActiveService(string strMSISDN, string strPackage,string strOpt)
        {
            Out_CheckActive outCheckActive = new Out_CheckActive();
            try
            {
                string urlSMS = "";

                urlSMS = string.Format(ConfigurationManager.AppSettings["subscribeActiveService"], new string[] { strMSISDN, strPackage });
                        
                if (urlSMS != "")
                {

                    string rtn = new push().SendGet(urlSMS);
                    Insert_Sipsendtrans_SportApp(strMSISDN, strOpt, "", "A", urlSMS, rtn);
                    //rtn = "<?xml version=1.0 encoding=utf-8?>  <Out_RecvReq xmlns:xsi=http://www.w3.org/2001/XMLSchema-instance xmlns:xsd=http://www.w3.org/2001/XMLSchema xmlns=http://wap.isport.co.th>    <RecvReq>      <status>success</status>      <desc />    </RecvReq>  </Out_RecvReq>";
                    if (MobileLibrary.Utilities.GetDataXML("Out_RecvReq", rtn, "status") == "success")
                    {
                        outCheckActive.status = "Success";
                        outCheckActive.Mess = ConfigurationManager.AppSettings["smsSubscribeSuccess"];
                    }
                    else
                    {
                        outCheckActive.status = "Error";
                        outCheckActive.Mess = ConfigurationManager.AppSettings["smsSubscribeError"];
                    }

                    // INSERT BAK_SIP_SEND_TRANS_SPORTAPP
                    Insert_Sipsendtrans_SportApp(strMSISDN, strOpt, "", "A", urlSMS, rtn);


                }
            }
            catch (Exception ex)
            {
                outCheckActive.status = "Error";
                outCheckActive.Mess = ex.Message;
                WebLibrary.ExceptionManager.WriteError("CommandActiveService >> " + ex.Message);
            }
            return outCheckActive;
        }
        #endregion

        #region Isport App Get Service SMS
        public XDocument CommandGetSubscribeService(string optCode,string appName)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", "SMS Service ")
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {

                DataSet ds = new AppCode_Banner().GetBannerByAppName(appName, optCode, "detail");

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = ds.Tables[0].Rows[i];
                        rtnXML.Element("SportApp").Add(
                            AppCode_Utility.GetImageElementSportPhone("Privilege", dr["big_img"].ToString() // big
                            , dr["medium_img"].ToString() // medium
                            , dr["small_img"].ToString() // small
                            , dr["title"].ToString()
                            , dr["footer"].ToString()
                            , dr["detail"].ToString()
                            , dr["phone_no"].ToString()
                            , dr["link"].ToString()
                            , dr["banner_type"].ToString()));

                    }
                }
                rtnXML.Element("SportApp").Add(new XElement("status", "success")
                , new XElement("message", ""));
            }
            catch(Exception ex)
            {

                ExceptionManager.WriteError("CommandGetSubscribeService >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));

            }
            return rtnXML;
        }


       
        public XDocument CommandGetSMSService(XDocument rtnXML,string optCode,string mpCode)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConnPack, CommandType.StoredProcedure, "usp_isportapp_getservice"
                    , new SqlParameter[] { new SqlParameter("@opt_id", optCode) });
                string shortCode = "",cancelCode = "";
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        shortCode = dr["PSN_NUMBER3"].ToString() == "" ? dr["PSN_NUMBER2"].ToString() : dr["PSN_NUMBER3"].ToString();
                        cancelCode = dr["cancel_number3"].ToString() == "" ? dr["cancel_number2"].ToString() : dr["cancel_number3"].ToString();
                        XElement element = new XElement("SMSService"
                            , new XAttribute("pssv_id", dr["pssv_id"].ToString())
                            , new XAttribute("pssv_name", dr["pssv_desc"].ToString())
                            , new XAttribute("pssv_shortcode", shortCode + mpCode)
                            , new XAttribute("pssv_price", dr["psv_price"].ToString())
                            , new XAttribute("pssv_promotion", "รับฟรี 7 วัน เฉพาะลูกค้าใหม่ที่ยังไม่เคยใช้บริการ")
                            , new XAttribute("pssv_desc", ConfigurationManager.AppSettings["smsService" + dr["pssv_id"].ToString()].ToString())
                            , new XAttribute("pssv_image", "")
                            , new XAttribute("pssv_cancel", cancelCode)
                            );
                        rtnXML.Element("SportApp").Add(element);
                    }
                    //rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    //, new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "error")
                   , new XElement("message", "ขออภัยไม่พบข้อมูล เพิ่มเติมโทร 02-502-6767"));
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.WriteError("CommandGetSubscribeService >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));

            }
            return rtnXML;
        }

        public XDocument CommandGetSMSService_IsportStarSoccer(XDocument rtnXML, string optCode, string mpCode)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConnPack, CommandType.StoredProcedure, "usp_isportapp_getservice_starsoccerapp"
                    , new SqlParameter[] { new SqlParameter("@opt_id", optCode) });
                string shortCode = "", cancelCode = "";
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        shortCode = dr["PSN_NUMBER3"].ToString() == "" ? dr["PSN_NUMBER2"].ToString() : dr["PSN_NUMBER3"].ToString();
                        cancelCode = dr["cancel_number3"].ToString() == "" ? dr["cancel_number2"].ToString() : dr["cancel_number3"].ToString();
                        XElement element = new XElement("SMSService"
                            , new XAttribute("pssv_id", dr["pssv_id"].ToString())
                            , new XAttribute("pssv_name", dr["pssv_desc"].ToString())
                            , new XAttribute("pssv_shortcode", shortCode + mpCode)
                            , new XAttribute("pssv_price", dr["psv_price"].ToString())
                            , new XAttribute("pssv_promotion", "")
                            , new XAttribute("pssv_desc", string.Format( ConfigurationManager.AppSettings["smsService" + dr["pssv_id"].ToString()].ToString(), dr["pssv_desc"].ToString()))
                            , new XAttribute("pssv_remark1", "ค่าบริการเพียงข้อความละ " + dr["psv_price"].ToString() + " บาทเท่านั้น")
                            , new XAttribute("pssv_remark2", "")
                            , new XAttribute("pssv_image", "ยกเลิกบริการโทร 02-502-6767")
                            , new XAttribute("pssv_cancel", cancelCode)
                            );
                        rtnXML.Element("SportApp").Add(element);
                    }
                    //rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    //, new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "error")
                   , new XElement("message", "ขออภัยไม่พบข้อมูล เพิ่มเติมโทร 02-502-6767"));
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.WriteError("CommandGetSubscribeService >> " + ex.Message);
                //throw new Exception("CommandGetSMSService_IsportStarSoccer >>" + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                   , new XElement("message", ex.Message));

            }
            return rtnXML;
        }
        #endregion

        #region Check Active SMS 
        public string CommandCheckActive(string msdn,string optCode,string psnNumber)
        {
            string rtn ="N";
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_checkactive_sms",
                    new SqlParameter[] {new SqlParameter("@optCode",optCode)
                    ,new SqlParameter("@psn_number",psnNumber)
                    ,new SqlParameter("@msnd",msdn)});
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    rtn = ds.Tables[0].Rows[0]["dupsubc_flag"].ToString();
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("CommandGetSubscribeService >> " + ex.Message);
            }
            return rtn ; 
        }
        #endregion

    }
}
