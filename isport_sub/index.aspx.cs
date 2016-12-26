using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using MobileLibrary;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;
using System.Configuration;
using OracleDataAccress;
using System.Data.OracleClient;
namespace isport_sub
{



    public partial class index : PageBase
    {
 

        [WebMethod]
        public static string GetOTP(string  msisdn,string opt)
        {
            try
            {
                //return msisdn + "|" + opt;
                //return new Post.push().SendGet("http://192.168.103.180/isportws/isportsub.aspx");
                string otp = new Random().Next(1000, 9999).ToString();
                opt = "0" + opt;
                msisdn = "66"+ msisdn;
                //string s = AppCode_Subscribe.GenOTP("66"+msisdn, "01", otp, "WebSub");
                if (opt != "99")
                {

                    int rtn = InsertSubscribeMember(msisdn, msisdn + opt, msisdn + opt, opt, otp, "WebSub"); // imei = phone+ OPT , IMSI = phone + OPT

                    SqlHelper.ExecuteNonQuery(AppMain.strConnPack, CommandType.StoredProcedure, "usp_inst_queue_otp"
                        , new SqlParameter[] { new SqlParameter("@phone_no",msisdn ) 
                                                    ,new SqlParameter("@opt_code",opt)
                                                    ,new SqlParameter("@otp",otp)
                                                    ,new SqlParameter("@app_name","WebSub")});
                }
                return otp;

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
                return "";
                
            }
        }


        [WebMethod]
        public static string SubmitOTP(string msisdn, string opt,string pssvid)
        {
            try
            {
                //ExceptionManager.WriteError(msisdn + opt + pssvid);
                //return "3333333333333333333";
                return CallSubscribe_IsportSportPool(opt, msisdn, pssvid);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
                return "";

            }
        }

        public static int InsertSubscribeMember(string msisdn, string imsi, string imei, string optCode, string otpCode, string appName)
        {
            try
            {
                return OrclHelper.ExecuteNonQuery(AppMain.strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.ISPORTAPP_Insert_Activate"
                     , new OracleParameter[] { OrclHelper.GetOracleParameter("p_phone_no", msisdn, OracleType.VarChar , ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_imsi", imsi, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_imei", imei, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_opt_code", optCode, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_app_name", appName, OracleType.VarChar, ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_otp_code", otpCode, OracleType.VarChar, ParameterDirection.Input)
                                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string  CallSubscribe_IsportSportPool(string optCode, string msisdn,string pssvId)
        {
            string strRequestHTML = "";
            try
            {
                string psnNumber = SqlHelper.ExecuteScalar(AppMain.strConnPack, CommandType.Text, "select * from ufn_get_serviceinfo(" + optCode + ", " + pssvId + ", 'S', '0000')").ToString();
                string strUrl = "", mpCode = new isport_sub.index().bProperty_MPCODE;
                switch (optCode)
                {
                    case "01":
                        strUrl = "http://wap.isport.co.th/cgi/sms/ais/infopack/subscribe.aspx?TRANSID=00000000000001900&CMD=DLVRMSG&FET=IVR&NTYPE=GSM&FROM=" + msisdn + "&TO=" + psnNumber + "&CODE=REQUEST&CTYPE=TEXT&CONTENT=&Act=S&Promotion=D&pssvid="+pssvId+"&mpcode=" + mpCode;
                        break;
                    case "02":
                        string serviceCode = psnNumber.Substring(6) + mpCode;
                        string sNo = psnNumber.Substring(8);

                        strUrl = "http://wap.isport.co.th/cgi/sms/dtac/infopack/subscribe.aspx?RefNo=1900&Msg=" + serviceCode + "&Msn=" + msisdn + "&Sno=" + sNo + "&Encoding=E&MsgType=E&User=i-SPORT&Password=ispOrt&Act=S&Promotion=D&SvNumber=" + psnNumber + "&mpcode=" + mpCode;
                        break;
                    case "03":
                        strUrl = "http://wap.isport.co.th/cgi/sms/orange/infopack/subscribe.aspx?local=spFnwGRz_css&MessageID=0000000001900&To=4511266&From=" + msisdn + "&Content=" + psnNumber + "&ServiceID=0101312062&Act=S&Promotion=D&channel=wap&mpcode=" + mpCode ;
                        break;
                    case "04":
                        strUrl = "http://wap.isport.co.th/cgi/sms/realmove/infopack/subscribe.aspx?local=spFnwGRz_css&MessageID=0000000001900&To=4511266&From=" + msisdn + "&Content=" + psnNumber + "&ServiceID=0101312062&Act=S&Promotion=D&channel=wap&mpcode=" + mpCode ;
                        break;
                }



                System.Net.WebClient v = new System.Net.WebClient();
                Byte[] byteRquestHTML = v.DownloadData(strUrl);
                strRequestHTML = System.Text.UTF8Encoding.UTF8.GetString(byteRquestHTML);
                //Utilities.GetDataXML(
                ExceptionManager.WriteError("URL Request : " + strUrl + " Status >>" + strRequestHTML);
                return strRequestHTML;
            }
            catch (Exception ex)
            {
                throw new Exception("CallSubscribe_IsportStarSoccer>> " + ex.Message);
            }
        }

        public override void GenHeader(string optCode, string projectType)
        {
            lblbanner.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, optCode, projectType, "", "index.aspx", "0"));
        }
        public override void PreGenContent(string optCode, string projectType)
        {
            //throw new NotImplementedException();
        }
        public override void GenFooter(string optCode, string projectType)
        {
            frmMain.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooter(AppMain.strConn, optCode, projectType, "", "index.aspx", "0", bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));
        }
        public override void Subscribe_portal_log(string msisdn, string userAgent, string sgId, string optCode, string prjId, string mpCode, string scsId)
        {
            //throw new NotImplementedException();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string s = "";
        }
    }
}
