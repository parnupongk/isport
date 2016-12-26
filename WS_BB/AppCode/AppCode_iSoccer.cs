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
    public class AppCode_iSoccer : AppCode_Base
    {
        public static string UpdateCustomer(string msisdn,string imei,string model,string appName)
        {
            try
            {
                int rtn = OrclHelper.ExecuteNonQuery(strConnOracle, CommandType.StoredProcedure, "GAMEs.customer_edit"
                    , new OracleParameter[] {OrclHelper.GetOracleParameter("p_phone_no",msisdn,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_imei",imei,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_mobile_agent",model,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_game_name",appName,OracleType.VarChar,ParameterDirection.Input)
                                            });
                return rtn.ToString();
            }
            catch(Exception ex)
            {
                throw new Exception("UpdateCustomer >> " + ex.Message);
            }
        }

        public static DataSet GetGameAnswerResult(string msisdn,string imei,string matchDate)
        {
            try
            {
                DataSet dsAnwser = OrclHelper.Fill(AppCode_Base.strConnOracle, CommandType.StoredProcedure, "GAMES.check_answer_result", "GAME_SUBSCRIBE"
                                , new OracleParameter[] { OrclHelper.GetOracleParameter("p_phone_no", msisdn, OracleType.VarChar, ParameterDirection.Input)
                                                         ,OrclHelper.GetOracleParameter("p_imei",imei,OracleType.VarChar,ParameterDirection.Input)
                                                         ,OrclHelper.GetOracleParameter("p_game_name",AppName.iSoccer.ToString(),OracleType.VarChar,ParameterDirection.Input)
                                                         ,OrclHelper.GetOracleParameter("p_match_date",matchDate,OracleType.VarChar,ParameterDirection.Input)
                                                         ,OrclHelper.GetOracleParameter("o_cursor","",OracleType.Cursor,ParameterDirection.Output)
                                                        });
                return dsAnwser;
            }
            catch(Exception ex)
            {
                throw new Exception("GetGameAnswerResult >> " + ex.Message);
            }
        }

    }
}