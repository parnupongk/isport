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
    public partial class football_tded_team : PageBase
    {

        public override void GenHeader(string optCode, string projectType)
        {
            frmMain.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, mU.mobileOPT, "bb", "", "indexl.aspx", "0"));
        }
        public override void PreGenContent(string optCode, string projectType)
        {
            // Request["pcnt_id"] บริการทีเด็ดสปอร์ตแมน 3B/sms, ทีเด็ดฟุตบอล 10B/sms
            // Request["m_id"] บริการ ทีเด็ดจ่าฝูง

            string check = "Y";
            check   =  (Request["pcnt_id"] ==null || Request["pcnt_id"] == "")? isport_service.ServiceSMS.MemberSms_CheckActive(AppMain.strConnPack, Request["pssv_id"], mU.mobileOPT, mU.mobileNumber) : "Y";
            if (check == "Y")
            {
                DataSet ds = (Request["pcnt_id"] ==null || Request["pcnt_id"] == "")? isport_service.ServiceTded.GetSipContentByPcntDetail(AppMain.strConnPack, Request["m_id"])
                                                                                                                        : isport_service.ServiceTded.GetSipContentByPcntId(AppMain.strConnPack,Request["pcnt_id"]);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (Request["m_id"] != null && Request["m_id"] != "")
                    {
                        bProperty_SGID = "267";
                        string sql = "select dbo.ufn_BB_display_match_result(" + Request["m_id"] + ",'L','" + DateTime.Now + "');";
                        object obj = SqlHelper.ExecuteScalar(AppMain.strConn, CommandType.Text, sql);
                        if (obj != null)
                        {
                            string[] str = obj.ToString().Split('|');
                            frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'>" + str[1] + " " + str[0] + " " + str[3] + "</div></div>"));
                        }
                    }
                    else
                    {
                        // sportman pcat = 214
                        // tded football pcat = 238

                        bProperty_SGID = (ds.Tables[0].Rows[0]["pcat_id"].ToString() == "214")? "270" : "271" ; // ต้องแยก บริการทีเด็ดสปอร์ตแมน 3B/sms  และ ทีเด็ดฟุตบอล 10B/sms
                    }

                    frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", "ความน่าจะเป็นของเกม ", "football_tded_team.aspx", "0", "", false, ""));
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(@"<div class='row featurette'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>
                                                                                            </div><div class=col-xs-11 col-sm-11 col-lg-11 col-md-11>" + ds.Tables[0].Rows[0]["pcnt_detail_local"].ToString() + "</div></div>"));
                    // pcnt_title_local = ส่ง sms

                    frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", "ผลการแข่งขันที่คาด ", "football_tded_team.aspx", "0", "", false, ""));
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(@"<div class='row featurette'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1>
                                                                                            </div><div class=col-xs-11 col-sm-11 col-lg-11 col-md-11>" + ds.Tables[0].Rows[0]["pcnt_detail"].ToString() + "</div></div>"));

                }
            }
            else
            {
                frmMain.Controls.Add(ServiceWapUI_GenControls.genText("", "ขออภัยค่ะ กรุณาสมัครบริการก่อนเข้าใช้งานค่ะ", "", "0", "tdedtopteam_sub", false, ""));
                frmMain.Controls.Add(new isport_service.ServiceWapUI_Content().GenContent(AppMain.strConn, "", "0", "0", mU.mobileOPT, "tdedtopteam_sub", "", "football_tded_team.aspx", "0"));
            }
        }
        public override void GenFooter(string optCode, string projectType)
        {
            frmMain.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooter(AppMain.strConn, mU.mobileOPT, "isport", "", "indexl.aspx", "0", bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));
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
