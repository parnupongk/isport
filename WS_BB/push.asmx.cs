using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Mail;
namespace WS_BB
{
    /// <summary>
    /// Summary description for push
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class push : System.Web.Services.WebService
    {

        [WebMethod]
        public string SendPost(String IP_PORT, String COMMAND)
        {
            try
            {
                //WebLibrary.LogsManager.WriteLogs("Data_Post", IP_PORT, COMMAND);
                string responseHtml = "";
                string url =
                    string.Format(IP_PORT);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0 (Windows; U; "
                    + "Windows NT 5.1; en-US; rv:1.7) "
                    + "Gecko/20040707 Firefox/0.9.2";
                request.Method = "POST";
                request.Accept =
                    "text/xml,application/xml,application/xhtml+xml,"
                    + "text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                request.KeepAlive = true;
                request.ContentType = @"application/x-www-form-urlencoded";

                string postData =
                    string.Format(COMMAND);
                request.Method = "POST";

                byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                //System.Text.Encoding.GetEncoding("tis-620").GetBytes(postData);

                request.ContentLength = postBuffer.Length;
                Stream postDataStream = request.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();

                // Get the response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Encoding enc = System.Text.Encoding.UTF8;
                StreamReader responseStream = new StreamReader(response.GetResponseStream(), enc, true);
                responseHtml = responseStream.ReadToEnd();
                response.Close();
                responseStream.Close();

                return responseHtml;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [WebMethod]
        public string SendGet(String IP_PORT)
        {
            try
            {
                string responseHtml = "";
                string url =
                    string.Format(IP_PORT);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0 (Windows; U; "
                    + "Windows NT 5.1; en-US; rv:1.7) "
                    + "Gecko/20040707 Firefox/0.9.2";
                request.Method = "GET";
                request.Accept =
                    "text/xml,application/xml,application/xhtml+xml,"
                    + "text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                request.KeepAlive = true;
                request.ContentType = @"application/x-www-form-urlencoded";

                // Get the response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Encoding enc = System.Text.Encoding.UTF8;
                StreamReader responseStream = new StreamReader(response.GetResponseStream(), enc, true);
                responseHtml = responseStream.ReadToEnd();
                response.Close();
                responseStream.Close();

                return responseHtml;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


    }
}
