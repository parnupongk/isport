using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace isport_foxhun.Models
{
    public class PlayerViewModels : AppCodeModel
    {
        public List<foxhun_player> GetPlayerByTeamId(string teamId)
        {
            try
            {
                using (var db = this.GetDBContext())
                {
                    var player = db.foxhun_player
                        .Where(r => r.team_id == teamId);
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