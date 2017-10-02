using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace isport_foxhun.Models
{
    public class MatchViewListModels
    {
        public List<MatchViewModels> list { get; set; }
    }
    public class MatchViewModels
    {
        public List<foxhun_team> teamList { get; set; }

        [Display(Name ="Date of Birth")]
        [DataType(DataType.Date)]
        public string matchdate { get; set; }
        public string team1 { get; set; }
        public string team2 { get; set; }
        public string ht { get; set; }
        public string ft { get; set; }
        public string field { get; set; }
        public string id { get; set; }
        public string team_code1 { get; set; }
        public string team_code2 { get; set; }
    }
    public class MatchModels : AppCodeModel
    {
        public List<MatchViewModels> GetMatchById(string matchId)
        {
            try
            {
                List<MatchViewModels> model = new List<MatchViewModels>();
                using (var db = this.GetDBContext())
                {
                    var match = from m in db.foxhun_match
                                join t1 in db.foxhun_team on m.team1 equals t1.id
                                join t2 in db.foxhun_team on m.team2 equals t2.id
                                where( m.id == matchId )
                                select new MatchViewModels
                                {
                                    team1 = t1.name,
                                    team2 = t2.name,
                                    matchdate = m.matchdate,//DateTime.ParseExact(m.matchdate, "mm/dd/yyyy h:mm tt", System.Globalization.CultureInfo.InvariantCulture),
                                    field = m.field,
                                    id = m.id,
                                    team_code1 = m.team1,
                                    team_code2 = m.team2
                                };
                    return match.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<MatchViewModels> GetMatchAll()
        {
            try
            {
                List<MatchViewModels> model = new List<MatchViewModels>();
                using (var db = this.GetDBContext())
                {
                    var match = from m in db.foxhun_match
                                join t1 in db.foxhun_team on m.team1 equals t1.id
                                join t2 in db.foxhun_team on m.team2 equals t2.id
                                orderby m.matchdate descending
                                select  new MatchViewModels
                                {
                                    team1 = t1.name,
                                    team2 = t2.name,
                                    matchdate = m.matchdate,//DateTime.ParseExact(m.matchdate, "mm/dd/yyyy h:mm tt", System.Globalization.CultureInfo.InvariantCulture),
                                    field = m.field,
                                    id = m.id
                                };
                    return match.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static int insertScout(string matchId,string userId,string playerId,string teamId,string color)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(strConnDB, CommandType.StoredProcedure, "usp_foxhun_scoutinsert"
                    , new SqlParameter[] {new SqlParameter("@id",Guid.NewGuid())
                                    ,new SqlParameter("@createdate",DateTime.Now)
                                    ,new SqlParameter("@match_id",matchId)
                                    ,new SqlParameter("@user_id",userId)
                                    ,new SqlParameter("@player_id",playerId)
                                    ,new SqlParameter("@team_id",teamId)
                                    ,new SqlParameter("@color",color)
                                        });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static int insertMatch(string matchId,string txtMatcDate,string team1,string team2,string txtField,string txtRound)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(strConnDB, CommandType.StoredProcedure, "usp_foxhun_matchinsert"
                    , new SqlParameter[] {new SqlParameter("@id",matchId)
                                        ,new SqlParameter("@createdate",DateTime.Now)
                                        ,new SqlParameter("@updatedate",DateTime.Now)
                                        ,new SqlParameter("@matchdate",txtMatcDate)
                                        ,new SqlParameter("@team1",team1)
                                        ,new SqlParameter("@team2",team2)
                                        ,new SqlParameter("@ht","")
                                        ,new SqlParameter("@ft","")
                                        ,new SqlParameter("@field",txtField)
                                        ,new SqlParameter("@round",txtRound)
                                        });
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}