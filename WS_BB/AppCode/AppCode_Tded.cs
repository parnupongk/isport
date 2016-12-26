using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml.Linq;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;
namespace WS_BB
{
    public class AppCode_Tded : AppCode_Base
    {
        public XDocument CommangGetTded(string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingHeader"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapui_tded_selectsportpool");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    rtnXML.Element("SportApp").Add(new XElement("desctiption", System.Configuration.ConfigurationManager.AppSettings["wordingTded"]));
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["tded"].ToString() != "")
                        {
                            rtnXML.Element("SportApp").Add(
                                new XElement("detail"
                                    , new XAttribute("tded", dr["tded"].ToString())
                                    , new XAttribute("tded_name", dr["cat_name_local"].ToString())
                                    ));
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
            catch(Exception ex)
            {
                ExceptionManager.WriteError("CommangGetTded >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }


        public XDocument CommangGetFuntong(string contestGroupId,string countryId,string lang)
        {
            XDocument rtnXML = new XDocument(new XElement("SportApp",
                new XAttribute("header", ConfigurationManager.AppSettings["wordingHeader"])
                , new XAttribute("date", AppCode_LiveScore.DateText(DateTime.Now.ToString("yyyyMMdd")))
                ));
            try
            {
                // มองอย่างเซียน
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballfuntong"
                    , new SqlParameter[] { new SqlParameter("@contestGroupId",contestGroupId)
                    ,new SqlParameter("@countryId",countryId)});

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string temp="",img16="";
                    string[] funtone=null;
                    XElement xElement = null;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        #region Add XML
                        if (temp != dr["contestgroupid"].ToString())
                        {
                            if (xElement != null) rtnXML.Element("SportApp").Add(xElement);
                            img16 = dr["PIC_16X11"].ToString() == "" ? "default.png" : dr["PIC_16X11"].ToString();
                            xElement = new XElement("League",
                                 new XAttribute("contestGroupName", (lang == Country.th.ToString()) ? dr["class_name_local"].ToString() : dr["contestgroup_name"].ToString())
                                , new XAttribute("contestGroupId", dr["contestgroupid"].ToString())
                                , new XAttribute("contestURLImages", ConfigurationManager.AppSettings["ApplicationPathCountryIcon16x11"].ToString() + img16)
                                , new XAttribute("tmName", dr["tm_name"].ToString())
                                , new XAttribute("tmSystem", dr["tm_system"].ToString())
                                );
                        }
                        funtone = dr["funtong"].ToString().Split('|');
                        xElement.Add(new XElement("Match"
                                ,new XAttribute("mschId", dr["msch_id"].ToString())
                               , new XAttribute("matchId", dr["match_id"].ToString())
                               , new XAttribute("teamCode1", dr["team_id1"].ToString()) // id isportfeed
                               , new XAttribute("teamCode2", dr["team_id2"].ToString()) // id isportfeed
                               , new XAttribute("teamName1", (lang == Country.th.ToString()) ? dr["isportteamname1"].ToString() : dr["team_name1"].ToString())
                               , new XAttribute("teamName2", (lang == Country.th.ToString()) ? dr["isportteamname2"].ToString() : dr["team_name2"].ToString())
                               , new XAttribute("liveChannel", (lang == Country.th.ToString()) ? dr["live_channel_local"].ToString() : dr["live_channel"].ToString())
                               , new XAttribute("matchDate", AppCode_LiveScore.DatetoText(dr["matchdate"].ToString(), lang))
                               , new XAttribute("matchTime", dr["matchtime"].ToString())
                               , new XAttribute("price", dr["price"].ToString().Split('|').Length > 1 ? dr["price"].ToString().Split('|')[1] : "")
                               , new XAttribute("funtong1", funtone.Length > 0 ? funtone[0] : ")")
                               , new XAttribute("funtong2", funtone.Length > 1 ? funtone[1] : "")
                               ));
                        temp = dr["contestgroupid"].ToString();
                        #endregion
                    }

                    if (xElement != null) rtnXML.Element("SportApp").Add(xElement); // add Element สุดท้าย

                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", ""));

                }
                else
                {
                    rtnXML.Element("SportApp").Add(new XElement("status", "success")
                    , new XElement("message", "ไม่ข้อมูลมองอย่างเซียน"));
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("CommangGetFuntong >> " + ex.Message);
                rtnXML.Element("SportApp").Add(new XElement("status", "error")
                    , new XElement("message", ex.Message));
            }
            return rtnXML;
        }
    }
}
