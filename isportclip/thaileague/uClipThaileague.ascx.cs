using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mobile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using MobileLibrary;
namespace isportclip
{
    public partial class uClipThaileague : System.Web.UI.MobileControls.MobileUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        public void uDataBindByTeam(string teamCode,string redirectURL)
        {
            try
            {
                // Bind By Team Code
                //this.Controls.AddAt(this.Controls.Count, Utilities.Lable(Utilities.DatetoText(contentDate), true, "Left"));
                DataTable dt = AppCodeThaileague.SelectByTeamCode(teamCode);
                string url = "";

                foreach (DataRow dr in dt.Rows)
                {
                    this.Controls.AddAt(this.Controls.Count, Utilities.Images("images/ball.gif", false, "", "Left"));
                    url = redirectURL + "&cdate=" + dr["content_date"].ToString()
                        + "&cscat=" + dr["cscat_id"].ToString()
                        + "&cid=" + dr["clip_id"].ToString();
                    this.Controls.AddAt(this.Controls.Count, Utilities.Link(dr["title"].ToString(), true, url, "Left"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void uDataBind(string catId,string redirectURL)
        {
            try
            {
                // Bind Get Day
                DataTable dt = AppCodeThaileague.SelectByCcatID(catId, "");
                string url = "";
                foreach(DataRow dr in dt.Rows )
                {
                    this.Controls.AddAt(this.Controls.Count, Utilities.Images("images/ball.gif", false, "", "Left"));
                    url = redirectURL + "&cdate=" + dr["content_date"].ToString();
                    this.Controls.AddAt(this.Controls.Count, Utilities.Link(Utilities.DatetoText(dr["content_date"].ToString()), true, url, "Left"));
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// แสดงได้มากกว่า 1 Sub category
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="contentDate"></param>
        /// <param name="subCate"></param>
        /// <param name="redirectURL"></param>
        public void uDataBind(string catId,string contentDate,string[] subCate, string redirectURL)
        {
            try
            {
                // Bind GetContent by subCat 
                this.Controls.AddAt(this.Controls.Count, Utilities.Lable(Utilities.DatetoText(contentDate), true, "Left"));
                DataTable dt = AppCodeThaileague.SelectByCcatID(catId, contentDate);
                string url = "",obj="";
                for (int index = 0; index < subCate.Length;index++ )
                {
                    DataView dv = AppCodeThaileague.FiilterBySubCat(dt.DefaultView, subCate[index]);

                    #region คลิปล่าสุด
                    this.Controls.AddAt(this.Controls.Count, Utilities.Lable("คลิปล่าสุด", true, "Left"));
                    for (int i = 0;  i < dv.Count;i++ )
                    {
                        if (i < 5)
                        {
                            this.Controls.AddAt(this.Controls.Count, Utilities.Images("images/ball.gif", false, "", "Left"));
                            url = redirectURL + "&cdate=" + contentDate
                                + "&cid=" + dv[i]["clip_id"].ToString();
                            this.Controls.AddAt(this.Controls.Count, Utilities.Link(dv[i]["title"].ToString(), true, url, "Left"));
                        }

                    }
                    url = redirectURL + "&cdate=" + contentDate
                            + "&cscat=" + subCate[index];
                    if (dv.Count > 5) this.Controls.AddAt(this.Controls.Count, Utilities.Link("ดูคลิปทั้งหมด", true, url, "Left"));
                    #endregion

                    #region คลิปช็อตเด็ดประจำวัน
                    this.Controls.AddAt(this.Controls.Count, Utilities.Newline());
                    this.Controls.AddAt(this.Controls.Count, Utilities.Lable("คลิปช็อตเด็ดประจำวัน", true, "Left"));
                    for (int i = 0; i < dv.Count; i++)
                    {
                        if (obj=="" || obj != dv[i]["msch_id"].ToString())
                        {
                            this.Controls.AddAt(this.Controls.Count, Utilities.Images("images/ball.gif", false, "", "Left"));
                            url = redirectURL + "&cdate=" + contentDate
                                + "&mschid=" + dv[i]["msch_id"].ToString();
                            this.Controls.AddAt(this.Controls.Count, Utilities.Link(
                                dv[i]["teamcode1"].ToString() + "-" + dv[i]["teamcode2"].ToString()
                                , true, url, "Left"));
                        }
                        obj = dv[i]["msch_id"].ToString();
                    }
                    url = redirectURL + "&cdate=" + contentDate
                            + "&cscat=" + subCate[index];
                    this.Controls.AddAt(this.Controls.Count, Utilities.Link("แสดงทุกทีมที่เตะวันนี้", true, url, "Left"));
                    #endregion
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void uDataBind(string catId, string contentDate,string subCate, string redirectURL)
        {
            try
            {
                // Bind GetContentAll by subCat
                this.Controls.AddAt(this.Controls.Count, Utilities.Lable(Utilities.DatetoText(contentDate), true, "Left"));
                DataTable dt = AppCodeThaileague.SelectByCcatID(catId, contentDate);
                string url = "";
                DataView dv = AppCodeThaileague.FiilterBySubCat(dt.DefaultView, subCate);
                for (int i = 0; i < dv.Count; i++)
                {
                    this.Controls.AddAt(this.Controls.Count, Utilities.Images("images/ball.gif", false, "", "Left"));
                    url = redirectURL + "&cdate=" + contentDate
                        + "&cscat=" + subCate
                        + "&cid=" + dv[i]["clip_id"].ToString();
                    this.Controls.AddAt(this.Controls.Count, Utilities.Link(dv[i]["title"].ToString(), true, url, "Left"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void uDataBind(string catId, string contentDate, int mschID, string redirectURL)
        {
            try
            {
                // Bind GetContentAll by subCat
                this.Controls.AddAt(this.Controls.Count, Utilities.Lable(Utilities.DatetoText(contentDate), true, "Left"));
                DataTable dt = AppCodeThaileague.SelectByMSCHID(mschID.ToString());
                string url = "";
                foreach(DataRow dr in dt.Rows )
                {
                    this.Controls.AddAt(this.Controls.Count, Utilities.Images("images/ball.gif", false, "", "Left"));
                    url = redirectURL + "&cdate=" + contentDate
                        + "&cid=" + dr["clip_id"].ToString();
                    this.Controls.AddAt(this.Controls.Count, Utilities.Link(dr["title"].ToString(), true, url, "Left"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void uDataBind(string catId, string contentDate, string subCate, string clipID, string redirectURL)
        {
            try
            {
                // Bind GetContent by clipID
                this.Controls.AddAt(this.Controls.Count, Utilities.Lable(Utilities.DatetoText(contentDate), true, "Left"));
                DataTable dt = AppCodeThaileague.SelectByClipID(clipID);
                foreach(DataRow dr in dt.Rows )
                {
                    this.Controls.AddAt(this.Controls.Count, Utilities.Lable(dr["cscat_name"].ToString(), true, "Left"));
                    this.Controls.AddAt(this.Controls.Count, Utilities.Lable(dr["title"].ToString(), true, "Left"));
                    this.Controls.AddAt(this.Controls.Count, Utilities.Lable("Description : ", true, "Left"));
                    this.Controls.AddAt(this.Controls.Count, Utilities.Lable(dr["Description"].ToString().Replace("\r\n",""), true, "Left"));
                    this.Controls.AddAt(this.Controls.Count, Utilities.Images(
                        ConfigurationManager.AppSettings["IsportPathPic"] + "/" + dr["cscat_id"].ToString() +"/"+ dr["pic_path"].ToString()
                        , true
                        , ConfigurationManager.AppSettings["IsportPathClip"] + "/" + dr["cscat_id"].ToString() + "/" + dr["clip_path"].ToString(), "Center"));

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
