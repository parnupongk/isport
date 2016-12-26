using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
namespace isport.admin
{
    public partial class adminNotify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            PushNotification(txtNotify.Text);
        }

        private bool PushNotification(string pushMessage)
        {
            bool isPushMessageSend = false;

            /*string postString = "";
            string urlpath = "https://api.parse.com/1/push";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(urlpath);
            postString = "{ \"channels\": [ \"test\"  ], " +
                             "\"data\" : {\"alert\":\"" + pushMessage + "\", \"image\":\"http://ad.files-media.com/www/images/434150fd0feb6e50fb9a55ea847c0cb5.jpg\"}," +
                             "\"is_background\": [ \"false\"]" +
                             "}";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = postString.Length;
            httpWebRequest.Headers.Add("X-Parse-Application-Id", "0UGuCZvzNnoMMx68nh7ZQhTUfB7Fw3mWn9MjM08X");
            httpWebRequest.Headers.Add("X-Parse-REST-API-KEY", "iUbslfJaHwa9hUAQwhH5RFXPnHt99bskG19VWQr3");
            httpWebRequest.Method = "POST";

            byte[] bdata = System.Text.Encoding.UTF8.GetBytes(postString);
            httpWebRequest.ContentLength = bdata.Length;
            Stream requestWriter = httpWebRequest.GetRequestStream();
            requestWriter.Write(bdata, 0, bdata.Length);
            requestWriter.Close();

            /*StreamWriter requestWriter = new StreamWriter(httpWebRequest.GetRequestStream(), System.Text.Encoding.UTF8);
            requestWriter.Write(bdata,0, bdata.Length);
            requestWriter.Close();

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                JObject jObjRes = JObject.Parse(responseText);
                if (Convert.ToString(jObjRes).IndexOf("true") != -1)
                {
                    isPushMessageSend = true;
                }
            }*/

            return isPushMessageSend;
        }

    }
}