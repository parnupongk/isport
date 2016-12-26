using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
namespace WS_BB
{
    public partial class fbscoredetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request["mId"] != null)
            {
                GetSetScoreDetail();
            }
        }
        private void GetSetScoreDetail()
        {
            AppCode_LiveScore liveScore = new AppCode_LiveScore();
            DataSet ds = liveScore.GetFootballMatchInfo(Request["mId"]);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string teamCode1 = ds.Tables[0].Rows[0]["team_id1"].ToString();
                string teamCode2 = ds.Tables[0].Rows[0]["team_id2"].ToString();
                string teamName1 = ds.Tables[0].Rows[0]["isportteamname1"].ToString() == "" ? ds.Tables[0].Rows[0]["team_name1"].ToString() : ds.Tables[0].Rows[0]["isportteamname1"].ToString();
                string teamName2 = ds.Tables[0].Rows[0]["isportteamname2"].ToString() == "" ? ds.Tables[0].Rows[0]["team_name2"].ToString() : ds.Tables[0].Rows[0]["isportteamname2"].ToString();
                lblLeague.Text = ds.Tables[0].Rows[0]["CLASS_NAME_LOCAL"].ToString() == "" ? ds.Tables[0].Rows[0]["tm_name"].ToString() : ds.Tables[0].Rows[0]["CLASS_NAME_LOCAL"].ToString();
                lblScore.Text = teamName1 + " " + ds.Tables[0].Rows[0]["score_home"].ToString() + " - " + ds.Tables[0].Rows[0]["score_away"].ToString() + " " + teamName2;
                ds = liveScore.GetFootballScoreDetail(Request["mId"]);
                gvGame.DataSource = ds.Tables[0];
                gvGame.DataBind();

                // Player
                lblTeam1.Text = teamName1;
                lblTeam2.Text = teamName2;
                ds = liveScore.GetFootballMatchPlayer(Request["mId"]);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // Team1
                    DataView dv = FillterPlayer(ds.Tables[0].DefaultView, teamCode1);
                    gvPlayer1.DataSource = dv;
                    gvPlayer1.DataBind();

                    dv = FillterPlayer(ds.Tables[0].DefaultView, teamCode2);
                    gvPlayer2.DataSource = dv;
                    gvPlayer2.DataBind();
                }

                Page.Title = "Ais Sport Arena";
                AddMeta(lblLeague.Text, lblScore.Text);
            }
        }
        private DataView FillterPlayer(DataView dv,string teamCode)
        {
            try
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
                str.Append(" competitor_id ='" + teamCode + "'");
                dv.RowFilter = str.ToString();
                return dv;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void AddMeta(string title,string desc )
        {
            HtmlMeta meta = new HtmlMeta();
            meta.Content = title;
            meta.Name = "title";
            Page.Header.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Content = desc;
            meta.Name = "description";
            Page.Header.Controls.Add(meta);
        }
        protected void gvGame_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            /*if (e.RowType == DevExpress.Web.ASPxGridView.GridViewRowType.Data && e.Row.Cells.Count>1)
            {
                Control ctr = new Control(),ctr1 = new Control() ;
                ControlCollection ctls = e.Row.Cells[1].Controls;
                foreach (Control c in ctls)
                {
                    ctr = getcontrol(c, "img");
                }
                e.Row.Cells[0].Text = e.GetValue("MINUTE") + "\"";
                Image img = (Image)ctr;
                if (object.Equals(e.GetValue("evenType"), "2nd Yellow"))
                {
                    img.ImageUrl = "images/ic_2yellow.png";
                }
                else if (object.Equals(e.GetValue("evenType"), "Red"))
                {
                    img.ImageUrl = "images/ic_red.png";
                }
                else if (object.Equals(e.GetValue("evenType"), "Yellow"))
                {
                    img.ImageUrl = "images/ic_yellow.png";
                }
                else if (object.Equals(e.GetValue("evenType"), "Penalty"))
                {
                    img.ImageUrl = "images/ic_kickoff.png";
                }
                else if (object.Equals(e.GetValue("evenType"), "Own Goal"))
                {
                    img.ImageUrl = "images/ic_kickoff.png";
                }
                else if (object.Equals(e.GetValue("evenType"), "Goal"))
                {
                    img.ImageUrl = "images/ic_kickoff.png";
                }
                else if (object.Equals(e.GetValue("evenType"), "Swap"))
                {
                    img.ImageUrl = "images/ic_sub.png";
                }
            }
            //e.Row.Cells[1].Controls.Add(img);*/
        }
        private Control getcontrol(Control c, string idControl)
        {
            ControlCollection ctls = c.Controls;
            Control crtn = null;
            foreach (Control c1 in ctls)
            {
                if (idControl == c1.ID) crtn = c1;
                getcontrol(c1, idControl);
            }
            return crtn;
        }
    }
}
