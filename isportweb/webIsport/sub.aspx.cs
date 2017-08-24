using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using WebLibrary;
namespace webIsport
{
    public partial class sub : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string projectType = Request["p"] == null || Request["p"] == "" ? "isportweb" : Request["p"];

                    if (Request["p"] == null || Request["p"] == "")
                    {
                        GenNews_fromSQL();//GenNews();
                        GenNewsFootballThai();
                        //lblAds.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, "", "isportweb", "", "detail.aspx", "0"));
                        lblAds.Controls.Add(new isport_service.ServiceWapUI_Content().GenContent(AppMain.strConn
                         , AppMain.strConnOracle
                        , Request["mid"] == null ? "0" : Request["mid"]
                        , Request["level"] == null ? "0" : Request["level"]
                        , "", "result", "", "0", Request["class_id"]));
                    }
                    else
                    {
                        divNews.Visible = false;
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
        private void GenNews_fromSQL()
        {
            try
            {
                string url = "", headNews = "", detailNews = "", imgURL = "";
                string[] detail = new string[] { };
                DataSet ds = new isport_service.AppCode().SelectUIByLevel_Wap(AppMain.strConn, "0", "0", "", "newsmain");
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "content_createdate desc";
                int index = 0;
                foreach (DataRowView dr in dv)
                {
                    //DataRow dr = ds.Tables[0].Rows[index];

                    imgURL = dr["content_image"] == null || dr["content_image"].ToString() == "" ? imgURL : dr["content_image"].ToString();
                    imgURL = imgURL.IndexOf("http") > -1 ? imgURL : "http://wap.isport.co.th/isportui/" + imgURL.Replace("~/", "");

                    url = "detail.aspx?" + "_id=" + dr["content_id"].ToString() + ((HttpContext.Current.Request.QueryString.ToString() == "") ? "" : "&" + HttpContext.Current.Request.QueryString);//"&sg=" + dr["ui_sg_id"].ToString()
                    // + "&class_id=" + classId;//+ "&scs_id=" + bProperty_SCSID + "&mp_code=" + bProperty_MPCODE + "&prj=" + bProperty_PRJ + "&p=" + Request["p"];
                    detail = dr["content_text"].ToString().Split('|');
                    if (index == 0)
                    {
                        headNews = "<div class='thumbnail'><a href='" + url + "'>"
                            + "<img class='img-full' src='" + imgURL + "'>"
                            + "<div class='caption'><h4 class='media-heading'>" + ((detail.Length > 0) ? detail[0] : "") + "</h4></a>"
                            + "<p>" + ((detail.Length > 1) ? detail[1].Substring(0, 200) : dr["content_text"].ToString()) + "</p>"
                            + "<small-gray><i class='fa fa-clock-o fa-1' aria-hidden='true'></i>" + DateTime.ParseExact(dr["content_createdate"].ToString(), "M/d/yyyy h:mm:ss tt", null).ToString("d/MMM/yy H:s") + "</small-gray></div></div>";



                    }
                    else
                    {
                        detailNews += "<div class='media'><a class='pull-left' href='" + url + "'>"
                            + "<img src='" + imgURL + "'></a>"
                            + "<div class='media-body'><h4 small>" + ((detail.Length > 0) ? detail[0] : "") + "</h4 small>"
                            + "<div class='media-button'><small-gray><i class='fa fa-clock-o fa-1' aria-hidden='true'></i>" + DateTime.ParseExact(dr["content_createdate"].ToString(), "M/d/yyyy h:mm:ss tt", null).ToString("d/MMM/yy H:s") + "</small-gray></div></div></div>";

                    }

                    index++;
                }

                lblNews.Controls.AddAt(lblNews.Controls.Count, new LiteralControl("<div class='col-md-5'>" + headNews + "</div>"));
                lblNews.Controls.AddAt(lblNews.Controls.Count, new LiteralControl("<div class='col-md-4'>" + detailNews + "</div>"));
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GenNews_fromSQL >> " + ex.Message);
            }
        }
        private void GenNews()
        {
            try
            {
                string url = "",classId="",headNews="",detailNews="";
                classId = (classId == null || classId == "" ? "1" : classId);
                string[] str = classId.Split(',');
                classId = str.Length > 0 ? str[0] : classId;
                DataTable dt = new isport_service.ServiceNews().News_SelectOracle(AppMain.strConnOracle, "6", classId);
                for ( int index=0;index<dt.Rows.Count;index++)
                {
                     DataRow drContent = dt.Rows[index];
                     url = "detail.aspx?" + "pcnt_id=" + drContent["news_id"].ToString() + ((HttpContext.Current.Request.QueryString.ToString() == "") ? "" : "&" + HttpContext.Current.Request.QueryString);//"&sg=" + dr["ui_sg_id"].ToString()
                    // + "&class_id=" + classId;//+ "&scs_id=" + bProperty_SCSID + "&mp_code=" + bProperty_MPCODE + "&prj=" + bProperty_PRJ + "&p=" + Request["p"];

                    if (index == 0)
                    {
                        headNews = "<div class='thumbnail'><a href='" + url + "'>"
                            + "<img class='img-full' src='http://sms-gw.samartbug.com/isportimage/images/500x300/" + drContent["news_images_600"].ToString() + "'>"
                            + "<div class='caption'><h4 class='media-heading'>" + drContent["news_header_th"].ToString() + "</h4></a>"
                            + "<p>" + drContent["news_title_th"].ToString() + "</p>"
                            + "<small-gray><i class='fa fa-clock-o fa-1' aria-hidden='true'></i>" + DateTime.ParseExact(drContent["news_createdate"].ToString(), "M/d/yyyy h:mm:ss tt", null).ToString("d/MMM/yy H:s") + "</small-gray></div></div>";



                    }
                    else
                    {
                        detailNews += "<div class='media'><a class='pull-left' href='" + url + "'>"
                            + "<img src='http://sms-gw.samartbug.com/isportimage/images/120x75/" + drContent["news_images_190"].ToString() + "'></a>"
                            + "<div class='media-body'><h4 small>" + drContent["news_header_th"].ToString() + "</h4 small>"
                            + "<div class='media-button'><small-gray><i class='fa fa-clock-o fa-1' aria-hidden='true'></i>" + DateTime.ParseExact(drContent["news_createdate"].ToString(), "M/d/yyyy h:mm:ss tt", null).ToString("d/MMM/yy H:s") + "</small-gray></div></div></div>";

                    }


                }

                //lblNews.Controls.AddAt(lblNews.Controls.Count, new LiteralControl("<div class='row'><div class='page-header'><h1><img src='images/news-icon.png' alt='isport'/>ข่าว</h1></div>"));
                lblNews.Controls.AddAt(lblNews.Controls.Count, new LiteralControl("<div class='col-md-5'>" + headNews + "</div>"));
                lblNews.Controls.AddAt(lblNews.Controls.Count, new LiteralControl("<div class='col-md-4'>"+ detailNews +"</div>"));
                //lblNews.Controls.AddAt(lblNews.Controls.Count, new LiteralControl("<div class='col-md-3'><div class='fb - page' data-href='https://www.facebook.com/isportclub' data-small-header='false' data-adapt-container-width='true' data-hide-cover='false' data-show-facepile='true' data-show-posts='false'><div class='fb-xfbml-parse-ignore'><blockquote cite='https://www.facebook.com/isportclub'><a href='https://www.facebook.com/isportclub'>Isportclub</a></blockquote></div></div>  </div>"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GenNewsFootballThai()
        {
            try
            {
                string url = "";
                lblNewsFbThai.Controls.AddAt(lblNewsFbThai.Controls.Count, new LiteralControl("<div class='row'><div class='page-header'><h2><img src='images/news-icon.png' alt='isport'/>ข่าว ฟุตบอลไทย</h2></div>"));
                DataTable dt = new isport_service.ServiceNews().News_SelectOracle(AppMain.strConnOracle, "6", "42");
                for (int index = 0; index < dt.Rows.Count; index++)
                {
                    DataRow drContent = dt.Rows[index];
                    url = "detail.aspx?" + "pcnt_id=" + drContent["news_id"].ToString() + ((HttpContext.Current.Request.QueryString.ToString() == "") ? "" : "&" + HttpContext.Current.Request.QueryString);//"&sg=" + dr["ui_sg_id"].ToString()
                    lblNewsFbThai.Controls.AddAt(lblNewsFbThai.Controls.Count, new LiteralControl("<div class='col-xs-12 col-md-4 col-sm-6'>"));
                    lblNewsFbThai.Controls.AddAt(lblNewsFbThai.Controls.Count, new LiteralControl("<div class='thumbnail'><a href='" + url + "'>"
                            + "<img class='img-full' src='http://sms-gw.samartbug.com/isportimage/images/500x300/" + drContent["news_images_600"].ToString() + "'>"
                            + "<div class='caption'><h5 class='media-heading'>" + drContent["news_header_th"].ToString() + "</h5></a>"
                            + "<p>" + ((drContent["news_title_th"].ToString().Length > 90) ? drContent["news_title_th"].ToString().Substring(0,90) : drContent["news_title_th"].ToString()) + "...</p>"
                            + "<small-gray><i class='fa fa-clock-o fa-1' aria-hidden='true'></i>" + DateTime.ParseExact(drContent["news_createdate"].ToString(), "M/d/yyyy h:mm:ss tt", null).ToString("d/MMM/yy H:s") + "</small-gray></div></div>"));

                    lblNewsFbThai.Controls.AddAt(lblNewsFbThai.Controls.Count, new LiteralControl("</div>"));

                }
                lblNewsFbThai.Controls.AddAt(lblNewsFbThai.Controls.Count, new LiteralControl("</div"));

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }
        }
    }
}