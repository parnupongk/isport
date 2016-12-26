using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Data;
using MobileLibrary;
using WebLibrary;

namespace WS_BB
{
    public class StatusWS
    {
        public string strStatus;
        public string strErrMess;
    }
    public class AppCode_PageStatus : AppCode_Base
    {

        /// <summary>
        /// GenStatus
        /// </summary>
        public static XDocument GenStatus(XDocument rtnXML,string strStatus,string strErr)
        {
            try
            {
                rtnXML.Element("SportApp").Add(new XElement("Status", strStatus)
                    , new XElement("Status_Message", strErr));

                return rtnXML;
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
        public static StatusWS CheckErrorParameter(HttpRequest Request)
        {
            StatusWS ws = new StatusWS();
            if (Request["pn"] != "notification")
            {
                if (Request["ap"] == "" || Request["ap"] == null) ws.strErrMess += "pls. input app name,";
                if ((Request["imei"] == "" || Request["imei"] == null ) && (Request["imsi"] == "" || Request["imsi"] == null) ) ws.strErrMess += "pls. input IMEI,IMSI";
                //if () ws.strErrMess += "pls. input IMSI,";
                //if (Request["ano"] == "" || Request["ano"] == null) strErr += "pls. input ano";
                if (ws.strErrMess != "") ws.strStatus = "Error";
            }
            return ws;
        }


    }
}
