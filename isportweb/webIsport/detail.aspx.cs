using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using MobileLibrary;
namespace webIsport
{
    public partial class detail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["pcnt_id"] != null && Request["pcnt_id"] != "")
            {
                GenNewsDetail(Request["pcnt_id"], "", Request["p"] == null ? "" : Request["p"]);
                GenAds();
            }
        }

        private void GenAds()
        {
            //lblAds.Controls.Add( new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, "", "isportweb", "", "detail.aspx", "0"));
        }

        #region Gen News Detail
        private void GenNewsDetail(string pcntId, string opt, string projectType)
        {
            try
            {
                string headNews = "";
                string pcnt = pcntId.Split(',').Length > 0 ? pcntId.Split(',')[0] : pcntId;
                DataTable dt = News_SelectByIdOracle(pcnt);
                if (dt.Rows.Count > 0)
                {
                    var img = "<meta property=\"og:image\" content=\"http://sms-gw.samartbug.com/isportimage/images/500x300/" + dt.Rows[0]["news_images_600"].ToString() + "\" />";
                    var title = "<meta property=\"og:title\" content=\"" + dt.Rows[0]["news_header_th"].ToString() + "\" />";
                    var desc = "<meta property=\"og:description\" content=\"" + dt.Rows[0]["news_title_th"].ToString() + "\" />";
                    var imgsrc= "<link rel='image_src' href='http://sms-gw.samartbug.com/isportimage/images/500x300/" + dt.Rows[0]["news_images_600"].ToString() + "'/>";
                    var url = "<meta content='http://www.isport.co.th/detail.aspx?&pcnt_id="+ Request["id"] +"' name='og:url'>";
                    var desc1 = "<meta name='description' content='" + dt.Rows[0]["news_header_th"].ToString() + "' />";
                    litMeta.Text = img + title + desc + imgsrc + url;
                    Page.Title = dt.Rows[0]["news_header_th"].ToString();
                    
                    lblHeader.Text = dt.Rows[0]["news_header_th"].ToString();
                    headNews = ""
                            + "<div class='thumbnail'>"
                            + "<img class='img' src='http://sms-gw.samartbug.com/isportimage/images/500x300/" + dt.Rows[0]["news_images_600"].ToString() + "'>"
                            + "<div class='caption'>"
                            + "<p class='lead'>" + dt.Rows[0]["news_title_th"].ToString() + "</p>"
                            + "<p class='lead'>" + ReplaceTagHTML(dt.Rows[0]["news_detail_th1"].ToString()) + "</p></div></div>";

                    lblNews.Controls.AddAt(lblNews.Controls.Count, new LiteralControl(headNews));
                    //lblNews.Controls.AddAt(lblNews.Controls.Count, new LiteralControl("<div class='row'><p>" + dt.Rows[0]["news_title_th"] + "</p></div>"));
                    //lblNews.Controls.AddAt(lblNews.Controls.Count, new LiteralControl("<div class='row'><p>" + ReplaceTagHTML(dt.Rows[0]["news_detail_th1"].ToString()) + "</p></div>"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GenNewsDetail>>" + ex.Message);
            }
        }

        public DataTable News_SelectByIdOracle(string newsId)
        {
            try
            {
                DataSet ds = null;
                using (OracleConnection conn = new OracleConnection(AppMain.strConnOracle))
                {
                    ds = OracleDataAccress.OrclHelper.Fill(conn, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsById", "siamsport_news"
                        , new OracleParameter[] {OracleDataAccress.OrclHelper.GetOracleParameter("p_newsId",newsId,OracleType.VarChar ,ParameterDirection.Input ) 
                                                             ,OracleDataAccress.OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor ,ParameterDirection.Output ) });
                }
                return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string ReplaceTagHTML(string detail)
        {
            try
            {
                string rtn = detail;
                if (detail.IndexOf("<") > -2)
                {
                    if (detail.IndexOf("<") == 0)
                    {
                        rtn = rtn.Substring(detail.IndexOf(">") + 1);
                    }
                    else if (detail.IndexOf(">") > -1)
                    {
                        rtn = rtn.Substring(0, detail.IndexOf("<") - 1) + rtn.Substring(detail.IndexOf(">") + 1);
                    }
                    else
                    {
                        rtn = rtn.Substring(0, detail.IndexOf("<") - 1);
                    }
                    rtn = ReplaceTagHTML(rtn);
                }
                return rtn;
            }
            catch
            {
                return detail;
            }
        }
        #endregion
    }
}