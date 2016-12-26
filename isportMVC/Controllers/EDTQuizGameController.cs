using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using isport;
namespace isportMVC.Controllers
{
    public class EDTQuizGameController : Controller
    {
        // GET: EDTQuizGame
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult EDTQuizGameInsert(Models.EDTQuizGameModelInsert model)
        {
            string strSuccess="", strError = "";
            try
            {
                string img = model.image;
                string question = model.txtQuestion;
                string choise = model.txtChoise;
                string answer = model.txtAnswer;

                isport.AppCode appCode = new AppCode();
                  string  pcntId = appCode.Insert_SipContent("301"
                            ,isport_service_core.AppCode.GenGoogleAPI_ShoterURL("http://wap.isport.co.th/isportui/?p="+model.idEDT+"&pcat=301")
                            , question.Replace("\r", "").Replace("\n", "").Length > 150 ? question.Replace("\r", "").Replace("\n", "").Substring(0, 150) : question.Replace("\r", "").Replace("\n", "")
                            , choise.Replace("\r", "").Replace("\n", "").Length > 150 ? choise.Replace("\r", "").Replace("\n", "").Substring(0, 150) : choise.Replace("\r", "").Replace("\n", "")
                            , answer.Replace("\r", "").Replace("\n", "")
                            , DateTime.ParseExact(model.displayDate, "dd/MM/yyyy",null).ToString("yyyyMMdd")
                            , ""
                            , "N"
                            ,model.image);

                string uiID = Guid.NewGuid().ToString();
                appCode.InsertUI(GetUIRow(uiID, pcntId, model.idEDT), GetContentRow(uiID,model));




                strSuccess = "Success";
            }
            catch(Exception ex)
            {
                strSuccess = "500";
                strError = ex.Message;
                Response.TrySkipIisCustomErrors = true;
                //return Content(ex.Message, "text/html");
            }

            return Json(new { strSuccess = strSuccess, strError = strError }, 0);
        }
        private isportDS.wapisport_contentRow GetContentRow(string masterID,Models.EDTQuizGameModelInsert model)
        {
            try
            {
                isportDS.wapisport_contentRow drContent = new isportDS.wapisport_contentDataTable().Newwapisport_contentRow();
                drContent.content_id = Guid.NewGuid().ToString();
                drContent.master_id = masterID;
                drContent.content_icon = "";
                drContent.content_createdate = DateTime.Now;
                drContent.content_link = "";
                drContent.content_text = model.txtQuestion +" <br/>" +model.txtChoise;
                drContent.content_align = "Center";
                drContent.content_breakafter = true;
                drContent.content_color = "Black";
                drContent.content_bold = "true";
                drContent.content_isredirect = false;
                drContent.content_bgcolor = "white";
                drContent.content_txtsize = "medium";
                drContent.content_isgallery = false;


                #region Single File


                drContent.content_image = model.image;//ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileName;
                    //ExceptionManager.WriteError(filUpload.PostedFile.ContentType);


                #endregion


                return drContent;
            }
            catch (Exception ex)
            {
                throw new Exception("GetContentRow>>" + ex.Message);
            }
        }
        private isportDS.wapisport_uiRow GetUIRow(string uiID, string pcntId,string prjType)
        {
            try
            {
                //string sgConfigName = "SG_PCAT" + ddlSubName.SelectedValue;
                isportDS.wapisport_uiRow drUi = new isportDS.wapisport_uiDataTable().Newwapisport_uiRow();
                //ViewState["lastId"] = txtName.Text;
                drUi.ui_id = uiID;
                drUi.ui_master_id = pcntId; // เวลา Edite จะได้ไปแก้ไขที่ Sip_content ได้
                drUi.ui_projecttype = prjType;//ddlProject.SelectedValue;//txtProject.Text;//ConfigurationManager.AppSettings["Isport_ProjectType"].ToString();
                drUi.ui_level = 0;
                drUi.ui_index = 0;
                drUi.ui_operator = "All";
                drUi.ui_startdate = DateTime.Now;
                drUi.ui_createdate = DateTime.Now;// DateTime.ParseExact(txtDisplayDate.Text, "yyyyMMdd", null);
                drUi.ui_updatedate = DateTime.Now;
                drUi.ui_createuser = "";
                drUi.ui_updateuser = "";
                drUi.ui_createip = Request.UserHostAddress;
                drUi.ui_updateip = Request.UserHostAddress;
                drUi.ui_ismaster = false;
                drUi.ui_ispayment = false;
                drUi.ui_sg_id = "";
                drUi.ui_isnews = false;
                drUi.ui_isnews_top = "";
                drUi.ui_contentname = "";
                drUi.ui_ispaymentwap = false;
                drUi.ui_ispaymentsms = false;
                return drUi;
            }
            catch (Exception ex)
            {
                throw new Exception("GetUIRow>>" + ex.Message);
            }
        }

