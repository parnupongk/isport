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
using WebLibrary;
using MobileLibrary;
using isport_service;

namespace isport_payment
{
    public partial class pay_dtac1 : PageBase
    {
        private string urlRedirect = "", svId = "", cId = "", cpRefId = "";
        //private string cpId = "894";
        private string cpId = "511";
        public override void GenHeader(string optCode, string projectType)
        {
            
        }
        public override void PreGenContent(string optCode, string projectType)
        {
            try
            {

                #region Request Paramenter
                string strPara = "&sg=" + bProperty_SGID + "&lng=" + bProperty_LNG + "&size=" + bProperty_SIZE + "&mp=" + bProperty_MP
                    + "&prj=" + bProperty_PRJ + "&scs_id=" + bProperty_SCSID;
                #endregion

                MobileUtilities mU = Utilities.getMISDN(Request);

                cId = ":" + bProperty_SCSID + ":" + bProperty_SGID + ":" + bProperty_MP + ":" + bProperty_PRJ
                    + ":1:" + DateTime.Now.Second.ToString().PadLeft(1, '0');  //test 20130610

                string str5Bath = "" , str10Bath = "",str50Bath = "";
                string sdpId = Guid.NewGuid().ToString();
                string sdpPara = new AppCode_Dtac_Payment().PaymentDtacSDPInsert_Parameter(sdpId, mU.mobileNumber, bProperty_SGID, bProperty_LNG, bProperty_MP, bProperty_PRJ, bProperty_SCSID, "");

                cpRefId = sdpId;
                cId = "00001";

                if (!(mU.mobileNumber == null || mU.mobileOPT == null))
                {
                    string content = AppCode.GetServiceGroup(bProperty_SGID);
                    if (bProperty_SGID == "98") str5Bath = "ค่าบริการครั้งละ 5 บาท";
                    else if (bProperty_SGID == "95" || bProperty_SGID == "255") str5Bath = "ค่าบริการครั้งละ 5 บาท/เรื่อง";
                    else str5Bath = "ค่าบริการครั้งละ 5 บาท/1 ชั่วโมง";

                    #region Setcontent By content level
                    if (content == "1")
                    {
                        //5 บาท
                        svId = "45110037";
                        urlRedirect = string.Format(ConfigurationManager.AppSettings["dtacAOCRedirectURL"].ToString(), "WPRD", cpId, svId, cpRefId, cId);

                        frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", str5Bath, "pay_dtac.aspx", "0", "", false, ""));
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(isport_service.ServiceWapUI_GenControls.genTextLink("[ OK ]", "", "", urlRedirect, "", "")));

                    }
                    else if (content == "2")
                    {
                        // 10 บาท
                        svId = "45110036";
                        str10Bath = "ค่าบริการครั้งละ 10 บาท/1 ชั่วโมง";
                        urlRedirect = string.Format(ConfigurationManager.AppSettings["dtacAOCRedirectURL"].ToString(), "WPRD", cpId, svId, cpRefId, cId);

                        frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", str10Bath, "pay_dtac.aspx", "0", "", false, ""));
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(isport_service.ServiceWapUI_GenControls.genTextLink("[ OK ]", "", "", urlRedirect, "", "")));
                    }
                    else if (content == "0")
                    {
                        // 5 bath
                        svId = "45110037";
                        str50Bath = (bProperty_SGID == "95" || bProperty_SGID == "255") ? "หรือแบบไม่จำกัดครั้ง 50 บาท/7 วัน" : "หรือแบบไม่จำกัดครั้ง 50 บาท/30 วัน";


                        urlRedirect = string.Format(ConfigurationManager.AppSettings["dtacAOCRedirectURL"].ToString(), "WPRD", cpId, svId, cpRefId, cId);
                        frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", str5Bath, "pay_dtac.aspx", "0", "", false, ""));
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(isport_service.ServiceWapUI_GenControls.genTextLink("[ OK ]", "", "", urlRedirect, "", "")));

                        // 50 bath
                        string svId50 = bProperty_SGID == "95" || bProperty_SGID == "255" ? "45110531" : "45110530";
                        string pId = bProperty_SGID == "95" || bProperty_SGID == "255" ? "45110531001" : "45110530001";

                        urlRedirect = string.Format(ConfigurationManager.AppSettings["dtacAOCSubscribeRedirectURL"].ToString(), "WREG", cpId, pId, cpRefId, cId);
                        frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", str50Bath, "pay_dtac.aspx", "0", "", false, ""));
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl(isport_service.ServiceWapUI_GenControls.genTextLink("[ OK ]", "", "", urlRedirect, "", "")));
                    }
                    else
                    {
                        // ไม่มี service
                        frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", "ขออภัยไม่พบ Service", "pay_dtac.aspx", "0", "", false, ""));
                        
                    }
                    #endregion

                }
                else
                {
                    // ไม่มี Phone Number
                    frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", "ขออภัยไม่พบ Mobile number", "pay_dtac.aspx", "0", "", false, ""));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("pay_datc:setcontent>>" + ex.Message);
            }
        }
        public override void GenFooter(string optCode, string projectType)
        {
            frmMain.Controls.AddAt(frmMain.Controls.Count, new isport_service.ServiceWapUI_Footer().GenFooter(AppCode.GetIsportConnectionString, optCode, projectType, "", "pay_ais.aspx", "0", bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));
        }
        public override void Subscribe_portal_log(string msisdn, string userAgent, string sgId, string optCode, string prjId, string mpCode, string scsId)
        {
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
