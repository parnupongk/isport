using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using OracleDataAccress;
using MobileLibrary;
namespace WS_BB
{
    public partial class feednews : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/xml";
            if (!IsPostBack && Request["projectcode"] != null)
            {
                
                GenNews();
            }
        }
        private void GenNews()
        {


            string status = "success", desc = "", header = "", title = "", detail = "", imageS = "", imageM = "", imageL = "";
            try
            {
                string sportType = Request["sporttype"] == null ? "00001" : Request["sporttype"];
                MobileUtilities muMobile = new MobileUtilities();
                new AppCode_Logs().Logs_Insert("FeedNewsToAis", "", "", sportType, muMobile.mobileOPT + "|" + muMobile.mobileNumber, AppCode_Base.AppName.FeedNewToAis.ToString(),"","",muMobile.mobileNumber,"",muMobile.mobileOPT,AppCode_Base.GETIP(),"");
                DataSet ds = new AppCode_News().CommandGetNewsByImagesCopyright(sportType, "10", "th", "");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int index = new Random().Next(0, 9);
                    DataRow dr = ds.Tables[0].Rows[index];
                    header = dr["news_header_th"].ToString();
                    title = dr["news_title_th"].ToString();
                    detail = AppCode_News.ReplaceTagHTML( dr["news_detail_th1"].ToString());
                    imageS = ConfigurationManager.AppSettings["ApplicationURLImages"] + "120x75/" + dr["news_images_190"].ToString();
                    imageM = ConfigurationManager.AppSettings["ApplicationURLImages"] + "325x200/" + dr["news_images_350"].ToString();
                    imageL = ConfigurationManager.AppSettings["ApplicationURLImages"] + "500x300/" + dr["news_images_600"].ToString();
                    detail += ConfigurationManager.AppSettings["wordingFooterFeedNews"];
                }
                
            }
            catch (Exception ex)
            {
                status = "error";
                desc = ex.Message;
            }
            Response.Write("<?xml version='1.0' encoding='UTF-8'?>");
            Response.Write("<isportapp>");
            Response.Write("<header>" + header + "</header>");
            Response.Write("<title>" + title + "</title>");
            Response.Write("<detail>" + detail + "</detail>");
            Response.Write("<images_s> " + imageS + "</images_s>");
            Response.Write("<images_m>" + imageM + "</images_m>");
            Response.Write("<images_l>" + imageL + "</images_l>");
            Response.Write("<status>" + status + "</status>");
            Response.Write("<status_message>" + desc + "</status_message>");
            Response.Write("</isportapp>");
        }
    }
}
