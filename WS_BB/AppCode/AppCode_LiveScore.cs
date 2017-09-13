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

    public class AppCode_LiveScore : AppCode_Base
    {

        #region Database Isport
        public struct Out_LiveScore
        {
            public string status;
            public string errorMess;
            public List<MatchClassLiveScore> liveScore;
        }
        public  class MatchClassLiveScore
        {
            public string mschName;
            public string mschNameEN;
            public string mschID ;
            public string scsDescTH ;
            public string scsDescEN ;

            // ผลการแข่ง
            public string teamName1;
            public string teamName1EN;
            public string teamName2;
            public string teamName2EN;
            public string resultTeam1;
            public string resultTeam2;
            public string match_status;
            public string result;
            public string resultHT; // ผลครึ่งแรก
            public string resultBT; // ครึ่งหลัง
            public string match_detail; // ชื่อผู้เล่น
            public string match_detailEN; // ชื่อผู้เล่น
            public string team_name_local;
            public string team_name;
            public string time_result;
            public string type_detail; // G=glod,Y=Yellwor,R=Red
            public string team_code;
            public string match_date;
            public string detailStatus;
        }

        public static string DateTimeText(string date)
        {
            try
            {
                System.Globalization.CultureInfo thai = new System.Globalization.CultureInfo("th-TH");
                if (date != "")
                {

                    int year = int.Parse(date.Substring(0, 4))+ 543;
                    string month = date.Substring(4, 2);
                    string day = date.Substring(6, 2);
                    string hh = date.Substring(8, 2);
                    string mm = date.Substring(10, 2);
                    return DateTime.ParseExact(year.ToString() + month + day + hh + mm, "yyyyMMddHHmm", thai).ToString("ddd d MMM yyyy HH:mm", thai);
                }
                else
                {
                    return DateTime.ParseExact(date, "yyyyMMdd", thai).ToString("ddd d MMM yyyy", thai);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string DateText(string date)
        {
            try
            {
                System.Globalization.CultureInfo thai = new System.Globalization.CultureInfo("th-TH");
                if (date != "")
                {

                    int year = int.Parse(date.Substring(0, 4))+543;
                    string month = date.Substring(4, 2);
                    string day = date.Substring(6, 2);
                    return DateTime.ParseExact(year.ToString()+month+day, "yyyyMMdd", thai).ToString("ddd d MMM yyyy", thai);
                }
                else
                {
                    return DateTime.ParseExact(date, "yyyyMMdd", thai).ToString("ddd d MMM yyyy", thai);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string DateTimetoText(string date)
        {
            try
            {
                System.Globalization.CultureInfo thai = new System.Globalization.CultureInfo("th-TH");
                return DateTime.ParseExact(date, "yyyyMMddHHmm", thai).ToString("d MMMM yyyy HH:mm", thai);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string DatetoText(string date)
        {
            try
            {
                System.Globalization.CultureInfo thai = new System.Globalization.CultureInfo("th-TH");
                return DateTime.ParseExact(date, "yyyyMMdd", thai).ToString("d MMMM yyyy", thai);
            }
            catch (Exception ex)
            {
                System.Globalization.CultureInfo thai = new System.Globalization.CultureInfo("en-US");
                return DateTime.ParseExact(date, "yyyyMMdd", thai).ToString("d MMMM yyyy", thai);
            }
        }

        public static string DatetoText(string date,string country)
        {
            try
            {
                // ห้ามแก้ format นี้ไปใช้กับ app iSoccer
                string countryInfo = (country == Country.th.ToString()) ? "th-TH" : "en-US";
                System.Globalization.CultureInfo thai = new System.Globalization.CultureInfo(countryInfo);
                return DateTime.ParseExact(date, "yyyyMMdd", thai).ToString("dd/MM/yy", thai);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                string countryInfo =  "en-US";
                System.Globalization.CultureInfo thai = new System.Globalization.CultureInfo(countryInfo);
                return DateTime.ParseExact(date, "yyyyMMdd", thai).ToString("dd/MM", thai);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strMenu"></param>
        /// <param name="scsId"></param>
        /// <param name="isScore">ถ้าเป็นสรุปผล isScore = true </param>
        /// <returns></returns>
        public Out_LiveScore CommandLiveScore(string strMenu,string scsId,bool isScore)
        {
            Out_LiveScore liveScores = new Out_LiveScore();
            List<MatchClassLiveScore> liveScore = new List<MatchClassLiveScore>();
            try
            {
                string timeRef = DateTime.Now.ToString("HH:mm");
                TimeSpan diff24 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 00, 00);
                TimeSpan diff18 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,17, 00, 00);
                string addTime = (diff18.Hours < 0 && diff24.Hours >= 0) ? "0" : "+1";
                addTime = isScore ? "0" : addTime;
                string time = "", classID = "";
                if (strMenu == "WC")
                {
                    time = " 16:00";
                    classID = "256";
                }
                else
                {
                    time = " 17:00";
                    classID = "";
                }
                DataSet ds = SqlHelper.ExecuteDataset(strConn, "usp_BB_football_livescore_select"
                    , new SqlParameter[] { new SqlParameter("@strAdd_time",addTime)
                    ,new SqlParameter("@strTime",time)
                    ,new SqlParameter("@class_id",classID)
                    ,new SqlParameter("@scs_id",scsId)
                    ,new SqlParameter("@sportType","00001")
                    });


                MatchClassLiveScore leageName;
                DataSet matchDS = null;

                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    matchDS = SqlHelper.ExecuteDataset(strConn, "usp_BB_footballlivescore_selectmatchschedule"
                        , new SqlParameter[] { new SqlParameter("@scs_id_in",dr["scs_id"].ToString())
                    ,new SqlParameter("@date_in",dr["match_date"].ToString())
                    ,new SqlParameter("@strlng","L")});
                    DataSet dsResult=null;
                    foreach (DataRow matchDR in matchDS.Tables[0].Rows)
                    {
                        leageName = new MatchClassLiveScore();
                        leageName.mschName = dr["class_name"].ToString();
                        leageName.mschNameEN = dr["class_name_en"].ToString();
                        leageName.match_date = DatetoText(dr["match_date"].ToString());
                        leageName.mschID = matchDR["msch_id"].ToString();
                        //leageName.scsDescTH = matchDR["scs_desc"].ToString();
                        string[] str = matchDR["scs_desc"].ToString().Split('|');
                        if (str.Length == 5)
                        {
                            leageName.match_status = str[0];
                            leageName.teamName1 = str[1] + "(" + matchDR["team_level1"].ToString() + ")";
                            leageName.resultTeam1 = str[2];
                            leageName.teamName2 = str[3] + "(" + matchDR["team_level2"].ToString() + ")"; ;
                            leageName.resultTeam2 = str[4];
                        }
                        str = matchDR["scs_desc_en"].ToString().Split('|');
                        if (str.Length == 5)
                        {
                            leageName.teamName1EN = str[1] + "(" + matchDR["team_level1"].ToString() + ")";
                            leageName.teamName2EN = str[3] + "(" + matchDR["team_level2"].ToString() + ")"; ;
                        }

                        // Check ว่ามี detail หรือไม่
                        dsResult = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_livescore_result"
                    , new SqlParameter[] { new SqlParameter("@msch_id", matchDR["msch_id"].ToString()) });
                        leageName.detailStatus = (dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0) ? "Y" : "N";

                        liveScore.Add(leageName);
                    }
                    
                }
            }
            catch(Exception ex)
            {
                liveScores.status = "Error";
                liveScores.errorMess = "GetLiveScore >> " + ex.Message;
                ExceptionManager.WriteError("GetLiveScore >> " + ex.Message);
            }
            liveScores.liveScore = liveScore;
            return liveScores;
        }


        /// <summary>
        /// CommandLiveScore 
        /// </summary>
        /// <param name="strMenu"></param>
        /// <param name="scsId"></param>
        /// <param name="isScore">ถ้าเป็นสรุปผล isScore = true </param>
        /// <returns></returns>
        public XElement CommandLiveScore(string elementName,string scsId,string sportType, bool isScore,string lang)
        {
            XElement element = new XElement(elementName);
            try
            {
                #region Set date
                string timeRef = DateTime.Now.ToString("HH:mm");
                TimeSpan diff24 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 00, 00);
                TimeSpan diff18 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 00, 00);
                string addTime = (diff18.Hours < 0 && diff24.Hours >= 0) ? "0" : "+1";
                addTime = isScore ? "0" : addTime;
                string time = "", classID = "";
                #endregion

                #region Get Header
                DataSet ds = SqlHelper.ExecuteDataset(strConn, "usp_BB_football_livescore_select"
                    , new SqlParameter[] { new SqlParameter("@strAdd_time",addTime)
                    ,new SqlParameter("@strTime",time)
                    ,new SqlParameter("@class_id",classID)
                    ,new SqlParameter("@scs_id",scsId)
                    ,new SqlParameter("@sportType",sportType)
                    });
                #endregion

                DataSet matchDS = null;
                XElement xElement = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    #region Add Header
                    xElement = new XElement(new XElement("LeagueScore"
                        , new XAttribute("date", DatetoText(dr["match_date"].ToString(), lang))
                        , new XAttribute("scs_id", dr["scs_id"].ToString())
                        , new XAttribute("class_name", (lang == Country.th.ToString()) ? dr["class_name"].ToString() : dr["class_name_en"].ToString())
                        ));
                    #endregion

                    #region Get Detail
                    matchDS = SqlHelper.ExecuteDataset(strConn, "usp_BB_footballlivescore_selectmatchschedule"
                        , new SqlParameter[] { new SqlParameter("@scs_id_in",dr["scs_id"].ToString())
                    ,new SqlParameter("@date_in",dr["match_date"].ToString())
                    ,new SqlParameter("@strlng","L")});
                    #endregion

                    string match_status = "", teamName1 = "", resultTeam1 = "", teamName2 = "", resultTeam2 = "", teamName1EN = "", teamName2EN = "";
                    foreach (DataRow matchDR in matchDS.Tables[0].Rows)
                    {
                        #region Add Detail
                        string[] str = matchDR["scs_desc"].ToString().Split('|');
                        if (str.Length == 5)
                        {
                            match_status = str[0];
                            teamName1 = str[1] + "(" + matchDR["team_level1"].ToString() + ")";
                            resultTeam1 = str[2];
                            teamName2 = str[3] + "(" + matchDR["team_level2"].ToString() + ")"; ;
                            resultTeam2 = str[4];
                        }
                        str = matchDR["scs_desc_en"].ToString().Split('|');
                        if (str.Length == 5)
                        {
                            teamName1EN = str[1] + "(" + matchDR["team_level1"].ToString() + ")";
                            teamName2EN = str[3] + "(" + matchDR["team_level2"].ToString() + ")"; ;
                        }
                        DataSet dsResult = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_livescore_result"
                    , new SqlParameter[] { new SqlParameter("@msch_id", matchDR["msch_id"].ToString()) });

                        xElement.Add(new XElement("MatchScore"
                            , new XAttribute("msch_id", matchDR["msch_id"].ToString())
                            , new XAttribute("match_status", match_status)
                            , new XAttribute("teamName1", (lang == Country.th.ToString()) ? teamName1 : teamName1EN)
                            , new XAttribute("resultTeam1", resultTeam1)
                            , new XAttribute("teamName2", (lang == Country.th.ToString()) ? teamName2 : teamName2EN)
                            , new XAttribute("resultTeam2", resultTeam2)
                            , new XAttribute("isDetail", (dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0) ? "Y" : "N")
                            ));
                        #endregion
                    }

                    element.Add(xElement);
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandLiveScore XDoc >> " + ex.Message);
               // rtnXML.Element("SportApp").Add(new XElement("status", "error")
               //     , new XElement("message", ex.Message));
            }
            return element;
        }

        /// <summary>
        /// CommandLiveScore 
        /// </summary>
        /// <param name="strMenu"></param>
        /// <param name="scsId"></param>
        /// <param name="isScore">ถ้าเป็นสรุปผล isScore = true </param>
        /// <returns></returns>
        public XDocument CommandLiveScore_IStore(XDocument rtnXML, string scsId, string sportType, bool isScore, string lang)
        {
            try
            {
                #region Set date
                string timeRef = DateTime.Now.ToString("HH:mm");
                TimeSpan diff24 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 00, 00);
                TimeSpan diff18 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 00, 00);
                string addTime = (diff18.Hours < 0 && diff24.Hours >= 0) ? "0" : "+1";
                addTime = isScore ? "0" : addTime;
                string time = "", classID = "";
                #endregion

                #region Get Header
                DataSet ds = SqlHelper.ExecuteDataset(strConn, "usp_BB_football_livescore_select"
                    , new SqlParameter[] { new SqlParameter("@strAdd_time",addTime)
                    ,new SqlParameter("@strTime",time)
                    ,new SqlParameter("@class_id",classID)
                    ,new SqlParameter("@scs_id",scsId)
                    ,new SqlParameter("@sportType",sportType)
                    });
                #endregion

                DataSet matchDS = null;
                XElement xElement = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    #region Add Header
                    xElement = new XElement(new XElement("League"
                        , new XAttribute("date", DatetoText(dr["match_date"].ToString(), lang))
                        , new XAttribute("contestGroupId", dr["scs_id"].ToString())
                        , new XAttribute("tmName", (lang == Country.th.ToString()) ? dr["class_name"].ToString() : dr["class_name_en"].ToString())
                        , new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dr["class_name"].ToString() : dr["class_name_en"].ToString())
                        , new XAttribute("contestURLImages", "")
                        ,new XAttribute("tmSystem","league")
                        ));
                    #endregion

                    #region Get Detail
                    matchDS = SqlHelper.ExecuteDataset(strConn, "usp_BB_footballlivescore_selectmatchschedule"
                        , new SqlParameter[] { new SqlParameter("@scs_id_in",dr["scs_id"].ToString())
                    ,new SqlParameter("@date_in",dr["match_date"].ToString())
                    ,new SqlParameter("@strlng","L")});
                    #endregion

                    string match_status = "", teamName1 = "", resultTeam1 = "", teamName2 = "", resultTeam2 = "", teamName1EN = "", teamName2EN = "";
                    foreach (DataRow matchDR in matchDS.Tables[0].Rows)
                    {
                        #region Add Detail
                        string[] str = matchDR["scs_desc"].ToString().Split('|');
                        if (str.Length == 5)
                        {
                            match_status = str[0];
                            teamName1 = str[1] + "(" + matchDR["team_level1"].ToString() + ")";
                            resultTeam1 = str[2];
                            teamName2 = str[3] + "(" + matchDR["team_level2"].ToString() + ")"; ;
                            resultTeam2 = str[4];
                        }
                        str = matchDR["scs_desc_en"].ToString().Split('|');
                        if (str.Length == 5)
                        {
                            teamName1EN = str[1] + "(" + matchDR["team_level1"].ToString() + ")";
                            teamName2EN = str[3] + "(" + matchDR["team_level2"].ToString() + ")"; ;
                        }
                        DataSet dsResult = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_livescore_result"
                    , new SqlParameter[] { new SqlParameter("@msch_id", matchDR["msch_id"].ToString()) });

                        xElement.Add(new XElement("Score"
                            , new XAttribute("mschId", matchDR["msch_id"].ToString())
                            , new XAttribute("match_status", match_status)
                            , new XAttribute("teamName1", (lang == Country.th.ToString()) ? teamName1 : teamName1EN)
                            , new XAttribute("resultTeam1", resultTeam1)
                            , new XAttribute("teamName2", (lang == Country.th.ToString()) ? teamName2 : teamName2EN)
                            , new XAttribute("resultTeam2", resultTeam2)
                            , new XAttribute("isDetail", (dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0) ? "true" : "false")
                            ));
                        #endregion
                    }

                    rtnXML.Element("SportApp").Add(xElement);
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandLiveScore XDoc >> " + ex.Message);
                // rtnXML.Element("SportApp").Add(new XElement("status", "error")
                //     , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// CommandLiveScore_ ใช้สำหรับ application แทนข้อมูล sportcc
        /// </summary>
        /// <param name="strMenu"></param>
        /// <param name="scsId"></param>
        /// <param name="isScore">ถ้าเป็นสรุปผล isScore = true </param>
        /// <returns></returns>
        public XDocument CommandLiveScore_(XDocument rtnXML, string scsId, string sportType, bool isScore, string lang)
        {
            try
            {
                #region Set date
                string timeRef = DateTime.Now.ToString("HH:mm");
                TimeSpan diff24 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 00, 00);
                TimeSpan diff18 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 00, 00);
                string addTime = (diff18.Hours < 0 && diff24.Hours >= 0) ? "0" : "+1";
                addTime = isScore ? "0" : addTime;
                string time = "", classID = "";
                #endregion

                #region Get Header
                DataSet ds = SqlHelper.ExecuteDataset(strConn, "usp_BB_football_livescore_select"
                    , new SqlParameter[] { new SqlParameter("@strAdd_time",addTime)
                    ,new SqlParameter("@strTime",time)
                    ,new SqlParameter("@class_id",classID)
                    ,new SqlParameter("@scs_id",scsId)
                    ,new SqlParameter("@sportType",sportType)
                    });
                #endregion

                DataSet matchDS = null;
                XElement xElement = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    #region Add Header
                    xElement = new XElement(new XElement("League"
                        , new XAttribute("date", DatetoText(dr["match_date"].ToString(), lang))
                        , new XAttribute("contestGroupId", dr["scs_id"].ToString())
                        , new XAttribute("tmName", (lang == Country.th.ToString()) ? dr["class_name"].ToString() : dr["class_name_en"].ToString())
                        , new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dr["class_name"].ToString() : dr["class_name_en"].ToString())
                        , new XAttribute("contestURLImages", "")
                        , new XAttribute("tmSystem", "league")
                        ));
                    #endregion

                    #region Get Detail
                    matchDS = SqlHelper.ExecuteDataset(strConn, "usp_BB_footballlivescore_selectmatchschedule"
                        , new SqlParameter[] { new SqlParameter("@scs_id_in",dr["scs_id"].ToString())
                    ,new SqlParameter("@date_in",dr["match_date"].ToString())
                    ,new SqlParameter("@strlng","L")});
                    #endregion

                    string match_status = "", teamName1 = "", resultTeam1 = "", teamName2 = "", resultTeam2 = "", teamName1EN = "", teamName2EN = "";
                    foreach (DataRow matchDR in matchDS.Tables[0].Rows)
                    {
                        #region Add Detail
                        string[] str = matchDR["scs_desc"].ToString().Split('|');
                        if (str.Length == 5)
                        {
                            match_status = str[0];
                            teamName1 = str[1] + "(" + matchDR["team_level1"].ToString() + ")";
                            resultTeam1 = str[2];
                            teamName2 = str[3] + "(" + matchDR["team_level2"].ToString() + ")"; ;
                            resultTeam2 = str[4];
                        }
                        str = matchDR["scs_desc_en"].ToString().Split('|');
                        if (str.Length == 5)
                        {
                            teamName1EN = str[1] + "(" + matchDR["team_level1"].ToString() + ")";
                            teamName2EN = str[3] + "(" + matchDR["team_level2"].ToString() + ")"; ;
                        }
                        DataSet dsResult = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_livescore_result"
                    , new SqlParameter[] { new SqlParameter("@msch_id", matchDR["msch_id"].ToString()) });

                        xElement.Add(new XElement("Score"
                            , new XAttribute("contestGroupId", "")
                            , new XAttribute("matchId", "")
                            , new XAttribute("score_home_ht", "")
                            , new XAttribute("score_away_ht", "")
                            , new XAttribute("teamCode1", "")
                            , new XAttribute("teamCode2", "")
                             , new XAttribute("matchDate", DatetoText(dr["match_date"].ToString(), lang))
                            , new XAttribute("mschId", matchDR["msch_id"].ToString())
                            , new XAttribute("status", match_status)
                            , new XAttribute("minutes", "FT")
                            , new XAttribute("curent_period", "Finished")
                            , new XAttribute("teamName1", (lang == Country.th.ToString()) ? teamName1 : teamName1EN)
                            , new XAttribute("score_home", resultTeam1)
                            , new XAttribute("teamName2", (lang == Country.th.ToString()) ? teamName2 : teamName2EN)
                            , new XAttribute("score_away", resultTeam2)
                            , new XAttribute("isDetail", (dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0) ? "true" : "false")
                            ));
                        #endregion
                    }

                    rtnXML.Element("SportApp").Add(xElement);
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandLiveScore XDoc >> " + ex.Message);
                // rtnXML.Element("SportApp").Add(new XElement("status", "error")
                //     , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// Get ประตู ใบเหลือง ใบแดง
        /// </summary>
        /// <param name="mschId"></param>
        /// <returns></returns>
        public Out_LiveScore CommandLiveScoreResult(string mschId)
        {
            Out_LiveScore outResult = new Out_LiveScore();
            List<MatchClassLiveScore> matchResults = new List<MatchClassLiveScore>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_livescore_selectbymschid"
                    , new SqlParameter[] { new SqlParameter("@msch_id", mschId) });

                foreach( DataRow dr in ds.Tables[0].Rows )
                {

                    DataSet dsResult = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_livescore_result"
                    , new SqlParameter[] { new SqlParameter("@msch_id",mschId)});
                    foreach(DataRow drResult in dsResult.Tables[0].Rows)
                    {
                        MatchClassLiveScore match = new MatchClassLiveScore();
                        match.mschID = dr["msch_id"].ToString();
                        match.mschName = dr["CLASS_NAME_LOCAL"].ToString();
                        match.mschNameEN = dr["CLASS_NAME"].ToString();
                        match.scsDescTH = dr["scs_desc"].ToString();
                        match.resultHT = "ครึ่งแรก "+dr["resultHT"].ToString();
                        match.resultBT = "ครึ่งหลัง "+dr["resultBT1"].ToString() +"-"+ dr["resultBT2"].ToString();
                        match.teamName1 = dr["teamCode1TH"].ToString();
                        match.teamName2 = dr["teamCode2TH"].ToString();
                        match.teamName1EN = dr["teamCode1EN"].ToString();
                        match.teamName2EN = dr["teamCode2EN"].ToString();
                        match.match_detail = drResult["MATCH_DETAIL"].ToString();
                        match.match_detailEN = drResult["MATCH_DETAIL_EN"].ToString();
                        match.team_name_local = drResult["TEAM_NAME_LOCAL"].ToString();
                        match.team_name = drResult["TEAM_NAME"].ToString();
                        match.time_result = drResult["TIME_RESULT"].ToString();
                        match.type_detail = drResult["TYPE_DETAIL"].ToString();
                        match.team_code = drResult["team_code"].ToString();
                        match.result = dr["scs_desc"].ToString();
                        matchResults.Add(match);
                    }
                }
                
            }
            catch(Exception ex)
            {
                outResult.status = "Error";
                outResult.errorMess = ex.Message;
                ExceptionManager.WriteError("CommandLiveScoreResult >>" + ex.Message);
            }
            outResult.liveScore = matchResults;
            return outResult;
        }

        /// <summary>
        /// Get ประตู ใบเหลือง ใบแดง
        /// </summary>
        /// <param name="mschId"></param>
        /// <returns></returns>
        public XDocument CommandScoreResult(string mschId)
        {
            XDocument rtnXML = new XDocument(new XElement("SportPhone",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                , new XAttribute("smsMessage", ConfigurationManager.AppSettings["wordingSMS"])
                ));
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_livescore_selectbymschid"
                    , new SqlParameter[] { new SqlParameter("@msch_id", mschId) });

                XElement xElement = new XElement("Score"
                    , new XAttribute("contestGroupName", ds.Tables[0].Rows[0]["CLASS_NAME_LOCAL"].ToString())
                    , new XAttribute("tmName", ds.Tables[0].Rows[0]["CLASS_NAME"].ToString())
                    , new XAttribute("teamName1", ds.Tables[0].Rows[0]["teamCode1TH"].ToString())
                    , new XAttribute("teamName2", ds.Tables[0].Rows[0]["teamCode2TH"].ToString())
                    //, new XAttribute("teamName2", ds.Tables[0].Rows[0]["teamCode2TH"].ToString())
                    , new XAttribute("scoreHome", ds.Tables[0].Rows[0]["result_team_code1"].ToString())
                    , new XAttribute("scoreAway", ds.Tables[0].Rows[0]["result_team_code2"].ToString())); 
                    
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    DataSet dsResult = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_livescore_result"
                    , new SqlParameter[] { new SqlParameter("@msch_id", mschId) });
                    foreach (DataRow drResult in dsResult.Tables[0].Rows)
                    {
                        xElement.Add(new XElement("ScoreDetail"
                            , new XAttribute("playerName", drResult["MATCH_DETAIL"].ToString())
                            , new XAttribute("minute", drResult["TIME_RESULT"].ToString())
                            , new XAttribute("teamName", drResult["TEAM_NAME"].ToString())
                            , new XAttribute("evenType", drResult["TYPE_DETAIL"].ToString())
                            ));
                    }
                }
                rtnXML.Element("SportPhone").Add(xElement);
            }
            catch (Exception ex)
            {
               
                ExceptionManager.WriteError("CommandLiveScoreResult >>" + ex.Message);
            }

            return rtnXML;
        }

        /// <summary>
        /// CommandGetScoreResult
        /// </summary>
        /// <param name="rtnXML"></param>
        /// <param name="scsId"></param>
        /// <param name="date"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetScoreResult(XDocument rtnXML, string scsId, string date, string lang)
        {
            XElement element = null;
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_selectresult"
                    , new SqlParameter[] { new SqlParameter("@scs_id_in",scsId)
                                                        ,new SqlParameter("@date_in",date)
                                                        ,new SqlParameter("@strlng",lang)});

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string scs = "", img16 = "";//, teamName1 = "", teamName2="";
                    bool isDetail = false;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        #region Add XML
                        if (scs != dr["scs_id"].ToString())
                        {
                            if (element != null) rtnXML.Element("SportApp").Add(element);
                            img16 = dr["PIC_16X11"].ToString() == "" ? "default.png" : dr["PIC_16X11"].ToString();
                            element = new XElement("League",
                             new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dr["scs_desc_local"].ToString() == "" ? dr["scs_desc"].ToString() : dr["scs_desc_local"].ToString()) : dr["scs_desc"].ToString())
                            , new XAttribute("contestGroupId", dr["scs_id"].ToString())
                            , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                            , new XAttribute("tmName", (lang == Country.th.ToString()) ? dr["class_name_local"].ToString() : dr["class_name"].ToString())
                            , new XAttribute("tmSystem", dr["class_name"].ToString()));

                        }
                        //teamName1 = (lang == Country.th.ToString()) ? (dr["isportteamname1"].ToString() == "" ? dr["team_name1"].ToString() : dr["isportteamname1"].ToString()) : dr["team_name1"].ToString();
                        //teamName2 = (lang == Country.th.ToString()) ? (dr["isportteamname2"].ToString() == "" ? dr["team_name2"].ToString() : dr["isportteamname2"].ToString()) : dr["team_name2"].ToString();
                        isDetail = false;//int.Parse(dr["countcard"].ToString()) > 0 || int.Parse(dr["countscore"].ToString()) > 0 ? true : false;
                        element.Add(new XElement("Score"
                           , new XAttribute("teamCode1", dr["team_code1"].ToString())
                           , new XAttribute("teamCode2", dr["team_code2"].ToString())
                           , new XAttribute("teamName1", dr["teamname1_th"] +"("+ dr["team_level1"].ToString()+")")
                           , new XAttribute("teamName2", dr["teamname2_th"] +"("+ dr["team_level2"].ToString()+")")
                           , new XAttribute("status", dr["result_time"].ToString())
                           , new XAttribute("minutes", dr["result_time"].ToString() == "FT" ? dr["result_time"].ToString()  : dr["result_time_minute"].ToString())
                           , new XAttribute("curent_period", MatchType.Finished.ToString())
                           , new XAttribute("score_home", dr["result_team_code1"].ToString() == "" ? "0" : dr["result_team_code1"].ToString())
                           , new XAttribute("score_away", dr["result_team_code2"].ToString() == "" ? "0" : dr["result_team_code2"].ToString())
                           , new XAttribute("score_home_ht", dr["result_team_code1_ht"].ToString() == "" ? dr["result_team_code1"].ToString() : dr["result_team_code1_ht"].ToString())
                           , new XAttribute("score_away_ht", dr["result_team_code2_ht"].ToString() == "" ? dr["result_team_code2"].ToString() : dr["result_team_code2_ht"].ToString())
                           , new XAttribute("matchId", "")
                           , new XAttribute("mschId", dr["msch_id"].ToString())
                           , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(date, lang))
                           , new XAttribute("contestGroupId", scsId)
                           , new XAttribute("isDetail", isDetail)
                           ));
                        scs = dr["scs_id"].ToString();

                        #endregion
                    }

                    if (element != null) rtnXML.Element("SportApp").Add(element); // add Element สุดท้าย
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return rtnXML;
        }

        /// <summary>
        /// CommandgetMatchScheduleByResultType
        /// </summary>
        /// <param name="xDoc"></param>
        /// <param name="eleName"></param>
        /// <param name="resultType"></param>
        /// <param name="teamCode"></param>
        /// <returns></returns>
        public XDocument CommandgetMatchScheduleByResultType(XDocument xDoc ,string eleName,string resultType , string teamCode)
        {
            string status = "Success", desc = "";
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_GetMatchSchduleByResultType"
                    , new SqlParameter[] { new SqlParameter("@resultType",resultType) 
                                                        ,new SqlParameter("@strTeamCode",teamCode)
                                                    });
                XElement element = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (resultType == "Result")
                    {
                         element = new XElement("Match", new XAttribute("msch_id", dr["msch_id"].ToString())
                                                                                                    , new XAttribute("match_date", AppCode_LiveScore.DateTimeText(dr["match_date"].ToString()))
                                                                                                    , new XAttribute("match_desc", dr["pcnt_detail"].ToString())
                                                                                                    , new XAttribute("season_name", dr["class_name_local"].ToString())
                                                                                                    );

                        DataSet dsResult = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_livescore_result"
                        , new SqlParameter[] { new SqlParameter("@msch_id", dr["msch_id"].ToString()) });
                        foreach (DataRow drResult in dsResult.Tables[0].Rows)
                        {
                            element.Add(new XElement("MatchDetail"
                                , new XAttribute("playerName", drResult["MATCH_DETAIL"].ToString())
                                , new XAttribute("minute", drResult["TIME_RESULT"].ToString())
                                , new XAttribute("teamName", drResult["TEAM_NAME"].ToString())
                                , new XAttribute("evenType", AppCode_FootballAnalysis.ConvertEventType(drResult["TYPE_DETAIL"].ToString()))
                                ));
                        }
                    }
                    else
                    {
                        // Program
                         element = new XElement("Match", new XAttribute("msch_id", dr["msch_id"].ToString())
                                                                                                    , new XAttribute("match_date", AppCode_LiveScore.DateTimeText(dr["match_date"].ToString()))
                                                                                                    , new XAttribute("match_team1", dr["teamname1"].ToString())
                                                                                                    , new XAttribute("match_team2", dr["teamname2"].ToString())
                                                                                                    ,new XAttribute("season_name",dr["class_name_local"].ToString())
                                                                                                    );

                    }

                    xDoc.Element(eleName).Add(element);
                }
            }
            catch(Exception ex)
            {
                status = "Error";
                desc = ex.Message;
                ExceptionManager.WriteError("CommandgetMatchScheduleByResultType  >>" + ex.Message);
            }
            xDoc.Element(eleName).Add(new XElement("Status", status)
                                                           , new XElement("Description", desc));
            return xDoc;
        }
        #endregion

        #region Database IsportFeed

        /// <summary>
        /// Gen Livescore สำหรับ gen file score detail
        /// </summary>
        /// <returns></returns>
        public DataSet GetScore()
        {
            DataSet ds = new DataSet();
            string timeRef = DateTime.Now.ToString("HH:mm");
            TimeSpan diff24 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 00, 00);
            TimeSpan diff18 = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 00, 00);
            decimal addTime = (diff18.Hours < 0 && diff24.Hours >= 0) ? 0 : +1; // เวลาปัจจุบันเลย 18.00 ถ้าเลย Day + 1 

            ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballscore"
                           , new SqlParameter[] {new SqlParameter("@dateadd",addTime)
                    ,new SqlParameter("@byweek","N")
                    ,new SqlParameter("@status","inprogress")
                    ,new SqlParameter("@contestGroupId","")
                    });

            //,new SqlParameter("@status","inprogress")
            return ds;
        }

        public enum MatchType
        {
            Finished
            ,
            inprogress
                ,
            Abandoned
                ,
            NSY
                , Postponed
        }

        /// <summary>
        /// FillterGetScore
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="contestGroupId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        private DataView FillterGetScore(DataView dv,string contestGroupId,string countryId)
        {
            try
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
                if (contestGroupId != "")
                {
                    str.Append(" contestgroupid in (" + contestGroupId + ")");
                }
                else if (countryId != "")
                {
                    str.Append(" country_id in (" + countryId + ")");
                }
                dv.RowFilter = str.ToString();
                return dv;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// CommandGetScore
        /// </summary>
        /// <param name="matchType"></param>
        /// <param name="addDate"></param>
        /// <param name="isByWeek"></param>
        /// <returns></returns>
        public string CommandGetScore_test(XDocument rtnXML, string countryId, string lang, string contestGroupId, MatchType matchType, decimal addDate, string isByWeek)
        {

                return new push().SendGet("http://203.149.62.184/genfile/Isport_GetScore");
               
        }

        /// <summary>
        /// CommandGetScore
        /// </summary>
        /// <param name="matchType"></param>
        /// <param name="addDate"></param>
        /// <param name="isByWeek"></param>
        /// <returns></returns>
        public XDocument CommandGetScore(XDocument rtnXML, string countryId, string lang, string contestGroupId, MatchType matchType, decimal addDate, string isByWeek)
        {
            try
            {
                DataSet ds = null;
                string teamName1 = "", teamName2 = "";
                bool isDetail = false;
                if (contestGroupId == "")
                {
                    using (SqlConnection sqlConn = new SqlConnection(strConnFeed))
                    {
                        if (sqlConn.State == ConnectionState.Closed) sqlConn.Open();
                        SqlCommand cmd = new SqlCommand("usp_sportcc_getfootballscorebycountryId", sqlConn);
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@dateadd", addDate);
                        cmd.Parameters.AddWithValue("@byweek", isByWeek);
                        cmd.Parameters.AddWithValue("@status", matchType.ToString());
                        cmd.Parameters.AddWithValue("@countryId", countryId.IndexOf(",") > 0 ? "" : countryId);
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        ds = new DataSet();
                        da.Fill(ds);
                    }

                        // Get by country
                        /*ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballscorebycountryId"
                        , new SqlParameter[] {new SqlParameter("@dateadd",addDate)
                    ,new SqlParameter("@byweek",isByWeek)
                    ,new SqlParameter("@status",matchType.ToString())
                    ,new SqlParameter("@countryId",countryId.IndexOf(",")>0?"":countryId)
                    });*/
                }
                else
                {
                    // Get by contestGroupId
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballscore"
                       , new SqlParameter[] {new SqlParameter("@dateadd",addDate)
                    ,new SqlParameter("@byweek",isByWeek)
                    ,new SqlParameter("@status",matchType.ToString())
                    ,new SqlParameter("@contestGroupId",contestGroupId.IndexOf(",")>0?"":contestGroupId)
                });
                }

                
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = FillterGetScore(ds.Tables[0].DefaultView, contestGroupId, countryId);
                    string contentGroupid = "",img16="";
                    XElement xElement = null;
                    int countLeague = 0;
                    if (dv.Count > 0)
                    {
                        for (int index = 0; index < dv.Count && countLeague < 50 ; index++)
                        {
                            #region Add XML
                            if (contentGroupid != dv[index]["tm_id"].ToString()) // ต้องเปลี่ยน TM_ID เพราะ contentGroupID thai league มันซ้ำกัน
							{
                                countLeague++;
                                if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                                img16 = dv[index]["PIC_48X48"].ToString() == "" ? "default.png" : dv[index]["PIC_48X48"].ToString();
                                xElement = new XElement("League",
                                 new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dv[index]["class_name_local"].ToString() == "" ? dv[index]["contestgroup_name"].ToString() : dv[index]["class_name_local"].ToString()) : dv[index]["contestgroup_name"].ToString())
                                , new XAttribute("contestGroupId", dv[index]["contestgroupid"].ToString())
                                , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString() + img16)
                                , new XAttribute("tmName", (lang == Country.th.ToString()) ? dv[index]["tm_name"].ToString() : dv[index]["tm_name"].ToString())
                                , new XAttribute("tmSystem", dv[index]["tm_system"].ToString()));

                            }
                            teamName1 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname1"].ToString() == "" ? dv[index]["team_name1"].ToString() : dv[index]["isportteamname1"].ToString()) : dv[index]["team_name1"].ToString();
                            teamName2 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname2"].ToString() == "" ? dv[index]["team_name2"].ToString() : dv[index]["isportteamname2"].ToString()) : dv[index]["team_name2"].ToString();
                            isDetail = int.Parse(dv[index]["countcard"].ToString()) > 0 || int.Parse(dv[index]["countscore"].ToString()) > 0 ? true : false;
                            string minutes = dv[index]["curent_period"].ToString() == MatchType.Finished.ToString() ? "FT" :  dv[index]["minutes"].ToString();
                            minutes = dv[index]["curent_period"].ToString() == MatchType.Postponed.ToString() ? "Post." :  minutes;
                            xElement.Add(new XElement("Score"
                               , new XAttribute("teamCode1", dv[index]["team_id1"].ToString())
                               , new XAttribute("teamCode2", dv[index]["team_id2"].ToString())
                               , new XAttribute("teamName1", teamName1.Replace("?", "") + dv[index]["placeteam1"].ToString())
                               , new XAttribute("teamName2", teamName2.Replace("?", "") + dv[index]["placeteam2"].ToString())
                               , new XAttribute("status", dv[index]["status"].ToString())
                               , new XAttribute("minutes", minutes)
                               , new XAttribute("curent_period", dv[index]["curent_period"].ToString())
                               , new XAttribute("score_home", dv[index]["score_home"].ToString() == "" ? "0" : dv[index]["score_home"].ToString())
                               , new XAttribute("score_away", dv[index]["score_away"].ToString() == "" ? "0" : dv[index]["score_away"].ToString())
                               , new XAttribute("score_home_ht", dv[index]["score_home_ht"].ToString() == "" ? dv[index]["score_home"].ToString() : dv[index]["score_home_ht"].ToString())
                               , new XAttribute("score_away_ht", dv[index]["score_away_ht"].ToString() == "" ? dv[index]["score_away"].ToString() : dv[index]["score_away_ht"].ToString())
                               , new XAttribute("matchId", dv[index]["match_id"].ToString())
                               , new XAttribute("mschId", dv[index]["msch_id"].ToString())
                               , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dv[index]["matchdate"].ToString(), lang))
                               , new XAttribute("contestGroupId", dv[index]["contestgroupid"].ToString())
                               , new XAttribute("isDetail", isDetail)
                               ));
                            contentGroupid = dv[index]["tm_id"].ToString(); // ต้องเปลี่ยน TM_ID เพราะ contentGroupID thai league มันซ้ำกัน

                            #endregion
                        }

                        if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย

                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                        , new XElement("message", ""));
                    }
                    else
                    {
                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                        , new XElement("message", (matchType == MatchType.inprogress) ? "ขณะนี้ไม่มีการแข่งขัน" : "ไม่พบข้อมูลผลการแข่งขัน"));
                    }

                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                        , new XElement("message", (matchType == MatchType.inprogress) ? "ขณะนี้ไม่มีการแข่งขัน" : "ไม่พบข้อมูลผลการแข่งขัน"));
                    //CommandGetScore(rtnXML, "", lang, "", AppCode_LiveScore.MatchType.Finished, addDate, "N");
                }

            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScore >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// CommandGetScoreiSoccerGame
        /// </summary>
        /// <param name="matchType"></param>
        /// <param name="addDate"></param>
        /// <param name="isByWeek"></param>
        /// <returns></returns>
        public XDocument CommandGetScoreiSoccerGame(XDocument rtnXML, string countryId, string lang, string contestGroupId, MatchType matchType, decimal addDate, string isByWeek,string msisdn,string imei)
        {
            try
            {
                DataSet ds = null;
                string teamName1 = "", teamName2 = "";
                bool isDetail = false;
                if (contestGroupId == "")
                {
                    // Get by country
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballscorebycountryId"
                    , new SqlParameter[] {new SqlParameter("@dateadd",addDate)
                    ,new SqlParameter("@byweek",isByWeek)
                    ,new SqlParameter("@status",matchType.ToString())
                    ,new SqlParameter("@countryId",countryId.IndexOf(",")>0?"":countryId)
                });
                }
                else
                {
                    // Get by contestGroupId
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballscore"
                       , new SqlParameter[] {new SqlParameter("@dateadd",addDate)
                    ,new SqlParameter("@byweek",isByWeek)
                    ,new SqlParameter("@status",matchType.ToString())
                    ,new SqlParameter("@contestGroupId",contestGroupId.IndexOf(",")>0?"":contestGroupId)
                });
                }


                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = FillterGetScore(ds.Tables[0].DefaultView, contestGroupId, countryId);
                    DataSet dsAnwser = new DataSet();
                    string contentGroupid = "", img16 = "", strAnswer="n";
                    XElement xElement = null;
                    if (dv.Count > 0)
                    {
                        for (int index = 0; index < dv.Count; index++)
                        {
                            #region Add XML
                            if (contentGroupid != dv[index]["tm_id"].ToString()) // ต้องเปลี่ยน TM_ID เพราะ contentGroupID thai league มันซ้ำกัน
							{
                                if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                                img16 = dv[index]["PIC_48X48"].ToString() == "" ? "default.png" : dv[index]["PIC_48X48"].ToString();
                                xElement = new XElement("League",
                                 new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dv[index]["class_name_local"].ToString() == "" ? dv[index]["contestgroup_name"].ToString() : dv[index]["class_name_local"].ToString()) : dv[index]["contestgroup_name"].ToString())
                                , new XAttribute("contestGroupId", dv[index]["contestgroupid"].ToString())
                                , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString() + img16)
                                , new XAttribute("tmName", (lang == Country.th.ToString()) ? dv[index]["tm_name"].ToString() : dv[index]["tm_name"].ToString())
                                , new XAttribute("tmSystem", dv[index]["tm_system"].ToString()));

                                try {
                                    dsAnwser = AppCode_iSoccer.GetGameAnswerResult(msisdn, imei, AppCode_LiveScore.DatetoText(dv[index]["matchdate"].ToString()));
                                }
                                catch(Exception ex)
                                {
                                    ExceptionManager.WriteError(ex.Message);
                                }

                            }

                            if (dsAnwser.Tables.Count > 0 && dsAnwser.Tables[0].Rows.Count > 0)
                            {
                                var anwsers = from a in dsAnwser.Tables[0].AsEnumerable()
                                              where a.Field<decimal>("msch_id") == decimal.Parse(dv[index]["msch_id"].ToString())
                                              select a;

                                foreach (var anwser in anwsers)
                                {
                                    strAnswer = anwser["answer_result"].ToString();
                                }
                            }

                            teamName1 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname1"].ToString() == "" ? dv[index]["team_name1"].ToString() : dv[index]["isportteamname1"].ToString()) : dv[index]["team_name1"].ToString();
                            teamName2 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname2"].ToString() == "" ? dv[index]["team_name2"].ToString() : dv[index]["isportteamname2"].ToString()) : dv[index]["team_name2"].ToString();
                            isDetail = int.Parse(dv[index]["countcard"].ToString()) > 0 || int.Parse(dv[index]["countscore"].ToString()) > 0 ? true : false;
                            string minutes = dv[index]["curent_period"].ToString() == MatchType.Finished.ToString() ? "FT" : dv[index]["minutes"].ToString();
                            minutes = dv[index]["curent_period"].ToString() == MatchType.Postponed.ToString() ? "Post." : minutes;
                            xElement.Add(new XElement("Score"
                               , new XAttribute("teamCode1", dv[index]["team_id1"].ToString())
                               , new XAttribute("teamCode2", dv[index]["team_id2"].ToString())
                               , new XAttribute("teamName1", teamName1.Replace("?", "") + dv[index]["placeteam1"].ToString())
                               , new XAttribute("teamName2", teamName2.Replace("?", "") + dv[index]["placeteam2"].ToString())
                               , new XAttribute("status", dv[index]["status"].ToString())
                               , new XAttribute("minutes", minutes)
                               , new XAttribute("curent_period", dv[index]["curent_period"].ToString())
                               , new XAttribute("score_home", dv[index]["score_home"].ToString() == "" ? "0" : dv[index]["score_home"].ToString())
                               , new XAttribute("score_away", dv[index]["score_away"].ToString() == "" ? "0" : dv[index]["score_away"].ToString())
                               , new XAttribute("score_home_ht", dv[index]["score_home_ht"].ToString() == "" ? dv[index]["score_home"].ToString() : dv[index]["score_home_ht"].ToString())
                               , new XAttribute("score_away_ht", dv[index]["score_away_ht"].ToString() == "" ? dv[index]["score_away"].ToString() : dv[index]["score_away_ht"].ToString())
                               , new XAttribute("matchId", dv[index]["match_id"].ToString())
                               , new XAttribute("mschId", dv[index]["msch_id"].ToString())
                               , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dv[index]["matchdate"].ToString(), lang))
                               , new XAttribute("contestGroupId", dv[index]["contestgroupid"].ToString())
                               , new XAttribute("isDetail", isDetail)
                               , new XAttribute("anwser", strAnswer) // y,n
                               ));
                            contentGroupid = dv[index]["tm_id"].ToString(); // ต้องเปลี่ยน TM_ID เพราะ contentGroupID thai league มันซ้ำกัน

							#endregion
						}

                        if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย

                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                        , new XElement("message", ""));
                    }
                    else
                    {
                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                        , new XElement("message", (matchType == MatchType.inprogress) ? "ขณะนี้ไม่มีการแข่งขัน" : "ไม่พบข้อมูลผลการแข่งขัน"));
                    }

                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                        , new XElement("message", (matchType == MatchType.inprogress) ? "ขณะนี้ไม่มีการแข่งขัน" : "ไม่พบข้อมูลผลการแข่งขัน"));
                    //CommandGetScore(rtnXML, "", lang, "", AppCode_LiveScore.MatchType.Finished, addDate, "N");
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScore >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// CommandGetScoreByTeamId
        /// </summary>
        /// <param name="rtnXML"></param>
        /// <param name="teamId"></param>
        /// <param name="lang"></param>
        /// <param name="matchType"></param>
        /// <param name="addDate"></param>
        /// <param name="isByWeek"></param>
        /// <returns></returns>
        public XDocument CommandGetScoreByTeamId(XDocument rtnXML, string teamId, string lang, MatchType matchType, decimal addDate, string isByWeek)
        {
            try
            {
                DataSet ds = null;
                string teamName1 = "", teamName2 = "";
                bool isDetail = false;

                    // Get by contestGroupId
                ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballscorebyteamId"
                       , new SqlParameter[] {new SqlParameter("@dateadd",addDate)
                    ,new SqlParameter("@byweek",isByWeek)
                    ,new SqlParameter("@status",matchType.ToString())
                    ,new SqlParameter("@teamId",teamId)
                    });



                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = FillterGetScore(ds.Tables[0].DefaultView, "", "");
                    string contentGroupid = "", img16 = "";
                    XElement xElement = null;
                    if (dv.Count > 0)
                    {
                        for (int index = 0; index < dv.Count; index++)
                        {
                            #region Add XML
                            if (contentGroupid != dv[index]["tm_id"].ToString()) // ต้องเปลี่ยน TM_ID เพราะ contentGroupID thai league มันซ้ำกัน
							{
                                if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                                img16 = dv[index]["PIC_48X48"].ToString() == "" ? "default.png" : dv[index]["PIC_48X48"].ToString();
                                xElement = new XElement("League",
                                 new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dv[index]["class_name_local"].ToString() == "" ? dv[index]["contestgroup_name"].ToString() : dv[index]["class_name_local"].ToString()) : dv[index]["contestgroup_name"].ToString())
                                , new XAttribute("contestGroupId", dv[index]["contestgroupid"].ToString())
                                , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString() + img16)
                                , new XAttribute("tmName", (lang == Country.th.ToString()) ? dv[index]["tm_name"].ToString() : dv[index]["tm_name"].ToString())
                                , new XAttribute("tmSystem", dv[index]["tm_system"].ToString()));

                            }
                            teamName1 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname1"].ToString() == "" ? dv[index]["team_name1"].ToString() : dv[index]["isportteamname1"].ToString()) : dv[index]["team_name1"].ToString();
                            teamName2 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname2"].ToString() == "" ? dv[index]["team_name2"].ToString() : dv[index]["isportteamname2"].ToString()) : dv[index]["team_name2"].ToString();
                            isDetail = int.Parse(dv[index]["countcard"].ToString()) > 0 || int.Parse(dv[index]["countscore"].ToString()) > 0 ? true : false;
                            string minutes = dv[index]["curent_period"].ToString() == MatchType.Finished.ToString() ? "FT" : dv[index]["minutes"].ToString();
                            minutes = dv[index]["curent_period"].ToString() == MatchType.Postponed.ToString() ? "Post." : minutes;
                            xElement.Add(new XElement("Score"
                               , new XAttribute("teamCode1", dv[index]["team_id1"].ToString())
                               , new XAttribute("teamCode2", dv[index]["team_id2"].ToString())
                               , new XAttribute("teamName1", teamName1.Replace("?", "") + dv[index]["placeteam1"].ToString())
                               , new XAttribute("teamName2", teamName2.Replace("?", "") + dv[index]["placeteam2"].ToString())
                               , new XAttribute("status", dv[index]["status"].ToString())
                               , new XAttribute("minutes", minutes)
                               , new XAttribute("curent_period", dv[index]["curent_period"].ToString())
                               , new XAttribute("score_home", dv[index]["score_home"].ToString() == "" ? "0" : dv[index]["score_home"].ToString())
                               , new XAttribute("score_away", dv[index]["score_away"].ToString() == "" ? "0" : dv[index]["score_away"].ToString())
                               , new XAttribute("score_home_ht", dv[index]["score_home_ht"].ToString() == "" ? dv[index]["score_home"].ToString() : dv[index]["score_home_ht"].ToString())
                               , new XAttribute("score_away_ht", dv[index]["score_away_ht"].ToString() == "" ? dv[index]["score_away"].ToString() : dv[index]["score_away_ht"].ToString())
                               , new XAttribute("matchId", dv[index]["match_id"].ToString())
                               , new XAttribute("mschId", dv[index]["msch_id"].ToString())
                               , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dv[index]["matchdate"].ToString(), lang))
                               , new XAttribute("contestGroupId", dv[index]["contestgroupid"].ToString())
                               , new XAttribute("isDetail", isDetail)
                               ));
                            contentGroupid = dv[index]["tm_id"].ToString(); // ต้องเปลี่ยน TM_ID เพราะ contentGroupID thai league มันซ้ำกัน

							#endregion
						}

                        if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย

                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                        , new XElement("message", ""));
                    }
                    else
                    {
                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                        , new XElement("message", (matchType == MatchType.inprogress) ? "ขณะนี้ไม่มีการแข่งขัน" : "ไม่พบข้อมูลผลการแข่งขัน"));
                    }

                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                        , new XElement("message", (matchType == MatchType.inprogress) ? "ขณะนี้ไม่มีการแข่งขัน" : "ไม่พบข้อมูลผลการแข่งขัน"));
                    //CommandGetScore(rtnXML, "", lang, "", AppCode_LiveScore.MatchType.Finished, addDate, "N");
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScore >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// CommandGetScore
        /// </summary>
        /// <param name="matchType"></param>
        /// <param name="addDate"></param>
        /// <param name="isByWeek"></param>
        /// <returns></returns>
        public XDocument CommandGetScore_IStore(XDocument rtnXML,string contestGroupId,string lang)
        {
            try
            {
                string teamName1 = "", teamName2 = "",img16="";
                bool isDetail = false;

                    // Get by contestGroupId
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballscorebygroupdate"
                    ,new SqlParameter[]{new SqlParameter("@contestGroupId",contestGroupId)});

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    XElement xElement = null;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        img16 = dr["PIC_16X11"].ToString() == "" ? "default.png" : dr["PIC_16X11"].ToString();
                        xElement = new XElement("League",
                                     new XAttribute("date", DatetoText(dr["match_date"].ToString(), lang))
                                     ,new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dr["class_name_local"].ToString() == "" ? dr["contestgroup_name"].ToString() : dr["class_name_local"].ToString()) : dr["contestgroup_name"].ToString())
                                    , new XAttribute("contestGroupId", dr["contestgroupid"].ToString())
                                    , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                                    , new XAttribute("tmName", (lang == Country.th.ToString()) ? dr["tm_name"].ToString() : dr["tm_name"].ToString())
                                    , new XAttribute("tmSystem", dr["tm_system"].ToString()));

                        DataSet dsDetail = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballscorebydate"
                        , new SqlParameter[] { new SqlParameter("@date", dr["match_date"].ToString()  )
                        ,new SqlParameter("@contestGroupId",contestGroupId)});

                        if (dsDetail.Tables.Count > 0 && dsDetail.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dv in dsDetail.Tables[0].Rows)
                            {
                                #region Add XML
                                teamName1 = (lang == Country.th.ToString()) ? (dv["isportteamname1"].ToString() == "" ? dv["team_name1"].ToString() : dv["isportteamname1"].ToString()) : dv["team_name1"].ToString();
                                teamName2 = (lang == Country.th.ToString()) ? (dv["isportteamname2"].ToString() == "" ? dv["team_name2"].ToString() : dv["isportteamname2"].ToString()) : dv["team_name2"].ToString();
                                isDetail = int.Parse(dv["countcard"].ToString()) > 0 || int.Parse(dv["countscore"].ToString()) > 0 ? true : false;
                                xElement.Add(new XElement("Score"
                                   //, new XAttribute("teamCode1", dv["team_id1"].ToString())
                                   //, new XAttribute("teamCode2", dv["team_id2"].ToString())
                                   , new XAttribute("teamName1", teamName1.Replace("?","") + dv["placeteam1"].ToString())
                                   , new XAttribute("teamName2", teamName2.Replace("?","") + dv["placeteam2"].ToString())
                                   , new XAttribute("match_status", dv["status"].ToString() == MatchType.Finished.ToString() ? "FT" : dv["status"].ToString())
                                   //, new XAttribute("minutes", dv["curent_period"].ToString() == MatchType.Finished.ToString() ? "FT" : dv["minutes"].ToString())
                                   //, new XAttribute("curent_period", dv["curent_period"].ToString())
                                   , new XAttribute("resultTeam1", dv["score_home"].ToString() == "" ? "0" : dv["score_home"].ToString())
                                   , new XAttribute("resultTeam2", dv["score_away"].ToString() == "" ? "0" : dv["score_away"].ToString())
                                   //, new XAttribute("score_home_ht", dv["score_home_ht"].ToString() == "" ? "0" : dv["score_home_ht"].ToString())
                                  // , new XAttribute("score_away_ht", dv["score_away_ht"].ToString() == "" ? "0" : dv["score_away_ht"].ToString())
                                   //, new XAttribute("matchId", dv["match_id"].ToString())
                                   , new XAttribute("mschId", dv["msch_id"].ToString())
                                   //, new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dv["matchdate"].ToString(), lang))
                                   //, new XAttribute("contestGroupId", dv["contestgroupid"].ToString())
                                   , new XAttribute("isDetail", isDetail)
                                   ));

                                #endregion
                            }
                            rtnXML.Element("SportApp").Add(xElement);
                        }
                    }
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                            , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                        , new XElement("message",  "ไม่พบข้อมูลผลการแข่งขัน"));
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScore >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// CommandGetTextScore
        /// </summary>
        /// <param name="matchType"></param>
        /// <param name="addDate"></param>
        /// <param name="isByWeek"></param>
        /// <returns></returns>
        public XDocument CommandGetTextScore(XDocument rtnXML, string countryId, string lang, string contestGroupId, MatchType matchType, decimal addDate, string isByWeek)
        {
            try
            {
                DataSet ds = null;
                string teamName1 = "", teamName2 = "", scoreHome = "", scoreAway = "", scoreHomeHt = "", scoreAwayHt = "";

                #region GetDataSource

                    if (contestGroupId == "")
                    {
                        // Get by country
                        ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballscorebycountryId"
                        , new SqlParameter[] {new SqlParameter("@dateadd",addDate)
                    ,new SqlParameter("@byweek",isByWeek)
                    ,new SqlParameter("@status",matchType.ToString())
                    ,new SqlParameter("@countryId",countryId)
                    });
                    }
                    else
                    {
                        // Get by contestGroupId
                        ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballscore"
                           , new SqlParameter[] {new SqlParameter("@dateadd",addDate)
                    ,new SqlParameter("@byweek",isByWeek)
                    ,new SqlParameter("@status",matchType.ToString())
                    ,new SqlParameter("@contestGroupId",contestGroupId)
                    });
                    }

                #endregion

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string contentGroupid = "",txtScore="";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        #region Add Text
                        if (contentGroupid != dr["contestgroupid"].ToString())
                        {
                            txtScore += (lang == Country.th.ToString()) ? dr["class_name_local"].ToString() : dr["contestgroup_name"].ToString();
                            txtScore += " : ";
                        }
                        teamName1 = (lang == Country.th.ToString()) ? (dr["isportteamname1"].ToString() == "" ? dr["team_name1"].ToString() : dr["isportteamname1"].ToString()) : dr["team_name1"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (dr["isportteamname2"].ToString() == "" ? dr["team_name2"].ToString() : dr["isportteamname2"].ToString()) : dr["team_name2"].ToString();

                        txtScore += dr["curent_period"].ToString() == MatchType.Finished.ToString() ? "FT" : dr["minutes"].ToString();
                        txtScore += " ";
                        txtScore += teamName1.Replace("?","") + dr["placeteam1"].ToString();
                        txtScore += " ";
                        scoreHome = dr["score_home"].ToString() == "" ? "0" : dr["score_home"].ToString();
                        scoreAway = dr["score_away"].ToString() == "" ? "0" : dr["score_away"].ToString();
                        scoreHomeHt = dr["score_home_ht"].ToString() == "" ? dr["score_home"].ToString() : dr["score_home_ht"].ToString();
                        scoreAwayHt = dr["score_away_ht"].ToString() == "" ? dr["score_away"].ToString() : dr["score_away_ht"].ToString();

                        txtScore += dr["curent_period"].ToString() == "1st half" ? scoreHomeHt + "-" + scoreAwayHt : scoreHome + "-" + scoreAway;
                        txtScore += " ";
                        txtScore += teamName2.Replace("?","") + dr["placeteam2"].ToString();
                        txtScore += " , ";
                        contentGroupid = dr["contestgroupid"].ToString();
                        #endregion
                    }

                     rtnXML.Element("SportApp").Add(new XElement("Score_score",txtScore));
                    
                    rtnXML.Element("SportApp").Add(new XElement("status_score", "success")
                    , new XElement("message", ""));

                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status_score", "success")
                        , new XElement("message_score", (matchType == MatchType.inprogress) ? "ขณะนี้ไม่มีการแข่งขัน" : "ไม่พบข้อมูลผลการแข่งขัน"));
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetLeagueTable >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status_score", "error")
                    , new XElement("message_score", ex.Message));
            }
            return rtnXML;
        }

        #region Get Score Detail
        /// <summary>
        /// CommandGetScoreDetail
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="mschId"></param>
        /// <returns></returns>
        public XDocument CommandGetScoreDetail(string matchId,string mschId,string lang)
        {
            
                XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                ,new XAttribute("date",AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                , new XAttribute("smsMessage", ConfigurationManager.AppSettings["wordingSMS"])
                ));
            try
            {
                DataSet ds = new DataSet();
                string teamName1 = "", teamName2 = "";
                if (matchId != "")
                {

                    #region matchId Get match Info
                    ds = GetFootballMatchInfo(matchId);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        teamName1 = (lang == Country.th.ToString()) ? (ds.Tables[0].Rows[0]["isportteamname1"].ToString() == "" ? ds.Tables[0].Rows[0]["team_name1"].ToString() : ds.Tables[0].Rows[0]["isportteamname1"].ToString()) : ds.Tables[0].Rows[0]["team_name1"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (ds.Tables[0].Rows[0]["isportteamname2"].ToString() == "" ? ds.Tables[0].Rows[0]["team_name2"].ToString() : ds.Tables[0].Rows[0]["isportteamname2"].ToString()) : ds.Tables[0].Rows[0]["team_name2"].ToString();
                        rtnXML.Element("SportApp").Add(new XElement("Score"
                            , new XAttribute("contestGroupId",ds.Tables[0].Rows[0]["contestgroupid"].ToString())
                            , new XAttribute("contestName", (lang == Country.th.ToString()) ? ds.Tables[0].Rows[0]["class_name_local"].ToString() : ds.Tables[0].Rows[0]["contestgroup_name"].ToString())
                            , new XAttribute("contestURLImages", "")
                            , new XAttribute("scoreType",ds.Tables[0].Rows[0]["score_type"].ToString())
                            , new XAttribute("scoreHome", ds.Tables[0].Rows[0]["score_home"].ToString())
                            , new XAttribute("scoreAway", ds.Tables[0].Rows[0]["score_away"].ToString())
                            , new XAttribute("tmName", (lang == Country.th.ToString()) ? ds.Tables[0].Rows[0]["tm_name"].ToString() : ds.Tables[0].Rows[0]["tm_name"].ToString())
                            , new XAttribute("teamCode1", ds.Tables[0].Rows[0]["team_id1"].ToString())
                            , new XAttribute("teamCode2", ds.Tables[0].Rows[0]["team_id2"].ToString())
                            , new XAttribute("teamName1", teamName1.Replace("?",""))
                            , new XAttribute("teamName2", teamName2.Replace("?",""))
                            , new XAttribute("likeURL", ConfigurationManager.AppSettings["ApplicationURLFaceBookScoreDetail"] + matchId)
                            , new XAttribute("shareURL", ConfigurationManager.AppSettings["ApplicationURLFaceBookScoreDetail"] + matchId)
                            ));
                    }

                    ds = GetFootballScoreDetail(matchId);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach(DataRow dr in ds.Tables[0].Rows)
                        {
                            rtnXML.Element("SportApp").Element("Score").Add(new XElement("ScoreDetail"
                            //, new XAttribute("playerName", (lang == Country.th.ToString()) ? dr["playerTH"].ToString() : dr["playerEN"].ToString())
                            , new XAttribute("playerName", dr["playerEN"].ToString().Replace("?", ""))
                            , new XAttribute("teamName", (lang == Country.th.ToString()) ? (dr["teamNameTH"].ToString() == "" ? dr["teamNameEN"].ToString().Replace("?", "") : dr["teamNameTH"].ToString().Replace("?", "")) : dr["teamNameEN"].ToString().Replace("?", ""))
                            ,new XAttribute("teamId",dr["team_id"].ToString())
                            ,new XAttribute("evenType",dr["evenType"].ToString())
                            ,new XAttribute("minute",dr["minute"].ToString())
                            ));
                        }
                    }

                    #endregion

                    #region Get Player
                    ds = GetFootballMatchPlayer(matchId) ;
                    XElement players = new XElement("MatchPlayer");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        XElement player = null;
                        string teamCode = "";
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (teamCode != dr["competitor_id"].ToString())
                            {
                                if (player != null) players.Add(player);
                                player = new XElement("team"
                                        , new XAttribute("teamId", dr["competitor_id"].ToString())
                                        , new XAttribute("name", dr["team_name"].ToString())
                                        );
                            }
                            player.Add(
                                new XElement("player"
                                    , new XAttribute("name", dr["player_name"].ToString())
                                    , new XAttribute("position", MapPosition(dr["position"].ToString()))
                                    ));
                            teamCode = dr["competitor_id"].ToString();
                        }

                        if (player != null) players.Add(player); // Add row สุดท้าย

                        
                    }
                    else
                    {
                        players.Add(new XElement("message","ขออภัยไม่พบข้อมูล!!"));
                    }
                    #endregion

                    rtnXML.Element("SportApp").Add(players);

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", ""));

                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", "ขณะนี้ไม่มีการแข่งขัน"));
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("CommandGetScoreDetail >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }

        /// <summary>
        ///    CommandGetScoreDetail ( Isport StarSoccer  จะไม่มีรายชื่อผู่เล่น )
        /// </summary>
        /// <param name="rtnXML"></param>
        /// <param name="matchId"></param>
        /// <param name="mschId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetScoreDetail(XDocument rtnXML,string matchId, string mschId, string lang)
        {
            try
            {
                DataSet ds = new DataSet();
                string teamName1 = "", teamName2 = "";
                if (matchId != "")
                {

                    #region matchId Get match Info
                    ds = GetFootballMatchInfo(matchId);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        teamName1 = (lang == Country.th.ToString()) ? (ds.Tables[0].Rows[0]["isportteamname1"].ToString() == "" ? ds.Tables[0].Rows[0]["team_name1"].ToString() : ds.Tables[0].Rows[0]["isportteamname1"].ToString()) : ds.Tables[0].Rows[0]["team_name1"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (ds.Tables[0].Rows[0]["isportteamname2"].ToString() == "" ? ds.Tables[0].Rows[0]["team_name2"].ToString() : ds.Tables[0].Rows[0]["isportteamname2"].ToString()) : ds.Tables[0].Rows[0]["team_name2"].ToString();
                        rtnXML.Element("SportApp").Add(new XElement("Score"
                            , new XAttribute("contestGroupId", ds.Tables[0].Rows[0]["contestgroupid"].ToString())
                            , new XAttribute("contestName", (lang == Country.th.ToString()) ? ds.Tables[0].Rows[0]["class_name_local"].ToString() : ds.Tables[0].Rows[0]["contestgroup_name"].ToString())
                            , new XAttribute("contestURLImages", "")
                            , new XAttribute("scoreType", ds.Tables[0].Rows[0]["score_type"].ToString())
                            , new XAttribute("scoreHome", ds.Tables[0].Rows[0]["score_home"].ToString())
                            , new XAttribute("scoreAway", ds.Tables[0].Rows[0]["score_away"].ToString())
                            , new XAttribute("tmName", (lang == Country.th.ToString()) ? ds.Tables[0].Rows[0]["tm_name"].ToString() : ds.Tables[0].Rows[0]["tm_name"].ToString())
                            , new XAttribute("teamCode1", ds.Tables[0].Rows[0]["team_id1"].ToString())
                            , new XAttribute("teamCode2", ds.Tables[0].Rows[0]["team_id2"].ToString())
                            , new XAttribute("teamName1", teamName1.Replace("?", ""))
                            , new XAttribute("teamName2", teamName2.Replace("?", ""))
                            , new XAttribute("likeURL", ConfigurationManager.AppSettings["ApplicationURLFaceBookScoreDetail"] + matchId)
                            , new XAttribute("shareURL", ConfigurationManager.AppSettings["ApplicationURLFaceBookScoreDetail"] + matchId)
                            ));
                    }

                    ds = GetFootballScoreDetail(matchId);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            rtnXML.Element("SportApp").Element("Score").Add(new XElement("ScoreDetail"
                                //, new XAttribute("playerName", (lang == Country.th.ToString()) ? dr["playerTH"].ToString() : dr["playerEN"].ToString())
                            , new XAttribute("playerName", dr["playerEN"].ToString().Replace("?", ""))
                            , new XAttribute("teamName", (lang == Country.th.ToString()) ? (dr["teamNameTH"].ToString() == "" ? dr["teamNameEN"].ToString().Replace("?", "") : dr["teamNameTH"].ToString().Replace("?", "")) : dr["teamNameEN"].ToString().Replace("?", ""))
                            , new XAttribute("teamId", dr["team_id"].ToString())
                            , new XAttribute("evenType", dr["evenType"].ToString())
                            , new XAttribute("minute", dr["minute"].ToString())
                            ));
                        }
                    }

                    #endregion

                    #region Get Player
                    //ds = GetFootballMatchPlayer(matchId);
                    //XElement players = new XElement("MatchPlayer");
                    //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    //{
                    //    XElement player = null;
                    //    string teamCode = "";
                    //    foreach (DataRow dr in ds.Tables[0].Rows)
                    //    {
                    //        if (teamCode != dr["competitor_id"].ToString())
                    //        {
                    //            if (player != null) players.Add(player);
                    //            player = new XElement("team"
                    //                    , new XAttribute("teamId", dr["competitor_id"].ToString())
                    //                    , new XAttribute("name", dr["team_name"].ToString())
                    //                    );
                    //        }
                    //        player.Add(
                    //            new XElement("player"
                    //                , new XAttribute("name", dr["player_name"].ToString())
                    //                , new XAttribute("position", MapPosition(dr["position"].ToString()))
                    //                ));
                    //        teamCode = dr["competitor_id"].ToString();
                    //    }

                    //    if (player != null) players.Add(player); // Add row สุดท้าย


                    //}
                    //else
                    //{
                    //    players.Add(new XElement("message", "ขออภัยไม่พบข้อมูล!!"));
                    //}
                    #endregion

                    //rtnXML.Element("SportApp").Add(players);

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", ""));

                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", "ขณะนี้ไม่มีการแข่งขัน"));
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
        
        public DataSet GetFootballMatchPlayer(string matchId)
        {
            try
            {
                return SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballmatchplayer"
                           , new SqlParameter[] { new SqlParameter("@match_id", matchId) });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataSet GetFootballMatchInfo(string matchId)
        {
            try
            {
                return SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballmatchinfo"
                        , new SqlParameter[] { new SqlParameter("@match_id", matchId) });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataSet GetFootballScoreDetail(string matchId)
        {
            try
            {
                return SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballscoredetail"
                        , new SqlParameter[] { new SqlParameter("@match_id", matchId) });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string MapPosition(string position)
        {
            string rtn = "";
            switch (position)
            {
                case "gk": rtn = "GK"; break;
                case "defender": rtn = "DEF"; break;
                case "midfield": rtn = "MID"; break;
                case "striker": rtn = "FOR"; break;
            }
            return rtn;
        }

        #endregion


        #endregion
    }
}
