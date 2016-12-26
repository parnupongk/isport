using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web;
using Microsoft.ApplicationBlocks.Data;
using System.Xml;
using System.Xml.Linq;

namespace isport_service
{
    public class ServiceTopScore
    {
        public static void GenFootballTopScore(string strConn, Control ctr, string classId, string projectType, bool isHeader, string pageRedirect)
        {
            try
            {
                DataSet dsClass = ServiceTable.FootballClass(strConn, classId);
                string className = dsClass.Tables.Count > 0 && dsClass.Tables[0].Rows.Count > 0 ? dsClass.Tables[0].Rows[0]["class_name_local"].ToString() : "";
                ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl(@"<div class='footballtablehead'><h4>" + className + "</h4></div>"));

               
                DataSet ds = FootballTopScore(strConn, classId);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string str = "";
                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl(@"<div class='footballtablehead'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>ลำดับ</div>
                                                                                                                                                            <div class=col-xs-3 col-sm-3 col-lg-3 col-md-3>ทีม</div>
                                                                                                                                                            <div class=col-xs-3 col-sm-3 col-lg-3 col-md-3>ชื่อ</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>ประตู</div></div>"));
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        str += "<div class='footballtable'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" +dr["row"] + "</div>";
                        str += "<div class=col-xs-3 col-sm-3 col-lg-3 col-md-3>" + dr["team_name"] + "</div>";
                        str += "<div class=col-xs-3 col-sm-3 col-lg-3 col-md-3>" + dr["PLAYER_LNAME"] + "</div>";
                        str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + dr["score"] + "</div></div>";

                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl(str));
                        str = "";
                    }
                }
                else
                {
                    ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genTextHeader("", "ขออภัยไม่พบข้อมูลการแข่งขันค่ะ", pageRedirect, "0", projectType, false, ""));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GenFootballTopScore>>" + ex.Message);
            }
        }

        private static DataSet FootballTopScore(string strConn, string classId)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_getfootballtopscorebyclassid"
                    , ds, new string[] { },
                    new SqlParameter[] { new SqlParameter("@class_id", classId) });
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("FootballTopScore>>" + ex.Message);
            }
        }
    }
}
