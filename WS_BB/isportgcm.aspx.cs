using System;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Data;
using System.Net;
using System.Text;
using System.IO;
using WebLibrary;
using MobileLibrary;


namespace WS_BB
{
    public partial class isportgcm : System.Web.UI.Page
    {
        XmlDocument xmlDoc = new XmlDocument();
        XDocument rtnXML = new XDocument(new XElement("SportApp",
                             new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                             , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))));

        private string strErr = "", strStatus = "Success";
        private MobileUtilities uMobile = new MobileUtilities();
        protected void Page_Load(object sender, EventArgs e)
        {
            uMobile = Utilities.getMISDN(Request);
            if( Request["ap"] != null && Request["pn"] != null && Request["key"]!= null)
            {
                ExceptionManager.WriteError("Key Token >>" + Request["key"]);
            }else if(Request["ap"] != null && Request["pn"] != null && Request["dvid"] != null && Request["mess"] != null)
            {
                strErr = SendNotification(Request["dvid"], Request["mess"]);
            }

            GenStatus();
            xmlDoc.Load(rtnXML.CreateReader());
            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(xmlDoc, Newtonsoft.Json.Formatting.Indented));
        }

        public string SendNotification(string deviceId, string message)
        {
            string GoogleAppID = "AIzaSyCPtGdDKci85Sptp-9STn1VYB8jN5Zt6do";
            var SENDER_ID = "752241083303";
            var value = message;
            WebRequest tRequest;
            tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));

            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message= " + value + "&data.time = " +
            System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";
            Console.WriteLine(postData);
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();

            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);

            String sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
            tResponse.Close();
            return sResponseFromServer;
        }

        private void GenStatus()
        {
            try
            {

                rtnXML.Element("SportApp").Add(new XElement("Status", strStatus)
                    , new XElement("Status_Message", strErr));

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GenStatus>>" + ex.Message);
            }
        }
    }

    
}