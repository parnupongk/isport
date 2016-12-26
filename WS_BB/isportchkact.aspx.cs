using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using MobileLibrary;
namespace WS_BB
{
    public partial class isportchkact : System.Web.UI.Page
    {
        //===================================================================================================================
        //
        //
        //                      ส่งให้กับ app content ccafe
        //
        //
        //
        //===================================================================================================================

        XDocument rtnXML = new XDocument(new XElement("SportApp"));

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                if (Request["p"] == "ccafekey")
                    ContentCCafeGetKey();
                else if (Request["p"] == "sub")
                    SubScribePackage();
                else
                    Checkactive();
            }
        }
        /// <summary>
        /// ใช้ gen key ให้กับ app content ccafe
        /// </summary>
        public void ContentCCafeGetKey()
        {
            
            string rtn = "";
            MobileUtilities mU = Utilities.getMISDN(Request);
            XmlDocument xmlDoc = new XmlDocument();
            rtnXML.Element("SportApp").Add(new XElement("key", ConfigurationManager.AppSettings["CCAFEKEYACCESS"]),new XElement("status", "succcess")
                    , new XElement("message", ""));

            xmlDoc.Load(rtnXML.CreateReader());
            //rtn += "<isportapp><key>" + ConfigurationManager.AppSettings["CCAFEKEYACCESS"] + "</key>";
            //rtn += "<status>success</status>";
            //rtn += "<message></message></isportapp>";
            try
            {
                new AppCode_Logs().Logs_Insert("SportPhone_GetKey", "", "", Request["type"], mU.mobileNumber + "," + Request["imei"] + "," + Request["imsi"] + "," + Request["model"], AppCode_Base.AppName.SportPhone.ToString(), Request["imei"], Request["imsi"], mU.mobileNumber, Request["model"], mU.mobileOPT, AppCode_Base.GETIP(), "");
            }
            catch { }
            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(xmlDoc, Newtonsoft.Json.Formatting.Indented));
        }

        public void SubScribePackage()
        {
            Response.ContentType = "text/xml";
            MobileUtilities mU = Utilities.getMISDN(Request);
            AppCode_Subscribe.Out_CheckActive outCheckActive = new isport().SubscribePackage(mU.mobileNumber, mU.mobileOPT, "378", "S", "D", GetMpCodeByTeamCode(), "");
            //Response.Write(new AppCode_Subscribe().CommandSubscribePackage(mU.mobileNumber, mU.mobileOPT, "378", "S", "D", "0023"));
            //AppCode_Subscribe.Out_CheckActive outCheckActive = new AppCode_Subscribe().CommandSubscribePackage("66877998335", "02", "378", "S", "D", "0023");
            Response.Write("<isportapp><status>" + outCheckActive.status + "</status>");
            Response.Write("<message>" + outCheckActive.Mess + "</message></isportapp>");
        }
        private string GetMpCodeByTeamCode()
        {
            string mpCode= "0000";
            if (Request["teamCode"] == "0000001204") mpCode = "0023";
            else if (Request["teamCode"] == "0000001498") mpCode = "0050";
            else if (Request["teamCode"] == "0000001194") mpCode = "0061";
            return mpCode;
        }
        public void Checkactive()
        {
            string active = "N", status = "", desc = "";
            Response.Write("<?xml version='1.0' encoding='UTF-8'?>");
            XmlDocument xmlDoc = new XmlDocument();
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                         new XAttribute("header", "")
                         , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                         ));
            xmlDoc.Load(rtnXML.CreateReader());
            if (Request["projectcode"] != null && Request["projectcode"].ToUpper() == "LSP")
            {

                // Check Active 
                #region
                MobileLibrary.MobileUtilities mU = MobileLibrary.Utilities.getMISDN(Request);
                string psnNumber = "";
                switch (mU.mobileOPT)
                {
                    case "01": psnNumber = "*45111001175"; break;
                    case "02": psnNumber = "*1989433175"; break;
                    case "03": psnNumber = "10075"; break;
                }

                active = new AppCode_Subscribe().CommandCheckActive(mU.mobileNumber, mU.mobileOPT, psnNumber);
                status = "Success";
                #endregion

            }
            else
            {
                active = "N";
                status = "Error";
                desc = "can not find project code!!";
                //Response.Write(" can not find project code!!");
            }
            Response.Write("<isportapp><active>" + active + "</active>");
            Response.Write("<status>" + status + "</status>");
            Response.Write("<message>" + desc + "</message></isportapp>");
        }
    }
}
