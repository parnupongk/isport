using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using OracleDataAccress;
namespace WS_BB
{
    public class AppCode_Banner : AppCode_Base
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="optCode"></param>
        /// <param name="bannerType"></param>
        /// <returns></returns>
        public DataSet GetBannerByAppName(string appName, string optCode, string bannerType)
        {
            try
            {
                return OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_SelectByOpt_Banner", "ISPORTAPP_BANNER"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_app_name",appName,OracleType.NVarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_opt_code",optCode,OracleType.NVarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_banner_type",bannerType,OracleType.NVarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                    });
            }
            catch (Exception ex)
            {
                throw new Exception("GetBannerByAppName >> " + ex.Message);
            }
        }
    }
}
