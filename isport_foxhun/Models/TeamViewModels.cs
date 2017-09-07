using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace isport_foxhun.Models
{
    public class TeamViewModels
    {
        public List<foxhun_team> modelTeam { get; set; }
        public List<foxhun_player> modelPlayer { get; set; }
    }

    public class TeamModels : AppCodeModel
    {
        public List<foxhun_team> GetTeamAll()
        {
            try
            {
                using (var db = this.GetDBContext())
                {
                    var team = db.foxhun_team;
                    return team.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<foxhun_team> GetTeamById(string id)
        {
            try
            {
                using (var db = this.GetDBContext())
                {
                    var team = db.foxhun_team
                        .Where(r => r.id == id);
                    return team.ToList();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}