using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;
using System.Data.OracleClient;
using OracleDataAccress;

namespace WS_BB
{
    public class AppCode_Utility : AppCode_Base
    {
        public static string GetOperatorByIMSI(string imsiCode )
        {
            try
            {
                string rtn = "";
                if (ConfigurationManager.AppSettings["Application_IMSI_AIS"].IndexOf(imsiCode) > -1)
                {
                    rtn = "01";
                }
                else if (ConfigurationManager.AppSettings["Application_IMSI_DTAC"].IndexOf(imsiCode) > -1)
                {
                    rtn = "02";
                }
                else if (ConfigurationManager.AppSettings["Application_IMSI_TRUE"].IndexOf(imsiCode) > -1)
                {
                    rtn = "03";
                }
                else if (ConfigurationManager.AppSettings["Application_IMSI_TRUEH"].IndexOf(imsiCode) > -1)
                {
                    rtn = "04";
                }
                else if (ConfigurationManager.AppSettings["Application_IMSI_3GX"].IndexOf(imsiCode) > -1)
                {
                    rtn = "05";
                }
                return rtn;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static bool CheckAisCustomer(string ip)
        {
            bool rtn = false;
            try
            {
                if (ConfigurationManager.AppSettings["ApplicationListAisIP"].ToString().IndexOf(ip) > -1
                    || ip.IndexOf("203.170.230") > -1
                    || ip.IndexOf("203.170.22") > -1
                    || ip.IndexOf("203.170.229") > -1
                    || ip.IndexOf("202.149.25") > -1
                    || ip.IndexOf("203.146.63") > -1
                    || ip.IndexOf("119.31.0") > -1
                    || ip.IndexOf("119.31.32") > -1
                    || ip.IndexOf("119.31.64") > -1
                    || ip.IndexOf("119.31.96") > -1
                    || ip.IndexOf("119.31.112") > -1
                    || ip.IndexOf("119.31.5") > -1
                    || ip.IndexOf("119.31") > -1
                    || ip.IndexOf("110.49") > -1 )
                    {
                        rtn = true;
                    }
              
            }
            catch{}
            return rtn;
        }

        #region Setting ( iPhone )

        public DataSet CommandGetSetting()
        {
            try
            {
                DataSet dsSetting = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetSettingByType", "ISPORTAPP_SETTING",
                                        new OracleParameter[] { OrclHelper.GetOracleParameter("p_model_type", "iphone", OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                                });
                return dsSetting;
            }
            catch (Exception ex)
            {
                throw new Exception("CommandGetSetting >> " + ex.Message);
            }
        }
        public XDocument CommandSaveSetting(string setting,string tokenid, string code, string type)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp"));
            try
            {

                    OrclHelper.ExecuteNonQuery(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_Insert_Setting"
                        , new OracleParameter[] { 
                                OrclHelper.GetOracleParameter("p_model_code", code, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_token_id", tokenid, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_setting", setting, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_model_type", type, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_create_date", DateTime.Now, OracleType.DateTime , ParameterDirection.Input)
                        }).ToString();
                rtnXML.Element("SportApp").Add(new XElement("status", "sucess"), new XElement("message", ""));
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("CommandSaveSetting >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error"), new XElement("message", ex.Message));
            }
            return rtnXML;
        }
        #endregion

        #region Notification

