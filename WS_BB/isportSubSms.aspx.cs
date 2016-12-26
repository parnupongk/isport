using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Data;
using WebLibrary;
using MobileLibrary;

namespace WS_BB
{
    public partial class isportSubSms : System.Web.UI.Page
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
            CheckErrorParameter();
            if (!IsPostBack && strErr == "")
            {
                if (Request["ap"] == AppCode_Base.AppName.MTUTD.ToString()) // App MTUTD
                {
                    if (Request["pn"] == "sms") // gen sms service
                    {
                        GenSMSService();
                    }
                }

            }

            GenStatus();
            xmlDoc.Load(rtnXML.CreateReader());
            //Response.Write(xmlDoc.InnerXml);
            string json = AppCode_XmlToJson.XmlToJSON(xmlDoc);
            Response.Write(json);
        }
        private void GenSMSService()
        {
            try
            {
                new AppCode_Logs().Logs_Insert("MTUTD_GenSMSPackage", "", "", "android", uMobile.mobileOPT + "|" + uMobile.mobileNumber + "|" + Request["imei"] + "|" + Request["imsi"]
                    , AppCode_Base.AppName.MTUTD.ToString(),Request["imei"],Request["imsi"],uMobile.mobileNumber,Request["model"],uMobile.mobileOPT,AppCode_Base.GETIP(),"");

                string optCode = uMobile.mobileOPT;//AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"]));

                rtnXML = new AppCode_Subscribe().CommandGetSMSService(rtnXML,optCode,ConfigurationManager.AppSettings["MpCode_MTUTD"].ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("GenSMSService >>" + ex.Message);
            }
        }
        /// <summary>
        /// GenStatus
        /// </summary>
        private void GenStatus()
        {
            try
            {

                rtnXML.Element("SportApp").Add(new XElement("Status", strStatus)
                    , new XElement("Status_Message", strErr));

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GenError>>" + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// CheckErrorParameter
        /// </summary>
        private void CheckErrorParameter()
        {
            if (Request["ap"] == "" || Request["ap"] == null) strErr += "pls. input app name,";
            if (Request["imei"] == "" || Request["imei"] == null) strErr += "pls. input IMEI,";
            if (Request["imsi"] == "" || Request["imsi"] == null) strErr += "pls. input IMSI,";
            //if (Request["ano"] == "" || Request["ano"] == null) strErr += "pls. input ano";
            if (strErr != "") strStatus = "Error";
        }
    }
}
