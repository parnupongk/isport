using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MobileLibrary;
using WebLibrary;
namespace isport
{
    public partial class check : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MobileUtilities mU = Utilities.getMISDN(Request);
                string vStrName = Request.ServerVariables["HTTP_USER_AGENT"];

                GenMenuHeader(vStrName, mU.mobileOPT);
            }
            catch
            {
                Response.Redirect("index.aspx?" + Request.QueryString, false);
            }
        }

        private void GenMenuHeader(string vStrName, string opt)
        {
            try
            {
                if (vStrName.ToLower().IndexOf("symbian") > -1 || Request.Browser.ScreenPixelsWidth < 320 || vStrName.ToLower().IndexOf("nokia") > -1)
                {
                    // low
                    //if (Request["p"] == "d02campaignh" || Request["p"] == "d02campaign")
                    //{
                       // Response.Redirect("index.aspx?" + Request.QueryString ,false);
                    //}
                    //else 
                    Response.Redirect("http://wap.isport.co.th/sport_center/isport/index.aspx?" + Request.QueryString,false);
                }
                else if (vStrName.ToLower().IndexOf("opera") > -1)
                {
                    Response.Redirect("index.aspx?" + Request.QueryString, false);
                }
                else
                {
                    // hight
                    //if (Request["p"] == "d02campaignh" || Request["p"] == "d02campaign")
                    //{
                    //Response.Redirect("indexl.aspx?p=d02campaignh&sg=" + Request["sg"] + "&mp_code=" + Request["mp_code"],false);
                    //}
                    //else 
                    Response.Redirect("index.aspx?" + Request.QueryString, false);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
