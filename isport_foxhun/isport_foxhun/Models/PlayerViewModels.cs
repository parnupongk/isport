using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using isport_foxhun.Models;
namespace isport_foxhun.Models
{
    public class PlayerViewModels : AppCodeModel
    {
        public bool GetIsPlayertoScouted(string pId)
        {
            try
            {
                using (var db = this.GetDBContext())
                {
                    var list = db.foxhun_scout
                        .Where(x => x.player_id == pId);

                    return list.ToList().Count > 0 ? true : false;
                }
            }
            catch
            {
                return false;
            }
        }
        public List<foxhun_player> GetPlayerByTeamId(string teamId)
        {
            try
            {
                using (var db = this.GetDBContext())
                {
                    var player = db.foxhun_player
                                 .Where(r => r.team_id == teamId)
                                 .OrderBy(r => r.seq);
                                  
                    return player.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<PlayerVModel> GetPlayertoScoutedByTeamId(string teamId)
        {
            try
            {
                using (var db = this.GetDBContext())
                {
                    var player = (from s in db.foxhun_player
                                  where (s.team_id == teamId) // (r => r.team_id == teamId)
                                  orderby (s.seq)
                                  select new PlayerVModel
                                  {
                                      id = s.id,
                                      region = s.region,
                                      team = s.team,
                                      datetime = s.datetime,
                                      index = s.index,
                                      seq = s.seq,
                                      position = s.position,
                                      name = s.name,
                                      control = s.control,
                                      attack = s.attack,
                                      tacktick = s.tacktick,
                                      defense = s.defense,
                                      physical = s.physical,
                                      mental = s.mental,
                                      adversary = s.adversary,
                                      contact = s.contact,
                                      HT = s.HT,
                                      FT = s.FT,
                                      competition = s.competition,
                                      score = s.score,
                                      see = s.see,
                                      detail = s.detail,
                                      wight = s.wight,
                                      hight = s.hight,
                                      age = s.age,
                                      country = s.country,
                                      image = s.image,
                                      sum = s.sum,
                                      team_id = s.team_id,
                                      number = s.number,
                                      birthday = s.birthday,
                                      nameen = s.nameen,
                                      size = s.size,
                                      sizepants = s.sizepants

                                  }).AsEnumerable()
                                 .Select(x => new PlayerVModel
                                 {
                                     id = x.id,
                                     region = x.region,
                                     team = x.team,
                                     datetime = x.datetime,
                                     index = x.index,
                                     seq = x.seq,
                                     position = x.position,
                                     name = x.name,
                                     control = x.control,
                                     attack = x.attack,
                                     tacktick = x.tacktick,
                                     defense = x.defense,
                                     physical = x.physical,
                                     mental = x.mental,
                                     adversary = x.adversary,
                                     contact = x.contact,
                                     HT = x.HT,
                                     FT = x.FT,
                                     competition = x.competition,
                                     score = x.score,
                                     see = x.see,
                                     detail = x.detail,
                                     wight = x.wight,
                                     hight = x.hight,
                                     age = x.age,
                                     country = x.country,
                                     image = x.image,
                                     sum = x.sum,
                                     team_id = x.team_id,
                                     number = x.number,
                                     birthday = x.birthday,
                                     nameen = x.nameen,
                                     size = x.size,
                                     sizepants = x.sizepants,
                                     isScouted = GetIsPlayertoScouted(x.id)
                                 });
                    return player.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}