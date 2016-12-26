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
using System.Data;
using isport_service;
namespace isport_kissmodel
{
    public partial class index : PageBase
    {
        MobileUtilities mU = null;
        private static string projectName = "kissmodel";
        private XDocument rtnXMl;

        public override void GenHeader(string optCode, string projectType)
        {
            try
            {

                /*if (rtnXMl != null)
                {
                    IEnumerable<XElement> content = (from el in rtnXMl.Root.Elements("Content") where (string)el.Attribute("isFree") == "Y" select el);
                    string s = "", s1 = "";
                    string id = "", id1 = "";
                    foreach (XElement xml in content)
                    {
                        XElement media = xml.Element("Media");
                        if (s == "" && media.Attribute("pic").Value != "")
                        {
                            s = media.Attribute("pic").Value;
                            id = xml.Attribute("id").Value;
                        }
                        else if (s1 == "" && media.Attribute("pic").Value != "")
                        {
                            s1 = media.Attribute("pic").Value;
                            id1 = xml.Attribute("id").Value;
                        }

                    }
                    img1.Attributes.Add("src", s);
                    img1.Attributes.Add("data-src", s);
                    img2.Attributes.Add("src", s1);
                    img2.Attributes.Add("data-src", s1);
                    linkDetail1.HRef = "detail.aspx?id=" + id;
                    linkDetail2.HRef = "detail.aspx?id=" + id1;
                }*/
            }
            catch (Exception ex)
            {
                throw new Exception("Header >>"+ex.Message);
            }

        }
        public override void PreGenContent(string optCode, string projectType)
        {
            try
            {
                if (Request["p"] != null && Request["p"] != "")
                {
                    lblContent.Controls.Add(new isport_service.ServiceWapUI_Content().GenContent(AppCodeKiss.strConn, AppCodeKiss.strConnOracle, "", "0", mU.mobileOPT, Request["p"].ToString(), "index.aspx", "", ""));
                }
                else
                {
                    if (rtnXMl != null)
                    {
                        IEnumerable<XElement> contentActive = (from el in rtnXMl.Root.Elements("Active") select el);
                        IEnumerable<XElement> content = (from el in rtnXMl.Root.Elements("Content") where (string)el.Attribute("type")=="model" select el);

                        // Clip VIP (Subscribe)
                        //lblContent.Controls.AddAt(lblContent.Controls.Count, new LiteralControl("<div class='row'>"));
                        for (int i = 0; i < content.Count<XElement>(); i++)
                        {

                            XElement ele = content.ElementAt(i).Element("Media");
                            if (ele != null)
                            {
                                lblContent.Controls.AddAt(lblContent.Controls.Count, new LiteralControl(
                                                " <div class='work'><a href='inner.aspx?id=" + content.ElementAt(i).Attribute("id").Value + "'>"
                                                + "<img class='media'  src='" + ele.Attribute("pic").Value + "'>"
                                                + "<div class='caption'><div class='work_title'><h1>Kiss Model Sexy Girls</h1><p>" + content.ElementAt(i).Attribute("title").Value + "</p></div></div></a></div>"));
                            }
                        }
                        //lblContent.Controls.AddAt(lblContent.Controls.Count, new LiteralControl("</div>"));

                        // Privilege
                        /*lblContent.Controls.AddAt(lblContent.Controls.Count, new LiteralControl("<div class='row'>"));
                        content = (from el in rtnXMl.Root.Elements("Privilege") select el);
                        foreach (XElement xml in content)
                        {
                            lblContent.Controls.AddAt(lblContent.Controls.Count, new LiteralControl(
                            "<div class='col-sm-6 col-md-4 col-xs-12'><div class='thumbnail'><a href='" + xml.Attribute("url").Value + "'>"
                            + "<img src='" + xml.Attribute("image1").Value + "'>"
                            + "<div class='caption'>" + xml.Attribute("title").Value + "</div></a></div></div>"));
                        }

                        lblContent.Controls.AddAt(lblContent.Controls.Count, new LiteralControl("</div>"));*/


                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Content >> "+ex.Message);
            }
        }
        public override void GenFooter(string optCode, string projectType)
        {
            lblFooter.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppCodeKiss.strConn, optCode, "kiss", "", "index.aspx", Level.ToString()));
           //lblFooter.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooterListGroup(AppCodeKiss.strConn, optCode, projectName, "", "index.aspx", Level.ToString(), bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));
        }
        public override void Subscribe_portal_log(string msisdn, string userAgent, string sgId, string optCode, string prjId, string mpCode, string scsId)
        {
            isport_service.ServiceWap_Logs.Subscribe_portal_log(AppCodeKiss.strConnOracle, mU.mobileNumber, bProperty_USERAGENT
                , "284", mU.mobileOPT, "60", bProperty_MPCODE, "");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (Request["s"] != null)
                    {
                        // service jza
                        DataSet ds = ServiceTded.GetSipContentByPcntId(AppCodeKiss.strConnPack, Request["s"]);
                        string fileName = HttpUtility.UrlPathEncode(ds.Tables[0].Rows[0]["pcnt_detail"].ToString());
                        string url = string.Format(ConfigurationManager.AppSettings["URLClip"], fileName);
                        
                        if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().IndexOf("iphone") > -1)
                        {
                            url = string.Format(ConfigurationManager.AppSettings["URLClipiPhone"], ds.Tables[0].Rows[0]["pcnt_detail"].ToString());
                        }
                        Response.Redirect(url, false);
                        //Response.Write(url);
                    }
                    else
                    {

                        string xml = new isportWS.push().SendGet(String.Format(ConfigurationManager.AppSettings["APIKISSMODELMAIN"], DateTime.Now.ToString("MMddyyyy")));
                        rtnXMl = XDocument.Parse(xml);

                        Session["KissContent"] = rtnXMl;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Page Load >> "+ex.Message);
            }
        }
    }
}