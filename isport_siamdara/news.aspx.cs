using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using MobileLibrary;
using WebLibrary;

namespace isport_siamdara
{
    public partial class news : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (Request["m"] != null) Response.AppendCookie(new HttpCookie("User-Identity-Forward-msisdn", Request["m"]));
                    MobileUtilities mU = Utilities.getMISDN(Request);
                    string vStrName = Request.ServerVariables["HTTP_USER_AGENT"];
                    GenMenuHeader(vStrName, mU.mobileOPT);
                    //if (mU.mobileNumber != "")
                    //{
                    GenContent(mU.mobileOPT, mU.mobileNumber, vStrName);
                    //}
                    //else if (Request["o"] == null && Request["m"] == null) Response.Redirect(ConfigurationManager.AppSettings["URLGetMSISDN"] + Request.Url.Query, false);
                    //else GenMessage("ขออภัยค่ะ กรุณาใช้ Internet จากเครื่องโทรศัพท์" + mU.mobileNumber, mU.mobileOPT);
                    GenFooter(mU.mobileOPT);

                    try
                    {
                        isport_service.ServiceWap_Logs.Subscribe_portal_log(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString(), mU.mobileNumber, vStrName
                            , ConfigurationManager.AppSettings["SiamDaraNewsSG"], mU.mobileOPT, "58", "0025", "");
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.WriteError(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                GenMessage("ขออภัยค่ะ ระบบเกิดข้อผิดพลาด กรุณาเข้าใช้งานใหม่ " + ex.Message, "");
                ExceptionManager.WriteError(ex.Message);
            }
        }

        #region Header
        private void GenMenuHeader(string vStrName, string opt)
        {
            try
            {
                if (vStrName.ToLower().IndexOf("symbian") > 0)
                {
                    divMenuHeader.Attributes.Add("style", "display:none");
                    divMenuHeader_low.Attributes.Add("style", "display:block");
                    lblHeaderLow.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString(), opt, "", "", "news.aspx", "0"));
                }
                else
                {
                    divMenuHeader.Attributes.Add("style", "display:block");
                    divMenuHeader_low.Attributes.Add("style", "display:none");
                    frmMain.Controls.Add(new isport_service.ServiceWapUI_Header().GenHeader(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString(), opt, "", "", "news.aspx", "0"));
                }
                //else 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Content
        private void GenContent(string opt,string msisdn,string vStrName)
        {
            try
            {
                //DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["IsportConnectionString"].ToString(), CommandType.StoredProcedure, "usp_getsip_content_bypcntid"
                //        , new SqlParameter[] { new SqlParameter("@pcnt_id", Request["n"]) });
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString(), CommandType.StoredProcedure, "usp_wapisport_uiselectbysgid"
                    ,new SqlParameter[]{new SqlParameter("@sg_id", "258")});

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string chk = "Y";//AppCode.CheckSmsSubScribe(opt, msisdn,"");
                    if ( chk == "Y")
                    {
                        if (Request["n"] != null && Request["n"].ToString()!= "")
                        {
                            frmMain.Controls.AddAt(frmMain.Controls.Count,new isport_service.ServiceWapUI_Content().GenContent(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString(), "", "0", "0", opt, Request["n"], "news.aspx", "", ""));
                        }
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string rowCSS = dr["content_bgcolor"] == null || dr["content_bgcolor"].ToString() == "" ? "row" : "row_" + dr["content_bgcolor"].ToString();
                            string classImg = "img-small";
                            //if ((bool)dr["ui_ispayment"]) // is Header
                            //{

                            
                            //{
                                //frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("", dr["content_text"].ToString(), "news.aspx", "0", "siamdara", false, ""));
                                //frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genImagesHeader("news.aspx", "" + ConfigurationManager.AppSettings["URLImagesPath"]  + dr["content_image"].ToString().Replace("~/", ""), "img-news", "0", "siamdara", false, ""));
                            //}
                            //else
                            //{
                                frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><a href='news.aspx?n=" + dr["ui_projecttype"] + "'><div class=col-xs-3 col-sm-3 col-lg-3 col-md-5><img class='" + classImg + "' src='" + ConfigurationManager.AppSettings["URLImagesPath"] + dr["content_image"].ToString().Replace("~/", "") + "'></div><div class=col-xs-9 col-sm-9 col-lg-9 col-md-7>" + dr["content_text"] + "</div></a></div>"));
                           //}
                            //frmMain.Controls.AddAt(frmMain.Controls.Count,isport_service.ServiceWapUI_GenControls.genImagesHeader("news.aspx",ConfigurationManager.AppSettings["SiamDataImgPath"].ToString() + ds.Tables[0].Rows[0]["pic_path_70"].ToString(),"img-news","0","siamdara",false,""));
                            //frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genImagesHeader("news.aspx", "http://sms-gw.samartbug.com/isportimage/images/325x200/130809D4U94062.jpg", "img-news", "0", "siamdara", false, ""));
                            //frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genTextHeader("",ds.Tables[0].Rows[0]["pcnt_title_local"].ToString(),"news.aspx", "0", "siamdara", false, ""));
                            //frmMain.Controls.AddAt(frmMain.Controls.Count, isport_service.ServiceWapUI_GenControls.genText("", ds.Tables[0].Rows[0]["pcnt_detail_local"].ToString(), "news.aspx", "0", "siamdara", false, ""));
                        }
                    }
                    else if (chk == "N")
                    {
                        // CheckActive = N  or msisdn = ""
                        GenMessage("ขออภัยค่ะ กรุณาสมัครบริการก่อนเข้าใช้งานค่ะ" + msisdn, opt);
                    }
 
                }
                else
                {
                    GenMessage("ขออภัยค่ะ ไม่พบข้อมูลที่ต้องการกรุณาเข้าใช้งานใหม่" + msisdn, opt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GenDaraClip >>" + ex.Message);
            }
        }
        #endregion

        #region Footer
        private void GenFooter(string opt)
        {
            try
            {
                 frmMain.Controls.Add(new isport_service.ServiceWapUI_Footer().GenFooter(ConfigurationManager.ConnectionStrings["IsportdbConnectionString"].ToString(), opt, "siamdara", "", "news.aspx", "0","00025","58","",""));
            }
            catch (Exception ex)
            {
                throw new Exception("GenFooter >> " + ex.Message);
            }
        }
        #endregion

        #region Gen Message
        private void GenMessage(string message, string opt)
        {
            try
            {
                if (Request.Browser.ScreenPixelsWidth > int.Parse(ConfigurationManager.AppSettings["ModeScreenSize"]))
                {
                    frmMain.Controls.Add(new LiteralControl("<div class='row'><div class='col-xs-12 col-sm-12 col-lg-12'><p>" + message + "</p></div></div>"));
                }
                else
                {

                    //var div = Page.FindControl("id1").vis;
                    //div.Visibility = true;
                    frmMain.Controls.Add(new LiteralControl(message));
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception("GenMessage>> " + ex.Message);
            }
        }
        #endregion
    }
}
