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
    public partial class sportcc_icountry : MobileBase
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
            if (Request["type"] != null)
            {
                muContent.Controls.AddAt(muContent.Controls.Count, Utilities.Lable(
                        "ประเทศต่างๆ"
                        , true, "Left", "Title", ""));
                DataSet ds = new AppCodeSportCC().GetDataFootballCountryAll();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    muContent.Controls.AddAt(muContent.Controls.Count
                        , Utilities.Link(dr["country_name"].ToString(), true
                        , "../sportcc_ileague.aspx?p=cc&type=" + Request["type"] + "&cId=" + dr["COUNTRY_ID"].ToString()
                        , "Left"));
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