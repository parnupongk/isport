using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using isport_foxhun.Models;
using WebLibrary;
using System.IO;
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

        [HttpPost]
        public ActionResult Update(TeamViewModel model, HttpPostedFileBase fileLogo, HttpPostedFileBase fileDocument)
        {
            string str = Request.Form.AllKeys.ToString();
            HttpPostedFileBase file = null;
            string path = "~/Uploadfiles/" + model.name.Replace(" ", "") + "/";
            for (int i = 1; i < 3; i++)
            {
                switch (i)
                {
                    case 1: file = fileLogo; break;
                    case 2: file = fileDocument; break;
                }
                if (file != null && file.ContentLength > 0)
                {
                    string pic = Path.GetFileName(file.FileName);

                    try
                    {
                        if (!Directory.Exists(Server.MapPath(path)))
                        {
                            Directory.CreateDirectory(Server.MapPath(path));
                        }
                    }
                    catch { }
                    file.SaveAs(Server.MapPath(path)+pic);
                    // Update data model
                    switch (i)
                    {
                        case 1: model.image = path + pic; ; break;
                        case 2: model.file2 = path + pic; break;
                    }

                }
            }

            new TeamModels().TeamUpdate(model);

            return RedirectToAction("index", "team", new { id = model.id });//View("index", "team", new { id= model.id} );
        }

        public ActionResult TeamList(string txtSearch)
        {
            TeamViewModelList model = new TeamViewModelList();
            try
            {
                model.teamList = txtSearch != null && txtSearch != "" ?  new TeamModels().GetTeamBySeach(txtSearch) : new TeamModels().GetTeamAll();
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }
            return View(model);
        }

        public ActionResult TeamDetail(TeamViewModelList model)
        {
            return PartialView(model);
        }

        public ActionResult GetPlayerByTeamId(TeamViewModels model)
        {
            PlayerViewModelList player = new PlayerViewModelList();
            player.playerList = model.modelPlayer;
            return PartialView("~/Views/Home/PlayerDetail.cshtml", player);
        }

        /*
        public ActionResult TeamDetail()
        {
            TeamViewModelList models = new TeamViewModelList();
            models.teamList = new TeamModels().GetTeamAll();
            return PartialView(models);
        }*/

        
    }
}
