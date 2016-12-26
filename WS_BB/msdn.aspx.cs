using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebLibrary;
namespace WS_BB
{
    public partial class msdn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*string str = "";
            str = new push().SendPost("http://prepaid.tot3g.net/p.aspx","");
            Response.Write(str);
            foreach (string v in Request.ServerVariables)
            {
                str += v + " value : " + Request.ServerVariables[v]  + "<br/>" ;
            }

            Response.Write(str);*/

           
           MobileLibrary.MobileUtilities mU = MobileLibrary.Utilities.getMISDN(Request);
            Response.Write("|" + mU.mobileOPT + "|" + mU.mobileNumber);
            /*
           ExceptionManager.WriteError(mU.mobileOPT + "|" + mU.mobileNumber);
          string s = "",ccc="",X_OPER="",X_APN="";
           string[] allCookies = Request.Cookies.AllKeys;
           foreach (string cookie in allCookies)
           {
               s += "|" + cookie;
               //Response.Write(cookie + "value : " + Request.Cookies[cookie].Value + "<br/>") ;
               ExceptionManager.WriteError(cookie + "value : " + Request.Cookies[cookie].Value);
           }
           string[] allServer = Request.ServerVariables.AllKeys;
           foreach (string all in allServer)
           {

               ExceptionManager.WriteError( all + " server value : " + Request.ServerVariables[all]);
              //Response.Write(all + " server value : " + Request.ServerVariables[all] + "<br/>") ;
           } 

           */

        }
    }
}