using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using WebLibrary;
using MobileLibrary;
using OracleDataAccress;
namespace WS_BB
{
    public partial class imobilesaletracker : System.Web.UI.Page
    {
        MobileUtilities muMobile = null;
        XmlDocument xmlDoc = new XmlDocument();
        XDocument rtnXML = new XDocument(new XElement("SportApp"));
        protected void Page_Load(object sender, EventArgs e)
        {
            rtnXML.Element("SportApp").Add(new XElement("Response"
                       , new XAttribute("isactive", false)));
            try
            {
                if(  Request["model"] != null && Request["imei"] != null )
                {

                #region Insert and Post Data
                muMobile = Utilities.getMISDN(Request);
                string opt = "520";
                if (muMobile.mobileOPT != null && muMobile.mobileOPT == "01")
                {
                    opt += "01";
                }
                else if( muMobile.mobileOPT != null && muMobile.mobileOPT == "02" )
                {
                    opt += "18";
                }
                else if (muMobile.mobileOPT != null && muMobile.mobileOPT == "03")
                {
                    opt += "99";
                }
                else if (muMobile.mobileOPT != null && muMobile.mobileOPT == "04")
                {
                    opt += "00";
                }

                //AppCode_Utility.GetOperatorByIMSI(Utilities.getOTPTypeByIMSI(Request["imsi"]));

                DataSet ds = OrclHelper.Fill(AppCode_Base.strConnOracle, CommandType.StoredProcedure, "ISPORT_IMOBILE.SportApp_Select_SaleTracker","imobile"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("P_IMEI", Request["imei"] == null ? "" : Request["imei"], OracleType.VarChar, ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("P_MSISDN", muMobile.mobileNumber == null ? "" : muMobile.mobileNumber, OracleType.VarChar, ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("o_cursor", "", OracleType.Cursor, ParameterDirection.Output) });

                if ((!(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)) || (ConfigurationManager.AppSettings["imobile_saletracker_allowlist"].IndexOf(muMobile.mobileNumber) > -1 && muMobile.mobileNumber != null && muMobile.mobileNumber != "") || (ConfigurationManager.AppSettings["imobile_saletracker_allowimei"].IndexOf(Request["imei"]) > -1))
                {
                    string url = String.Format(ConfigurationManager.AppSettings["imobile_slaetracker_urlpost"], Request["model"], opt, muMobile.mobileNumber, Request["imei"], Request["imsi"], Request["imsi1"]
                                                            , Request["cid"], Request["lac"], Request["psc"], Request["latitude"], Request["longitude"], Request["imei1"]);
                    string rtn = new push().SendGet(url);
                    if (rtn.IndexOf("DONE") > 0)
                    {
                        rtnXML.Element("SportApp").Element("Response").Attribute("isactive").SetValue("true");
                        //rtnXML.Element("SportApp").Add(new XElement("Response"
                        //        , new XAttribute("isactive", true)));
                    }

                    int status = OrclHelper.ExecuteNonQuery(AppCode_Base.strConnOracle, CommandType.StoredProcedure, "ISPORT_IMOBILE.SportApp_Insert_SaleTracker"
                        , new OracleParameter[] {OrclHelper.GetOracleParameter("P_ID",Guid.NewGuid().ToString(),OracleType.VarChar,ParameterDirection.Input)
                                                                ,OrclHelper.GetOracleParameter("P_CREATE_DATE",DateTime.Now,OracleType.DateTime,ParameterDirection.Input)
                                                                ,OrclHelper.GetOracleParameter("P_MODEL",Request["model"]==null?"":Request["model"],OracleType.VarChar,ParameterDirection.Input)
                                                                ,OrclHelper.GetOracleParameter("P_OPT",Request["opt"]==null?"":Request["opt"],OracleType.VarChar,ParameterDirection.Input)
                                                                ,OrclHelper.GetOracleParameter("P_MSISDN",muMobile.mobileNumber,OracleType.VarChar,ParameterDirection.Input)
                                                                ,OrclHelper.GetOracleParameter("P_IMEI",Request["imei"]==null?"":Request["imei"],OracleType.VarChar,ParameterDirection.Input)
                                                                ,OrclHelper.GetOracleParameter("P_IMSI",Request["imsi"]==null?"":Request["imsi"],OracleType.VarChar,ParameterDirection.Input)
                                                                ,OrclHelper.GetOracleParameter("P_IMSI1",Request["imsi1"]==null?"":Request["imsi1"],OracleType.VarChar,ParameterDirection.Input)
                                                                ,OrclHelper.GetOracleParameter("P_CID",Request["cid"] == null ? "" : Request["cid"] ,OracleType.VarChar,ParameterDirection.Input)
                                                                ,OrclHelper.GetOracleParameter("P_LAC",Request["lac"] == null ? "" : Request["lac"],OracleType.VarChar,ParameterDirection.Input)
                                                                ,OrclHelper.GetOracleParameter("P_PSC",Request["psc"] == null ? "" : Request["psc"],OracleType.VarChar,ParameterDirection.Input)
                                                                ,OrclHelper.GetOracleParameter("P_LATITUDE",Request["v"] == null ? "1" : Request["v"],OracleType.VarChar,ParameterDirection.Input)
                                                                ,OrclHelper.GetOracleParameter("P_LONGITUDE",Request.ServerVariables["REMOTE_ADDR"].ToString(),OracleType.VarChar,ParameterDirection.Input)
                                                                ,OrclHelper.GetOracleParameter("P_RETURN",rtn,OracleType.VarChar,ParameterDirection.Input)
                                                                ,OrclHelper.GetOracleParameter("P_IMEI1",Request["imei1"]==null?"":Request["imei1"],OracleType.VarChar,ParameterDirection.Input)
                                                                });

                    //WebLibrary.LogsManager.WriteLogs("imobilesaletracker", "insert status : " + status, "");
                }
                else if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    rtnXML.Element("SportApp").Element("Response").Attribute("isactive").SetValue("true");
                }

                #endregion

                }
            }
            catch (Exception ex)
            {
                WebLibrary.LogsManager.WriteLogs("imobilesaletracker_error", ex.Message, "");
            }
            xmlDoc.Load(rtnXML.CreateReader());
            string json = AppCode_XmlToJson.XmlToJSON(xmlDoc);
            Response.Write(json);
        }
    }
}
