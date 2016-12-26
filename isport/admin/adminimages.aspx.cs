using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using WebLibrary;
namespace isport.admin
{
    public partial class adminimages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridDataBind();
            }
        }

        private void GridDataBind()
        {
            try
            {
                gvImages.DataSource = new AppImages().ImagesSelect();
                gvImages.DataBind();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                FileUpload filUpload = (FileUpload)selectProductsPopUp.Windows[0].FindControl("filUpload");
                if (filUpload.PostedFile.ContentLength > 0)
                {
                    string fileName = "icon_" + DateTime.Now.ToString("yyyMMddHHss") + filUpload.FileName;
                    filUpload.PostedFile.SaveAs(Server.MapPath(ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileName));
                    isportDS.wapisport_imagesRow dr = new isportDS.wapisport_imagesDataTable().Newwapisport_imagesRow();
                    dr.images_id = Guid.NewGuid().ToString();
                    dr.images_date = DateTime.Now;
                    dr.images_path = ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileName;
                    dr.images_type = AppCode.ImagesType.icon.ToString();
                    new AppImages().ImagesInsert(dr);
                }
                selectProductsPopUp.Windows[0].ShowOnPageLoad = false;
                GridDataBind();
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }
        }

        protected void gvImages_RowCommand(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewRowCommandEventArgs e)
        {
            try
            {
                if (e.CommandArgs.CommandName == "Delete")
                {
                    new AppImages().ImagesDelete(e.KeyValue.ToString());
                    GridDataBind();
                }
            }
            catch(Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            selectProductsPopUp.Windows[0].ShowOnPageLoad = true;
        }

        protected void gvImages_PageIndexChanged(object sender, EventArgs e)
        {
            GridDataBind();
        }
    }
}
