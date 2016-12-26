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
    public partial class pay_true : mBasePage
    {
        public override void SetHeader()
        {
            Setheader(pnlHeader);
        }
        public override void SetContent()
        {
            //
            // หน้าตัดเงินของ True ต้อง fix ทั้งชื่อและ path (http://wap.isport.co.th/wap/payment/accept_session_orange.aspx)
            //
            lbl5Bath.Text = "5 baht/Session";
            lnk5Bath.NavigateUrl = "http://wap.isport.co.th/wap/payment/accept_session_orange.aspx?SERVICE_ID=1041100002&sg="
                + bProperty_SGID + "&LNG=" + bProperty_LNG + "&mp=" + bProperty_MP 
                + "&prj=" + bProperty_PRJ + "&size=" + bProperty_SIZE + "&scs_id=" + bProperty_SCSID;

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