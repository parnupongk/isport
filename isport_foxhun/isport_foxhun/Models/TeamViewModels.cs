using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace isport_foxhun.Models
{
    public class TeamViewModels
    {
        public List<foxhun_team> modelTeam { get; set; }
        public List<foxhun_player> modelPlayer { get; set; }
    }

    public class TeamModels : AppCodeModel
    {
        public int TeamUpdate(TeamViewModel model)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(strConnDB, CommandType.StoredProcedure, "usp_foxhun_teamupdate",
                    new SqlParameter[] {new SqlParameter("@id",model.id)
                                        ,new SqlParameter("@region",model.region)
                                        ,new SqlParameter("@seq",model.seq)
                                        ,new SqlParameter("@update_date",DateTime.Now)
                                        ,new SqlParameter("@name",model.name)
                                        ,new SqlParameter("@detail",model.detail)
                                        ,new SqlParameter("@image",model.image)
                                        ,new SqlParameter("@username",model.username)
                                        ,new SqlParameter("@contact",model.contact)
                                        ,new SqlParameter("@contact1",model.contact1)
                                        ,new SqlParameter("@contact2",model.contact2)
                                        ,new SqlParameter("@phone",model.phone)
                                        ,new SqlParameter("@phone1",model.phone1)
                                        ,new SqlParameter("@contact3",model.contact3)

                                        ,new SqlParameter("@file2",model.file2)
                                        ,new SqlParameter("@file3",model.file3)
                                        ,new SqlParameter("@file4",model.file4)

                                        ,new SqlParameter("@file5",model.file5)
                                        ,new SqlParameter("@file6",model.file6)
                                        ,new SqlParameter("@file7",model.file7)
                                        ,new SqlParameter("@file8",model.file8)
                                        ,new SqlParameter("@file9",model.file9)
                                        ,new SqlParameter("@file10",model.file10)

                                        ,new SqlParameter("@contact4",model.contact4)
                                        ,new SqlParameter("@contact5",model.contact5)
                                        ,new SqlParameter("@contact6",model.contact6)
                                        ,new SqlParameter("@phone2",model.phone2)
                                        ,new SqlParameter("@phone3",model.phone3)
                                        ,new SqlParameter("@phone4",model.phone4)
                                        ,new SqlParameter("@phone5",model.phone5)
                                        ,new SqlParameter("@phone6",model.phone6)
                                        });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<foxhun_team> GetTeamBySeach(string txtSearch)
        {
            try
            {
                using (var db = this.GetDBContext())
                {
                    var team = from m in db.foxhun_team
                               where m.name.Contains(txtSearch) || m.region.Contains(txtSearch)
                               select m;
                    return team.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
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