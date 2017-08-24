using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using isport_foxhun.Models;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
namespace isport_foxhun.Controllers
{
    public class PlayerController : Controller
    {
        // new player
        public ActionResult Parameter(string position)
        {
            PlayerViewModel model = new PlayerViewModel();

            model = GetParameterByPosition(model,position);

            return PartialView(model);
        }

        // update player
        public ActionResult Parameter1(PlayerViewModel model)
        {

            return PartialView("parameter", model);
        }

        private PlayerViewModel GetParameterByPosition(PlayerViewModel model ,string position)
        {
            string[] strs = new string[6];

            try
            {


                #region get Config
                if (position == "CB" || position == "LB" || position == "FB" || position == "LWB" || position == "RWB" || position == "DM")
                {
                    strs[0] = "ControlNameCBTH";
                    strs[1] = "AttackNameCBTH";
                    strs[2] = "DefenseNameCBTH";
                    strs[3] = "TacktickNameCBTH";
                    strs[4] = "PhysicalNameCBTH";
                    strs[5] = "MentalNameCBTH";
                }
                else if (position == "CM" || position == "AM")
                {
                    strs[0] = "ControlNameCMTH";
                    strs[1] = "AttackNameCMTH";
                    strs[2] = "DefenseNameCMTH";
                    strs[3] = "TacktickNameCMTH";
                    strs[4] = "PhysicalNameCMTH";
                    strs[5] = "MentalNameCMTH";
                }
                else if (position == "LW" || position == "RW")
                {
                    strs[0] = "ControlNameWTH";
                    strs[1] = "AttackNameWTH";
                    strs[2] = "DefenseNameWTH";
                    strs[3] = "TacktickNameWTH";
                    strs[4] = "PhysicalNameWTH";
                    strs[5] = "MentalNameWTH";
                }
                else if (position == "CF" || position == "WF" || position == "ST")
                {
                    strs[0] = "ControlNameSTTH";
                    strs[1] = "AttackNameSTTH";
                    strs[2] = "DefenseNameSTTH";
                    strs[3] = "TacktickNameSTTH";
                    strs[4] = "PhysicalNameSTTH";
                    strs[5] = "MentalNameSTTH";
                }
                else if (position == "GK")
                {
                    strs[0] = "ControlNameGKTH";
                    strs[1] = "AttackNameGKTH";
                    strs[2] = "DefenseNameGKTH";
                    strs[3] = "TacktickNameGKTH";
                    strs[4] = "PhysicalNameGKTH";
                    strs[5] = "MentalNameGKTH";
                }
                #endregion

                ParameterViewModel pModel;
                List<ParameterViewModel> pModels;
                string[] paramValue = new string[] { };
                int index = 0;
                for (int i = 0; i < strs.Length; i++)
                {
                    pModels = new List<ParameterViewModel>();
                    if (model.player != null)
                    {
                        switch (i)
                        {
                            case 0: paramValue = model.player.control.Split(','); break;
                            case 1: paramValue = model.player.attack.Split(','); break;
                            case 2: paramValue = model.player.defense.Split(','); break;
                            case 3: paramValue = model.player.tacktick.Split(','); break;
                            case 4: paramValue = model.player.physical.Split(','); break;
                            case 5: paramValue = model.player.mental.Split(','); break;
                        }
                    }
                    index = 0;
                    foreach (string s in ConfigurationManager.AppSettings[strs[i]].Split(','))
                    {
                        pModel = new ParameterViewModel();
                        pModel.name = s;
                        try
                        {
                            pModel.value = (paramValue.Length >= index && paramValue[index] != "") ? int.Parse(paramValue[index]) : 0;
                        }
                        catch { pModel.value = 0; }
                        pModels.Add(pModel);
                        index++;
                    }
                    switch (i)
                    {
                        case 0: model.Control = pModels; break;
                        case 1: model.Attack = pModels; break;
                        case 2: model.Defense = pModels; break;
                        case 3: model.Tacktick = pModels; break;
                        case 4: model.Physical = pModels; break;
                        case 5: model.Mental = pModels; break;
                    }

                }
            }
            catch
            { }
            return model;
        }

        public ActionResult History(string player_id)
        {
            PlayerHistoryViewModel pModel = new PlayerHistoryViewModel();
            pModel.playerHistory = new AppCodeModel().GetPlayerHistoryById(player_id);

            return PartialView(pModel);
        }
        // GET: Player
        public ActionResult Index(string id)
        {
            PlayerViewModel model = new PlayerViewModel();
            foxhun_player player = new foxhun_player();
            model.player = player;
            if (id != null && id != "")
            {

                List<foxhun_player> pList = new AppCodeModel().GetPlayerById(id);

                model.player = pList[0];

                model = GetParameterByPosition(model, model.player.position);

            }

            

            return View(model);

        }

        [HttpPost]
        public JsonResult Province(string Prefix)
        {
            //Note : you can bind same list from database  
            List<foxhun_province> ObjList = new AppCodeModel().GetProvinceAll();

            //Searching records from list using LINQ query  
            var CityName = (from N in ObjList
                            where N.pvnName.StartsWith(Prefix)
                            select new { N.pvnName });
            return Json(CityName, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddPlayer(PlayerViewModel model, HttpPostedFileBase image)
        {
            HttpPostedFileBase file = image;
            if( file != null && file.ContentLength > 0 )
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = Server.MapPath("~/images/")+ pic;

                file.SaveAs(path);
                // Update data model
                model.player.image= "~/images/"+pic;
            }

            string[] txtParam = new string[] {"ctr_","att_","def_","tac_","phy_","men_" };
            model.player.team_id = (Request["team"] != null && Request["team"] != "") ?Request["team"]:"";
            model.player.sum = 0;
            for (int i =0;i<txtParam.Length;i++)
            {
                for(int index=0;index<10;index++)
                {
                    if( Request.Form[txtParam[i]+index.ToString()] != null && Request.Form[txtParam[i] + index.ToString()] != "")
                    {
                        model.player.sum += int.Parse(Request.Form[txtParam[i] + index.ToString()]);
                        switch (i)
                        {
                            case 0: model.player.control += Request.Form[txtParam[i] + index.ToString()] +","; break;
                            case 1: model.player.attack += Request.Form[txtParam[i] + index.ToString()] + ","; break;
                            case 2: model.player.defense += Request.Form[txtParam[i] + index.ToString()] + ","; break;
                            case 3: model.player.tacktick += Request.Form[txtParam[i] + index.ToString()] + ","; break;
                            case 4: model.player.physical += Request.Form[txtParam[i] + index.ToString()] + ","; break;
                            case 5: model.player.mental += Request.Form[txtParam[i] + index.ToString()] + ","; break;
                        }
                    }
                }
            }

            model.player.datetime = DateTime.Now;
            if (model.player.id == null || model.player.id == "")
            {
                model.player.id = Guid.NewGuid().ToString();
                AppCodeModel.InsertPlayer(model);
            }
            else
            {
                model.player.id = model.player.id;
                AppCodeModel.UpdatePlayer(model);
            }
            
                                        
            return RedirectToAction("Index", "Player");
        }
    }
}