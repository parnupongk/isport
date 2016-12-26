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
using MobileLibrary;
using WebLibrary;
namespace isport
{
    public partial class sportcc_i1 : MobileBase
    {
        MobileUtilities mU;
        public override void SetHeader()
        {
            frmMain.Title = "wap.isport.co.th";
            mU = Utilities.getMISDN(Request);
            // Header
            muHeader.Level = Request["level"] == null ? 0 : int.Parse(Request["level"]);
            muHeader.OperatorType = mU.mobileOPT == "" ? AppCode.OperatorType.All.ToString() : mU.mobileOPT;
            muHeader.uDataBind(bProperty_Mobile.bProperty_LNG, bProperty_Mobile.bProperty_MP
                        , bProperty_Mobile.bProperty_SIZE, bProperty_Mobile.bProperty_PRJ
                        , bProperty_Mobile.bProperty_SGID, bProperty_Mobile.bProperty_SCSID,Request["p"]);
        }
        public override void SetContent()
        {
            DataSet ds = new DataSet();
            string title = "";
            string timeRef = DateTime.Now.ToString("HH:mm");
            TimeSpan diff24 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 00, 00);
            TimeSpan diff18 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 00, 00);

            decimal addTime = (diff18.Hours < 0 && diff24.Hours >= 0) ? 0 : +1;

            switch (Request["m"].ToString())
            {
                case "ls":
                    title = "ผลสดๆ HOT!";
                    ds = new AppCodeSportCC().GetDataScore(AppCodeSportCC.MatchType.inprogress, addTime, "N");
                    break;
                case "td":
                    title = "สรุปผลวันนี้ ";
                    ds = new AppCodeSportCC().GetDataScore(AppCodeSportCC.MatchType.Finished, addTime, "N");
                    break;
                case "lt":
                    title = "สรุปผลเมื่อวานนี้";
                    ds = new AppCodeSportCC().GetDataScore(AppCodeSportCC.MatchType.Finished, -1, "N");
                    break;
                case "wk":
                    title = "ผลการแข่งขัน";
                    ds = new AppCodeSportCC().GetDataScore(AppCodeSportCC.MatchType.Finished, -7, "Y");
                    break;
                case "st":
                    title = "สถิติ";
                    ds = new AppCodeSportCC().GetDataScore(AppCodeSportCC.MatchType.NSY, 0, "N");
                    break;
            }

            muContent.Controls.AddAt(muContent.Controls.Count, Utilities.Lable(title, true, "Left","Title",""));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string strDate = "", strCountryId = "", scoreHome = "", scoreAway = "", status = "",teamName1="",teamName2="";
                
                // Check Active
                new AppCode_CheckActive().CheckAllService(bProperty_Mobile.bProperty_SGID, bProperty_Mobile.bProperty_LNG, bProperty_Mobile.bProperty_SGID
                    , bProperty_Mobile.bProperty_SCSID, bProperty_Mobile.bProperty_MP, bProperty_Mobile.bProperty_PRJ);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (strDate != dr["matchdate"].ToString().Substring(0, 8))
                    {
                        muContent.Controls.AddAt(muContent.Controls.Count,
                            Utilities.Lable(Utilities.DatetoText(dr["matchdate"].ToString().Substring(0, 8)), true, "Left", Color.Brown.ToString(), "true"));
                    }

                    if (strCountryId != dr["country_id"].ToString())
                    {
                        muContent.Controls.AddAt(muContent.Controls.Count,
                            Utilities.Lable(dr["tm_name"].ToString() + " ( " + dr["country_name"] + " )", true, "Left", Color.Brown.ToString(), "true"));
                    }

                    teamName1 = dr["isportTeamName1"].ToString() == "" ? dr["team_name1"].ToString() : dr["isportTeamName1"].ToString();
                    teamName2= dr["isportTeamName2"].ToString() == "" ? dr["team_name2"].ToString() : dr["isportTeamName2"].ToString();
                    if (dr["status"].ToString() == AppCodeSportCC.MatchType.Finished.ToString())
                    {
                        muContent.Controls.AddAt(muContent.Controls.Count, Utilities.Lable(
                        dr["MINUTES"].ToString() + " " + teamName1 + " " + dr["score_home"].ToString() + "-" + dr["score_away"].ToString()
                        + " " + teamName2 + " (" + dr["score_home_ht"].ToString() + "-" + dr["score_away_ht"].ToString() + ")"
                        , true, "Left", "Desc", ""));

                    }
                    else if (dr["status"].ToString() == AppCodeSportCC.MatchType.NSY.ToString())
                    {
                        muContent.Controls.AddAt(muContent.Controls.Count, Utilities.Link(
                        dr["MatchTime"].ToString() + " " + teamName1 + " VS " + teamName2
                        , true, "../sportcc_istatic.aspx?tgId=" + dr["contestgroupid"].ToString() + "&mId=" + dr["match_id"].ToString() + "&p=cc"
                        , "Left"));
                    }
                    else if (dr["status"].ToString() == AppCodeSportCC.MatchType.inprogress.ToString())
                    {
                        scoreHome = dr["score_home"].ToString() ;
                        scoreAway = dr["score_away"].ToString() ;

                        status = dr["minutes"].ToString() == "" ? dr["curent_period"].ToString() : "* " + dr["minutes"].ToString();
                        muContent.Controls.AddAt(muContent.Controls.Count, Utilities.Lable(
                        status + " " + teamName1 + " " + scoreHome
                        + " - "
                        + scoreAway + " " + teamName2
                        , true, "Left", "Desc", ""));
                    }

                    strDate = dr["matchdate"].ToString().Substring(0, 8);
                    strCountryId = dr["country_id"].ToString();
                }
            }
            else
            {
                muContent.Controls.AddAt(muContent.Controls.Count, Utilities.Lable(
                        "ข้อมูลกำลัง update กรุณาเข้ามาใหม่ภายหลัง"
                        , true, "Left", "Desc", ""));
            }
        }
        public override void SetFooter()
        {
            // Footer
            muFooter.Level = Request["level"] == null ? 0 : int.Parse(Request["level"]);
            muFooter.OperatorType = mU.mobileOPT == "" ? AppCode.OperatorType.All.ToString() : mU.mobileOPT;
            muFooter.uDataBind(bProperty_Mobile.bProperty_LNG, bProperty_Mobile.bProperty_MP
                        , bProperty_Mobile.bProperty_SIZE, bProperty_Mobile.bProperty_PRJ
                        , bProperty_Mobile.bProperty_SGID, bProperty_Mobile.bProperty_SCSID);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}