using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
namespace isport_service
{
    public class ServiceSMS
    {
        public static string MemberSms_CheckActive(string strConn, string pssvID, string optCode, string phoneNo)
        {
            try
            {
                string rtn = "";
                SqlDataReader dr =SqlHelper.ExecuteReader(strConn, CommandType.StoredProcedure, "usp_wapisport_membersms_checkactive"
                    ,new SqlParameter[] { new SqlParameter("@pssv_id",pssvID)
                    ,new SqlParameter("@opt_code",optCode)
                    ,new SqlParameter("@phone_no",phoneNo)});
                while(dr.Read() )
                {
                    rtn = dr["dupsubc_flag"].ToString();
                }
                return rtn;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
