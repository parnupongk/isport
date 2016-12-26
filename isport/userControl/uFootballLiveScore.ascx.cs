using System;
using System.Collections;
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
using WebLibrary;
using MobileLibrary;

namespace isport
{
    public partial class uFootballLiveScore : System.Web.UI.MobileControls.MobileUserControl
    {
        public string ClassID
        {
            get
            {
                return ViewState["ClassID"] == null ? "" : ViewState["ClassID"].ToString();
            }
            set
            {
                ViewState["ClassID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void ClassDataBinding()
        {
            try
            {
                TimeSpan time = DateTime.Now.TimeOfDay - new TimeSpan(01, 00, 00);
                TimeSpan times = DateTime.Now.TimeOfDay - new TimeSpan(17, 00, 00);
                string strAddTime = times.Hours < 0 && time.Hours >= 0 ? "" : "+1";
                
                DataSet ds = new AppCode().FootballLiveScoreSelectbyClass(ClassID, strAddTime, " 16:00");
                foreach(DataRow dr in ds.Tables[0].Rows )
                {
                    this.Controls.AddAt(this.Controls.Count,Utilities.Lable(dr["class_name"].ToString(),true,"Left" ));
                    GenDataSource(dr["scs_id"].ToString(), dr["match_date"].ToString());
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
               
        }
        private void GenDataSource(string scsId,string matchDate)
        {
            try
            {
                DataSet ds =  new AppCode().FootballLiveScoreSelectbyMatch(scsId, matchDate, "L");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this.Controls.AddAt(this.Controls.Count, Utilities.Link(dr["scs_desc"].ToString(), true
                        ,"index.aspx?" + Request.QueryString.ToString()
                    , "Left"));
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
