using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web;
using Microsoft.ApplicationBlocks.Data;
using MobileLibrary;
namespace isport_service
{
    /*=============================================================================

     *                    content_link ต้องเปลี่ยน & เป็น | ให้หมด
     ==============================================================================* */
    public class ServiceWapUI_Content
    {

        /// <summary>
        /// GenContent
        /// </summary>
        /// <param name="strConn"></param>
        /// <param name="strOrclConn"></param>
        /// <param name="masterId"></param>
        /// <param name="level"></param>
        /// <param name="operatorType"></param>
        /// <param name="projectType"></param>
        /// <param name="pageRedirect"></param>
        /// <param name="pageMaster">Default "0"</param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public Control GenContent(string strConn,string strOrclConn, string masterId, string level, string operatorType, string projectType, string pageRedirect,string pageMaster,string classId)
        {
            try
            {
                string classImg = "img";
                string imgControl = "";
                string imgGallery = "";
                bool isGallery = false , isBeginGroup = false , isCloseGroup = false ;
                MobileUtilities mU = Utilities.getMISDN(System.Web.HttpContext.Current.Request);
                Control ctr = new Control();
                DataSet ds = new AppCode().SelectUIByLevel_Wap(strConn, masterId, level, operatorType, projectType);

                ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row'>"));

                DataView dv = ds.Tables[0].DefaultView;
                dv.RowFilter = new System.Text.StringBuilder().Append("ui_ispayment =true").ToString();

                for (int index=0 ; index < ds.Tables[0].Rows.Count ;index++)
                {
                    DataRow dr = ds.Tables[0].Rows[index];

                    if ((bool)dr["ui_ispayment"]) // isHeader 
                    {
                        // เริ่ม group ใหม่
                        if (index > 0 && isBeginGroup)
                        {
                            ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("</div>"));
                            isCloseGroup = true;
                        }
                        isBeginGroup = true;
                        switch (dv.Count)
                        {
                            case 2: ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='col-xs-12 col-md-6 col-sm-6'>")); break;
                            case 3: ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='col-xs-12 col-md-4 col-sm-6'>")); break;
                            default: ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='col-xs-12 col-md-4 col-sm-6'>")); break;
                        }
                        
                        isCloseGroup = false;
                    }

                    
                    string linkContent = "",link = "" ,mpCode="" ,imgURL = "";
                    string rowCSS = dr["content_bgcolor"] == null || dr["content_bgcolor"].ToString() == "" ? "row" : "row_" + dr["content_bgcolor"].ToString() + "_" + dr["content_align"].ToString();
                    string txtCSS = dr["content_txtsize"] == null || dr["content_txtsize"].ToString() == "" ? "p" : dr["content_txtsize"].ToString();


                    if (dr["content_link"].ToString().IndexOf(".mp4") > -1)
                    {
                        // mp4 ให้แสดงเป็น tag vedio เลย
                        string wowza = dr["content_link"].ToString().IndexOf("zero") > -1 ? "http://203.149.53.75/isport/content_free/" : "http://203.149.53.75/isport/content/";

                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><video width='100%' autoplay controls poster='full/http/link/to/image/file.png'><source src='" + wowza + dr["content_link"].ToString() + "' type='video/mp4'/>Your browser does not support the video tag.</video></div>"));

                        //break;

                    }
                    else if (dr["content_link"].ToString().IndexOf(".3gp") > -1)
                    {
                        link = "http://203.149.53.75/isport/content/" + dr["content_link"].ToString();
                    }
                    else
                    {
                        link = GenLink(pageRedirect, dr["content_link"].ToString(), mU.mobileNumber, dr["ui_sg_id"].ToString(),mU.mobileOPT);

                    }

                    imgURL = dr["content_image"] == null || dr["content_image"].ToString() == "" ? imgURL : dr["content_image"].ToString();
                    imgURL = imgURL.IndexOf("http") > -1 ? imgURL : "http://wap.isport.co.th/isportui/" + imgURL.Replace("~/", "") ;

                    if (dr["content_link"] != null && dr["content_link"].ToString() != "")
                    {

                        #region Gen Link
                        // =================Gen Link ================
                        //
                        //             ** ถ้าเป็น link สมัครบริการต้อง check msisdn ถ้าไม่มีจะไม่ show link สมัคร
                        //
                        //=======================================
                        if (dr["content_isredirect"].ToString() != "" && (bool)dr["content_isredirect"])
                        {
                            HttpContext.Current.Response.Redirect(link, false);
                        }
                        else if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                        {

                            #region Image Link
                            classImg = "img";

                                if (dr["content_align"].ToString() == "Center")
                                {
                                    classImg = "img-center";
                                }
                                else if (dr["content_align"].ToString() == "Right")
                                {
                                    classImg = "img-right";
                                }
                            if ((bool)dr["content_breakafter"])
                            {
                                if (imgControl == "")
                                {
                                    if (dr["content_text"].ToString() == "")
                                    {
                                        if (dr["ui_contentname"].ToString() != "subscribe")
                                        {
                                            ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><a href='" + link + "'><img class='" + classImg + "' src='" + imgURL + "'></a></div>"));
                                        }
                                        else //if (mU.mobileNumber != "")                       
                                        {
                                             
                                            ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><a href='" + link + "'><img class='" + classImg + "' src='" + imgURL + "'></a></div>"));
                                        }
                                    }
                                    else
                                    {
                                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='media'><a href='" + link + "'><img class='pull-left' src='" + imgURL + "'><div class='media-body'>" + dr["content_text"] + "</div></a></div>"));
                                    }
                                    imgControl = "";
                                }
                                else
                                {
                                    imgControl += "<a href='" + link + "'><img class='img' src='" + imgURL + "'></a>";
                                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='"+rowCSS+"'>" + imgControl + "</div>"));
                                }
                                imgControl = "";
                            }
                            else
                            {
                                imgControl += "<a href='" + link + "'><img class='img' src='" + imgURL + "'></a>";
                            }

                            #endregion

                        }
                        else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                        {

                            #region Text Link
                            if ((bool)dr["content_breakafter"])
                            {
                                if (imgControl == "")
                                {
                                    // Text link
                                    if (dr["content_icon"] != null && dr["content_icon"].ToString() != "")
                                    {
                                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><" + txtCSS + "><img class='img' src='http://wap.isport.co.th/isportui/" + dr["content_icon"].ToString().Replace("~/", "") + "'><a class='img' href='" + link + "'>" + dr["content_text"].ToString() + "</" + txtCSS + "></a></div>"));
                                    }
                                    else
                                    {
                                        if (dr["ui_contentname"].ToString() != "subscribe")
                                        {
                                            ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><a href='" + link + "'><" + txtCSS + ">" + dr["content_text"].ToString() + "</" + txtCSS + "></a></div>"));
                                        }
                                        else //if (mU.mobileNumber != "")
                                        {
                                            string btnClass = "button-download";
                                            switch(dr["content_color"].ToString() )
                                            {
                                                case "Red": btnClass += "-Red";break;
                                                case "Blue": btnClass += "-Blue"; break;
                                                case "Green": btnClass += "-Green"; break;
                                                case "Yellow": btnClass += "-Yellow"; break;
                                            }
                                            ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><a class='"+ btnClass + "' href='" + link + "'><" + txtCSS + ">" + dr["content_text"].ToString() + "</" + txtCSS + "></a></div>"));
                                        }
                                    }
                                }
                                else
                                {
                                    imgControl += " &nbsp;<a  href='" + link + "'><" + txtCSS + ">" + dr["content_text"].ToString() + "</" + txtCSS + "></a>";
                                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='"+rowCSS+"'>" + imgControl + "</div>"));
                                    imgControl = "";
                                }
                            }
                            else
                            {
                                imgControl += " &nbsp;<a  href='" + link + "'><" + txtCSS + ">" + dr["content_text"].ToString() + "</" + txtCSS + "></a>";
                            }

                            #endregion

                        }

                        #endregion

                    }
                    else
                    {

                        if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                        {

                            #region Gen Images
                            if (masterId == "0")
                            {
                                if (dr["content_align"].ToString() == "Left" && (bool)dr["ui_ispayment"])
                                {
                                    classImg = "img-full";
                                }
                                else if (dr["content_align"].ToString() == "Center")
                                {
                                    classImg = "img-center";
                                }
                                else if (dr["content_align"].ToString() == "Right")
                                {
                                    classImg = "img-right";
                                }
                            }

                            if (dr["content_isgallery"].ToString() != "" && (bool)dr["content_isgallery"])
                            {

                                classImg = (imgGallery == "") ? "img-center" : "img-gallery"; // รูปแรกต้องให้เต็มหน้าจอ
                                if (index == ds.Tables[0].Rows.Count - 1 && imgGallery != "")
                                {
                                    //rowCSS += "_" + dr["content_align"];
                                    imgGallery += "<a href='" + imgURL + "'><img class='" + classImg + "' src='" + imgURL + "'></a>";
                                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'>" + imgGallery + "</div>"));
                                    imgGallery = "";
                                }
                                else
                                    imgGallery += "<a href='" + imgURL + "'><img class='" + classImg + "' src='" + imgURL + "'></a>";

                            }
                            else if (dr["content_image"].ToString().IndexOf("pdf") > -1 )
                            {
                                // download pdf 
                                ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><a href='" + imgURL + "'><img class='" + classImg + "' src='images/pdf-download.jpg'></a></div>"));
                            }
                            else ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genImagesHeader(dr, link, level, projectType, (bool)dr["ui_ismaster"], classImg));

                            #endregion

                        }

                        if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                        {

                            #region Text
                            //genText(dr);
                            if (imgControl == "")
                            {
                                ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genText(dr, pageRedirect, level, projectType, (bool)dr["ui_ismaster"], (bool)dr["ui_ispayment"]));
                            }
                            else
                            {
                                imgControl += "<" + txtCSS + ">" + dr["content_text"].ToString() + "</" + txtCSS + ">";
                                ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'>" + imgControl + "</div>"));
                                imgControl = "";
                            }
                            #endregion

                        }

                        
                    }

