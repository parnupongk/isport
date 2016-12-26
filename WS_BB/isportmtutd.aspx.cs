using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Data;
using MobileLibrary;
using WebLibrary;

namespace WS_BB
{
    public partial class isportmtutd : System.Web.UI.Page
    {
        XmlDocument xmlDoc = new XmlDocument();
        XDocument rtnXML = new XDocument(new XElement("SportApp",
                             new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                             , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))));

        private string strErr = "", strStatus = "Success";
        private MobileUtilities uMobile = new MobileUtilities();
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isData = false;
            uMobile = Utilities.getMISDN(Request);
            CheckErrorParameter();
            if (!IsPostBack && strErr == "")
            {
                if (Request["ap"] == AppCode_Base.AppName.MTUTD.ToString()) // App MTUTD
                {
                    if (Request["pn"] == "non") // notification
                    {
                        // Notification
                        isData = GenNotification();

                        // Activity
                        isData = (GenActivity()) ? true : isData;
                    }
                }

            }

            GenStatus(isData);
            xmlDoc.Load(rtnXML.CreateReader());
            //Response.Write(xmlDoc.InnerXml);
            string json = AppCode_XmlToJson.XmlToJSON(xmlDoc);
            Response.Write(json);
        }
        private bool GenActivity()
        {
            try
            {
                bool isData = false;
                new AppCode_Logs().Logs_Insert("MTUTD_GenActivity", "", "", "android", uMobile.mobileOPT + "|" + uMobile.mobileNumber + "|" + Request["imei"] + "|" + Request["imsi"]
                   , AppCode_Base.AppName.MTUTD.ToString(), Request["imei"], Request["imsi"], uMobile.mobileNumber, "", uMobile.mobileOPT, AppCode_Base.GETIP(),"");

                rtnXML.Element("SportApp").Add(new XElement("activity","")
                        , new XElement("activity_url", ""));
                isData = false;
                return isData;
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("GenActivity>>" + ex.Message);
                throw new Exception("GenNotification >>" + ex.Message);
            }
        }
        private bool GenNotification()
        {
            try
            {
                bool isData = false;
                new AppCode_Logs().Logs_Insert("MTUTD_GenNotification", "", "", "android", uMobile.mobileOPT + "|" + uMobile.mobileNumber + "|" + Request["imei"] + "|" + Request["imsi"]
                   , AppCode_Base.AppName.MTUTD.ToString(), Request["imei"], Request["imsi"], uMobile.mobileNumber, "", uMobile.mobileOPT, AppCode_Base.GETIP(), "");

                string teamCode = "0000001204";
                int min = Request["min"] == null || Request["min"] == "" ? int.Parse(ConfigurationManager.AppSettings["ApplicationnotificationMinute"] ): int.Parse( Request["min"].ToString() ) ;
                rtnXML = new AppCode_Utility().CommandGetNotification_BallThai(rtnXML, teamCode, min, AppCode_Utility.Country.th.ToString());
                isData = rtnXML.Element("SportApp").Element("notification").Value == "" ? false : true ;
                return isData;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GenNotification>>" + ex.Message);
                throw new Exception("GenNotification >>" + ex.Message);
            }
        }
        /// <summary>
        /// GenStatus
        /// </summary>
        private void GenStatus(bool isData)
        {
            try
            {
                if (!isData)
                {
                    strStatus = "404";
                    strErr = "content not found";
                }

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
