using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Runtime.Serialization;
using System.Configuration;
using WebLibrary;
using isport_edt;
using MobileLibrary;
using Newtonsoft.Json;
namespace WS_BB
{
    [DataContract]
    public class Content_EDT
    {
        #region Data
        [DataMember(Name = "linkmore", IsRequired = false)]
        public string linkmore { get; set; }

        [DataMember(Name = "type", IsRequired = false)]
        public string type { get; set; }

        [DataMember(Name = "entry_id", IsRequired = false)]
        public string entry_id { get; set; }

        [DataMember(Name = "name", IsRequired = false)]
        public string name { get; set; }

        [DataMember(Name = "detail", IsRequired = false)]
        public string detail { get; set; }

        [DataMember(Name = "image_cover_thumb", IsRequired = false)]
        public string image_cover_thumb { get; set; }

        [DataMember(Name = "image_cover", IsRequired = false)]
        public string image_cover { get; set; }

        [DataMember(Name = "gallery_thumb", IsRequired = false)]
        public List<string> gallery_thumb { get; set; }

        [DataMember(Name = "gallery", IsRequired = false)]
        public List<string> gallery { get; set; }

        [DataMember(Name = "status", IsRequired = false)]
        public string status { get; set; }

        [DataMember(Name = "status_mess", IsRequired = false)]
        public string status_mess { get; set; }

        #endregion
    }
    [DataContract]
    public class MediaContents
    {
        [DataMember(Name = "MediaContent", IsRequired = false)]
        public List<MediaContent> MediaContent { get; set; }


        [DataMember(Name = "errStatus", IsRequired = false)]
        public string errStatus { get; set; }

        [DataMember(Name = "errMessage", IsRequired = false)]
        public string errMessage { get; set; }

    }
    public class MediaContent
    {
        public string pcntId;
        public string mediaName;
        public string mediaMessage;
        public string mediaImage;
    }
    public partial class feedmedia : System.Web.UI.Page
    {

        protected MobileUtilities muMobile;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                muMobile = Utilities.getMISDN(Request);

                if (Request["ap"] == AppCode_Base.AppName.FeedAis.ToString() && Request["pn"] == "edt")
                {
                    Response.Write(GetFeedEDT());

                    new AppCode_Logs().Logs_Insert(Request["ap"] + "_" + Request["pn"] , "", Request.Url.ToString(), "android", muMobile.mobileOPT + "|" + muMobile.mobileNumber + "|" + Request["imei"] + "|" + Request["imsi"]
                        , Request["ap"], Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
                }
                else if (!IsPostBack && Request["date"] != null)
                {
                    Response.Write(GetContent(Request["date"]));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }
        }

        private string GetFeedEDT()
        {
            List<Content_EDT> edtContents = new List<Content_EDT>();
            Content_EDT edtContent = new Content_EDT();
            try
            {


                string txtDate = DateTime.Now.AddDays(-30).ToString("MM/dd/yyyy");

                System.Data.DataSet ds = new AppCode_EDTContent().CommandGetEDTByDisplayDate(txtDate);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        edtContent = new Content_EDT();
                        string urlContent = string.Format(ConfigurationManager.AppSettings["EDTURLCONTENT"], dr["xml_id"].ToString());
                        string xml = new push().SendGet(urlContent);
                        AppCode_EDT edt = JsonConvert.DeserializeObject<AppCode_EDT>(xml);
                        edtContent.type = edt.type;
                        edtContent.entry_id = edt.entry_id;
                        edtContent.name = edt.name;
                        edtContent.detail = edt.detail;
                        edtContent.image_cover_thumb = edt.image_cover_thumb;
                        edtContent.image_cover = edt.image_cover;
                        edtContent.gallery_thumb = edt.gallery_thumb;
                        edtContent.gallery = edt.gallery;
                        edtContent.linkmore = ConfigurationManager.AppSettings["EDTURLSUBScribe"];
                        edtContent.status = "success";
                        edtContents.Add(edtContent);
                    }


                }
                else
                {
                    edtContent.status = "error";
                    edtContent.status_mess = "not found data";
                    edtContent.type = txtDate;
                    edtContents.Add(edtContent);
                }
            }
            catch (Exception ex)
            {
                edtContent.status = "error";
                edtContent.status_mess = ex.Message;
                edtContents.Add(edtContent);
            }


            return Newtonsoft.Json.JsonConvert.SerializeObject(edtContents, Newtonsoft.Json.Formatting.Indented);

        }

        private string GetContent(string date)
        {
            // 
            MediaContents contents = new MediaContents();
            List<MediaContent> content = new List<MediaContent>();

            try
            {

                #region content

                DataSet ds = SqlHelper.ExecuteDataset(AppCode_Base.strConn, CommandType.StoredProcedure, "usp_isport_feedkisscontent"
                    , new SqlParameter[] { new SqlParameter("@contentDate", date) });

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MediaContent m = new MediaContent();
                        m.pcntId = dr["ui_pcnt_id"].ToString();
                        m.mediaName = dr["content_link"].ToString();
                        m.mediaMessage = dr["title_local"].ToString();
                        m.mediaImage = System.Configuration.ConfigurationManager.AppSettings["ApplicationIsportURLImages"] + dr["content_image"].ToString().Replace("~", "..");
                        content.Add(m);

                    }
                    contents.MediaContent = content;
                    contents.errStatus = "success";
                    contents.errMessage = "";
                }
                else
                {
                    contents.MediaContent = content;
                    contents.errStatus = "fail";
                    contents.errMessage = "";
                }

                #endregion

            }
            catch (Exception ex)
            {
                contents.errStatus = "error";
                contents.errMessage = ex.Message;
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(contents, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
