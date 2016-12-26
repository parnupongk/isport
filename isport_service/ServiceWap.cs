using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using OracleDataAccress;
using WebLibrary;
namespace isport_service
{
    public class ServiceWap
    {
        public string DtacCheckActive(string chageCode, string msisdn, string contentId, string cpRefId, string model)
        {
            try
            {
                string command = "<cpa-request-aoc>";
                command += "<authentication>";
                command += "<user>Isport511</user>";
                command += "<password>Y466FJXit</password>";
                command += "</authentication>  ";
                command += "<information>";
                command += "<productid>" + chageCode + "</productid>";
                command += "<msisdn>" + msisdn + "</msisdn>";
                command += "<cp-ref-id>" + contentId + "</cp-ref-id>";
                command += "<mobile-model>" + model + "</mobile-model>";
                command += "<timestamp>" + "4" + DateTime.Now.ToString("yyMMddhhmmss") + "511" + DateTime.Now.ToString("fff") + "</timestamp>";
                command += "</information>";
                command += "</cpa-request-aoc>";

                return new sendService.push().SendPost("http://sdpapi.dtac.co.th:8319/SAG/services/cpa/aoc/information", command);
            }
            catch (Exception ex)
            {
                throw new Exception("DtacCheckActive >> " + ex.Message);
            }
        }

        #region Check Active
        public static string WapCheckActive(string orclStrConn, string msisdn, string chageCode50, string opt, string sgId, string pcntId, string mp, string prj, string strLinkPay)
        {
            try
            {
                string rtn = "N";
                OracleConnection conn = new OracleConnection(orclStrConn);
                string sqlFunction = ((sgId == "95" || sgId == "255") || pcntId != "" ) ? "select wap.CHK_ACTIVE_BYCNT('" + msisdn + "','" + opt + "','" + sgId + "','" + pcntId + "','" + mp + "','" + prj + "') as CHECK_ACTIVE from dual"
                                                                                    : "select wap.CHK_ACTIVE('" + msisdn + "','" + opt + "','" + sgId + "','" + mp + "','" + prj + "') as CHECK_ACTIVE from dual";
                //ExceptionManager.WriteError(sqlFunction);
                IDataReader dr = OracleDataAccress.OrclHelper.ExecuteReader(conn, CommandType.Text, sqlFunction);
                while (dr.Read())
                {
                    rtn = dr["CHECK_ACTIVE"].ToString();
                }
                if (conn.State == ConnectionState.Open) conn.Close();

                if (opt == "02" && rtn == "N" && strLinkPay == "0")
                {
                    string rtnXml = new ServiceWap().DtacCheckActive(chageCode50, msisdn, sgId + mp + prj, "", "imobile");
                    ExceptionManager.WriteError(rtnXml);
                    if (MobileLibrary.Utilities.GetDataXML("cpa-response-aoc", rtnXml, "status") == "200" && rtnXml.IndexOf("subs-expire") > 0)
                    {
                        string dateExpire = MobileLibrary.Utilities.GetDataXML("cpa-response-aoc", rtnXml, "subs-expire");
                        DateTime dateEx = new DateTime(int.Parse(dateExpire.Substring(0, 4)), int.Parse(dateExpire.Substring(4, 2)), int.Parse(dateExpire.Substring(6, 2))
                                                                        , int.Parse(dateExpire.Substring(8, 2)), int.Parse(dateExpire.Substring(10, 2)), int.Parse(dateExpire.Substring(12, 2)));
                        dateEx.AddDays(+1);
                        if (dateEx > DateTime.Now)
                        {
                            rtn = "Y";
                        }
                    }
                }

                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("WapCheckActive>> " + ex.Message);
            }
        }
        #endregion

        #region Check Service
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orclStrConn"></param>
        /// <param name="response"></param>
        /// <param name="chageCode50">ใช้ chage code 50 ของ Dtac (ส่งไป check ที่ dtac)</param>
        /// <param name="ipAllow"></param>
        /// <param name="ipCurrent"></param>
        /// <param name="opt"></param>
        /// <param name="msisdn"></param>
        /// <param name="userAgent"></param>
        /// <param name="sg"></param>
        /// <param name="scsId"></param>
        /// <param name="mp"></param>
        /// <param name="prj"></param>
        public static void WapCheckAllService(string orclStrConn,System.Web.HttpResponse response,string chageCode50,string ipAllow,string ipCurrent,string opt,string msisdn,string userAgent,string sg,string scsId,string mp,string prj)
        {
            try
            {

                if (!(ipAllow.IndexOf(ipCurrent) > -1))
                {
                    // ip not allow
                    string active = ServiceWap.WapCheckActive(orclStrConn, msisdn, chageCode50, opt, sg, scsId, mp, prj, "0");
                    ExceptionManager.WriteError("msisdn : " + msisdn + " sg : " + sg + " active : " + active + " ip :" + ipCurrent);
                    if (active == "N")
                    {
                        if (opt == "01")
                        {
                            response.Redirect("http://wap.isport.co.th/sport_center/paymentnew/pay_ais.aspx?SG=" + sg + "&mp=" + mp + "&SCS_id=" + scsId + "&prj=" + prj, false);
                        }
                        else if (opt == "02")
                        {
                            response.Redirect("http://wap.isport.co.th/sport_center/paymentnew/pay_dtac.aspx?SG=" + sg + "&mp=" + mp + "&prj=" + prj + "&SCS_id=" + scsId, false);
                        }
                        else if (opt == "03" || opt == "04")
                        {
                            response.Redirect("http://wap.isport.co.th/isportui/indexh.aspx?p=bctrue", false);
                        }
                        else response.Redirect("http://wap.isport.co.th/isportui/indexh.aspx?p=errorip", false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("isport_service.WapCheckAllService >> " + ex.Message);
            }
        }
        #endregion
    }
}
