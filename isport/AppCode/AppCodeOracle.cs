using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using OracleDataAccress;
namespace isport
{
    public class AppCodeOracle : AppMain
    {
        #region SIP_content
        public DataTable SIP_Content_SelectTop(string top)
        {
            try
            {
                DataSet ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "Wap_UI.SIP_Content_GetNews", "i_SIP_CONTENT"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_rumnum", top, OracleType.VarChar, ParameterDirection.Input)
                    ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)});
                return ds.Tables[0];
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Subscribe_portal_log
        public int Subscribe_portal_log(string phoneNo, string usageAgent, string sgId
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
                throw new Exception("Subscribe_portal_log_Insert>>" + ex.Message + " mp_code="+mpCode + " scs_id=" + scsId + " prj=" + prjId);
            }
        }
        #endregion
    }
}
