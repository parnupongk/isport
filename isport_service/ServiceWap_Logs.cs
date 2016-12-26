using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using OracleDataAccress;
namespace isport_service
{
    public class ServiceWap_Logs
    {
        public string InsertOracleLogsPaymentFile(string strConn, string detail, string fName)
        {
            try
            {
                OracleConnection oConn = new OracleConnection(strConn);
                string rtn = OracleDataAccress.OrclHelper.ExecuteNonQuery(oConn, CommandType.StoredProcedure, "WAP_PAYMENT.SectionPaymentLogsInsert_test"
                                    , new OracleParameter[] {OracleDataAccress.OrclHelper.GetOracleParameter("p_file_name", fName, OracleType.VarChar, ParameterDirection.Input) 
                                    , OracleDataAccress.OrclHelper.GetOracleParameter("p_file_detail", detail, OracleType.VarChar, ParameterDirection.Input) 
                                    , OracleDataAccress.OrclHelper.GetOracleParameter("p_file_date", DateTime.Now, OracleType.DateTime, ParameterDirection.Input)}).ToString();

                if(oConn.State == ConnectionState.Open)oConn.Close();
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static int Subscribe_portal_log(string strConnOracle,string phoneNo, string usageAgent, string sgId
            , string optCode, string prjId, string mpCode, string scsId)
        {
            try
            {
                int rtn = OrclHelper.ExecuteNonQuery(strConnOracle, CommandType.StoredProcedure, "WAP_UI.i_subscribe_portal_log_insert",
                    new OracleParameter[] {OrclHelper.GetOracleParameter("p_phone_no",phoneNo,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_usage_date",DateTime.Now,OracleType.DateTime,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_usage_agent",usageAgent,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_sg_id",sgId,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_opt_code",optCode,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_prj_id",prjId,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_mp_code",mpCode,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_scs_id",scsId,OracleType.VarChar,ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("p_usage_date_txt",DateTime.Now.ToString("yyyyMMdd"),OracleType.VarChar,ParameterDirection.Input)});
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("Subscribe_portal_log_Insert>>" + ex.Message);
            }
        }

    }
}
