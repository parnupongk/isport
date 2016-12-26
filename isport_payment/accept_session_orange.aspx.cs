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
using System.Configuration;
using WebLibrary;
namespace isport_payment
{
    public partial class accept_session_orange : mBasePage
    {
        //
        // หน้าตัดเงินของ True ต้อง fix ทั้งชื่อและ path (http://wap.isport.co.th/web/payment/accept_session_orange.aspx)
        //

        public override void SetHeader(){}

        public override void SetContent()
        {
            try
            {
                LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(),"TrueSession_Accept", Request.QueryString.ToString(), "");
                Response.Redirect("pay_true_redirect.aspx?" + Request.QueryString,false);
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("setcontent >>" + ex.Message);
                lblMessage.Text = ConfigurationManager.AppSettings["subScribeSystemErrorMessage"];
            }
        }
        public override void SetFooter() 
        {
            Setfooter(pnlFooter);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}