                    isGallery = (dr["content_isgallery"].ToString() != "" && (bool)dr["content_isgallery"]) ? (bool)dr["content_isgallery"] :  false ;
                    if (imgGallery != "" && !isGallery)
                    {

                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'>" + imgGallery + "</div>"));
                        imgGallery = "";

                    }


                    #region Gen Content by Content Type

                    genNews(dr,strOrclConn,ctr,classId);

                    if (dr["ui_contentname"].ToString() == "footballlivescore")
                    {
                        genScore(strConn, ctr, classId ,projectType, (bool)dr["ui_ispayment"],pageRedirect);
                    }
                    else if (dr["ui_contentname"].ToString() == "footballprogram")
                    {
                        genProgram(strConn, ctr, classId, projectType, (bool)dr["ui_ispayment"], pageRedirect);
                    }
                    else if (dr["ui_contentname"].ToString() == "footballtable")
                    {
                        ServiceTable.genTable(strConn, ctr, classId, projectType, (bool)dr["ui_ispayment"], pageRedirect);
                    }
                    else if (dr["ui_contentname"].ToString() == "analyse")
                    {
                        // analyse by sg_id , dr["ui_sg_id"].ToString()
                        ServiceAnalyse.GenAnalyse(strConn, ctr, dr["ui_sg_id"].ToString(), classId, projectType, (bool)dr["ui_ispayment"], link);
                    }
                    else if (dr["ui_contentname"].ToString() == "topscore")
                    {
                        ServiceTopScore.GenFootballTopScore(strConn, ctr, classId, projectType, (bool)dr["ui_ispayment"], link);
                    }
                    else if (dr["ui_contentname"].ToString() == "tded")
                    {
                        ServiceTded.GenTded(strConn, ctr, dr["ui_sg_id"].ToString(), classId, projectType, (bool)dr["ui_ispayment"], link);
                    }

