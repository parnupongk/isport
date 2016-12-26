using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebLibrary;

namespace isport.userControl
{
    public partial class uMenu : System.Web.UI.UserControl
    {
        public string MenuID
        {
            get
            {
                return ViewState["MenuID"] == null ? "" : ViewState["MenuID"].ToString();
            }
            set
            {
                ViewState["MenuID"] = value;
            }
        }
        public string MenuType
        {
            get
            {
                return ViewState["MenuType"] == null ? "" : ViewState["MenuType"].ToString();
            }
            set
            {
                ViewState["MenuType"] = value;
            }
        }
        public string PageRedirect
        {
            get
            {
                return ViewState["PageRedirect"] == null ? "" : ViewState["PageRedirect"].ToString();
            }
            set
            {
                ViewState["PageRedirect"] = value;
            }
        }
        public void uGenMenu()
        {
            try
            {
                GetMenuUI();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
 
                

        }
        private void GetMenuUI()
        {
            try
            {

                DataSet ds = new AppCode().SelectUIByID(MenuID);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) GenContent(ds.Tables[0].Rows[0]);

            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }
        }
        private void GenContent(DataRow dr)
        {
            try
            {
                if (dr["content_icon"] != null && dr["content_icon"].ToString() != "")
                {
                    // Gen Icon
                    Image img = new Image();
                    img.ImageUrl = dr["content_icon"].ToString();       
                    this.Controls.AddAt(this.Controls.Count, img);
                }
                if (dr["content_link"] != null && dr["content_link"].ToString() != "")
                {
                    // Gen Link
                    HyperLink hlink = new HyperLink();
                    if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                    {
                        hlink.ImageUrl = dr["content_image"].ToString();
                    }
                    else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                    {
                        hlink.Text = dr["content_text"].ToString();
                    }
                    hlink.NavigateUrl = dr["content_link"].ToString();
                    this.Controls.AddAt(this.Controls.Count, hlink);
                }
                else
                {
                    if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                    {
                        Image img = new Image();
                        img.ImageUrl = dr["content_image"].ToString();
                        this.Controls.AddAt(this.Controls.Count, img);
                    }
                    if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                    {
                        Label lbl = new Label();
                        lbl.Text = dr["content_text"].ToString();
                        this.Controls.AddAt(this.Controls.Count, lbl);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}