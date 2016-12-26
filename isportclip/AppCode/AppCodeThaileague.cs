using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
namespace isportclip
{
    public class AppCodeThaileague : AppMain
    {
        public static DataTable SelectByTeamCode(string teamCode)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure
                    , "usp_wapisport_contentclip_selectbyteamcode", ds
                    , new string[] { "CONTENT_CLIP" }
                    , new SqlParameter[] { new SqlParameter("@teamCode",teamCode)});
                return ds.Tables[0];
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataView FiilterBySubCat(DataView dv, string subCatID)
        {
            try
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
                str.Append(" cscat_id=" + subCatID);
                dv.Sort = "MSCH_ID";
                dv.RowFilter = str.ToString();
               
                return dv;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataTable SelectByCcatID(string ccatId,string contentDate)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure
                    , "usp_wapisport_contentclip_selectbyccatid", ds
                    , new string[] { "CONTENT_CLIP" }
                    , new SqlParameter[] { new SqlParameter("@ccat_id",ccatId)
                    ,new SqlParameter("@content_date",contentDate)});
                return ds.Tables[0];
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataTable SelectByClipID(string clipID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure
                    , "usp_wapisport_contentclip_selectbyclipid", ds
                    , new string[] { "CONTENT_CLIP" }
                    , new SqlParameter[] { new SqlParameter("@clip_id",clipID)});
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataTable SelectByMSCHID(string mschID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure
                    , "usp_wapisport_contentclip_selectbymschid", ds
                    , new string[] { "CONTENT_CLIP" }
                    , new SqlParameter[] { new SqlParameter("@msch_id", mschID) });
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
