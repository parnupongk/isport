using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using isport_foxhun.Models;
namespace isport_foxhun.Controllers
{
    [LoginExpireAttribute]
    public class MatchController : Controller
    {
        // GET: Match
        public ActionResult Index(string id)
        {
            ViewBag.scout_id = id;
            MatchViewModels models = new MatchViewModels();
            models.teamList = new TeamModels().GetTeamAll();
            var list = new SelectList(new[]
            {
                new { ID = "blue", Name = "blue" },
                new { ID = "white", Name = "white" },
                new { ID = "red", Name = "red" },
            }, "ID", "Name");

            ViewData["listcolor"] = list;
            return View(models);
        }

        [HttpPost]
        public ActionResult Match()
        {

            string matchId = Guid.NewGuid().ToString();
            string[] team = Request.Form["teamList"] != null ? Request.Form["teamList"].Split(',') :null;
            if (team.Length > 0)
            {
                int rtn = MatchModels.insertMatch(matchId, Request.Form["matchdate"], (team.Length > 0 ? team[0] : ""), (team.Length > 1 ? team[1] : ""), Request.Form["field"],Request.Form["txtRound"]);
                for (int i = 1; i < 3; i++)
                {
                    string[] playerId = Request.Form["infoPlayer_team" + i.ToString()] != null ? Request.Form["infoPlayer_team" + i.ToString()].Split(',') : null;
                    if (rtn > 0 && playerId != null && playerId.Length > 0)
                    {
                        foreach (string p in playerId)
                        {
                            // insert scout
                            MatchModels.insertScout(matchId, commom.AppUtils.Session.User.id, p, team[i-1],( i==1? Request.Form["listcolor1"] : Request.Form["listcolor2"]));
                        }

                    }
                }
                
            }
            else
            {
                ModelState.AddModelError("", "Invalid Team");
            }

            return RedirectToAction("index", "Match", new { id = matchId });
        }

        public ActionResult match_list(string scout_id)
        {
            MatchViewListModels model = new MatchViewListModels();
            model.list = new MatchModels().GetMatchAll();
            return PartialView(model);
        }
        public ActionResult viewplayerbyteam(string team_id,string team,string color)
        {
            ViewBag.team = team;
            ViewBag.team_id = team_id;
            ViewBag.color = color;
            PlayertoScoutedViewModelList model = new PlayertoScoutedViewModelList();
            model.playerList = new PlayerViewModels().GetPlayertoScoutedByTeamId(team_id);
            return PartialView(model);
        }
    }
}