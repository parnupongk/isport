using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using MobileLibrary;
using isport_service;
namespace isport
{
    public partial class football_analyse : PageBase
    {
        public override void GenHeader(string optCode, string projectType)
        {
            frmMain.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, mU.mobileOPT, "bb", "", "indexl.aspx", "0"));
        }
        public override void PreGenContent(string optCode, string projectType)
        {
            string catId = (Request["cat_id"] ==null || Request["cat_id"] == "") ? "00016" : Request["cat_id"];
            DataSet ds = AppCode.GetSportContent("", catId);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string[] analyse = dr["scnt_detail_local"].ToString().Split('|');

                frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", dr["class_name_local"].ToString(), "football_tded_team.aspx", "0", "", false, ""));

                string sql = "select dbo.ufn_BB_display_match_result(" + dr["msch_id"] + ",'L','" + DateTime.Now + "');";
                object obj = SqlHelper.ExecuteScalar(AppMain.strConn, CommandType.Text, sql);
                if (obj != null)
                {
                    string[] str = obj.ToString().Split('|');
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'>" + str[1] + " " + str[0] + " " + str[3] + "</div></div>"));
                }

                //frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'>" + dr["class_name"] + "</div></div>"));
                // Select By Class
                frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", "ความน่าจะเป็นของเกม ", "football_tded_team.aspx", "0", "", false, ""));
                frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(@"<div class='row featurette'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>
                                                                                            </div><div class=col-xs-11 col-sm-11 col-lg-11 col-md-11>" + analyse[0] + "</div></div>"));
                // pcnt_title_local = ส่ง sms
                if (analyse.Length > 0)
                {
                    frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", "ผลการแข่งขันที่คาด ", "football_tded_team.aspx", "0", "", false, ""));
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(@"<div class='row featurette'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>
                                                                                            </div><div class=col-xs-11 col-sm-11 col-lg-11 col-md-11>" + analyse[1] + "</div></div>"));
                }
            }
        }
        public override void GenFooter(string optCode, string projectType)
        {
            frmMain.Controls.Add(new ServiceWapUI_Footer().GenFooter(AppCode.strConn, mU.mobileOPT, bProperty_PRJ, "", "football_analyse.aspx", "0", bProperty_MPCODE, "isport", bProperty_SCSID, ""));
        }
        public override void Subscribe_portal_log(string msisdn, string userAgent, string sgId, string optCode, string prjId, string mpCode, string scsId)
        {
            isport_service.ServiceWap_Logs.Subscribe_portal_log(AppMain.strConnOracle, msisdn, userAgent, sgId, optCode, prjId, mpCode, scsId);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
