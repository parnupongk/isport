using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using OracleDataAccress;
using MobileLibrary;
using isport_service;
using System.Text;
using Newtonsoft.Json;
namespace isport_edt
{
    public partial class detail : PageBase
    {
        public string lat="" , lng = "";
        public string Lat
        {
            get
            {
                return lat;
            }
            set
            {
                lat = value;
            }
        }
        public string Lng
        {
            get
            {
                return lng;
            }
            set
            {
                lng = value;
            }
        }
        DataSet dsData = null;
        private string idPrevious = "", idNext = "";
        private void GetDataEDT()
        {
            try
            {
                if (Session["EDTDATA"] != null) dsData = (DataSet)Session["EDTDATA"];
                else
                {
                    dsData = new AppCode_EDT_Conn().CommandGetEDTBySMSId(Request["sid"]);
                    Session["EDTDATA"] = dsData;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public override void GenHeader(string optCode, string projectType)
        {
            lblbanner.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, optCode, projectType, "", "index.aspx", "0"));
        }
        public override void PreGenContent(string optCode, string projectType)
        {
            GenContent(ProjectType);
        }
        public override void GenFooter(string optCode, string projectType)
        {
            try
            {
                GetDataEDT();
                // add previous and next button
                if (dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsData.Tables[0].Rows.Count; i++)
                    {
                        if (dsData.Tables[0].Rows[i]["xml_id"].ToString() == Request["cid"])
                        {
                            idPrevious = (i > 0) ? dsData.Tables[0].Rows[i - 1]["xml_id"].ToString() : "";
                            idNext = (i == dsData.Tables[0].Rows.Count - 1) ? "" : dsData.Tables[0].Rows[i + 1]["xml_id"].ToString();
                        }
                    }


                }

                string ctn = "<nav>";
                ctn += "<ul class='pager'>";
                ctn += "<li class='previous " + ((idPrevious == "") ? "disabled" : "") + "' ><a href='detail.aspx?sid=" + Request["sid"] + "&cid=" + idPrevious + "'><span aria-hidden='true'>&larr;</span> Older</a></li>";
                ctn += "<li class='next " + ((idNext == "") ? "disabled" : "") + "'><a href='detail.aspx?sid=" + Request["sid"] + "&cid=" + idNext + "'>Newer <span aria-hidden='true'>&rarr;</span></a></li>";
                ctn += "</ul>";
                ctn += "</nav>";

                lblNext.Controls.AddAt(lblNext.Controls.Count, new LiteralControl(ctn));

                //frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div id='modal1' class='modal'><div class='modal-header'>"));
                //frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button>"));
                //frmMain.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(AppMain.strConn, optCode, projectType, "", "detail.aspx?cid=&sid=", "0"));
                //frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("</div></div>"));
                //frmMain.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooterListGroup(AppMain.strConn, optCode, projectType, "", "index.aspx", "0", bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));

                lblFooter.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooterListGroup(AppMain.strConn, optCode, "edtguru", "", "index.aspx", "0", bProperty_MPCODE, bProperty_PRJ, bProperty_SCSID, Request["class_id"]));

            }
            catch (Exception ex)
            {
                throw new Exception("GenFooter >> " + ex.Message);
            }
        }
        public override void Subscribe_portal_log(string msisdn, string userAgent, string sgId, string optCode, string prjId, string mpCode, string scsId)
        {
            isport_service.ServiceWap_Logs.Subscribe_portal_log(AppMain.strConnOracle, mU.mobileNumber, bProperty_USERAGENT
                            , "282", mU.mobileOPT, "1", bProperty_MPCODE, "");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //GetDataEDT();
            bProperty_MPCODE = bProperty_MPCODE == "0025" ? "0804" : bProperty_MPCODE;
            if (Request["m"] != null) Response.AppendCookie(new HttpCookie("User-Identity-Forward-msisdn", Request["m"]));
            mU = Utilities.getMISDN(Request);

            string ipAllow = ConfigurationManager.AppSettings["IsportAllowIP"];
            string[] ipCurrents = Request.ServerVariables["REMOTE_ADDR"].ToString().Split('.');
            string ipCurrent = (ipCurrents.Length > 3) ? ipCurrents[0] + "." + ipCurrents[1] + "." + ipCurrents[2] : Request.ServerVariables["REMOTE_ADDR"].ToString();

            if (mU.mobileNumber != "" || ipAllow.IndexOf(ipCurrent) > -1)
            {
                string check = ServiceSMS.MemberSms_CheckActive(AppMain.strConnPack, ConfigurationManager.AppSettings["PSSVID"], mU.mobileOPT, mU.mobileNumber);
                if (check == "Y")
                {
                    // write content
                    GenContent(ProjectType);
                }
                else
                {
                    // page subscribe
                    Response.Redirect("http://edtguide.net?p=edtguide", false);
                }
            }
            else if (Request["o"] == null && Request["m"] == null) Response.Redirect(ConfigurationManager.AppSettings["URLGetMSISDN"] + "/detail.aspx?"+Request.QueryString, false);
            //else GenMessage("ขออภัยค่ะ กรุณาใช้ Internet จากเครื่องโทรศัพท์" + mU.mobileNumber, mU.mobileOPT);

        }

