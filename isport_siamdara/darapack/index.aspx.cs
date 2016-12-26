using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using MobileLibrary;
namespace isport_siamdara.darapack
{
    public partial class index : PageBase
    {

        public override void GenHeader(string optCode, string projectType)
        {
            frmMain.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString(), mU.mobileOPT, "", "", "index.aspx", "0"));
        }
        public override void PreGenContent(string optCode, string projectType)
        {
            // sg ต้องใส่ที่ link เลย
            string link = "http://wap.isport.co.th/isportui/redirect.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
        + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
        + "&scs_id=" + bProperty_SCSID;
            link += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";

            string pageMaster = "index.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
        + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
        + "&sg=" + bProperty_SGID + "&scs_id=" + bProperty_SCSID;
            pageMaster += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";
            pageMaster += "&p=" + projectType;

            frmMain.Controls.Add(new isport_service.ServiceWapUI_Content().GenContent(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString()
                , ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString()
                , Request["mid"] == null ? "0" : Request["mid"]
                , Request["level"] == null ? "0" : Request["level"]
                , mU.mobileOPT, projectType, link, pageMaster, Request["class_id"]));
        }
        public override void GenFooter(string optCode, string projectType)
        {
            frmMain.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooter(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString(), mU.mobileOPT, "siamdara", "", "index.aspx", "0", "0025", "57", "", ""));
        }
        public override void Subscribe_portal_log(string msisdn, string userAgent, string sgId, string optCode, string prjId, string mpCode, string scsId)
        {
            isport_service.ServiceWap_Logs.Subscribe_portal_log(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString(), mU.mobileNumber, userAgent
                                , bProperty_SGID, mU.mobileOPT, "57", "0025", "");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            bProperty_SGID = "283";
        }
    }
}