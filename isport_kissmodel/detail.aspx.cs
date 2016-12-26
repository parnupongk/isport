using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using MobileLibrary;
using isport_service;
namespace isport_kissmodel
{
    public partial class detail : PageBase
    {
        private static string projectName = "kissmodel";
        private XDocument rtnXMl;
        private IEnumerable<XElement> content;
        private IEnumerable<XElement> mediaContent ;
        
        // fb
        private string fbTitle;
        private string fbDetail;
        private string fbImage;

        private void addMetaFacebook()
        {
            var vimg = "<meta property=\"og:image\" content=\"" + fbImage + "\" />";
            var vtitle = "<meta property=\"og:title\" content=\"" + fbTitle + "\" />";
            var vdesc = "<meta property=\"og:description\" content=\"" + fbDetail + "\" />";
            var vimgsrc = "<link rel='image_src' href='" + fbImage + "'/>";
            var vurl = "<meta content='http://kissmodel.net/detail.aspx?&id=" + Request["id"] + "' name='og:url'>";
            var vdesc1 = "<meta name='description' content='" + fbDetail + "' />";
            litMeta.Text = vimg + vtitle + vdesc + vimgsrc + vurl;
        }

        public override void GenHeader(string optCode, string projectType)
        {
            
        }
        public override void GenFooter(string optCode, string projectType)
        {
            lblFooter.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppCodeKiss.strConn, optCode, "kiss", "", "index.aspx", Level.ToString()));
            addMetaFacebook();
            //lblFooter.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooterListGroup(AppCodeKiss.strConn, optCode, projectName, "", "index.aspx", Level.ToString(), bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));
        }
        public override void PreGenContent(string optCode, string projectType)
        {
            if (content != null && content.Count<XElement>() > 0)
            {

                XElement ele = content.ElementAt(0);
                // profile
                lblContent.Controls.Add(isport_service.ServiceWapUI_GenControls.genTextHeader("", "Profile", "", Level.ToString(), projectName, false, ""));
                lblContent.Controls.Add(isport_service.ServiceWapUI_GenControls.genText("", ele.Attribute("nName").Value, "", Level.ToString(), projectName, false, ""));
                lblContent.Controls.Add(new LiteralControl("<div class='row'><div class='col-xs-4 col-sm-4 col-lg-4'><p>" + ele.Attribute("shape").Value + "</p></div><div class='col-xs-4 col-sm-4 col-lg-4'>" + ele.Attribute("weight").Value + "</div><div class='col-xs-4 col-sm-4 col-lg-4'>" + ele.Attribute("high").Value + "</div></div>"));

                lblContent.Controls.Add(new LiteralControl("<div class='fb_like'><div class='fb-like' data-share='true' data-width='320' data-show-faces='true'></div></div>"));
                
                // Detail
                fbTitle = ele.Attribute("title").Value;
                fbDetail = ele.Attribute("detail").Value;
                lblContent.Controls.Add(isport_service.ServiceWapUI_GenControls.genTextHeader("", ele.Attribute("title").Value, "", Level.ToString(), projectName, false, ""));
                lblContent.Controls.Add(isport_service.ServiceWapUI_GenControls.genText("", ele.Attribute("detail").Value, "", Level.ToString(), projectName, false, ""));

                

                mediaContent = ele.Elements("Media");
                foreach (XElement xml in mediaContent)
                {
                    if (xml.Attribute("pic").Value != "")
                    {
                        fbImage = xml.Attribute("pic").Value;
                        lblContent.Controls.Add(new LiteralControl("<div class='row'><div class='col-xs-12 col-sm-12 col-lg-12'><img class='img-center' src='" + xml.Attribute("pic").Value.ToString() + "'/></div></div>"));
                    }
                    else if (xml.Attribute("clip").Value != "")
                    {
                        lblContent.Controls.AddAt(lblContent.Controls.Count, new LiteralControl("<div class='row_Black_Center'><video width='400' height='auto' controls poster='full/http/link/to/image/file.png'><source src='" + xml.Attribute("clip").Value.Replace("rtsp://203.149.30.25:1935", "http://203.149.53.75") + "' type='video/mp4'/>Your browser does not support the video tag.</video></div>"));
                    }
                }

                lblContent.Controls.Add(isport_service.ServiceWapUI_GenControls.genTextHeader("", "Inter View ", "", Level.ToString(), projectName, false, ""));
                lblContent.Controls.Add(isport_service.ServiceWapUI_GenControls.genText("", ele.Attribute("interview").Value, "", Level.ToString(), projectName, false, ""));

            }
        }
        public override void Subscribe_portal_log(string msisdn, string userAgent, string sgId, string optCode, string prjId, string mpCode, string scsId)
        {
            isport_service.ServiceWap_Logs.Subscribe_portal_log(AppCodeKiss.strConnOracle, mU.mobileNumber, bProperty_USERAGENT
                , "284", mU.mobileOPT, "60", bProperty_MPCODE, "");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["KissContent"] != null)
                {
                    rtnXMl = (XDocument)Session["KissContent"];
                }
                else
                {
                    string xml = new isportWS.push().SendGet(String.Format(ConfigurationManager.AppSettings["APIKISSMODELMAIN"], DateTime.Now.ToString("MMddyyyy")));
                    rtnXMl = XDocument.Parse(xml);
                }

                content = (from el in rtnXMl.Root.Elements("Content") where (string)el.Attribute("id") == Request["id"] select el);

            }
            
        }
    }
}