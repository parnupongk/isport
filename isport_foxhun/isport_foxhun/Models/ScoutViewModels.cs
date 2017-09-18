using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using isport_foxhun.commom;
using WebLibrary;
namespace isport_foxhun.Models
{
    public class ScoutViewListModels
    {
        public List<ScoutViewModels> list { get; set; }
    }
    public class ScoutViewModels
    {
        public string id { get; set; }
        public Nullable<System.DateTime> createdate { get; set; }
        public string match_id { get; set; }
        public string match { get; set; }
        public string user_id { get; set; }
        public string player_id { get; set; }
        public string player { get; set; }
        public string player_position { get; set; }
        public string player_image { get; set; }
        public string team_id { get; set; }
        public string team { get; set; }
    }
    public class ScoutModels : AppCodeModel
    {

        public static string GetMatchName(string matchId)
        {
            string rtn = "";
            try
            {
                List<MatchViewModels> model = new MatchModels().GetMatchById(matchId);

                if (model.Count > 0)
                {
                    rtn = model[0].team1 + " vs " + model[0].team2 + " วันที่ " + model[0].matchdate;
                }
            }
            catch(Exception ex) { ExceptionManager.WriteError(ex.Message); }
            return rtn;
        }
        public List<ScoutViewModels> GetScoutByMatchId(string matchId)
        {
            try
            {
                using (var db = this.GetDBContext())
                {
                    var scout = (from s in db.foxhun_scout
                                 from s1 in db.foxhun_player.Where(p => p.id == s.player_id)
                                 from t in db.foxhun_team.Where(p => p.id == s.team_id)
                                 where (s.match_id == matchId) // && s.user_id == AppUtils.Session.User.id
                                 select new ScoutViewModels
                                 {
                                     id = s.id,
                                     match_id = s.match_id,
                                     match = "",//GetMatchName(s.match_id).ToString(),
                                     team = t.name,
                                     player = s1.name,
                                     player_id = s1.id,
                                     player_position = s1.position,
                                     player_image = s1.image
                                 })
                                .AsEnumerable()
                                .Select(x => new ScoutViewModels {
                                    id = x.id,
                                    match_id = x.match_id,
                                    match = GetMatchName(x.match_id).ToString(),
                                    team = x.team,
                                    player = x.player,
                                    player_id = x.player_id,
                                    player_position = x.player_position,
                                    player_image = x.player_image
                                });
                    return scout.ToList();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}