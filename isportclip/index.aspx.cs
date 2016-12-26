using System;
using System.Collections;
using System.Collections.Generic;
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
using MobileLibrary;

namespace isportclip
{
    public partial class index : MobileBase
    {
        private string URL
        {
            get
            {
               return this.AbsoluteFilePath + "?lng=" + bProperty_Mobile.bProperty_LNG + "&size=" + bProperty_Mobile.bProperty_SIZE + "&mp_code=" + bProperty_Mobile.bProperty_MPCODE + "&sg=" + bProperty_Mobile.bProperty_SGID + "&prj=" + bProperty_Mobile.bProperty_PRJ +"&scs_id="+bProperty_Mobile.bProperty_SCSID;
            }
        }
        public override void SetHeader()
        {
            // Check Active
            MobileUtilities mU = Utilities.getMISDN(Request);
            if (bProperty_Mobile.bProperty_MPCODE != "0023" || bProperty_Mobile.bProperty_MPCODE != "0050" || bProperty_Mobile.bProperty_MPCODE != "0061")
            {
                if (isport_service.ServiceSMS.MemberSms_CheckActive(
                   AppCodeThaileague.strConnIsportPack, "378"
                   , mU.mobileOPT, mU.mobileNumber).Trim() == "N") Response.Redirect("http://wap.isport.co.th/sport_center/sms/display_service.aspx?lng=L&size=N&mp=0000&sg=203&prj=1&service_group=16&scs_id=2277", false);
            }
            
            // Set Header
            frmMain.Controls.AddAt(frmMain.Controls.Count,Utilities.Images("images/ic_tpl.gif",true,"","Center"));
            frmMain.Controls.AddAt(frmMain.Controls.Count, Utilities.Lable("§≈‘ª‰Œ‰≈∑Ï·≈–´ÁÕ¥‡¥Á¥", true, "Left"));
            frmMain.Controls.AddAt(frmMain.Controls.Count, Utilities.Newline());
            Subscribe_portal_log_Insert(AppCodeThaileague.strConnOracle, mU.mobileNumber, bProperty_Mobile.bProperty_USERAGENT, bProperty_Mobile.bProperty_SGID
                , mU.mobileOPT, bProperty_Mobile.bProperty_PRJ, bProperty_Mobile.bProperty_MP, bProperty_Mobile.bProperty_SCSID);
        }
        public override void SetContent()
        {
            uClipThaileague uClip = new uClipThaileague();
            if (Request["cid"] != null)
            {
                uClip.uDataBind(bProperty_Mobile.bProperty_SCSID, Request["cdate"], Request["cscat"], Request["cid"], URL);
            }
            else if (Request["cscat"] != null)
            {
                uClip.uDataBind(bProperty_Mobile.bProperty_SCSID, Request["cdate"], Request["cscat"], URL);
            }
            else if(Request["mschid"] != null)
            {
                uClip.uDataBind(bProperty_Mobile.bProperty_SCSID, Request["cdate"], int.Parse(Request["mschid"]), URL);
            }
            else if (Request["cdate"] != null)
            {
                uClip.uDataBind(bProperty_Mobile.bProperty_SCSID, Request["cdate"], new string[] { "12" }, URL);
            }
            else if (Request["teamcode"] != null)
            {
                uClip.uDataBindByTeam( Request["teamcode"], URL);
            }
            else
            {
                uClip.uDataBind(bProperty_Mobile.bProperty_SCSID, URL);
            }
            frmMain.Controls.AddAt(frmMain.Controls.Count, uClip);
            
        }
        public override void SetFooter()
        {
            frmMain.Controls.AddAt(frmMain.Controls.Count, Utilities.Newline());
            frmMain.Controls.AddAt(frmMain.Controls.Count, Utilities.Images("images/back.gif", false, "", "Left"));
            frmMain.Controls.AddAt(frmMain.Controls.Count, Utilities.Link(" °≈—∫", true
                , Request.UrlReferrer == null ? URL : Request.UrlReferrer.ToString()
                , "Left"));
            Control[] ctrl = new Control[] { 
                Utilities.Images("images/HeaderBannerfuntong.gif", true, "http://wap.isport.co.th/sport_center/sms/display_service.aspx?lng="+bProperty_Mobile.bProperty_LNG+"&size="+bProperty_Mobile.bProperty_SIZE+"&mp="+bProperty_Mobile.bProperty_MP+"&sg="+bProperty_Mobile.bProperty_SGID+"&prj="+bProperty_Mobile.bProperty_PRJ+"&pssv_id=480", "Center")
            ,Utilities.Images("images/HeaderBannerSms.gif", true, "http://wap.isport.co.th/sport_center/sms/index.aspx?lng="+bProperty_Mobile.bProperty_LNG+"&size="+bProperty_Mobile.bProperty_SIZE+"&mp="+bProperty_Mobile.bProperty_MP+"&sg="+bProperty_Mobile.bProperty_SGID+"&prj="+bProperty_Mobile.bProperty_PRJ, "Center")};

            frmMain.Controls.AddAt(frmMain.Controls.Count, ctrl[new Random().Next(0,2)]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}