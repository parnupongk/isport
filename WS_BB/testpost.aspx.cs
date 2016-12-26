using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobileLibrary;
using WebLibrary;
namespace WS_BB
{
    public partial class testpost : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //======================================================== 
            //
            //              ใช้กับ tded love , siamdaranews
            //
            //========================================================
            string st = "";
            //if(Request["m"] != null )Response.AppendCookie(new HttpCookie("User-Identity-Forward-msisdn", WebLibrary.EncrptHelper.MD5Decryp( Request["m"] )));
            MobileUtilities mU = Utilities.getMISDN(Request);

            if (mU.mobileOPT == "02")
            {
                st = "02";
                Response.AppendCookie(new HttpCookie("Plain-User-Identity-Forward-msisdn", mU.mobileNumber));
            }
            else if (mU.mobileOPT == "01")
            {

                if (Request.Cookies["User-Identity-Forward-msisdn"] != null)
                {
                    st = Request.Cookies["User-Identity-Forward-msisdn"].Value.ToString();
                }
                else
                {
                    st = Request.Cookies["User-Identity-Forward-ppp-username"].Value.ToString();
                }
                Response.AppendCookie(new HttpCookie("User-Identity-Forward-msisdn", st));

            }
            else if (mU.mobileOPT == "03" || mU.mobileOPT == "04")
            {
                Response.AddHeader("HTTP_ORANGE_CLI", mU.mobileNumber);
            }


            if (Request.Url.Query.IndexOf("http") > -1)
            {

                if (st == "")
                {
                    // "&o=" คือให้ siamdaranews.com/index.aspx รู้ว่ามีการ fllow มาแล้วจะได้ไม่วน loop
                    Response.Redirect(Request.Url.Query.Substring(Request.Url.Query.IndexOf("http")).Replace("??", "?") + "&o=", false);
                }
                else
                {
                    if (st == "02")
                    {
                        Response.Redirect(Request.Url.Query.Substring(Request.Url.Query.IndexOf("http")).Replace("??", "?") + "&o=" + mU.mobileNumber, false);
                    }
                    else Response.Redirect(Request.Url.Query.Substring(Request.Url.Query.IndexOf("http")).Replace("??", "?") + "&m=" + st, false);
                }
            }
            //Response.Write("msisdn:" + mU.mobileNumber + " opt : " + mU.mobileOPT);
            //System.Collections.Specialized.NameValueCollection c = Request.Headers;
            //foreach (string s in c.AllKeys)
            //{
            //    Response.Write(" Key : " + s + " value : " + c[s]);
            //    Response.Write("<br/>");
            //}
        }
    }
}
