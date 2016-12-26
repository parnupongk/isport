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
    public partial class sportcc_istatic : MobileBase
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
            if (Request["mId"] != null && Request["tgId"] != null)
            {
                
                DataSet ds = new AppCodeSportCC().GetDataFootballStaticbyMatch(Request["mId"], Request["tgId"]);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string teamName="";
                    for (int index = 0; index < ds.Tables[0].Rows.Count; index++)
                    {
                        DataRow dr = ds.Tables[0].Rows[index];
                        if (index == 0)
                        {
                            muContent.Controls.AddAt(muContent.Controls.Count
                                , Utilities.Lable(dr["tm_name"].ToString(), true, "Left", "Header", ""));
                            muContent.Controls.AddAt(muContent.Controls.Count
                                                    , Utilities.Lable(Utilities.DatetoText(dr["matchdate"].ToString().Substring(0,8))
                                                    , true, "Left", "Header", ""));
                            muContent.Controls.AddAt(muContent.Controls.Count
                                , Utilities.Lable(dr["place"].ToString(), true, "Left", "Header", ""));
                        }

                        if (teamName != dr["teamname"].ToString())
                        {
                            muContent.Controls.AddAt(muContent.Controls.Count
                                , Utilities.Lable(dr["teamname"].ToString() + " 5 นัดหลังสุด", true, "Left", "Header", ""));
                        }

                        muContent.Controls.AddAt(muContent.Controls.Count
                                , Utilities.Lable(dr["team_name1"].ToString() + " " + dr["score_home"].ToString()
                                + " - "
                                + dr["score_away"].ToString() + " " + dr["team_name2"].ToString()
                                , true, "Left", "Desc", ""));

                        teamName = dr["teamname"].ToString();
                    }
                }
                else
                {
                    muContent.Controls.AddAt(muContent.Controls.Count
                        , Utilities.Lable("ขออภัยไม่พบสถิติของคู่นี้!!", true, "Left", "Title", ""));
                }
                
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