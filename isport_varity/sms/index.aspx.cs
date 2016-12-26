using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using isport_service;
using MobileLibrary;
namespace isport_varity.sms
{
    public partial class index : PageBase
    {
        public override void GenHeader(string optCode, string projectType)
        {
            frmMain.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppCode.strIsportConn, mU.mobileOPT, "taijai", "", "index.aspx", "0"));
        }
        public override void GenFooter(string optCode, string projectType)
        {
            frmMain.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooter(AppCode.strIsportConn, mU.mobileOPT, "varity", "", "index.aspx", "0", bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));
        }
        public override void PreGenContent(string optCode, string projectType)
        {
            string title = "", detail = "";
            DataSet ds = new isport_service.AppCode().SelectUIByLevel_Wap(AppCode.strIsportConn, "0", "0", mU.mobileOPT, Request["p"]);
            if(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 )
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (i == 0) title = ds.Tables[0].Rows[i]["content_text"].ToString();
                    else detail += "<div class='rowempty'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1><img src='image/" + i.ToString() + ".png'/></div><div class=col-xs-11 col-sm-11 col-lg-1 col-md-11>" + ds.Tables[0].Rows[i]["content_text"].ToString() + "</div></div>";
                }
            }

            frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(@"<div class='panel panel-danger'><div class='panel-heading'><h3 class='panel-title'><img src='image/head.gif'/>" + title + "</h3></div><div class='panel-body'>" + detail + "</div></div>"));
            
        }
        public override void Subscribe_portal_log(string msisdn, string userAgent, string sgId, string optCode, string prjId, string mpCode, string scsId)
        {
            isport_service.ServiceWap_Logs.Subscribe_portal_log(AppCode.strIsportOraConn, mU.mobileNumber, bProperty_USERAGENT
                            , "268", mU.mobileOPT, "1", "0025", "");
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
