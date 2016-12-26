using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mobile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using WebLibrary;
namespace isport_payment
{
    public partial class pay_ais : mBasePage
    {

        #region
        //public static RSACryptoServiceProvider rsa;

        public String PublicEncryption(byte[] data)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //ExceptionManager.WriteError(Server.MapPath("wap_id_rsa_public.xml"));
            rsa.FromXmlString(File.ReadAllText(Server.MapPath("wap_id_rsa_public.xml")));

            byte[] encryptedData;

            encryptedData = rsa.Encrypt(data, false);
            return Convert.ToBase64String(encryptedData);
        } 

        #endregion

        public override void SetHeader()
        {
            Setheader( pnlHeader );
        }
        public override void SetContent()
        {
            //bProperty_SIZE + ":" + bProperty_LNG + ":" +
            string cpSessionId =  bProperty_SGID + ":" + bProperty_MP + ":" + bProperty_PRJ + ":" + bProperty_SCSID;

            string cURL5B = PublicEncryption(System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["aisRedirectPathURL"] + "pay_ais_redirect.aspx?CPsessionID=" + cpSessionId + ":4511005"));
            string cURL10B = PublicEncryption(System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["aisRedirectPathURL"] + "pay_ais_redirect.aspx?CPsessionID=" + cpSessionId + ":4511007"));
            string cURL50B = PublicEncryption(System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["aisRedirectPathURL"] + "pay_ais_redirect.aspx?CPsessionID=" + cpSessionId + ":4511115"));

            string spsID = PublicEncryption(System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyyMMddHmmssff")));

            if (Request["plink"] != null && Request["plink"] == "1")
            {
                // 5 bath
                Response.Redirect(ConfigurationManager.AppSettings["aisSubScribeURL"] + "?cmd=s_exp&ch=WAP&SN=4511005&spsID="
                + Server.UrlEncode(spsID) + "&spName=511&cct=10&cURL=" + Server.UrlEncode(cURL5B),false);
            }
            else if (Request["plink"] != null && Request["plink"] == "2")
            {
                // 10 bath
                Response.Redirect(ConfigurationManager.AppSettings["aisSubScribeURL"] + "?cmd=s_exp&ch=WAP&SN=4511007&spsID="
                + Server.UrlEncode(spsID) + "&spName=511&cct=10&cURL=" + Server.UrlEncode(cURL10B),false);
            }

            else //if (Request["plink"] == null && Request["plink"] == "")
            {

                lbl5Bath.Text = "ค่าบริการครั้งละ 5 บาท/1 ชั่วโมง";
                lnk5Bath.NavigateUrl = ConfigurationManager.AppSettings["aisSubScribeURL"] + "?cmd=s_exp&ch=WAP&SN=4511005&spsID="
                    + Server.UrlEncode(spsID) + "&spName=511&cct=10&cURL=" + Server.UrlEncode(cURL5B);

                lbl50Bath.Text = (bProperty_SGID == "95" || bProperty_SGID == "255") ? "หรือแบบไม่จำกัดครั้ง 50 บาท/7 วัน" : "หรือแบบไม่จำกัดครั้ง 50 บาท/30 วัน";
                lnk50Bath.NavigateUrl = ConfigurationManager.AppSettings["aisSubScribeURL"] + "?cmd=s_exp&ch=WAP&SN=4511115&spsID="
                    + Server.UrlEncode(spsID) + "&spName=511&cct=10&cURL=" + Server.UrlEncode(cURL50B);

            }

        }
        public override void SetFooter()
        {
            Setfooter(pnlFooter);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}