using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobileLibrary;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WebLibrary;
namespace isport
{
    public partial class confirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                #region main check
                string strMpCode = "";
                try
                {
                    MobileUtilities mU = Utilities.getMISDN(Request);
                    if (mU.mobileNumber != "" && mU.mobileOPT != "")
                    {
                        
                        string strPsnNUmber = "", strSNO = "";
                        if (Request["mp_code"] != null && Request["mp_code"].Split(',').Length > 0)
                        {
                            strMpCode = Request["mp_code"].Split(',')[0].ToString();
                        }

                        DataSet ds = new DataSet();
                        SqlHelper.FillDataset(AppCode.strConnPack, CommandType.StoredProcedure, "usp_wapisport_getsipservicenumber", ds, new string[] { "SIP_SERVICE_NUMBER" }
                        , new SqlParameter[] {new SqlParameter("@pssv_id",Request["pssv_id"])
                                                            ,new SqlParameter("@opt_code",mU.mobileOPT)
                                                            ,new SqlParameter("@psn_action",Request["command"])
                                                                });

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            strPsnNUmber = ds.Tables[0].Rows[0]["PSN_NUMBER2"].ToString();
                            strSNO = ds.Tables[0].Rows[0]["PSN_CODE"].ToString();
                        }

                        strMpCode = (strMpCode == "" || strMpCode == "0000") ? "0025" : strMpCode;

                        string strURL = "";
                        string strServiceCode = "";


                        switch (mU.mobileOPT)
                        {
                            case "01":
                                strURL = String.Format(ConfigurationManager.AppSettings["IsportSubScribeSMSAIS"], mU.mobileNumber, strPsnNUmber, "", Request["pro"], Request["pssv_id"], strMpCode); break;
                            case "02":
                                strServiceCode = strPsnNUmber.Substring(8, 2);
                                strSNO = strPsnNUmber.Substring(6, 2);
                                strURL = string.Format(ConfigurationManager.AppSettings["IsportSubScribeSMSDtac"], strServiceCode, mU.mobileNumber, strSNO, "", Request["pro"], strPsnNUmber, strMpCode); break;
                            default: // true
                                string strPSNServiceId = ds.Tables[0].Rows[0]["PSN_SERVICE_ID"].ToString();
                                strURL = string.Format(ConfigurationManager.AppSettings["IsportSubScribeSMSTrue"], strSNO, mU.mobileNumber, strPsnNUmber, strPSNServiceId, "", Request["pro"], strMpCode); break;
                        }

                        string rtn = new SendService.sendpost().SendGet(strURL);
                        ExceptionManager.WriteError("subscript result >> " + rtn + " URL : " + strURL);

                        if (Request["ad"] != null && Request["clickid"] != null)
                        {
                            string url = "https://code.yengo.com/cpa/track.js?ad={0}&c={1}";
                            rtn = new SendService.sendpost().SendGet(string.Format(url, Request["ad"], Request["clickid"]));
                            ExceptionManager.WriteError("postback yengo >>" + rtn);

                        }

                        if(strMpCode == "8021" && Request["click_id"] != null)
                        {
                            string url = "http://tk.pluckyaff.com/advBack.php?click_id={0}&adv_id=32&security_code=6803565087199bbc24493b13636641d4";
                            rtn = new SendService.sendpost().SendGet(string.Format(url, Request["click_id"]));
                            ExceptionManager.WriteError("M&M >>" + rtn);
                        }

                        string r = (Request["p"] != null && Request["p"] != "") ? Request["p"] :"confirm";

                        Response.Redirect("http://wap.isport.co.th/isportui/index.aspx?p="+r+"&prj=" + Request["prj"] + "&mp_code=" + strMpCode + "&p1=" + Request["p"], false);

                    }
                    else
                    {
                        pageError("can not find msisdn or telco");
                        Response.Redirect("http://wap.isport.co.th/isportui/index.aspx?p=errorip&prj=" + Request["prj"] + "&mp_code=" + strMpCode + "&p1=" + Request["p"], false);
                        
                    }

                }
                catch (Exception ex)
                {
                    ExceptionManager.WriteError("error >> " + ex.Message);
                    Response.Redirect("http://wap.isport.co.th/isportui/index.aspx?p=errorip&prj=" + Request["prj"] + "&mp_code=" + strMpCode + "&p1=" + Request["p"], false);
                }

                #endregion

            }
            else
            {
                pageError("ispostback");
            }

        }
        

        private void pageError(string mess)
        {
            ExceptionManager.WriteError("page error >> " + mess);
        }
    }
}