using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
namespace isport_service
{
    public class ServiceProgram
    {
        public DataSet FootballProgram(string strConn, string classId, string strLng)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapui_footballprogram"
                    , ds, new string[] { },
                    new SqlParameter[] { new SqlParameter("@class_id", classId) });
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// FootballProgramByDate
        /// </summary>
        /// <param name="strConn"></param>
        /// <param name="classId"></param>
        /// <param name="strLng"></param>
        /// <param name="strDate">yyyyMMdd</param>
        /// <returns></returns>
        public DataSet FootballProgramByDate(string strConn, string classId, string strLng,string strDate)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapui_footballprogrambydate"
                    , ds, new string[] { },
                    new SqlParameter[] { new SqlParameter("@class_id", classId)
                                                    ,new SqlParameter("@date", strDate)});
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
