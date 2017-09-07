using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using isport_foxhun.Models;
namespace isport_foxhun.Controllers
{
    [LoginExpireAttribute]
    public class TeamController : Controller
    {
        // GET: Team
        public ActionResult Index(string id)
        {
            TeamViewModels models = new TeamViewModels();
            models.modelTeam = new TeamModels().GetTeamById(id);
            if (models.modelTeam.Count > 0)
            {
                models.modelPlayer = new PlayerViewModels().GetPlayerByTeamId(models.modelTeam[0].id);
            }
            return View(models);
        }

        public ActionResult GetPlayerByTeamId(TeamViewModels model)
        {
            PlayerViewModelList player = new PlayerViewModelList();
            player.playerList = model.modelPlayer;
            return PartialView("~/Views/Home/PlayerDetail.cshtml", player);
        }

        public ActionResult TeamDetail()
        {
            TeamViewModelList models = new TeamViewModelList();
            models.teamList = new TeamModels().GetTeamAll();
            return PartialView(models);
        }

        
    }
}
