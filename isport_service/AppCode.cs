using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
namespace isport_service
{
    public class AppCode
    {
        public enum Country
        {
            en
            , th
        }
        public static string DatetoText(string date, string country)
        {
            try
            {
                string countryInfo = (country == Country.th.ToString()) ? "th-TH" : "en-US";
                System.Globalization.CultureInfo thai = new System.Globalization.CultureInfo(countryInfo);
                return DateTime.ParseExact(date, "yyyyMMdd", thai).ToString("dd/MM/yyyy", thai);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                string countryInfo = "en-US";
                System.Globalization.CultureInfo thai = new System.Globalization.CultureInfo(countryInfo);
                return DateTime.ParseExact(date, "yyyyMMdd", thai).ToString("dd/MM/yyyy", thai);
            }
        }

        /// <summary>
        /// SelectFooterByoperator
        /// </summary>
        /// <param name="operatorName"></param>
        /// <returns></returns>
        public DataSet SelectFooterByoperator(string strConn,string operatorName, string projectType)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_footerselectbyoperator", ds
                    , new string[] { "wapisport_footer" }
                    , new SqlParameter[] { new SqlParameter("@footer_operator",operatorName) 
                    ,new SqlParameter("@footer_projecttype",projectType)});
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// SelectHeaderByOperator
        /// </summary>
        /// <param name="operatorName"></param>
        /// <returns></returns>
        public DataSet SelectHeaderByOperator(string strConn, string operatorName, string projectType)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_headerselectbyoperator", ds
                    , new string[] { "wapisport_header" }
                    , new SqlParameter[] { new SqlParameter("@header_operator", operatorName)
                    ,new SqlParameter("@header_projecttype",projectType)});
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// SelectUIByLevel ** Fillter Enddate
        /// </summary>
        /// <param name="master_id"></param>
        /// <param name="ui_level"></param>
        /// <returns></returns>
        public DataSet SelectUIByLevel_Wap(string strConn ,string master_id, string ui_level, string operatorName, string ui_projecttype)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_uiselectbylevel_wap", ds
                    , new string[] { "wapisport_ui" }
                    , new SqlParameter[]{new SqlParameter("@ui_level",ui_level)
                    , new SqlParameter("@master_id",master_id)
                    , new SqlParameter("@ui_operator",operatorName)
                    ,new SqlParameter("@ui_projecttype",ui_projecttype)});
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
