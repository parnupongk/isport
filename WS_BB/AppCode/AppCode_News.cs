using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Xml.Linq;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;
using OracleDataAccress;

namespace WS_BB
{
    public class AppCode_News : AppCode_Base
    {

        #region Database isport
        public struct Out_News
        {
            public string status;
            public string errorMess;
            public List<news> sportNews;
        }
        public class news
        {
            public string pcntId;
            public string pcntTitle;
            public string pcntTitleLocal;
            public string pcntDisplayTime;
            public string pcntDetail;
            public string pcntDetailLocal;
            public string picPath70;
            public string picPath36;
            
        }
        public Out_News CommandGetNews(string pcatId)
        {
            Out_News outNews = new Out_News();
            List<news> sportNews = new List<news>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConnPack, CommandType.StoredProcedure, "usp_BB_sport_news",
                    new SqlParameter[] {new SqlParameter("@scs_id",pcatId) });
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    news n = new news();
                    n.pcntId = dr["pcnt_id"].ToString();
                    n.pcntTitle = dr["pcnt_title"].ToString();
                    n.pcntTitleLocal = dr["pcnt_title_local"].ToString();
                    n.pcntDisplayTime = dr["display_time"].ToString();
                    n.pcntDetail = dr["pcnt_detail"].ToString();
                    n.pcntDetailLocal = dr["pcnt_detail_local"].ToString();
                    n.picPath70 = dr["pic_path_70"].ToString();
                    n.picPath36 = dr["pic_path_36"].ToString();
                    sportNews.Add(n);
                }
            }
            catch (Exception ex)
            {
                outNews.status = "Error";
                outNews.errorMess = ex.Message;
                WebLibrary.ExceptionManager.WriteError(ex.Message);
            }
            outNews.sportNews = sportNews;
            return outNews;
        }
        public XElement Command_GetSportContent_Ranking(string elementName, string sportType, string lang)
        {
            XElement rtn = new XElement(elementName);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_sportcontent_ranking"
                    , new SqlParameter[] {new SqlParameter("@sport_type",sportType) });
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    rtn.Add(new XAttribute("Detail", dr["scnt_detail"].ToString()));
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("Command_GetSportContent_Ranking >> " + ex.Message);
            }
            return rtn;
        }
        public XElement Command_GetSportContent(string elementName,string catId, string sportType, string lang)
        {
            XElement rtn = new XElement(elementName);
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(strConn))
                {
                    if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                    DataSet ds = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "usp_wapisport_sportclassbysporttype",
                        new SqlParameter[] { new SqlParameter("@sport_type",sportType) });
                    XElement xEe = null;
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        xEe = new XElement("SportClass", new XAttribute("className", dr["class_name"].ToString()));

                        DataSet dsSeason = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "usp_wapisport_sportclassseasonbyclassid",
                            new SqlParameter[] { new SqlParameter("@class_id",dr["class_id"].ToString())
                            ,new SqlParameter("@result_type",catId)});
                        XElement xE = null;
                        foreach(DataRow drSeason in dsSeason.Tables[0].Rows)
                        {

                            xE = new XElement("SportClassSeason", new XAttribute("seasonName", drSeason["scs_desc"].ToString()));
                            xE.Add(Command_GetSportContentDetail(elementName, catId, drSeason["scs_id"].ToString(), lang, sportType));
                            xEe.Add(xE);
                        }
                        
                        rtn.Add(xEe);
                    }
                    sqlConn.Close();
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("Command_GetSportContent >> " + ex.Message);
            }
            return rtn;
        }

        public XDocument Command_GetSportContent(XDocument rtn, string catId, string sportType, string lang)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(strConn))
                {
                    if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                    DataSet ds = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "usp_wapisport_sportclassbysporttype",
                        new SqlParameter[] { new SqlParameter("@sport_type", sportType) });
                    XElement xEe = null;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        xEe = new XElement("SportClass", new XAttribute("className", dr["class_name"].ToString()));

                        DataSet dsSeason = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "usp_wapisport_sportclassseasonbyclassid",
                            new SqlParameter[] { new SqlParameter("@class_id",dr["class_id"].ToString())
                            ,new SqlParameter("@result_type",catId)});
                        XElement xE = null;
                        foreach (DataRow drSeason in dsSeason.Tables[0].Rows)
                        {

                            xE = new XElement("SportClassSeason", new XAttribute("seasonName", drSeason["scs_desc"].ToString()));
                            xE.Add(Command_GetSportContentDetail("Season", catId, drSeason["scs_id"].ToString(), lang, sportType));
                            xEe.Add(xE);
                        }

                        rtn.Element("SportApp").Add(xEe);
                    }
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Command_GetSportContent >> " + ex.Message);
            }
            return rtn;
        }

        public XElement Command_GetSportContentDetail(string elementName, string catId, string scsId, string lang, string sportType)
        {
            XElement rtn = new XElement(elementName);
            try
            {
                DataSet ds = null;

                    ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_sportcontentbyscsid"
                            , new SqlParameter[] {new SqlParameter("@scs_id",scsId=="" ?"0":scsId)
                            ,new SqlParameter("@cat_id",catId)});

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            rtn.Add(new XAttribute("title", lang == Country.th.ToString() ? dr["scnt_title_local"].ToString() : dr["scnt_title"].ToString())
                                , new XAttribute("detail", lang == Country.th.ToString() ? dr["scnt_detail_local"].ToString() : dr["scnt_detail"].ToString()));
                        }
                    }
                    else
                    {
                        rtn.Add(new XAttribute("title", "")
                                , new XAttribute("detail", ""));
                    }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Command_GetSportContent >> " + ex.Message);
            }
            return rtn;
        }

        public XElement Command_GetSportContent(string elementName,string catId,string scsId,string lang,string sportType)
        {
            XElement rtn = new XElement(elementName);
            try
            {
                DataSet ds = null;
                if (sportType == "00002" || sportType == "00003" || sportType == "00004" || sportType == "00006" || sportType == "00007" || sportType == "00005" || sportType == "00008")
                {
                    ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_sportcontent_sporttype"
                            , new SqlParameter[] {
                        new SqlParameter("@cat_id",catId)
                        ,new SqlParameter("@content_date" , DateTime.Now.AddDays(-2))
                        ,new SqlParameter("@sport_type",sportType)
                    });
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_sportcontent_othersport"
                            , new SqlParameter[] {
                        new SqlParameter("@cat_id",catId)
                        ,new SqlParameter("@content_date" , DateTime.Now.AddDays(-2))
                        ,new SqlParameter("@scs_id",scsId=="" ?"0":scsId)
                    });
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        rtn.Add(new XElement("Data",
                            new XAttribute("title", lang == Country.th.ToString() ? dr["scnt_title_local"].ToString() : (dr["scnt_title"].ToString() == "") ? dr["scnt_title_local"].ToString() : dr["scnt_title"].ToString())
                            , new XAttribute("detail", lang == Country.th.ToString() ? dr["scnt_detail_local"].ToString() : (dr["scnt_detail"].ToString() == "") ? dr["scnt_detail_local"].ToString() : dr["scnt_detail"].ToString())
                            ));
                    }
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("Command_GetSportContent >> " + ex.Message);
            }
            return rtn;
        }

        #endregion

        #region Database isportfeed

        public XDocument Command_GetNewsBySportType_forSportPhone(string sportType, string lang, string type)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {

                    // Fix Sport Type Isport ไม่ตรงกับ Siamsport
                    switch (sportType)
                    {
                        case "00004": sportType = "6"; break;
                        case "00005": sportType = "4"; break;
                        case "00006": sportType = "5"; break;
                    }

                    DataSet ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetTopDateNews", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataSet dsDetail = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByDate", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_date", dr["match_date"].ToString(), OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
     
                            foreach (DataRow drDetail in dsDetail.Tables[0].Rows)
                            {
                                rtnXML.Element("SportApp").Add(AddElementNews(drDetail, false));
                            }

                        }

                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                    }
                    else
                    {
                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                    }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }

        public XDocument Command_GetNewsBySportType(string sportType, string lang, string type)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {

                    // Fix Sport Type Isport ไม่ตรงกับ Siamsport
                    switch (sportType)
                    {
                        case "00004": sportType = "6"; break;
                        case "00005": sportType = "4"; break;
                        case "00006": sportType = "5"; break;
                    }

                    DataSet ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetTopDateNews", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataSet dsDetail = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByDate", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_date", dr["match_date"].ToString(), OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                            XElement xElement = new XElement("News", new XAttribute("Date", AppCode_LiveScore.DatetoText(dr["match_date"].ToString().Substring(0, 8))));
                            foreach (DataRow drDetail in dsDetail.Tables[0].Rows)
                            {
                                xElement.Add(AddElementNews(drDetail,false));
                                
                            }
                            rtnXML.Element("SportApp").Add(xElement);
                        }

                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                    }
                    else
                    {
                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                    }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }
        public XDocument Command_GetSportClip(XDocument rtnXML, string sportType, string rowCount, string lang, string type)
        {
            try
            {
                int rowMin = 0, rowMax = int.Parse(rowCount);
                string clipURL = "", fileName = "";


                DataSet ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetClip", "SIAMSPORT_CLIP",
                        new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        fileName = dr["clip_stream"].ToString().Substring(dr["clip_stream"].ToString().IndexOf("=") + 1);
                        clipURL = string.Format((type == "iphone" ? ConfigurationManager.AppSettings["ApplicationURLClip_iphone"] : ConfigurationManager.AppSettings["ApplicationURLClip_default"]), fileName);
                        rtnXML.Element("SportApp").Add(new XElement("Clip"
                            , new XAttribute("clip_id", dr["clip_id"].ToString())
                            , new XAttribute("clip_topic", dr["clip_topic"].ToString())
                            , new XAttribute("clip_images", ConfigurationManager.AppSettings["ApplicationURLImages"] + "120x75clip_tn/" + dr["thumb_image"].ToString())
                            , new XAttribute("clip_url", clipURL)
                            , new XAttribute("clip_date", DateTime.Now.ToString("dd/MM/yyyy"))
                            ));
                    }

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                 , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                   , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Command_GetSportClip >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }
        public XDocument Command_GetSportClip(string sportType, string rowCount, string lang, string type)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                                                new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                                                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                                                ));
            try
            {
                int rowMin = 0, rowMax = int.Parse(rowCount);
                string clipURL = "",fileName = "";


                    DataSet ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetClip", "SIAMSPORT_CLIP",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            fileName = dr["clip_stream"].ToString().Substring(dr["clip_stream"].ToString().IndexOf("=") + 1);
                            fileName += "_small";
                            clipURL = string.Format((type == "iphone" ? ConfigurationManager.AppSettings["ApplicationURLClip_iphone"] : ConfigurationManager.AppSettings["ApplicationURLClip_default"]), fileName);
                            rtnXML.Element("SportApp").Add(new XElement("Clip"
                                , new XAttribute("clip_id", dr["clip_id"].ToString())
                                , new XAttribute("clip_topic", dr["clip_topic"].ToString())
                                , new XAttribute("clip_images", ConfigurationManager.AppSettings["ApplicationURLImages"] + "120x75clip_tn/" + dr["thumb_image"].ToString())
                                , new XAttribute("clip_url", clipURL)
                                , new XAttribute("clip_date", DateTime.Now.ToString("dd/MM/yyyy"))
                                ));
                        }

                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                    }
                    else
                    {
                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                    }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Command_GetSportClip >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XDocument Command_GetNewsBySportType_FeedDtac(XDocument rtnXML,string sportType, string rowCount, string lang, string type)
        {
            try
            {

                    // Fix Sport Type Isport ไม่ตรงกับ Siamsport
                    switch (sportType)
                    {
                        case "00004": sportType = "6"; break;
                        case "00005": sportType = "4"; break;
                        case "00006": sportType = "5"; break;
                    }
                    int rowMin = 0, rowMax = int.Parse(rowCount);

                    DataSet ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsBySportType", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            rtnXML.Element("channel").Add(new XElement("item"
                                , new XElement("title", dr["news_header_th"].ToString())
                                , new XElement("guid", dr["news_id"].ToString())
                                , new XElement("description", ReplaceTagHTML(dr["news_title_th"].ToString()))
                                , new XElement("link", "http://wap.isport.co.th/isportui/football_news_detail.aspx?p=feeddtac&pcnt_id=" + dr["news_id"].ToString())
                                , new XElement("enclosure", new XAttribute("url", "http://wap.isport.co.th/isportws/images/feeddtac/" + dr["news_sport_type"].ToString()+".jpg"), new XAttribute("type", "image/jpeg"))
                                , new XElement("pubDate", DateTime.Now.ToString("ddd, dd MMM yyyy HH:MM:ss") + " +0700")
                                ));
                        }

                        rtnXML.Element("channel").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                    }
                    else
                    {
                        rtnXML.Element("channel").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                    }


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("channel").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }

        public XDocument Command_GetNewsBySportType_AIS(string sportType, string rowCount, string lang, string type)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {


                int rowMin = 0, rowMax = int.Parse(rowCount);
                if (type == "ipad")
                {
                    rowCount = rowCount == "" ? "0" : rowCount;
                    rowMax = int.Parse(rowCount) * 5;
                    rowMin = rowMax == 5 ? 0 : rowMax - 5;
                }


                DataSet ds = null;
                if (sportType == "00001")
                {
                    ds = SqlHelper.ExecuteDataset(strConnPack, CommandType.StoredProcedure, "usp_isportstarsoccer_sport_news",
                    new SqlParameter[] { new SqlParameter("@scs_id", "") });
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(strConnPack, CommandType.StoredProcedure, "usp_isportstarsoccer_sport_news_bysporttype",
                    new SqlParameter[] { new SqlParameter("@sport_type", sportType) });
                }

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string detail = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (sportType == "00001")
                        {
                            dr["news_images_190"] = "http://wap.isport.co.th/isportws/images/arena_news_190.png";
                            dr["news_images_600"] = "http://wap.isport.co.th/isportws/images/arena_news_600.png";
                            dr["news_images_350"] = "http://wap.isport.co.th/isportws/images/arena_news_350.png";
                        }
                        else
                        {
                            dr["news_images_190"] = "http://wap.isport.co.th/isportws/images/arena_othernews_190.png";
                            dr["news_images_600"] = "http://wap.isport.co.th/isportws/images/arena_othernews_60.png";
                            dr["news_images_350"] = "http://wap.isport.co.th/isportws/images/arena_othernews_350.png";
                        }
                       // rtnXML.Element("SportApp").Add(AddElementNews(dr, false));
                        detail = ReplaceTagHTML(dr["news_detail_th1"].ToString());
                        rtnXML.Element("SportApp").Add(new XElement("News"
                               , new XAttribute("news_id", dr["news_id"].ToString())
                               , new XAttribute("news_header", dr["news_header_th"].ToString())
                               , new XAttribute("news_title", dr["news_title_th"].ToString())
                               , new XAttribute("news_detail", detail)
                               , new XAttribute("news_images_190", dr["news_images_190"].ToString())
                               , new XAttribute("news_images_1000", dr["news_images_1000"].ToString())
                               , new XAttribute("news_images_600", dr["news_images_600"].ToString())
                               , new XAttribute("news_images_400", dr["news_images_400"].ToString())
                               , new XAttribute("news_images_350", dr["news_images_350"].ToString())
                               , new XAttribute("news_images_description", dr["news_images_description"].ToString())
                               , new XAttribute("news_url_fb", ConfigurationManager.AppSettings["ApplicationURLFaceBookNews"] + dr["news_id"].ToString())));
                    }

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                 , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                   , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }


        public XDocument Command_GetNewsBySportType(string sportType, string rowCount, string lang,string type)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {

                    // Fix Sport Type Isport ไม่ตรงกับ Siamsport
                    switch (sportType)
                    {
                        case "00004":sportType = "6";break;
                        case "00005":sportType = "4";break;
                        case "00006":sportType = "5";break;
                    }
                    int rowMin = 0, rowMax = int.Parse(rowCount);
                    if (type == "ipad")
                    {
                        rowCount = rowCount == "" ? "0" : rowCount;
                        rowMax = int.Parse(rowCount) * 5;
                        rowMin = rowMax == 5 ? 0 : rowMax - 5;
                    }

                    DataSet ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsBySportType", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            rtnXML.Element("SportApp").Add(AddElementNews(dr,false));
                        }

                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                    }
                    else
                    {
                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                    }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }

        public DataSet CommandGetNewsByImagesCopyright(string sportType, string rowCount, string lang, string type)
        {
            DataSet ds = null;
            try
            {

                    ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByImagesCopy", "SIAMSPORT_NEWS",
                           new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", 10, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", 0, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetNewsByImagesCopyright >> " + ex.Message);
                throw new Exception(ex.Message);
            }
            return ds;
        }

        public XDocument Command_GetNewsFeedAis(XDocument rtnXML, string contestGroupId, string sportType, string rowCount, string lang, string countryId, string type)
        {
            try
            {


                    DataSet ds = null;

                    #region GetData
                    int rowMin = 0, rowMax = int.Parse(rowCount);
                        // Select By League
                    ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByLeagueIdCopy", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_contestGroupid", contestGroupId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });

                    #endregion

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string detail = "";
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            detail = ReplaceTagHTML(dr["news_detail_th1"].ToString());
                            rtnXML.Element("SportApp").Add(new XElement("News"
                               , new XAttribute("news_id", dr["news_id"].ToString())
                               , new XAttribute("news_header", dr["news_header_th"].ToString())
                               , new XAttribute("news_title", dr["news_title_th"].ToString())
                               , new XAttribute("news_detail", detail)
                               , new XAttribute("news_url_fb", ConfigurationManager.AppSettings["ApplicationURLFaceBookNews"] + dr["news_id"].ToString())
                               ));
                        }

                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                    }
                    else
                    {
                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                    }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }

        public XDocument Command_GetNews_FeedDtac(XDocument rtnXML, string contestGroupId, string sportType, string rowCount, string lang, string countryId, string type)
        {
            try
            {

                    DataSet ds = null;

                    #region GetData
                    int rowMin = 0, rowMax = int.Parse(rowCount);

                    if (contestGroupId == "")
                    {
                        if (countryId == "")
                        {
                            // contestGroupId = "" and countryId="";
                            ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsBySportType", "SIAMSPORT_NEWS",
                                new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                        }
                        else
                        {
                            // Select By Country
                            ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByCountry1", "SIAMSPORT_NEWS",
                                new OracleParameter[] { OrclHelper.GetOracleParameter("p_countryId", countryId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                        }
                    }
                    else
                    {
                        // Select By League
                        ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByLeagueId", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_contestGroupid", contestGroupId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                    }
                    #endregion

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                       
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            rtnXML.Element("channel").Add(new XElement("item"
                                , new XElement("title", dr["news_header_th"].ToString())
                                , new XElement("guid", dr["news_id"].ToString())
                                , new XElement("description", ReplaceTagHTML(dr["news_title_th"].ToString()))
                                //, new XElement("link", "http://wap.isport.co.th/isportui/football_news_detail.aspx?p=feeddtac&pcnt_id=" + dr["news_id"].ToString())
                                , new XElement("link", "http://wap.isport.co.th/isportui/indexl.aspx?p=d02SSC")
                                , new XElement("enclosure", new XAttribute("url", "http://wap.isport.co.th/isportws/images/feeddtac/" + GetImageDefault_FeedDtac(dr["news_league_id"].ToString())), new XAttribute("type", "image/jpeg"))
                                , new XElement("pubDate", DateTime.Now.ToString("ddd, dd MMM yyyy HH:MM:ss") + " +0700")
                                ));
                        }

                        rtnXML.Element("channel").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                    }
                    else
                    {
                        rtnXML.Element("channel").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                    }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("channel").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        private string GetImageDefault_FeedDtac(string leagueId)
        {
            try
            {
                string rtn = "";
                switch (leagueId )
                {
                    case "1": rtn = "cupen.jpg"; break;
                    case "2": rtn = "cupen.jpg"; break;
                    case "3": rtn = "cupitaly.jpg"; break;
                    case "4": rtn = "cupgermany.jpg"; break;
                    case "5": rtn = "cupspain.jpg"; break;
                    case "6": rtn = "cupfrance.jpg"; break;
                    default: rtn = "cupother.jpg"; break;
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public XDocument Command_GetNewsByDate(XDocument rtnXML, string newsDate, string sportType, string rowCount, string lang, string countryId, string type)
        {
            try
            {

                    DataSet ds = null;

                    #region GetData
                    int rowMin = 0, rowMax = int.Parse(rowCount);

                        rowCount = rowCount == "" ? "1" : rowCount;
                        rowMax = int.Parse(rowCount) * 10;
                        rowMin = rowMax == 10 ? 0 : rowMax - 10;


                    ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByDateMaxMin", "SIAMSPORT_NEWS",
                                new OracleParameter[] { OrclHelper.GetOracleParameter("p_date", newsDate, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                      
                    #endregion

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            rtnXML.Element("SportApp").Add(AddElementNews(dr, false));
                        }

                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                    }
                    else
                    {
                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                    }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }

        public XDocument Command_GetNews_AIS(XDocument rtnXML, string contestGroupId, string sportType, string rowCount, string lang, string countryId, string type)
        {
            try
            {

                DataSet ds = null;


                contestGroupId = (sportType == "00001") ? contestGroupId : "";


                #region GetData
                int rowMin = 0, rowMax = int.Parse(rowCount);
                if (type == "ipad")
                {
                    rowCount = rowCount == "" ? "0" : rowCount;
                    rowMax = int.Parse(rowCount) * 6;
                    rowMin = rowMax == 6 ? 0 : rowMax - 6;
                }
                ds = SqlHelper.ExecuteDataset(strConnPack, CommandType.StoredProcedure, "usp_isportstarsoccer_sport_news",
                    new SqlParameter[] { new SqlParameter("@scs_id", "") });
                #endregion

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //if (sportType == "00001") rtnXML.Element("SportApp").Add(AddElementFIXNewsEuro2012(type));
                    string detail = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        dr["news_images_190"] =  "http://wap.isport.co.th/isportws/images/arena_news_190.png";
                        dr["news_images_600"] = "http://wap.isport.co.th/isportws/images/arena_news_600.png";
                        dr["news_images_350"] = "http://wap.isport.co.th/isportws/images/arena_news_350.png";
                        detail = ReplaceTagHTML(dr["news_detail_th1"].ToString());
                        rtnXML.Element("SportApp").Add( new XElement("News"
                               , new XAttribute("news_id", dr["news_id"].ToString())
                               , new XAttribute("news_header", dr["news_header_th"].ToString())
                               , new XAttribute("news_title", dr["news_title_th"].ToString())
                               , new XAttribute("news_detail", detail)
                               , new XAttribute("news_images_190",  dr["news_images_190"].ToString())
                               , new XAttribute("news_images_1000",  dr["news_images_1000"].ToString())
                               , new XAttribute("news_images_600",  dr["news_images_600"].ToString())
                               , new XAttribute("news_images_400",dr["news_images_400"].ToString())
                               , new XAttribute("news_images_350",  dr["news_images_350"].ToString())
                               , new XAttribute("news_images_description", dr["news_images_description"].ToString())
                               , new XAttribute("news_url_fb", ConfigurationManager.AppSettings["ApplicationURLFaceBookNews"] + dr["news_id"].ToString())));
                    }

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                 , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                   , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }

        public XDocument Command_GetNews(XDocument rtnXML, string contestGroupId, string sportType, string rowCount, string lang, string countryId,string type)
        {
            try
            {

                    DataSet ds = null;


                    contestGroupId = (sportType == "00001") ? contestGroupId : "";

                    if (sportType == "00004") sportType = "00011"; // fomula 1 ไม่มีข่าว
                    else if (sportType == "00005") sportType = "00004";
                    else if (sportType == "00006") sportType = "00005";
                    else if (sportType == "00008") sportType = "00009"; // สนุกไม่มีข่าว


                    #region GetData
                    int rowMin = 0, rowMax = int.Parse(rowCount);
                    if (type == "ipad")
                    {
                        rowCount = rowCount == "" ? "0" : rowCount;
                        rowMax = int.Parse(rowCount) * 6;
                        rowMin = rowMax == 6 ? 0 : rowMax - 6;
                    }
                    if (contestGroupId == "")
                    {
                        if (countryId == "")
                        {
                            // contestGroupId = "" and countryId="";
                            ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsBySportType", "SIAMSPORT_NEWS",
                                new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                        }
                        else
                        {
                            // Select By Country
                            ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByCountry1", "SIAMSPORT_NEWS",
                                new OracleParameter[] { OrclHelper.GetOracleParameter("p_countryId", countryId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                        }
                    }
                    else
                    {
                        // Select By League
                        ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByLeagueId", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_contestGroupid", contestGroupId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                    }
                    #endregion

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        //if (sportType == "00001") rtnXML.Element("SportApp").Add(AddElementFIXNewsEuro2012(type));
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            rtnXML.Element("SportApp").Add(AddElementNews(dr, false));
                        }

                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                    }
                    else
                    {
                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                    }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }

        public XDocument Command_GetNews_Test(XDocument rtnXML, string contestGroupId, string sportType, string rowCount, string lang, string countryId, string type)
        {
            try
            {

                DataSet ds = null;


                contestGroupId = (sportType == "00001") ? contestGroupId : "";


                #region GetData
                int rowMin = 0, rowMax = int.Parse(rowCount);
                if (type == "ipad")
                {
                    rowCount = rowCount == "" ? "0" : rowCount;
                    rowMax = int.Parse(rowCount) * 6;
                    rowMin = rowMax == 6 ? 0 : rowMax - 6;
                }
                if (contestGroupId == "")
                {
                    if (countryId == "")
                    {
                        // contestGroupId = "" and countryId="";
                        ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsBySportType", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                    }
                    else
                    {
                        // Select By Country
                        ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByCountry1", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_countryId", countryId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                    }
                }
                else
                {
                    // Select By League
                    ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByLeagueId", "SIAMSPORT_NEWS",
                        new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_contestGroupid", contestGroupId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                }
                #endregion

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //if (sportType == "00001") rtnXML.Element("SportApp").Add(AddElementFIXNewsEuro2012(type));
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dr["news_detail_th1"] = "";
                        
                        rtnXML.Element("SportApp").Add(AddElementNews(dr, false));
                    }

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                 , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                   , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }

        public XDocument Command_GetNewsImages(XDocument rtnXML, string contestGroupId, string sportType, string rowCount, string lang, string countryId, string type)
        {
            try
            {
                DataSet ds = null;

                #region GetData
                int rowMin = 0, rowMax = int.Parse(rowCount);
                if (type == "ipad")
                {
                    rowCount = rowCount == "" ? "0" : rowCount;
                    rowMax = int.Parse(rowCount) * 6;
                    rowMin = rowMax == 6 ? 0 : rowMax - 6;
                }
                if (contestGroupId == "")
                {
                    // contestGroupId = "" and countryId="";
                    ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetImagesBySportType", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                }
                else
                {
                    // Select By League
                    if (int.Parse(sportType) == 8)
                    {
                        // FHM
                        ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetImagesByLeagueId", "SIAMSPORT_NEWS",
                           new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_contestGroupid", contestGroupId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                    }
                    else
                    {
                        ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByLeagueId", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_contestGroupid", contestGroupId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                    }
                }
                #endregion

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (int.Parse(sportType) == 8)
                        {
                            rtnXML.Element("SportApp").Add(AddElementImagesFHM(dr, "News"));
                        }
                        else
                        {

                            rtnXML.Element("SportApp").Add(AddElementNews(dr, true));
                        }
                    }

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                 , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                   , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }

        public XDocument Command_GetNewsImages_AIS(XDocument rtnXML, string contestGroupId, string sportType, string rowCount, string lang, string countryId, string type,string fhmElementName)
        {
            try
            {
                    DataSet ds = null;

                    #region GetData
                    int rowMin = 0, rowMax = int.Parse(rowCount);
                    if (type == "ipad")
                    {
                        rowCount = rowCount == "" ? "0" : rowCount;
                        rowMax = int.Parse(rowCount) * 6;
                        rowMin = rowMax == 6 ? 0 : rowMax - 6;
                    }
                    if (contestGroupId == "")
                    {
                            // contestGroupId = "" and countryId="";
                        ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetImagesBySportType", "SIAMSPORT_NEWS",
                                new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                    }
                    else
                    {
                        // Select By League
                        if (int.Parse(sportType) == 8)
                        {
                               // FHM
                            ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetImagesByLeagueId", "SIAMSPORT_NEWS",
                               new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_contestGroupid", contestGroupId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                        }
                        else
                        {
                            ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByLeagueId", "SIAMSPORT_NEWS",
                                new OracleParameter[] { OrclHelper.GetOracleParameter("p_rowmax", rowMax, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_contestGroupid", contestGroupId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                        }
                    }
                    #endregion

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (int.Parse(sportType) == 8)
                            {
                                rtnXML.Element("SportApp").Add(AddElementImagesFHM(dr, fhmElementName));
                            }
                            else
                            {
                                dr["news_images_190"] = "http://wap.isport.co.th/isportws/images/arena_news_190.png";
                                dr["news_images_600"] = "http://wap.isport.co.th/isportws/images/arena_news_600.png";
                                dr["news_images_350"] = "http://wap.isport.co.th/isportws/images/arena_news_350.png";
                                rtnXML.Element("SportApp").Add(AddElementNews(dr, true));
                            }
                        }

                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                    }
                    else
                    {
                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                    }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }

        public XDocument Command_GetNewsFootballThai_FeedDtac(XDocument rtnXML, string contestGroupId, string sportType, string rowCount, string lang, string countryId, string type)
        {
            try
            {

                    int rowMin = 0, rowMax = int.Parse(rowCount);
                    if (type == "ipad")
                    {
                        rowCount = rowCount == "" ? "0" : rowCount;
                        rowMax = int.Parse(rowCount) * 6;
                        rowMin = rowMax == 6 ? 0 : rowMax - 6;
                    }

                    DataSet ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByFootballThai", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_scs_id", contestGroupId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmax", rowMax , OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin , OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            rtnXML.Element("channel").Add(new XElement("item"
                                , new XElement("title", dr["news_header_th"].ToString())
                                , new XElement("guid", dr["news_id"].ToString())
                                , new XElement("description", ReplaceTagHTML(dr["news_title_th"].ToString()))
                                //, new XElement("link", "http://wap.isport.co.th/isportui/football_news_detail.aspx?p=feeddtac&pcnt_id=" + dr["news_id"].ToString())
                                , new XElement("link", "http://wap.isport.co.th/isportui/indexl.aspx?p=d02SSC")
                                , new XElement("enclosure", new XAttribute("url", ConfigurationManager.AppSettings["ApplicationURLImages"] + "120x75/" + dr["news_images_190"].ToString()), new XAttribute("type", "image/jpeg"))
                                , new XElement("pubDate", DateTime.Now.ToString("ddd, dd MMM yyyy HH:MM:ss") + " +0700")
                                ));
                            
                        }

                        rtnXML.Element("channel").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                    }
                    else
                    {
                        rtnXML.Element("channel").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                    }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("channel").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }

        public XDocument Command_GetNewsFootballThai(XDocument rtnXML, string contestGroupId, string sportType, string rowCount, string lang, string countryId,string type)
        {
            try
            {

                int rowMin = 0, rowMax = int.Parse(rowCount);
                if (type == "ipad")
                {
                    rowCount = rowCount == "" ? "0" : rowCount;
                    rowMax = int.Parse(rowCount) * 6;
                    rowMin = rowMax == 6 ? 0 : rowMax - 6;
                }

                DataSet ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsByFootballThai", "SIAMSPORT_NEWS",
                        new OracleParameter[] { OrclHelper.GetOracleParameter("p_scs_id", contestGroupId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmax", rowMax , OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_rowmin", rowMin , OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_sportType", sportType, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        rtnXML.Element("SportApp").Add(AddElementNews(dr, false));
                    }

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                 , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                   , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }

        public static string ReplaceTagHTML(string detail)
        {
            try
            {
                string rtn = detail;
                if (detail.IndexOf("<") > -2)
                {
                    if (detail.IndexOf("<") == 0)
                    {
                        rtn = rtn.Substring(detail.IndexOf(">") + 1);
                    }
                    else if (detail.IndexOf(">") > -1)
                    {
                        rtn = rtn.Substring(0, detail.IndexOf("<") - 1) + rtn.Substring(detail.IndexOf(">") + 1);
                    }
                    else
                    {
                        rtn = rtn.Substring(0, detail.IndexOf("<") - 1);
                    }
                    rtn = ReplaceTagHTML(rtn);
                }
                return rtn;
            }
            catch
            {
                return detail;
            }
        }

        private XElement AddElementImagesFHM(DataRow dr,string elementName )
        {
            try
            {
                //string detail = ReplaceTagHTML(dr["news_detail_th1"].ToString() + dr["news_detail_th2"].ToString());
                XElement rtn = null;

                rtn = new XElement(elementName //"News"
                              , new XAttribute("news_id", dr["news_id"].ToString())
                              , new XAttribute("contestgroupid", dr["news_league_id"].ToString())
                              , new XAttribute("news_images_190", ConfigurationManager.AppSettings["ApplicationPathImageFHM"] + "190/" + dr["news_images_190"].ToString())
                              , new XAttribute("news_images_1000", ConfigurationManager.AppSettings["ApplicationPathImageFHM"] + "500x300/" + dr["news_images_1000"].ToString())
                              , new XAttribute("news_images_600", ConfigurationManager.AppSettings["ApplicationPathImageFHM"] + "600/" + dr["news_images_600"].ToString())
                              , new XAttribute("news_images_400", ConfigurationManager.AppSettings["ApplicationPathImageFHM"] + "500x300/" + dr["news_images_400"].ToString())
                              , new XAttribute("news_images_350", ConfigurationManager.AppSettings["ApplicationPathImageFHM"] + "350/" + dr["news_images_350"].ToString())
                              , new XAttribute("news_images_description", dr["news_images_description"].ToString())
                              );


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("AddElementNews>>" + ex.Message);
            }
        }


        private XElement AddElementNews(DataRow dr ,bool isImages)
        {
            try
            {
                //string detail = ReplaceTagHTML(dr["news_detail_th1"].ToString() + dr["news_detail_th2"].ToString());
                string detail = ReplaceTagHTML(dr["news_detail_th1"].ToString());
                XElement rtn = null;
                if (isImages)
                {
                     rtn = new XElement("News"
                               , new XAttribute("news_id", dr["news_id"].ToString())
                               , new XAttribute("contestgroupid", dr["contestgroupid"].ToString())
                               , new XAttribute("news_images_190", ConfigurationManager.AppSettings["ApplicationURLImages"] + "120x75/" + dr["news_images_190"].ToString())
                               , new XAttribute("news_images_1000", ConfigurationManager.AppSettings["ApplicationURLImages"] + "500x300/" + dr["news_images_1000"].ToString())
                               , new XAttribute("news_images_600", ConfigurationManager.AppSettings["ApplicationURLImages"] + "500x300/" + dr["news_images_600"].ToString())
                               , new XAttribute("news_images_400", ConfigurationManager.AppSettings["ApplicationURLImages"] + "500x300/" + dr["news_images_400"].ToString())
                               , new XAttribute("news_images_350", ConfigurationManager.AppSettings["ApplicationURLImages"] + "325x200/" + dr["news_images_350"].ToString())
                               , new XAttribute("news_images_description", dr["news_images_description"].ToString())
                               );
                }
                else
                {
                     rtn = new XElement("News"
                               , new XAttribute("news_id", dr["news_id"].ToString())
                               , new XAttribute("news_header", dr["news_header_th"].ToString())
                               , new XAttribute("news_title", dr["news_title_th"].ToString())
                               , new XAttribute("news_detail", detail)
                               , new XAttribute("news_images_190", ConfigurationManager.AppSettings["ApplicationURLImages"] + "120x75/" + dr["news_images_190"].ToString())
                               , new XAttribute("news_images_1000", ConfigurationManager.AppSettings["ApplicationURLImages"] + "500x300/" + dr["news_images_1000"].ToString())
                               , new XAttribute("news_images_600", ConfigurationManager.AppSettings["ApplicationURLImages"] + "500x300/" + dr["news_images_600"].ToString())
                               , new XAttribute("news_images_400", ConfigurationManager.AppSettings["ApplicationURLImages"] + "500x300/" + dr["news_images_400"].ToString())
                               , new XAttribute("news_images_350", ConfigurationManager.AppSettings["ApplicationURLImages"] + "325x200/" + dr["news_images_350"].ToString())
                               , new XAttribute("news_images_description", dr["news_images_description"].ToString())
                               , new XAttribute("news_url_fb", ConfigurationManager.AppSettings["ApplicationURLFaceBookNews"] + dr["news_id"].ToString())
                               );
                }
               
                return rtn;
            }
            catch(Exception ex)
            {
                throw new Exception("AddElementNews>>" + ex.Message);
            }
        }

        public DataSet Command_GetNewsById(string newsId)
        {
            try
            {
                DataSet ds = null;

                    ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsById", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_newsId", newsId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });

                    

                return ds;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Command_GetNewsById >> " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region TPL Clip
        public XDocument CommandGetContentClipbyCatId(XDocument xDoc, string pageNumber, string eleName, string lang, string teamCode)
        {
            string status = "Success", desc = "";
            try
            {
            }
            catch (Exception ex)
            {
                status = "Error";
                desc = ex.Message;
                ExceptionManager.WriteError("CommandGetContentClipbyCatId >> " + ex.Message);
            }
            xDoc.Element(eleName).Add(new XElement("status", status), new XElement("message", desc));
            return xDoc;
        }
        public XDocument CommandGetContentClipbyTeamCode(XDocument xDoc,string pageNumber,string eleName,string lang,string teamCode)
        {
            string status = "Success", desc = "";
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_contentclip_selectbyteamcode",
                    new SqlParameter[] {new SqlParameter("@teamCode",teamCode) });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    pageNumber = int.Parse(pageNumber) > 3 ? "3" : pageNumber;
                    int start = pageNumber == "1" ? 0 : (pageNumber == "2" ? 3 : 6);
                    for (int i = start; i < start + 3; i++)
                    {
                        DataRow dr = ds.Tables[0].Rows[i];
                        xDoc.Element(eleName).Add(new XElement("Clip", new XAttribute("clip_path", ConfigurationManager.AppSettings["IsportPathClip"] + "/" + dr["cscat_id"].ToString() + "/" + dr["clip_path"].ToString())
                                                                                                   , new XAttribute("pic_path", ConfigurationManager.AppSettings["IsportPathPic"] + "/" + dr["cscat_id"].ToString() + "/" + dr["pic_path"].ToString())
                                                                                                   , new XAttribute("clip_date", AppCode_LiveScore.DateText(dr["content_date"].ToString()))
                                                                                                   , new XAttribute("clip_title", dr["title"].ToString())
                                                                                                   , new XAttribute("clip_desc", dr["description"].ToString())
                                                                                                   , new XAttribute("msch_id", dr["msch_id"].ToString())
                                                                                                   ));
                    }
                }
                else
                {
                    status = "Error";
                    desc = "clip not find";
                }

            }
            catch(Exception ex)
            {
                status = "Error";
                desc = ex.Message;
                ExceptionManager.WriteError("CommandGetContentClipbyTeamCode >> " + ex.Message);
            }
            xDoc.Element(eleName).Add(new XElement("status", status), new XElement("message", desc));
            return xDoc;
        }
        #endregion

    }
}
