using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using WebLibrary;
using MobileLibrary;
using isport_service;

namespace isport_payment
{
    public partial class pay_ais1 : PageBase
    {
        public String PublicEncryption(byte[] data)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //ExceptionManager.WriteError(Server.MapPath("wap_id_rsa_public.xml"));

            if (Request["plink"] == "0")
            {
                rsa.FromXmlString(File.ReadAllText(Server.MapPath("wap_id_rsa_pub.xml")));
            }
            else
            {
                rsa.FromXmlString(File.ReadAllText(Server.MapPath("wap_id_rsa_public.xml")));
            }

            byte[] encryptedData;

            encryptedData = rsa.Encrypt(data, false);
            return Convert.ToBase64String(encryptedData);
        } 

        public override void GenHeader(string optCode, string projectType)
        {
            new isport_service.ServiceWapUI_Header().GenHeader(AppCode.GetIsportConnectionString, optCode, projectType, "", "pay_ais.aspx", "0");
        }

        public override void PreGenContent(string optCode, string projectType)
        {
            string cpSessionId = bProperty_SGID + ":" + bProperty_MP + ":" + bProperty_PRJ + ":" + bProperty_SCSID;

            string cURLConfirm = PublicEncryption(System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["subScribeAOCSMS"]));
            string cURL5B = PublicEncryption(System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["aisRedirectPathURL"] + "pay_ais_redirect.aspx?CPsessionID=" + cpSessionId + ":4511005"));
            string cURL10B = PublicEncryption(System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["aisRedirectPathURL"] + "pay_ais_redirect.aspx?CPsessionID=" + cpSessionId + ":4511007"));
            string cURL50B = PublicEncryption(System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["aisRedirectPathURL"] + "pay_ais_redirect.aspx?CPsessionID=" + cpSessionId + ":4511115"));

            string spsID = PublicEncryption(System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyyMMddHmmssff")));

            if (Request["plink"] != null && Request["plink"] == "3")
            {
                Response.Redirect(ConfigurationManager.AppSettings["aisURLSubScribeAOCSMS"] + "?cmd=s_exp&ch=WAP&SN=" + Request["sn"] + "&spsID="
                + DateTime.Now.ToString("yyyyMMddHmmssff") + "&spName=511&cct=09&cURL=" + Server.UrlEncode(cURLConfirm), false);
            }
            else if (Request["plink"] != null && Request["plink"] == "1")
            {
                // 5 bath
                Response.Redirect(ConfigurationManager.AppSettings["aisSubScribeURL"] + "?cmd=s_exp&ch=WAP&SN=4511005&spsID="
                + Server.UrlEncode(spsID) + "&spName=511&cct=10&cURL=" + Server.UrlEncode(cURL5B), false);
            }
            else if (Request["plink"] != null && Request["plink"] == "2")
            {
                // 10 bath
                Response.Redirect(ConfigurationManager.AppSettings["aisSubScribeURL"] + "?cmd=s_exp&ch=WAP&SN=4511007&spsID="
                + Server.UrlEncode(spsID) + "&spName=511&cct=10&cURL=" + Server.UrlEncode(cURL10B), false);
            }

            else //if (Request["plink"] == null && Request["plink"] == "")
            {
                // ==================================== 5 bath ================================================//
                string txt = "ค่าบริการครั้งละ 5 บาท/1 ชั่วโมง";
                string link = ConfigurationManager.AppSettings["aisSubScribeURL"] + "?cmd=s_exp&ch=WAP&SN=4511005&spsID="
                    + Server.UrlEncode(spsID) + "&spName=511&cct=10&cURL=" + Server.UrlEncode(cURL5B);

                frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", txt , "pay_ais.aspx", "0", "", false, ""));
                frmMain.Controls.AddAt(frmMain.Controls.Count,new LiteralControl( isport_service.ServiceWapUI_GenControls.genTextLink("[ OK ]","","",link,"","")));

                //=================================== 50 bath ================================================//

                txt = (bProperty_SGID == "95" || bProperty_SGID == "255") ? "หรือแบบไม่จำกัดครั้ง 50 บาท/7 วัน" : "หรือแบบไม่จำกัดครั้ง 50 บาท/30 วัน";
                link = ConfigurationManager.AppSettings["aisSubScribeURL"] + "?cmd=s_exp&ch=WAP&SN=4511115&spsID="
                    + Server.UrlEncode(spsID) + "&spName=511&cct=10&cURL=" + Server.UrlEncode(cURL50B);

                frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", txt, "pay_ais.aspx", "0", "", false, ""));
                frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(isport_service.ServiceWapUI_GenControls.genTextLink("[ OK ]", "", "", link, "", "")));
                

            }

            LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "AisSubScribeCDG_Start", "msisdn:" + mU.mobileNumber + " session =" + cpSessionId, "");
        }

        public override void GenFooter(string optCode, string projectType)
        {
            frmMain.Controls.AddAt(frmMain.Controls.Count,new isport_service.ServiceWapUI_Footer().GenFooter(AppCode.GetIsportConnectionString, optCode, projectType, "", "pay_ais.aspx", "0", bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));
        }


        public override void Subscribe_portal_log(string msisdn, string userAgent, string sgId, string optCode, string prjId, string mpCode, string scsId)
        {
            //throw new NotImplementedException();
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
