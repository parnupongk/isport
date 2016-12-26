using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using OracleDataAccress;
using System.Data;
using System.Data.OracleClient;
using WebLibrary;
namespace WS_BB
{
    /// <summary>
    /// Summary description for imobilegame1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class imobilegame1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string iSoccerRegis(string mobileNo,string imei, string name, string lastName, string idCard, string brithDay, string sex, string region, string email, string phone,string mobileAgent,string otpCode,string appName)
        {
            response rtn = new response();
            try
            {

                SqlHelper.ExecuteNonQuery(AppCode_Base.strConn, "usp_game_customer_inst"
                    , new SqlParameter[] {new SqlParameter("@pphone_no", mobileNo)
                    ,new SqlParameter("@pfname", name)
                    ,new SqlParameter("@plname", lastName)
                    ,new SqlParameter("@pid_card", idCard)
                    ,new SqlParameter("@psex", sex)
                    ,new SqlParameter("@pdistinct", region)
                    ,new SqlParameter("@pemail", email)
                    ,new SqlParameter("@pphone_no2", phone)
                    ,new SqlParameter("@mobile_agent", "")
                    ,new SqlParameter("@popt_code", "")
                    ,new SqlParameter("@pgame_name", AppCode_Base.AppName.iSoccer.ToString())});
                //ExceptionManager.WriteError(muMobile.mobileNumber + "|" + s2 + "|" + idCard + " " + birthDay + " " + sex + " " + Server.UrlDecode(region) + " " + phone);

                int obj = OrclHelper.ExecuteNonQuery(AppCode_Base.strConnOracle, CommandType.StoredProcedure, "games.customer_insert"
                    , new OracleParameter[] {OrclHelper.GetOracleParameter("p_phone_no",mobileNo,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_imei",imei,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_fname",name,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_lname",lastName,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_id_card",idCard,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_birthdate",brithDay,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_sex",sex,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_distinct",region,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_email",email,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_phone_no2",phone,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_mobile_agent",mobileAgent,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_opt_code",otpCode,OracleType.VarChar,ParameterDirection.Input)
                                            ,OrclHelper.GetOracleParameter("p_game_name",appName,OracleType.VarChar,ParameterDirection.Input)
                                            });

                ExceptionManager.WriteError("insert status : "+obj);
                rtn.status = "success";
                rtn.message = "";
            }
            catch(Exception ex)
            {
                rtn.status = "error";
                rtn.message = ex.Message;
                ExceptionManager.WriteError("insert status : " + ex.Message);
            }
            return new JavaScriptSerializer().Serialize(rtn);
        }
    }
    public class response
    {
        public string status { get; set; }
        public string message { get; set; }
    }
    public class customer
    {
        public string mobileNo { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public string idCard { get; set; }
        public string brithDay { get; set; }
        public string sex { get; set; }
        public string region { get; set; }
        public string email { get; set; }
        public string phone { get; set; }

    }
}
