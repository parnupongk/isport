using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace isport_varity
{
    public class AppCode
    {
        public static string strIsportConn = ConfigurationManager.ConnectionStrings["IsporttdedConnectionString"].ToString();
        public static string strIsportPackConn = ConfigurationManager.ConnectionStrings["IsporttdedPackConnection"].ToString();
        public static string strIsportOraConn = ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString();

        /// <summary>
        /// GetSipContentBypCatId
        /// </summary>
        /// <param name="pCatId">default = 176</param>
        /// <param name="sexType">default = 4</param>
        /// <returns></returns>
        public DataTable GetSipContentBypCatId(string pCatId,string sexType)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strIsportPackConn, CommandType.StoredProcedure, "usp_wapui_varity_selectcontentbypcat"
                    , new SqlParameter[] { new SqlParameter("@pcat_id",pCatId) 
                                                        ,new SqlParameter("@ptype",sexType)});
                return ds.Tables.Count>0? ds.Tables[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception("GetSipContentBypCatId >>" + ex.Message);
            }
        }
    }
}
