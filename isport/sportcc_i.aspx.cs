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
    public partial class sportcc_i : MobileBase
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
            muContent.MasterID = Request["mid"] == null ? "0" : Request["mid"].ToString();
            muContent.Level = Request["level"] == null ? 0 : int.Parse(Request["level"]);
            muContent.OperatorType = mU.mobileOPT == "" ? AppCode.OperatorType.All.ToString() : mU.mobileOPT;
            muContent.ProjectType = Request["p"] == null ? AppCode.ProjectType.bb.ToString() : Request["p"].ToString();

            //if (muContent.Level == 0)
            //{
            //    // Get Content Manual
            //    muSportCC.uDataBind();
            //}

            //Content
            muContent.uDataBind(bProperty_Mobile.bProperty_LNG, bProperty_Mobile.bProperty_MP
                        , bProperty_Mobile.bProperty_SIZE, bProperty_Mobile.bProperty_PRJ
                        , bProperty_Mobile.bProperty_SGID, bProperty_Mobile.bProperty_SCSID);

        }
        public override void SetFooter()
        {
            // Footer
            muFooter.Level = Request["level"] == null ? 0 : int.Parse(Request["level"]);
            if (muFooter.Level > 0)
            {
                muFooter.OperatorType = mU.mobileOPT == "" ? AppCode.OperatorType.All.ToString() : mU.mobileOPT;
                muFooter.uDataBind(bProperty_Mobile.bProperty_LNG, bProperty_Mobile.bProperty_MP
                            , bProperty_Mobile.bProperty_SIZE, bProperty_Mobile.bProperty_PRJ
                            , bProperty_Mobile.bProperty_SGID, bProperty_Mobile.bProperty_SCSID);
            }

            // Insert Portal Logs
            Subscribe_portal_log_Insert(AppCode.strConnOracle, mU.mobileNumber, bProperty_Mobile.bProperty_USERAGENT, bProperty_Mobile.bProperty_SGID
                , mU.mobileOPT, bProperty_Mobile.bProperty_PRJ, bProperty_Mobile.bProperty_MP, bProperty_Mobile.bProperty_SCSID);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}