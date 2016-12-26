using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using Microsoft.ApplicationBlocks.Data;
using OracleDataAccress;

namespace isport_service
{
    public class ServiceNews
    {
        public DataTable News_SelectOracle(string strConnOracle, string top, string class_id)
        {
            try
            {
                DataSet ds = null;
                int topRow = int.Parse(top);
                using (OracleConnection conn = new OracleConnection(strConnOracle))
                {
                    ds = OracleDataAccress.OrclHelper.Fill(conn, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsBySportClass", "siamsport_news"
                        , new OracleParameter[] {OracleDataAccress.OrclHelper.GetOracleParameter("p_sportclass",class_id,OracleType.VarChar ,ParameterDirection.Input)
                                                            ,OracleDataAccress.OrclHelper.GetOracleParameter("p_rowmax",topRow,OracleType.Int32,ParameterDirection.Input )
                                                            ,OracleDataAccress.OrclHelper.GetOracleParameter("p_rowmin",0,OracleType.Int32,ParameterDirection.Input ) 
                                                            ,OracleDataAccress.OrclHelper.GetOracleParameter("p_sportType","00001",OracleType.VarChar ,ParameterDirection.Input ) 
                                                             ,OracleDataAccress.OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor ,ParameterDirection.Output ) });
                }
                return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
