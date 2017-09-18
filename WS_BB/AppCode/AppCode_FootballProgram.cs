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
    public class AppCode_FootballProgram : AppCode_Base
    {
        public class MatchLike
        {
            public float precentTeam1 = 0;
            public float precentTeam2 = 0;
            public float star = 1;
        }

        #region database isport 
        public struct Out_Program
        {
            public string status;
            public string errorMess;
            public List<MatchClass> program;
        }
        public class MatchClass
        {
            public string scsId;
            public string mschId;
            public string classNameEN;
            public string classNameTH;
            public string matchTime;
            public string teamName1TH;
            public string teamName2TH;
            public string teamName1EN;
            public string teamName2EN;
            public string liveChannel;
            public string matchDateTime;
            public string trendGame;
        }
        private DataView FillterProgramByScsId(DataView dv, string scsId,string matchDate)
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
            if (matchDate == "")
                str.Append("scs_id='" + scsId + "'");
            else
                str.Append("match_datetime='" + matchDate + "'");

            dv.RowFilter = str.ToString();
            return dv;
        }
        public Out_Program CommandGetFootballProgram(string scsId)
        {
            Out_Program outProgram = new Out_Program();
            List<MatchClass> mClass = new List<MatchClass>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_program"
                    ,new SqlParameter[]{new SqlParameter("@scs_id",scsId)});
                DataSet dsDetail = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_program_detail"
                    , new SqlParameter[] { new SqlParameter("@scs_id", scsId) });
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    MatchClass mc;
                    DataView dv = scsId ==""? FillterProgramByScsId(dsDetail.Tables[0].DefaultView, dr["scs_id"].ToString(),"")
                        : FillterProgramByScsId(dsDetail.Tables[0].DefaultView, "", dr["match_date"].ToString());
                    foreach (DataRowView drv in dv )
                    {
                        mc = new MatchClass();
                        mc.scsId = dr["scs_id"].ToString();
                        mc.classNameEN = dr["class_name_en"].ToString();
                        mc.classNameTH = dr["class_name_th"].ToString();
                        mc.mschId = drv["msch_id"].ToString();
                        mc.matchTime = drv["match_time"].ToString();
                        mc.teamName1EN = drv["teamName1_en"].ToString();
                        mc.teamName1TH = drv["teamName1_th"].ToString();
                        mc.teamName2EN = drv["teamName2_en"].ToString();
                        mc.teamName2TH = drv["teamName2_th"].ToString();
                        mc.liveChannel = drv["live_channel"].ToString();
                        mc.matchDateTime = AppCode_LiveScore.DatetoText( drv["match_datetime"].ToString().Substring(0,8));
                        mc.trendGame = drv["trendGame"].ToString();
                        mClass.Add(mc);
                    }
                }
                outProgram.program = mClass;
                outProgram.status = "Success";
            }
            catch(Exception ex)
            {
                outProgram.status = "Error";
                outProgram.errorMess = ex.Message;
                WebLibrary.ExceptionManager.WriteError(ex.Message);
            }
            return outProgram;
        }

        public XDocument CommandGetFootballProgram_Isportstarsoccer(XDocument rtnXML,string elementName,string date , string scsId, string lang)
        {
            XElement element = null ;
            string str = "";
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_program"
                    , new SqlParameter[] { new SqlParameter("@scs_id", scsId) });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataSet dsDetail = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_program_detail"
                        , new SqlParameter[] { new SqlParameter("@scs_id", scsId) });
                    XElement xElement = null;
                    string img16 = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (date == dr["match_date"].ToString())
                        {
                            DataView dv = scsId == "" ? FillterProgramByScsId(dsDetail.Tables[0].DefaultView, dr["scs_id"].ToString(), "")
                                : FillterProgramByScsId(dsDetail.Tables[0].DefaultView, "", dr["match_date"].ToString());

                            #region Add Header
                            str = (lang == AppCode_Base.Country.th.ToString()) ? ds.Tables[0].Rows[0]["class_name_th"].ToString() : ds.Tables[0].Rows[0]["class_name_en"].ToString();
                            element = new XElement(elementName, new XAttribute("contestGroupName", str)
                                , new XAttribute("contestGroupId", "")
                                , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + ds.Tables[0].Rows[0]["PIC_16X11"].ToString())
                                , new XAttribute("tmName", str)
                                , new XAttribute("tmSystem", "")
                                , new XAttribute("isDetail", "false"));
                            #endregion

                            foreach (DataRowView drv in dv)
                            {
                                string[] analysis = drv["analysis"].ToString().Split('|');

                                #region Add XML Detail
                                xElement = new XElement("Match",
                                    new XAttribute("mschId", drv["msch_id"].ToString())
                                    , new XAttribute("matchId", "")
                                    , new XAttribute("teamCode1", "") // id isportfeed
                                    , new XAttribute("teamCode2", "") // id isportfeed
                                    , new XAttribute("teamName1", (lang == Country.th.ToString()) ? drv["teamName1_th"].ToString() : drv["teamName1_en"].ToString())
                                    , new XAttribute("teamName2", (lang == Country.th.ToString()) ? drv["teamName2_th"].ToString() : drv["teamName2_en"].ToString())
                                    , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? drv["live_channel_local"].ToString() : drv["live_channel"].ToString())
                                    , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(drv["match_datetime"].ToString(), lang))
                                    , new XAttribute("matchTime", drv["match_time"].ToString())
                                    , new XAttribute("analysis", analysis.Length > 0 ? analysis[0] : "วิเคราะห์เกมส์ก่อนแข่ง 1 ชม.")
                                    , new XAttribute("analysis1", analysis.Length > 1 ? analysis[1] : "วิเคราะห์เกมส์ก่อนแข่ง 1 ชม.")
                                    , new XAttribute("isDetail", analysis.Length > 1 ? "true" : "false")
                                    );
                                #endregion

                                element.Add(xElement);
                            }
                            
                        }

                        rtnXML.Element("SportApp").Add(element);
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetFootballProgram >> " + ex.Message);
                //rtnXML.Element("SportApp").Add(new XElement("status", "error")
                //    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XDocument CommandGetFootballProgram_IsportPool(XDocument rtnXML,string elementName, string date, string scsId, string lang)
        {
            XElement element = null;
            string str = "";
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_program"
                    , new SqlParameter[] { new SqlParameter("@scs_id", scsId) });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataSet dsDetail = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_program_detail"
                        , new SqlParameter[] { new SqlParameter("@scs_id", scsId) });
                    XElement xElement = null;
                    string img16 = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (date == dr["match_date"].ToString())
                        {
                            DataView dv = scsId == "" ? FillterProgramByScsId(dsDetail.Tables[0].DefaultView, dr["scs_id"].ToString(), "")
                                : FillterProgramByScsId(dsDetail.Tables[0].DefaultView, "", dr["match_date"].ToString());

                            #region Add Header
                            str = (lang == AppCode_Base.Country.th.ToString()) ? ds.Tables[0].Rows[0]["class_name_th"].ToString() : ds.Tables[0].Rows[0]["class_name_en"].ToString();
                            element = new XElement(elementName, new XAttribute("contestGroupName", str)
                                , new XAttribute("contestGroupId", "")
                                , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + ds.Tables[0].Rows[0]["PIC_16X11"].ToString())
                                , new XAttribute("tmName", str)
                                , new XAttribute("tmSystem", "")
                                , new XAttribute("isDetail", "false"));
                            #endregion

                            foreach (DataRowView drv in dv)
                            {
                                string[] analysis = drv["analysis"].ToString().Split('|');
                                MatchLike matchLike = GetMatchLike(drv["msch_id"].ToString());
                                #region Add XML Detail
                                xElement = new XElement("Match",
                                    new XAttribute("mschId", drv["msch_id"].ToString())
                                    , new XAttribute("matchId", drv["msch_id"].ToString())
                                    , new XAttribute("teamCode1", drv["team_code1"].ToString()) // id isportfeed
                                    , new XAttribute("teamCode2", drv["team_code2"].ToString()) // id isportfeed
                                    , new XAttribute("teamName1", (lang == Country.th.ToString()) ? drv["teamName1_th"].ToString() : drv["teamName1_en"].ToString())
                                    , new XAttribute("teamName2", (lang == Country.th.ToString()) ? drv["teamName2_th"].ToString() : drv["teamName2_en"].ToString())
                                    , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? drv["live_channel_local"].ToString() : drv["live_channel"].ToString())
                                    , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(drv["match_datetime"].ToString(), lang))
                                    , new XAttribute("matchTime", drv["match_time"].ToString())
                                    , new XAttribute("analysis", analysis.Length > 0 ? analysis[0] : "วิเคราะห์เกมส์ก่อนแข่ง 1 ชม.")
                                    , new XAttribute("trend", analysis.Length > 1 ? analysis[1] : "")
                                    , new XAttribute("isDetail", analysis.Length > 1 ? "true" : "false")
                                    , new XAttribute("precentteam1", matchLike.precentTeam1.ToString())
                                    , new XAttribute("precentteam2", matchLike.precentTeam2.ToString())
                                    , new XAttribute("starlike", matchLike.star)
                                    );
                                #endregion

                                element.Add(xElement);
                            }

                        }

                        rtnXML.Element("SportApp").Add(element);
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetFootballProgram >> " + ex.Message);
                //rtnXML.Element("SportApp").Add(new XElement("status", "error")
                //    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        public XElement CommandGetFootballProgram(string elementName,string scsId,string lang)
        {
            XElement element = new XElement(elementName);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_program"
                    , new SqlParameter[] { new SqlParameter("@scs_id", scsId) });
                DataSet dsDetail = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_program_detail"
                    , new SqlParameter[] { new SqlParameter("@scs_id", scsId) });
                XElement xElement = null;
                string img16 = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DataView dv = scsId == "" ? FillterProgramByScsId(dsDetail.Tables[0].DefaultView, dr["scs_id"].ToString(), "")
                        : FillterProgramByScsId(dsDetail.Tables[0].DefaultView, "", dr["match_date"].ToString());

                    #region Add XML Header
                    img16 = dr["PIC_16X11"].ToString() == "" ? "default.png" : dr["PIC_16X11"].ToString();
                    xElement = new XElement("LeagueProgram",
                             new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dr["class_name_th"].ToString() : dr["class_name_en"].ToString())
                            , new XAttribute("contestGroupId", dr["scs_id"].ToString())
                            , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                            //, new XAttribute("tmName", dr["tm_name"].ToString())
                            //, new XAttribute("tmSystem", dr["tm_system"].ToString())
                            );
                    #endregion

                    foreach (DataRowView drv in dv)
                    {
                        string[] analysis = drv["analysis"].ToString().Split('|');
                        #region Add XML Detail
                        xElement.Add(
                            new XElement("MatchProgram",
                            new XAttribute("mschId", drv["msch_id"].ToString())
                            //, new XAttribute("teamCode1", dr["team_id1"].ToString()) // id isportfeed
                            //, new XAttribute("teamCode2", dr["team_id2"].ToString()) // id isportfeed
                            , new XAttribute("teamName1", (lang == Country.th.ToString()) ? drv["teamName1_th"].ToString() : drv["teamName1_en"].ToString())
                            , new XAttribute("teamName2", (lang == Country.th.ToString()) ? drv["teamName2_th"].ToString() : drv["teamName2_en"].ToString())
                            , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? drv["live_channel_local"].ToString() : drv["live_channel"].ToString())
                            , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(drv["match_datetime"].ToString(), lang))
                            , new XAttribute("matchTime", drv["match_time"].ToString())
                            , new XAttribute("analysis1", analysis.Length>0 ? analysis[0] : "วิเคราะห์เกมส์ก่อนแข่ง 1 ชม." )
                            , new XAttribute("analysis2", analysis.Length > 1 ? analysis[1] : "วิเคราะห์เกมส์ก่อนแข่ง 1 ชม.")
                            ));
                        #endregion
                    }
                    element.Add(xElement);
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetFootballProgram >> " + ex.Message);
                //rtnXML.Element("SportApp").Add(new XElement("status", "error")
                //    , new XElement("message", ex.Message));
            }
            return element;
        }

        public XDocument CommandGetFootballProgram_iStore(XDocument rtnXML, string scsId, string lang)
        {
            
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_program"
                    , new SqlParameter[] { new SqlParameter("@scs_id", scsId) });
                DataSet dsDetail = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_BB_football_program_detail"
                    , new SqlParameter[] { new SqlParameter("@scs_id", scsId) });
                XElement xElement = null;
                string img16 = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DataView dv = scsId == "" ? FillterProgramByScsId(dsDetail.Tables[0].DefaultView, dr["scs_id"].ToString(), "")
                        : FillterProgramByScsId(dsDetail.Tables[0].DefaultView, "", dr["match_date"].ToString());
        
                    #region Add XML Header
                    img16 = dr["PIC_16X11"].ToString() == "" ? "default.png" : dr["PIC_16X11"].ToString();
                        xElement = new XElement("League",
                                 new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dr["class_name_th"].ToString() : dr["class_name_en"].ToString())
                                , new XAttribute("contestGroupId", dr["scs_id"].ToString())
                                , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                            , new XAttribute("date", AppCode_LiveScore.DatetoText(dr["match_date"].ToString()))
                            //, new XAttribute("tmSystem", dr["tm_system"].ToString())
                                );
                  
                    #endregion

                    foreach (DataRowView drv in dv)
                    {
                        string[] analysis = drv["analysis"].ToString().Split('|');
                        #region Add XML Detail
                        xElement.Add(
                            new XElement("Match",
                            new XAttribute("mschId", drv["msch_id"].ToString())
                            , new XAttribute("matchId", "")
                            , new XAttribute("isDetail", "false")
                            //, new XAttribute("teamCode1", dr["team_id1"].ToString()) // id isportfeed
                            //, new XAttribute("teamCode2", dr["team_id2"].ToString()) // id isportfeed
                            , new XAttribute("teamName1", (lang == Country.th.ToString()) ? drv["teamName1_th"].ToString() : drv["teamName1_en"].ToString())
                            , new XAttribute("teamName2", (lang == Country.th.ToString()) ? drv["teamName2_th"].ToString() : drv["teamName2_en"].ToString())
                            , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? drv["live_channel_local"].ToString() : drv["live_channel"].ToString())
                            , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(drv["match_datetime"].ToString(), lang))
                            , new XAttribute("matchTime", drv["match_time"].ToString())
                            , new XAttribute("analysis1", analysis.Length > 0 ? analysis[0] : "วิเคราะห์เกมส์ก่อนแข่ง 1 ชม.")
                            , new XAttribute("analysis2", analysis.Length > 1 ? analysis[1] : "วิเคราะห์เกมส์ก่อนแข่ง 1 ชม.")
                            ));
                        #endregion
                    }

                    rtnXML.Element("SportApp").Add(xElement);
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetFootballProgram >> " + ex.Message);
                //rtnXML.Element("SportApp").Add(new XElement("status", "error")
                //    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        #endregion

        #region  Database IsportFeed

        /// <summary>
        /// FillterProgram
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="contestGroupId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        private DataView FillterProgram(DataView dv,string contestGroupId,string countryId)
        {
            try
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
                if (contestGroupId != null && contestGroupId != "")
                {
                    // CountryId
                    str.Append(" CONTESTGROUPID in (" + contestGroupId + ")");
                }
                else if (countryId != null && countryId != "")
                {
                    // contestGroupId
                    str.Append(" COUNTRY_ID in (" + countryId + ")");
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
        /// CommandGetProgramByTeam
        /// </summary>
        /// <param name="rtnXML"></param>
        /// <param name="teamid"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetProgramByTeam(XDocument rtnXML, string teamid, string lang)
        {
            try
            {
                DataSet ds = null;


                ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogrambyteam"
                            , new SqlParameter[] { new SqlParameter("@teamid", teamid) });


                
                bool isDetail = false, isLeageDetail = false;
                string contentGroupid = "", teamName1 = "", teamName2 = "";
                XElement xElement = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    // บอลไทย
                    //rtnXML.Element("SportApp").Add(CommandGetFootballProgram_Isportstarsoccer("League", date, "3405", lang));

                    //for (int index = 0; index < dv.Count; index++)
                    //{
                    #region Add XML

                    if (contentGroupid != dr["tm_id"].ToString())
                    {
                        if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                        string img16 = dr["PIC_48X48"].ToString() == "" ? "default.png" : dr["PIC_48X48"].ToString();
                        isLeageDetail = dr["contestgroupid"].ToString().IndexOf("21,85,116,135,19,13,22,89,817,427") > -1 ? true : false;
                        xElement = new XElement("League_Program",
                         new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dr["class_name_local"].ToString() == "" ? dr["contestgroup_name"].ToString() : dr["class_name_local"].ToString()) : dr["contestgroup_name"].ToString())
                        , new XAttribute("contestGroupId", dr["contestgroupid"].ToString())
                        , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString() + img16)
                        , new XAttribute("tmName", dr["tm_name"].ToString())
                        , new XAttribute("tmSystem", dr["tm_system"].ToString())
                        , new XAttribute("isDetail", isLeageDetail)
                        );

                    }

                    string analysis = dr["trendgame"].ToString().Split('|').Length > 0 ? dr["trendgame"].ToString().Split('|')[0] : "";
                    string trend = dr["trendgame"].ToString().Split('|').Length > 1 ? dr["trendgame"].ToString().Split('|')[1] : "";
                    isDetail = (dr["trendgame"].ToString() == "" && dr["handicap_update_local"].ToString() == "") ? false : true;
                    teamName1 = (lang == Country.th.ToString()) ? (dr["isportteamname1"].ToString() == "" ? dr["team_name1"].ToString() : dr["isportteamname1"].ToString()) : dr["team_name1"].ToString();
                    teamName2 = (lang == Country.th.ToString()) ? (dr["isportteamname2"].ToString() == "" ? dr["team_name2"].ToString() : dr["isportteamname2"].ToString()) : dr["team_name2"].ToString();
                    xElement.Add(
                        new XElement("Match_Program",
                        new XAttribute("mschId", dr["msch_id"].ToString())
                        , new XAttribute("matchId", dr["match_id"].ToString())
                        , new XAttribute("teamCode1", dr["team_id1"].ToString()) // id isportfeed
                        , new XAttribute("teamCode2", dr["team_id2"].ToString()) // id isportfeed
                        , new XAttribute("teamName1", teamName1 + dr["placeteam1"].ToString())
                        , new XAttribute("teamName2", teamName2 + dr["placeteam2"].ToString())
                        , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? dr["live_channel_local"].ToString() : dr["live_channel"].ToString())
                        , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dr["matchdate"].ToString(), lang))
                        , new XAttribute("matchTime", dr["matchtime"].ToString())
                        , new XAttribute("isDetail", isDetail)
                        , new XAttribute("analyse", analysis)
                        , new XAttribute("trend", trend)
                        ));

                    contentGroupid = dr["tm_id"].ToString();

                    #endregion
                    //}

                    

                   
                }
                if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย
                rtnXML.Element("SportApp").Add(
                       new XElement("status_program", "success")
                       , new XElement("message_program", ""));
                //else
                //{
                    //rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     //  , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                //}
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetProgramByLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status_program", "error")
                    , new XElement("message_program", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// CommandGetProgramByTeam
        /// </summary>
        /// <param name="rtnXML"></param>
        /// <param name="teamid"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetProgramByTeam_SportPool(XDocument rtnXML, string teamid, string lang)
        {
            try
            {
                DataSet ds = null;


                ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogrambyteam"
                            , new SqlParameter[] { new SqlParameter("@teamid", teamid) });



                bool isDetail = false, isLeageDetail = false;
                string contentGroupid = "", teamName1 = "", teamName2 = "";
                XElement xElement = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    // บอลไทย
                    //rtnXML.Element("SportApp").Add(CommandGetFootballProgram_Isportstarsoccer("League", date, "3405", lang));

                    //for (int index = 0; index < dv.Count; index++)
                    //{
                    #region Add XML

                    if (contentGroupid != dr["tm_id"].ToString())
                    {
                        if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                        string img16 = dr["PIC_48X48"].ToString() == "" ? "default.png" : dr["PIC_48X48"].ToString();
                        isLeageDetail = dr["contestgroupid"].ToString().IndexOf("21,85,116,135,19,13,22,89,817,427") > -1 ? true : false;
                        xElement = new XElement("League_Program",
                         new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dr["class_name_local"].ToString() == "" ? dr["contestgroup_name"].ToString() : dr["class_name_local"].ToString()) : dr["contestgroup_name"].ToString())
                        , new XAttribute("contestGroupId", dr["contestgroupid"].ToString())
                        , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString() + img16)
                        , new XAttribute("tmName", dr["tm_name"].ToString())
                        , new XAttribute("tmSystem", dr["tm_system"].ToString())
                        , new XAttribute("isDetail", isLeageDetail)
                        );

                    }
                    #region Match Like
                    MatchLike matchLike = GetMatchLike(dr["match_id"].ToString());

                    #endregion
                    string analysis = dr["trendgame"].ToString().Split('|').Length > 0 ? dr["trendgame"].ToString().Split('|')[0] : "";
                    string trend = dr["trendgame"].ToString().Split('|').Length > 1 ? dr["trendgame"].ToString().Split('|')[1] : "";
                    isDetail = (dr["trendgame"].ToString() == "" && dr["handicap_update_local"].ToString() == "") ? false : true;
                    teamName1 = (lang == Country.th.ToString()) ? (dr["isportteamname1"].ToString() == "" ? dr["team_name1"].ToString() : dr["isportteamname1"].ToString()) : dr["team_name1"].ToString();
                    teamName2 = (lang == Country.th.ToString()) ? (dr["isportteamname2"].ToString() == "" ? dr["team_name2"].ToString() : dr["isportteamname2"].ToString()) : dr["team_name2"].ToString();
                    xElement.Add(
                        new XElement("Match_Program",
                        new XAttribute("mschId", dr["msch_id"].ToString())
                        , new XAttribute("matchId", dr["match_id"].ToString())
                        , new XAttribute("teamCode1", dr["team_id1"].ToString()) // id isportfeed
                        , new XAttribute("teamCode2", dr["team_id2"].ToString()) // id isportfeed
                        , new XAttribute("teamName1", teamName1 + dr["placeteam1"].ToString())
                        , new XAttribute("teamName2", teamName2 + dr["placeteam2"].ToString())
                        , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? dr["live_channel_local"].ToString() : dr["live_channel"].ToString())
                        , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dr["matchdate"].ToString(), lang))
                        , new XAttribute("matchTime", dr["matchtime"].ToString())
                        , new XAttribute("isDetail", isDetail)
                        , new XAttribute("analyse", analysis)
                        , new XAttribute("trend", trend)
                        , new XAttribute("precentteam1", matchLike.precentTeam1)
                         , new XAttribute("precentteam2", matchLike.precentTeam2)
                         , new XAttribute("starlike", matchLike.star)
                        ));

                    contentGroupid = dr["tm_id"].ToString();

                    #endregion
                    //}




                }
                if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย
                rtnXML.Element("SportApp").Add(
                       new XElement("status_program", "success")
                       , new XElement("message_program", ""));
                //else
                //{
                //rtnXML.Element("SportApp").Add(new XElement("status", "success")
                //  , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                //}
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetProgramByLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status_program", "error")
                    , new XElement("message_program", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// CommandGetProgramByDate
        /// </summary>
        /// <param name="date">20120131</param>
        /// <param name="contestGroupId"></param>
        /// <param name="countryId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetProgramByDate(XDocument rtnXML,string date,string contestGroupId, string countryId, string lang)
        {
            try
            {
                rtnXML= CommandGetFootballProgram_Isportstarsoccer(rtnXML,"League", date, "", lang);

                rtnXML.Element("SportApp").Add(
                        new XElement("status", "success")
                        , new XElement("message", ""));
                /*

                DataSet ds = null;

                if (contestGroupId == null || contestGroupId == "")
                {
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogrambydate"
                            , new SqlParameter[] { new SqlParameter("@datetxt", date) });
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogram"
                            , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId.IndexOf(",") > 0 ? "" : contestGroupId) });
                }

                DataView dv = FillterProgram(ds.Tables[0].DefaultView, contestGroupId, countryId);
                bool isDetail = false, isLeageDetail = false;
                string contentGroupid = "", teamName1 = "", teamName2 = "";
                XElement xElement = null;

                // บอลไทย ไปใช้ที่ sportcc 
                

                if (dv.Count > 0)
                {
                    

                    for (int index = 0; index < dv.Count; index++)
                    {
                        #region Add XML

                        if (contentGroupid != dv[index]["tm_id"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            string img16 = dv[index]["PIC_48X48"].ToString() == "" ? "default.png" : dv[index]["PIC_48X48"].ToString();
                            isLeageDetail = dv[index]["contestgroupid"].ToString().IndexOf("21,85,116,135,19,13,22,89,817,427") > -1 ? true : false;
                            xElement = new XElement("League",
                             new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dv[index]["class_name_local"].ToString() == "" ? dv[index]["contestgroup_name"].ToString() : dv[index]["class_name_local"].ToString()) : dv[index]["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dv[index]["contestgroupid"].ToString())
                            , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString() + img16)
                            , new XAttribute("tmName", dv[index]["tm_name"].ToString())
                            , new XAttribute("tmSystem", dv[index]["tm_system"].ToString())
                            , new XAttribute("isDetail", isLeageDetail)
                            );

                        }

                        string analysis = dv[index]["trendgame"].ToString().Split('|').Length > 0 ? dv[index]["trendgame"].ToString().Split('|')[0] : "";
                        string trend = dv[index]["trendgame"].ToString().Split('|').Length > 1 ? dv[index]["trendgame"].ToString().Split('|')[1] : "";
                        isDetail = (dv[index]["trendgame"].ToString() == "" && dv[index]["handicap_update_local"].ToString() == "") ? false : true;
                        teamName1 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname1"].ToString() == "" ? dv[index]["team_name1"].ToString() : dv[index]["isportteamname1"].ToString()) : dv[index]["team_name1"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname2"].ToString() == "" ? dv[index]["team_name2"].ToString() : dv[index]["isportteamname2"].ToString()) : dv[index]["team_name2"].ToString();
                        xElement.Add(
                            new XElement("Match",
                            new XAttribute("mschId", dv[index]["msch_id"].ToString())
                            , new XAttribute("matchId", dv[index]["match_id"].ToString())
                            , new XAttribute("teamCode1", dv[index]["team_id1"].ToString()) // id isportfeed
                            , new XAttribute("teamCode2", dv[index]["team_id2"].ToString()) // id isportfeed
                            , new XAttribute("teamName1", teamName1 + dv[index]["placeteam1"].ToString())
                            , new XAttribute("teamName2", teamName2 + dv[index]["placeteam2"].ToString())
                            , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? dv[index]["live_channel_local"].ToString() : dv[index]["live_channel"].ToString())
                            , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dv[index]["matchdate"].ToString(), lang))
                            , new XAttribute("matchTime", dv[index]["matchtime"].ToString())
                            , new XAttribute("isDetail", isDetail)
                            , new XAttribute("analyse", analysis)
                            , new XAttribute("trend", trend)
                            ));

                        contentGroupid = dv[index]["tm_id"].ToString();

                        #endregion
                    }

                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย

                    
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ")); 
                }
                */
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetProgramByLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }


        /// <summary>
        /// CommandGetProgramByDate
        /// </summary>
        /// <param name="date">20120131</param>
        /// <param name="contestGroupId"></param>
        /// <param name="countryId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetProgramByDate_SportPool(XDocument rtnXML, string date, string contestGroupId, string countryId, string lang)
        {
            try
            {
                rtnXML= CommandGetFootballProgram_IsportPool(rtnXML,"League", date, "", lang);

                rtnXML.Element("SportApp").Add(
                      new XElement("status", "success")
                      , new XElement("message", ""));

                /*
                DataSet ds = null;

                if (contestGroupId == null || contestGroupId == "")
                {
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogrambydate"
                            , new SqlParameter[] { new SqlParameter("@datetxt", date) });
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogram"
                            , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId.IndexOf(",") > 0 ? "" : contestGroupId) });
                }

                DataView dv = FillterProgram(ds.Tables[0].DefaultView, contestGroupId, countryId);
                bool isDetail = false, isLeageDetail = false;
                string contentGroupid = "", teamName1 = "", teamName2 = "";
                XElement xElement = null;

                // บอลไทย ไปใช้ที่ sportcc
                


                if (dv.Count > 0)
                {
                    

                    for (int index = 0; index < dv.Count; index++)
                    {
                        #region Add XML

                        if (contentGroupid != dv[index]["tm_id"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            string img16 = dv[index]["PIC_48X48"].ToString() == "" ? "default.png" : dv[index]["PIC_48X48"].ToString();
                            isLeageDetail = dv[index]["contestgroupid"].ToString().IndexOf("21,85,116,135,19,13,22,89,817,427") > -1 ? true : false;
                            xElement = new XElement("League",
                             new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dv[index]["class_name_local"].ToString() == "" ? dv[index]["contestgroup_name"].ToString() : dv[index]["class_name_local"].ToString()) : dv[index]["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dv[index]["contestgroupid"].ToString())
                            , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon48x48"].ToString() + img16)
                            , new XAttribute("tmName", dv[index]["tm_name"].ToString())
                            , new XAttribute("tmSystem", dv[index]["tm_system"].ToString())
                            , new XAttribute("isDetail", isLeageDetail)
                            );

                        }

                        MatchLike matchLike = GetMatchLike(dv[index]["match_id"].ToString());
                        string analysis = dv[index]["trendgame"].ToString().Split('|').Length > 0 ? dv[index]["trendgame"].ToString().Split('|')[0] : "";
                        string trend = dv[index]["trendgame"].ToString().Split('|').Length > 1 ? dv[index]["trendgame"].ToString().Split('|')[1] : "";
                        isDetail = (dv[index]["trendgame"].ToString() == "" && dv[index]["handicap_update_local"].ToString() == "") ? false : true;
                        teamName1 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname1"].ToString() == "" ? dv[index]["team_name1"].ToString() : dv[index]["isportteamname1"].ToString()) : dv[index]["team_name1"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname2"].ToString() == "" ? dv[index]["team_name2"].ToString() : dv[index]["isportteamname2"].ToString()) : dv[index]["team_name2"].ToString();
                        xElement.Add(
                            new XElement("Match",
                            new XAttribute("mschId", dv[index]["msch_id"].ToString())
                            , new XAttribute("matchId", dv[index]["match_id"].ToString())
                            , new XAttribute("teamCode1", dv[index]["team_id1"].ToString()) // id isportfeed
                            , new XAttribute("teamCode2", dv[index]["team_id2"].ToString()) // id isportfeed
                            , new XAttribute("teamName1", teamName1 + dv[index]["placeteam1"].ToString())
                            , new XAttribute("teamName2", teamName2 + dv[index]["placeteam2"].ToString())
                            , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? dv[index]["live_channel_local"].ToString() : dv[index]["live_channel"].ToString())
                            , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dv[index]["matchdate"].ToString(), lang))
                            , new XAttribute("matchTime", dv[index]["matchtime"].ToString())
                            , new XAttribute("isDetail", isDetail)
                            , new XAttribute("analyse", analysis)
                            , new XAttribute("trend", trend)
                            , new XAttribute("precentteam1", matchLike.precentTeam1.ToString())
                            , new XAttribute("precentteam2", matchLike.precentTeam2.ToString())
                            , new XAttribute("starlike", matchLike.star)
                            ));

                        contentGroupid = dv[index]["tm_id"].ToString();

                        #endregion
                    }

                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย

                    rtnXML.Element("SportApp").Add(
                        new XElement("status", "success")
                        , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));

                   // rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    //  , new XElement("message", ""));
                }
                */
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommandGetProgramByLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }


        /// <summary>
        /// CommandGetProgramByLeague
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <param name="countryId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetProgramByLeague(string contestGroupId,string countryId,string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", "")
                ,new XAttribute("date",AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                DataSet ds = null;

                    if (contestGroupId == "")
                    {
                        ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogrambycountryId"
                            , new SqlParameter[] { new SqlParameter("@countryId", countryId.IndexOf(",") > 0 ? "" : countryId) });

                    }
                    else
                    {
                        ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogram"
                            , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId.IndexOf(",") > 0 ? "" : contestGroupId) });
                    }
                
                DataView dv = FillterProgram(ds.Tables[0].DefaultView, contestGroupId, countryId);
                bool isDetail = false, isLeageDetail = false;
                string contentGroupid = "", teamName1 = "", teamName2 = "" ;
                XElement xElement = null;
                if (dv.Count > 0)
                {
                    for (int index = 0; index < dv.Count; index++)
                    {
                        #region Add XML

                        if (contentGroupid != dv[index]["tm_id"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            string img16 = dv[index]["PIC_16X11"].ToString() == "" ? "default.png" : dv[index]["PIC_16X11"].ToString();
                            isLeageDetail = dv[index]["contestgroupid"].ToString().IndexOf("21,85,116,135,19,13,22,89,817,427") > -1 ? true : false;
                            xElement = new XElement("League",
                             new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dv[index]["class_name_local"].ToString() == "" ? dv[index]["contestgroup_name"].ToString() : dv[index]["class_name_local"].ToString()) : dv[index]["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dv[index]["contestgroupid"].ToString())
                            , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                            , new XAttribute("tmName", dv[index]["tm_name"].ToString())
                            , new XAttribute("tmSystem", dv[index]["tm_system"].ToString())
                            , new XAttribute("isDetail", isLeageDetail)
                            );

                        }

                        isDetail = (dv[index]["trendgame"].ToString() == "" && dv[index]["handicap_update_local"].ToString() == "") ? false : true;
                        teamName1 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname1"].ToString() == "" ? dv[index]["team_name1"].ToString() : dv[index]["isportteamname1"].ToString()) : dv[index]["team_name1"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname2"].ToString() == "" ? dv[index]["team_name2"].ToString() : dv[index]["isportteamname2"].ToString()) : dv[index]["team_name2"].ToString();
                        xElement.Add(
                            new XElement("Match",
                            new XAttribute("mschId", dv[index]["msch_id"].ToString())
                            , new XAttribute("matchId", dv[index]["match_id"].ToString())
                            , new XAttribute("teamCode1", dv[index]["team_id1"].ToString()) // id isportfeed
                            , new XAttribute("teamCode2", dv[index]["team_id2"].ToString()) // id isportfeed
                            , new XAttribute("teamName1", teamName1 + dv[index]["placeteam1"].ToString())
                            , new XAttribute("teamName2", teamName2 + dv[index]["placeteam2"].ToString())
                            , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? dv[index]["live_channel_local"].ToString() : dv[index]["live_channel"].ToString())
                            , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dv[index]["matchdate"].ToString(), lang))
                            , new XAttribute("matchTime", dv[index]["matchtime"].ToString())
                            , new XAttribute("isDetail", isDetail)
                            ));

                        contentGroupid = dv[index]["tm_id"].ToString();

                        #endregion
                    }

                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย

                    rtnXML.Element("SportApp").Add(
                        new XElement("status", "success")
                        , new XElement("message", ""));
                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("CommandGetProgramByLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// CommandGetProgramCheerBallThai
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <param name="countryId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetProgramCheerBallThai(string contestGroupId, string countryId, string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", "")
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                DataSet ds = null;


                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogramCheerBallThai");


                DataView dv = FillterProgram(ds.Tables[0].DefaultView, contestGroupId, countryId);
                bool isDetail = false, isLeageDetail = false;
                string contentGroupid = "", teamName1 = "", teamName2 = "";
                XElement xElement = null;
                if (dv.Count > 0)
                {
                    for (int index = 0; index < dv.Count; index++)
                    {
                        #region Add XML

                        if (contentGroupid != dv[index]["tm_id"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            string img16 = dv[index]["PIC_16X11"].ToString() == "" ? "default.png" : dv[index]["PIC_16X11"].ToString();
                            isLeageDetail = dv[index]["contestgroupid"].ToString().IndexOf("21,85,116,135,19,13,22,89,817,427") > -1 ? true : false;
                            xElement = new XElement("League",
                             new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dv[index]["class_name_local"].ToString() == "" ? dv[index]["contestgroup_name"].ToString() : dv[index]["class_name_local"].ToString()) : dv[index]["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dv[index]["contestgroupid"].ToString())
                            , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                            , new XAttribute("tmName", dv[index]["tm_name"].ToString())
                            , new XAttribute("tmSystem", dv[index]["tm_system"].ToString())
                            , new XAttribute("isDetail", isLeageDetail)
                            );

                        }

                        isDetail = (dv[index]["trendgame"].ToString() == "" && dv[index]["handicap_update_local"].ToString() == "") ? false : true;
                        teamName1 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname1"].ToString() == "" ? dv[index]["team_name1"].ToString() : dv[index]["isportteamname1"].ToString()) : dv[index]["team_name1"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname2"].ToString() == "" ? dv[index]["team_name2"].ToString() : dv[index]["isportteamname2"].ToString()) : dv[index]["team_name2"].ToString();
                        xElement.Add(
                            new XElement("Match",
                            new XAttribute("mschId", dv[index]["msch_id"].ToString())
                            , new XAttribute("matchId", dv[index]["match_id"].ToString())
                            , new XAttribute("teamCode1", dv[index]["team_id1"].ToString()) // id isportfeed
                            , new XAttribute("teamCode2", dv[index]["team_id2"].ToString()) // id isportfeed
                            , new XAttribute("teamName1", teamName1 + dv[index]["placeteam1"].ToString())
                            , new XAttribute("teamName2", teamName2 + dv[index]["placeteam2"].ToString())
                            , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? dv[index]["live_channel_local"].ToString() : dv[index]["live_channel"].ToString())
                            , new XAttribute("matchDate", (dv[index]["place1"].ToString() != "")? dv[index]["place1"].ToString() + " - " + dv[index]["place2"].ToString() : AppCode_LiveScore.DatetoText(dv[index]["matchdate"].ToString(), lang))
                            , new XAttribute("matchTime", (dv[index]["place1"].ToString() != "") ? "" :dv[index]["matchtime"].ToString())
                            , new XAttribute("isDetail", isDetail)
                            ));

                        contentGroupid = dv[index]["tm_id"].ToString();

                        #endregion
                    }

                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย

                    rtnXML.Element("SportApp").Add(
                        new XElement("status", "success")
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
                ExceptionManager.WriteError("CommandGetProgramByLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }


        /// <summary>
        /// CommandGetProgramByLeague_iStore
        /// </summary>
        /// <param name="rtnXML"></param>
        /// <param name="contestGroupId"></param>
        /// <param name="countryId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetProgramByLeague_iStore(XDocument rtnXML , string contestGroupId, string countryId, string lang)
        {
            try
            {
                DataSet ds =  SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogram_ccafe"
                        , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId) });

                bool isDetail = false;
                string matchDate = "", teamName1 = "", teamName2 = "";
                XElement xElement = null;
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        #region Add XML

                        if (matchDate != dr["matchdate"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            string img16 = dr["PIC_16X11"].ToString() == "" ? "default.png" : dr["PIC_16X11"].ToString();
                            xElement = new XElement("League",
                             new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dr["class_name_local"].ToString() == "" ? dr["contestgroup_name"].ToString() : dr["class_name_local"].ToString()) : dr["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dr["contestgroupid"].ToString())
                            , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                            , new XAttribute("date", AppCode_LiveScore.DatetoText(dr["matchdate"].ToString()))
                           // , new XAttribute("tmSystem", dr["tm_system"].ToString())
                            );

                        }
                        string[] analysis = dr["trendgame"].ToString().Split('|');
                        isDetail = (dr["trendgame"].ToString() == "" && dr["handicap_update_local"].ToString() == "") ? false : true;
                        teamName1 = (lang == Country.th.ToString()) ? (dr["isportteamname1"].ToString() == "" ? dr["team_name1"].ToString() : dr["isportteamname1"].ToString()) : dr["team_name1"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (dr["isportteamname2"].ToString() == "" ? dr["team_name2"].ToString() : dr["isportteamname2"].ToString()) : dr["team_name2"].ToString();
                        xElement.Add(
                            new XElement("Match",
                            new XAttribute("mschId", dr["msch_id"].ToString())
                            , new XAttribute("matchId", dr["match_id"].ToString())
                            //, new XAttribute("teamCode1", dr["team_id1"].ToString()) // id isportfeed
                            //, new XAttribute("teamCode2", dr["team_id2"].ToString()) // id isportfeed
                            , new XAttribute("teamName1", teamName1 + dr["placeteam1"].ToString())
                            , new XAttribute("teamName2", teamName2 + dr["placeteam2"].ToString())
                            , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? dr["live_channel_local"].ToString() : dr["live_channel"].ToString())
                            , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dr["matchdate"].ToString(), lang))
                            , new XAttribute("matchTime", dr["matchtime"].ToString())
                            , new XAttribute("isDetail", isDetail)
                             //, new XAttribute("analysis1", analysis.Length > 0 ? analysis[0] : "วิเคราะห์เกมส์ก่อนแข่ง 1 ชม.")
                            , new XAttribute("analysis1", analysis.Length > 1 ? analysis[1] : "")
                            ));

                        matchDate = dr["matchdate"].ToString();

                        #endregion
                    }

                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย


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
                ExceptionManager.WriteError("CommandGetProgramByLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// CommandGetProgramGroupByDate ใช้ที่ imobilegame
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <param name="countryId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetProgramGroupByDate(XDocument rtnXML,string contestGroupId, string countryId, string lang,string msisdn,string imei)
        {
            try
            {
                DataSet ds = null;

                msisdn = "66892037059";
                imei = "354016076692720";

                if (contestGroupId == "")
                {
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogrambycountryId"
                        , new SqlParameter[] { new SqlParameter("@countryId", countryId.IndexOf(",") > 0 ? "" : countryId) });

                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogram"
                        , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId.IndexOf(",") > 0 ? "" : contestGroupId) });
                }

                DataView dv = FillterProgram(ds.Tables[0].DefaultView, contestGroupId, countryId);
                DataSet dsAnwser = new DataSet();
                
                bool isDetail = false, isLeageDetail = false;
                string strDate = "", teamName1 = "", teamName2 = "",strAnswer="";
                XElement xElement = null;
                if (dv.Count > 0)
                {
                    for (int index = 0; index < dv.Count; index++)
                    {
                        #region Add XML

                        string ss = AppCode_LiveScore.DatetoText(dv[index]["matchdate"].ToString(), lang);
                        if (strDate != ss)
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            string img16 = dv[index]["PIC_16X11"].ToString() == "" ? "default.png" : dv[index]["PIC_16X11"].ToString();
                            isLeageDetail = dv[index]["contestgroupid"].ToString().IndexOf("21,85,116,135,19,13,22,89,817,427") > -1 ? true : false;
                            xElement = new XElement("League",
                             new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dv[index]["class_name_local"].ToString() == "" ? dv[index]["contestgroup_name"].ToString() : dv[index]["class_name_local"].ToString()) : dv[index]["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dv[index]["contestgroupid"].ToString())
                            , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                            , new XAttribute("tmName", dv[index]["tm_name"].ToString())
                            , new XAttribute("tmSystem", dv[index]["tm_system"].ToString())
                            , new XAttribute("isDetail", isLeageDetail)
                            , new XAttribute("matchDate", ss)
                            );

                            try {
                                dsAnwser = AppCode_iSoccer.GetGameAnswerResult(msisdn, imei, ss);
                            }
                            catch(Exception ex)
                            {
                                ExceptionManager.WriteError(ex.Message + " data " + msisdn + " " + imei + " " + ss);
                            }
                        }

                        if (dsAnwser.Tables.Count >0 && dsAnwser.Tables[0].Rows.Count > 0)
                        {
                            var anwsers = from a in dsAnwser.Tables[0].AsEnumerable()
                                          where a.Field<string>("msch_id") == dv[index]["msch_id"].ToString()
                                          select a;

                            //strAnswer = anwsers.ToArray()[0].ToString();
                            foreach (var anwser in anwsers)
                            {
                                strAnswer = anwser["answer"].ToString();
                            }
                        }

                        isDetail = (dv[index]["trendgame"].ToString() == "" && dv[index]["handicap_update_local"].ToString() == "") ? false : true;
                        teamName1 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname1"].ToString() == "" ? dv[index]["team_name1"].ToString() : dv[index]["isportteamname1"].ToString()) : dv[index]["team_name1"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (dv[index]["isportteamname2"].ToString() == "" ? dv[index]["team_name2"].ToString() : dv[index]["isportteamname2"].ToString()) : dv[index]["team_name2"].ToString();
                        xElement.Add(
                            new XElement("Match",
                            new XAttribute("mschId", dv[index]["msch_id"].ToString())
                            , new XAttribute("matchId", dv[index]["match_id"].ToString())
                            , new XAttribute("teamCode1", dv[index]["team_id1"].ToString()) // id isportfeed
                            , new XAttribute("teamCode2", dv[index]["team_id2"].ToString()) // id isportfeed
                            , new XAttribute("teamName1", teamName1 + dv[index]["placeteam1"].ToString())
                            , new XAttribute("teamName2", teamName2 + dv[index]["placeteam2"].ToString())
                            , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? dv[index]["live_channel_local"].ToString() : dv[index]["live_channel"].ToString())
                            , new XAttribute("matchDate", ss)
                            , new XAttribute("matchTime", dv[index]["matchtime"].ToString())
                            , new XAttribute("isDetail", isDetail)
                            , new XAttribute("anwser", strAnswer)
                            ));

                        strDate = ss;

                        #endregion
                    }

                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย

                    rtnXML.Element("SportApp").Add(
                        new XElement("status", "success")
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
                ExceptionManager.WriteError("CommandGetProgramByLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }


        /// <summary>
        /// CommandGetAnalysisByLeagueFeedAis
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <param name="countryId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetAnalysisByLeagueFeedAis(string contestGroupId, string countryId, string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingHeader"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                DataSet ds = null;
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogram"
                   , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId) });

                DataView dv = FillterIsDetail(ds.Tables[0].DefaultView);
                string contentGroupid = "", teamName1 = "", teamName2 = "";
                XElement xElement = null;
                if (dv.Count > 0)
                {
                    foreach (DataRowView dr in dv)
                    {
                        #region Add XML

                        if (contentGroupid != dr["tm_id"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            string img16 = dr["PIC_16X11"].ToString() == "" ? "default.png" : dr["PIC_16X11"].ToString();
                            xElement = new XElement("League",
                             new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dr["class_name_local"].ToString() == "" ? dr["contestgroup_name"].ToString() : dr["class_name_local"].ToString()) : dr["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dr["contestgroupid"].ToString())
                            , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                            , new XAttribute("tmName", dr["tm_name"].ToString())
                            , new XAttribute("tmSystem", dr["tm_system"].ToString())
                            );

                        }
                        teamName1 = (lang == Country.th.ToString()) ? (dr["isportteamname1"].ToString() == "" ? dr["team_name1"].ToString() : dr["isportteamname1"].ToString()) : dr["team_name1"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (dr["isportteamname2"].ToString() == "" ? dr["team_name2"].ToString() : dr["isportteamname2"].ToString()) : dr["team_name2"].ToString();
                        xElement.Add(
                            new XElement("Match",
                            new XAttribute("mschId", dr["msch_id"].ToString())
                            , new XAttribute("matchId", dr["match_id"].ToString())
                            , new XAttribute("teamCode1", dr["team_id1"].ToString()) // id isportfeed
                            , new XAttribute("teamCode2", dr["team_id2"].ToString()) // id isportfeed
                            , new XAttribute("teamName1", teamName1 + dr["placeteam1"].ToString())
                            , new XAttribute("teamName2", teamName2 + dr["placeteam2"].ToString())
                            , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? dr["live_channel_local"].ToString() : dr["live_channel"].ToString())
                            , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dr["matchdate"].ToString(), lang))
                            , new XAttribute("matchTime", dr["matchtime"].ToString())
                            , new XAttribute("analyse", dr["trendgame"].ToString())
                            ));

                        contentGroupid = dr["tm_id"].ToString();

                        #endregion
                    }

                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย

                    rtnXML.Element("SportApp").Add(
                    new XElement("status", "success")
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
                ExceptionManager.WriteError("CommandGetProgramByLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        ///  เพิ่ม attibute วิเคราะห์ เพิ่ม % like and star
        /// </summary>
        /// <param name="rtnXML"></param>
        /// <param name="contestGroupId"></param>
        /// <param name="countryId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetAnalysisByLeague_SportPool(XDocument rtnXML, string contestGroupId, string countryId, string lang)
        {
            try
            {
                DataSet ds = null;
                if (contestGroupId == "")
                {
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogrambycountryId"
                   , new SqlParameter[] { new SqlParameter("@countryId", countryId) });
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogram"
                   , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId) });
                }

                DataView dv = FillterIsDetail(ds.Tables[0].DefaultView);
                string contentGroupid = "", teamName1 = "", teamName2 = "", analysis = "";
                XElement xElement = null;
                if (dv.Count > 0)
                {
                    foreach (DataRowView dr in dv)
                    {
                        #region Add XML

                        if (contentGroupid != dr["tm_id"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            string img16 = dr["PIC_16X11"].ToString() == "" ? "default.png" : dr["PIC_16X11"].ToString();
                            xElement = new XElement("League",
                             new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dr["class_name_local"].ToString() == "" ? dr["contestgroup_name"].ToString() : dr["class_name_local"].ToString()) : dr["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dr["contestgroupid"].ToString())
                            , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                            , new XAttribute("tmName", dr["tm_name"].ToString())
                            , new XAttribute("tmSystem", dr["tm_system"].ToString())
                            );

                        }

                        #region Match Like 
                        MatchLike matchLike = GetMatchLike(dr["match_id"].ToString());
                        
                        #endregion

                        teamName1 = (lang == Country.th.ToString()) ? (dr["isportteamname1"].ToString() == "" ? dr["team_name1"].ToString() : dr["isportteamname1"].ToString()) : dr["team_name1"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (dr["isportteamname2"].ToString() == "" ? dr["team_name2"].ToString() : dr["isportteamname2"].ToString()) : dr["team_name2"].ToString();
                        analysis = dr["trendgame"].ToString().Split('|').Length > 0 ? dr["trendgame"].ToString().Split('|')[0] : "";
                        string trend = dr["trendgame"].ToString().Split('|').Length > 1 ? dr["trendgame"].ToString().Split('|')[1] : "";
                        xElement.Add(
                            new XElement("Match",
                            new XAttribute("mschId", dr["msch_id"].ToString())
                            , new XAttribute("matchId", dr["match_id"].ToString())
                            , new XAttribute("teamCode1", dr["team_id1"].ToString()) // id isportfeed
                            , new XAttribute("teamCode2", dr["team_id2"].ToString()) // id isportfeed
                            , new XAttribute("teamName1", teamName1 + dr["placeteam1"].ToString())
                            , new XAttribute("teamName2", teamName2 + dr["placeteam2"].ToString())
                            , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? dr["live_channel_local"].ToString() : dr["live_channel"].ToString())
                            , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dr["matchdate"].ToString(), lang))
                            , new XAttribute("matchTime", dr["matchtime"].ToString())
                            , new XAttribute("isDetail", true)
                            , new XAttribute("analysis", analysis)
                            , new XAttribute("trend", trend)
                            , new XAttribute("precentteam1", matchLike.precentTeam1)
                            , new XAttribute("precentteam2", matchLike.precentTeam2)
                            , new XAttribute("starlike", matchLike.star)
                            ));

                        contentGroupid = dr["tm_id"].ToString();

                        #endregion
                    }

                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย

                    rtnXML.Element("SportApp").Add(
                    new XElement("status", "success")
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
                ExceptionManager.WriteError("CommandGetProgramByLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        ///  เพิ่ม attibute วิเคราะห์
        /// </summary>
        /// <param name="rtnXML"></param>
        /// <param name="contestGroupId"></param>
        /// <param name="countryId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetAnalysisByLeague(XDocument rtnXML,string contestGroupId, string countryId, string lang)
        {
            try
            {
                DataSet ds = null;
                if (contestGroupId == "")
                {
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogrambycountryId_ss"
                   , new SqlParameter[] { new SqlParameter("@countryId", countryId) });
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogram_ss"
                   , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId) });
                }

                DataView dv = FillterIsDetail(ds.Tables[0].DefaultView);
                string contentGroupid = "", teamName1 = "", teamName2 = "",analysis="";
                XElement xElement = null;
                if (dv.Count > 0)
                {
                    foreach (DataRowView dr in dv)
                    {
                        #region Add XML

                        if (contentGroupid != dr["tm_id"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            string img16 = dr["PIC_16X11"].ToString() == "" ? "default.png" : dr["PIC_16X11"].ToString();
                            xElement = new XElement("League",
                             new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dr["class_name_local"].ToString() == "" ? dr["contestgroup_name"].ToString() : dr["class_name_local"].ToString()) : dr["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dr["contestgroupid"].ToString())
                            , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                            , new XAttribute("tmName", dr["tm_name"].ToString())
                            , new XAttribute("tmSystem", dr["tm_system"].ToString())
                            );

                        }
                        teamName1 = (lang == Country.th.ToString()) ? (dr["isportteamname1"].ToString() == "" ? dr["team_name1"].ToString() : dr["isportteamname1"].ToString()) : dr["team_name1"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (dr["isportteamname2"].ToString() == "" ? dr["team_name2"].ToString() : dr["isportteamname2"].ToString()) : dr["team_name2"].ToString();
                        analysis = dr["trendgame"].ToString().Split('|').Length > 0 ? dr["trendgame"].ToString().Split('|')[0] : "";
                        string trend = dr["trendgame"].ToString().Split('|').Length > 1 ? dr["trendgame"].ToString().Split('|')[1] : "";
                        xElement.Add(
                            new XElement("Match",
                            new XAttribute("mschId", dr["msch_id"].ToString())
                            , new XAttribute("matchId", dr["match_id"].ToString())
                            , new XAttribute("teamCode1", dr["team_id1"].ToString()) // id isportfeed
                            , new XAttribute("teamCode2", dr["team_id2"].ToString()) // id isportfeed
                            , new XAttribute("teamName1", teamName1 + dr["placeteam1"].ToString())
                            , new XAttribute("teamName2", teamName2 + dr["placeteam2"].ToString())
                            , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? dr["live_channel_local"].ToString() : dr["live_channel"].ToString())
                            , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dr["matchdate"].ToString(), lang))
                            , new XAttribute("matchTime", dr["matchtime"].ToString())
                            , new XAttribute("isDetail", true)
                            , new XAttribute("analysis", analysis)
                            , new XAttribute("trendgame", trend)
                            ));

                        contentGroupid = dr["tm_id"].ToString();

                        #endregion
                    }

                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย

                    rtnXML.Element("SportApp").Add(
                    new XElement("status", "success")
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
                ExceptionManager.WriteError("CommandGetProgramByLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// CommandGetAnalysisByLeague_test
        /// </summary>
        /// <param name="rtnXML"></param>
        /// <param name="contestGroupId"></param>
        /// <param name="countryId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetAnalysisByLeague_test(XDocument rtnXML, string contestGroupId, string countryId, string lang)
        {
            try
            {
                DataSet ds = null;
                if (contestGroupId == "")
                {
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogrambycountryId_test"
                   , new SqlParameter[] { new SqlParameter("@countryId", countryId) });
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogram"
                   , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId) });
                }

                DataView dv = FillterIsDetail(ds.Tables[0].DefaultView);
                string contentGroupid = "", teamName1 = "", teamName2 = "", analysis = "";
                XElement xElement = null;
                if (dv.Count > 0)
                {
                    foreach (DataRowView dr in dv)
                    {
                        #region Add XML

                        if (contentGroupid != dr["tm_id"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            string img16 = dr["PIC_16X11"].ToString() == "" ? "default.png" : dr["PIC_16X11"].ToString();
                            xElement = new XElement("League",
                             new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dr["class_name_local"].ToString() == "" ? dr["contestgroup_name"].ToString() : dr["class_name_local"].ToString()) : dr["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dr["contestgroupid"].ToString())
                            , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                            , new XAttribute("tmName", dr["tm_name"].ToString())
                            , new XAttribute("tmSystem", dr["tm_system"].ToString())
                            );

                        }
                        teamName1 = (lang == Country.th.ToString()) ? (dr["isportteamname1"].ToString() == "" ? dr["team_name1"].ToString() : dr["isportteamname1"].ToString()) : dr["team_name1"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (dr["isportteamname2"].ToString() == "" ? dr["team_name2"].ToString() : dr["isportteamname2"].ToString()) : dr["team_name2"].ToString();
                        analysis = dr["trendgame"].ToString().Split('|').Length > 0 ? dr["trendgame"].ToString().Split('|')[0] : "";
                        string trend = dr["trendgame"].ToString().Split('|').Length > 1 ? dr["trendgame"].ToString().Split('|')[1] : "";
                        xElement.Add(
                            new XElement("Match",
                            new XAttribute("mschId", dr["msch_id"].ToString())
                            , new XAttribute("matchId", dr["match_id"].ToString())
                            , new XAttribute("teamCode1", dr["team_id1"].ToString()) // id isportfeed
                            , new XAttribute("teamCode2", dr["team_id2"].ToString()) // id isportfeed
                            , new XAttribute("teamName1", teamName1 + dr["placeteam1"].ToString())
                            , new XAttribute("teamName2", teamName2 + dr["placeteam2"].ToString())
                            , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? dr["live_channel_local"].ToString() : dr["live_channel"].ToString())
                            , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dr["matchdate"].ToString(), lang))
                            , new XAttribute("matchTime", dr["matchtime"].ToString())
                            , new XAttribute("isDetail", true)
                            , new XAttribute("analysis", analysis)
                             , new XAttribute("trend", trend)
                            ));

                        contentGroupid = dr["tm_id"].ToString();

                        #endregion
                    }

                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย

                    rtnXML.Element("SportApp").Add(
                    new XElement("status", "success")
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
                ExceptionManager.WriteError("CommandGetProgramByLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        ///  CommandGetAnalysisByLeague
        /// </summary>
        /// <param name="rtnXML"></param>
        /// <param name="contestGroupId"></param>
        /// <param name="countryId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public XDocument CommandGetAnalysisByLeague(string contestGroupId, string countryId, string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingHeader"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                DataSet ds = null;
                if (contestGroupId == "")
                {
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogrambycountryId"
                   , new SqlParameter[] { new SqlParameter("@countryId", countryId) });
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogram"
                   , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId) });
                }

                DataView dv = FillterIsDetail(ds.Tables[0].DefaultView);
                string contentGroupid = "", teamName1 = "", teamName2 = "";
                XElement xElement = null;
                if (dv.Count > 0)
                {
                    foreach (DataRowView dr in dv)
                    {
                        #region Add XML

                        if (contentGroupid != dr["tm_id"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            string img16 = dr["PIC_16X11"].ToString() == "" ? "default.png" : dr["PIC_16X11"].ToString();
                            xElement = new XElement("League",
                             new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? (dr["class_name_local"].ToString() == "" ? dr["contestgroup_name"].ToString() : dr["class_name_local"].ToString()) : dr["contestgroup_name"].ToString())
                            , new XAttribute("contestGroupId", dr["contestgroupid"].ToString())
                            , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                            , new XAttribute("tmName", dr["tm_name"].ToString())
                            , new XAttribute("tmSystem", dr["tm_system"].ToString())
                            );

                        }
                        teamName1 = (lang == Country.th.ToString()) ? (dr["isportteamname1"].ToString() == "" ? dr["team_name1"].ToString() : dr["isportteamname1"].ToString()) : dr["team_name1"].ToString();
                        teamName2 = (lang == Country.th.ToString()) ? (dr["isportteamname2"].ToString() == "" ? dr["team_name2"].ToString() : dr["isportteamname2"].ToString()) : dr["team_name2"].ToString();
                        xElement.Add(
                            new XElement("Match",
                            new XAttribute("mschId", dr["msch_id"].ToString())
                            , new XAttribute("matchId", dr["match_id"].ToString())
                            , new XAttribute("teamCode1", dr["team_id1"].ToString()) // id isportfeed
                            , new XAttribute("teamCode2", dr["team_id2"].ToString()) // id isportfeed
                            , new XAttribute("teamName1", teamName1 + dr["placeteam1"].ToString())
                            , new XAttribute("teamName2", teamName2 + dr["placeteam2"].ToString())
                            , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? dr["live_channel_local"].ToString() : dr["live_channel"].ToString())
                            , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dr["matchdate"].ToString(), lang))
                            , new XAttribute("matchTime", dr["matchtime"].ToString())
                            , new XAttribute("isDetail", true)
                            ));

                        contentGroupid = dr["tm_id"].ToString();

                        #endregion
                    }

                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย

                    rtnXML.Element("SportApp").Add(
                    new XElement("status", "success")
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
                ExceptionManager.WriteError("CommandGetProgramByLeague >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }

        /// <summary>
        /// FillterIsDetail
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        private DataView FillterIsDetail(DataView dv)
        {
            try
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
                str.Append(" trendgame <>'' and handicap_update_local <>'' ");
                dv.RowFilter = str.ToString();
                return dv;
            }
            catch(Exception ex)
            {
                throw new Exception("FillterIsDetail>> " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        private MatchLike GetMatchLike(string matchId)
        {
            try
            {
                string err = "";
                MatchLike matchLike = new MatchLike();
                DataSet  dsMatchLike =  OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_TEST.ISPORTAPP_Select_MatchLike", "isportapp_match_like",
                                new OracleParameter[] { OrclHelper.GetOracleParameter("p_MSCHID",matchId,OracleType.VarChar,ParameterDirection.Input)
                                                                    ,OrclHelper.GetOracleParameter("o_cursor","",OracleType.Cursor,ParameterDirection.Output)});

                if (dsMatchLike.Tables.Count > 0 && dsMatchLike.Tables[0].Rows.Count > 0)
                {

                    matchLike.precentTeam1 = (dsMatchLike.Tables[0].Rows[0]["percen1"] == null || dsMatchLike.Tables[0].Rows[0]["percen1"].ToString().Trim()  == "") ? new Random().Next(10, 80) : float.Parse(dsMatchLike.Tables[0].Rows[0]["percen1"].ToString());
                    matchLike.precentTeam2 = (dsMatchLike.Tables[0].Rows[0]["percen2"] == null || dsMatchLike.Tables[0].Rows[0]["percen2"].ToString().Trim() == "") ? 100 - matchLike.precentTeam1 : float.Parse(dsMatchLike.Tables[0].Rows[0]["percen2"].ToString());
                    matchLike.star = (dsMatchLike.Tables[0].Rows[0]["star"] == null || dsMatchLike.Tables[0].Rows[0]["star"].ToString().Trim() == "") ? new Random().Next(1, 5) : float.Parse(dsMatchLike.Tables[0].Rows[0]["star"].ToString());
                }
                else
                {
                    matchLike.precentTeam1 = new Random().Next(10, 80);
                    matchLike.precentTeam2 = 100 - matchLike.precentTeam1;
                    matchLike.star = new Random().Next(1, 5);
                }
                return matchLike;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + matchId);
            }
        }

        #endregion

        #region Database Oracle
        public XDocument Command_GetProgramByMonth( string contestGroupId, string year, string month, string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {

                    if (DateTime.Now.Month > 0 && DateTime.Now.Month < 7)
                    {
                        if (int.Parse(month) > 6 && int.Parse(month) < 13) year = (DateTime.Now.Year - 1).ToString();
                        else year = DateTime.Now.Year.ToString();
                    }
                    else
                    {
                        if (int.Parse(month) > 6 && int.Parse(month) < 13) year = DateTime.Now.Year.ToString() ;
                        else year = (DateTime.Now.Year+1).ToString();
                    }

                    DataSet ds = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetMatchSchedule", "SIAMSPORT_NEWS",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_contestgroupid", contestGroupId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_month", month, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("p_year", year, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                    
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        XElement element = null;
                        string matchDate = "";
                        string img16 = ds.Tables[0].Rows[0]["PIC_16X11"].ToString() == "" ? "default.png" : ds.Tables[0].Rows[0]["PIC_16X11"].ToString();
                        rtnXML.Element("SportApp").Add(new XAttribute("leagueName", (lang == Country.th.ToString()) ? ds.Tables[0].Rows[0]["league_name"].ToString() : ds.Tables[0].Rows[0]["league_name_en"].ToString())
                            , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16));
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (matchDate != dr["match_date"].ToString())
                            {
                                if (element != null) rtnXML.Element("SportApp").Add(element);
                                element = new XElement("Program", new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dr["match_date"].ToString(), lang)));
                            }
                            element.Add(
                                    new XElement("Match", new XAttribute("matchId", dr["match_id"].ToString())
                                    , new XAttribute("teamCode1", dr["team_id1"].ToString()) // id siamsport
                                    , new XAttribute("teamCode2", dr["team_id2"].ToString()) // id siamsport
                                    , new XAttribute("teamName1", lang == Country.en.ToString() ? (dr["team_name1_en"].ToString() == "" ? dr["team_name1_th"].ToString() : dr["team_name1_en"].ToString()) : dr["team_name1_th"].ToString())
                                    , new XAttribute("teamName2", lang == Country.en.ToString() ? (dr["team_name2_en"].ToString() == "" ? dr["team_name2_th"].ToString() : dr["team_name2_en"].ToString() ) : dr["team_name2_th"].ToString())
                                    , new XAttribute("liveChannel", dr["channel"].ToString())
                                    //, new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dr["match_date"].ToString(), lang))
                                    , new XAttribute("matchTime", dr["match_time"].ToString())
                                    , new XAttribute("matchStatus", dr["match_status"].ToString())
                                    , new XAttribute("scoreTeam1", dr["score_ft_team1"].ToString())
                                    , new XAttribute("scoreTeam2", dr["score_ft_team2"].ToString())
                                    ));
                            matchDate = dr["match_date"].ToString();
                        }
                        if (element != null) rtnXML.Element("SportApp").Add(element);
                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                     , new XElement("message", ""));
                    }
                    else
                    {
                        DataSet dsClass = OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetSportClass", "i_sport_class",
                            new OracleParameter[] { OrclHelper.GetOracleParameter("p_contestgroupid", contestGroupId, OracleType.VarChar, ParameterDirection.Input)
                                ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                            });
                        if (dsClass.Tables.Count > 0 && dsClass.Tables[0].Rows.Count > 0)
                        {
                            string img16 = dsClass.Tables[0].Rows[0]["PIC_16X11"].ToString() == "" ? "default.png" : dsClass.Tables[0].Rows[0]["PIC_16X11"].ToString();
                            rtnXML.Element("SportApp").Add(new XAttribute("leagueName", dsClass.Tables[0].Rows[0]["class_name"].ToString())
                                , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16));

                        }
                        rtnXML.Element("SportApp").Add(new XElement("status", "success")
                       , new XElement("message", "ขอภัยไม่พบข้อมูลที่ต้องการ"));
                    }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Command_GetProgramByMonth >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }

            return rtnXML;
        }
        #endregion
    }
}