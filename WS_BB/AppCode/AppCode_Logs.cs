using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Xml.Linq;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;
using OracleDataAccress;

namespace WS_BB
{
    public class AppCode_Logs : AppCode_Base
    {


        public void Logs_Insert(string pageName,string contestGroupId,string countryId,string type
            ,string code,string appName,string imei,string imsi
            ,string phone_no,string model,string optCode,string  ip
            ,string isClick)
        {

            try
            {
                contestGroupId = contestGroupId == null ? "" : contestGroupId;
                countryId = countryId == null ? "" : countryId;
                type = type == null ? "" : type;
                code = code == null ? "" : code;
                imei = imei == null ? "" : imei;
                imsi = imsi == null ? "" : imsi;
                phone_no = phone_no == null ? "" : phone_no;
                model = model == null ? "" : model;
                optCode = optCode == null ? "" : optCode;
                ip = ip == null ? "" : ip;
                isClick = isClick == "" ? "N" : isClick;
                OrclHelper.ExecuteNonQuery(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_Insert_Logs"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("P_LOGS_DATE", DateTime.Now, OracleType.DateTime , ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("P_PAGE_NAME", pageName, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("P_CONTESTGROUPID", contestGroupId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("P_COUNTRYID", countryId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("P_MODEL_TYPE", type, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("P_MODEL_CODE", code, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("P_APP_NAME", appName, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("P_IMEI", imei, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("P_IMSI", imsi, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("P_PHONE_NO", phone_no, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("P_MODEL", model, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("P_OPT_CODE", optCode, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("P_LOGS_IP", ip, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("P_ISCLICK", isClick, OracleType.VarChar, ParameterDirection.Input)
                        }).ToString();
                
            }
            catch(Exception ex)
            {
                WebLibrary.ExceptionManager.WriteError("Logs_Insert >> " + ex.Message + "imei:" + imei + "imsi:"+imsi+"phone_no"+phone_no+"model:"+model+"opt_code:"+optCode+"logsIP:"+ip);
            }
        }
        public void Logs_Insert_IP()
        {

            try
            {
                string ip = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null || HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] =="") ?
                    HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]; 
                    
                OrclHelper.ExecuteNonQuery(strConnOracle, CommandType.StoredProcedure, "Isport_test.ISPORTAPP_Insert_logs_ip"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_logs_id", Guid.NewGuid().ToString(), OracleType.VarChar , ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_logs_date", DateTime.Now, OracleType.DateTime, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_logs_ip", ip, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_logs_url", System.Web.HttpContext.Current.Request.Url.ToString(), OracleType.VarChar, ParameterDirection.Input)
                        }).ToString();

            }
            catch (Exception ex)
            {
                WebLibrary.ExceptionManager.WriteError("Logs_Insert_IP >> " + ex.Message);
            }
        }

    }
}
