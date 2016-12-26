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
    public partial class sportcc_ileague : MobileBase
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
            DataBinding();
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
        private void DataBinding()
        {
            try
            {
                string url = ( Request["type"] == "p") ? "../sportcc_iprogram.aspx?p=cc" : "../sportcc_itable.aspx?p=cc";
                DataSet ds = new AppCodeSportCC().GetDataFootballLeagueByCountry(Request["cId"], Request["gId"]);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    muContent.Controls.AddAt(muContent.Controls.Count
                        , Utilities.Lable(ds.Tables[0].Rows[0]["COUNTRY_NAME"].ToString(), true, "Left", "Title", ""));
                }
                else
                {
                    muContent.Controls.AddAt(muContent.Controls.Count, Utilities.Lable(
                        "ข้อมูลกำลัง update กรุณาเข้ามาใหม่ภายหลัง"
                        , true, "Left", "Desc", ""));
                }
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    muContent.Controls.AddAt(muContent.Controls.Count
                        , Utilities.Link(dr["CONTEST_NAME"].ToString(), true, url + "&tgId=" + dr["contestgroupid"].ToString(), "Left"));
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Databinding >> " + ex.Message);
            }
        }
    }
}