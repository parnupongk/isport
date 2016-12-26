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
    public partial class sportcc_iprogram : MobileBase
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
            if (Request["tgId"] != null) DataBinding();
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
                // Program where by ContestGroupID
                DataSet ds = new AppCodeSportCC().GetDataFootballProgram(Request["tgId"]);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) GetContent(ds.Tables[0]);
                else
                {
                    muContent.Controls.AddAt(muContent.Controls.Count, Utilities.Lable(
                        "ข้อมูลกำลัง update กรุณาเข้ามาใหม่ภายหลัง"
                        , true, "Left", "Desc", ""));
                }

            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("DataBinding >> " + ex.Message);
            }

        }
        private void GetContent(DataTable dt )
        {
            try
            {
                string strDate="";
                muContent.Controls.AddAt(muContent.Controls.Count
                        , Utilities.Lable(dt.Rows[0]["TM_NAME"].ToString(), true, "Left", "Title", ""));
                foreach(DataRow dr in dt.Rows)
                {
                    
                    if (strDate != dr["MtachDate"].ToString())
                    {
                        muContent.Controls.AddAt(muContent.Controls.Count
                            , Utilities.Lable(Utilities.DatetoText(dr["MtachDate"].ToString())
                            ,true,"Left","Title",""));
                    }

                    muContent.Controls.AddAt(muContent.Controls.Count
                            , Utilities.Lable(dr["MatchTime"].ToString() + " " + dr["TEAM_NAME1"].ToString() + " VS " + dr["TEAM_NAME2"].ToString()
                            , true, "Left", "Desc", ""));
                    strDate = dr["MtachDate"].ToString();
                }
            }
            catch(Exception ex)
            {
                throw new Exception("GetContent >> " + ex.Message);
            }
        }
    }
}