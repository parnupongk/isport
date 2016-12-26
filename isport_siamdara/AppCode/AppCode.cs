using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
namespace isport_siamdara
{
    public class AppCode
    {
        public static string CheckSmsSubScribe(string opt, string msisdn,string pssvId)
        {
            try
            {
                string rtn = "Y";
                rtn = isport_service.ServiceSMS.MemberSms_CheckActive(ConfigurationManager.ConnectionStrings["IsportConnectionString"].ToString(), pssvId, opt, msisdn);
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("CheckSmsSubScribe >> " + ex.Message);
            }
        }
    }
}