        private DataView FillterNotification(DataView dv,string countryId)
        {
            try
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
                if (countryId == "")
                {
                    //str.Append(" contestgroupid in (21,135,116,85,19)");
                }
                else
                {
                    str.Append(" COUNTRY_ID in (" + countryId + ")");
                }
                dv.RowFilter = str.ToString();
                return dv;
            }
            catch(Exception ex)
            {
                throw new Exception("FillterNotification >> " + ex.Message);
            }
        }
        private DataView FillterNotificationByContestGroupId(DataView dv, string contestGroupId)
        {
            try
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
                if (contestGroupId == "")
                {
                    str.Append(" contestgroupid in (21,135,116,85,19)");
                }
                else
                {
                    str.Append(" contestgroupid in (" + contestGroupId + ")");
                }
                dv.RowFilter = str.ToString();
                return dv;
            }
            catch (Exception ex)
            {
                throw new Exception("FillterNotification >> " + ex.Message);
            }
        }
        public DataSet GetNotification(int minute)
        {
            try
            {
                DateTime startDate = DateTime.Now.AddMinutes(-minute);
                DateTime enddate = DateTime.Now;
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getnotification"
                   , new SqlParameter[] { 
                       new SqlParameter("@startDate", startDate) 
                       ,new SqlParameter("@endDate", enddate)
                   });
                return ds; 
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataSet GetNotificationByTeam(int minute , string teamCode)
        {
            try
            {
                DateTime startDate = DateTime.Now.AddMinutes(-minute);
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getnotification_footballthai"
                   , new SqlParameter[] { 
                       new SqlParameter("@teamcode", teamCode) 
                       ,new SqlParameter("@dateStart", startDate)
                   });
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public XDocument CommandGetNotification_BallThai(XDocument rtnXML ,string teamCode, int minute, string lang)
        {
            try
            {
                DataSet ds = GetNotificationByTeam(minute,teamCode);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    rtnXML.Element("SportApp").Add(new XElement("notification", ds.Tables[0].Rows[0]["scs_desc"])
                        , new XElement("notification_url", "http://wap.isport.co.th/sport_center/isport/football_livescore.aspx?lng=L&mp=0000&size=N&sg=1&prj=47"));
                    
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("notification", "")
                        , new XElement("notification_url", ""));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetNotification_BallThai >> " + ex.Message);
                throw new Exception(ex.Message);
                //rtnXML.Element("SportApp").Add(new XElement("status", "error"), new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XDocument CommandGetNotification_SportPool(XDocument rtnXML, string teamCode, int minute)
        {
            try
            {

                rtnXML.Element("SportApp").Add(new XAttribute("title", "")
                                                                , new XAttribute("detail", "")
                                                                , new XAttribute("footer", "")
                                                                , new XAttribute("urlimgbig", ConfigurationManager.AppSettings["Application_SportPool_Image_Noti"])
                                                                , new XAttribute("urlimgsmall", "")
                                                                , new XAttribute("actioncall", "")
                                                                , new XAttribute("actionurl", "")
                                                                , new XAttribute("actionapp", "true"));

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetNotification_SportPool >> " + ex.Message);
                throw new Exception(ex.Message);
                //rtnXML.Element("SportApp").Add(new XElement("status", "error"), new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XDocument CommandGetNotificationByContestGroupId(string contestGroupId, int minute, string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp"));
            try
            {
                DataSet ds = GetNotification(minute);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = FillterNotificationByContestGroupId(ds.Tables[0].DefaultView, contestGroupId);
                    if (dv.Count > 0)
                    {
                        string contestName = lang == AppCode_Base.Country.th.ToString()
                            ? dv[0]["CLASS_NAME_LOCAL"].ToString() == "" ? dv[0]["CONTESTGROUP_NAME"].ToString() : dv[0]["CLASS_NAME_LOCAL"].ToString()
                            : dv[0]["CONTESTGROUP_NAME"].ToString();
                        contestName = lang == AppCode_Base.Country.th.ToString() ? "ประตู! " + contestName : "Gold! " + contestName;
                        rtnXML.Element("SportApp").Add(new XElement("notification", contestName));
                    }
                    else
                    {
                        rtnXML.Element("SportApp").Add(new XElement("notification", "none"));
                    }
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("notification", "none"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetNotification >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error"), new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XDocument CommandGetNotification(string countryId, int minute,string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp"));
            try
            {

                DataSet ds = GetNotification(minute);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = FillterNotification(ds.Tables[0].DefaultView, countryId);
                    if (dv.Count > 0)
                    {
                        string contestName = lang == AppCode_Base.Country.th.ToString()
                            ? dv[0]["CLASS_NAME_LOCAL"].ToString() == "" ? dv[0]["CONTESTGROUP_NAME"].ToString() : dv[0]["CLASS_NAME_LOCAL"].ToString()
                            : dv[0]["CONTESTGROUP_NAME"].ToString();
                        contestName = lang == AppCode_Base.Country.th.ToString() ? "ประตู! " + contestName : "Gold! " + contestName;
                        rtnXML.Element("SportApp").Add(new XElement("notification", contestName));
                    }
                    else
                    {
                        rtnXML.Element("SportApp").Add(new XElement("notification", "none"));
                    }
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("notification", "none"));
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("CommandGetNotification >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error"), new XElement("message", ex.Message));
            }
            return rtnXML;
        }

#endregion

        #region Event & About

        public XDocument CommangGetAbout(string projectCode,string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", "")
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {

                rtnXML.Element("SportApp").Add(new XElement("About"
                    ,new XAttribute("image1","xxxxxxxxxxxxxxxxxxxxxxx")
                    , new XAttribute("image2", "xxxxxxxxxxxxxxxxxxxxx")
                    , new XAttribute("image3", "xxxxxxxxxxxxxxxxxxxxxx")
                    ,new XAttribute("message","xxxxxxxxxxxxxxxxxxxxxxx")
                    )
                , new XElement("status", "success")
                , new XElement("message", ""));


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommangGetAbout >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        

        public XDocument CommangGetEvent(XDocument rtnXML, string projectCode, string lang, string type)
        {

            try
            {
                    rtnXML.Element("SportApp").Add(
                        GetImageElement("Privilege", "http://wap.isport.co.th/isportws/update.png"
                        , "http://wap.isport.co.th/isportws/update.png"
                        , ""
                        , "ขออภัยค่ะ ปิดระบบเพื่อปรังปรุงเวลา 21:00 - 05:00 ค่ะ"
                        , ""
                        , "http://wap.isport.co.th/update.aspx")
                        //, GetImageElement("Privilege", "http://wap.isport.co.th/isportws/images/privilege/privilege4.jpg"
                        //, "http://wap.isport.co.th/isportws/images/privilege/privilege4_320.jpg"
                        //, ""
                        //, "Sport Arena  SMS"
                        //, "025020442"
                        //, "http://www.isport.co.th/sportbuffet/")
                        //, GetImageElement("Privilege", "http://wap.isport.co.th/isportws/images/privilege/privilege2.jpg"
                        //, "http://wap.isport.co.th/isportws/images/privilege/privilege2_320.jpg"
                        //, ""
                        //, "sms สตาร์ซอคเก้อร์"
                        //, "025020466"
                        //, "http://www.isport.co.th/tded/")
                     , new XElement("status", "success")
                    , new XElement("message", ""));

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommangGetAbout >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

#endregion

        #region Banner

        public XDocument CommangGetPrivilege_IsportSportPool(XDocument rtnXML, string projectCode, string lang, string type, string optCode)
        {

            try
            {

                optCode = (optCode == "04") ? "03" : optCode ;
                DataSet ds = new AppCode_Banner().GetBannerByAppName(projectCode, optCode, "detail");

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = ds.Tables[0].Rows[i];
                        rtnXML.Element("SportApp").Add(
                            GetImageElementSportPool("Privilege", dr["big_img"].ToString() // big
                            , dr["medium_img"].ToString() // medium
                            , dr["small_img"].ToString() // small
                            , dr["title"].ToString()
                            , dr["footer"].ToString()
                            , dr["detail"].ToString()
                            , dr["phone_no"].ToString()
                            , dr["link"].ToString()));
                        
                    }
                }
                else
                {
                    rtnXML.Element("SportApp").Add(
                            GetImageElementSportPool("Privilege", "http://wap.isport.co.th/isportws/images/privilege/privilege4.png"
                            , "http://wap.isport.co.th/isportws/images/privilege/privilege4_320.png"
                            , ""
                            ,""
                            ,""
                            , "Sport News"
                            , "*451107703"
                            , "http://wap.isport.co.th"));
                }

                rtnXML.Element("SportApp").Add( new XElement("status", "success")
                        , new XElement("message", ""));
                   
  
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommangGetPrivilege_IsportSportPool >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XDocument CommangGetPrivilege_IsportStarsoccer(XDocument rtnXML, string projectCode, string lang, string type,string optCode)
        {

            try
            {
                optCode = (optCode == "04") ? "03" : optCode ;
                DataSet ds = new AppCode_Banner().GetBannerByAppName(AppCode_Base.AppName.StarSoccer.ToString(), optCode, "detail");

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = ds.Tables[0].Rows[i];
                        rtnXML.Element("SportApp").Add(
                            GetImageElement("Privilege", dr["medium_img"].ToString()
                            , dr["small_img"].ToString()
                            , dr["big_img"].ToString()
                            , dr["title"].ToString()
                            , dr["phone_no"].ToString()
                            , dr["link"].ToString()));
                        
                    }
                }
                else
                {
                    rtnXML.Element("SportApp").Add(
                            GetImageElement("Privilege", "http://wap.isport.co.th/isportws/images/privilege/privilege4.png"
                            , "http://wap.isport.co.th/isportws/images/privilege/privilege4_320.png"
                            , ""
                            , "Sport News"
                            , "*451107703"
                            , "http://wap.isport.co.th"));
                }

                rtnXML.Element("SportApp").Add( new XElement("status", "success")
                        , new XElement("message", ""));

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommangGetAbout >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XDocument CommangGetPrivilege(XDocument rtnXML, string projectCode, string lang, string type)
        {

            try
            {

                if (type == "ipad")
                {
                    // image1 = แนวตั้ง
                    // image2 = แนวนอน
                    rtnXML.Element("SportApp").Add(
                        GetImageElement("Privilege", "http://wap.isport.co.th/isportws/images/privilege/ipad/privilage1.png"
                        , "http://wap.isport.co.th/isportws/images/privilege/ipad/privilage1_ls.png"
                        , ""
                        , "Ais Book Store"
                        , "025020442"
                        , "http://www.ais.co.th/bookstore")
                        , GetImageElement("Privilege", "http://wap.isport.co.th/isportws/images/privilege/ipad/privilage2.png"
                        , "http://wap.isport.co.th/isportws/images/privilege/ipad/privilage2_ls.png"
                        , ""
                        , "Sport Arena  SMS"
                        , "025020442"
                        , "http://www.isport.co.th/tded/")
                        , GetImageElement("Privilege", "http://wap.isport.co.th/isportws/images/privilege/ipad/privilage3.png"
                        , "http://wap.isport.co.th/isportws/images/privilege/ipad/privilage3_ls.png"
                        , ""
                        , "Sport Arena  SMS"
                        , "025020442"
                        , "http://www.isport.co.th/tded/")
                     , new XElement("status", "success")
                    , new XElement("message", ""));
                }
                else
                {
                    DataSet ds = new AppCode_Banner().GetBannerByAppName(AppCode_Base.AppName.StarSoccer.ToString(), "01", "detail");

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataRow dr = ds.Tables[0].Rows[i];
                            rtnXML.Element("SportApp").Add(
                                GetImageElement("Privilege", dr["medium_img"].ToString()
                                , dr["small_img"].ToString()
                                , dr["big_img"].ToString()
                                , dr["title"].ToString()
                                , dr["phone_no"].ToString()
                                , dr["link"].ToString()));

                        }
                    }
                    else
                    {
                        rtnXML.Element("SportApp").Add(
                                GetImageElement("Privilege", "http://wap.isport.co.th/isportws/images/privilege/privilege4.png"
                                , "http://wap.isport.co.th/isportws/images/privilege/privilege4_320.png"
                                , ""
                                , "Sport News"
                                , "*451107703"
                                , "http://wap.isport.co.th"));
                    }

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                            , new XElement("message", ""));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommangGetAbout >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }
        public XDocument CommangGetBannerNews(string projectCode, string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", "BannerNews ")
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {

                rtnXML.Element("SportApp").Add(
                    GetImageElement("BannerNews", "http://wap.isport.co.th/isportws/images/privilege/ipad/PrivilegeFull_news.png"
                    , "http://wap.isport.co.th/isportws/images/privilege/ipad/PrivilegeFullLand_news.png"
                    , ""
                    , "Sport Arena  SMS"
                    , "025020442"
                    , "http://www.isport.co.th/sportarena/")
                 , new XElement("status", "success")
                , new XElement("message", ""));
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommangGetAbout >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }
        public XDocument CommangGetBanner(string projectCode, string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", "Banner ")
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                //http://wap.isport.co.th/isportws/banner/isportstarsoccer_banner.html
                rtnXML.Element("SportApp").Add(
                    GetImageElement("Banner", "http://wap.isport.co.th/isportws/images/privilege/ipad/banner.gif"
                    , ""
                    , ""
                    , "Sport Arena  FHM"
                    , "025020442"
                    , "http://wap.isport.co.th/isportws/banner/isportstarsoccer_banner.html")
                 , new XElement("status", "success")
                , new XElement("message", ""));
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommangGetAbout >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XElement GetImageElement(string tagName,string img1,string img2,string img3,string msg,string phone,string url)
        {
            return new XElement(tagName
                    , new XAttribute("image1", img1)
                    , new XAttribute("image2", img2)
                    , new XAttribute("image3", img3)
                    , new XAttribute("message", msg)
                    , new XAttribute("phone", phone)
                    , new XAttribute("url", url));
        }

        public static XElement GetImageElementSportPool(string tagName, string img1, string img2, string img3,string header,string title, string msg, string phone, string url)
        {
            return new XElement(tagName
                    , new XAttribute("image1", img1)
                    , new XAttribute("image2", img2)
                    , new XAttribute("image3", img3)
                    , new XAttribute("header", header)
                    , new XAttribute("title", title)
                    , new XAttribute("message", msg)
                    , new XAttribute("phone", phone)
                    , new XAttribute("url", url));
        }

        /// <summary>
        /// สำหรับ app content cafe
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="img1"></param>
        /// <param name="img2"></param>
        /// <param name="img3"></param>
        /// <param name="header"></param>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="phone"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static XElement GetImageElementSportPhone(string tagName, string img1, string img2, string img3, string header, string title, string msg, string phone, string url,string bannerType)
        {
            return new XElement(tagName
                    , new XAttribute("image1", img1)
                    , new XAttribute("image2", img2)
                    , new XAttribute("image3", img3)
                    , new XAttribute("header", header)
                    , new XAttribute("title", title)
                    , new XAttribute("message", msg)
                    , new XAttribute("phone", phone)
                    , new XAttribute("url", url)
                    , new XAttribute("bannertype", bannerType));
        }


        public XDocument CommandGetHeaderandFooter(XDocument rtnXML, string lang, string type)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["ApplicationPathImage"].ToString();
                rtnXML.Element("SportApp").Add(
                                   new XElement("Tag", "Tab")
                                    , new XElement("Footer"
                                        , new XElement("tabfootball",new XAttribute("sportType","00001"), new XAttribute("image",  url + "tabfootball.jpg"))
                                         , new XElement("tabtennis", new XAttribute("sportType","00003"),new XAttribute("image",  url + "tabtennis.jpg"))
                                         , new XElement("tabgoft", new XAttribute("sportType","00002"),new XAttribute("image",  url + "tabgoft.jpg"))
                                         , new XElement("tabservice", new XAttribute("sportType",""),new XAttribute("image",  url + "tabservice.jpg"))
                                            ));
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommangGetAbout >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XDocument CommandGetTab(XDocument rtnXML, string sportType,string lang, string type)
        {
            try
            {
                string url = ConfigurationManager.AppSettings["ApplicationPathImage"].ToString();
                string strTable = sportType == "00001" ? "ตารางคะแนน" : "อันดับโลก";
                rtnXML.Element("SportApp").Add(
                                   new XElement("Tag", "Tab")
                                    , new XElement("Tab"
                                        , new XElement("news", new XAttribute("image", url + "btsub_news.jpg"), new XAttribute("text", "ข่าว"))
                                         , new XElement("result", new XAttribute("image", url + "btsub_result.jpg"), new XAttribute("text", "ผลการแข่งขัน"))
                                         , new XElement("table", new XAttribute("image", url + "btsub_table.jpg"), new XAttribute("text", strTable))
                                         , new XElement("program", new XAttribute("image", url + "btsub_program.jpg"), new XAttribute("text", "โปรแกรมการแข่งขัน"))
                                            ));
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommangGetAbout >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }


        #endregion

    }
}
