using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace isport_service
{
    public class ServiceScore
    {
        #region Football liveScore
        public DataSet FootballLiveScoreSelectbyClass(string strConn ,string classId, string addTime, string time)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapui_football_livescore_select"
                    , ds, new string[] { },
                    new SqlParameter[] { new SqlParameter("@strAdd_time",addTime)
                    ,new SqlParameter("@strTime",time)
                    ,new SqlParameter("@class_id",classId) });
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataSet FootballLiveScoreSelectbyMatch(string strConn, string scsId, string matchDate, string strLng)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapui_footballlivescore_selectmatchschedule"
                    , ds, new string[] { },
                    new SqlParameter[] { new SqlParameter("@scs_id_in",scsId)
                    ,new SqlParameter("@date_in",matchDate)
                    ,new SqlParameter("@strlng",strLng) });
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}
