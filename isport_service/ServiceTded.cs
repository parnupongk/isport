using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web;
using Microsoft.ApplicationBlocks.Data;

namespace isport_service
{
    public class ServiceTded
    {
        public static void GenTded(string strConn, Control ctr, string sgId, string classId, string projectType, bool isHeader, string pageRedirect)
        {
            try
            {
                pageRedirect = pageRedirect.IndexOf('?') > -1 ? pageRedirect : pageRedirect + "?";
                classId = classId == null ? "" : classId;
                string url = "";
                DataSet objDs = GetSportContentBySgId(strConn, sgId);
                if (objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
                {

                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'>ชี้ขาดทีมทีเด็ด! ทีมเดียวเท่านั้นที่น่าเชียร์!!</div></div>"));

                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl(@"<div class='row featurette'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>
                                                                                            </div><div class=col-xs-11 col-sm-11 col-lg-11 col-md-11>" + objDs.Tables[0].Rows[0]["scnt_detail_local"].ToString() + "</div></div>"));

                }
                else
                {
                    ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genTextHeader("", "ขออภัยไม่พบข้อมูลการทีเด็ดค่ะ", pageRedirect, "0", projectType, false, ""));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GenAnalyse >> " + ex.Message);
            }
        }

        private static DataSet  GetSportContentBySgId(string strConn,string sgId)
        {
            try
            {
                return SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_sportcontent_sgid"
                        , new SqlParameter[] { new SqlParameter("@sg_id", sgId) });
            }
            catch (Exception ex)
            {
                throw new Exception("GetSportContentBySgId >> " + ex.Message);
            }
        }

        public static DataSet GetSipContentByPcntDetail(string strConn, string pcntDetail)
        {
            try
            {
                return SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getsip_content_bypcntdetail"
                    , new SqlParameter[] {new SqlParameter("@pcnt_detail",pcntDetail) });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataSet GetSipContentByPcntId(string strConn, string pcntId)
        {
            try
            {
                return SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_getsip_content_bypcntid"
                    , new SqlParameter[] { new SqlParameter("@pcnt_id", pcntId) });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
