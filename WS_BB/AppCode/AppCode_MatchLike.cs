using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using System.Data.OracleClient;
using OracleDataAccress;
using WebLibrary;

namespace WS_BB
{
    public class AppCode_MatchLike : AppCode_Base
    {
        public XDocument CommandMatchLike(XDocument rtnXML, string mschId, string team1
            ,string team2,string star,string ip
            ,string imei,string imsi,string phoneNo,string optCode)
        {
            try
            {

                OrclHelper.ExecuteNonQuery(strConnOracle, CommandType.StoredProcedure, "Isport_test.ISPORTAPP_Insert_MatchLike"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_MSCHID", mschId, OracleType.VarChar , ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_TEAM1", team1, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_TEAM2", team2, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_STAR", star, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_IP", ip, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_IMEI", imei, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_IMSI", imsi, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_PHONENO", phoneNo, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_OPTCODE", optCode, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_CREATEDATE", DateTime.Now, OracleType.DateTime, ParameterDirection.Input)
                        }).ToString();

                rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", ""));
                
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetSportpoolAnalysisAllLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;

        }
    }
}
