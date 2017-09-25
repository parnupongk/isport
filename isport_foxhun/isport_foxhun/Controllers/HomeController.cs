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
using isport_foxhun.commom;
using System.Configuration;
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

        /// <summary>
        /// Register Team
        /// </summary>
        /// <param name="region_id"></param>
        /// <returns></returns>
        public ActionResult Team()
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
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (!Directory.Exists(Server.MapPath(model.pathDirectory)))
                        {
                           Directory.CreateDirectory(Server.MapPath(model.pathDirectory));
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
        public ActionResult Team(TeamViewModel model, HttpPostedFileBase image, HttpPostedFileBase fileDoc, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3, HttpPostedFileBase image4, HttpPostedFileBase image5, HttpPostedFileBase image6)
        {
            try
            {


                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                string err = CheckUserDup(model.email, model.password);
                if (err != "")
                {
                    ModelState.AddModelError("", err);
                    return View();
                }
                else
                {
                    TeamViewModel modelFileUpload = null;
                    string path;

                    if (Session["KINGPOWER"] != null) modelFileUpload = (TeamViewModel)Session["KINGPOWER"];
                    else
                    {
                        modelFileUpload = new TeamViewModel();
                        modelFileUpload.pathDirectory = "~/Uploadfiles/" + model.name.Replace(" ", "") + "/";
                    }

                    #region upload image
                    HttpPostedFileBase file = null;

                    for (int i = 1; i < 9; i++)
                    {
                        switch (i)
                        {
                            case 1: file = image; break;
                            case 2: file = fileDoc; break;
                            case 3: file = image1; break;
                            case 4: file = image2; break;
                            case 5: file = image3; break;
                            case 6: file = image4; break;
                            case 7: file = image5; break;
                            case 8: file = image6; break;
                        }
                        if (file != null && file.ContentLength > 0)
                        {
                            string pic = Path.GetFileName(file.FileName);
                            path = Server.MapPath(modelFileUpload.pathDirectory) + pic;

                            try
                            {
                                if (!Directory.Exists(Server.MapPath(modelFileUpload.pathDirectory)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(modelFileUpload.pathDirectory));
                                }
                            }
                            catch { }
                            file.SaveAs(path);
                            // Update data model
                            switch (i)
                            {
                                case 1: model.image = modelFileUpload.pathDirectory + pic; ; break;
                                case 2: model.file2 = modelFileUpload.pathDirectory + pic; break;
                                case 3: model.file3 = modelFileUpload.pathDirectory + pic; break;
                                case 4: model.file4 = modelFileUpload.pathDirectory + pic; break;
                                case 5: model.file5 = modelFileUpload.pathDirectory + pic; break;
                                case 6: model.file6 = modelFileUpload.pathDirectory + pic; break;
                                case 7: model.file7 = modelFileUpload.pathDirectory + pic; break;
                                case 8: model.file8 = modelFileUpload.pathDirectory + pic; break;
                            }

                        }
                    }
                    /*
                    file = fileDoc;
                    if ( file != null && file.ContentLength > 0 )
                    {
                        string pic = System.IO.Path.GetFileName(file.FileName);
                        path = Server.MapPath(modelFileUpload.pathDirectory) + pic;         
                        file.SaveAs(path);                
                    }*/
                    #endregion


                    #region insert player

                    string teamId = Guid.NewGuid().ToString();
                    int rtn = 0;
                    if (modelFileUpload.pathDoc != null && modelFileUpload.pathDoc != "")
                        rtn = AppCodeModel.InsertPlayerFromExcel(Server.MapPath(modelFileUpload.pathDirectory), modelFileUpload.pathDirectory, modelFileUpload.pathDoc.Replace("|", ""), model.name, teamId, modelFileUpload.pathImages, model.region);
                    else rtn = 1;

                    if (rtn == 0) ViewBag.StatusMessage = "error";
                    else
                    {
                        #region insert user
                        foxhun_users user = new isport_foxhun.foxhun_users();
                        user.id = Guid.NewGuid().ToString();
                        user.createdate = DateTime.Now;
                        user.username = model.email;
                        user.password = model.password;
                        user.role = AppCodeModel.USERROLE.TEAM.ToString();
                        user.team_id = teamId;
                        new foxhunt_users().insert(user);
                        AppUtils.Session.User = user;
                        #endregion

                        #region insert team
                        SqlHelper.ExecuteNonQuery(AppCodeModel.strConnDB
                        , CommandType.StoredProcedure, "usp_foxhun_teaminsert"
                        , new SqlParameter[] {new SqlParameter("@id",teamId)
                ,new SqlParameter("@region",model.region)
                ,new SqlParameter("@seq",model.seq)
                ,new SqlParameter("@create_datetime",DateTime.Now)
                ,new SqlParameter("@update_date",DateTime.Now)
                ,new SqlParameter("@name",model.name)
                ,new SqlParameter("@detail",model.detail)
                ,new SqlParameter("@email",model.email)
                ,new SqlParameter("@image",model.image)
                ,new SqlParameter("@username",model.email)
                ,new SqlParameter("@password",model.password)
                ,new SqlParameter("@contact",model.contact)
                ,new SqlParameter("@phone",model.phone)
                ,new SqlParameter("@contact1",model.contact1)
                ,new SqlParameter("@phone1",model.phone1)
                ,new SqlParameter("@contact2",model.contact2)
                ,new SqlParameter("@contact3",model.contact3)
                ,new SqlParameter("@file1",modelFileUpload.pathDirectory)
                ,new SqlParameter("@file2",model.file2)
                ,new SqlParameter("@file3",model.file3)
                ,new SqlParameter("@file4",model.file4)
                ,new SqlParameter("@file5",model.file5) // image contact
                ,new SqlParameter("@file6",model.file6) // image contact1
                ,new SqlParameter("@file7",model.file7) // image contact2                 
                ,new SqlParameter("@file8",model.file8) // image contact3
                ,new SqlParameter("@file9","") // image contact4
                ,new SqlParameter("@file10","") // image contact5
                ,new SqlParameter("@contact4",model.contact4)
                ,new SqlParameter("@contact5",model.contact5)
                ,new SqlParameter("@contact6",model.contact6)
                ,new SqlParameter("@phone2",model.phone2)
                ,new SqlParameter("@phone3",model.phone3)
                ,new SqlParameter("@phone4",model.phone4)
                ,new SqlParameter("@phone5",model.phone5)
                ,new SqlParameter("@phone6",model.phone6)

                        });
                        #endregion

                        new AppCodeModel().SendEmail(
                            model.email
                            , ConfigurationManager.AppSettings["Isport_MailConfirm_Subject"]
                            , String.Format(ConfigurationManager.AppSettings["Isport_MailConfirm_Body"], model.email, model.password)
                            );
                    }

                    #endregion

                    TempData["Register_Message"] = System.Configuration.ConfigurationManager.AppSettings["Register_Message"];

                    return RedirectToAction("index", "team", new { id = teamId });//View("Region",model);//View(model); //RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
                }
            }catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public string CheckUserDup(string username,string password)
        {
            try {
                string rtn = "";
                List<foxhun_users> users =  new foxhunt_users().getUsers(username, password);
                if (users.Count > 0) rtn = "ขออภัยค่ะ ข้อมูลโรงเรียนของท่านได้เคยสมัครเรียบร้อยแล้ว กรุณาใช้ Email และ password Login เข้าระบบค่ะ";
                return rtn;
            }
            catch(Exception ex)
            { throw new Exception(ex.Message); }
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