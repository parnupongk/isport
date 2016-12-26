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
    public class ServiceTable
    {
        #region League Table
        public static void genTable(string strConn, Control ctr, string classId, string projectType, bool isHeader, string pageRedirect)
        {
            try
            {
                
                if (classId !=null && classId != "" )
                {
                    string[] strClassId = classId.Split(',');
                    string className = "";
                    foreach (string strC in strClassId)
                    {
                        DataSet dsClass = FootballClass(strConn, strC);
                        className = dsClass.Tables.Count > 0 && dsClass.Tables[0].Rows.Count > 0 ? dsClass.Tables[0].Rows[0]["class_name_local"].ToString() : "";
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl(@"<div class='rowprogramheader'><h4>" + className + "</h4></div>"));
                        DataSet ds = FootballTable(strConn, strC, "");
 
                        string str = "";
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && classId != "256")
                        {

                            #region write data
                            ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl(@"<div class='rowprogramheader'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>ลำดับ</div>
                                                                                                                                                            <div class=col-xs-3 col-sm-3 col-lg-3 col-md-3>ทีม</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>P</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>W</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>D</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>L</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>F</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>A</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>PTs</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>GD</div>
                                                                                                            </div>"));
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                str += "<div class='footballtable'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + dr["row"] + "</div>";
                                str += "<div class=col-xs-3 col-sm-3 col-lg-3 col-md-3>" + dr["short_name"] + "</div>";
                                str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + dr["total_match"] + "</div>";
                                str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + dr["total_win"] + "</div>";
                                str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + dr["total_draw"] + "</div>";
                                str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + dr["total_loss"] + "</div>";
                                str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + dr["total_f"] + "</div>";
                                str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + dr["total_a"] + "</div>";
                                str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + dr["point"] + "</div>";
                                str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + dr["gd"] + "</div></div>";

                                ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl(str));
                                str = "";
                            }
                            #endregion

                        }
                        else
                        {
                            #region xml Sport CC
                            XDocument xmldoc = FootballTableSportCC(dsClass.Tables[0].Rows[0]["feed_id"].ToString());
                            IEnumerable<XElement> childList = from el in xmldoc.Root.Elements() select el;
                            foreach (XElement e in childList)
                            {
                                if (e.Attribute("tmName") != null)
                                {
                                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl(@"<div class='rowprogramheader'>" + e.Attribute("tmName").Value + "</div>"));
                                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl(@"<div class='rowprogramheader'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>ลำดับ</div>
                                                                                                                                                            <div class=col-xs-3 col-sm-3 col-lg-3 col-md-3>ทีม</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>P</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>W</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>D</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>L</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>F</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>A</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>PTs</div>
                                                                                                                                                            <div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>GD</div></div>"));
                                    IEnumerable<XElement> c = from el in e.Elements() select el;
                                    foreach (XElement x in c)
                                    {
                                        str += "<div class='footballtable'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + x.Attribute("place").Value + "</div>";
                                        str += "<div class=col-xs-3 col-sm-3 col-lg-3 col-md-3>" + x.Attribute("teamName1").Value + "</div>";
                                        str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + x.Attribute("total_play").Value + "</div>";
                                        str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + x.Attribute("total_won").Value + "</div>";
                                        str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + x.Attribute("total_draws").Value + "</div>";
                                        str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + x.Attribute("total_lost").Value + "</div>";
                                        str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + x.Attribute("total_score").Value + "</div>"; // total score
                                        str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + x.Attribute("total_concede").Value + "</div>";
                                        str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + x.Attribute("total_point").Value + "</div>";
                                        str += "<div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>" + x.Attribute("total_diff").Value + "</div></div>";

                                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl(str));
                                        str = "";
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genTextHeader("", "ขออภัยไม่พบข้อมูลการแข่งขันค่ะ", pageRedirect, "0", projectType, false, ""));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("genTable >> " + ex.Message);
            }
        }
        #endregion

        public static DataSet FootballClass(string strConn, string classId)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_sportclassbyclassid"
                    , ds, new string[] { },
                    new SqlParameter[] { new SqlParameter("@class_id", classId) });
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static DataSet FootballTable(string strConn, string classId, string strLng)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_tablebyclassid"
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
        /// 
        /// </summary>
        /// <param name="contentGroupid">Feed ID (isportcc database)</param>
        /// <returns></returns>
        private static XDocument FootballTableSportCC(string contentGroupid)
        {
            try
            {

                string url = "http://wap.isport.co.th/isportws/starsoccer.aspx?ap=StarSoccer&pn=leaguetable&imei=123456789&imsi=android&lang=en&contentgroupid=" + contentGroupid;
                System.Net.ServicePointManager.Expect100Continue = false; 
                string rtn = new sendService.push().SendGet(url);
                XDocument xmldoc = new XDocument();
                xmldoc = XDocument.Parse(rtn);
                return xmldoc;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
