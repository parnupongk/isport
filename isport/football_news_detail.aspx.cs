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
namespace isport
{
    public partial class fotball_news_detail : MobileBase
    {
        public override void SetHeader()
        {
            //frmMain.Title = "wap.isport.co.th";
        }
        public override void SetContent()
        {
            /*
            MobileUtilities mU = Utilities.getMISDN(Request);

            // Header
            muHeader.Level = Request["level"] == null ? 0 : int.Parse(Request["level"]);
            muHeader.OperatorType = mU.mobileOPT == "" ? AppCode.OperatorType.All.ToString() : mU.mobileOPT;
            //frmMain.Controls.AddAt(frmMain.Controls.Count,Utilities.Lable(mU.mobileOPT+" "+mU.mobileNumber,true,"Left"));
            muHeader.uDataBind(bProperty_Mobile.bProperty_LNG, bProperty_Mobile.bProperty_MPCODE
                        , bProperty_Mobile.bProperty_SIZE, bProperty_Mobile.bProperty_PRJ
                        , bProperty_Mobile.bProperty_SGID, bProperty_Mobile.bProperty_SCSID,Request["p"]);

            //Content
            //if (Request["pcnt_id"] )

            muContent.uDataBindNewsDetail(Request["pcnt_id"], bProperty_Mobile.bProperty_LNG, bProperty_Mobile.bProperty_MPCODE
                        , bProperty_Mobile.bProperty_SIZE, bProperty_Mobile.bProperty_PRJ
                        , bProperty_Mobile.bProperty_SGID, bProperty_Mobile.bProperty_SCSID);


            // Footer
            muFooter.Level = Request["level"] == null ? 0 : int.Parse(Request["level"]);
            muFooter.OperatorType = mU.mobileOPT == "" ? AppCode.OperatorType.All.ToString() : mU.mobileOPT;
            muFooter.uDataBind(bProperty_Mobile.bProperty_LNG, bProperty_Mobile.bProperty_MPCODE
                        , bProperty_Mobile.bProperty_SIZE, bProperty_Mobile.bProperty_PRJ
                        , bProperty_Mobile.bProperty_SGID, bProperty_Mobile.bProperty_SCSID);

            // Insert Portal Logs
            Subscribe_portal_log_Insert(AppCode.strConnOracle, mU.mobileNumber, bProperty_Mobile.bProperty_USERAGENT, bProperty_Mobile.bProperty_SGID
                , mU.mobileOPT, bProperty_Mobile.bProperty_PRJ, bProperty_Mobile.bProperty_MPCODE, bProperty_Mobile.bProperty_SCSID);
                */
        }
        public override void SetFooter()
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("detail.aspx?"+ Request.QueryString,false);
        }
    }
}