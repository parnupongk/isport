using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Data;
using MobileLibrary;
using WebLibrary;

namespace WS_BB
{
    public abstract class feedMain : System.Web.UI.Page
    {
        protected XDocument rtnXML = new XDocument(new XElement("SportApp",
                            new XAttribute("header", ConfigurationManager.AppSettings["wordingLeagueTable"])
                            , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))));
        protected XmlDocument xmlDoc = new XmlDocument();
        protected string strErr = "", strStatus = "";
        private string code = "", imsi = "", imei = "", model = "";
        protected MobileUtilities muMobile = null;
        protected string optCode = "";

        #region Abstract class
        public abstract string CheckPageName(string pageName, string appName);
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                muMobile = Utilities.getMISDN(Request);
                optCode = muMobile.mobileOPT == null || muMobile.mobileOPT == "" ? AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"])) : muMobile.mobileOPT;
                StatusWS ws = AppCode_PageStatus.CheckErrorParameter(Request);
                strErr = ws.strErrMess;
                strStatus = ws.strStatus;
                if (!IsPostBack && (strErr == null || strErr == ""))
                {
                    Response.Write(CheckPageName(Request["pn"], Request["ap"]));

                    new AppCode_Logs().Logs_Insert(Request["ap"] + "_" + Request["pn"], strErr, Request.Url.ToString(), "android", muMobile.mobileOPT + "|" + muMobile.mobileNumber + "|" + Request["imei"] + "|" + Request["imsi"]
                        , Request["ap"], Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
                }
                else if (strErr != null && strErr != "")
                {

                    rtnXML = AppCode_PageStatus.GenStatus(rtnXML, strStatus, strErr);
                    xmlDoc.Load(rtnXML.CreateReader());
                    Response.Write(xmlDoc.InnerXml);

                    new AppCode_Logs().Logs_Insert(Request["ap"] + "_" + Request["pn"] + "_error", strErr, Request.Url.ToString(), "android", muMobile.mobileOPT + "|" + muMobile.mobileNumber + "|" + Request["imei"] + "|" + Request["imsi"]
                        , Request["ap"], Request["imei"], Request["imsi"], muMobile.mobileNumber, Request["model"], muMobile.mobileOPT, AppCode_Base.GETIP(), "");
                }
            }
            catch (Exception ex)
            {
                strStatus = "Error";
                strErr = ex.Message;
                ExceptionManager.WriteError("Main " + Request["pn"] + ">>" + ex.Message);

                rtnXML = AppCode_PageStatus.GenStatus(rtnXML, strStatus, strErr);
                xmlDoc.Load(rtnXML.CreateReader());
                Response.Write(xmlDoc.InnerXml);
            }
        }
        #endregion

        #region Check Active
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Y= Active , N = Not Active , E = ชาร์ต MT ไม่ได้ติดต่อกัน 7 วัน</returns>
        protected string CheckActiveandPromotion(AppCode_Base.AppName appName, string imsi, string imei, string promoDay, bool isChkPromo, string pssvId)
        {
            string isActive = "Y";
            try
            {
                if (isChkPromo)
                {

                    SubscribeMember sm = AppCode_Subscribe.IsportAppCheckExpirePromotion(muMobile.mobileNumber, imsi, imei, muMobile.mobileOPT, appName.ToString(), promoDay);

                    if (optCode == "05")
                    {
                        // 3GX
                        isActive = "Y";
                    }
                    else
                    {
                        // เช็คว่าเกินวันที่ให้ฟรีหรือยัง
                        if (sm.status == "Y")
                        {
                            // Y = เกินวันที่ให้ฟรี
                            // Check Dup
                            string msisdn = muMobile.mobileNumber == "" || muMobile.mobileNumber == null ? AppCode_Subscribe.CheckActive_AppMember(imsi, imei, appName.ToString()).msisdn : muMobile.mobileNumber;
                            //ExceptionManager.WriteError("CheckActiveandPromotion>>" + msisdn + "optCode:" + optCode);
                            isActive = AppCode_Subscribe.CheckActive_IsportStarSoccer_AppMember(optCode, msisdn, imsi, imei, appName.ToString(), pssvId);


                            // รอใช่งานรวมกับ MO
                            //if (isActive == "Y" && appName == AppCode_Base.AppName.SportPool)
                            //{
                            //    // check MT
                            //    isActive = AppCode_Subscribe.CheckActive_MT_Insub(msisdn, pssvId);
                            //    isActive = isActive == "N" ? "E" : isActive ;

                            //}

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CheckActiveandPromotion>>" + ex.Message);
            }
            return isActive;
        }
        #endregion

        #region Check Error
        private StatusWS CheckErrorParameter(HttpRequest Request)
        {
            StatusWS ws = new StatusWS();
            if (Request["pn"] != "notification")
            {
                if (Request["ap"] == "" || Request["ap"] == null) ws.strErrMess += "pls. input app name,";
                if (Request["imei"] == "" || Request["imei"] == null) ws.strErrMess += "pls. input IMEI,";
                if (Request["imsi"] == "" || Request["imsi"] == null) ws.strErrMess += "pls. input IMSI,";
                if (Request["ano"] == "" || Request["ano"] == null) strErr += "pls. input ano";
                if (ws.strErrMess != "") ws.strStatus = "Error";
            }
            return ws;
        }
        #endregion

        #region News
        /// <summary>
        /// GeNewsByContentGroup
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="contentGroupId"></param>
        /// <param name="rowCount"></param>
        /// <param name="lang"></param>
        /// <param name="msisdn"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string  GetNewsByContentGroup(string appName , string contentGroupId,string rowCount, string lang, string msisdn, string type)
        {
            try
            {
                // contestGroupId => ข่าว by league
                // countryId => ข่าว by ประเทศ
                contentGroupId = "807";
                //contestGroupId = "697";
                rowCount = rowCount == "" ? "10" : rowCount;
                //new AppCode_Logs().Logs_Insert("StarSoccer", contestGroupId, countryId, type, Context.Request.ServerVariables["REMOTE_ADDR"].ToString());
                //new AppCode_Logs().Logs_Insert(appName +"_News", contentGroupId, "", type, msisdn, AppCode_Base.AppName.FeedAis.ToString(), "", "", "", "", "", AppCode_Base.GETIP(), "");

                rtnXML = new AppCode_News().Command_GetNewsFeedAis(rtnXML, contentGroupId, "00001", rowCount, lang, "", type);
                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception(appName + "_GeNewsByContentGroup>>" + ex.Message);
            }
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
        protected string GetNewsByDate(string appName,string contentGroupId, string date, string rowCount, string type, string sportType, string lang)
        {
            try
            {

                date = date == "" || date == null ? DateTime.Now.ToString("yyyyMMdd") : date;

                int year = (date != null && date.Length > 4) ? int.Parse(date.Substring(0, 4)) : DateTime.Now.Year;
                year = (year > 2550) ? year - 543 : year;
                date = year.ToString() + date.Substring(4);
                rowCount = (rowCount == null || rowCount == "") ? "1" : rowCount;
                // News

                if (type != "genfile" && date == DateTime.Now.ToString("yyyyMMdd"))
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
                {
                    rtnXML = new AppCode_News().Command_GetNewsByDate(rtnXML, date, sportType, rowCount, lang, "", type);
                }

                xmlDoc.Load(rtnXML.CreateReader());
                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception(appName + "_GetNewsByDate>>" + ex.Message);
            }
        }
        #endregion

        #region Program
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
        protected string GetProgram(string appName,string date, string contentGroupId, string sportType, string lang, string type)
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

                if (type != "genfile" && date == DateTime.Now.ToString("yyyyMMdd") && contentGroupId == "" )
                {
                    try
                    {
                        rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_Program_FeedOPT"].ToString()));
                    }
                    catch
                    {
                        rtnXML = new AppCode_FootballProgram().CommandGetProgramByLeague( contentGroupId, "", lang);
                    }
                }
                else
                {
                    rtnXML = new AppCode_FootballProgram().CommandGetProgramByLeague( contentGroupId, "", lang);
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

        #region Score
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
        protected string GetResult(string appName, string contentGroupId, string scoreDate, string sportType, string lang, bool isFootballThai)
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

                if (scoreDate == DateTime.Now.ToString("yyyyMMdd"))
                {
                    try
                    {
                        rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_ScoreNew"].ToString()));
                    }
                    catch { }
                    if (rtnXML.Element("SportApp").Element("status") == null)
                    {
                        if (isFootballThai) rtnXML = ls.CommandGetScoreResult(rtnXML, "3405", scoreDate, lang);// บอลไทย
                        rtnXML = ls.CommandGetScore(rtnXML, "", lang, contentGroupId, AppCode_LiveScore.MatchType.Finished, diffDate.Days, "N");
                    }
                }
                else
                {
                    if (isFootballThai)  rtnXML = ls.CommandGetScoreResult(rtnXML, "3405", scoreDate, lang);// บอลไทย
                    rtnXML = ls.CommandGetScore(rtnXML, "", lang, contentGroupId, AppCode_LiveScore.MatchType.Finished, diffDate.Days, "N");
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
        /// <summary>
        /// GetLiveScore
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="contentGroupId"></param>
        /// <param name="sportType"></param>
        /// <param name="lang"></param>
        /// <param name="isFootballThai"></param>
        /// <returns></returns>
        protected string GetLiveScore(string appName, string contentGroupId, string sportType, string lang, bool isFootballThai)
        {
            try
            {
                // LiveScore
                AppCode_LiveScore ls = new AppCode_LiveScore();

                if (contentGroupId == "")
                {
                    try
                    {
                        rtnXML = XDocument.Parse(new push().SendGet(ConfigurationManager.AppSettings["IsportGenFile_LiveScore"].ToString()));
                    }
                    catch { }
                    if (rtnXML.Element("SportApp").Element("status") == null)
                    {
                        if (isFootballThai) rtnXML = ls.CommandGetScoreResult(rtnXML, "3405", DateTime.Now.ToString("yyyyMMdd"), lang);// บอลไทย
                        rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, "", lang, contentGroupId, AppCode_LiveScore.MatchType.inprogress, 0, "N");

                    }

                }
                else
                {
                    rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, "", lang, contentGroupId, AppCode_LiveScore.MatchType.inprogress, 0, "N");
                }

                if (rtnXML.Element("SportApp").Element("url") == null) rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));
                if (rtnXML.Element("SportApp").Element("League") == null)
                {
                    rtnXML.Element("SportApp").Element("status").Remove();
                    rtnXML.Element("SportApp").Element("message").Remove();
                    if (isFootballThai) rtnXML = ls.CommandGetScoreResult(rtnXML, "3405", DateTime.Now.ToString("yyyyMMdd"), lang);// บอลไทย
                    rtnXML = new AppCode_LiveScore().CommandGetScore(rtnXML, "", lang, contentGroupId, AppCode_LiveScore.MatchType.Finished, 0, "N");

                }
                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception(appName +"_GetLiveScore>>" + ex.Message);
            }
        }
        /// <summary>
        /// GetScoreDetail
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="matchId"></param>
        /// <param name="mschId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        protected string GetScoreDetail(string appName,string matchId, string mschId, string lang)
        {
            try
            {
                try
                {
                    // get score detail ต้องไปดึงที่ xml file ก่อน (ถ้าไม่เจอถึงไปที่ database)
                    string urlScore = ConfigurationManager.AppSettings["IsportGenFile_ScoreDetail"].ToString() + matchId + ".xml";
                    xmlDoc.LoadXml(new push().SendGet(urlScore));
                }
                catch { }

                if (xmlDoc.ChildNodes.Count == 0)
                {
                    xmlDoc.Load(new AppCode_LiveScore().CommandGetScoreDetail(matchId, mschId, lang).CreateReader());
                    //new AppCode_Logs().Logs_Insert_IP();
                }

                //rtnXML = new AppCode_LiveScore().CommandGetScoreDetail(rtnXML,matchId, mschId, lang);
                //xmlDoc.Load(rtnXML.CreateReader());
                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception(appName + "_GetScoreDetail>>" + ex.Message);
            }
        }
        #endregion

        #region League Table
        /// <summary>
        /// GetLeagueTable
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="contentGroupId"></param>
        /// <param name="sportType"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        protected string GetLeagueTable(string appName,string contentGroupId, string sportType, string lang)
        {
            try
            {

                rtnXML.Element("SportApp").Add(new XAttribute("url", "http://wap.isport.co.th"));
                contentGroupId = contentGroupId == "" ? ConfigurationManager.AppSettings["contestGroupDefault"] : contentGroupId;
                rtnXML = new AppCode_FootballAnalysis().CommandGetLeagueTable_IsportStarSoccer(rtnXML, contentGroupId, lang);

                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception(appName+"_GetLeagueTable>>" + ex.Message);
            }
        }
        #endregion

        #region Tded
        protected string GetTdedSportPool(string lang)
        {
            try
            {
                rtnXML = new AppCode_Tded().CommangGetTded(lang);
                xmlDoc.Load(rtnXML.CreateReader());

                return xmlDoc.InnerXml;
            }
            catch (Exception ex)
            {
                throw new Exception("GetTdedSportPool>> " + ex.Message);
            }
        }
        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion
    }
}
