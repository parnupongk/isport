using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using WebLibrary;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Data;
using OracleDataAccress;
namespace WS_BB
{
    public partial class imobilegame : feedMain
    {

        public override string CheckPageName(string pageName, string appName)
        {
            string rtn = "";
            if (appName == AppCode_Base.AppName.iSoccer.ToString())
            {
                if (pageName == "DataMatch")
                {
                    rtn = GamePostData(Request["datamatch"], Request["imei"], Request["date"], appName);
                }
                else if (pageName == "Main")
                {
                    rtn = GetProgramGroupdate(appName, Request["date"], Request["contentgroupid"], Request["sporttype"], Request["lang"], Request["type"], Request["imei"]);
                }
                else if (pageName == "News")
                {
                    rtn = GetNewsByDate(appName, Request["contentgroupid"], Request["date"], "", Request["type"], Request["sporttype"], Request["lang"]);
                }
                else if (pageName == "Result")
                {
                    rtn = GetResultiSoccer(appName, Request["contentgroupid"], Request["date"], Request["sporttype"], Request["lang"], Request["imei"]);
                }
                else if (pageName == "Rules")
                {
                    rtn = GetRules();
                }
                else if (pageName == "Awards")
                {
                    rtn = GetAwards();
                }
                else if (pageName == "Update")
                {
                    rtn = UpdateCustomer(Request["imei"], Request["model"], appName);
                }
                else if (pageName == "Ranking")
                {
                    rtn = GetRanking();
                }
            }
            else if (appName == AppCode_Base.AppName.CheerBallThai.ToString())
            {
                if (pageName == "Main") rtn = GetContentMain(Request["date"], Request["imei"],appName,muMobile.mobileNumber);
                else if (pageName == "DataMatch") rtn = GamePostData(Request["datamatch"], Request["imei"], Request["date"], appName);
                else if (pageName == "Update") rtn = UpdateCustomer(Request["imei"], Request["model"], appName);
                else if (pageName == "Awards")
                {
                    rtn = GetAwards();
                }
                else if (pageName == "News")
                {
                    rtn = GetNewsByDateCheerBallThai(appName, "16", Request["date"], "", Request["type"], Request["sporttype"], Request["lang"]);
                }else if(pageName == "Table")
                {
                    rtn = GetLeagueTableCheerBallThai(appName, "1001054", "00001", AppCode_Base.Country.en.ToString());
                }else if(pageName == "Program")
                {
                    rtn = GetCheerBallThaiProgram(appName, Request["date"], Request["contentgroupid"], Request["sporttype"], Request["lang"], Request["type"]);
                }else if(pageName == "Rewards")
                {
                    rtn = GetRewards();
                }
            }

            return rtn;
        }

        /// <summary>
        /// GetNewsByDate
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="contentGroupId"></param>
        /// <param name="date"></param>
        /// <param name="rowCount"></param>
        /// <param name="type"></param>
        /// <param name="sportType"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        protected string GetNewsByDateCheerBallThai(string appName, string contentGroupId, string date, string rowCount, string type, string sportType, string lang)
        {
            try
            {

                date = date == "" || date == null ? DateTime.Now.ToString("yyyyMMdd") : date;

                int year = (date != null && date.Length > 4) ? int.Parse(date.Substring(0, 4)) : DateTime.Now.Year;
                year = (year > 2550) ? year - 543 : year;
                date = year.ToString() + date.Substring(4);
                rowCount = (rowCount == null || rowCount == "") ? "1" : rowCount;
                // News

                /*if (type != "genfile" && date == DateTime.Now.ToString("yyyyMMdd"))
                {
                    try
                    {
                        rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_GetNews"].ToString()));
                    }
                    catch
                    {
                        rtnXML = new AppCode_News().Command_GetNewsByDate(rtnXML, date, sportType, rowCount, lang, "", type);
                    }
                }
                else
                {*/
                    rtnXML = new AppCode_News().Command_GetNewsFootballThai(rtnXML, "2612", sportType, "20", lang, "", type);
                //}

                xmlDoc.Load(rtnXML.CreateReader());
                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception(appName + "_GetNewsByDate>>" + ex.Message);
            }
        }


