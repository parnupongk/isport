using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;

namespace WS_BB
{
    public class AppCode_FootballAnalysis : AppCode_Base
    {

        #region Database Isport
        public struct Out_Analysis
        {
            public string status;
            public string errorMess;
            public List<MatchAnalysis> analysis;
            public List<MatchAnalysis_Detail> matchDetail;
            public List<MatchAnalysis_Level> matchLevel;
            public List<matchAnalysis_Top5Last> matchTop5Last;
            public List<matchAnalysis_Top5VS> matchTop5VS;
        }
        public class MatchAnalysis
        {
            // Title Name
            public string title1 = "ผลงานในลีก";
            public string title2 = "5 นัดหลังสุด";
            public string title3 = "5 นัดสุดท้ายที่พบกัน";
            public string title4 = "เอเชี่ยนแฮนดิแคป";
            public string title5 = "อัตราพลูเฉลี่ย";
            public string title6 = "แนวโน้มของเกม";
            public string title7 = "มองอย่างเซียน";
            // Match Detail , handicap , poolavg
            public string scsId;
            public string mschId;
            public string classNameTH;
            public string classNameEN;
            public string matchDateTime;
            public string matchTime;
            public string teamCode1;
            public string teamCode2;
            public string teamNameCode1;
            public string teamNameCode1EN;
            public string teamNameCode2;
            public string teamNameCode2EN;
            public string liveChannel;
            public string handicapLocal;
            public string poolLocal;
            public string trendforGame1;
            public string trendforGame2;
            // top 5 last , top 5 นัดสุดท้ายที่พบกัน
            public string result;
            public string result_team_code1;
            public string result_team_code2;
            
            // Level
            public string index;
            public string total_match;
            public string total_win;
            public string total_draw;
            public string total_loss;
            public string total_gs;
            public string total_ga;
            public string point;
            public string gd;
        }

        #region Class for I Phone
        public class MatchAnalysis_Detail
        {
            public string scsId;
            public string mschId;
            public string classNameTH;
            public string classNameEN;
            public string matchDateTime;
            public string matchTime;
            public string teamCode1;
            public string teamCode2;
            public string teamNameCode1;
            public string teamNameCode2;
            public string teamNameCode1EN;
            public string teamNameCode2EN;
            public string liveChannel;
            public string handicapLocal;
            public string poolAverrage;
            public string poolHigh;
            public string trendforGame1;
            public string trendforGame2;
        }
        public class MatchAnalysis_Level
        {
            public string index;

            public string total_match;
            public string total_win;
            public string total_draw;
            public string total_loss;
            public string total_gs;
            public string total_ga;
            public string point;
            public string gd;
            public string teamNameCode1;
            public string teamNameCode1EN;
            public string teamCode;
            public string groupCode;
        }
        public class matchAnalysis_Top5Last
        {
            public string result;
            public string result_team_code1;
            public string result_team_code2;
            public string teamNameCode1;
            public string teamNameCode2;
            public string teamNameCode1EN;
            public string teamNameCode2EN;
        }
        public class matchAnalysis_Top5VS
        {
            public string result_team_code1;
            public string result_team_code2;
            public string teamNameCode1;
            public string teamNameCode2;
            public string teamNameCode1EN;
            public string teamNameCode2EN;
        }
        #endregion

        #region Level

