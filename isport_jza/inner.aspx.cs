using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobileLibrary;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Configuration;

namespace isport_jza
{
    public partial class inner : PageBase
    {
        private static string projectName = "jza";
        private XDocument rtnXMl;
        private IEnumerable<XElement> content;
        private IEnumerable<XElement> mediaContent;

        public override void GenHeader(string optCode, string projectType)
        {
            lblBanner.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppCodeJza.strConn, optCode, "kiss", "", "index.aspx", Level.ToString()));
        }

        public override void PreGenContent(string optCode, string projectType)
        {
            if (content != null && content.Count<XElement>() > 0)
            {
                IEnumerable<XElement> contentActive = (from el in rtnXMl.Root.Elements("Active") select el);
                XElement ele = content.ElementAt(0);
                Page.Title = ele.Attribute("title").Value;
                lblHeader.Text = ele.Attribute("title").Value;
                lblDetailHeader.Text = MobileLibrary.Utilities.nl2br(ele.Attribute("detail").Value);
                lblDetail1.Text = MobileLibrary.Utilities.nl2br(ele.Attribute("detail1").Value);
                lblDetail2.Text = MobileLibrary.Utilities.nl2br(ele.Attribute("detail2").Value);
                lblDetail3.Text = MobileLibrary.Utilities.nl2br(ele.Attribute("detail2").Value);

                if( mU.mobileNumber != "" )
                {
                    linkSub.HRef =  ele.Attribute("url").Value;//contentActive.ElementAt(0).Attribute("phone").Value;
                }
                else linkSub.HRef = "tel:" + ele.Attribute("phone").Value + ConfigurationManager.AppSettings["MPCODE"];//contentActive.ElementAt(0).Attribute("phone").Value;
                mediaContent = ele.Elements("Media");
                string strPic = "", strVideo = "";
                foreach (XElement xml in mediaContent)
                {
                    if (xml.Attribute("pic").Value != "")
                    {
                        strPic = xml.Attribute("pic").Value;
                    }
                    else if (xml.Attribute("clip").Value != "")
                    {
                        strVideo = xml.Attribute("clip").Value;
                    }
                }
                if(strVideo != ""  )
                lblVideo.Controls.Add(new LiteralControl(@"<video width='100%' controls poster='"+strPic+"' id='videoPlayer'> <source src = '"+strVideo+"' type = 'video/mp4' >Your browser does not support the video tag. </video> "));

            }

        }
        public override void GenFooter(string optCode, string projectType)
        {

            IEnumerable<XElement> contentAll = (from el in rtnXMl.Root.Elements("Content") where (string)el.Attribute("type") == "jza" select el);
            for (int i = 0; i < contentAll.Count<XElement>() && i < 6; i++)
            {
                
                XElement ele = contentAll.ElementAt(i).Element("Media");
                if (ele != null)
                {
                    lblMore.Controls.AddAt(lblMore.Controls.Count, new LiteralControl(
                                    " <div class='work'><a href='inner.aspx?id=" + contentAll.ElementAt(i).Attribute("id").Value + "'>"
                                    + "<img class='media'  src='" + ele.Attribute("pic").Value + "'>"
                                    + "<div class='caption'><div class='work_title'><h1>JZA Model Sexy Girls</h1><p>" + contentAll.ElementAt(i).Attribute("title").Value + "</p></div></div></a></div>"));
                }
            }
        }

        public override void Subscribe_portal_log(string msisdn, string userAgent, string sgId, string optCode, string prjId, string mpCode, string scsId)
        {
            isport_service.ServiceWap_Logs.Subscribe_portal_log(AppCodeJza.strConnOracle, mU.mobileNumber, bProperty_USERAGENT
                , "285", mU.mobileOPT, "61", bProperty_MPCODE, "");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["m"] != null) Response.AppendCookie(new HttpCookie("User-Identity-Forward-msisdn", Request["m"]));
                if (Request["o"] != null && Request["o"] != "") Response.AppendCookie(new HttpCookie("Plain-User-Identity-Forward-msisdn", Request["o"]));
                mU = Utilities.getMISDN(Request);

                if (mU.mobileNumber == "" && Request["o"] == null && Request["m"] == null)
                {
                    Response.Redirect(ConfigurationManager.AppSettings["URLGetMSISDN"] + "/inner.aspx?" + Request.QueryString, false);
                }

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