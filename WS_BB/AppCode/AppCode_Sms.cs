using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;


namespace WS_BB
{
    public class AppCode_Sms : AppCode_Base
    {

        public struct Out_Sms
        {
            
            public string status;
            public string Mess;
            public string subTitle;
            public List<Sms> sms;
        }
        public class Sms
        {
            public string pssv_id;
            public string psv_id;
            public string pssv_desc;
            public string pssv_desc_local;

        }
        public Out_Sms CommandGetSmsService()
        {
            Out_Sms outSms = new Out_Sms();
            List<Sms> sms = new List<Sms>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConnPack, CommandType.StoredProcedure, "usp_BB_getservice_sms");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Sms s = new Sms();
                    s.pssv_id = dr["pssv_id"].ToString();
                    s.psv_id = dr["psv_id"].ToString();
                    s.pssv_desc = dr["pssv_desc"].ToString();
                    s.pssv_desc_local = dr["pssv_desc_local"].ToString();
                    sms.Add(s);
                }
            }
            catch (Exception ex)
            {
                outSms.status = "Error";
                outSms.Mess = ex.Message;
                WebLibrary.ExceptionManager.WriteError("CommandGetSmsService >> " + ex.Message);
            }
            
            outSms.sms = sms;
            return outSms;
        }

    }
}
