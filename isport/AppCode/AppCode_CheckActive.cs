using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using OracleDataAccress;
using WebLibrary;
using MobileLibrary;

namespace isport
{
    public class AppCode_CheckActive : AppMain
    {
        #region Check Allow IP
        private bool CheckAllowIP(string ip)
        {
            string[] allowIP = ConfigurationManager.AppSettings["isportAllowIP"].Split(',');
            foreach(string sIp in allowIP )
            {
                if (ip.IndexOf(sIp) > -1) return true;
            }
            return false;
        }
        #endregion

        #region Get Info Dtac
        public string Create_trandsaction_Dtac()
        {
            return "4" + DateTime.Now.ToString("yyMMddhhmmss") + "894" + DateTime.Now.ToString("fff");
        }
        public string DtacCheckactive(string serviceId,string msdn,string contentId,string model)
        {
            try
            {
                string command = @"<cpa-request-aoc>
                <authentication>
                    <user>TrOps</user>
                    <password>TqPZSp894</password>
                </authentication>
                <information>";
                command += "<service>" + serviceId + "</service>";
                command += "<msisdn>" + msdn + "</msisdn>";
                command += "<cp-ref-id>" + contentId + "</cp-ref-id>";
                command += "<mobile-model>" + model + "</mobile-model>";
                command += "<timestamp>" + Create_trandsaction_Dtac() + "</timestamp>";
                command += "</information>";
                command += "</cpa-request-aoc>";

                return new SendService.sendpost().SendPost(ConfigurationManager.AppSettings["DtacLinkGetInfo"].ToString(), command);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Check Active
        public string CheckActive(string msisdn, string optId, string sgId, string mp, string prj)
        {
            string rtn = "N";
            OracleConnection oConn = new OracleConnection(AppMain.strConnOracle);
            try
            {
                sgId = sgId == null || sgId == "" ? "0" : sgId;
                optId = optId == null || optId == "" ? "0" : optId;
                string strSql = "select  WAP.CHK_ACTIVE('" + msisdn + "','" + optId + "'," + sgId + ",'" + mp + "'," + prj + ") as CHECK_ACTIVE from dual";
                IDataReader dr = OrclHelper.ExecuteReader(oConn, CommandType.Text, strSql);
                while (dr.Read())
                {
                    rtn = dr["check_active"].ToString();
                }
                oConn.Close();
                if (optId == "02" || optId == "'02'" || rtn == "N")
                {
                    string rtnXML = DtacCheckactive("8451110500001", msisdn, sgId + mp + prj, "imo");
                    System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Parse(rtnXML);
                    //string status = Utilities.GetDataXML("cpa-response-aoc", rtnXML, "status");

                    if (xDoc.Element("status") != null && xDoc.Element("subs-expire") != null && xDoc.Element("status").Value == "200")
                    {
                        string dateEx = xDoc.Element("subs-expire").Value;  //Utilities.GetDataXML("cpa-response-aoc", rtnXML, "subs-expire");
                        string year = "20" + dateEx.Substring(0,2);
                        DateTime dateExpire = new DateTime(int.Parse(year), int.Parse(dateEx.Substring(2, 2)), int.Parse(dateEx.Substring(4, 2)), int.Parse(dateEx.Substring(6, 2))
                            , int.Parse(dateEx.Substring(8, 2)), int.Parse(dateEx.Substring(10, 2)));
                        if (dateExpire > DateTime.Now) rtn = "Y";
                    }
                }
            }
            catch (Exception ex)
            {
                if(oConn.State != ConnectionState.Closed) oConn.Close();
                ExceptionManager.WriteError("CheckActive >> "+ ex.Message);
            }
            return rtn;
        }

        public string CheckActive_ByCNT(string msisdn, string optId, string sgId, string pcntId, string mp, string prj)
        {
            string rtn = "N";
            OracleConnection oConn = new OracleConnection(AppMain.strConnOracle);
            try
            {
                sgId = sgId == null || sgId == "" ? "0" : sgId;
                optId = optId == null || optId == "" ? "0" : optId;
                string strSql = "select  WAP.CHK_ACTIVE_BYCNT('" + msisdn + "','" + optId + "','" + sgId + "','" + pcntId + "','" + mp + "','" + prj + "') as CHECK_ACTIVE from dual";
                IDataReader dr = OrclHelper.ExecuteReader(oConn, CommandType.Text, strSql);
                while (dr.Read())
                {
                    rtn = dr["check_active"].ToString();
                }
                oConn.Close();
                if (optId == "02" || optId == "'02'" || rtn == "N")
                {
                    string rtnXML = DtacCheckactive("8451120500001", msisdn, sgId + mp + prj, "imo");
                    //string status = Utilities.GetDataXML("cpa-response-aoc", rtnXML, "status");
                    System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Parse(rtnXML);
                    if (xDoc.Element("status") != null && xDoc.Element("subs-expire") != null && xDoc.Element("status").Value == "200")
                    {
                        string dateEx = xDoc.Element("subs-expire").Value;//Utilities.GetDataXML("cpa-response-aoc", rtnXML, "subs-expire");
                        string year = "20" + dateEx.Substring(0, 2);
                        DateTime dateExpire = new DateTime(int.Parse(year), int.Parse(dateEx.Substring(2, 2)), int.Parse(dateEx.Substring(4, 2)), int.Parse(dateEx.Substring(6, 2))
                            , int.Parse(dateEx.Substring(8, 2)), int.Parse(dateEx.Substring(10, 2)));
                        if (dateExpire > DateTime.Now) rtn = "Y";
                    }
                }
            }
            catch (Exception ex)
            {
                if (oConn.State != ConnectionState.Closed) oConn.Close();
                ExceptionManager.WriteError("CheckActive_ByCNT >> " + ex.Message);
            }
            return rtn;
        }

        #endregion


        public void CheckAllService(string sgId, string lng, string size, string scsId, string mp, string prj)
        {
            try
            {
                size = size == "" ? "s" : size;
                sgId = sgId == "" ? "1" : sgId;
                mp = mp == "" ? "0000" : mp;
                scsId  = scsId == ""?"0":scsId;
                lng = lng == ""?"L":lng;
                prj = prj==""?"1":prj;
                string ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                MobileUtilities mu = Utilities.getMISDN(HttpContext.Current.Request);
                string checkActive = CheckActive(mu.mobileNumber, mu.mobileOPT, sgId, mp, prj);
                if (!CheckAllowIP(ip) && checkActive =="N")
                {
                    
                    if (mu.mobileOPT == "'01'" || mu.mobileOPT == "01")
                    {
                        // AIS
                        string linkPay = new AppCode().GetServiceGroupBySgId(sgId);
                        string cpSessionId = size + ":" + lng + ":" + sgId + ":" + mp + ":" + prj + ":" + scsId;
                        string spsID = DateTime.Now.ToString("yyyyMMddHmmssff");
                        if (linkPay == "1")

                            HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings["aisSubScribeURL"] + "?cmd=login&ch=WAP&SN=4511005&spsID="
                + spsID + "&spName=511&spURL=" + ConfigurationManager.AppSettings["isportAcceptPathURL"] + "pay_ais_session.aspx?CPsessionID=" + cpSessionId, false);

                        else if (linkPay == "2")

                            HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings["aisSubScribeURL"] + "?cmd=login&ch=WAP&SN=4511115&spsID="
                + spsID + "&spName=511&spURL=" + ConfigurationManager.AppSettings["isportAcceptPathURL"] + "pay_ais_session.aspx?CPsessionID=" + cpSessionId, false);

                        else
                            HttpContext.Current.Response.Redirect("http://wap.isport.co.th/sport_center/paymentnew/pay_ais.aspx?sg=" + sgId + "&lng=" + lng + "&size=" + size + "&prj=" + prj + "&mp=" + mp + "&scsid=" + scsId, false);

                    }
                    else if (mu.mobileOPT == "'02'" || mu.mobileOPT == "02")
                    {
                        HttpContext.Current.Response.Redirect("http://wap.isport.co.th/sport_center/paymentnew/pay_dtac.aspx?SG=" + sgId + "+LNG=" + lng + "+size=" + size + "+mp=" + mp + "+prj=" + prj + "+SCS_id=" + scsId, false);
                    }
                    else if (mu.mobileOPT == "'03'" || mu.mobileOPT == "03")
                    {
                        HttpContext.Current.Response.Redirect("http://wap.isport.co.th/sport_center/paymentnew/pay_true.aspx?SG=" + sgId + "+LNG=" + lng + "+size=" + size + "+mp=" + mp + "+prj=" + prj + "+SCS_id=" + scsId, false);
                    }
                    else
                    {
                        // Redirect to error OPT
                        HttpContext.Current.Response.Redirect("index.aspx?p=erropt", false);
                    }

                }
                
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