        private void GenContent(string projectType)
        {
            try
            {
                if (Request["cid"] != null && Request["cid"] != "")
                {

                    string urlContent = string.Format(ConfigurationManager.AppSettings["URLCONTENT"], Request["cid"]);
                    string xml = new isport_service.sendService.push().SendGet(urlContent);
                    AppCode_EDT edt = Newtonsoft.Json.JsonConvert.DeserializeObject<AppCode_EDT>(xml);

                    #region Content Detail

                    lblImgCover.Controls.AddAt(lblImgCover.Controls.Count, isport_service.ServiceWapUI_GenControls.genImagesHeader("detail.aspx", edt.image_cover, "img-full", "0", projectType, false, ""));
                    lblBodyDetail.Text = edt.detail;
                    lblBodyHeader.Text = edt.name;
                    lblTitle.Controls.AddAt(lblTitle.Controls.Count, isport_service.ServiceWapUI_GenControls.genText("images/" + edt.type + ".png", edt.name, "", "0", projectType, false, ""));

                    string ctn = "";
                    Lat = edt.lat;
                    Lng = edt.lng;
                    try
                    {
                        if (edt.business != null && edt.business.Trim() != "") ctn += "<div class='list-group-item'><h4>ประเภทธุรกิจ : <small>" + edt.business + "</small></h4></div>";
                        if (edt.address != null && edt.address.Trim() != "") ctn += "<div class='list-group-item'><h4>ที่อยู่ : <small>" + edt.address + "</small></h4></div>";
                        if (edt.tel != null && edt.tel.Trim() != "") ctn += "<div class='list-group-item'><h4>เบอร์โทรศัพท์ : <small>" + edt.tel + "</small></h4></div>";
                        if (edt.web != null && edt.web.Trim() != "") ctn += "<div class='list-group-item'><h4>เว็บไซต์ : <small>" + edt.web + "</small></h4></div>";
                        if (edt.transport != null && edt.transport.Trim() != "") ctn += "<div class='list-group-item'><h4>การเดินทาง : <small>" + edt.transport + "</small></h4></div>";
                        if (edt.lat != null && edt.lat.Trim() != "") ctn += "<div class='list-group-item'><h4>นำทาง : <small><a href='http://maps.google.com/?q=" + Lat + "," + Lng + "' >นำทาง</a></small></h4></div>";
                        if (edt.location != null && edt.location.Trim() != "") ctn += "<div class='list-group-item'><h4>สถานที่ตั้ง/ทำเล : <small>" + edt.location + "</small></h4></div>";
                        if (edt.opentime != null && edt.opentime.Trim() != "") ctn += "<div class='list-group-item'><h4>วันเวลาเปิด-ปิด : <small>" + edt.opentime + "</small></h4></div>";
                    }
                    catch { }
                    lblContentDetail.Controls.AddAt(lblContentDetail.Controls.Count, new LiteralControl(ctn));

                    if (edt.type == "eat" || edt.type == "drink")
                    {
                        try
                        {
                            lblOtherDetail.Controls.AddAt(lblOtherDetail.Controls.Count, new LiteralControl(SetEatContent(edt)));
                        }
                        catch { }
                    }
                    else if (edt.type == "travel")
                    {
                        try
                        {
                            lblOtherDetail.Controls.AddAt(lblOtherDetail.Controls.Count, new LiteralControl(SetTravleContent(edt)));
                        }
                        catch { }
                    }

                    #endregion

                    int thumbCount = 0;
                    foreach (string imgThumb in edt.gallery_thumb)
                    {
                        if (thumbCount < 15)
                        {
                            lblImgConverThumb.Controls.AddAt(lblImgConverThumb.Controls.Count, new LiteralControl("<img class='img-gallery' src='" + imgThumb + "' onclick=javascript:$('#modal" + thumbCount.ToString() + "').modal('show'); alt='' />"));

                            // modal show image
                            frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div id='modal" + thumbCount.ToString() + "' class='modal'><div class='modal-header'>"));
                            frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button>"));
                            frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<img class='img-full' src='" + edt.gallery[thumbCount] + "' alt='' />"));
                            frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("</div></div>"));
                        }
                        thumbCount++;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("GenContent>>" + ex.Message);
            }
        }
        private string  SetEatContent(AppCode_EDT edt )
        {
            string ctn = "";
            if (edt.menu_guide.Trim() != "") ctn += "<div class='list-group-item'><h4>เมนูแนะนำ : <small>" + edt.menu_guide + "</small></h4></div>";
            if (edt.type_fo_food.Trim() != "") ctn += "<div class='list-group-item'><h4>ประเภทอาหาร : <small>" + edt.type_fo_food + "</small></h4></div>";
            if (edt.national_food.Trim() != "") ctn += "<div class='list-group-item'><h4>สัญชาติอาหาร : <small>" + edt.national_food + "</small></h4></div>";
            if (edt.other_services.Trim() != "") ctn += "<div class='list-group-item'><h4>บริการอื่นๆ : <small>" + edt.other_services + "</small></h4></div>";
            if (edt.type_of_music.Trim() != "") ctn += "<div class='list-group-item'><h4>ประเภทดนตรี : <small>" + edt.type_of_music + "</small></h4></div>";
            if (edt.no_of_table.Trim() != "") ctn += "<div class='list-group-item'><h4>จำนวนโต๊ะ : <small>" + edt.no_of_table + "</small></h4></div>";
            if (edt.alcohol.Trim() != "") ctn += "<div class='list-group-item'><h4>เครื่องดื่มแอลกอฮอล์ : <small>" + edt.alcohol + "</small></h4></div>";
            if (edt.corkage_fee.Trim() != "") ctn += "<div class='list-group-item'><h4>ค่าเปิดขวด : <small>" + edt.corkage_fee + "</small></h4></div>";
            if (edt.payment.Trim() != "") ctn += "<div class='list-group-item'><h4>วิธีการชำระเงิน : <small>" + edt.payment + "</small></h4></div>";

            return ctn;
        }
        private string  SetTravleContent(AppCode_EDT edt)
        {
            string ctn = "";
            if (edt.rooms.Trim() != "") ctn += "<div class='list-group-item'><h4>จำนวนห้องพัก : <small>" + edt.rooms + "</small></h4></div>";
            if (edt.facilities.Trim() != "") ctn += "<div class='list-group-item'><h4>สิ่งอำนวยความสะดวก : <small>" + edt.facilities + "</small></h4></div>";
            if (edt.lowest_rate.Trim() != "") ctn += "<div class='list-group-item'><h4>ราคาห้องต่ำสุด : <small>" + edt.lowest_rate + "</small></h4></div>";
            if (edt.hightest_rate.Trim() != "") ctn += "<div class='list-group-item'><h4>ราคาห้องสูงสุด : <small>" + edt.hightest_rate + "</small></h4></div>";
            if (edt.purpose.Trim() != "") ctn += "<div class='list-group-item'><h4>เหมาะสำหรับ : <small>" + edt.purpose + "</small></h4></div>";
            if (edt.travel_duration.Trim() != "") ctn += "<div class='list-group-item'><h4>ช่วงเวลาที่เหมาะแก่การท่องเที่ยว : <small>" + edt.travel_duration + "</small></h4></div>";
            if (edt.local_food.Trim() != "") ctn += "<div class='list-group-item'><h4>อาหารพื้นบ้าน : <small>" + edt.local_food + "</small></h4></div>";
            if (edt.souvenir.Trim() != "") ctn += "<div class='list-group-item'><h4>ของฝาก : <small>" + edt.souvenir + "</small></h4></div>";
            
            return ctn;
        }
    }
}
