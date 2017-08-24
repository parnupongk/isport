using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using isport_foxhun.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
namespace isport_foxhun.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string txtSearch)
        {
            PlayerViewModelList pList = new Models.PlayerViewModelList();
            pList.playerList = new AppCodeModel().GetPlayerAll(txtSearch);
            return View(pList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Terms()
        {

            return PartialView();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Region()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

       

        public ActionResult PlayerDetail(PlayerViewModelList model)
        {
            //PlayerViewModelList pList = new Models.PlayerViewModelList();
            //pList.playerList = new AppCodeModel().GetPlayerAll(txtSearch);
            return PartialView(model);
        }

        public ActionResult Team(string region_id)
        {
            ViewBag.Message = "Your contact page.";
            TeamViewModel model = new TeamViewModel();
            //model.region = region_id;
            //model.region_name = AppCodeModel.GetRegionsNameById(region_id);
            return View(model);
        }
        public Models.TeamViewModel uploadFiles(string teamName)
        {
            
            TeamViewModel model = new TeamViewModel();
            if (Session["KINGPOWER"] != null)
            {
                model = (TeamViewModel)Session["KINGPOWER"];
            }
            else
            {
                //model.pathImages = "";
                //model.pathExcel = "";
                //model.pathDoc = "";
                teamName = teamName.Replace(" ", "");
                model.pathDirectory = "~/Uploadfiles/" + teamName + "/";
            }
            string fileName = "", fileExtension = "", savefilepath="";
            foreach (string s in Request.Files)
            {
                try
                {
                    var file = Request.Files[s];
                    fileName = file.FileName;
                    fileExtension = file.ContentType;
                    Session["KINGPOWERExtension"] += fileExtension + "|";
                    //ViewBag.Images = (ViewBag.Images == null) ? fileName + "|" : ViewBag.Images + fileName + "|";

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (!System.IO.Directory.Exists(Server.MapPath(model.pathDirectory)))
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath(model.pathDirectory));
                        }

                        //model.pathDirectory = "~/Uploadfiles/" + teamName;
                        //newfilename = string.Format("{0}", fileName);
                        savefilepath = Server.MapPath(model.pathDirectory) + fileName;
                        file.SaveAs(savefilepath);

                        if( fileExtension.IndexOf("png") > -1 || fileExtension.IndexOf("jpeg") >-1 || fileExtension.IndexOf("gif") >-1 || fileExtension.IndexOf("jpg") > -1)
                        {
                            
                            model.pathImages += fileName + "|";
                        }
                        else
                        {
                            model.pathDoc += fileName + "|";
                        }

                        /*
                        switch (index)
                        {
                            case 1: model.team.file1 = "~/Uploadfiles/" + newfilename; break;
                            case 2: model.team.file2 = "~/Uploadfiles/" + newfilename; break;
                            case 3: model.team.file3 = "~/Uploadfiles/" + newfilename; break;
                            case 4: model.team.file4 = "~/Uploadfiles/" + newfilename; break;
                            case 5: model.team.file5 = "~/Uploadfiles/" + newfilename; break;
                            case 6: model.team.file6 = "~/Uploadfiles/" + newfilename; break;
                            case 7: model.team.file7 = "~/Uploadfiles/" + newfilename; break;
                            case 8: model.team.file8 = "~/Uploadfiles/" + newfilename; break;
                            case 9: model.team.file9 = "~/Uploadfiles/" + newfilename; break;
                            case 10: model.team.file10 = "~/Uploadfiles/" + newfilename; break;
                        }*/


                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            Session["KINGPOWER"] = model;
            return model;

        }



        // POST: /Manage/AddRegion
        [HttpPost]
        public ActionResult AddTeam(TeamViewModel model, HttpPostedFileBase image)
        {
            string img = Session["KINGPOWERExtension"].ToString();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            TeamViewModel modelFileUpload = null;
            string path;

            if (Session["KINGPOWER"] != null) modelFileUpload = (TeamViewModel)Session["KINGPOWER"];

            #region upload image
            HttpPostedFileBase file = image;
            if (file != null && file.ContentLength > 0)
            {
                //if (!System.IO.Directory.Exists(modelFileUpload.pathDirectory))
                //{
                    //System.IO.Directory.CreateDirectory(modelFileUpload.pathDirectory);
               //}
                string pic = System.IO.Path.GetFileName(file.FileName);
                path = Server.MapPath(modelFileUpload.pathDirectory) + pic;

                file.SaveAs(path);
                // Update data model
                model.team.image = modelFileUpload.pathDirectory + pic;
            }

            #endregion
            

            #region insert player

            string teamId = Guid.NewGuid().ToString();
            //Server.MapPath
            //path = Server.MapPath( modelFileUpload.pathDirectory + modelFileUpload.pathDoc.Replace("|",""));
            int rtn = AppCodeModel.InsertPlayerFromExcel(Server.MapPath(modelFileUpload.pathDirectory), modelFileUpload.pathDirectory,modelFileUpload.pathDoc.Replace("|", ""), model.team.name, teamId, modelFileUpload.pathImages,model.team.region);

            if (rtn == 0) ViewBag.StatusMessage = "error";
            else
            {
                #region insert team
                SqlHelper.ExecuteNonQuery(AppCodeModel.strConnDB
                , CommandType.StoredProcedure, "usp_foxhun_teaminsert"
                , new SqlParameter[] {new SqlParameter("@id",teamId)
                ,new SqlParameter("@region",model.team.region)
                ,new SqlParameter("@seq",model.team.seq)
                ,new SqlParameter("@create_datetime",DateTime.Now)
                ,new SqlParameter("@update_date",DateTime.Now)
                ,new SqlParameter("@name",model.team.name)
                ,new SqlParameter("@detail",model.team.name)
                ,new SqlParameter("@email",model.team.email)
                ,new SqlParameter("@image",model.team.image)
                ,new SqlParameter("@username",model.team.username)
                ,new SqlParameter("@password",model.team.password)
                ,new SqlParameter("@contact",model.team.contact)
                ,new SqlParameter("@phone",model.team.phone)
                ,new SqlParameter("@contact1",model.team.contact1)
                ,new SqlParameter("@phone1",model.team.phone1)
                ,new SqlParameter("@contact2",model.team.contact2)
                ,new SqlParameter("@phone2",model.team.phone2)
                ,new SqlParameter("@file1",modelFileUpload.pathDirectory)
                ,new SqlParameter("@file2",modelFileUpload.pathDoc)
                ,new SqlParameter("@file3",modelFileUpload.pathImages)
                ,new SqlParameter("@file4","")
                ,new SqlParameter("@file5","")
                ,new SqlParameter("@file6","")
                ,new SqlParameter("@file7","")
                ,new SqlParameter("@file8","")
                ,new SqlParameter("@file9","")
                ,new SqlParameter("@file10","")


                });
                #endregion
            }

            #endregion

            return RedirectToAction("Team", "Home", new {  });//View("Region",model);//View(model); //RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        [HttpGet]
        public ActionResult GetTeamByRegionID(string region_id)
        {
            TeamViewModelList teamList = new TeamViewModelList();
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(AppCodeModel.strConnDB
                , CommandType.StoredProcedure, "usp_foxhun_teamselectall", new SqlParameter[] { new SqlParameter("@region_id", region_id) });             
            teamList.teamList = (from DataRow dr in ds.Tables[0].Rows
                                     select new foxhun_team()
                                     {
                                         email = dr["email"].ToString(),
                                         id = dr["id"].ToString(),
                                         region = dr["region"].ToString(),
                                         seq = int.Parse(dr["seq"].ToString()),
                                         name = dr["name"].ToString(),
                                         detail = dr["detail"].ToString(),
                                         create_datetime = DateTime.Parse(dr["create_datetime"].ToString())
                                     }).ToList();
            //var list = ds.Tables[0].
            /*var accName = (from N in list
                           where N.casLevel6.Contains(Prefix) && (N.casIDLevel2 == int.Parse(casIDLevel3))
                           select new { N.casLevel6, N.casID });*/
            return PartialView("TeamDetail", teamList);//Json(ds.Tables[0], JsonRequestBehavior.AllowGet);
        }


        // POST: /Manage/AddRegion
        [HttpPost]
        public ActionResult AddRegion(RegionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            SqlHelper.ExecuteNonQuery(AppCodeModel.strConnDB
                , CommandType.StoredProcedure, "usp_foxhun_regioninsert"
                , new SqlParameter[] {new SqlParameter("@id",Guid.NewGuid().ToString())
                ,new SqlParameter("@seq",model.seq)
                ,new SqlParameter("@datetime",DateTime.Now)
                ,new SqlParameter("@name",model.name)
                ,new SqlParameter("@detail",model.detail)});
            return RedirectToAction("Region", "Home");//View("Region",model);//View(model); //RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        [HttpGet]
        public ActionResult GetAllRegion()
        {
            RegionViewModelList regionList = new RegionViewModelList();
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(AppCodeModel.strConnDB
                , CommandType.StoredProcedure, "usp_foxhun_regionselectall");
            regionList.regionList = (from DataRow dr in ds.Tables[0].Rows
                                     select new RegionViewModel()
                                     {
                                         id = dr["id"].ToString(),
                                         seq = int.Parse(dr["seq"].ToString()),
                                         name = dr["name"].ToString(),
                                         detail = dr["detail"].ToString(),
                                         datetime = DateTime.Parse(dr["datetime"].ToString())
                                     }).ToList();
            //var list = ds.Tables[0].
            /*var accName = (from N in list
                           where N.casLevel6.Contains(Prefix) && (N.casIDLevel2 == int.Parse(casIDLevel3))
                           select new { N.casLevel6, N.casID });*/
            return PartialView("RegionDetail",regionList);//Json(ds.Tables[0], JsonRequestBehavior.AllowGet);
        }
    }
}