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
    public class ServiceAnalyse
    {
        public static void GenAnalyse(string strConn, Control ctr, string sgId, string classId, string projectType, bool isHeader, string pageRedirect)
        {
            try
            {
                pageRedirect = pageRedirect.IndexOf('?') > -1 ? pageRedirect : pageRedirect + "?";
                classId = classId == null ? "" : classId;
                // Select All Class
                string url = "";
                DataSet objDs = GetSportContentBySgId(strConn, sgId, classId);
                if (objDs.Tables.Count > 0 && objDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in objDs.Tables[0].Rows)
                    {
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'>" + dr["class_name"] + "</div></div>"));
                        // Select By Class
                        DataSet objDs2 = GetSportContentByClassId(strConn, sgId, dr["class_id"].ToString());
                        int index = 1;string cssName = "rowprogramdetail";
                        foreach (DataRow drDetail in objDs2.Tables[0].Rows)
                        {
                            cssName = (index % 2) == 0 ? "rowprogramdetail_" : "rowprogramdetail";
                            url = pageRedirect + "|msch_id=" + drDetail["msch_id"];//+ "|" + HttpContext.Current.Request.QueryString;
                            ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl(@"<div class='"+cssName+"'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1></div><a href='" + url + "' class='linkprogram'><div class=col-xs-11 col-sm-11 col-lg-11 col-md-11>" + drDetail["match_detail"].ToString() + "</div></a></div>"));
                            url = "";
                            index++;
                        }
                    }
                }
                else
                {
                    ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genTextHeader("", "ขออภัยไม่พบข้อมูลการวิเคราะห์ค่ะ", pageRedirect, "0", projectType, false, ""));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GenAnalyse >> " + ex.Message);
            }
        }

        private static DataSet GetSportContentByClassId(string strConn, string sgId, string classId)
        {
            try
            {
                return SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_sportcontentbyclassid"
                        , new SqlParameter[] {new SqlParameter("@sg_id", sgId)
                                                    , new SqlParameter("@class_id", classId)});
            }
            catch (Exception ex)
            {
                throw new Exception("GetSportContentByClassId>> " + ex.Message);
            }
        }
        private static DataSet  GetSportContentBySgId(string strConn,string sgId,string classId)
        {
            try
            {
                return SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_sportcontentbysgid"
                        , new SqlParameter[] { new SqlParameter("@sg_id", sgId)
                                                    , new SqlParameter("@class_id", classId) });
            }
            catch (Exception ex)
            {
                throw new Exception("GetSportContentBySgId >> " + ex.Message);
            }
        }

    }
}
