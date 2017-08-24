using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using Microsoft.ApplicationBlocks.Data;
using OracleDataAccress;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
namespace isport
{

    public class GoogleResponse
    {
        public string Kind { get; set; }
        public string Id { get; set; }
        public string LongUrl { get; set; }
    }
    public class AppCode : AppMain
    {

        #region Google API URLShoter Framwork4.0
        public static string GetShortURL(string longUrl)
        {
            WebRequest request = WebRequest.Create("https://www.googleapis.com/urlshortener/v1/url?key=AIzaSyBVEcI59w8iUqBInxCuT2-RpSwYjyHZ0cA");
            request.Method = "POST";
            request.ContentType = "application/json";
            string requestData = string.Format(@"{{""longUrl"": ""{0}""}}", longUrl);
            byte[] requestRawData = Encoding.ASCII.GetBytes(requestData);
            request.ContentLength = requestRawData.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(requestRawData, 0, requestRawData.Length);
            requestStream.Close();

            WebResponse response = request.GetResponse();
            StreamReader responseReader = new StreamReader(response.GetResponseStream());
            string responseData = responseReader.ReadToEnd();
            responseReader.Close();

            var deserializer = new JavaScriptSerializer();
            var results = deserializer.Deserialize<GoogleResponse>(responseData);
            return results.Id;
        }


        #endregion