        public Models.EDTQuizGameModelList uploadFiles()
        {
            Session["EDTDATA"] = null;
            Models.EDTQuizGameModelList listModel = new Models.EDTQuizGameModelList();
            listModel.listModel = new List<Models.EDTQuizGameModel>();
            foreach (string s in Request.Files)
            {
                try
                {
                    var file = Request.Files[s];
                    string fileName = file.FileName;
                    string fileExtension = file.ContentType;
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string newfilename = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddHHmmsss"), fileName);
                        string savefilepath = Server.MapPath("~/Uploadfiles/") + newfilename;
                        // Upload File
                        file.SaveAs(savefilepath);
                        var newFile = new FileInfo(savefilepath);
                        //While File is not accesable because of writing process
                        //while (IsFileLocked(newFile)) { }

                        DataTable dt = getData(savefilepath);
                        foreach(DataRow dr in dt.Rows)
                        {
                            try
                            {
                                string json = new IsportWS.push().SendGet(string.Format(ConfigurationManager.AppSettings["Isport_EDT_GetDATA"], GetEDTID(dr[0].ToString())));

                                List<Models.EDTQuizGameModel> list = (List<Models.EDTQuizGameModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(json, typeof(List<Models.EDTQuizGameModel>));

                                listModel.listModel.Add(list[0]);
                            }
                            catch(Exception ex) {
                                string err = ex.Message;
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Session["EDTDATA"] = listModel;
            return listModel;

        }

        public ActionResult EDTModelList()
        {
            Models.EDTQuizGameModelList listModel= (Models.EDTQuizGameModelList)Session["EDTDATA"];
            return PartialView(listModel);
        }

        private string GetEDTID(string link)
        {
            string[] edtIds = link.Split('/');
            string edtId = edtIds[edtIds.Length - 1];

            if (edtId.IndexOf('_') > -1) edtId = (edtId.Split('_'))[0];
            else
            {
                // format ไม่ปกติ ex. travel.edtguide.com/386769 , eat.edtguide.com/387435/sand-sea-sun-restaurant
                foreach (string id in edtIds)
                {
                    try
                    {
                        int number;
                        bool result = Int32.TryParse(id, out number);
                        if (result)
                        {
                            edtId = id;
                            break;
                        }
                    }
                    catch { }
                }
            }

            return edtId;
        }

        private DataTable getData(string path)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("link"), new DataColumn("id") });
            //Response.Write(path);
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source= " + path + " ; Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(connectionString);
            if (conn.State == ConnectionState.Open) conn.Close();
            conn.Open();

            try
            {
                string sql = "select * from [EDT$A:F]";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataReader drRead = cmd.ExecuteReader();
                DataRow dr = null;
                while (drRead.Read())
                {
                    dr = dt.NewRow();
                    try
                    {

                        if (drRead[0].ToString().Trim() != "")
                        {
                            dr["link"] = drRead[0];
                            dt.Rows.Add(dr);
                        }
                    }
                    catch { }
                    
                }
            }
            catch { }

            return dt;
        }


        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            //file is not locked
            return false;
        }
    }
}