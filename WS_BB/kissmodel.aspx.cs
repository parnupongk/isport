using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using OracleDataAccress;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Configuration;
using isport_service;
namespace WS_BB
{
    public partial class kissmodel : feedMain
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public override string CheckPageName(string pageName, string appName)
        {
            string rtn = "";
            if (pageName == "main")
            {
                GetActive();
                rtn = GetContentMain(Request["date"]);
            }
            else if (pageName == "service")
            {
                rtn = GenSMSService("th", "", "");
            }
            else if (pageName == "varity")
            {
                rtn = GetContentVarity();
            }

            rtnXML = AppCode_PageStatus.GenStatus(rtnXML, strStatus, strErr);

            return rtn;

        }

        #region Get Active
        private void GetActive()
        {
            try
            {
                string checkActive = CheckActiveandPromotion(AppCode_Base.AppName.KissModel, Request["imsi"], Request["imei"], "0", true, "504");
                rtnXML.Element("SportApp").Add(new XElement("Active",
                                                   new XAttribute("isactive", checkActive)
                                                , new XAttribute("otp_status", "new")
                                                , new XAttribute("header", ConfigurationManager.AppSettings["Application_KissModel_Header"])
                                                , new XAttribute("detail", ConfigurationManager.AppSettings["Application_KissModel_Detail"])
                                                , new XAttribute("footer", ConfigurationManager.AppSettings["Application_KissModel_Footer"])
                                                , new XAttribute("footer1", "")
                                                , new XAttribute("phone", ConfigurationManager.AppSettings["Application_IsportKissModel_PSNNumber"])
                                                , new XAttribute("url", ConfigurationManager.AppSettings["Application_IsportKissModel_URLSubscribe"])
                                                ));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Varity
        public string GetContentVarity()
        {
            try
            {
                DataTable dt = isport_service.ServiceVarity.GetSipContentBypCatId(AppCode_Base.strConnPack, "176", "4");
                if (dt.Rows.Count > 0)
                {
                    XElement element;

                    foreach(DataRow dr in dt.Rows)
                    {
                        element = new XElement("Content");
                        element.Add(new XAttribute("title", dr["name"].ToString())
                                            ,new XAttribute("id",dr["pcnt_id"].ToString())
                                            , new XAttribute("url", "http://wap.isport.co.th/isportui/upload/201506051123940.JPG")
                                            );

                        DataTable dtDetail = isport_service.ServiceVarity.GetSipContentDetailByPcntId(AppCode_Base.strConnOracle, dr["pcnt_id"].ToString());
                        if (dtDetail != null && dtDetail.Rows.Count > 0)
                        {
                            foreach(DataRow drDetail in dtDetail.Rows)
                            {
                                element.Add(new XElement("detail", new XAttribute("detail_local", drDetail["detail_local"].ToString() )
                                                                                    , new XAttribute("id", drDetail["pcnt_id"].ToString() ))
                                                                                      );
                            }
                        }

                        rtnXML.Element("SportApp").Add(element);
                    }


                }

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get SMS Service
        private string GenSMSService(string lang, string type, string sportType)
        {
            try
            {

                string optCode = muMobile.mobileOPT;// AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"]));

                rtnXML.Element("SportApp").Add(new XAttribute("wording1", "เช็คทีเด็ดก่อนเชียร์บอล ไม่ผิดหวัง!!"));
                rtnXML.Element("SportApp").Add(new XAttribute("wording2", ""));
                rtnXML.Element("SportApp").Add(new XAttribute("banner", "http://wap.isport.co.th/isportws/banner/isportsportpool_banner.html"));
                rtnXML.Element("SportApp").Add(new XAttribute("adview", (DateTime.Now.Hour > 0 && DateTime.Now.Hour < 6) ? "true" : ConfigurationManager.AppSettings["Application_IsportStarSoccer_adView"].ToString()));

                rtnXML = new AppCode_Utility().CommangGetPrivilege_IsportSportPool(rtnXML, AppCode_Base.AppName.KissModel.ToString(), lang, type, optCode);

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GenSMSService >>" + ex.Message);
            }
        }
        #endregion

        #region Gen Content

        private string GetContentMain(string date)
        {
            try
            {

                string strDate = DateTime.ParseExact(date, "MMddyyyy", null).AddDays(7).ToString("MMddyyyy");
                DataSet ds = OrclHelper.Fill(AppCode_Base.strConnOracle, CommandType.StoredProcedure, "ISPORT_TEST.KissApp_Select_ContentByDate", "Kiss_Content"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("P_DISPLAY_TXT",strDate,OracleType.VarChar,ParameterDirection.Input)
                                                          , OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                                                        });

                GenSMSService("th", "", "");

                if (ds.Tables.Count > 0)
                {

                    DataSet dsMedia;
                    XElement element;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            element = new XElement("Content");
                            element.Add(new XAttribute("title", dr["title"].ToString())
                                                                        , new XAttribute("detail", dr["title_detail"].ToString())
                                                                        , new XAttribute("detail1", dr["title_detail1"].ToString())
                                                                        , new XAttribute("detail2", dr["title_detail2"].ToString())
                                                                        , new XAttribute("footer", dr["footer"].ToString())
                                                                        , new XAttribute("isFree", dr["free"].ToString())
                                                                        , new XAttribute("fName", dr["fname"].ToString())
                                                                        , new XAttribute("lName", dr["lname"].ToString())
                                                                        , new XAttribute("nName", "น้อง " + dr["nname"].ToString())
                                                                        , new XAttribute("interview", dr["interview"].ToString())
                                                                        , new XAttribute("type", dr["type"].ToString())
                                                                        , new XAttribute("display_date", dr["display_date"].ToString())
                                                                        , new XAttribute("id", dr["KC_ID"].ToString())
                                                                        , new XAttribute("shape", "สัดส่วน : " + dr["shape"].ToString())
                                                                        , new XAttribute("weight", "น้ำหนัก : " + dr["km_w"].ToString())
                                                                        , new XAttribute("high", "ส่วนสูง : " + dr["km_h"].ToString())
                                                                        , new XAttribute("phone", dr["type"].ToString() == "jza" ? ConfigurationManager.AppSettings["Application_IsportJZA_PSNNumber"] : ConfigurationManager.AppSettings["Application_IsportKissModel_PSNNumber"])
                                                                        , new XAttribute("url", dr["type"].ToString() == "jza" ? ConfigurationManager.AppSettings["Application_IsportJza_URLSubscribe"] : ConfigurationManager.AppSettings["Application_IsportKissModel_URLSubscribe"])
                                                                        );

                            dsMedia = OrclHelper.Fill(AppCode_Base.strConnOracle, CommandType.StoredProcedure, "ISPORT_TEST.KissApp_Select_MediaByKCID", "KISS_MEDIA"
                                                                , new OracleParameter[] {OrclHelper.GetOracleParameter("P_KC_ID",dr["kc_id"].ToString(),OracleType.VarChar,ParameterDirection.Input)
                                                                                                   , OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                                                                                                     });
                            if (dsMedia.Tables.Count > 0 && dsMedia.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow drMedia in dsMedia.Tables[0].Rows)
                                {

                                    element.Add(new XElement("Media", new XAttribute("pic", drMedia["pic"].ToString() == "" ? "" : ConfigurationManager.AppSettings["ApplicationIsportURLImages"] + drMedia["pic"])
                                                                                        , new XAttribute("clip", drMedia["clip"].ToString() == "" ? "" : String.Format(ConfigurationManager.AppSettings["ApplicationIsportURLClip_default"], drMedia["clip"]))
                                                                                                                ));
                                }
                            }

                            rtnXML.Element("SportApp").Add(element);

                        }

                        catch { }
                    }
                }

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

    }
}