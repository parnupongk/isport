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
    public partial class isportSub : System.Web.UI.Page
    {
        XmlDocument xmlDoc = new XmlDocument();
        XDocument rtnXML = new XDocument(new XElement("SportApp",
                             new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                             , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))));

        private string strErr = "", strStatus = "Success";
        private MobileUtilities uMobile = new MobileUtilities();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                uMobile = Utilities.getMISDN(Request);
                CheckErrorParameter();
                if (!IsPostBack && strErr == "")
                {
                    //if (Request["ap"] == AppCode_Base.AppName.MTUTD.ToString() || Request["ap"] == AppCode_Base.AppName.StarSoccer.ToString() ) // App MTUTD
                    //{
                        if (Request["pn"] == "chk") // check active
                        {
                            GenCheckActive(Request["ap"]);
                        }
                        else if (Request["pn"] == "otp") // gen OTP 
                        {
                            GenOTP(Request["ap"]);
                        }
                        else if (Request["pn"] == "smo") // Submit OTP
                        {
                            SubOTP(Request["ap"]);
                        }
                        else if (Request["pn"] == "sub")
                        {
                            // ส่งสมัคร

                        }
                    //}

                }
                else if (strErr != "")
                {
                    new AppCode_Logs().Logs_Insert("MTUTD_ErrorParameter", strErr, "", "android", uMobile.mobileOPT + "|" + uMobile.mobileNumber + "|" + Request["imei"] + "|" + Request["imsi"]
                        , Request["ap"], Request["imei"], Request["imsi"], uMobile.mobileNumber, Request["model"], uMobile.mobileOPT, AppCode_Base.GETIP(),"");
                }
            }
            catch(Exception ex)
            {
                strStatus = "Error";
                strErr = ex.Message;

                ExceptionManager.WriteError("Error Request : " + Request.QueryString + " Err Message >>" + ex.Message);
            }
            GenStatus();
            xmlDoc.Load(rtnXML.CreateReader());
            string json = AppCode_XmlToJson.XmlToJSON(xmlDoc);
            Response.Write( json);
        }

        #region submit OTP

        private void CallSubscribe_IsportSportPool(string optCode, string msisdn)
        {
            try
            {
                // app Isport starsoccer ต้อง call add member ( service 10B/W )
                string strUrl = "";
                switch (optCode)
                {
                    case "01":
                        strUrl = "http://wap.isport.co.th/cgi/sms/ais/infopack/subscribe.aspx?TRANSID=00000000000001900&CMD=DLVRMSG&FET=IVR&NTYPE=GSM&FROM=" + msisdn + "&TO=" + ConfigurationManager.AppSettings["Application_IsportSportPool_PSNNumber_AIS"] + "&CODE=REQUEST&CTYPE=TEXT&CONTENT=&Act=S&Promotion=D&pssvid=504&mpcode=" + ConfigurationManager.AppSettings["MpCode_ISPORTSPORTPOOL"];
                        break;
                    case "02":
                        string serviceCode = ConfigurationManager.AppSettings["Application_IsportSportPool_PSNNumber_Dtac"].Substring(6) + ConfigurationManager.AppSettings["MpCode_ISPORTSPORTPOOL"];
                        string sNo = ConfigurationManager.AppSettings["Application_IsportSportPool_PSNNumber_Dtac"].Substring(8);
                        strUrl = "http://wap.isport.co.th/cgi/sms/dtac/infopack/subscribe.aspx?RefNo=1900&Msg=" + serviceCode + "&Msn=" + msisdn + "&Sno=" + sNo + "&Encoding=E&MsgType=E&User=i-SPORT&Password=ispOrt&Act=S&Promotion=D&SvNumber=" + ConfigurationManager.AppSettings["Application_IsportSportPool_PSNNumber_Dtac"] + "&mpcode=" + ConfigurationManager.AppSettings["MpCode_ISPORTSPORTPOOL"];
                        break;
                    case "03":
                        strUrl = "http://wap.isport.co.th/cgi/sms/orange/infopack/subscribe.aspx?local=spFnwGRz_css&MessageID=0000000001900&To=4511266&From=" + msisdn + "&Content=" + ConfigurationManager.AppSettings["Application_IsportSportPool_PSNNumber_True"] + "&ServiceID=0101312062&Act=S&Promotion=D&channel=wap&mpcode=" + ConfigurationManager.AppSettings["MpCode_ISPORTSPORTPOOL"]; ;
                        break;
                    case "04":
                        strUrl = "http://wap.isport.co.th/cgi/sms/realmove/infopack/subscribe.aspx?local=spFnwGRz_css&MessageID=0000000001900&To=4511266&From=" + msisdn + "&Content=" + ConfigurationManager.AppSettings["Application_IsportSportPool_PSNNumber_True"] + "&ServiceID=0101312062&Act=S&Promotion=D&channel=wap&mpcode=" + ConfigurationManager.AppSettings["MpCode_ISPORTSPORTPOOL"]; ;
                        break;
                }

                if (optCode == "03" || optCode=="04") ExceptionManager.WriteError("URL Request : " + strUrl + " Status >> start request " );

                System.Net.WebClient v = new System.Net.WebClient();
                Byte[] byteRquestHTML = v.DownloadData(strUrl);
                string strRequestHTML = System.Text.UTF8Encoding.UTF8.GetString(byteRquestHTML);
                
                ExceptionManager.WriteError("URL Request : " + strUrl + " Status >>" + strRequestHTML);
            }
            catch (Exception ex)
            {
                throw new Exception("CallSubscribe_IsportStarSoccer>> " + ex.Message);
            }
        }

        private void CallSubscribe_IsportStarSoccer(string optCode,string msisdn)
        {
            try
            {
                // app Isport starsoccer ต้อง call add member ( service 10B/W )
                string strUrl = "";
                switch(optCode)
                {
                    case "01" :
                        strUrl = "http://wap.isport.co.th/cgi/sms/ais/infopack/subscribe.aspx?TRANSID=00000000000001900&CMD=DLVRMSG&FET=IVR&NTYPE=GSM&FROM=" + msisdn + "&TO=" + ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_AIS"]  + "&CODE=REQUEST&CTYPE=TEXT&CONTENT=&Act=S&Promotion=D&pssvid=596&mpcode=" + ConfigurationManager.AppSettings["MpCode_ISPORTSTARSOCCER"];
                        break;
                    case "02" :
                        string serviceCode = ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_Dtac"].Substring(6) + ConfigurationManager.AppSettings["MpCode_ISPORTSTARSOCCER"];
                        string sNo = ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_Dtac"].Substring(8);
                        strUrl = "http://wap.isport.co.th/cgi/sms/dtac/infopack/subscribe.aspx?RefNo=1900&Msg=" + serviceCode + "&Msn=" + msisdn + "&Sno=" + sNo + "&Encoding=E&MsgType=E&User=i-SPORT&Password=ispOrt&Act=S&Promotion=D&SvNumber=" +ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_Dtac"] +"&mpcode=" + ConfigurationManager.AppSettings["MpCode_ISPORTSTARSOCCER"];
                        break;
                    case "03" :
                        strUrl = "http://wap.isport.co.th/cgi/sms/orange/infopack/subscribe.aspx?local=spFnwGRz_css&MessageID=0000000001900&To=4511216&From=" + msisdn + "&Content=" + ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_True"] + "&ServiceID=0101312558&Act=S&Promotion=D&channel=wap&mpcode=" + ConfigurationManager.AppSettings["MpCode_ISPORTSTARSOCCER"]; ;
                        break;
                    case "04":
                        strUrl = "http://wap.isport.co.th/cgi/sms/realmove/infopack/subscribe.aspx?local=spFnwGRz_css&MessageID=0000000001900&To=4511216&From=" + msisdn + "&Content=" + ConfigurationManager.AppSettings["Application_IsportStarSoccer_PSNNumber_True"] + "&ServiceID=2101312558&Act=S&Promotion=D&channel=wap&mpcode=" + ConfigurationManager.AppSettings["MpCode_ISPORTSTARSOCCER"]; ;
                        break;
                }
                  
                 
                    
                System.Net.WebClient v = new System.Net.WebClient();
                Byte[] byteRquestHTML  = v.DownloadData(strUrl);
                string strRequestHTML = System.Text.UTF8Encoding.UTF8.GetString(byteRquestHTML);
                //Utilities.GetDataXML(
                ExceptionManager.WriteError("URL Request : " + strUrl + " Status >>" + strRequestHTML);
            }
            catch (Exception ex)
            {
                throw new Exception("CallSubscribe_IsportStarSoccer>> " + ex.Message);
            }
        }


        private void SubOTP(string appName)
        {
            try
            {

                bool isActive = false;
                // Get Oparator Type
                string optCode = "";
                if (appName == "sportarena")
                {
                    optCode = "01";//uMobile.mobileOPT;
                }
                else //optCode = uMobile.mobileOPT;
                {
                    optCode = (uMobile.mobileOPT == null || uMobile.mobileOPT == "") ? AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"])) : uMobile.mobileOPT;
                }
                // Check Map OPT vs IMIE,IMSE  ( Set Active ="Y" )
                string rtnUpdate = "0";
                if (appName == AppCode_Base.AppName.SportPool.ToString())
                {
                    // SportPool ไม่มี OTP
                    rtnUpdate = "1";
                }
                else rtnUpdate = AppCode_Subscribe.UpdateSubscribeMember(Request["imsi"], Request["imei"], optCode, Request["otp"], appName);
               


                isActive = rtnUpdate != "0" ? true : false;

                if (isActive)
                {
                    string active = "Y"
                        , header = "สมัครสมาชิก " + appName + " App."
                        , detail = "คุณได้สมัครเป็นสมาชิก " + appName + " เรียบร้อย "
                        , footer = "ขอบคุณค่ะ";
                    // Isport StarSoccer Call Subscribe service 10B/W

                    string msisdn = uMobile.mobileNumber == "" || uMobile.mobileNumber == null ? Request["ano"] : uMobile.mobileNumber;
                    msisdn = (msisdn.Length == 10) ? "66" + msisdn.Substring(1) : msisdn;
                    msisdn = (msisdn.Length == 11) ? msisdn : AppCode_Subscribe.CheckActive_AppMember(Request["imsi"], Request["imei"], appName).msisdn;
                    if (AppCode_Subscribe.CheckRecurring_IsportStarSoccer_AppMember(optCode, msisdn, Request["imsi"], Request["imei"], appName) == "Y")
                    {
                        // Y คือ รอ Recurring ไม่ต้องส่งสมัคร และไม่ให้เข้า App
                        detail = "เนื่องจากยอดเงินของคุณไม่พอ กรุณาเติมเงินเพื่อเข้าใช้บริการด้วยค่ะ";
                        active = "N";
                    }
                    else
                    {
                        if (appName == AppCode_Base.AppName.StarSoccer.ToString())
                        {
                            CallSubscribe_IsportStarSoccer(optCode, msisdn);
                        }
                        else if (appName == AppCode_Base.AppName.SportPool.ToString())
                        {
                            CallSubscribe_IsportSportPool(optCode, msisdn);
                        }
                    }


                    rtnXML.Element("SportApp").Add(new XElement("Response"
                    , new XAttribute("isactive", active)
                    , new XAttribute("header", header)
                    , new XAttribute("detail", detail)
                    , new XAttribute("footer", footer)));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("Response"
                    , new XAttribute("isactive", "N")
                    , new XAttribute("header", "สมัครสมาชิก " + appName + " App.")
                    , new XAttribute("detail", "คุณกรอกรหัสไม่ถูกต้อง กรุณาตรวจสอบใหม่อีกครั้ง หรือ ปิด WIFI  " + (char)13 + " หรือ ติดต่อเจ้าหน้าที่โทร. 02-502-6767")
                    , new XAttribute("footer", "ขอบคุณค่ะ")));
                }

                new AppCode_Logs().Logs_Insert(appName + "_SubmitOTP", "", "", "android", uMobile.mobileOPT + "|" + uMobile.mobileNumber + "|" + Request["imei"] + "|" + Request["imsi"]
                    , appName, Request["imei"], Request["imsi"], uMobile.mobileNumber, Request["model"], uMobile.mobileOPT, AppCode_Base.GETIP(), "");

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("SubOPT >>" + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Gen OTP
        private void GenOTP(string appName)
        {
            try
            {
                

                // Get Oparator Type
                // iphone get imsi ไม่ได้ต้องหา optcode เอง
                string optCode = "";
                if (appName == "sportarena")
                {
                    optCode = "01";//uMobile.mobileOPT;
                }
                //else optCode = uMobile.mobileOPT;
                // ไม่ check opt จาก imsi เพราะกรณีเครื่องสอง sim จะทำให้เบอร์กับ opt ไม่ถูกต้อง
                else optCode = (uMobile.mobileOPT == null || uMobile.mobileOPT == "") ? AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"])) : uMobile.mobileOPT;

                // Gen OTP 
                string otp = new Random().Next(1000, 9999).ToString();
                string otpAuto = "";

                // Insert Member ( set active = "N")
                string msisdn = ( uMobile.mobileNumber == null || uMobile.mobileNumber == "") ? Request["ano"] : uMobile.mobileNumber ;
                msisdn = (msisdn.Length == 10) ? "66" + msisdn.Substring(1) : msisdn;

                string isActive = "N";
                if (appName == AppCode_Base.AppName.StarSoccer.ToString())
                {
                    isActive = AppCode_Subscribe.CheckActive_IsportStarSoccer_AppMember(optCode, uMobile.mobileNumber, Request["imsi"], Request["imei"], appName);
                }
                else
                {
                    isActive = AppCode_Subscribe.CheckActive_AppMember(Request["imsi"], Request["imei"], appName).status;
                }
                if (isActive == "N")
                {
                    // insert first access
                    SubscribeMember sm = AppCode_Subscribe.IsportAppCheckExpirePromotion(msisdn, Request["imsi"], Request["imei"], optCode
                            , appName, ConfigurationManager.AppSettings["Application_Day_Promotion"]);

                    int rtn = AppCode_Subscribe.InsertSubscribeMember(msisdn, Request["imsi"], Request["imei"], optCode, otp, appName);
                    //ExceptionManager.WriteError("GenOTP>>" + rtn);

                    string sysANO = (uMobile.mobileNumber != null && uMobile.mobileNumber != "" && uMobile.mobileNumber.Length > 10)?"0"+uMobile.mobileNumber.Substring(2):"";
                    if (Request["ano"] == sysANO)
                    {
                        otpAuto = otp;
                    }

                    if (optCode != "" && msisdn != "")
                    {
                        // Gen SMS OTP
                        AppCode_Subscribe.GenOTP(msisdn, optCode, otp, appName);
                    }
                }


                rtnXML.Element("SportApp").Add(new XElement("Response"
                    , new XAttribute("isactive", isActive)
                    , new XAttribute("header", "สมัครสมาชิก " + appName + " App.")
                    , new XAttribute("detail", "กรุณายืนยันรหัสผ่าน ")
                    , new XAttribute("footer", "กรุณารอรับ sms password")
                    , new XAttribute("auto", otpAuto)
                    ));


                new AppCode_Logs().Logs_Insert(appName + "_GenOTP", "", "", "android", uMobile.mobileOPT + "|" + uMobile.mobileNumber + "|" + Request["imei"] + "|" + Request["imsi"]
                   , appName, Request["imei"], Request["imsi"], uMobile.mobileNumber, Request["model"], uMobile.mobileOPT, AppCode_Base.GETIP(), "");

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GenOTP>>" + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Check Active
        /// <summary>
        /// MTUTD , SportArena ที่ใช้
        /// </summary>
        /// <param name="appName"></param>
        private void GenCheckActive(string appName)
        {
            try
            {
                
                SubscribeMember sm = AppCode_Subscribe.IsportAppCheckExpirePromotion(uMobile.mobileNumber, Request["imsi"], Request["imei"], uMobile.mobileOPT
                        , appName, ConfigurationManager.AppSettings[appName + "_Application_Day_Promotion"]);

                if (sm.status == "Y")
                {
                    // Check Active by imei , imsi
                    sm = AppCode_Subscribe.CheckActive_AppMember(Request["imsi"], Request["imei"], appName);
                    //string isActive = "Y";

                    /*if ( appName=="sportarena" && Request["oper"] != null && Request["oper"].IndexOf("ais") > -1)
                    {
                        // เป็นเบอร์ ais
                    }
                    else
                    {
                        // ไม่ใช่เบอร์ ais ให้ throw error
                        throw new Exception("เฉพาะลูกค้า Ais เท่านั้น");
                    }*/

                }
                else
                {
                    // ยังไม่เลยวันฟรี
                    sm.status = "Y";
                }

                rtnXML.Element("SportApp").Add(new XElement("Response"
                    , new XAttribute("isactive", sm.status)
                    , new XAttribute("header", "สมัครเป็นสมาชิก " + appName + " App.")
                    , new XAttribute("detail", "ลุ้นรับสิทธิพิเศษจากสโมสรมากมาย")
                    , new XAttribute("footer", "กรุณากรอกหมายเลขโทรศัพท์มือถือ (668-xxxxxxxxx)")));

                new AppCode_Logs().Logs_Insert(appName + "_CheckActive", "", "", "android", uMobile.mobileOPT + "|" + uMobile.mobileNumber + "|" + Request["imei"] + "|" + Request["imsi"]
                    , appName, Request["imei"], Request["imsi"], uMobile.mobileNumber, Request["model"], uMobile.mobileOPT, AppCode_Base.GETIP(), "");

            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("GenCheckActive>>" + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Gen Status Page
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
        private void CheckErrorParameter()
        {
            if (Request["ap"] == "" || Request["ap"] ==null) strErr += "pls. input app name,";
            if (Request["imei"] == "" || Request["imei"] == null) strErr += "pls. input IMEI,";
            if (Request["imsi"] == "" || Request["imsi"] ==null) strErr += "pls. input IMSI,";
            if ((Request["pn"] == "otp" ) && (Request["ano"] == null || Request["ano"] == "" || Request["ano"].Length < 10)) strErr += "pls. input ano or format ano"; // check active ไม่ต้องใส่ ano
            if (Request["pn"] == "smo" && (Request["otp"] == null || Request["otp"] == "")) strErr += "pls. input OTP";
            if (strErr != "") strStatus = "Error";
        }
        #endregion

    }
}
