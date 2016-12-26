using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using OracleDataAccress;
using MobileLibrary;
using isport_service;
using WebLibrary;
using Newtonsoft.Json;
namespace isport_edt
{
    public partial class index : PageBase
    {

        public override void GenHeader(string optCode, string projectType)
        {
            lblbanner.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, optCode, projectType, "", "index.aspx", "0"));

            if (projectType != "")
            {
                string link = "http://wap.isport.co.th/isportui/redirect.aspx" + "?mp_code=" + bProperty_MPCODE + "&prj=" + bProperty_PRJ + "&scs_id=" + bProperty_SCSID;
                link += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";

                lblContentSub.Controls.AddAt(lblContentSub.Controls.Count, new isport_service.ServiceWapUI_Content().GenContent(AppMain.strConn, optCode, "0", "0", "", projectType, link, "", ""));
            }

        }
        public override void PreGenContent(string optCode, string projectType)
        {
            try
            {
                string[] ranId = ConfigurationManager.AppSettings["RandomContentId"].ToString().Split(',');
                string id = (Request["category"] == null) ? ranId[new Random().Next(ranId.Length)] : Request["category"];
                DataSet ds = new AppCode_EDT_Conn().CommandGetEDTBySMSId(id);
                Session["EDTDATA"] = ds;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {


                    string ctn = "", urlContent = "", xml = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        // header
                        if (dr["title"].ToString() != "")
                        {
                            ctn = "<h1>" + dr["title"].ToString() + "</h1>";
                            lblTitle.Controls.AddAt(lblTitle.Controls.Count, new LiteralControl(ctn));
                        }
                        try
                        {
                            // content
                            urlContent = string.Format(ConfigurationManager.AppSettings["URLCONTENT"], dr["xml_id"]);
                            xml = new isport_service.sendService.push().SendGet(urlContent);
                            AppCode_EDT edt = JsonConvert.DeserializeObject<AppCode_EDT>(xml);
                            ctn = "<div class='media'>";
                            ctn += "               <a class='pull-left' href='../detail.aspx?cid=" + edt.entry_id + "&sid=" + Request["category"] + "'><img src='" + edt.image_cover_thumb + "' alt='...'></a>";
                            ctn += "               <div class='media-body'>";
                            ctn += "                    <h4 class='media-heading'>" + edt.name + "</h4>";
                            ctn += "                    <p>" + edt.detail.Substring(0, (edt.detail.Length > 150) ? 150 : edt.detail.Length) + "</p>";
                            ctn += "</div></div>";

                            lblContent.Controls.AddAt(lblContent.Controls.Count, new LiteralControl(ctn));
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.WriteError("PreGenContent >> " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("PreGenContent >> " + ex.Message);
            }
        }
        public override void GenFooter(string optCode, string projectType)
        {
            lblFooter.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooterListGroup(AppMain.strConn, optCode, "edtguide", "", "index.aspx", "0", bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));
        }
        public override void Subscribe_portal_log(string msisdn, string userAgent, string sgId, string optCode, string prjId, string mpCode, string scsId)
        {
           // isport_service.ServiceWap_Logs.Subscribe_portal_log(AppMain.strConnOracle , mU.mobileNumber, bProperty_USERAGENT
           //                 , "282", mU.mobileOPT, "1", bProperty_MPCODE, "");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            bProperty_MPCODE = bProperty_MPCODE == "0025" ? "0804" : bProperty_MPCODE;

            
            if (Request["m"] != null) Response.AppendCookie(new HttpCookie("User-Identity-Forward-msisdn", Request["m"]));
            if (Request["o"] != null && Request["o"] != "") Response.AppendCookie(new HttpCookie("Plain-User-Identity-Forward-msisdn", Request["o"]));
            mU = Utilities.getMISDN(Request);

            if (mU.mobileNumber == "" && Request["o"] == null && Request["m"] == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["URLGetMSISDN"] + "/index.aspx?" + Request.QueryString, false);
            }
        }
    }
}