        #region Match Quiz
        public int MatchQuizDeleteByMschId(string pcntId)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.StoredProcedure, "usp_match_quiz_delect"
                    , new SqlParameter[] { new SqlParameter("@pcnt_id", pcntId) });
            }
            catch (Exception ex)
            {
                throw new Exception("MatchQuizDeleteByMschId >> " + ex.Message);
            }
        }
        #endregion

        #region Sip sub service
        public DataTable Select_SipSubService(string pcatId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConnPack, CommandType.StoredProcedure, "usp_isport_getsipsubservice"
                                                                        ,new SqlParameter[]{new SqlParameter("@pcat_id",pcatId)});
                return ds.Tables.Count > 0 ? ds.Tables[0] : null ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Sip Content

        public DataSet Select_SipContentBuCatID(string pcatId)
        {
            try
            {
                return SqlHelper.ExecuteDataset(strConnPack, CommandType.StoredProcedure, "usp_wapui_selectsipcontentbycatid"
                    , new SqlParameter[] { new SqlParameter("@pcat_id",pcatId) });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Update_SipContent
        /// </summary>
        /// <param name="pcnt_id"></param>
        /// <param name="pcntTitleLocal"></param>
        /// <param name="pcntDetail"></param>
        /// <param name="pcntDetailLocal"></param>
        /// <param name="displayDate"></param>
        /// <param name="updateBy"></param>
        /// <returns></returns>
        public string Update_SipContent(string pcnt_id, string pcntTitleLocal, string pcntDetail,string pcntDetailLocal,DateTime displayDate,string updateBy)
        {
            try
            {
                /*OrclHelper.ExecuteNonQuery(strConnOracle, CommandType.StoredProcedure, "WAP.UPDATE_I_SIP_CONTENT"
                    , new OracleParameter[] {OrclHelper.GetOracleParameter("p_pcnt_id",pcnt_id,OracleType.Number,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_pcnt_title_local",pcntTitleLocal,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_pcnt_detail",pcntDetail,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_update_date",DateTime.Now,OracleType.DateTime,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_update_by",updateBy,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_pcnt_detail_local",pcntDetailLocal,OracleType.VarChar,ParameterDirection.Input)});*/

                return SqlHelper.ExecuteNonQuery(strConnPack, CommandType.StoredProcedure, "usp_isport_update_sipcontent"
                    , new SqlParameter[] { new SqlParameter("@pcnt_id",pcnt_id)
                                                       ,new SqlParameter("@pcnt_title_local",pcntTitleLocal)
                                                        ,new SqlParameter("@pcnt_display_date",displayDate)
                                                        ,new SqlParameter("@pcnt_detail",pcntDetail)
                                                        ,new SqlParameter("@pcnt_update_by",updateBy)
                                                        ,new SqlParameter("@pcnt_detail_local",pcntDetailLocal)}).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pcnt_id"></param>
        /// <returns></returns>
        public string Delete_SipContent(string pcnt_id)
        {
            try
            {
                try
                {
                    OrclHelper.ExecuteNonQuery(strConnOracle, CommandType.StoredProcedure, "WAP.DEL_I_SIP_CONTENT"
                        , new OracleParameter[] { OrclHelper.GetOracleParameter("p_pcnt_id", pcnt_id, OracleType.Number, ParameterDirection.Input) });
                }
                catch { }
                return SqlHelper.ExecuteNonQuery(strConnPack, CommandType.StoredProcedure, "usp_isport_delete_sipcontent"
                    , new SqlParameter[] {new SqlParameter("@pcnt_id",pcnt_id) }).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Insert_SipContent
        /// </summary>
        /// <param name="pcatId"></param>
        /// <param name="pcntTitle">url link</param>
        /// <param name="pcntTitleLocal">text sms</param>
        /// <param name="displayDate">YYYYMMdd</param>
        /// <param name="createBy"></param>
        /// <returns></returns>
        public string  Insert_SipContent(string pcatId,string pcntTitle,string pcntTitleLocal,string pcntDetail,string pcntDetailLocal,string displayDate,string createBy,string isSendNow)
        {
            try
            {
                string pcntId = SqlHelper.ExecuteScalar(strConnPack, CommandType.StoredProcedure, "usp_insert_sip_content"
                    , new SqlParameter[] { new SqlParameter("@pcat_id",pcatId)
                                                        ,new SqlParameter("@pcnt_title",pcntTitle) 
                                                        ,new SqlParameter("@pcnt_title_local",pcntTitleLocal)
                                                        ,new SqlParameter("@pcnt_detail",pcntDetail) //sms
                                                        ,new SqlParameter("@pcnt_detail_local",pcntDetailLocal) // sms
                                                        ,new SqlParameter("@pcnt_sendnow",isSendNow)
                                                        ,new SqlParameter("@pcnt_display_date",displayDate)
                                                        ,new SqlParameter("@create_by",createBy)
                                                        ,new SqlParameter("@update_date",DateTime.Now)
                                                        ,new SqlParameter("@update_by",createBy)
                                                        ,new SqlParameter("@pic_path_70","")
                                                        ,new SqlParameter("@pic_path_36","")}).ToString();
                try
                {
                    Insert_SipContent_Oracle(pcntId, pcatId, pcntTitle, pcntTitleLocal, pcntDetail, pcntDetailLocal, displayDate, createBy, isSendNow);
                }
                catch { }
                return pcntId;
            }
            catch (Exception ex)
            {
                throw new Exception("Insert_SipContent>>" + ex.Message);
            }
        }

        /// <summary>
        /// Insert_SipContent ใช้กับ edtgame
        /// </summary>
        /// <param name="pcatId"></param>
        /// <param name="pcntTitle">url link</param>
        /// <param name="pcntTitleLocal">text sms</param>
        /// <param name="displayDate">YYYYMMdd</param>
        /// <param name="createBy"></param>
        /// <returns></returns>
        public string Insert_SipContent(string pcatId, string pcntTitle, string pcntTitleLocal, string pcntDetail, string pcntDetailLocal, string displayDate, string createBy, string isSendNow,string pathImage)
        {
            try
            {
                string pcntId = SqlHelper.ExecuteScalar(strConnPack, CommandType.StoredProcedure, "usp_insert_sip_content"
                    , new SqlParameter[] { new SqlParameter("@pcat_id",pcatId)
                                                        ,new SqlParameter("@pcnt_title",pcntTitle)
                                                        ,new SqlParameter("@pcnt_title_local",pcntTitleLocal)
                                                        ,new SqlParameter("@pcnt_detail",pcntDetail) //sms
                                                        ,new SqlParameter("@pcnt_detail_local",pcntDetailLocal) // sms
                                                        ,new SqlParameter("@pcnt_sendnow",isSendNow)
                                                        ,new SqlParameter("@pcnt_display_date",displayDate)
                                                        ,new SqlParameter("@create_by",createBy)
                                                        ,new SqlParameter("@update_date",DateTime.Now)
                                                        ,new SqlParameter("@update_by",createBy)
                                                        ,new SqlParameter("@pic_path_70",pathImage)
                                                        ,new SqlParameter("@pic_path_36","")}).ToString();

                Insert_SipContent_Oracle(pcntId, pcatId, pcntTitle, pcntTitleLocal, pcntDetail, pcntDetailLocal, displayDate, createBy, isSendNow);

                return pcntId;
            }
            catch (Exception ex)
            {
                throw new Exception("Insert_SipContent>>" + ex.Message);
            }
        }

        /// <summary>
        /// Insert_SipContent_EDT (edt จะต้องทำการ check ก่อนการ insert เพราะเป็นการ upload excel กรณี upload ซ้ำ)
        /// </summary>
        /// <param name="pcatId"></param>
        /// <param name="pcntTitle">url link</param>
        /// <param name="pcntTitleLocal">text sms</param>
        /// <param name="displayDate">YYYYMMdd</param>
        /// <param name="createBy"></param>
        /// <returns></returns>
        public string Insert_SipContent_EDT(string pcatId, string pcntTitle, string pcntTitleLocal, string pcntDetail, string pcntDetailLocal, string displayDate, string createBy, string isSendNow,string typeContent)
        {
            try
            {
                string pcntId = SqlHelper.ExecuteScalar(strConnPack, CommandType.StoredProcedure, "usp_insert_sip_content_edt"
                    , new SqlParameter[] { new SqlParameter("@pcat_id",pcatId)
                                                        ,new SqlParameter("@pcnt_title",pcntTitle) 
                                                        ,new SqlParameter("@pcnt_title_local",pcntTitleLocal)
                                                        ,new SqlParameter("@pcnt_detail",pcntDetail) //sms
                                                        ,new SqlParameter("@pcnt_detail_local",pcntDetailLocal) // sms
                                                        ,new SqlParameter("@pcnt_sendnow",isSendNow)
                                                        ,new SqlParameter("@pcnt_display_date",displayDate)
                                                        ,new SqlParameter("@create_by",createBy)
                                                        ,new SqlParameter("@update_date",DateTime.Now)
                                                        ,new SqlParameter("@update_by",createBy)
                                                        ,new SqlParameter("@pic_path_70",typeContent)
                                                        ,new SqlParameter("@pic_path_36","")}).ToString();

                Insert_SipContent_Oracle(pcntId, pcatId, pcntTitle, pcntTitleLocal, pcntDetail, pcntDetailLocal, displayDate, createBy, isSendNow);

                return pcntId;
            }
            catch (Exception ex)
            {
                throw new Exception("Insert_SipContent>>" + ex.Message);
            }
        }

        /// <summary>
        /// Insert_SipContent
        /// </summary>
        /// <param name="pcatId"></param>
        /// <param name="pcntTitle">url link</param>
        /// <param name="pcntTitleLocal">text sms</param>
        /// <param name="displayDate">YYYYMMdd</param>
        /// <param name="createBy"></param>
        /// <returns></returns>
        public void Insert_SipContent_Oracle(string pcntId,string pcatId, string pcntTitle, string pcntTitleLocal, string pcntDetail, string pcntDetailLocal, string displayDate, string createBy, string isSendNow)
        {
            try
            {
                
                DateTime date = new DateTime(int.Parse(displayDate.Substring(0, 4)), int.Parse(displayDate.Substring(4, 2)), int.Parse(displayDate.Substring(6, 2)), DateTime.Now.Hour, DateTime.Now.Minute, 0);

                OrclHelper.ExecuteNonQuery(strConnOracle, CommandType.StoredProcedure, "WAP.INST_I_SIP_CONTENT"
                    , new OracleParameter[] {OrclHelper.GetOracleParameter("p_pcnt_id",pcntId,OracleType.Number,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_pcat_id",pcatId,OracleType.Int16,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_pcnt_title",pcntTitle,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_pcnt_title_local",pcntTitleLocal,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_pcnt_detail",pcntDetail,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_pcnt_detail_local",pcntDetailLocal,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_pcnt_sendnow",isSendNow,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_pcnt_display_date",date,OracleType.DateTime,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_create_by",createBy,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_update_date",DateTime.Now,OracleType.DateTime,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_update_by",createBy,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_pic_path_70","",OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_pic_path_36","",OracleType.VarChar,ParameterDirection.Input)});

            }
            catch (Exception ex)
            {
                throw new Exception("Insert_SipContent_Oracle >>" + ex.Message);
            }
        }
        #endregion

        #region Get Sport Content
        public DataRow GetAdminUsers(string usrName, string usrPass)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapui_selectusers"
                    , new SqlParameter[] { new SqlParameter("@usrName", usrName), new SqlParameter("@usrPass", usrPass) });

                return (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) ? ds.Tables[0].Rows[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int GetCountSportContent(string scsId,string catId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_sportcontentbycatid"
                    , new SqlParameter[] { new SqlParameter("@scs_id",scsId),new SqlParameter("@cat_id",catId) });

                return (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) ? ds.Tables[0].Rows.Count : 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet  GetSportContent(string scsId, string catId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_sportcontentbycatid"
                    , new SqlParameter[] { new SqlParameter("@scs_id", scsId), new SqlParameter("@cat_id", catId) });

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Sport Class
        public string GetClassNameByClassId(string classId,string lang)
        {
            try
            {
                string rtn = "";
                DataSet ds  = SqlHelper.ExecuteDataset(strConn,CommandType.StoredProcedure,"usp_wapisport_sportclassbyclassid"
                    ,new SqlParameter[]{new SqlParameter("@class_id",classId)});
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    rtn = (lang.ToLower() == "e") ? ds.Tables[0].Rows[0]["class_name"].ToString() : ds.Tables[0].Rows[0]["class_name_local"].ToString();
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("GetClassNameByClassId>>" + ex.Message);
            }
        }
        #endregion

        #region Get Service Group
        public string GetServiceGroupBySgId(string sgId)
        {
            try
            {
                string rtn = "";
                using (OracleConnection oConn = new OracleConnection(strConnOracle))
                {
                    OracleParameter oPut = new OracleParameter();
                    oPut.ParameterName = "Content_level";
                    oPut.OracleType = OracleType.Int32;
                    oPut.Direction = ParameterDirection.Output;

                    OracleParameter iPut = new OracleParameter("p_sg_id", sgId);
                    iPut.Direction = ParameterDirection.Input;
                    iPut.OracleType = OracleType.Int32;

                    object obj = OrclHelper.ExecuteScalar(oConn, CommandType.StoredProcedure
                    , "WAP_PAYMENT.ServiceGroup_GetContentLevel"
                    , new OracleParameter[] { iPut, oPut });

                    rtn = obj == null ? "" : obj.ToString();
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get News
        public DataTable News_Select(string pcatId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConnPack, CommandType.StoredProcedure, "usp_wapui_sport_news",
                    new SqlParameter[] { new SqlParameter("@pcat_id", pcatId)
                    ,new SqlParameter("@select_top",3)});
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable  News_SelectOracle(string top,string class_id)
        {
            try
            {
                DataSet ds =  null;
                int topRow = int.Parse(top);
                using(OracleConnection conn = new OracleConnection(strConnOracle) )
                {
                    ds = OracleDataAccress.OrclHelper.Fill(conn, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsBySportClass", "siamsport_news"
                        , new OracleParameter[] {OracleDataAccress.OrclHelper.GetOracleParameter("p_sportclass",class_id,OracleType.VarChar ,ParameterDirection.Input)
                                                            ,OracleDataAccress.OrclHelper.GetOracleParameter("p_rowmax",topRow,OracleType.Int32,ParameterDirection.Input )
                                                            ,OracleDataAccress.OrclHelper.GetOracleParameter("p_rowmin",0,OracleType.Int32,ParameterDirection.Input ) 
                                                            ,OracleDataAccress.OrclHelper.GetOracleParameter("p_sportType","00001",OracleType.VarChar ,ParameterDirection.Input ) 
                                                             ,OracleDataAccress.OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor ,ParameterDirection.Output ) });
                }
                return ds != null && ds.Tables.Count>0 ? ds.Tables[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable News_SelectByIdOracle(string newsId)
        {
            try
            {
                DataSet ds = null;
                using (OracleConnection conn = new OracleConnection(strConnOracle))
                {
                    ds = OracleDataAccress.OrclHelper.Fill(conn, CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetNewsById", "siamsport_news"
                        , new OracleParameter[] {OracleDataAccress.OrclHelper.GetOracleParameter("p_newsId",newsId,OracleType.VarChar ,ParameterDirection.Input ) 
                                                             ,OracleDataAccress.OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor ,ParameterDirection.Output ) });
                }
                return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Header
        /// <summary>
        /// DeleteHeader
        /// </summary>
        /// <param name="headerID"></param>
        /// <returns></returns>
        public int DeleteHeader(string headerID)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();

                    rtn = SqlHelper.ExecuteNonQuery(sqlTran, CommandType.StoredProcedure, "usp_wapisport_headerdeletebyid"
                        , new SqlParameter[] { new SqlParameter("@header_id", headerID) });

                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQuery(sqlTran, CommandType.StoredProcedure, "usp_wapisport_contentdeletebymasterid"
                        , new SqlParameter[] { new SqlParameter("@master_id", headerID) });

                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
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
        public DataSet SelectHeaderByOperator(string operatorName,string projectType)
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
        /// SelectHeaderAll
        /// </summary>
        /// <returns></returns>
        public DataSet SelectHeaderAll(string projectType)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_headerselectall", ds
                    , new string[] { "wapisport_header" }, new SqlParameter[] { new SqlParameter("@header_projecttype",projectType) });
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// UpdateHeader
        /// </summary>
        /// <param name="drHeader"></param>
        /// <param name="drContent"></param>
        /// <returns></returns>
        public int UpdateHeader(isportDS.wapisport_headerRow drHeader, isportDS.wapisport_contentRow drContent)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();
                    rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_headerupdate", drHeader);
                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_contentupdate", drContent);
                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// InsertHeader
        /// </summary>
        /// <param name="drHeader"></param>
        /// <param name="drContent"></param>
        /// <returns></returns>
        public int InsertHeader(isportDS.wapisport_headerRow drHeader, isportDS.wapisport_contentRow drContent)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();
                    rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_headerinsert", drHeader);
                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_contentinsert", drContent);
                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region UI

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectType"></param>
        /// <returns></returns>
        public string DeleteUIByProjectType(string projectType)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, CommandType.StoredProcedure, "usp_wapisport_uideletebyprojecttype"
                    , new SqlParameter[] { new SqlParameter("@ui_projecttype",projectType) }).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// SelectUIGroupProjectType
        /// </summary>
        /// <returns></returns>
        public DataSet SelectUIGroupProjectType()
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_uigroupprojectype", ds
                    , new string[] { "wapisport_ui" });
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// SelectUIGroupProjectTypeNotNull
        /// </summary>
        /// <returns></returns>
        public DataSet SelectUIGroupProjectTypeNotNull()
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_uigroupprojectypenotnull", ds
                    , new string[] { "wapisport_ui" });
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// SelectUIGroupProjectTypeByPcatId
        /// </summary>
        /// <returns></returns>
        public DataSet SelectUIGroupProjectTypeByPcatId(string pcatId)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_uigroupprojectypebypcatid", ds
                    , new string[] { "wapisport_ui" },new SqlParameter[]{new SqlParameter("@pcat_id",pcatId)});
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// SelectUIByID
        /// </summary>
        /// <param name="ui_id"></param>
        /// <returns></returns>
        public DataSet SelectUIByID(string ui_id)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_uiselectbyid", ds
                    , new string[] { "wapisport_ui" }
                    , new SqlParameter[]{new SqlParameter("@ui_id",ui_id)});
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// SelectUIByLevel ** admin
        /// </summary>
        /// <param name="master_id"></param>
        /// <param name="ui_level"></param>
        /// <returns></returns>
        public DataSet SelectUIByLevel(string master_id, int ui_level,string ui_projecttype)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_uiselectbylevel", ds
                    , new string[] { "wapisport_ui" }
                    , new SqlParameter[]{new SqlParameter("@ui_level",ui_level)
                    , new SqlParameter("@master_id",master_id)
                    ,new SqlParameter("@ui_projecttype",ui_projecttype)});
                return ds;
            }
            catch(Exception ex)
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
        public DataSet SelectUIByLevel_Wap(string master_id, int ui_level,string operatorName,string ui_projecttype)
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

        /// <summary>
        /// DeleteUI
        /// </summary>
        /// <param name="ui_ID"></param>
        /// <returns></returns>
        public int DeleteUI(string ui_ID)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();

                    rtn = SqlHelper.ExecuteNonQuery(sqlTran, CommandType.StoredProcedure, "usp_wapisport_uideletebyid"
                        , new SqlParameter[]{new SqlParameter("@ui_id",ui_ID)});

                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQuery(sqlTran, CommandType.StoredProcedure, "usp_wapisport_contentdeletebymasterid"
                        , new SqlParameter[] { new SqlParameter("@master_id", ui_ID) });

                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// InsertUI
        /// </summary>
        /// <param name="drUI"></param>
        /// <param name="drContent"></param>
        /// <returns></returns>
        public int InsertUI(isportDS.wapisport_uiRow drUI,isportDS.wapisport_contentRow drContent )
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();
                    rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_uiinsert", drUI);
                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_contentinsert", drContent);
                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback(); 
                }

                try
                {
                    //InsertUIOracle(drUI, drContent);
                }
                catch (Exception ex) { WebLibrary.ExceptionManager.WriteError(ex.Message); }
                

                return rtn;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int InsertUIOracle(isportDS.wapisport_uiRow drUI, isportDS.wapisport_contentRow drContent)
        {
            try
            {
                int rtn = 0;
                rtn = OrclHelper.ExecuteNonQuery(strConnOracle, CommandType.StoredProcedure, "WAP_UI.wapisport_uiinsert"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_ui_id",drUI.ui_id,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_master_id",drUI.ui_master_id,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_projecttype",drUI.ui_projecttype,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_level",drUI.ui_level,OracleType.Int32,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_index",drUI.ui_index,OracleType.Int32,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_operator",drUI.ui_operator,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_startdate",drUI.ui_startdate,OracleType.DateTime,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_enddate",null ,OracleType.DateTime,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_createdate",(drUI.ui_createdate ==null ? drUI.ui_startdate : drUI.ui_createdate),OracleType.DateTime,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_updatedate",(drUI.ui_updatedate ==null ? drUI.ui_startdate : drUI.ui_updatedate),OracleType.DateTime,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_createuser",drUI.ui_createuser,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_updateuser",drUI.ui_updateuser,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_createip",drUI.ui_createip,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_updateip",drUI.ui_updateip,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_ismaster",drUI.ui_ismaster?1:0,OracleType.Int32,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_ispayment",drUI.ui_ispayment?1:0,OracleType.Int32,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_sg_id",drUI.ui_sg_id,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_isnews",drUI.ui_isnews?1:0,OracleType.Int32,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_isnews_top",drUI.ui_isnews_top,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_contentname",drUI.ui_contentname,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_ispaymentwap",drUI.ui_ispaymentwap?1:0,OracleType.Int32,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_ui_ispaymentsms",drUI.ui_ispaymentsms?1:0,OracleType.Int32,ParameterDirection.Input)
                                                         });
                rtn = OrclHelper.ExecuteNonQuery(strConnOracle, CommandType.StoredProcedure, "WAP_UI.wapisport_uiisert"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_content_id",drContent.content_id,OracleType.NVarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_master_id",drContent.master_id,OracleType.NVarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_content_icon",drContent.content_icon,OracleType.NVarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_content_createdate",drContent.content_createdate,OracleType.DateTime,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_content_image",drContent.content_image,OracleType.NVarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_content_link",drContent.content_link,OracleType.NVarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_content_text",drContent.content_text,OracleType.NVarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_content_align",drContent.content_align,OracleType.NVarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_content_breakafter",drContent.content_breakafter?1:0,OracleType.Int32,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_content_color",drContent.content_color,OracleType.NVarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_content_bold",drContent.content_color,OracleType.NVarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_content_isredirect",drContent.content_isredirect?1:0,OracleType.Int32,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_content_bgcolor",drContent.content_bgcolor,OracleType.NVarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_content_txtsize",drContent.content_txtsize,OracleType.NVarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_content_isgallery",drContent.content_isgallery?1:0,OracleType.Int32,ParameterDirection.Input)
                                                         });

                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("InsertUIOracle >> " + ex.Message);
                

            }
        }

        // <summary>
        /// InsertUI
        /// </summary>
        /// <param name="drUI"></param>
        /// <param name="drContent"></param>
        /// <returns></returns>
        public int InsertUI(isportDS.wapisport_uiRow drUI)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    rtn = SqlHelper.ExecuteNonQueryTypedParams(conn, "usp_wapisport_uiinsert", drUI);
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int UpdateUI(isportDS.wapisport_uiRow drUI)
        {
            try
            {
                return SqlHelper.ExecuteNonQueryTypedParams(strConn, "usp_wapisport_uiupdate", drUI);
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message);
            }
        }

        public int UpdateContentTitle(string ui_projecttype ,string content_text)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(strConn, "usp_wapisport_contentupdatetitle"
                    , new SqlParameter[] {new SqlParameter("@content_text", content_text)
                    ,new SqlParameter("@ui_projecttype", ui_projecttype)});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// UpdateUI
        /// </summary>
        /// <param name="drUI"></param>
        /// <param name="drContent"></param>
        /// <returns></returns>
        public int UpdateUI(isportDS.wapisport_uiRow drUI, isportDS.wapisport_contentRow drContent)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();
                    rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_uiupdate", drUI);
                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_contentupdate1", drContent);
                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Footer
        /// <summary>
        /// DeleteFooter
        /// </summary>
        /// <param name="footerID"></param>
        /// <returns></returns>
        public int DeleteFooter(string footerID)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();

                    rtn = SqlHelper.ExecuteNonQuery(sqlTran, CommandType.StoredProcedure, "usp_wapisport_footerdeletebyid"
                        , new SqlParameter[] { new SqlParameter("@footer_id", footerID) });

                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQuery(sqlTran, CommandType.StoredProcedure, "usp_wapisport_contentdeletebymasterid"
                        , new SqlParameter[] { new SqlParameter("@master_id", footerID) });

                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
 
        /// <summary>
        /// SelectFooterByoperator
        /// </summary>
        /// <param name="operatorName"></param>
        /// <returns></returns>
        public DataSet SelectFooterByoperator(string operatorName,string projectType)
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
        /// SelectFooterAll
        /// </summary>
        /// <returns></returns>
        public DataSet SelectFooterAll(string projectType)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapisport_footerselectall", ds
                    , new string[] { "wapisport_footer" }, new SqlParameter("@footer_projecttype",projectType));
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// UpdateFooter
        /// </summary>
        /// <param name="drUI"></param>
        /// <param name="drContent"></param>
        /// <returns></returns>
        public int UpdateFooter(isportDS.wapisport_footerRow drFooter, isportDS.wapisport_contentRow drContent)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();
                    rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_footerupdate", drFooter);
                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_contentupdate", drContent);
                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// InsertUI
        /// </summary>
        /// <param name="drFooter"></param>
        /// <param name="drContent"></param>
        /// <returns></returns>
        public int InsertFooter(isportDS.wapisport_footerRow drFooter, isportDS.wapisport_contentRow drContent)
        {
            try
            {
                int rtn = 0;
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlTransaction sqlTran = conn.BeginTransaction();
                    rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_footerinsert", drFooter);
                    if (rtn > 0)
                    {
                        rtn = SqlHelper.ExecuteNonQueryTypedParams(sqlTran, "usp_wapisport_contentinsert", drContent);
                        if (rtn > 0) sqlTran.Commit();
                        else sqlTran.Rollback();
                    }
                    else sqlTran.Rollback();
                }


                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Football liveScore
        public DataSet FootballLiveScoreSelectbyClass(string classId,string addTime,string time)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapui_football_livescore_select"
                    , ds, new string[] { },
                    new SqlParameter[] { new SqlParameter("@strAdd_time",addTime)
                    ,new SqlParameter("@strTime",time)
                    ,new SqlParameter("@class_id",classId) });
                return ds;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataSet FootballLiveScoreSelectbyMatch(string scsId, string matchDate, string strLng)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlHelper.FillDataset(strConn, CommandType.StoredProcedure, "usp_wapui_footballlivescore_selectmatchschedule"
                    , ds, new string[] { },
                    new SqlParameter[] { new SqlParameter("@scs_id_in",scsId)
                    ,new SqlParameter("@date_in",matchDate)
                    ,new SqlParameter("@strlng",strLng) });
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
