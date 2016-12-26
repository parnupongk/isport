using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
namespace isport
{
    public class AppImages : AppMain
    {
        public int ImagesInsert(isportDS.wapisport_imagesRow dr )
        {
            try
            {
                return SqlHelper.ExecuteNonQueryTypedParams(strConn, "usp_wapisport_imagesinsert", dr);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataSet ImagesSelect()
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_imagesselectall"
                    , ds, new string[] { "wapisport_images" });
                return ds;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int ImagesDelete(string imagesID)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.StoredProcedure, "usp_wapisport_imagesdelete"
                    , new SqlParameter[] {new SqlParameter("@images_id",imagesID) });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
