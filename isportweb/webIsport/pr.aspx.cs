using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebLibrary;
namespace webIsport
{
    public partial class pr : System.Web.UI.Page
    {

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if(Request["p"] ==null  )
            this.MasterPageFile = "~/master_isport.master";
            else this.MasterPageFile = "~/master_isport_sub.master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string projectType = Request["p"] == null || Request["p"] == "" ? "isportweb_pr" : Request["p"];

                    if (Request["p"] == null || Request["p"] == "")
                    {
                        divfbPage.Attributes.Add("style", "display:none;");
                        divContent.Attributes.Add("class", "col-md-12");
                    }
                    else
                    {
                        GenAds();
                        CreateMataData(projectType);
                    }




                    lblContent.Controls.Add(new isport_service.ServiceWapUI_Content().GenContent(AppMain.strConn
                    , AppMain.strConnOracle
                    , Request["mid"] == null ? "0" : Request["mid"]
                    , Request["level"] == null ? "0" : Request["level"]
                    , "", projectType, "http://www.isport.co.th/check.aspx?", "0", Request["class_id"]));


                    isport_service.ServiceWap_Logs.Subscribe_portal_log(AppMain.strConnOracle, "", Request.ServerVariables["HTTP_USER_AGENT"], "", ""
                        , "41", "", "");
                }
                catch (Exception ex)
                {
                    ExceptionManager.WriteError(ex.Message);
                }
            }
        }


        private void CreateMataData(string projectType)
        {
            try {
                DataSet ds = new isport_service.AppCode().SelectUIByLevel_Wap(AppMain.strConn, Request["mid"] == null ? "0" : Request["mid"]
                , Request["level"] == null ? "0" : Request["level"]
                , "", projectType);
                 
                string strTitle = "", strDesc = "", strImg = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                    {
                        if (strTitle == "")
                            strTitle = dr["content_text"].ToString();
                        else if (strDesc == "")
                            strDesc = dr["content_text"].ToString();
                    }

                    if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                    {
                        if (strImg == "")
                            
                        strImg = dr["content_image"].ToString();
                        strImg = strImg.IndexOf("http") > -1 ? strImg : "http://wap.isport.co.th/isportui/" + strImg.Replace("~/", "");
                    }

                    if (strTitle != "" && strDesc != "" && strImg != "") break;
                }

                var img = "<meta property=\"og:image\" content=\"" + strImg + "\" />";
                var title = "<meta property=\"og:title\" content=\"" + strTitle + "\" />";
                var desc = "<meta property=\"og:description\" content=\"" + strDesc + "\" />";
                var imgsrc = "<link rel='image_src' href='" + strImg + "'/>";
                var url = "<meta content='http://www.isport.co.th/pr.aspx?&p=" + Request["p"] + "' name='og:url'>";
                litMeta.Text = img + title + desc + imgsrc + url;
                Page.Title = strTitle;
            }
            catch(Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }

        private void GenAds()
        {
            //lblAds.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, "", "isport", "", "detail.aspx", "0"));
        }

    }

}