                    #endregion

                }


                if (isBeginGroup && !isCloseGroup)
                {
                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("</div>"));
                }

                ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("</div>"));

                return ctr;
            }
            catch (Exception ex)
            {
                throw new Exception("isport_service.GenContent >>" + ex.Message);
            }
        }

        #region Gen Link
        private string GenLink(string strPageRedirect,string strDbLink,string strMobile,string strSgID,string strOptCode)
        {
            try
            {
                string[] links = strDbLink.Split('|');
                string linkContent, mpCode,link;
                if (links.Length > 1 )
                {
                    if( strMobile != "") //&& ((DateTime.Now.Hour > 21 && DateTime.Now.Hour<24) || (DateTime.Now.Hour > 0 && DateTime.Now.Hour < 6) )
                    {
                        linkContent = links[0];
                    }
                    else
                    {
                        linkContent = links[1];
                    }
                }
                else
                {
                    linkContent = links[0];
                }

                linkContent = linkContent.Replace('&', '|');
                mpCode = System.Web.HttpContext.Current.Request["mp_code"] == null ? "" : System.Web.HttpContext.Current.Request["mp_code"];
                if(mpCode != "" && strOptCode.Replace("'","") =="01")
                {
                    mpCode = "00" + mpCode;
                }
                link = strPageRedirect + "&sg=" + strSgID + "&r=" + linkContent.Replace("?mp_code", mpCode);
                return link;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Gen Image  Content
        private string genImageContent(Control ctr, DataRow dr, int rowCurrent, int rowCount, string imgControl, string rowCSS, string masterId, string link,string  level,string  projectType)
        {
            try
            {
                string classImg = "img";
                
                return imgControl;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Program
        private void genProgram(string strConn, Control ctr, string classId, string projectType, bool isHeader, string pageRedirect)
        {
            try
            {
                string[] strClassId = classId.Split(',');
                if (strClassId.Length > 0)
                {
                    foreach (string str in strClassId)
                    {
                        DataSet ds = new ServiceProgram().FootballProgram(strConn, str, "L");
                        string team = "",cssName= "rowprogramdetail_";
                        
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            #region order by date
                            if (str == "")
                            {
                                DataView dv = ds.Tables[0].DefaultView;
                                //dv.Sort = " scs_name";
                                ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'><h4> โปรแกรมการแข่งขันวันที่ " + AppCode.DatetoText(ds.Tables[0].Rows[0]["match_datetime"].ToString(), AppCode.Country.th.ToString()) + "</h4></div></div>"));
                                ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'>คลิกที่ชื่อทีมเพื่อดูสถิติ</div></div>"));
                                int index = 1;

                                foreach (DataRowView dr in dv)
                                {

                                    if (team != dr["scs_name"].ToString())
                                    {
                                        index = 1;
                                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'><h4>" + dr["scs_name"].ToString() + "</h4></div></div>"));
                                    }

                                    cssName = (index % 2) == 0 ? "rowprogramdetail_" : "rowprogramdetail";
                                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='"+ cssName + "'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1></div><a  class='linkprogram' href='football_static.aspx?" + HttpContext.Current.Request.QueryString + "&msch_id=" + dr["msch_id"] + "'><div class=col-xs-11 col-sm-11 col-lg-11 col-md-11>" + dr["match_time"] + " " + dr["teamName1_th"] + " VS " + dr["teamName2_th"] + "</div></a></div>"));

                                    team = dr["scs_name"].ToString();

                                    index++;
                                }
                            }
                            else
                            {
                                ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'><h4> โปรแกรมการแข่งขัน</h4></div></div>"));
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {

                                    if (team != dr["match_datetime"].ToString())
                                    {
                                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'><h4>" + dr["scs_name"].ToString() + " " + AppCode.DatetoText(dr["match_datetime"].ToString(), AppCode.Country.th.ToString()) + "</h4></div></div>"));
                                    }
                                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row featurette'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1></div><div class=col-xs-11 col-sm-11 col-lg-11 col-md-11>" + dr["match_time"] + " " + dr["teamName1_th"] + " VS " + dr["teamName2_th"] + "</div></a></div>"));

                                    team = dr["match_datetime"].ToString();


                                }
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genTextHeader("", "ขออภัยไม่พบข้อมูลการแข่งขันค่ะ", pageRedirect, "0", projectType, false, ""));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("genProgram >> " + ex.Message);
            }
        }
        #endregion

        #region Score
        private void genScore(string strConn, Control ctr,string classId,string projectType,bool isHeader,string pageRedirect)
        {
            try
            {
                TimeSpan time = DateTime.Now.TimeOfDay - new TimeSpan(01, 00, 00);
                TimeSpan times = DateTime.Now.TimeOfDay - new TimeSpan(17, 00, 00);
                string strAddTime = times.Hours < 0 && time.Hours >= 0 ? "" : "+1";
                classId = classId == null ? "" : classId;
                DataSet ds = new ServiceScore().FootballLiveScoreSelectbyClass(strConn, classId, strAddTime, " 16:00");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'><h1>ผลฟุตบอลวันนี้</h1></div></div>"));

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowprogramheader'><div class='col-xs-12 col-sm-12 col-lg-12'><h4>" + dr["class_name"].ToString() + "</h4></div></div>"));
                        DataSet dsDetail = new ServiceScore().FootballLiveScoreSelectbyMatch(strConn, dr["scs_id"].ToString(), dr["match_date"].ToString(), "L");
                        int index = 1;
                        string cssName = "rowprogramdetail_";
                        foreach (DataRow drDetail in dsDetail.Tables[0].Rows)
                        {
                            //ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genText("", drDetail["scs_desc"].ToString(), pageRedirect, "0", projectType, false, ""));
                            cssName = (index % 2) == 0 ? "rowprogramdetail_" : "rowprogramdetail";
                            ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='"+ cssName + "'><div class=col-xs-1 col-sm-1 col-lg-1 col-md-1></div><a class='linkprogram' href='http://wap.isport.co.th/isportui/football_result_detail.aspx?" + HttpContext.Current.Request.QueryString + "&msch_id=" + drDetail["msch_id"] + "'><div class=col-xs-11 col-sm-11 col-lg-11 col-md-11>" + drDetail["scs_desc"].ToString() + "</div></a></div>"));
                            index++;
                        }
                    }
                }
                else
                {
                    ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genTextHeader("", "ขออภัยไม่พบข้อมูลการแข่งขันค่ะ", pageRedirect, "0", projectType, false, ""));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("genScore >> " + ex.Message);
            }
        }
        #endregion

        #region News
        private void genNews(DataRow dr,string strOrclConn,Control ctr,string classId)
        {
            try
            {
                bool isNews = dr["ui_isnews"].ToString() == "" ? false : (bool)dr["ui_isnews"];
                if (isNews)
                {
                    // Gen News
                    string url = "";
                    classId = (classId == null || classId == "" ? "1" : classId);
                    string[] str = classId.Split(',');
                    classId = str.Length > 0 ? str[0] : classId;
                    DataTable dt = new ServiceNews().News_SelectOracle(strOrclConn, dr["ui_isnews_top"].ToString(), classId);
                    foreach (DataRow drContent in dt.Rows)
                    {

                        url = "detail.aspx?" + HttpContext.Current.Request.QueryString + "&pcnt_id=" + drContent["news_id"].ToString();//"&sg=" + dr["ui_sg_id"].ToString()
                        // + "&class_id=" + classId;//+ "&scs_id=" + bProperty_SCSID + "&mp_code=" + bProperty_MPCODE + "&prj=" + bProperty_PRJ + "&p=" + Request["p"];

                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl(
                            "<div class='col-xs-12 col-md-4 col-sm-6'><div class='thumbnail'><a  href='" + url + "'>"
                            + "<img class='img-full' src='http://sms-gw.samartbug.com/isportimage/images/500x300/" + drContent["news_images_600"].ToString() + "'>"
                            + "<div class='caption'>" + drContent["news_header_th"].ToString() + "</a></div>"
                            + " <div class='media-button'><small-gray><i class='fa fa - clock - o fa - 1' aria-hidden='true'></i>" + DateTime.ParseExact(drContent["news_createdate"].ToString(), "M/d/yyyy h:mm:ss tt", null).ToString("d/MMM/yy H:s") + "</small-gray></div></div></div>"));



                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
