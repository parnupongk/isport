using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
namespace isport
{
    public class AppCodeSportCC : AppMain
    {
        public enum MatchType
        {
            Finished
            ,
            inprogress
                ,
            Abandoned
                ,
            NSY
                , Postponed
        }

        /// <summary>
        /// GetCountScore
        /// </summary>
        /// <param name="matchType"></param>
        /// <param name="addDate"></param>
        /// <param name="isByWeek">Y or N</param>
        /// <returns></returns>
        public string GetCountScore(MatchType matchType,decimal addDate,string isByWeek)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballscore"
                    , new SqlParameter[] {new SqlParameter("@dateadd",addDate)
                    ,new SqlParameter("@byweek",isByWeek)
                    ,new SqlParameter("@status",matchType.ToString())
                    ,new SqlParameter("@contestGroupId","")
                    });

                return ds.Tables.Count > 0 ? ds.Tables[0].Rows.Count.ToString() : "0";
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// GetCountScore
        /// </summary>
        /// <param name="matchType"></param>
        /// <param name="addDate"></param>
        /// <param name="isByWeek">Y or N</param>
        /// <returns></returns>
        public DataSet GetDataScore(MatchType matchType, decimal addDate, string isByWeek)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballscore"
                    , new SqlParameter[] {new SqlParameter("@dateadd",addDate)
                    ,new SqlParameter("@byweek",isByWeek)
                    ,new SqlParameter("@status",matchType.ToString())
                    ,new SqlParameter("@contestGroupId","")
                    });

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataSet GetDataFootballTable(string contestGroupId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballtable"
                    , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId) }); // where ด้วย contestgroupId

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ต้องส่ง ContestgroupId
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <returns></returns>
        public DataSet GetDataFootballProgram(string contestGroupId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballprogram"
                    , new SqlParameter[] { new SqlParameter("@contestgroupid", contestGroupId) });

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataSet GetDataFootballLeagueByCountry(string countryId,string contestGroupId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getleaguefootballbycountry"
                    , new SqlParameter[] { new SqlParameter("@COUNTRY_ID", countryId==null?"":countryId)
                    ,new SqlParameter("@contestgroupid",contestGroupId ==null ?"":contestGroupId)});

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataSet GetDataFootballCountryAll()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballcountry"
                    ,new SqlParameter[]{new SqlParameter("@countryId","")});

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataSet GetDataFootballStaticbyMatch(string matchId,string contestGroupId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strConnFeed, CommandType.StoredProcedure, "usp_sportcc_getfootballstaticbymatch"
                    , new SqlParameter[] { new SqlParameter("@match_id", matchId)
                    ,new SqlParameter("@contestGroupId",contestGroupId)});

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
