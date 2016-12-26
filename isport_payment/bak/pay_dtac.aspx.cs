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
using MobileLibrary;
using WebLibrary;
namespace isport_payment
{
    public partial class pay_dtac : mBasePage
    {
        private string urlRedirect = "", svId = "", cId = "", cpRefId = "";
        //private string cpId = "894";
        private string cpId = "511";
        public override void SetHeader()
        {
            Setheader(pnlHeader);
        }
        public override void SetContent()
        {
            try
            {

                #region Request Paramenter
                string strPara = "&sg=" + bProperty_SGID + "&lng=" + bProperty_LNG + "&size=" + bProperty_SIZE + "&mp=" + bProperty_MP
                    + "&prj=" + bProperty_PRJ + "&scs_id=" + bProperty_SCSID;
                #endregion

                MobileUtilities mU = Utilities.getMISDN(Request);
                //cId = ":"+bProperty_SIZE + ":" + bProperty_LNG + ":" + bProperty_SGID + ":" + bProperty_MP + ":" + bProperty_PRJ
                //    + ":" + bProperty_SCSID + ":" + "1";// +":" + DateTime.Now.Second.ToString().PadLeft(2, '0');  // ตัด SCSID เพราะทำให้เกิน 32 DIGI

                //cId = ":" + bProperty_SIZE + ":" + bProperty_LNG + ":" + bProperty_SGID + ":" + bProperty_MP + ":" + bProperty_PRJ
                //    + ":1:" + DateTime.Now.Second.ToString().PadLeft(1, '0') ;  Production

                cId = ":" + bProperty_SCSID + ":" + bProperty_SGID + ":" + bProperty_MP + ":" + bProperty_PRJ
                    + ":1:" + DateTime.Now.Second.ToString().PadLeft(1, '0') ;  //test 20130610

                string sdpId = Guid.NewGuid().ToString();
                string sdpPara = new AppCode_Dtac_Payment().PaymentDtacSDPInsert_Parameter(sdpId, mU.mobileNumber, bProperty_SGID, bProperty_LNG, bProperty_MP, bProperty_PRJ, bProperty_SCSID, "");

                //cpRefId = bProperty_SCSID + ":" + new AppCode_Dtac_Payment().CreateTransaction_Dtac;
                cpRefId = sdpId;//+ ":" + new AppCode_Dtac_Payment().CreateTransaction_Dtac;
                cId = "00001";

                if (!(mU.mobileNumber == null || mU.mobileOPT == null))
                {
                    string content = AppCode.GetServiceGroup(bProperty_SGID);
                    if (bProperty_SGID == "98") lbl5Bath.Text = "ค่าบริการครั้งละ 5 บาท";
                    else if (bProperty_SGID == "95" || bProperty_SGID == "255") lbl5Bath.Text = "ค่าบริการครั้งละ 5 บาท/เรื่อง";
                    else lbl5Bath.Text = "ค่าบริการครั้งละ 5 บาท/1 ชั่วโมง";

                    #region Setcontent By content level
                    if (content == "1")
                    {
                        //5 บาท
                        //svId = "4511001020"; production

                        svId = "45110037";
                        //cId = svId + cId;
                        //cId = "4511000030:N:L:95:0000:1::1";
                        
                        //urlRedirect = string.Format(ConfigurationManager.AppSettings["dtacAOCRedirectURL"].ToString(), "WPRD", cpId, svId, cId, cpRefId);
                        urlRedirect = string.Format(ConfigurationManager.AppSettings["dtacAOCRedirectURL"].ToString(), "WPRD", cpId, svId, cpRefId, cId);

                        lnk5Bath.NavigateUrl = urlRedirect; //"pay_dtac_aoc.aspx?service_id=4511000030&type=ses" + strPara;
                        pnlContent.Controls.AddAt(pnlContent.Controls.Count, lbl5Bath);
                        pnlContent.Controls.AddAt(pnlContent.Controls.Count, lnk5Bath);
                    }
                    else if (content == "2")
                    {
                        // 10 บาท
                        lbl10Bath.Text = "ค่าบริการครั้งละ 10 บาท/1 ชั่วโมง";
                        //svId = "4511001010"; Production
                        // test 20130610
                        svId = "45110036";

                        //cId = svId + cId;
                        //cId = "4511001010:N:L:142:0000:1::1";
                        //urlRedirect = string.Format(ConfigurationManager.AppSettings["dtacAOCRedirectURL"].ToString(), "WPRD", cpId, svId, cId, cpRefId);

                        urlRedirect = string.Format(ConfigurationManager.AppSettings["dtacAOCRedirectURL"].ToString(), "WPRD", cpId, svId, cpRefId, cId);

                        lnk10Bath.NavigateUrl = urlRedirect;//"pay_dtac_aoc.aspx?service_id=4511000010&type=ses" + strPara;
                        pnlContent.Controls.AddAt(pnlContent.Controls.Count, lbl10Bath);
                        pnlContent.Controls.AddAt(pnlContent.Controls.Count, lnk10Bath);
                    }
                    else
                        if (content == "0")
                        {
                            // 5,50 บาท
                            lbl50Bath.Text = (bProperty_SGID == "95" || bProperty_SGID == "255") ? "หรือแบบไม่จำกัดครั้ง 50 บาท/7 วัน" : "หรือแบบไม่จำกัดครั้ง 50 บาท/30 วัน";
                            // 5 บาท
                            //svId = "4511001020";

                            svId = "45110037"; 
                            // test service app 4511000213 ,  5 bath ปกติ 4511001020
                            //cId = "4511001020:N:L:95:0000:1::1";

                            //urlRedirect = string.Format(ConfigurationManager.AppSettings["dtacAOCRedirectURL"].ToString(), "WPRD", cpId, svId, svId + cId, cpRefId);
                            //cpRefId = "00001";
                            //cId = "00001";

                            urlRedirect = string.Format(ConfigurationManager.AppSettings["dtacAOCRedirectURL"].ToString(), "WPRD", cpId, svId, cpRefId,cId);

                            lnk5Bath.NavigateUrl = urlRedirect;//"pay_dtac_aoc.aspx?service_id=4511000030&type=ses" + strPara;

                            // 50
                            //svId = "000100";
                            //string svId50 = bProperty_SGID == "95" ? "8451120500001" : "8451110500001"; Production

                            string svId50 = bProperty_SGID == "95" || bProperty_SGID == "255" ? "45110531" : "45110530";
                            string pId = bProperty_SGID == "95" || bProperty_SGID == "255" ? "45110531001" : "45110530001";
                            // test 20130610 45110530 50/M ,45110531 50/W 

                            // (845110002100001 => 10 bath/week) (8451120500001 => 50B/Week) sport( 8451110500001 => 50B/M)

                            //svId = bProperty_SGID == "95" ? "1" : "2";
                            // 1 = 50B7Day (8451120500001) , 2 = 50B30Day (8451110500001)

                            //cId = svId + cId;
                            //cId = bProperty_SGID == "95" ? "1:N:L:95:0000:1::1" : "2:N:L:142:0000:1::1";

                            //cpRefId = cId;
                            //urlRedirect = string.Format(ConfigurationManager.AppSettings["dtacAOCRedirectURL"].ToString(), "WREG", cpId, svId50, cId, cpRefId); // Production
                            //cpRefId = "00001";
                            //cpRefId = bProperty_SGID;

                            urlRedirect = string.Format(ConfigurationManager.AppSettings["dtacAOCSubscribeRedirectURL"].ToString(), "WREG", cpId, pId, cpRefId, cId);
                            lnk50Bath.NavigateUrl = urlRedirect;//"pay_dtac_redirect.aspx?service_id=000100&type=sub" + strPara;
                            pnlContent.Controls.AddAt(pnlContent.Controls.Count, lbl5Bath);
                            pnlContent.Controls.AddAt(pnlContent.Controls.Count, lnk5Bath);
                            pnlContent.Controls.AddAt(pnlContent.Controls.Count, lbl50Bath);
                            pnlContent.Controls.AddAt(pnlContent.Controls.Count, lnk50Bath);
                        }
                        else
                        {
                            // ไม่มี service
                            lbl5Bath.Text = "ขออภัยไม่พบ Service";
                            lbl5Bath.StyleReference = "messageError";
                            pnlContent.Controls.AddAt(pnlContent.Controls.Count, lbl5Bath);
                        }
                    #endregion

                }
                else
                {
                    // ไม่มี Phone Number
                    lbl5Bath.Text = "ขออภัยไม่พบ Mobile number";
                    lbl5Bath.StyleReference = "messageError";
                    pnlContent.Controls.AddAt(pnlContent.Controls.Count, lbl5Bath);
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("pay_datc:setcontent>>"+ex.Message);
            }
        }

        public override void SetFooter()
        {
            Setfooter(pnlFooter);

            if (Request["type"] != null && Request["type"].ToString() == "test")
            {
                System.Web.UI.MobileControls.Image img = new System.Web.UI.MobileControls.Image();
                img.ImageUrl = "images/icon_dtac.jpg";
                img.BreakAfter = false;

                string url = string.Format(System.Configuration.ConfigurationManager.AppSettings["dtacLinkBack"], svId, cId);
                Link lnk = new Link();
                lnk.Text = "Back to Dtac";
                lnk.NavigateUrl = url;

                pnlContent.Controls.AddAt(pnlContent.Controls.Count, img);
                pnlContent.Controls.AddAt(pnlContent.Controls.Count, lnk);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}