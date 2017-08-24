using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.OleDb;
namespace isport_foxhun.Models
{
    public class AppCodeModel
    {
        public static string strConnDB = System.Configuration.ConfigurationManager.ConnectionStrings["IsportWSConnectionString"].ToString();

        public isportEntities GetDBContext()
        {
            var ctx = new isportEntities();
            ctx.Configuration.AutoDetectChangesEnabled = true;
            ctx.Configuration.ValidateOnSaveEnabled = false;
            // added for manage execute timeout in 3 minutes
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)ctx).ObjectContext.CommandTimeout = 180;
            return ctx;
        }
        #region player

        public static int InsertPlayerFromExcel(string path, string partRoot, string excelName,string teamName,string teamId,string pathImage,string region)
        {
            int rtn = 0;
            
            string[] pathImages = pathImage.Split('|');
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("index"), new DataColumn("birthday")
                , new DataColumn("number"), new DataColumn("position")
                , new DataColumn("name"), new DataColumn("detail")
                , new DataColumn("wight"), new DataColumn("hight")
                , new DataColumn("age") });
            //Response.Write(path);
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source= " + path + excelName + " ; Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(connectionString);
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();

            try
            {
                string sql = "select * from [player$A:I]";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataReader drRead = cmd.ExecuteReader();
                PlayerViewModel model = new PlayerViewModel();
                int rowCount = 0;
                while (drRead.Read())
                {
                    try
                    {

                        if (drRead[0].ToString().Trim() != "")
                        {
                            
                            model.player = new foxhun_player();
                            model.player.id = Guid.NewGuid().ToString();
                            model.player.region = region;
                            model.player.team_id = teamId;
                            model.player.team = teamName;
                            model.player.control = "0";
                            model.player.attack = "0";
                            model.player.tacktick = "0";
                            model.player.defense = "0";
                            model.player.physical = "0";
                            model.player.mental = "0";
                            model.player.birthday = drRead[1].ToString();                            
                            model.player.position = drRead[3].ToString();
                            model.player.name = drRead[4].ToString();
                            model.player.detail = drRead[5].ToString();
                            model.player.datetime = DateTime.Now;
                            try
                            {
                                model.player.number = drRead[2] != null? int.Parse(drRead[2].ToString()) : 0;
                                model.player.wight = drRead[6] !=null ? int.Parse(drRead[6].ToString()) : 0;
                                model.player.hight = drRead[7] !=null ? int.Parse(drRead[7].ToString()) : 0;
                                model.player.age = drRead[8] !=null ? int.Parse(drRead[8].ToString()) : 0;
                            }
                            catch { }
                            try
                            {
                                for (int i = 0; i < pathImages.Length; i++)
                                {
                                    if( (pathImages[rowCount] == i.ToString() + ".jpg") || (pathImages[rowCount] == i.ToString() + ".png")) model.player.image = partRoot + pathImages[rowCount];
                                }
                            }
                            catch { }
                            rtn +=InsertPlayer(model);
                            rowCount++;
                        }
                    }
                    catch(Exception ex) { Console.WriteLine(ex.Message); }

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return rtn;
        }

        public static int InsertPlayer(PlayerViewModel p)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(strConnDB, CommandType.StoredProcedure, "usp_foxhun_playerinsert"
                    , new SqlParameter[] {
                        new SqlParameter("@id",p.player.id)
                        ,new SqlParameter("@region",p.player.region)
                        ,new SqlParameter("@team",p.player.team)
                        ,new SqlParameter("@datetime",p.player.datetime)
                        ,new SqlParameter("@seq",p.player.seq)
                        ,new SqlParameter("@position",p.player.position)
                        ,new SqlParameter("@name",p.player.name)
                        ,new SqlParameter("@control",p.player.control)
                        ,new SqlParameter("@attack",p.player.attack)
                        ,new SqlParameter("@tacktick",p.player.tacktick)
                        ,new SqlParameter("@defense",p.player.defense)
                        ,new SqlParameter("@physical",p.player.physical)
                        ,new SqlParameter("@mental",p.player.mental)
                        ,new SqlParameter("@adversary",p.player.adversary)
                        ,new SqlParameter("@contact",p.player.contact)
                        ,new SqlParameter("@HT",p.player.HT)
                        ,new SqlParameter("@FT",p.player.FT)
                        ,new SqlParameter("@competition",p.player.competition)
                        ,new SqlParameter("@score",p.player.score)
                        ,new SqlParameter("@see",p.player.see)
                        ,new SqlParameter("@detail",p.player.detail)
                        ,new SqlParameter("@wight",p.player.wight)
                        ,new SqlParameter("@hight",p.player.hight)
                        ,new SqlParameter("@age",p.player.age)
                        ,new SqlParameter("@country",p.player.country)
                        ,new SqlParameter("@image",p.player.image)
                        ,new SqlParameter("@sum",p.player.sum)
                        ,new SqlParameter("@team_id",p.player.team_id)
                        ,new SqlParameter("@number",p.player.number)
                        ,new SqlParameter("@birthday",p.player.birthday)
                    });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static int UpdatePlayer(PlayerViewModel p)
        {
            try
            {

                return SqlHelper.ExecuteNonQuery(strConnDB, CommandType.StoredProcedure, "usp_foxhun_playerupdate"
                    , new SqlParameter[] {
                        new SqlParameter("@id",p.player.id)
                        ,new SqlParameter("@region",p.player.region)
                        ,new SqlParameter("@team",p.player.team)
                        ,new SqlParameter("@datetime",p.player.datetime)
                        ,new SqlParameter("@seq",p.player.seq)
                        ,new SqlParameter("@position",p.player.position)
                        ,new SqlParameter("@name",p.player.name)
                        ,new SqlParameter("@control",p.player.control)
                        ,new SqlParameter("@attack",p.player.attack)
                        ,new SqlParameter("@tacktick",p.player.tacktick)
                        ,new SqlParameter("@defense",p.player.defense)
                        ,new SqlParameter("@physical",p.player.physical)
                        ,new SqlParameter("@mental",p.player.mental)
                        ,new SqlParameter("@adversary",p.player.adversary)
                        ,new SqlParameter("@contact",p.player.contact)
                        ,new SqlParameter("@HT",p.player.HT)
                        ,new SqlParameter("@FT",p.player.FT)
                        ,new SqlParameter("@competition",p.player.competition)
                        ,new SqlParameter("@score",p.player.score)
                        ,new SqlParameter("@see",p.player.see)
                        ,new SqlParameter("@detail",p.player.detail)
                        ,new SqlParameter("@wight",p.player.wight)
                        ,new SqlParameter("@hight",p.player.hight)
                        ,new SqlParameter("@age",p.player.age)
                        ,new SqlParameter("@country",p.player.country)
                        ,new SqlParameter("@image",p.player.image)
                        ,new SqlParameter("@sum",p.player.sum)
                    });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<foxhun_team> GetTeamByRegion(string region)
        {
            try
            {
                using (var db = this.GetDBContext())
                {
                    var team = db.foxhun_team
                        .Where(r => r.region == region);
                    return team.ToList();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<foxhun_player_history> GetPlayerHistoryById(string player_id)
        {
            try
            {
                using (var db = this.GetDBContext())
                {
                    var cas = db.foxhun_player_history
                        .Where(r => r.player_id == player_id);                              
                    return cas.ToList();
                }

                //var pList = new isportEntities().foxhun_player;
                //return pList.ToList<PlayerViewModel>();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public List<foxhun_player> GetPlayerById(string playerId)
        {
            try
            {
                using (var db = this.GetDBContext())
                {
                    var cas = db.foxhun_player.Where( r => r.id == playerId);
                    return cas.ToList();
                }

                //var pList = new isportEntities().foxhun_player;
                //return pList.ToList<PlayerViewModel>();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public List<foxhun_player> GetPlayerAll()
        {
            try
            {
                using (var db = this.GetDBContext())
                {
                    var cas = db.foxhun_player;
                    return cas.ToList();
                }

                //var pList = new isportEntities().foxhun_player;
                //return pList.ToList<PlayerViewModel>();
            }
            catch(Exception ex)
            { throw new Exception(ex.Message); }
        }
        public List<foxhun_player> GetPlayerAll(string txtSearch)
        {
            try
            {
                txtSearch = txtSearch == null ? "" : txtSearch;
                using (var db = this.GetDBContext())
                {
                    var cas = from m in db.foxhun_player
                              where m.name.Contains(txtSearch) || m.position.Contains(txtSearch)
                              select m; 
                    return cas.ToList();
                }

                //var pList = new isportEntities().foxhun_player;
                //return pList.ToList<PlayerViewModel>();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public List<foxhun_province> GetProvinceAll()
        {
            try
            {
                using (var db = this.GetDBContext())
                {
                    var cas = db.foxhun_province;
                    return cas.ToList();
                }

                //var pList = new isportEntities().foxhun_player;
                //return pList.ToList<PlayerViewModel>();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        #endregion
        public static string GetRegionsNameById(string region_id)
        {
            try
            {
                string rtn = "";
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(strConnDB, CommandType.StoredProcedure, "usp_foxhun_regionselectbyid"
                    , new SqlParameter[] { new SqlParameter("@region_id", region_id)});
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) rtn = ds.Tables[0].Rows[0]["name"].ToString();

                return rtn;            
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}