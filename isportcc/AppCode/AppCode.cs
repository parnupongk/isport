using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
namespace isportcc
{
    public class AppCode : AppMain
    {
        #region Header
        /// <summary>
        /// DeleteHeader
        /// </summary>
        /// <param name="headerID"></param>
        /// <returns></returns>
        public int DeleteHeader(string headerID)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();

                    rtn = SqlHelper.ExecuteNonQuery(sqlTran, CommandType.StoredProcedure, "usp_wapisport_headerdeletebyid"
                        , new SqlParameter[] { new SqlParameter("@header_id", headerID) });

                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQuery(sqlTran, CommandType.StoredProcedure, "usp_wapisport_contentdeletebymasterid"
                        , new SqlParameter[] { new SqlParameter("@master_id", headerID) });

                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// SelectHeaderByOperator
        /// </summary>
        /// <param name="operatorName"></param>
        /// <returns></returns>
        public DataSet SelectHeaderByOperator(string operatorName, string projectType)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_headerselectbyoperator", ds
                    , new string[] { "wapisport_header" }
                    , new SqlParameter[] { new SqlParameter("@header_operator", operatorName)
                    ,new SqlParameter("@header_projecttype",projectType)});
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// SelectHeaderAll
        /// </summary>
        /// <returns></returns>
        public DataSet SelectHeaderAll(string projectType)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_headerselectall", ds
                    , new string[] { "wapisport_header" }, new SqlParameter[] { new SqlParameter("@header_projecttype", projectType) });
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// UpdateHeader
        /// </summary>
        /// <param name="drHeader"></param>
        /// <param name="drContent"></param>
        /// <returns></returns>
        public int UpdateHeader(isportDS.wapisport_headerRow drHeader, isportDS.wapisport_contentRow drContent)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();
                    rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_headerupdate", drHeader);
                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_contentupdate", drContent);
                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// InsertHeader
        /// </summary>
        /// <param name="drHeader"></param>
        /// <param name="drContent"></param>
        /// <returns></returns>
        public int InsertHeader(isportDS.wapisport_headerRow drHeader, isportDS.wapisport_contentRow drContent)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();
                    rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_headerinsert", drHeader);
                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_contentinsert", drContent);
                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region UI

        /// <summary>
        /// SelectUIByID
        /// </summary>
        /// <param name="ui_id"></param>
        /// <returns></returns>
        public DataSet SelectUIByID(string ui_id)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_uiselectbyid", ds
                    , new string[] { "wapisport_ui" }
                    , new SqlParameter[] { new SqlParameter("@ui_id", ui_id) });
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// SelectUIByLevel ** admin
        /// </summary>
        /// <param name="master_id"></param>
        /// <param name="ui_level"></param>
        /// <returns></returns>
        public DataSet SelectUIByLevel(string master_id, int ui_level, string ui_projecttype)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_uiselectbylevel", ds
                    , new string[] { "wapisport_ui" }
                    , new SqlParameter[]{new SqlParameter("@ui_level",ui_level)
                    , new SqlParameter("@master_id",master_id)
                    ,new SqlParameter("@ui_projecttype",ui_projecttype)});
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// SelectUIByLevel ** Fillter Enddate
        /// </summary>
        /// <param name="master_id"></param>
        /// <param name="ui_level"></param>
        /// <returns></returns>
        public DataSet SelectUIByLevel_Wap(string master_id, int ui_level, string operatorName, string ui_projecttype)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_uiselectbylevel_wap", ds
                    , new string[] { "wapisport_ui" }
                    , new SqlParameter[]{new SqlParameter("@ui_level",ui_level)
                    , new SqlParameter("@master_id",master_id)
                    , new SqlParameter("@ui_operator",operatorName)
                    ,new SqlParameter("@ui_projecttype",ui_projecttype)});
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// DeleteUI
        /// </summary>
        /// <param name="ui_ID"></param>
        /// <returns></returns>
        public int DeleteUI(string ui_ID)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();

                    rtn = SqlHelper.ExecuteNonQuery(sqlTran, CommandType.StoredProcedure, "usp_wapisport_uideletebyid"
                        , new SqlParameter[] { new SqlParameter("@ui_id", ui_ID) });

                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQuery(sqlTran, CommandType.StoredProcedure, "usp_wapisport_contentdeletebymasterid"
                        , new SqlParameter[] { new SqlParameter("@master_id", ui_ID) });

                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// InsertUI
        /// </summary>
        /// <param name="drUI"></param>
        /// <param name="drContent"></param>
        /// <returns></returns>
        public int InsertUI(isportDS.wapisport_uiRow drUI, isportDS.wapisport_contentRow drContent)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();
                    rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_uiinsert", drUI);
                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_contentinsert", drContent);
                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// UpdateUI
        /// </summary>
        /// <param name="drUI"></param>
        /// <param name="drContent"></param>
        /// <returns></returns>
        public int UpdateUI(isportDS.wapisport_uiRow drUI, isportDS.wapisport_contentRow drContent)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();
                    rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_uiupdate", drUI);
                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_contentupdate", drContent);
                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Footer
        /// <summary>
        /// DeleteFooter
        /// </summary>
        /// <param name="footerID"></param>
        /// <returns></returns>
        public int DeleteFooter(string footerID)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();

                    rtn = SqlHelper.ExecuteNonQuery(sqlTran, CommandType.StoredProcedure, "usp_wapisport_footerdeletebyid"
                        , new SqlParameter[] { new SqlParameter("@header_id", footerID) });

                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQuery(sqlTran, CommandType.StoredProcedure, "usp_wapisport_contentdeletebymasterid"
                        , new SqlParameter[] { new SqlParameter("@master_id", footerID) });

                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// SelectFooterByoperator
        /// </summary>
        /// <param name="operatorName"></param>
        /// <returns></returns>
        public DataSet SelectFooterByoperator(string operatorName, string projectType)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_footerselectbyoperator", ds
                    , new string[] { "wapisport_footer" }
                    , new SqlParameter[] { new SqlParameter("@footer_operator",operatorName) 
                    ,new SqlParameter("@footer_projecttype",projectType)});
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// SelectFooterAll
        /// </summary>
        /// <returns></returns>
        public DataSet SelectFooterAll(string projectType)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_footerselectall", ds
                    , new string[] { "wapisport_footer" }, new SqlParameter("@footer_projecttype", projectType));
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// UpdateFooter
        /// </summary>
        /// <param name="drUI"></param>
        /// <param name="drContent"></param>
        /// <returns></returns>
        public int UpdateFooter(isportDS.wapisport_footerRow drFooter, isportDS.wapisport_contentRow drContent)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();
                    rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_footerupdate", drFooter);
                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_contentupdate", drContent);
                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// InsertUI
        /// </summary>
        /// <param name="drFooter"></param>
        /// <param name="drContent"></param>
        /// <returns></returns>
        public int InsertFooter(isportDS.wapisport_footerRow drFooter, isportDS.wapisport_contentRow drContent)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();
                    rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_footerinsert", drFooter);
                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_contentinsert", drContent);
                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