        #region CheerBallThai League Table
        /// <summary>
        /// GetLeagueTable
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="contentGroupId"></param>
        /// <param name="sportType"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        protected string GetLeagueTableCheerBallThai(string appName, string contentGroupId, string sportType, string lang)
        {
            try
            {

                rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));
                contentGroupId = contentGroupId == "" ? ConfigurationManager.AppSettings["contestGroupDefault"] : contentGroupId;
                rtnXML = new AppCode_FootballAnalysis().CommandGetLeagueTable_CheerBallThai(rtnXML, contentGroupId, lang);

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception(appName + "_GetLeagueTable>>" + ex.Message);
            }
        }
        #endregion

        #region CheerBallThai Rewards
        private string GetRewards()
        {
            XElement element;
            element = new XElement("Content");
            element.Add(new XAttribute("title", ConfigurationManager.AppSettings["CheerBallThai_Rewards_Header"])
                                                        , new XAttribute("detail", ConfigurationManager.AppSettings["CheerBallThai_Rewards_Detail"])
                                                        , new XAttribute("detail1", "")
                                                        , new XAttribute("detail2","")
                                                        , new XAttribute("footer", ConfigurationManager.AppSettings["CheerBallThai_Rewards_Footer"])
                                                        , new XAttribute("isFree", "")
                                                        , new XAttribute("fName", "")
                                                        , new XAttribute("lName", "")
                                                        , new XAttribute("nName", "")
                                                        , new XAttribute("interview", "")
                                                        , new XAttribute("type", "")
                                                        , new XAttribute("display_date", "")
                                                        , new XAttribute("id", "")
                                                        , new XAttribute("shape", "")
                                                        , new XAttribute("weight", "")
                                                        , new XAttribute("high", "")
                                                        , new XAttribute("answer", "")
                                                        );

            rtnXML.Element("SportApp").Add(element);


            rtnXML.Element("SportApp").Add(
new XElement("status", "success")
, new XElement("message", ""));

            xmlDoc.Load(rtnXML.CreateReader());
            return xmlDoc.InnerXml;

        }
        #endregion

        #region CheerBallthai Program
        /// <summary>
        /// GetProgram // ใช่ที่ feedais และ feeddtac , imobilegame
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="date"></param>
        /// <param name="contentGroupId"></param>
        /// <param name="sportType"></param>
        /// <param name="lang"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetCheerBallThaiProgram(string appName, string date, string contentGroupId, string sportType, string lang, string type)
        {
            try
            {
                // GetProgram
                date = (date == null || date == "") ? DateTime.Now.ToString("yyyyMMdd") : date;
                int year = (date != null && date.Length > 4) ? int.Parse(date.Substring(0, 4)) : DateTime.Now.Year;
                year = (year > 2550) ? year - 543 : year;
                date = year.ToString() + date.Substring(4);
                date = (date == null || date == "") ? DateTime.Now.ToString("yyyyMMdd") : date;
                contentGroupId = (contentGroupId == null || contentGroupId == "") ? "" : contentGroupId;

                rtnXML.Element("SportApp").SetAttributeValue("date", AppCode_LiveScore.DateText(date));
                rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));

                rtnXML = new AppCode_FootballProgram().CommandGetProgramCheerBallThai("", "", lang);

                /* if (type != "genfile" && date == DateTime.Now.ToString("yyyyMMdd") && contentGroupId == "")
                 {
                     try
                     {
                         rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_Program_FeedOPT"].ToString()));
                     }
                     catch
                     {
                         rtnXML = new AppCode_FootballProgram().CommandGetProgramCheerBallThai(contentGroupId, "", lang);
                     }
                 }
                 else
                 {
                     rtnXML = new AppCode_FootballProgram().CommandGetProgramCheerBallThai(contentGroupId, "", lang);
                 }*/

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception(appName + " Get Program>>" + ex.Message);
            }
        }
        #endregion

        #region CheerBallThai get ranking
        private string GetCheerBallThaiRanking(string appName)
        {
            try
            {
                string rtn = "0";
                DataSet ds = OrclHelper.Fill(AppCode_Base.strConnOracle, CommandType.StoredProcedure, "GAMES.score_rank_select", "GAME_SCORE_RANK"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_phone_no",muMobile.mobileNumber,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_game_name",appName ,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_period","null" ,OracleType.VarChar,ParameterDirection.Input)
                                                          , OrclHelper.GetOracleParameter("o_cursor","",OracleType.Cursor,ParameterDirection.Output)
                                                        });

                if( ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0  )
                {
                    rtn = ds.Tables[0].Rows[0][0].ToString();
                }
                return rtn;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region CheerBallThai Gen Content main

        private string GetContentMain(string date,string imei,string appName,string msisdn)
        {
            try
            {
                rtnXML.Element("SportApp").Add(new XAttribute("serverversion", "1.0"));
                rtnXML.Element("SportApp").Add(new XAttribute("status", CheckActive(imei, appName)));
                rtnXML.Element("SportApp").Add(new XAttribute("score", GetCheerBallThaiRanking(appName)));
                rtnXML.Element("SportApp").Add(new XAttribute("msisdn", muMobile.mobileNumber));
                rtnXML.Element("SportApp").Add(new XAttribute("optcode", muMobile.mobileOPT));
                rtnXML.Element("SportApp").SetAttributeValue("date", AppCode_LiveScore.DateText(date));
                rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));
                rtnXML.Element("SportApp").Add(new XAttribute("imgshare", "http://wap.isport.co.th/isportws/isoccer/i_soccer_rewards.png"));
                rtnXML.Element("SportApp").Add(new XAttribute("download", "http://wap.isport.co.th/cheerballthai/cheerballthai.apk"));
                rtnXML.Element("SportApp").Add(new XAttribute("titleshare", "ลุ้นรถ Porche"));
                rtnXML.Element("SportApp").Add(new XAttribute("detailshare", "ลุ้นรถ Porche ลุ้นรวยทันใจ กับ i-Mobile ฟุตบอลเกม เฉพาะลูกค้า i - mobile เท่านั้น"));

                string strDate = DateTime.ParseExact(date, "yyyyMMdd", null).ToString("MMddyyyy");
                DataSet ds = OrclHelper.Fill(AppCode_Base.strConnOracle, CommandType.StoredProcedure, "GAMES.Select_ContentByDate", "Kiss_Content"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("P_DISPLAY_TXT",strDate,OracleType.VarChar,ParameterDirection.Input)
                                                          , OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)
                                                        });

                if (ds.Tables.Count > 0)
                {

                    DataSet dsMedia;
                    DataSet dsAnwser = null;
                    XElement element;
                    string ss = "", strAnswer = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ss = AppCode_LiveScore.DatetoText(DateTime.Now.ToString("yyyyMMdd"), AppCode_Base.Country.th.ToString());
                        try
                        {
                            dsAnwser = AppCode_iSoccer.GetGameAnswerResult(msisdn, imei, ss);
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.WriteError(ex.Message + " data " + msisdn + " " + imei + " " + ss);
                        }

                        if (dsAnwser.Tables.Count > 0 && dsAnwser.Tables[0].Rows.Count > 0)
                        {
                            var anwsers = from a in dsAnwser.Tables[0].AsEnumerable()
                                          where a.Field<string>("msch_id") == dr["KC_ID"].ToString()
                                          select a;

                            foreach (var anwser in anwsers)
                            {
                                strAnswer = anwser["answer"].ToString();
                            }
                        }

                        element = new XElement("Content");
                        element.Add(new XAttribute("title", dr["title"].ToString())
                                                                    , new XAttribute("detail", dr["title_detail"].ToString())
                                                                    , new XAttribute("detail1", dr["title_detail1"].ToString())
                                                                    , new XAttribute("detail2", dr["title_detail2"].ToString())
                                                                    , new XAttribute("footer", dr["footer"].ToString())
                                                                    , new XAttribute("isFree", dr["free"].ToString())
                                                                    , new XAttribute("fName", dr["fname"].ToString())
                                                                    , new XAttribute("lName", dr["lname"].ToString())
                                                                    , new XAttribute("nName", dr["nname"].ToString())
                                                                    , new XAttribute("interview", dr["interview"].ToString())
                                                                    , new XAttribute("type", dr["type"].ToString())
                                                                    , new XAttribute("display_date", ss)
                                                                    , new XAttribute("id", dr["KC_ID"].ToString())
                                                                    , new XAttribute("shape", "สัดส่วน : " + dr["shape"].ToString())
                                                                    , new XAttribute("weight", "น้ำหนัก : " + dr["km_w"].ToString())
                                                                    , new XAttribute("high", "ส่วนสูง : " + dr["km_h"].ToString())
                                                                    , new XAttribute("answer", strAnswer)
                                                                    );


                        strAnswer = "";
                        /*dsMedia = OrclHelper.Fill(AppCode_Base.strConnOracle, CommandType.StoredProcedure, "ISPORT_TEST.KissApp_Select_MediaByKCID", "KISS_MEDIA"
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
                        }*/

                        rtnXML.Element("SportApp").Add(element);
                    }

                    
                    rtnXML.Element("SportApp").Add(
        new XElement("status", "success")
        , new XElement("message", ""));
                }

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;

            }
            catch (Exception ex)
            {
                rtnXML.Element("SportApp").Add(
new XElement("status", "error")
, new XElement("message", ex.Message));
            }

            xmlDoc.Load(rtnXML.CreateReader());

            return xmlDoc.InnerXml;
        }

        #endregion

        #region Get Ranking
        private string GetRanking()
        {
            try
            {
                rtnXML.Element("SportApp").Add(new XAttribute("userno", "19"));
                rtnXML.Element("SportApp").Add(new XAttribute("ranking", "1232"));

                XElement element = new XElement("content" , new XAttribute("name","week"),new XAttribute("header","คะแนนประจำ สัปดาห์"),new XAttribute("detail",""),new XAttribute("footer",""));
                element.Add(new XElement("list", new XAttribute("name", "08932432XXXXX"), new XAttribute("title","345")));
                element.Add(new XElement("list", new XAttribute("name", "08932432XXXXX"), new XAttribute("title", "344")));
                element.Add(new XElement("list", new XAttribute("name", "08932432XXXXX"), new XAttribute("title", "3343")));
                element.Add(new XElement("list", new XAttribute("name", "08932432XXXXX"), new XAttribute("title", "342")));
                rtnXML.Element("SportApp").Add(element);

                element = new XElement("content", new XAttribute("name", "month"), new XAttribute("header", "คะแนนประจำ เดือน"), new XAttribute("detail", ""), new XAttribute("footer", ""));
                element.Add(new XElement("list", new XAttribute("name", "08932432XXXXX"), new XAttribute("title", "345")));
                element.Add(new XElement("list", new XAttribute("name", "08932432XXXXX"), new XAttribute("title", "344")));
                element.Add(new XElement("list", new XAttribute("name", "08932432XXXXX"), new XAttribute("title", "3343")));
                element.Add(new XElement("list", new XAttribute("name", "08932432XXXXX"), new XAttribute("title", "342")));
                rtnXML.Element("SportApp").Add(element);

                element = new XElement("content", new XAttribute("name", "campaign"), new XAttribute("header", "คะแนนประจำ ฤดูกาล"), new XAttribute("detail", ""), new XAttribute("footer", ""));
                element.Add(new XElement("list", new XAttribute("name", "08932432XXXXX"), new XAttribute("title", "345")));
                element.Add(new XElement("list", new XAttribute("name", "08932432XXXXX"), new XAttribute("title", "344")));
                element.Add(new XElement("list", new XAttribute("name", "08932432XXXXX"), new XAttribute("title", "3343")));
                element.Add(new XElement("list", new XAttribute("name", "08932432XXXXX"), new XAttribute("title", "342")));
                rtnXML.Element("SportApp").Add(element);


                rtnXML.Element("SportApp").Add(
                            new XElement("status", "success")
                            , new XElement("message", ""));
                xmlDoc.Load(rtnXML.CreateReader());
                return xmlDoc.InnerXml;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion


        #region Update Customer
        public string UpdateCustomer(string imei,string model,string appName)
        {
            try
            {
                string rtn = AppCode_iSoccer.UpdateCustomer(muMobile.mobileNumber, imei, model, appName);
                rtnXML.Element("SportApp").Add(
                            new XElement("status", "success")
                            , new XElement("message", ""));
                xmlDoc.Load(rtnXML.CreateReader());
                return xmlDoc.InnerXml;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region Get Awards
        private string GetAwards()
        {
            try
            {
                rtnXML = XDocument.Load(Server.MapPath("isoccer/awards.xml"));
                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Rules
        private string GetRules()
        {
            try
            {
                rtnXML.Element("SportApp").Add(new XElement("content"
                    , new XAttribute("name", "Rules")
                    , new XAttribute("header", ConfigurationManager.AppSettings["iSoccer_Rules_Header"])
                    , new XAttribute("detail", ConfigurationManager.AppSettings["iSoccer_Rules_Detail"])
                    , new XAttribute("footer", ConfigurationManager.AppSettings["iSoccer_Rules_Footer"])
                    ));
                rtnXML.Element("SportApp").Add(
                            new XElement("status", "success")
                            , new XElement("message", ""));

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Check Active
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imei"></param>
        /// <returns>status 100 not active , 200 active, 400 change imei,500 allaedy regis </returns>
        private string CheckActive(string imei,string appName)
        {
            try
            {
                string rtn = "";
                DataSet ds = OrclHelper.Fill(AppCode_Base.strConnOracle, CommandType.StoredProcedure, "GAMES.check_activate", "game_customer"
                    , new OracleParameter[] {OrclHelper.GetOracleParameter("p_phone_no", muMobile.mobileNumber,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_imei",imei,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_game_name",appName,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("o_cursor","",OracleType.Cursor,ParameterDirection.Output)
                                            });

                if(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    rtn = ds.Tables[0].Rows[0]["status"].ToString();
                }

                return rtn;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region get result

        /// <summary>
        /// GetResult
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="contentGroupId"></param>
        /// <param name="scoreDate"></param>
        /// <param name="sportType"></param>
        /// <param name="lang"></param>
        /// <param name="isFootballThai"></param>
        /// <returns></returns>
        protected string GetResultiSoccer(string appName, string contentGroupId, string scoreDate, string sportType, string lang,string imei)
        {
            try
            {
                // GetResult
                AppCode_LiveScore ls = new AppCode_LiveScore();

                scoreDate = scoreDate == "" ? DateTime.Now.ToString("yyyyMMdd") : scoreDate;
                int year = (scoreDate != null && scoreDate.Length > 4) ? int.Parse(scoreDate.Substring(0, 4)) : DateTime.Now.Year;
                year = (year > 2550) ? year - 543 : year;
                scoreDate = year.ToString() + scoreDate.Substring(4);


                TimeSpan diffDate = new DateTime(int.Parse(scoreDate.Substring(0, 4)), int.Parse(scoreDate.Substring(4, 2)), int.Parse(scoreDate.Substring(6, 2))) - DateTime.Now;

                if (contentGroupId == "" && scoreDate == DateTime.Now.ToString("yyyyMMdd"))
                {
                    //try
                    //{
                        //rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_ScoreNew"].ToString()));
                    //}
                    //catch { }
                    ////if (rtnXML.Element("SportApp").Element("status") == null)
                    //{
                        rtnXML = ls.CommandGetScoreiSoccerGame(rtnXML, "", lang, contentGroupId, AppCode_LiveScore.MatchType.Finished, diffDate.Days, "N",muMobile.mobileNumber,imei);
                    //}
                }
                else
                {
                    rtnXML = ls.CommandGetScoreiSoccerGame(rtnXML, "", lang, contentGroupId, AppCode_LiveScore.MatchType.Finished, diffDate.Days, "N", muMobile.mobileNumber, imei);
                }

                rtnXML.Element("SportApp").SetAttributeValue("date", AppCode_LiveScore.DateText(scoreDate));
                rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception(appName + "_GetResult>>" + ex.Message);
            }
        }

        #endregion

        #region Game Post Data
        private string GamePostData(string dataAnswer,string imei,string dateProgram,string appName)
        {
            try {
                string[] datas = dataAnswer.Split('|');
                ExceptionManager.WriteError(muMobile.mobileNumber + "|" + dataAnswer);
                dateProgram = string.IsNullOrEmpty(dateProgram) ? AppCode_LiveScore.DatetoText(DateTime.Now.ToString("yyyyMMdd"),AppCode_Base.Country.th.ToString()) : dateProgram;
                foreach (string data in datas)
                {
                    

                    if (!string.IsNullOrEmpty(data))
                    {
                        string[] match = data.Split(':');

                        int rtn = OrclHelper.ExecuteNonQuery(AppCode_Base.strConnOracle, CommandType.StoredProcedure, "GAMES.subscribe_insert"
                            , new OracleParameter[] {OrclHelper.GetOracleParameter("p_phone_no",muMobile.mobileNumber,OracleType.VarChar,ParameterDirection.Input)
                                                ,OrclHelper.GetOracleParameter("p_imei",imei,OracleType.VarChar,ParameterDirection.Input)
                                                ,OrclHelper.GetOracleParameter("p_msch_id",match[0],OracleType.VarChar,ParameterDirection.Input)
                                                ,OrclHelper.GetOracleParameter("p_match_date",dateProgram,OracleType.VarChar,ParameterDirection.Input)
                                                ,OrclHelper.GetOracleParameter("p_answer",match[1],OracleType.VarChar,ParameterDirection.Input)
                                                ,OrclHelper.GetOracleParameter("p_game_name",appName,OracleType.VarChar,ParameterDirection.Input)
                                                    });

                        //ExceptionManager.WriteError("imei=" + imei+ " "+muMobile.mobileNumber + "|" + rtn);
                    }
                }
                
                rtnXML.Element("SportApp").Add(
                            new XElement("status", "success")
                            , new XElement("message", ""));
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError("dateProgram:" + dateProgram + ex.Message);
                rtnXML.Element("SportApp").Add(
                            new XElement("status", "error")
                            , new XElement("message", ex.Message));
            }

            xmlDoc.Load(rtnXML.CreateReader());

            return xmlDoc.InnerXml;
        }
        #endregion

        #region Program Group by Date
        /// <summary>
        /// GetProgram // ใช่ที่ feedais และ feeddtac , imobilegame
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="date"></param>
        /// <param name="contentGroupId"></param>
        /// <param name="sportType"></param>
        /// <param name="lang"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetProgramGroupdate(string appName, string date, string contentGroupId, string sportType, string lang, string type,string imei)
        {
            try
            {
                // GetProgram
                date = (date == null || date == "") ? DateTime.Now.ToString("yyyyMMdd") : date;
                int year = (date != null && date.Length > 4) ? int.Parse(date.Substring(0, 4)) : DateTime.Now.Year;
                year = (year > 2550) ? year - 543 : year;
                date = year.ToString() + date.Substring(4);
                date = (date == null || date == "") ? DateTime.Now.ToString("yyyyMMdd") : date;
                contentGroupId = (contentGroupId == null || contentGroupId == "") ? "" : contentGroupId;
                // bomupdate
                contentGroupId = "";
                rtnXML.Element("SportApp").Add(new XAttribute("status",CheckActive(imei,appName)));
                rtnXML.Element("SportApp").Add(new XAttribute("msisdn",muMobile.mobileNumber));
                rtnXML.Element("SportApp").Add(new XAttribute("optcode", muMobile.mobileOPT));
                rtnXML.Element("SportApp").SetAttributeValue("date", AppCode_LiveScore.DateText(date));
                rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));
                rtnXML.Element("SportApp").Add(new XAttribute("imgshare", "http://wap.isport.co.th/isportws/isoccer/i_soccer_rewards.png"));
                rtnXML.Element("SportApp").Add(new XAttribute("titleshare", "ลุ้นรถ Porche"));
                rtnXML.Element("SportApp").Add(new XAttribute("detailshare", "ลุ้นรถ Porche ลุ้นรวยทันใจ กับ i-Mobile ฟุตบอลเกม เฉพาะลูกค้า i - mobile เท่านั้น"));
                //muMobile.mobileNumber = "66892037059";
                if (type != "genfile" && date == DateTime.Now.ToString("yyyyMMdd") && contentGroupId == "")
                {
                    try
                    {
                        rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_Program_FeedOPT"].ToString()));
                    }
                    catch
                    {
                        rtnXML = new AppCode_FootballProgram().CommandGetProgramGroupByDate(rtnXML,contentGroupId, "", lang,muMobile.mobileNumber, imei);
                    }
                }
                else
                {
                    rtnXML = new AppCode_FootballProgram().CommandGetProgramGroupByDate(rtnXML,contentGroupId, "", lang, muMobile.mobileNumber, imei);
                }

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception(appName + " Get Program>>" + ex.Message);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}