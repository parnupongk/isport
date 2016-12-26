using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tamir.SharpSsh;
using WebLibrary;
using MobileLibrary;

using System.Web.Services;
using System.Net; 
namespace isport
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //MobileUtilities mU = Utilities.getMISDN(Request);
            //Response.Write(mU.mobileOPT + " " + mU.mobileNumber);
            //SendSMS();
            //string vStrName = Request.ServerVariables["HTTP_USER_AGENT"];
            //Response.Write(vStrName.ToLower().IndexOf("opera"));

            string str = "14/10/2015 13:47:47";
            DateTime d = DateTime.ParseExact(str, "dd/MM/yyyy HH:mm:ss", null);
            string s = d.ToString();              
        }

        public void SendSMS()
        {
            UriBuilder urlBuilder = new UriBuilder();
            urlBuilder.Host = "127.0.0.1";
            urlBuilder.Port = 8800;

            //string PhoneNumber = "60123456789"; 
            string PhoneNumber = "4511216";
            string message = "Just a simple text";

            urlBuilder.Query = string.Format("PhoneNumber=%2B" + PhoneNumber + "&Text=" + message);

            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(urlBuilder.ToString(), false));
            HttpWebResponse httpResponse = (HttpWebResponse)(httpReq.GetResponse());

            Response.Write(httpResponse.StatusCode.ToString());
            Response.Write("sms send successfully");
        } 

        protected void Button1_Click(object sender, EventArgs e)
        {

            txt.Text = AppCode.GetShortURL(txt.Text);
            //Response.Write(file.PostedFile.FileName);
            /*
            SshTransferProtocolBase sshCp;
            sshCp = new Sftp("192.168.101.121", "isport");
            sshCp.Password = "isport@clip";
            try
            {
                if (file.PostedFile.ContentLength > 0)
                {
                    string typw = file.PostedFile.ContentType;
                    string fileName = "kiss_" + DateTime.Now.ToString("ddMMyyyyHHMMss") + file.FileName;
                    string pathFull = Server.MapPath("upload/" + fileName);
                    file.PostedFile.SaveAs(pathFull);
                    sshCp.Connect();

                    sshCp.Put(pathFull, "/data0/wowza/isport/content/"+fileName);
                    sshCp.Close();
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
                sshCp.Close();
            }*/
        }
    }
}
