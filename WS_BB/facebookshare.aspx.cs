using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
namespace WS_BB
{
    public partial class facebookshare : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request["id"] != null)
            {
                // GenNews
                DataSet ds = new AppCode_News().Command_GetNewsById(Request["id"].ToString());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    Page.Title =  ds.Tables[0].Rows[0]["news_header_th"].ToString();
                    lblTitle.Text = ds.Tables[0].Rows[0]["news_title_th"].ToString();
                    lblHeader.Text = ds.Tables[0].Rows[0]["news_header_th"].ToString();
                    lblDetail.Text = ds.Tables[0].Rows[0]["news_detail_th1"].ToString() + ds.Tables[0].Rows[0]["news_detail_th2"].ToString();
                    img.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["ApplicationURLImages"] + "325x200/" + ds.Tables[0].Rows[0]["news_images_350"].ToString();


                    HtmlMeta meta = new HtmlMeta();
                    meta.Content = ds.Tables[0].Rows[0]["news_header_th"].ToString();
                    meta.Name = "title";
                    Page.Header.Controls.Add(meta);

                    meta = new HtmlMeta();
                    meta.Content = ds.Tables[0].Rows[0]["news_title_th"].ToString();
                    meta.Name = "description";
                    Page.Header.Controls.Add(meta);

                }

            }
            //else Response.Redirect("http://wap.isport.co.th");
        }
    }
}