        /// <summary>
        /// Level
        /// </summary>
        /// <param name="scsId"></param>
        /// <param name="teamCode1"></param>
        /// <param name="teamCode2"></param>
        /// <returns></returns>
        public List<MatchAnalysis> CommandGetFootballAnalysislevel(string scsId, string teamCode1, string teamCode2)
        {
            List<MatchAnalysis> matchAnalysis = new List<MatchAnalysis>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_analysis_levelvs"
                    , new SqlParameter[] { new SqlParameter("@scs_id",scsId)
                    ,new SqlParameter("@team_code1", teamCode1)
                    ,new SqlParameter("@team_code2", teamCode2) });
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MatchAnalysis msa = new MatchAnalysis();
                    msa.index = dr["row"].ToString();
                    msa.total_match = dr["total_match"].ToString();
                    msa.total_win = dr["total_win"].ToString();
                    msa.total_draw = dr["total_draw"].ToString();
                    msa.total_loss = dr["total_loss"].ToString();
                    msa.total_gs = dr["total_f"].ToString();
                    msa.total_ga = dr["total_a"].ToString();
                    msa.point = dr["point"].ToString();
                    msa.gd = dr["gd"].ToString();
                    msa.teamNameCode1 = dr["short_name"].ToString();
                    msa.teamNameCode1EN = dr["short_name_en"].ToString();
                    matchAnalysis.Add(msa);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetFootballAnalysislevel>> " + ex.Message);
                throw new Exception(ex.Message);
            }
            return matchAnalysis;
        }

        /// <summary>
        /// Level for iPhone
        /// </summary>
        /// <param name="scsId"></param>
        /// <param name="teamCode1"></param>
        /// <param name="teamCode2"></param>
        /// <returns></returns>
        public List<MatchAnalysis_Level> CommandGetFootballAnalysislevel_IPhone(string scsId, string teamCode1, string teamCode2)
        {
            List<MatchAnalysis_Level> matchAnalysis = new List<MatchAnalysis_Level>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_analysis_levelvs"
                    , new SqlParameter[] { new SqlParameter("@scs_id",scsId)
                    ,new SqlParameter("@team_code1", teamCode1)
                    ,new SqlParameter("@team_code2", teamCode2) });
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MatchAnalysis_Level msa = new MatchAnalysis_Level();
                    msa.index = dr["row"].ToString();
                    msa.total_match = dr["total_match"].ToString();
                    msa.total_win = dr["total_win"].ToString();
                    msa.total_draw = dr["total_draw"].ToString();
                    msa.total_loss = dr["total_loss"].ToString();
                    msa.total_gs = dr["total_f"].ToString();
                    msa.total_ga = dr["total_a"].ToString();
                    msa.point = dr["point"].ToString();
                    msa.gd = dr["gd"].ToString();
                    msa.teamNameCode1 = dr["short_name"].ToString();
                    msa.teamNameCode1EN = dr["short_name_en"].ToString();
                    matchAnalysis.Add(msa);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetFootballAnalysislevel_IPhone>> " + ex.Message);
                throw new Exception(ex.Message);
            }
            return matchAnalysis;
        }

        /// <summary>
        /// Level By SCS ID
        /// </summary>
        /// <param name="scsId"></param>
        /// <returns></returns>
        public List<MatchAnalysis_Level> CommandGetFootballAnalysislevelByScsId(string scsId)
        {
            List<MatchAnalysis_Level> matchAnalysis = new List<MatchAnalysis_Level>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_tablebyscsid"
                    , new SqlParameter[] { new SqlParameter("@scs_id",scsId)});
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MatchAnalysis_Level msa = new MatchAnalysis_Level();
                    msa.index = dr["row"].ToString();
                    msa.total_match = dr["total_match"].ToString();
                    msa.total_win = dr["total_win"].ToString();
                    msa.total_draw = dr["total_draw"].ToString();
                    msa.total_loss = dr["total_loss"].ToString();
                    msa.total_gs = dr["total_f"].ToString();
                    msa.total_ga = dr["total_a"].ToString();
                    msa.point = dr["point"].ToString();
                    msa.gd = dr["gd"].ToString();
                    msa.teamNameCode1 = dr["short_name"].ToString();
                    msa.teamNameCode1EN = dr["short_name_en"].ToString();
                    msa.teamCode = dr["team_code"].ToString();
                    msa.groupCode = dr["group_code"].ToString();
                    matchAnalysis.Add(msa);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetFootballAnalysislevelByScsId>> " + ex.Message);
                throw new Exception(ex.Message);
            }
            return matchAnalysis;
        }

        /// <summary>
        /// Level By SCS ID
        /// </summary>
        /// <param name="scsId"></param>
        /// <returns></returns>
        public XElement CommandGetFootballAnalysislevelByscsIdXML(string elementName,string scsId,string lang)
        {
            XElement element = new XElement(elementName);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_tablebyscsid"
                    , new SqlParameter[] { new SqlParameter("@scs_id", scsId) });
                
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    element.Add( new XElement("team",
                        new XAttribute("index",dr["row"].ToString()),
                        new XAttribute("total_match",dr["total_match"].ToString()),
                        new XAttribute("total_win",dr["total_win"].ToString()),
                        new XAttribute("total_draw",dr["total_draw"].ToString()),
                        new XAttribute("total_loss",dr["total_loss"].ToString()),
                        new XAttribute("total_gs",dr["total_f"].ToString()),
                        new XAttribute("total_ga",dr["total_a"].ToString()),
                        new XAttribute("point",dr["point"].ToString()),
                        new XAttribute("gd",dr["gd"].ToString()),
                        new XAttribute("teamNameCode1", lang == Country.th.ToString() ? dr["short_name"].ToString() : dr["short_name_en"].ToString()),
                        new XAttribute("groupCode",dr["group_code"].ToString())
                        ));
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetFootballAnalysislevelByScsId>> " + ex.Message);
                //throw new Exception(ex.Message);
            }
            return element;
        }

        /// <summary>
        /// Level By SCS ID
        /// </summary>
        /// <param name="scsId"></param>
        /// <returns></returns>
        public XElement CommandGetFootballAnalysislevelByscsIdXML_IStore( string scsId, string lang)
        {
            XElement element = new XElement("LeagueTable"
                ,new XAttribute("contestURLImages","")
                , new XAttribute("tmName", lang == Country.th.ToString() ? "ฟุตบอลไทยลีก" : "Thai League"));
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_tablebyscsid"
                    , new SqlParameter[] { new SqlParameter("@scs_id", scsId) });

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    element.Add(new XElement("team",
                        new XAttribute("index", dr["row"].ToString()),
                        new XAttribute("total_match", dr["total_match"].ToString()),
                        new XAttribute("total_win", dr["total_win"].ToString()),
                        new XAttribute("total_draw", dr["total_draw"].ToString()),
                        new XAttribute("total_loss", dr["total_loss"].ToString()),
                        new XAttribute("total_gs", dr["total_f"].ToString()),
                        new XAttribute("total_ga", dr["total_a"].ToString()),
                        new XAttribute("point", dr["point"].ToString()),
                        new XAttribute("gd", dr["gd"].ToString()),
                        new XAttribute("teamNameCode1", lang == Country.th.ToString() ? dr["short_name"].ToString() : dr["short_name_en"].ToString()),
                        new XAttribute("groupCode", dr["group_code"].ToString())
                        ));

                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetFootballAnalysislevelByScsId>> " + ex.Message);
                //throw new Exception(ex.Message);
            }
            return element;
        }

        #endregion

        #region top 5 นัดสุดท้ายที่พบกัน

        /// <summary>
        /// top 5 นัดสุดท้ายที่พบกัน
        /// </summary>
        /// <param name="teamCode1"></param>
        /// <param name="teamCode2"></param>
        /// <returns></returns>
        public List<MatchAnalysis> CommandGetFootballAnalysisTop5VS(string teamCode1, string teamCode2)
        {
            List<MatchAnalysis> matchAnalysis = new List<MatchAnalysis>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_analysis_top5vs"
                    , new SqlParameter[] { new SqlParameter("@team_code1", teamCode1)
                    ,new SqlParameter("@team_code2", teamCode2) });
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MatchAnalysis msa = new MatchAnalysis();
                    msa.result_team_code1 = dr["resultteam1"].ToString();
                    msa.result_team_code2 = dr["resultteam2"].ToString();
                    msa.teamNameCode1 = dr["teamcode1th"].ToString();
                    msa.teamNameCode2 = dr["teamcode2th"].ToString();
                    msa.teamNameCode1EN = dr["teamcode1en"].ToString();
                    msa.teamNameCode2EN = dr["teamcode2en"].ToString();
                    matchAnalysis.Add(msa);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetFootballAnalysisTop5VS>> " + ex.Message);
                throw new Exception(ex.Message);
            }
            return matchAnalysis;
        }

        /// <summary>
        /// top 5 นัดสุดท้ายที่พบกัน for iphone
        /// </summary>
        /// <param name="teamCode1"></param>
        /// <param name="teamCode2"></param>
        /// <returns></returns>
        public List<matchAnalysis_Top5VS> CommandGetFootballAnalysisTop5VS_IPhone(string teamCode1, string teamCode2)
        {
            List<matchAnalysis_Top5VS> matchAnalysis = new List<matchAnalysis_Top5VS>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_analysis_top5vs"
                    , new SqlParameter[] { new SqlParameter("@team_code1", teamCode1)
                    ,new SqlParameter("@team_code2", teamCode2) });
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    matchAnalysis_Top5VS msa = new matchAnalysis_Top5VS();
                    msa.result_team_code1 = dr["resultteam1"].ToString();
                    msa.result_team_code2 = dr["resultteam2"].ToString();
                    msa.teamNameCode1 = dr["teamcode1th"].ToString();
                    msa.teamNameCode2 = dr["teamcode2th"].ToString();
                    msa.teamNameCode1EN = dr["teamcode1en"].ToString();
                    msa.teamNameCode2EN = dr["teamcode2en"].ToString();
                    matchAnalysis.Add(msa);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetFootballAnalysisTop5VS_IPhone>> " + ex.Message);
                throw new Exception(ex.Message);
            }
            return matchAnalysis;
        }

        #endregion

        #region Get 5 นัดหลังสุดของทั้ง 2 ทีม (ทุกลีก)
        /// <summary>
        /// Get 5 นัดหลังสุดของทั้ง 2 ทีม (ทุกลีก)
        /// </summary>
        /// <param name="teamCode1"></param>
        /// <param name="teamCode2"></param>
        /// <returns></returns>
        public List<MatchAnalysis> CommandGetFootballAnalysisTop5Last(string teamCode1, string teamCode2)
        {
            List<MatchAnalysis> matchAnalysis = new List<MatchAnalysis>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_analysis_top5last"
                    , new SqlParameter[] { new SqlParameter("@team_code1", teamCode1)
                    ,new SqlParameter("@team_code2", teamCode2) });
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MatchAnalysis msa = new MatchAnalysis();
                    msa.result = dr["result"].ToString();
                    msa.result_team_code1 = dr["result_team_code1"].ToString();
                    msa.result_team_code2 = dr["result_team_code2"].ToString();
                    msa.teamNameCode1 = dr["teamcode1th"].ToString();
                    msa.teamNameCode2 = dr["teamcode2th"].ToString();
                    msa.teamNameCode1EN = dr["teamcode1en"].ToString();
                    msa.teamNameCode2EN = dr["teamcode2en"].ToString();
                    matchAnalysis.Add(msa);
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.WriteError("CommandGetFootballAnalysisTop5Last>> " + ex.Message);
                throw new Exception(ex.Message);
            }
            return matchAnalysis;
        }

        /// <summary>
        /// Get 5 นัดหลังสุดของทั้ง 2 ทีม (ทุกลีก) for i phone
        /// </summary>
        /// <param name="teamCode1"></param>
        /// <param name="teamCode2"></param>
        /// <returns></returns>
        public List<matchAnalysis_Top5Last> CommandGetFootballAnalysisTop5Last_IPhone(string teamCode1, string teamCode2)
        {
            List<matchAnalysis_Top5Last> matchAnalysis = new List<matchAnalysis_Top5Last>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_analysis_top5last"
                    , new SqlParameter[] { new SqlParameter("@team_code1", teamCode1)
                    ,new SqlParameter("@team_code2", teamCode2) });
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    matchAnalysis_Top5Last msa = new matchAnalysis_Top5Last();
                    msa.result = dr["result"].ToString();
                    msa.result_team_code1 = dr["result_team_code1"].ToString();
                    msa.result_team_code2 = dr["result_team_code2"].ToString();
                    msa.teamNameCode1 = dr["teamcode1th"].ToString();
                    msa.teamNameCode2 = dr["teamcode2th"].ToString();
                    msa.teamNameCode1EN = dr["teamcode1en"].ToString();
                    msa.teamNameCode2EN = dr["teamcode2en"].ToString();
                    matchAnalysis.Add(msa);
                }
            }
            catch (Exception ex)
            {

                ExceptionManager.WriteError("CommandGetFootballAnalysisTop5Last_IPhone>> " + ex.Message);
                throw new Exception(ex.Message);
            }
            return matchAnalysis;
        }
        #endregion

        #region Get Detail
        /// <summary>
        /// Match Detail , handicap , poolavg ,แนวโน้มของเกม(มองอย่างเซียน)
        /// </summary>
        /// <param name="mschId"></param>
        /// <returns></returns>
        public List<MatchAnalysis> CommandGetFootballAnalysisDetail(string mschId)
        {

            List<MatchAnalysis> matchAnalysis = new List<MatchAnalysis>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_analysis_detail"
                    ,new SqlParameter[]{new SqlParameter("@msch_id",mschId)});
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    MatchAnalysis msa = new MatchAnalysis();
                    msa.scsId = dr["scs_id"].ToString();
                    msa.mschId = dr["msch_id"].ToString();
                    msa.classNameTH = dr["class_name_local"].ToString();
                    msa.classNameEN = dr["class_name"].ToString();
                    msa.matchDateTime = AppCode_LiveScore.DatetoText( dr["match_date"].ToString().Substring(0,8));
                    msa.matchTime = dr["match_time"].ToString();
                    msa.teamCode1 = dr["team_code1"].ToString();
                    msa.teamCode2 = dr["team_code2"].ToString();
                    msa.teamNameCode1 = dr["teamcode1th"].ToString();
                    msa.teamNameCode2 = dr["teamcode2th"].ToString();
                    msa.teamNameCode1EN = dr["teamcode1en"].ToString();
                    msa.teamNameCode2EN = dr["teamcode2en"].ToString();
                    msa.liveChannel = dr["live_channel_local"].ToString();
                    msa.handicapLocal = dr["handicap_update_local"].ToString();
                    msa.poolLocal = dr["scnt_detail_local"].ToString();
                    msa.trendforGame1 = dr["trendGame"].ToString();
                    msa.trendforGame2 = dr["trendGame"].ToString().Split('|').Length == 2 ? dr["trendGame"].ToString().Split('|')[1] : "";
                    matchAnalysis.Add(msa);
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("CommandGetFootballAnalysisDetail>> " + ex.Message);
                throw new Exception(ex.Message);
            }
            return matchAnalysis;
        }

        /// <summary>
        /// Match Detail , handicap , poolavg ,แนวโน้มของเกม(มองอย่างเซียน) fot i phone
        /// </summary>
        /// <param name="mschId"></param>
        /// <returns></returns>
        public List<MatchAnalysis_Detail> CommandGetFootballAnalysisDetail_IPhone(string mschId)
        {

            List<MatchAnalysis_Detail> matchAnalysis = new List<MatchAnalysis_Detail>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_analysis_detail"
                    , new SqlParameter[] { new SqlParameter("@msch_id", mschId) });
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MatchAnalysis_Detail msa = new MatchAnalysis_Detail();
                    msa.scsId = dr["scs_id"].ToString();
                    msa.mschId = dr["msch_id"].ToString();
                    msa.classNameTH = dr["class_name_local"].ToString();
                    msa.classNameEN = dr["class_name"].ToString();
                    msa.matchDateTime = AppCode_LiveScore.DatetoText(dr["match_date"].ToString().Substring(0, 8));
                    msa.matchTime = dr["match_time"].ToString();
                    msa.teamCode1 = dr["team_code1"].ToString();
                    msa.teamCode2 = dr["team_code2"].ToString();
                    msa.teamNameCode1 = dr["teamcode1th"].ToString();
                    msa.teamNameCode2 = dr["teamcode2th"].ToString();
                    msa.teamNameCode1EN = dr["teamcode1en"].ToString();
                    msa.teamNameCode2EN = dr["teamcode2en"].ToString();
                    msa.liveChannel = dr["live_channel_local"].ToString();
                    msa.handicapLocal = dr["handicap_update_local"].ToString();
                    msa.poolAverrage = dr["scnt_detail_local"].ToString().Split('|').Length == 6 ? dr["scnt_detail_local"].ToString() : "";
                    msa.poolHigh  = dr["scnt_detail_local"].ToString().Split('|').Length == 6 ? dr["scnt_detail_local"].ToString() : "";
                    msa.trendforGame1 = dr["trendGame"].ToString();
                    msa.trendforGame2 = dr["trendGame"].ToString().Split('|').Length == 2 ? dr["trendGame"].ToString().Split('|')[1] : "";
                    matchAnalysis.Add(msa);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetFootballAnalysisDetail>> " + ex.Message);
                throw new Exception(ex.Message);
            }
            return matchAnalysis;
        }

        #endregion

        #endregion

        #region Database IsportFeed

        public XDocument CommandGetSportpoolAnalysisAllLeague(XDocument rtnXML,string contestGroupId,string teamCode1, string teamCode2, string matchId, string lang, string type)
        {
            try
            {
                string teamName1 = "", teamName2 = "", teamName = "";

                #region Get วิเคราะห์ ,เอเชี่ยนแฮนดิแคป,ผลที่คาด
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballanalysis_detail"
                    , new SqlParameter[] { new SqlParameter("@match_id", matchId) });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string analyse = dr["trendgame"].ToString().Split('|').Length > 0 ? dr["trendgame"].ToString().Split('|')[0] : "";
                        string result = dr["trendgame"].ToString().Split('|').Length == 2 ? dr["trendgame"].ToString().Split('|')[1] : "";
                        rtnXML.Element("SportApp").Add(
                            new XElement("analyse_detail"
                                , new XAttribute("analyse", analyse)
                                , new XAttribute("handicap", dr["handicap_update_local"].ToString())
                                , new XAttribute("result", result)
                                , new XAttribute("poolavg", dr["poolavg"].ToString())
                                ));
                    }
                }
                #endregion

                #region Get Head to Head
                ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballanalysis_headtohead"
                    , new SqlParameter[] { new SqlParameter("@contestgroupid",contestGroupId)
                ,new SqlParameter("@teamcode1",teamCode1)
                ,new SqlParameter("@teamcode2",teamCode2)});
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int countWin1 = 0, countWin2 = 0
                        , countTie1 = 0, countTie2 = 0
                        , countLose1 = 0, countLose2 = 0
                        , countScore1 = 0, countScore2 = 0;
                    XElement xmlHead = new XElement("headtohead_detail");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (int.Parse(dr["result_team_code1"].ToString()) > int.Parse(dr["result_team_code2"].ToString()))
                        {
                            countWin1++;
                            countLose2++;
                        }
                        else if (int.Parse(dr["result_team_code1"].ToString()) == int.Parse(dr["result_team_code2"].ToString()))
                        {
                            countTie1++;
                            countTie2++;
                        }
                        else if (int.Parse(dr["result_team_code1"].ToString()) < int.Parse(dr["result_team_code2"].ToString()))
                        {
                            countWin2++;
                            countLose1++;
                        }
                        countScore1 += int.Parse(dr["result_team_code1"].ToString());
                        countScore2 += int.Parse(dr["result_team_code2"].ToString());

                        teamName1 = (lang == Country.th.ToString()) ? (dr["teamname1th"].ToString() == "" ? dr["teamname1en"].ToString() : dr["teamname1th"].ToString()) : dr["teamname1en"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (dr["teamname2th"].ToString() == "" ? dr["teamname2en"].ToString() : dr["teamname2th"].ToString()) : dr["teamname2en"].ToString();


                        xmlHead.Add(new XElement("detail" ,new XAttribute("team_id1",dr["team_id1"])
                                                                            ,new XAttribute("team_id2",dr["team_id2"])
                                                                            ,new XAttribute("team_name1",teamName1)
                                                                            ,new XAttribute("team_name2",teamName2)
                                                                            ,new XAttribute("result_team1",dr["result_team_code1"])
                                                                            ,new XAttribute("result_team2",dr["result_team_code2"])
                                                                            ));
                    }
                    // add head to head detail
                    rtnXML.Element("SportApp").Add(xmlHead);
                    //
                    teamName1 = (lang == Country.th.ToString()) ? (ds.Tables[0].Rows[0]["teamname1th"].ToString() == "" ? ds.Tables[0].Rows[0]["teamname1en"].ToString() : ds.Tables[0].Rows[0]["teamname1th"].ToString()) : ds.Tables[0].Rows[0]["teamname1en"].ToString();
                    teamName2 = (lang == Country.th.ToString()) ? (ds.Tables[0].Rows[0]["teamname2th"].ToString() == "" ? ds.Tables[0].Rows[0]["teamname2en"].ToString() : ds.Tables[0].Rows[0]["teamname2th"].ToString()) : ds.Tables[0].Rows[0]["teamname2en"].ToString();
                    rtnXML.Element("SportApp").Add(
                        new XElement("analyse_headtohead"
                            , new XElement("headtoHead_team"
                                , new XAttribute("teamName1", teamName1 + "(" + ds.Tables[0].Rows[0]["placeteam1"].ToString() + ")")
                                , new XAttribute("total_match", ds.Tables[0].Rows.Count)
                                , new XAttribute("total_win", countWin1)
                                , new XAttribute("total_draw", countTie1)
                                , new XAttribute("total_loss", countLose1)
                                , new XAttribute("total_gs", countScore1)
                                , new XAttribute("total_ga", countScore2)
                                , new XAttribute("team_code", teamCode1)
                                )
                            , new XElement("headtohead_team"
                                , new XAttribute("teamName2", teamName2 + "(" + ds.Tables[0].Rows[0]["placeteam2"].ToString() + ")")
                                , new XAttribute("total_match", ds.Tables[0].Rows.Count)
                                , new XAttribute("total_win", countWin2)
                                , new XAttribute("total_draw", countTie2)
                                , new XAttribute("total_loss", countLose2)
                                , new XAttribute("total_gs", countScore2)
                                , new XAttribute("total_ga", countScore1)
                                , new XAttribute("team_code", teamCode2)
                                )
                            ));
                }
                #endregion

                #region Get football table
                ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballtablebyteam"
                    , new SqlParameter[] { new SqlParameter("@contestgroupid",contestGroupId)
                ,new SqlParameter("@teamcode1",teamCode1)
                ,new SqlParameter("@teamCode2",teamCode2)});
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    XElement xElement = new XElement("LeagueTable");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        teamName1 = (lang == Country.th.ToString()) ? (dr["team_name_local"].ToString() == "" ? dr["team_name"].ToString() : dr["team_name_local"].ToString()) : dr["team_name"].ToString();
                        xElement.Add(new XElement("leaguetable_team"
                                    , new XAttribute("total_point", dr["total_point"].ToString())
                                    , new XAttribute("total_diff", dr["total_diff"].ToString())
                                    , new XAttribute("away_concede", dr["away_concede"].ToString())
                                    , new XAttribute("away_score", dr["away_score"].ToString())
                                    , new XAttribute("away_lost", dr["away_lost"].ToString())
                                    , new XAttribute("away_draws", dr["away_draws"].ToString())
                                    , new XAttribute("away_won", dr["away_won"].ToString())
                                    , new XAttribute("away_play", dr["away_play"].ToString())
                                    , new XAttribute("home_concede", dr["home_concede"].ToString())
                                    , new XAttribute("home_score", dr["home_score"].ToString())
                                    , new XAttribute("home_lost", dr["home_lost"].ToString())
                                    , new XAttribute("home_draws", dr["home_draws"].ToString())
                                    , new XAttribute("home_won", dr["home_won"].ToString())
                                    , new XAttribute("home_play", dr["home_play"].ToString())
                                    , new XAttribute("total_concede", dr["total_concede"].ToString())
                                    , new XAttribute("total_score", dr["total_score"].ToString())
                                    , new XAttribute("total_lost", dr["total_lost"].ToString())
                                    , new XAttribute("total_draws", dr["total_draws"].ToString())
                                    , new XAttribute("total_won", dr["total_won"].ToString())
                                    , new XAttribute("total_play", dr["total_play"].ToString())
                                    , new XAttribute("teamName1", teamName1)
                                    , new XAttribute("place", dr["place"].ToString())
                                    , new XAttribute("team_id", dr["team_id"].ToString())
                                    ));
                    }
                    rtnXML.Element("SportApp").Add(xElement);
                }
                #endregion

                #region Get football static
                ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballstaticAllleague"
                    , new SqlParameter[] { new SqlParameter("@match_id",matchId)});
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (type.ToLower() == "iphone")
                    {
                        for (int index = 0; index < 2; index++)
                        {
                            DataView dv = FillterFootbalStatic(ds.Tables[0].DefaultView, (index == 0) ? teamCode1 : teamCode2);
                            if (dv.Count > 0)
                            {

                                teamName = (lang == Country.th.ToString()) ? (dv[0]["teamnameTH"].ToString() == "" ? dv[0]["teamname"].ToString() : dv[0]["teamnameTH"].ToString()) : dv[0]["teamname"].ToString();
                            }
                            else
                            {
                                teamName = "";
                            }
                            XElement xElement = new XElement("LeagueStatic", new XAttribute("teamName", teamName));
                            for (int i = 0; i < dv.Count; i++)
                            {
                                teamName2 = (lang == Country.th.ToString()) ? (dv[i]["isportteamname2"].ToString() == "" ? dv[i]["team_name2"].ToString() : dv[i]["isportteamname2"].ToString()) : dv[i]["team_name2"].ToString();
                                teamName1 = (lang == Country.th.ToString()) ? (dv[i]["isportteamname1"].ToString() == "" ? dv[i]["team_name1"].ToString() : dv[i]["isportteamname1"].ToString()) : dv[i]["team_name1"].ToString();
                                xElement.Add(new XElement("leaguestatic_team",
                                    new XAttribute("teamName2", teamName2)
                                    , new XAttribute("teamName1", teamName1)
                                    , new XAttribute("result_team1", dv[i]["score_home"].ToString())
                                    , new XAttribute("result_team2", dv[i]["score_away"].ToString())
                                    //, new XAttribute("static_team", dr["teamname"].ToString())
                                    ));
                            }
                            rtnXML.Element("SportApp").Add(xElement);
                        }
                    }
                    else
                    {
                        XElement xElement = new XElement("LeagueStatic");
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            teamName = (lang == Country.th.ToString()) ? (dr["teamnameTH"].ToString() == "" ? dr["teamname"].ToString() : dr["teamnameTH"].ToString()) : dr["teamname"].ToString();
                            teamName2 = (lang == Country.th.ToString()) ? (dr["isportteamname2"].ToString() == "" ? dr["team_name2"].ToString() : dr["isportteamname2"].ToString()) : dr["team_name2"].ToString();
                            teamName1 = (lang == Country.th.ToString()) ? (dr["isportteamname1"].ToString() == "" ? dr["team_name1"].ToString() : dr["isportteamname1"].ToString()) : dr["team_name1"].ToString();
                            xElement.Add(new XElement("leaguestatic_team",
                                new XAttribute("teamName2", teamName2)
                                , new XAttribute("teamName1", teamName1)
                                , new XAttribute("result_team1", dr["score_home"].ToString())
                                , new XAttribute("result_team2", dr["score_away"].ToString())
                                , new XAttribute("static_team", teamName)
                                , new XAttribute("team_id", (teamCode1 == dr["team_id1"].ToString() || teamCode1 == dr["team_id2"].ToString())? teamCode1 :  teamCode2)
                                , new XAttribute("tm_name", dr["tm_name"].ToString())
                                , new XAttribute("date", dr["date_txt"].ToString())
                                ));
                        }
                        rtnXML.Element("SportApp").Add(xElement);
                    }
                }
                #endregion

                rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", ""));
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetSportpoolAnalysisAllLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// CommandGetSportpoolAnalysis
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <param name="teamCode1"></param>
        /// <param name="teamCode2"></param>
        /// <param name="matchId"></param>
        /// <returns></returns>
        public XDocument CommandGetSportpoolAnalysis(string contestGroupId, string teamCode1, string teamCode2, string matchId, string lang,string type)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingHeader"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                string teamName1 = "", teamName2 = "",teamName="";
                #region Get วิเคราะห์ ,เอเชี่ยนแฮนดิแคป,ผลที่คาด
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballanalysis_detail"
                    , new SqlParameter[] { new SqlParameter("@match_id", matchId) });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        string analyse = dr["trendgame"].ToString().Split('|').Length > 0 ? dr["trendgame"].ToString().Split('|')[0] : "";
                        string result = dr["trendgame"].ToString().Split('|').Length == 2 ? dr["trendgame"].ToString().Split('|')[1] : "";
                        rtnXML.Element("SportApp").Add(
                            new XElement("analyse_detail"
                                , new XAttribute("analyse", analyse)
                                ,new XAttribute("handicap",dr["handicap_update_local"].ToString())
                                ,new XAttribute("result",result)
                                ));
                    }
                }
                #endregion

                if (type != "BB")
                {

                    #region Get Head to Head
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballanalysis_headtohead"
                        , new SqlParameter[] { new SqlParameter("@contestgroupid",contestGroupId)
                    ,new SqlParameter("@teamcode1",teamCode1)
                    ,new SqlParameter("@teamcode2",teamCode2)});
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int countWin1 = 0, countWin2 = 0
                            , countTie1 = 0, countTie2 = 0
                            , countLose1 = 0, countLose2 = 0
                            , countScore1 = 0, countScore2 = 0;

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (int.Parse(dr["result_team_code1"].ToString()) > int.Parse(dr["result_team_code2"].ToString()))
                            {
                                countWin1++;
                                countLose2++;
                            }
                            else if (int.Parse(dr["result_team_code1"].ToString()) == int.Parse(dr["result_team_code2"].ToString()))
                            {
                                countTie1++;
                                countTie2++;
                            }
                            else if (int.Parse(dr["result_team_code1"].ToString()) < int.Parse(dr["result_team_code2"].ToString()))
                            {
                                countWin2++;
                                countLose1++;
                            }
                            countScore1 += int.Parse(dr["result_team_code1"].ToString());
                            countScore2 += int.Parse(dr["result_team_code2"].ToString());

                        }
                        teamName1 = (lang == Country.th.ToString()) ? (ds.Tables[0].Rows[0]["teamname1th"].ToString() == "" ? ds.Tables[0].Rows[0]["teamname1en"].ToString() : ds.Tables[0].Rows[0]["teamname1th"].ToString()) : ds.Tables[0].Rows[0]["teamname1en"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (ds.Tables[0].Rows[0]["teamname2th"].ToString() == "" ? ds.Tables[0].Rows[0]["teamname2en"].ToString() : ds.Tables[0].Rows[0]["teamname2th"].ToString()) : ds.Tables[0].Rows[0]["teamname2en"].ToString();
                        rtnXML.Element("SportApp").Add(
                            new XElement("analyse_headtohead"
                                , new XElement("headtoHead_team"
                                    , new XAttribute("teamName1", teamName1 + "("+ds.Tables[0].Rows[0]["placeteam1"].ToString() +")")
                                    , new XAttribute("total_match", ds.Tables[0].Rows.Count)
                                    , new XAttribute("total_win", countWin1)
                                    , new XAttribute("total_draw", countTie1)
                                    , new XAttribute("total_loss", countLose1)
                                    , new XAttribute("total_gs", countScore1)
                                    , new XAttribute("total_ga", countScore2)
                                    )
                                , new XElement("headtohead_team"
                                    , new XAttribute("teamName2", teamName2 + "("+ds.Tables[0].Rows[0]["placeteam2"].ToString()+")")
                                    , new XAttribute("total_match", ds.Tables[0].Rows.Count)
                                    , new XAttribute("total_win", countWin2)
                                    , new XAttribute("total_draw", countTie2)
                                    , new XAttribute("total_loss", countLose2)
                                    , new XAttribute("total_gs", countScore2)
                                    , new XAttribute("total_ga", countScore1)
                                    )
                                ));
                    }
                    #endregion

                    #region Get football table
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballtablebyteam"
                        , new SqlParameter[] { new SqlParameter("@contestgroupid",contestGroupId)
                    ,new SqlParameter("@teamcode1",teamCode1)
                    ,new SqlParameter("@teamCode2",teamCode2)});
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        XElement xElement = new XElement("LeagueTable");
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            teamName1 = (lang == Country.th.ToString()) ? (dr["team_name_local"].ToString() == "" ? dr["team_name"].ToString() : dr["team_name_local"].ToString()) : dr["team_name"].ToString();
                            xElement.Add(new XElement("leaguetable_team"
                                        , new XAttribute("total_point", dr["total_point"].ToString())
                                        , new XAttribute("total_diff", dr["total_diff"].ToString())
                                        , new XAttribute("away_concede", dr["away_concede"].ToString())
                                        , new XAttribute("away_score", dr["away_score"].ToString())
                                        , new XAttribute("away_lost", dr["away_lost"].ToString())
                                        , new XAttribute("away_draws", dr["away_draws"].ToString())
                                        , new XAttribute("away_won", dr["away_won"].ToString())
                                        , new XAttribute("away_play", dr["away_play"].ToString())
                                        , new XAttribute("home_concede", dr["home_concede"].ToString())
                                        , new XAttribute("home_score", dr["home_score"].ToString())
                                        , new XAttribute("home_lost", dr["home_lost"].ToString())
                                        , new XAttribute("home_draws", dr["home_draws"].ToString())
                                        , new XAttribute("home_won", dr["home_won"].ToString())
                                        , new XAttribute("home_play", dr["home_play"].ToString())
                                        , new XAttribute("total_concede", dr["total_concede"].ToString())
                                        , new XAttribute("total_score", dr["total_score"].ToString())
                                        , new XAttribute("total_lost", dr["total_lost"].ToString())
                                        , new XAttribute("total_draws", dr["total_draws"].ToString())
                                        , new XAttribute("total_won", dr["total_won"].ToString())
                                        , new XAttribute("total_play", dr["total_play"].ToString())
                                        , new XAttribute("teamName1", teamName1)
                                        , new XAttribute("place", dr["place"].ToString())
                                        ));
                        }
                        rtnXML.Element("SportApp").Add(xElement);
                    }
                    #endregion

                    #region Get football static
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballstaticbymatch"
                        , new SqlParameter[] { new SqlParameter("@match_id",matchId)
                    ,new SqlParameter("@contestGroupId",contestGroupId)});
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (type.ToLower() == "iphone")
                        {
                            for (int index = 0; index < 2; index++)
                            {
                                DataView dv = FillterFootbalStatic(ds.Tables[0].DefaultView, (index == 0) ? teamCode1 : teamCode2);
                                if (dv.Count > 0)
                                {

                                    teamName = (lang == Country.th.ToString()) ? (dv[0]["teamnameTH"].ToString() == "" ? dv[0]["teamname"].ToString() : dv[0]["teamnameTH"].ToString()) : dv[0]["teamname"].ToString();
                                }
                                else
                                {
                                    teamName = "";
                                }
                                XElement xElement = new XElement("LeagueStatic", new XAttribute("teamName", teamName));
                                for (int i = 0; i < dv.Count; i++)
                                {
                                    teamName2 = (lang == Country.th.ToString()) ? (dv[i]["isportteamname2"].ToString() == "" ? dv[i]["team_name2"].ToString() : dv[i]["isportteamname2"].ToString()) : dv[i]["team_name2"].ToString();
                                    teamName1 = (lang == Country.th.ToString()) ? (dv[i]["isportteamname1"].ToString() == "" ? dv[i]["team_name1"].ToString() : dv[i]["isportteamname1"].ToString()) : dv[i]["team_name1"].ToString();
                                    xElement.Add(new XElement("leaguestatic_team",
                                        new XAttribute("teamName2", teamName2)
                                        , new XAttribute("teamName1", teamName1)
                                        , new XAttribute("result_team1", dv[i]["score_home"].ToString())
                                        , new XAttribute("result_team2", dv[i]["score_away"].ToString())
                                        //, new XAttribute("static_team", dr["teamname"].ToString())
                                        ));
                                }
                                rtnXML.Element("SportApp").Add(xElement);
                            }
                        }
                        else
                        {
                            XElement xElement = new XElement("LeagueStatic");
                            foreach(DataRow dr in ds.Tables[0].Rows )
                            {
                                teamName = (lang == Country.th.ToString()) ? (dr["teamnameTH"].ToString() == "" ? dr["teamname"].ToString() : dr["teamnameTH"].ToString()) : dr["teamname"].ToString();
                                teamName2 = (lang == Country.th.ToString()) ? (dr["isportteamname2"].ToString() == "" ? dr["team_name2"].ToString() : dr["isportteamname2"].ToString()) : dr["team_name2"].ToString();
                                teamName1 = (lang == Country.th.ToString()) ? (dr["isportteamname1"].ToString() == "" ? dr["team_name1"].ToString() : dr["isportteamname1"].ToString()) : dr["team_name1"].ToString();
                                xElement.Add(new XElement("leaguestatic_team",
                                    new XAttribute("teamName2", teamName2)
                                    , new XAttribute("teamName1", teamName1)
                                    , new XAttribute("result_team1", dr["score_home"].ToString())
                                    , new XAttribute("result_team2", dr["score_away"].ToString())
                                    , new XAttribute("static_team", teamName)
                                    ));
                            }
                            rtnXML.Element("SportApp").Add(xElement);
                        }
                    }
                    #endregion

                }

                rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", ""));
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("CommandGetSportpoolAnalysis >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// FillterFootbalStatic
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="teamCode"></param>
        /// <returns></returns>
        private DataView FillterFootbalStatic(DataView dv, string teamCode)
        {
            try
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
                str.Append(" team_id1='" + teamCode + "' or team_id2='" + teamCode + "'");
                dv.RowFilter = str.ToString();
                return dv;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// CommandGetLeagueTable
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <returns></returns>
        public XDocument CommandGetLeagueTable(string contestGroupId,string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                string teamName = "";

                #region Get football table
                string leagueName = "";
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballtable"
                    , new SqlParameter[] { new SqlParameter("@contestgroupid",contestGroupId)});
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    XElement xElement = null;
                    string img16 = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (leagueName != dr["tm_name"].ToString())
                        {
                            if(xElement != null )rtnXML.Element("SportApp").Add(xElement);
                            img16 = ds.Tables[0].Rows[0]["PIC_16X11"].ToString() == "" ? "default.png" : ds.Tables[0].Rows[0]["PIC_16X11"].ToString();
                            xElement = new XElement("LeagueTable"
                                , new XAttribute("tmName", (lang == Country.th.ToString()) ? dr["tm_name"].ToString() : dr["tm_name"].ToString())
                                , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                                );
                        }
                        teamName = (lang == Country.th.ToString()) ? (dr["team_name_local"].ToString() == "" ? dr["team_name"].ToString() : dr["team_name_local"].ToString()) : dr["team_name"].ToString();
                        xElement.Add(new XElement("leaguetable_team"
                                    , new XAttribute("total_point", dr["total_point"].ToString())
                                    , new XAttribute("total_diff", dr["total_diff"].ToString())
                                    , new XAttribute("away_concede", dr["away_concede"].ToString())
                                    , new XAttribute("away_score", dr["away_score"].ToString())
                                    , new XAttribute("away_lost", dr["away_lost"].ToString())
                                    , new XAttribute("away_draws", dr["away_draws"].ToString())
                                    , new XAttribute("away_won", dr["away_won"].ToString())
                                    , new XAttribute("away_play", dr["away_play"].ToString())
                                    , new XAttribute("home_concede", dr["home_concede"].ToString())
                                    , new XAttribute("home_score", dr["home_score"].ToString())
                                    , new XAttribute("home_lost", dr["home_lost"].ToString())
                                    , new XAttribute("home_draws", dr["home_draws"].ToString())
                                    , new XAttribute("home_won", dr["home_won"].ToString())
                                    , new XAttribute("home_play", dr["home_play"].ToString())
                                    , new XAttribute("total_concede", dr["total_concede"].ToString())
                                    , new XAttribute("total_score", dr["total_score"].ToString())
                                    , new XAttribute("total_lost", dr["total_lost"].ToString())
                                    , new XAttribute("total_draws", dr["total_draws"].ToString())
                                    , new XAttribute("total_won", dr["total_won"].ToString())
                                    , new XAttribute("total_play", dr["total_play"].ToString())
                                    , new XAttribute("teamName1", teamName)
                                    , new XAttribute("place", dr["place"].ToString())
                                    , new XAttribute("teamid", dr["team_id"].ToString())
                                    ));
                        leagueName = dr["tm_name"].ToString();
                    }
                    if( xElement != null )rtnXML.Element("SportApp").Add(xElement);
                }

                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }
                
                #endregion

                string remark = "หมายเหตุ : ";
                if (contestGroupId == "21" || contestGroupId == "116" || contestGroupId == "135" || contestGroupId == "85" || contestGroupId == "19")
                {
                    remark += string.Format(ConfigurationManager.AppSettings["remarkWording" + contestGroupId], new string[] { Convert.ToChar(13).ToString(), Convert.ToChar(13).ToString(), Convert.ToChar(13).ToString() });
                    remark += Convert.ToChar(13).ToString();
                }
                remark += ConfigurationManager.AppSettings["remarkWording"];
                rtnXML.Element("SportApp").Add(new XElement("remark", remark));
                rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", ""));
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetLeagueTable >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// CommandGetTeam
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <returns></returns>
        public XDocument CommandGetTeam(string contestGroupId, string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                string teamName = "";

                #region Get football table
                string leagueName = "";
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballtable"
                    , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId) });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    XElement xElement = null;
                    string img16 = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (leagueName != dr["tm_name"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            img16 = ds.Tables[0].Rows[0]["PIC_16X11"].ToString() == "" ? "default.png" : ds.Tables[0].Rows[0]["PIC_16X11"].ToString();
                            xElement = new XElement("LeagueTable"
                                , new XAttribute("tmName", (lang == Country.th.ToString()) ? dr["tm_name"].ToString() : dr["tm_name"].ToString())
                                , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                                );
                        }
                        teamName = (lang == Country.th.ToString()) ? (dr["team_name_local"].ToString() == "" ? dr["team_name"].ToString() : dr["team_name_local"].ToString()) : dr["team_name"].ToString();
                        xElement.Add(new XElement("leaguetable_team"
                                    , new XAttribute("team_id", dr["team_id"].ToString())
                                    , new XAttribute("teamName1", teamName)
                                    , new XAttribute("place", dr["place"].ToString())
                                    ));
                        leagueName = dr["tm_name"].ToString();
                    }
                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                }

                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }

                #endregion

                rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", ""));
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetLeagueTable >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }


        /// <summary>
        /// CommandGetLeagueTable
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <returns></returns>
        public XDocument CommandGetLeagueTable_CheerBallThai(XDocument rtnXML, string contestGroupId, string lang)
        {
            rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                string teamName = "";

                #region Get football table
                string spName = "", leagueName = "";
                spName = "usp_sportcc_getfootballtableCheerBallThai";
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, spName);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    XElement xElement = null;
                    string img16 = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (leagueName != dr["tm_name"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            img16 = ds.Tables[0].Rows[0]["PIC_16X11"].ToString() == "" ? "default.png" : ds.Tables[0].Rows[0]["PIC_16X11"].ToString();
                            xElement = new XElement("LeagueTable"
                                , new XAttribute("tmName", (lang == Country.th.ToString()) ? dr["class_name_local"].ToString() : dr["tm_name"].ToString())
                                , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                                );
                        }
                        teamName = (lang == Country.th.ToString()) ? (dr["team_name_local"].ToString() == "" ? dr["team_name"].ToString() : dr["team_name_local"].ToString()) : dr["team_name"].ToString();
                        xElement.Add(new XElement("leaguetable_team"
                                    , new XAttribute("total_point", dr["total_point"].ToString())
                                    , new XAttribute("total_diff", dr["total_diff"].ToString())
                                    , new XAttribute("away_concede", dr["away_concede"].ToString())
                                    , new XAttribute("away_score", dr["away_score"].ToString())
                                    , new XAttribute("away_lost", dr["away_lost"].ToString())
                                    , new XAttribute("away_draws", dr["away_draws"].ToString())
                                    , new XAttribute("away_won", dr["away_won"].ToString())
                                    , new XAttribute("away_play", dr["away_play"].ToString())
                                    , new XAttribute("home_concede", dr["home_concede"].ToString())
                                    , new XAttribute("home_score", dr["home_score"].ToString())
                                    , new XAttribute("home_lost", dr["home_lost"].ToString())
                                    , new XAttribute("home_draws", dr["home_draws"].ToString())
                                    , new XAttribute("home_won", dr["home_won"].ToString())
                                    , new XAttribute("home_play", dr["home_play"].ToString())
                                    , new XAttribute("total_concede", dr["total_concede"].ToString())
                                    , new XAttribute("total_score", dr["total_score"].ToString())
                                    , new XAttribute("total_lost", dr["total_lost"].ToString())
                                    , new XAttribute("total_draws", dr["total_draws"].ToString())
                                    , new XAttribute("total_won", dr["total_won"].ToString())
                                    , new XAttribute("total_play", dr["total_play"].ToString())
                                    , new XAttribute("teamName1", teamName)
                                    , new XAttribute("place", dr["place"].ToString())
                                    , new XAttribute("teamid", dr["team_id"].ToString())
                                    ));
                        leagueName = dr["tm_name"].ToString();
                    }
                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                }

                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }

                #endregion

                string remark = "หมายเหตุ : ";
                if (contestGroupId == "21" || contestGroupId == "116" || contestGroupId == "135" || contestGroupId == "85" || contestGroupId == "19")
                {
                    remark += string.Format(ConfigurationManager.AppSettings["remarkWording" + contestGroupId], new string[] { Convert.ToChar(13).ToString(), Convert.ToChar(13).ToString(), Convert.ToChar(13).ToString() });
                    remark += Convert.ToChar(13).ToString();
                }
                remark += ConfigurationManager.AppSettings["remarkWording"];
                rtnXML.Element("SportApp").Add(new XElement("remark", remark));
                rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", ""));
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetLeagueTable >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// CommandGetLeagueTable
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <returns></returns>
        public XDocument CommandGetLeagueTable_IsportStarSoccer( XDocument rtnXML,string contestGroupId, string lang)
        {
            rtnXML  = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                string teamName = "";

                #region Get football table
                string spName ="" ,leagueName = "";
                spName = (contestGroupId == "1100000057")? "usp_IsportStarSoccer_tablebycontestgroupid" : "usp_sportcc_getfootballtable";
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, spName
                    , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId) });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    XElement xElement = null;
                    string img16 = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (leagueName != dr["tm_id"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            img16 = ds.Tables[0].Rows[0]["PIC_16X11"].ToString() == "" ? "default.png" : ds.Tables[0].Rows[0]["PIC_16X11"].ToString();
                            xElement = new XElement("LeagueTable"
                                , new XAttribute("tmName", (lang == Country.th.ToString()) ? dr["class_name_local"].ToString() : dr["tm_name"].ToString())
                                , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                                );
                        }
                        teamName = (lang == Country.th.ToString()) ? (dr["team_name_local"].ToString() == "" ? dr["team_name"].ToString() : dr["team_name_local"].ToString()) : dr["team_name"].ToString();
                        xElement.Add(new XElement("leaguetable_team"
                                    , new XAttribute("total_point", dr["total_point"].ToString())
                                    , new XAttribute("total_diff", dr["total_diff"].ToString())
                                    , new XAttribute("away_concede", dr["away_concede"].ToString())
                                    , new XAttribute("away_score", dr["away_score"].ToString())
                                    , new XAttribute("away_lost", dr["away_lost"].ToString())
                                    , new XAttribute("away_draws", dr["away_draws"].ToString())
                                    , new XAttribute("away_won", dr["away_won"].ToString())
                                    , new XAttribute("away_play", dr["away_play"].ToString())
                                    , new XAttribute("home_concede", dr["home_concede"].ToString())
                                    , new XAttribute("home_score", dr["home_score"].ToString())
                                    , new XAttribute("home_lost", dr["home_lost"].ToString())
                                    , new XAttribute("home_draws", dr["home_draws"].ToString())
                                    , new XAttribute("home_won", dr["home_won"].ToString())
                                    , new XAttribute("home_play", dr["home_play"].ToString())
                                    , new XAttribute("total_concede", dr["total_concede"].ToString())
                                    , new XAttribute("total_score", dr["total_score"].ToString())
                                    , new XAttribute("total_lost", dr["total_lost"].ToString())
                                    , new XAttribute("total_draws", dr["total_draws"].ToString())
                                    , new XAttribute("total_won", dr["total_won"].ToString())
                                    , new XAttribute("total_play", dr["total_play"].ToString())
                                    , new XAttribute("teamName1", teamName)
                                    , new XAttribute("place", dr["place"].ToString())
                                    , new XAttribute("teamid", dr["team_id"].ToString())
                                    ));
                        leagueName = dr["tm_id"].ToString();
                    }
                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                }

                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }

                #endregion

                string remark = "หมายเหตุ : ";
                if (contestGroupId == "21" || contestGroupId == "116" || contestGroupId == "135" || contestGroupId == "85" || contestGroupId == "19")
                {
                    remark += string.Format(ConfigurationManager.AppSettings["remarkWording" + contestGroupId], new string[] { Convert.ToChar(13).ToString(), Convert.ToChar(13).ToString(), Convert.ToChar(13).ToString() });
                    remark += Convert.ToChar(13).ToString();
                }
                remark += ConfigurationManager.AppSettings["remarkWording"];
                rtnXML.Element("SportApp").Add(new XElement("remark", remark));
                rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", ""));
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetLeagueTable >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// CommandGetLeagueTable
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <returns></returns>
        public XDocument CommandGetLeagueTable_IStore(string contestGroupId, string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                string teamName = "";

                #region Get football table
                string leagueName = "";
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballtable"
                    , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId) });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    XElement xElement = null;
                    string img16 = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (leagueName != dr["tm_name"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            img16 = ds.Tables[0].Rows[0]["PIC_16X11"].ToString() == "" ? "default.png" : ds.Tables[0].Rows[0]["PIC_16X11"].ToString();
                            xElement = new XElement("LeagueTable"
                                , new XAttribute("tmName", (lang == Country.th.ToString()) ? dr["tm_name"].ToString() : dr["tm_name"].ToString())
                                , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                                );
                        }
                        teamName = (lang == Country.th.ToString()) ? (dr["team_name_local"].ToString() == "" ? dr["team_name"].ToString() : dr["team_name_local"].ToString()) : dr["team_name"].ToString();
                        xElement.Add(new XElement("team"
                                    , new XAttribute("point", dr["total_point"].ToString())
                                    , new XAttribute("gd", dr["total_diff"].ToString())
                                    , new XAttribute("total_ga", dr["total_concede"].ToString())
                                    , new XAttribute("total_gs", dr["total_score"].ToString())
                                    , new XAttribute("total_loss", dr["total_lost"].ToString())
                                    , new XAttribute("total_draw", dr["total_draws"].ToString())
                                    , new XAttribute("total_win", dr["total_won"].ToString())
                                    , new XAttribute("total_match", dr["total_play"].ToString())
                                    , new XAttribute("teamNameCode1", teamName)
                                    , new XAttribute("index", dr["place"].ToString())
                                    , new XAttribute("groupCode","")
                                    ));
                        leagueName = dr["tm_name"].ToString();
                    }
                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                }

                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }

                #endregion

                string remark = "หมายเหตุ : ";
                if (contestGroupId == "21" || contestGroupId == "116" || contestGroupId == "135" || contestGroupId == "85" || contestGroupId == "19")
                {
                    remark += string.Format(ConfigurationManager.AppSettings["remarkWording" + contestGroupId], new string[] { Convert.ToChar(13).ToString(), Convert.ToChar(13).ToString(), Convert.ToChar(13).ToString() });
                    remark += Convert.ToChar(13).ToString();
                }
                remark += ConfigurationManager.AppSettings["remarkWording"];
                rtnXML.Element("SportApp").Add(new XElement("remark", remark));
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetLeagueTable >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }
        #endregion

        #region Player Team
        public XDocument CommandGetPlayerTeam(XDocument rtnXML ,string teamId,string land)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getplayerteam"
                    , new SqlParameter[] {new SqlParameter("@team_id",teamId) });

                
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    rtnXML.Element("SportApp").Add(new XElement("team"));
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        rtnXML.Element("SportApp").Element("team").Add(new XElement("player"
                            , new XAttribute("player_id", dr["player_id"])
                            , new XAttribute("player_name", dr["player_name"])
                            , new XAttribute("player_lname", dr["player_lname"])
                            , new XAttribute("player_fname", dr["player_fname"])
                            , new XAttribute("country_name", dr["country_name"])
                            , new XAttribute("position", dr["position"])
                            , new XAttribute("shirt_number", dr["shirt_number"])
                            , new XAttribute("date_birth", dr["date_of_birth"])
                            , new XAttribute("height", dr["height"])
                            , new XAttribute("weight", dr["weight"])
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
                ExceptionManager.WriteError("CommandGetLeagueTable >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }
        #endregion

        #region Player Top Score
        public XDocument CommandGetPlayerTopScore(string contestGroupId, string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                string playerName = "", teamName = "",img16="";
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_GetSportClassbyFeedId"
                    , new SqlParameter[] { new SqlParameter("@feed_id",contestGroupId)});
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    img16 =ds.Tables[0].Rows[0]["PIC_16X11"].ToString() == "" ? "default.png" : ds.Tables[0].Rows[0]["PIC_16X11"].ToString();
                    rtnXML.Element("SportApp").Add(new XAttribute("tmName", (lang == Country.th.ToString()) ? ds.Tables[0].Rows[0]["class_name_local"].ToString() : ds.Tables[0].Rows[0]["class_name"].ToString())
                        , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XAttribute("tmName", "")
                        , new XAttribute("contestURLImages", ""));
                }
                ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getplayertopscore"
                    , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId) });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int index=1;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        playerName = lang == AppCode_Base.Country.th.ToString() ? 
                            dr["player_name_local"].ToString() == "" ? dr["player_name"].ToString() : dr["player_name_local"].ToString() 
                            : dr["player_name"].ToString();
                        teamName = lang == AppCode_Base.Country.th.ToString() ?
                            dr["team_name_local"].ToString() == "" ? dr["team_name"].ToString() : dr["team_name_local"].ToString()
                            : dr["team_name"].ToString();
                        rtnXML.Element("SportApp").Add(new XElement("Detail"
                            , new XAttribute("player",playerName)
                            ,new XAttribute("team",teamName)
                            ,new XAttribute("score",dr["goalcount"].ToString())
                            ,new XAttribute("no",index.ToString())
                            ));
                        index++;
                    }

                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }
                rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", ""));
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetLeagueTable >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }
        #endregion

        #region Football Statistic
        public XDocument CommandGetFootballStatistic(XDocument xDoc,string eleName,string teamCode,string lang)
        {
            string status = "Success", desc = "";
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapui_football_statistic"
                    , new SqlParameter[] {new SqlParameter("@teamCode",teamCode) });
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    XElement element = new XElement("Match", new XAttribute("msch_id", dr["msch_id"].ToString())
                                                                                    , new XAttribute("teamName1", dr["teamcode1th"].ToString())
                                                                                    , new XAttribute("teamName2", dr["teamcode2th"].ToString())
                                                                                    , new XAttribute("result", dr["result"].ToString())
                                                                                    , new XAttribute("resulttext", dr["resulttext"].ToString())
                                                                                    , new XAttribute("match_date", AppCode_LiveScore.DateTimeText(dr["match_date"].ToString()))
                                                                                    );
                    DataSet dsResult = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_livescore_result"
                       , new SqlParameter[] { new SqlParameter("@msch_id", dr["msch_id"].ToString()) });
                    foreach (DataRow drResult in dsResult.Tables[0].Rows)
                    {
                        element.Add(new XElement("MatchDetail"
                            , new XAttribute("playerName", drResult["MATCH_DETAIL"].ToString())
                            , new XAttribute("minute", drResult["TIME_RESULT"].ToString())
                            , new XAttribute("teamName", drResult["TEAM_NAME"].ToString())
                            , new XAttribute("evenType", ConvertEventType(drResult["TYPE_DETAIL"].ToString()))
                            ));
                    }

                    xDoc.Element(eleName).Add(element);
                }
            }
            catch(Exception ex)
            {
                status = "Error";
                desc = ex.Message;
                ExceptionManager.WriteError("CommandGetFootballStatistic >> " + ex.Message);
            }

            xDoc.Element(eleName).Add(new XElement("status", status)
                    , new XElement("message",desc));
            return xDoc;
        }
        public static string ConvertEventType(string type)
        {
            string rtn = type;
            switch (type)
            {
                case "G": rtn = "ประตู"; break;
                case "Y": rtn = "ใบเหลือง"; break;
                case "R": rtn = "ใบแดง"; break;
            }
            return rtn;
        }
        #endregion
    }
}
