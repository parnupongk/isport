using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace isport.admin
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataRow dr = new AppCode().GetAdminUsers(txtUserName.Text,txtPassword.Text);
                if (dr != null)
                {

                    WebLibrary.MitTool.CreateCookie(dr["usr_id"].ToString(), Response, "isportwapadmin", 360);
                    Response.Redirect("newpage.aspx", false);

                }
            }
        }
    }
