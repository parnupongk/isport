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
    public abstract class mBasePage : System.Web.UI.MobileControls.MobilePage
    {

        #region Base Property
        public string bProperty_SGID
        {
            get
            {
                return ViewState["SGID"] == null ? "" : ViewState["SGID"].ToString();
            }
            set
            {
                ViewState["SGID"] = value;
            }
        }
        public string bProperty_LNG
        {
            get
            {
                return ViewState["LANG"] == null ? "L" : ViewState["LANG"].ToString();
            }
            set
            {
                ViewState["LANG"] = value;
            }
        }
        public string bProperty_SCSID
        {
            get
            {
                return ViewState["SCSID"] == null ? "" : ViewState["SCSID"].ToString();
            }
            set
            {
                ViewState["SCSID"] = value;
            }
        }
        public string bProperty_SIZE
        {
            get
            {
                return ViewState["SIZE"] == null ? "S" : ViewState["SIZE"].ToString();
            }
            set
            {
                ViewState["SIZE"] = value;
            }
        }
        public string bProperty_MP
        {
            get
            {
                return ViewState["MP"] == null ? "0000" : ViewState["MP"].ToString();
            }
            set
            {
            }
        }
        public string bProperty_PRJ
        {
            get
            {
                return ViewState["PRJ"] == null ? "1" : ViewState["PRJ"].ToString();
            }
            set
            {
                ViewState["PRJ"] = value;
            }
        }
        public string bProperty_USERAGENT
        {
            get
            {
                return Request.ServerVariables["HTTP_USER_AGENT"];
            }
        }
        #endregion

        #region Abstract class
        public abstract void SetHeader();

        public abstract void SetContent();

        public abstract void SetFooter();
        #endregion

        #region Default class
        protected void Setheader(System.Web.UI.MobileControls.Panel pnl)
        {
            System.Web.UI.MobileControls.Image img = new System.Web.UI.MobileControls.Image();
            img.BreakAfter = true;
            img.Alignment = Alignment.Center;
            img.ImageUrl = "images/logo.gif";
            pnl.Controls.AddAt(pnl.Controls.Count, img);
        }
        protected void Setfooter(System.Web.UI.MobileControls.Panel pnl)
        {
            System.Web.UI.MobileControls.Image img = new System.Web.UI.MobileControls.Image();
            img.ImageUrl = "images/home.gif";
            img.BreakAfter = false;
            
            Link lnk = new Link();
            lnk.Text = bProperty_LNG == "L" ? "กลับหน้าหลัก" : "Back to Home";
            lnk.NavigateUrl = (bProperty_SGID == "255") ? ConfigurationManager.AppSettings["tdedloveRoot"] : ConfigurationManager.AppSettings["isportRoot"];

            pnl.Controls.AddAt(pnl.Controls.Count, img);
            pnl.Controls.AddAt(pnl.Controls.Count, lnk);
        }
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {

                try
                {
                    bProperty_SGID = Request["sg"];
                    bProperty_LNG = Request["lng"];
                    bProperty_SCSID = Request["scs_id"];
                    bProperty_SIZE = Request["size"];
                    bProperty_MP = Request["mp"];
                    bProperty_PRJ = Request["prj"];

                    Page pag = (Page)sender;

                    SetHeader();
                    SetContent();
                    SetFooter();
                    
                }
                catch(Exception ex)
                {
                    ExceptionManager.WriteError("mBasePage Load>>" + ex.Message );   
                }

        }
        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion

    }
}
