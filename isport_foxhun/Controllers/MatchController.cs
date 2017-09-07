using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using isport_foxhun.Models;
namespace isport_foxhun.Controllers
{
    public class MatchController : Controller
    {
        // GET: Match
        public ActionResult Index()
        {
            MatchViewModels models = new MatchViewModels();
            models.teamList = new TeamModels().GetTeamAll();
            return View(models);
        }
    }
}