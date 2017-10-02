using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using isport_foxhun.Models;
namespace isport_foxhun.Controllers
{
    [LoginExpireAttribute]
    public class ScoutController : Controller
    {
        // GET: Scout
        public ActionResult Index(string match_id)
        {
            ViewBag.match_id = match_id;
            ScoutViewListModels model = new ScoutViewListModels();
            model.list = new ScoutModels().GetScoutByMatchId(match_id);
            List<MatchViewModels> mModel = new MatchModels().GetMatchById(match_id);
            if( mModel.Count > 0 )
            {
                ViewBag.TeamCode1 = mModel[0].team_code1;
                ViewBag.TeamCode2 = mModel[0].team_code2;
            }
            return View(model);
        }

        public ActionResult PlayerDetail(PlayerViewModelList model)
        {
            //PlayerViewModelList pList = new Models.PlayerViewModelList();
            //pList.playerList = new AppCodeModel().GetPlayerAll(txtSearch);
            return PartialView(model);
        }

        public ActionResult player(string player_id)
        {
            PlayerViewModelList model = new PlayerViewModelList();
            model.playerList = new AppCodeModel().GetPlayerById(player_id);
            return PartialView(model);
        }

        public ActionResult playertoscout(PlayerViewModelList model)
        {
            return PartialView(model);
        }
        public ActionResult ListPlayer(string txtSearch)
        {
            PlayerViewModelList pList = new Models.PlayerViewModelList();
            pList.playerList = new AppCodeModel().GetPlayerAll(txtSearch);
            pList.listScout = new ScoutModels().GetScoutByScoutId(commom.AppUtils.Session.User.id);
            return View(pList);
        }

        [HttpPost]
        public ActionResult insertplayer()
        {
            string matchId = Request.Form["item.match_id"];
            for (int i = 1; i < 3; i++)
            {
                string[] playerId = Request.Form["infoPlayer_team" + i.ToString()] != null ? Request.Form["infoPlayer_team" + i.ToString()].Split(',') : null;
                if ( playerId != null && playerId.Length > 0)
                {
                    foreach (string p in playerId)
                    {
                        // insert scout
                        MatchModels.insertScout(matchId, commom.AppUtils.Session.User.id, p, Request.Form["team"+i.ToString()], "blue");
                    }

                }
            }
            return RedirectToAction("index", "scout", new { match_id = matchId });
        }
    }
}