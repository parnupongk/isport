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
namespace WS_BB
{
    public class AppCode_FootballList : AppCode_Base
    {
        #region Database Isport
        public struct Out_FootballList
        {
            public string status;
            public string errorMess;
            public List<FootballClass> footballClass;
            public List<FootballTopScore> footballTopScore;
        }
        public class FootballClass
        {
            
            public string scsId;
            public string classId;
            public string classNameTH;
            public string classNameEN;
            public string countryId;
            public string countryTH;
            public string countryEN;
            public string imgPath;
        }
        public class FootballTopScore
        {
            public string playerCode;
            public string score;
            public string teamCode;
            public string teamNameEN;
            public string teamNameTH;
            public string playerNameTH;
            public string playerNameEN;
        }

        /// <summary>
        /// Get ลีก list
        /// </summary>
        /// <returns></returns>
        public Out_FootballList CommandGetFootBallClass()
        {
            Out_FootballList footballList = new Out_FootballList();
            List<FootballClass> footballClass = new List<FootballClass>();
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(strConn, "usp_BB_GetFooterClass");
                FootballClass football;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    football = new FootballClass();
                    football.scsId = dr["scs_id"].ToString();
                    football.classId = dr["class_id"].ToString();
                    football.classNameEN = dr["class_name"].ToString();
                    football.classNameTH = dr["class_name_local"].ToString();
                    football.countryId = dr["ctr_id"].ToString();
                    football.countryTH = dr["ctr_name_local"].ToString();
                    football.countryEN = dr["ctr_name"].ToString();
                    football.imgPath = "http://wap.isport.co.th/isportws/images/" + dr["class_id"].ToString()+".png";
                    footballClass.Add(football);
                }
                
            }
            catch (Exception ex)
            {
                footballList.status = "Error";
                footballList.errorMess = "GetLiveScore >> " + ex.Message;
            }
            footballList.footballClass = footballClass;
            return footballList;
        }

        /// <summary>
        /// Get ชื่อลีก
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public Out_FootballList CommandGetFootBallClassByClassID(string classId)
        {
            Out_FootballList footballList = new Out_FootballList();
            List<FootballClass> footballClass = new List<FootballClass>();
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(strConn, "usp_BB_SportClass_GetClassName"
                    , new SqlParameter[]{new SqlParameter("@class_id",classId) });
                FootballClass football;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    football = new FootballClass();
                    football.classId = dr["class_id"].ToString();
                    football.classNameEN = dr["class_name"].ToString();
                    football.classNameTH = dr["class_name_local"].ToString();
                    footballClass.Add(football);
                }

            }
            catch (Exception ex)
            {
                footballList.status = "Error";
                footballList.errorMess = "CommandGetFootBallClassByClassID >> " + ex.Message;
            }
            footballList.footballClass = footballClass;
            return footballList;
        }


        /// <summary>
        /// Get get อันดับดาวซัลโว
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public Out_FootballList CommandGetFootBallTopScore(string scsId)
        {
            Out_FootballList footballList = new Out_FootballList();
            List<FootballTopScore> topScore = new List<FootballTopScore>();
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(strConn, "usp_BB_football_topscore"
                    , new SqlParameter[] { new SqlParameter("@scs_Id", scsId) });
                FootballTopScore football;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    football = new FootballTopScore();
                    football.playerCode = dr["player_code"].ToString();
                    football.score = dr["score"].ToString();
                    football.teamCode = dr["team_code"].ToString();
                    football.teamNameTH = dr["teamNameTH"].ToString();
                    football.teamNameEN = dr["team_name"].ToString();
                    football.playerNameTH = dr["playerNameTH"].ToString();
                    football.playerNameEN = dr["playerNameEN"].ToString();
                    topScore.Add(football);
                }

            }
            catch (Exception ex)
            {
                footballList.status = "Error";
                footballList.errorMess = "CommandGetFootBallTopScore >> " + ex.Message;
            }
            footballList.footballTopScore = topScore;
            return footballList;
        }

        #endregion

        #region Database IsportFeed

        public DataView FillterLeague(DataView dv , string contestGroupId,string country)
        {
            try
            {
                System.Text.StringBuilder strB = new System.Text.StringBuilder(string.Empty);
                if (country != "")
                {
                    strB.Append(" country_id =" + country);
                    dv.RowFilter = strB.ToString();
                }
                else if(contestGroupId != "")
                {
                    strB.Append(" contestgroupid in (" + contestGroupId + ") ");
                    dv.RowFilter = strB.ToString();
                }
                return dv;
            }
            catch(Exception ex)
            {
                throw new Exception("FillterLeague >> " + ex.Message);
            }
        }
        public DataView FillterCountry(DataView dv, string countryId,bool isNotIN)
        {
            try
            {
                System.Text.StringBuilder strB = new System.Text.StringBuilder(string.Empty);
                if (countryId != "")
                {
                    string command = isNotIN ? " country_id not in (" + countryId + ") " : " country_id in (" + countryId + ") ";
                    strB.Append(command);
                    dv.RowFilter = strB.ToString();
                }
                return dv;
            }
            catch (Exception ex)
            {
                throw new Exception("FillterCountry >> " + ex.Message);
            }
        }

        public XDocument Command_GetFootballLeagueMain(string lang, string type)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingHeader"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                DataSet dsLeague = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballleague"
                    , new SqlParameter[] { new SqlParameter("@contestGroupId", "") });

                if (dsLeague.Tables.Count > 0 && dsLeague.Tables[0].Rows.Count > 0)
                {

                    #region Get & Add League
                    string eName = (type == "BB") ? "Country" : "TopLeague";
                    XElement xElement = new XElement(eName
                        //, new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                        //, new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                        , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                        , new XAttribute("countryName", "")
                        , new XAttribute("countryId", "")
                        );
                    DataView dv = FillterLeague(dsLeague.Tables[0].DefaultView, "21,85,135,116,19,13,14,422,325,807", "");
                    string img16 = "",img48="";
                    for (int i = 0; i < dv.Count; i++)
                    {
                        img16 = dv[i]["pic_16x11"].ToString()=="" ? "default.png" : dv[i]["pic_16x11"].ToString();
                        img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                        xElement.Add(new XElement("League",
                            new XAttribute("iconFileName16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                            , new XAttribute("iconFileName48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString() + img48)
                            , new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dv[i]["contestgroup_name_th"].ToString() : dv[i]["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dv[i]["contestgroupid"].ToString())
                            ));
                    }

                    // Add Thai league
                    xElement.Add(new XElement("League",
                            new XAttribute("iconFileName16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + "2612.png")
                            , new XAttribute("iconFileName48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString()  + "2612.png")
                            , new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? "ฟุตบอลไทยลีก" : "Thai League")
                            , new XAttribute("contestGroupId", "2612")
                            ));
                    rtnXML.Element("SportApp").Add(xElement);
                    #endregion

                    

                    //rtnXML = Command_GetFootballLeague_SiamSport(rtnXML, lang);

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "error")
                        , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Command_GetFootballLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XDocument Command_GetFootballLeague_IsportStarSoccer(XDocument rtnXML,string lang, string type)
        {
            try
            {
                DataSet dsLeague = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballleague"
                    , new SqlParameter[] { new SqlParameter("@contestGroupId", "") });

                if (dsLeague.Tables.Count > 0 && dsLeague.Tables[0].Rows.Count > 0)
                {
                    string img16 = "", img48 = "";

                    #region Get & Add League
                    //string eName = (type == "BB") ? "Country" : "TopLeague";
                    //XElement xElement = new XElement(eName
                    //    , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                    //    , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                    //    , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                    //    , new XAttribute("countryName", "")
                    //    , new XAttribute("countryId", "")
                    //    );
                    //DataView dv = FillterLeague(dsLeague.Tables[0].DefaultView, "21,85,135,116,19,13,14,422,325,807", "");
                    //for (int i = 0; i < dv.Count; i++)
                    //{
                    //    img16 = dv[i]["pic_16x11"].ToString() == "" ? "default.png" : dv[i]["pic_16x11"].ToString();
                    //    img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                    //    xElement.Add(new XElement("League",
                    //        new XAttribute("iconFileName16x11", img16)
                    //        , new XAttribute("iconFileName48x48", img48)
                    //        , new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dv[i]["contestgroup_name_th"].ToString() : dv[i]["contestgroup_name"].ToString())
                    //        , new XAttribute("contestGroupId", dv[i]["contestgroupid"].ToString())
                    //        ));
                    //}
                    //rtnXML.Element("SportApp").Add(xElement);
                    #endregion

                    #region StartSoccer โปรแกรมทั้งฤดูกาล
                    //if (type != "BB")
                    //{
                    //xElement = new XElement("StarSoccer_Menu"
                    //    , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                    //    , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                    //    , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                    //    , new XAttribute("countryName", "")
                    //    , new XAttribute("countryId", "")
                    //    );
                    //dv = FillterLeague(dsLeague.Tables[0].DefaultView, "21,85,135,116,19,13,22,89,817,427,807", "");
                    //for (int i = 0; i < dv.Count; i++)
                    //{
                    //    img16 = dv[i]["pic_16x11"].ToString() == "" ? "default.png" : dv[i]["pic_16x11"].ToString();
                    //    img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                    //    xElement.Add(new XElement("League",
                    //        new XAttribute("iconFileName16x11", img16)
                    //        , new XAttribute("iconFileName48x48", img48)
                    //        , new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dv[i]["contestgroup_name_th"].ToString() : dv[i]["contestgroup_name"].ToString())
                    //        , new XAttribute("contestGroupId", dv[i]["contestgroupid"].ToString())
                    //        ));
                    //}
                    //rtnXML.Element("SportApp").Add(xElement);
                    //}
                    #endregion

                    #region Get&Add Country
                    DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballcountry"
                    , new SqlParameter[] { new SqlParameter("@countryId", "") });
                    DataView dv = FillterCountry(ds.Tables[0].DefaultView, "907,276,380,724,250,764,916", false);
                    for (int i = 0; i < dv.Count; i++)
                    {
                        img16 = dv[i]["pic_16x11"].ToString() == "" ? "default.png" : dv[i]["pic_16x11"].ToString();
                        img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                        XElement xElement = new XElement("Country"
                        , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                        , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                        , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                        , new XAttribute("countryName", (lang == Country.th.ToString()) ? dv[i]["ctr_name_local"].ToString() : dv[i]["country_name"].ToString())
                        , new XAttribute("countryId", dv[i]["country_id"].ToString())
                        , new XAttribute("id", dv[i]["country_id"].ToString())
                        , new XAttribute("iconFileName16x11", img16)
                        , new XAttribute("iconFileName48x48", img48)
                        );

                        DataView dvLeague = (dv[i]["country_id"].ToString() == "916") ? FillterLeague(dsLeague.Tables[0].DefaultView, "807,13,14,325,326,422,524", "") : FillterLeague(dsLeague.Tables[0].DefaultView, "", dv[i]["country_id"].ToString());
                        for (int index = 0; index < dvLeague.Count; index++)
                        {
                            xElement.Add(new XElement("League",
                                //new XAttribute("iconFileName16x11", dvLeague[index]["pic_16x11"].ToString())
                                //, new XAttribute("iconFileName48x48", dvLeague[index]["pic_48x48"].ToString())
                                 new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dvLeague[index]["contestgroup_name_th"].ToString() : dvLeague[index]["contestgroup_name"].ToString())
                                , new XAttribute("contestGroupId", dvLeague[index]["contestgroupid"].ToString())
                                ));
                        }

                        rtnXML.Element("SportApp").Add(xElement);

                    }
                    #endregion

                    #region All Country
                    //if (type != "BB")
                    //{
                    //dv = FillterCountry(ds.Tables[0].DefaultView, "907,276,380,724,250", true);
                    //xElement = new XElement("Country"
                    //    , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                    //    , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                    //    , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                    //    , new XAttribute("countryName", (lang == Country.th.ToString()) ? "ประเทศ อื่นๆ" : "All Country")
                    //    , new XAttribute("countryId", "all")
                    //    , new XAttribute("id", "all")
                    //    , new XAttribute("iconFileName16x11", "")
                    //    , new XAttribute("iconFileName48x48", "")
                    //    );
                    //for (int i = 0; i < dv.Count; i++)
                    //{
                    //    img16 = dv[i]["pic_16x11"].ToString() == "" ? "default.png" : dv[i]["pic_16x11"].ToString();
                    //    img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                    //    DataView dvLeague = FillterLeague(dsLeague.Tables[0].DefaultView, "", dv[i]["country_id"].ToString());
                    //    xElement.Add(new XElement("League"
                    //    , new XAttribute("iconFileName16x11", img16)
                    //    , new XAttribute("iconFileName48x48", img48)
                    //    , new XAttribute("countryName", (lang == Country.th.ToString()) ? dv[i]["ctr_name_local"].ToString() : dv[i]["country_name"].ToString())
                    //    , new XAttribute("countryId", dv[i]["country_id"].ToString())
                    //    ));
                    //}
                    //rtnXML.Element("SportApp").Add(xElement);
                    ////}
                    #endregion

                    //rtnXML = Command_GetFootballLeague_SiamSport(rtnXML, lang);

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "error")
                        , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Command_GetFootballLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XDocument Command_GetFootballLeague_StarSoccer(string lang, string type)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingHeader"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                DataSet dsLeague = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballleague"
                    , new SqlParameter[] { new SqlParameter("@contestGroupId", "") });

                if (dsLeague.Tables.Count > 0 && dsLeague.Tables[0].Rows.Count > 0)
                {
                    string img16 = "", img48 = "";
                    #region Get & Add League
                    string eName = (type == "BB") ? "Country" : "TopLeague";
                    XElement xElement = new XElement(eName
                        , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                        , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                        , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                        , new XAttribute("countryName", "")
                        , new XAttribute("countryId", "")
                        );
                    DataView dv = FillterLeague(dsLeague.Tables[0].DefaultView, "21,85,135,116,19,13,14,422,325,807", "");
                    for (int i = 0; i < dv.Count; i++)
                    {
                        img16 = dv[i]["pic_16x11"].ToString() == "" ? "default.png" : dv[i]["pic_16x11"].ToString();
                        img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                        xElement.Add(new XElement("League",
                            new XAttribute("iconFileName16x11", img16)
                            , new XAttribute("iconFileName48x48", img48)
                            , new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dv[i]["contestgroup_name_th"].ToString() : dv[i]["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dv[i]["contestgroupid"].ToString())
                            ));
                    }
                    rtnXML.Element("SportApp").Add(xElement);
                    #endregion

                    #region StartSoccer โปรแกรมทั้งฤดูกาล
                    //if (type != "BB")
                    //{
                    xElement = new XElement("StarSoccer_Menu"
                        , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                        , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                        , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                        , new XAttribute("countryName", "")
                        , new XAttribute("countryId", "")
                        );
                    dv = FillterLeague(dsLeague.Tables[0].DefaultView, "21,85,135,116,19,13,22,89,817,427,807", "");
                    for (int i = 0; i < dv.Count; i++)
                    {
                        img16 = dv[i]["pic_16x11"].ToString() == "" ? "default.png" : dv[i]["pic_16x11"].ToString();
                        img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                        xElement.Add(new XElement("League",
                            new XAttribute("iconFileName16x11", img16)
                            , new XAttribute("iconFileName48x48", img48)
                            , new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dv[i]["contestgroup_name_th"].ToString() : dv[i]["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dv[i]["contestgroupid"].ToString())
                            ));
                    }
                    rtnXML.Element("SportApp").Add(xElement);
                    //}
                    #endregion

                    #region Get&Add Country
                    DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballcountry"
                    , new SqlParameter[] { new SqlParameter("@countryId", "") });
                    dv = FillterCountry(ds.Tables[0].DefaultView, "907,276,380,724,250,764,916", false);
                    for (int i = 0; i < dv.Count; i++)
                    {
                        img16 = dv[i]["pic_16x11"].ToString() == "" ? "default.png" : dv[i]["pic_16x11"].ToString();
                        img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                        xElement = new XElement("Country"
                        , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                        , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                        , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                        , new XAttribute("countryName", (lang == Country.th.ToString()) ? dv[i]["ctr_name_local"].ToString() : dv[i]["country_name"].ToString())
                        , new XAttribute("countryId", dv[i]["country_id"].ToString())
                        , new XAttribute("id", dv[i]["country_id"].ToString())
                        , new XAttribute("iconFileName16x11", img16)
                        , new XAttribute("iconFileName48x48", img48)
                        );

                        DataView dvLeague = (dv[i]["country_id"].ToString() == "916")? FillterLeague(dsLeague.Tables[0].DefaultView, "13,14,325,326,422", "")  : FillterLeague(dsLeague.Tables[0].DefaultView, "", dv[i]["country_id"].ToString());
                        for (int index = 0; index < dvLeague.Count; index++)
                        {
                            xElement.Add(new XElement("League",
                                //new XAttribute("iconFileName16x11", dvLeague[index]["pic_16x11"].ToString())
                                //, new XAttribute("iconFileName48x48", dvLeague[index]["pic_48x48"].ToString())
                                 new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dvLeague[index]["contestgroup_name_th"].ToString() : dvLeague[index]["contestgroup_name"].ToString())
                                , new XAttribute("contestGroupId", dvLeague[index]["contestgroupid"].ToString())
                                ));
                        }

                        rtnXML.Element("SportApp").Add(xElement);

                    }
                    #endregion

                    #region All Country
                    //if (type != "BB")
                    //{
                    dv = FillterCountry(ds.Tables[0].DefaultView, "907,276,380,724,250", true);
                    xElement = new XElement("Country"
                        , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                        , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                        , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                        , new XAttribute("countryName", (lang == Country.th.ToString()) ? "ประเทศ อื่นๆ" : "All Country")
                        , new XAttribute("countryId", "all")
                        , new XAttribute("id", "all")
                        , new XAttribute("iconFileName16x11", "")
                        , new XAttribute("iconFileName48x48", "")
                        );
                    for (int i = 0; i < dv.Count; i++)
                    {
                        img16 = dv[i]["pic_16x11"].ToString() == "" ? "default.png" : dv[i]["pic_16x11"].ToString();
                        img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                        DataView dvLeague = FillterLeague(dsLeague.Tables[0].DefaultView, "", dv[i]["country_id"].ToString());
                        xElement.Add(new XElement("League"
                        , new XAttribute("iconFileName16x11", img16)
                        , new XAttribute("iconFileName48x48", img48)
                        , new XAttribute("countryName", (lang == Country.th.ToString()) ? dv[i]["ctr_name_local"].ToString() : dv[i]["country_name"].ToString())
                        , new XAttribute("countryId", dv[i]["country_id"].ToString())
                        ));
                    }
                    rtnXML.Element("SportApp").Add(xElement);
                    //}
                    #endregion

                    rtnXML = Command_GetFootballLeague_SiamSport(rtnXML, lang);

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "error")
                        , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Command_GetFootballLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XDocument Command_GetFootballLeague_Sportarena(XDocument rtnXML, string lang, string type)
        {
            //XDocument rtnXML = new XDocument(new XElement("SportApp",
            //    new XAttribute("header", ConfigurationManager.AppSettings["wordingHeader"])
            //    , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
            //    ));
            try
            {
                DataSet dsLeague = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballleague"
                    , new SqlParameter[] { new SqlParameter("@contestGroupId", "") });

                if (dsLeague.Tables.Count > 0 && dsLeague.Tables[0].Rows.Count > 0)
                {
                    string img16 = "", img48 = "";
                    XElement xElement = null;
                    DataView dv = null;


                        #region Get&Add Country
                        DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballcountry"
                        , new SqlParameter[] { new SqlParameter("@countryId", "") });
                        dv = FillterCountry(ds.Tables[0].DefaultView, "907,276,380,724,250,916", false);
                        for (int i = 0; i < dv.Count; i++)
                        {
                            img16 = dv[i]["pic_16x11"].ToString() == "" ? "default.png" : dv[i]["pic_16x11"].ToString();
                            img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                            xElement = new XElement("Country"
                            , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                            , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                            , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                            , new XAttribute("countryName", (lang == Country.th.ToString()) ? dv[i]["ctr_name_local"].ToString() : dv[i]["country_name"].ToString())
                            , new XAttribute("countryId", dv[i]["country_id"].ToString())
                            , new XAttribute("id", dv[i]["country_id"].ToString())
                            , new XAttribute("iconFileName16x11", img16)
                            , new XAttribute("iconFileName48x48", img48)
                            );

                            DataView dvLeague = FillterLeague(dsLeague.Tables[0].DefaultView, "", dv[i]["country_id"].ToString());
                            for (int index = 0; index < dvLeague.Count; index++)
                            {
                                xElement.Add(new XElement("League",
                                    //new XAttribute("iconFileName16x11", dvLeague[index]["pic_16x11"].ToString())
                                    //, new XAttribute("iconFileName48x48", dvLeague[index]["pic_48x48"].ToString())
                                     new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dvLeague[index]["contestgroup_name_th"].ToString() : dvLeague[index]["contestgroup_name"].ToString())
                                    , new XAttribute("contestGroupId", dvLeague[index]["contestgroupid"].ToString())
                                    ));
                            }

                            rtnXML.Element("SportApp").Add(xElement);

                        }
                        #endregion

                    // get league thai
                        rtnXML = Command_GetFootballLeague_Sportarena(rtnXML, lang);



                        #region All Country
                        //if (type != "BB")
                        //{
                        dv = FillterCountry(ds.Tables[0].DefaultView, "907,276,380,724,250,764,916", true);
                        xElement = new XElement("Country"
                            , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                            , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                            , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                            , new XAttribute("countryName", (lang == Country.th.ToString()) ? "ประเทศ อื่นๆ" : "All Country")
                            , new XAttribute("countryId", "all")
                            , new XAttribute("id", "all")
                            , new XAttribute("iconFileName16x11", "")
                            , new XAttribute("iconFileName48x48", "")
                            );
                        for (int i = 0; i < dv.Count; i++)
                        {
                            img16 = dv[i]["pic_16x11"].ToString() == "" ? "default.png" : dv[i]["pic_16x11"].ToString();
                            img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                            DataView dvLeague = FillterLeague(dsLeague.Tables[0].DefaultView, "", dv[i]["country_id"].ToString());
                            xElement.Add(new XElement("League"
                            , new XAttribute("iconFileName16x11", img16)
                            , new XAttribute("iconFileName48x48", img48)
                            , new XAttribute("countryName", (lang == Country.th.ToString()) ? dv[i]["ctr_name_local"].ToString() : dv[i]["country_name"].ToString())
                            , new XAttribute("countryId", dv[i]["country_id"].ToString())
                            ));
                        }
                        rtnXML.Element("SportApp").Add(xElement);
                        //}
                        #endregion

                        


                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "error")
                        , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Command_GetFootballLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XDocument Command_GetFootballLeague(XDocument rtnXML, string lang, string type)
        {
            //XDocument rtnXML = new XDocument(new XElement("SportApp",
            //    new XAttribute("header", ConfigurationManager.AppSettings["wordingHeader"])
            //    , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
            //    ));
            try
            {
                DataSet dsLeague =  SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballleague"
                    , new SqlParameter[] { new SqlParameter("@contestGroupId","") });

                if (dsLeague.Tables.Count > 0 && dsLeague.Tables[0].Rows.Count > 0)
                {
                    string img16 = "", img48 = "";
                    #region Get & Add League
                    string eName = (type == "BB") ? "Country" : "TopLeague";
                    XElement xElement = new XElement(eName
                        , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                        , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                        , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                        , new XAttribute("countryName", "")
                        , new XAttribute("countryId", "")
                        );
                    DataView dv = FillterLeague(dsLeague.Tables[0].DefaultView, "21,85,135,116,19,13,14,422,325,807", "");
                    for (int i = 0; i < dv.Count; i++)
                    {
                        img16 = dv[i]["pic_16x11"].ToString() ==""? "default.png" : dv[i]["pic_16x11"].ToString();
                        img48 = dv[i]["pic_48x48"].ToString() ==""? "default48.png" : dv[i]["pic_48x48"].ToString();
                        xElement.Add(new XElement("League",
                            new XAttribute("iconFileName16x11", img16)
                            , new XAttribute("iconFileName48x48", img48)
                            , new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dv[i]["contestgroup_name_th"].ToString() : dv[i]["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dv[i]["contestgroupid"].ToString())
                            ));
                    }
                    rtnXML.Element("SportApp").Add(xElement);
                    #endregion

                    if (type != "androidtablet")
                    {

                        #region StartSoccer โปรแกรมทั้งฤดูกาล
                        //if (type != "BB")
                        //{
                        xElement = new XElement("StarSoccer_Menu"
                            , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                            , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                            , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                            , new XAttribute("countryName", "")
                            , new XAttribute("countryId", "")
                            );
                        dv = FillterLeague(dsLeague.Tables[0].DefaultView, "21,85,135,116,19,13,22,89,817,427,807", "");
                        for (int i = 0; i < dv.Count; i++)
                        {
                            img16 = dv[i]["pic_16x11"].ToString() == "" ? "default.png" : dv[i]["pic_16x11"].ToString();
                            img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                            xElement.Add(new XElement("League",
                                new XAttribute("iconFileName16x11", img16)
                                , new XAttribute("iconFileName48x48", img48)
                                , new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dv[i]["contestgroup_name_th"].ToString() : dv[i]["contestgroup_name"].ToString())
                                , new XAttribute("contestGroupId", dv[i]["contestgroupid"].ToString())
                                ));
                        }
                        rtnXML.Element("SportApp").Add(xElement);
                        //}
                        #endregion

                        #region SportPool Analysis
                        rtnXML.Element("SportApp").Add(Command_GetSportPoolLeague(lang, "", ""));
                        #endregion


                        #region Get&Add Country
                        DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballcountry"
                        , new SqlParameter[] { new SqlParameter("@countryId", "") });
                        dv = FillterCountry(ds.Tables[0].DefaultView, "907,276,380,724,250", false);
                        for (int i = 0; i < dv.Count; i++)
                        {
                            img16 = dv[i]["pic_16x11"].ToString() == "" ? "default.png" : dv[i]["pic_16x11"].ToString();
                            img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                            xElement = new XElement("Country"
                            , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                            , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                            , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                            , new XAttribute("countryName", (lang == Country.th.ToString()) ? dv[i]["ctr_name_local"].ToString() : dv[i]["country_name"].ToString())
                            , new XAttribute("countryId", dv[i]["country_id"].ToString())
                            , new XAttribute("id", dv[i]["country_id"].ToString())
                            , new XAttribute("iconFileName16x11", img16)
                            , new XAttribute("iconFileName48x48", img48)
                            );

                            DataView dvLeague = FillterLeague(dsLeague.Tables[0].DefaultView, "", dv[i]["country_id"].ToString());
                            for (int index = 0; index < dvLeague.Count; index++)
                            {
                                xElement.Add(new XElement("League",
                                    //new XAttribute("iconFileName16x11", dvLeague[index]["pic_16x11"].ToString())
                                    //, new XAttribute("iconFileName48x48", dvLeague[index]["pic_48x48"].ToString())
                                     new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dvLeague[index]["contestgroup_name_th"].ToString() : dvLeague[index]["contestgroup_name"].ToString())
                                    , new XAttribute("contestGroupId", dvLeague[index]["contestgroupid"].ToString())
                                    ));
                            }

                            rtnXML.Element("SportApp").Add(xElement);

                        }
                        #endregion

                        #region All Country
                        //if (type != "BB")
                        //{
                        dv = FillterCountry(ds.Tables[0].DefaultView, "907,276,380,724,250", true);
                        xElement = new XElement("Country"
                            , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                            , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                            , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                            , new XAttribute("countryName", (lang == Country.th.ToString()) ? "ประเทศ อื่นๆ" : "All Country")
                            , new XAttribute("countryId", "all")
                            , new XAttribute("id", "all")
                            , new XAttribute("iconFileName16x11", "")
                            , new XAttribute("iconFileName48x48", "")
                            );
                        for (int i = 0; i < dv.Count; i++)
                        {
                            img16 = dv[i]["pic_16x11"].ToString() == "" ? "default.png" : dv[i]["pic_16x11"].ToString();
                            img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                            DataView dvLeague = FillterLeague(dsLeague.Tables[0].DefaultView, "", dv[i]["country_id"].ToString());
                            xElement.Add(new XElement("League"
                            , new XAttribute("iconFileName16x11", img16)
                            , new XAttribute("iconFileName48x48", img48)
                            , new XAttribute("countryName", (lang == Country.th.ToString()) ? dv[i]["ctr_name_local"].ToString() : dv[i]["country_name"].ToString())
                            , new XAttribute("countryId", dv[i]["country_id"].ToString())
                            ));
                        }
                        rtnXML.Element("SportApp").Add(xElement);
                        //}
                        #endregion

                        rtnXML = Command_GetFootballLeague_SiamSport(rtnXML, lang);
                    }

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "error")
                        , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("Command_GetFootballLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XDocument Command_GetFootballTopLeague(XDocument rtnXML, string lang, string type)
        {
            //XDocument rtnXML = new XDocument(new XElement("SportApp",
            //    new XAttribute("header", ConfigurationManager.AppSettings["wordingHeader"])
            //    , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
            //    ));
            try
            {
                DataSet dsLeague = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballleague"
                    , new SqlParameter[] { new SqlParameter("@contestGroupId", "") });

                if (dsLeague.Tables.Count > 0 && dsLeague.Tables[0].Rows.Count > 0)
                {
                    string img16 = "", img48 = "";
                    #region Get & Add League
                    string eName = "TopLeague";
                    XElement xElement = new XElement(eName
                        , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                        , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                        , new XAttribute("sportType", dsLeague.Tables[0].Rows[0]["sport_type"].ToString())
                        , new XAttribute("countryName", "")
                        , new XAttribute("countryId", "")
                        );
                    DataView dv = FillterLeague(dsLeague.Tables[0].DefaultView, "21,85,135,116,19,13,14,422,325,807", "");
                    for (int i = 0; i < dv.Count; i++)
                    {
                        img16 = dv[i]["pic_16x11"].ToString() == "" ? "default.png" : dv[i]["pic_16x11"].ToString();
                        img48 = dv[i]["pic_48x48"].ToString() == "" ? "default48.png" : dv[i]["pic_48x48"].ToString();
                        xElement.Add(new XElement("League",
                            new XAttribute("iconFileName16x11", img16)
                            , new XAttribute("iconFileName48x48", img48)
                            , new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dv[i]["contestgroup_name_th"].ToString() : dv[i]["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dv[i]["contestgroupid"].ToString())
                            ));
                    }
                    rtnXML.Element("SportApp").Add(xElement);
                    #endregion

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "error")
                        , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Command_GetFootballLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        private XElement Command_GetSportPoolLeague(string lang,string contestGroupId,string countryId)
        {
            try
            {
                string img16 = "", img48 = "";
                DataSet dsLeague = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballanalysis_listleague");
                XElement xElement = new XElement("SportPool_Menu"
                            , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                            , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                            );
                if (dsLeague.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsLeague.Tables[0].Rows)
                    {
                        img16 = dr["pic_16x11"].ToString() == "" ? "default.png" : dr["pic_16x11"].ToString();
                        img48 = dr["pic_48x48"].ToString() == "" ? "default48.png" : dr["pic_48x48"].ToString();
                        xElement.Add(new XElement("League",
                            new XAttribute("iconFileName16x11", img16)
                            , new XAttribute("iconFileName48x48", img48)
                            , new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dr["CLASS_NAME_LOCAL"].ToString() : dr["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dr["contestgroupid"].ToString())
                            ));
                    }
                }
                return xElement;
            }
            catch(Exception ex)
            {
                throw new Exception("Command_GetSportPoolLeague >>" + ex.Message);
            }
        }

        private XDocument Command_GetFootballLeague_Sportarena(XDocument rtnXML, string lang)
        {
            try
            {
                //rtnXML.Element("SportApp").Add(new XElement("SiamSport"));
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballleague_siamsport");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    XElement xElement = null;
                    string sportType = "",eName = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        #region Add XML
                        if (sportType != dr["sport_type"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            eName = dr["sport_type"].ToString() == "00001" ? "Country" : "SportType";
                            if (eName == "Country")
                            {
                                xElement = new XElement(eName
                                            , new XAttribute("sportType", dr["sport_type"].ToString())
                                            , new XAttribute("sportName", (lang == Country.th.ToString()) ? dr["SPORT_NAME_LOCAL"].ToString() : dr["SPORT_NAME"].ToString())
                                            , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                                            , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                                            , new XAttribute("iconFileName16x11", dr["sport_type"].ToString() + ".png")
                                            , new XAttribute("iconFileName48x48", dr["sport_type"].ToString() + ".png")
                                            , new XAttribute("id", dr["scs_id"].ToString() )
                                            , new XAttribute("countryName", (lang == Country.th.ToString()) ? dr["ctr_name_local"].ToString() : dr["ctr_name"].ToString())
                                            , new XAttribute("countryId", dr["ctr_id"].ToString())
                                            );
                            }
                            else
                            {
                                xElement = new XElement(eName
                                            , new XAttribute("sportType", dr["sport_type"].ToString())
                                            , new XAttribute("sportName", (lang == Country.th.ToString()) ? dr["SPORT_NAME_LOCAL"].ToString() : dr["SPORT_NAME"].ToString())
                                            , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                                            , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                                            , new XAttribute("iconFileName16x11", dr["sport_type"].ToString() + ".png")
                                            );
                            }
                        }

                        xElement.Add(new XElement("League"
                            , new XAttribute("iconFileName16x11", dr["scs_id"].ToString() + ".png")
                                , new XAttribute("iconFileName48x48", dr["scs_id"].ToString() + ".png")
                                , new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dr["class_name_local"].ToString() : dr["class_name"].ToString())
                                , new XAttribute("contestGroupId", dr["scs_id"].ToString())
                            ));

                        sportType = dr["sport_type"].ToString();
                        #endregion
                    }
                    rtnXML.Element("SportApp").Add(xElement); // Row สุดท้าย

                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Command_GetFootballLeague_SiamSport >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", "Command_GetFootballLeague_SiamSport>>" + ex.Message));
            }
            return rtnXML;
        }

        private XDocument Command_GetFootballLeague_SiamSport(XDocument rtnXML,string lang)
        {
            try
            {
                rtnXML.Element("SportApp").Add(new XElement("SiamSport"));
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballleague_siamsport");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    XElement xElement = null;
                    string sportType = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        #region Add XML
                        if (sportType != dr["sport_type"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Element("SiamSport").Add(xElement);
                            
                            xElement = new XElement("SportType"
                                        , new XAttribute("sportType", dr["sport_type"].ToString())
                                        , new XAttribute("sportName", (lang == Country.th.ToString()) ? dr["SPORT_NAME_LOCAL"].ToString() : dr["SPORT_NAME"].ToString())
                                        , new XAttribute("iconURL_48x48", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString())
                                        , new XAttribute("iconURL_16x11", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString())
                                        , new XAttribute("iconFileName16x11", dr["sport_type"].ToString()+".png")
                                        );

                        }

                        xElement.Add(new XElement("League"
                            , new XAttribute("iconFileName16x11", dr["scs_id"].ToString() + ".png")
                                , new XAttribute("iconFileName48x48", dr["scs_id"].ToString() + ".png")
                                , new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dr["class_name_local"].ToString() : dr["class_name"].ToString())
                                , new XAttribute("contestGroupId", dr["scs_id"].ToString())
                            ));

                        sportType = dr["sport_type"].ToString();
                        #endregion
                    }
                    rtnXML.Element("SportApp").Element("SiamSport").Add(xElement); // Row สุดท้าย

                }
                
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("Command_GetFootballLeague_SiamSport >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", "Command_GetFootballLeague_SiamSport>>" + ex.Message));
            }
            return rtnXML;
        }
        #endregion
    }
}
