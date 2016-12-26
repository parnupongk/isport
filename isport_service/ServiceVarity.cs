using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web;
using Microsoft.ApplicationBlocks.Data;
using OracleDataAccress;
using System.Data.OracleClient;
namespace isport_service
{
    public class ServiceVarity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strConn"></param>
        /// <param name="ctr"></param>
        /// <param name="sgId"></param>
        /// <param name="classId"></param>
        /// <param name="projectType"></param>
        /// <param name="pageRedirect"></param>
        /// <param name="pCatId"></param>
        /// <param name="sexType"></param>
        /// <param name="top">top =0 show all</param>
        public static void GenVarity(string strConn,  Control ctr, string sgId, string classId, string projectType, string pageRedirect,string pCatId,string sexType,int  top,bool isRandom)
        {
            try
            {
                DataTable dt = GetSipContentBypCatId(strConn, pCatId, sexType);
                top = (top == 0) ? dt.Rows.Count : top;
                string link = ""; int rRandom = 0;
                Random random = new Random();
                for (int i = 0; i < top && i < dt.Rows.Count; i++)
                {
                    rRandom = (isRandom) ? random.Next(0, dt.Rows.Count) : i;
                    DataRow dr = dt.Rows[rRandom];

                    link = pageRedirect + "&pcnt_id=" + dr["pcnt_id"].ToString(); //+ "&pcat=" + pCatId;
                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row featurette'><img src='http://tdedlove.com/images/sexgroup" + sexType + ".png' class='img'><a class='img' href='" + link + "'>" + dr["name"].ToString() + "</a></div>"));

                }
            }
            catch (Exception ex)
            {
                throw new Exception(" GenVarity>> " + ex.Message);
            }
        }

        /// <summary>
        /// GetSipContentBypCatId
        /// </summary>
        /// <param name="pCatId">default = 176</param>
        /// <param name="sexType">default = 4</param>
        /// <returns></returns>
        public static  DataTable GetSipContentBypCatId(string strConn,string pCatId, string sexType)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapui_varity_selectcontentbypcat"
                    , new SqlParameter[] { new SqlParameter("@pcat_id",pCatId) 
                                                        ,new SqlParameter("@ptype",sexType)});
                return ds.Tables.Count > 0 ? ds.Tables[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception("GetSipContentBypCatId >>" + ex.Message);
            }
        }

        public static DataTable GetSipContentDetailByPcntId(string strConn, string pcntId)
        {
            try
            {
                DataSet ds = OrclHelper.Fill(strConn, CommandType.StoredProcedure, "Wap_UI.SIP_Content_VarityDetailByPcnt", "i_sip_content"
        , new OracleParameter[] {OrclHelper.GetOracleParameter("p_pcnt",pcntId,OracleType.VarChar,ParameterDirection.Input)
                                                          ,OrclHelper.GetOracleParameter("o_content","",OracleType.Cursor,ParameterDirection.Output)});


                return ds.Tables.Count > 0 ? ds.Tables[0] : null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
