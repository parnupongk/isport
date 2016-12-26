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
    public partial class sportcc_itable : MobileBase
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
            if (Request["tgId"] != null)
            {
                DataSet ds = new AppCodeSportCC().GetDataFootballTable(Request["tgId"]);
                //if ( ds.Tables.Count>0 && ds.Tables[0].Rows.Count > 0) frmMain.Controls.AddAt(0
                  //      , Utilities.Lable(ds.Tables[0].Rows[0]["TM_NAME"].ToString(), true, "Left", "Title", ""));
                footballTable.PLACE = "PLACE";
                footballTable.TEAM_NAME = "TEAM_NAME";
                footballTable.TOTAL_PLAY = "TOTAL_PLAY";
                footballTable.TOTAL_POINT = "TOTAL_POINT";
                footballTable.TOTAL_DIFF = "TOTAL_DIFF";
                footballTable.udataBinding(ds.Tables[0]);

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