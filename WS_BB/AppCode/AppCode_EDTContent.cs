using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OracleClient;
using System.Data;
using OracleDataAccress;
namespace WS_BB
{
    public class AppCode_EDTContent : AppCode_Base
    {
        /// <summary>
        /// ใช้ feed edt data to ais
        /// </summary>
        /// <param name="displayDate">MM/dd/yyyy</param>
        /// <returns></returns>
        public DataSet CommandGetEDTByDisplayDate(string displayDate)
        {
            try
            {
                return OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "Wap_UI.EDT_SELECTBY_DisplayDate", "edt_xml"
                                                        , new OracleParameter[] {OrclHelper.GetOracleParameter("p_display_date",displayDate,OracleType.VarChar,ParameterDirection.Input)
                                                                                            ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}