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
    public partial class adminfooter : System.Web.UI.Page
    {
        #region property
        /// <summary>
        /// Status row
        /// </summary>
        private string GetID
        {
            get
            {
                return ViewState["ID"] == null ? "" : ViewState["ID"].ToString();
            }
            set
            {
                ViewState["ID"] = value;
            }
        }
        private int RowCount
        {
            get
            {
                return ViewState["rowcount"] == null ? 0 : int.Parse(ViewState["rowcount"].ToString());
            }
            set
            {
                ViewState["rowcount"] = value;
            }
        }
        #endregion

        #region Pageload & databind
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBindProjectType();
                GridDataBinding();
            }
        }
        private void DataBindProjectType()
        {
            try
            {
                System.Data.DataSet ds = new AppCode().SelectUIGroupProjectType();
                ddlProject.DataTextField = "ui_projecttype";
                ddlProject.DataValueField = "ui_projecttype";
                ddlProject.DataSource = ds;
                ddlProject.DataBind();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("DataBindProjectType " + ex.Message);
            }
        }
        private void GridDataBinding()
        {
            try
            {
                System.Data.DataSet ds = new AppCode().SelectFooterAll(ddlProject.SelectedValue );
                RowCount = ds.Tables[0].Rows.Count;
                gvContent.DataSource = ds;
                gvContent.DataBind();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("GridDataBinding>>" + ex.Message);
            }
        }
        #endregion

        #region GetRow

        private isportDS.wapisport_footerRow GetFooterRow(string footerId)
        {
            try
            {
                isportDS.wapisport_footerRow dr = new isportDS.wapisport_footerDataTable().Newwapisport_footerRow();
                dr.footer_id = footerId;
                dr.footer_projecttype = txtProjectType.Text;//ddlProject.SelectedValue;
                dr.footer_level = ViewState["l"] == null ? 0 : int.Parse(ViewState["l"].ToString());
                dr.footer_index = int.Parse(((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIndex")).Text);
                dr.footer_operator = ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlOperator")).SelectedValue;
                dr.footer_startdate = ((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateStart")).Date;
                if (((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateEnd")).Text != "")
                {
                    dr.footer_enddate = ((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateEnd")).Date;
                }
                dr.footer_createdate = DateTime.Now;
                dr.footer_updatedate = DateTime.Now;
                dr.footer_createuser = "";
                dr.footer_updateuser = "";
                dr.footer_createip = Request.UserHostAddress;
                dr.footer_updateip = Request.UserHostAddress;
                dr.footer_ispayment = ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkPay")).Checked;
                dr.footer_isdefault = ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkDefault")).Checked;
                dr.footer_sg_id = ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtSgId")).Text;
                return dr;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private isportDS.wapisport_contentRow GetContentRow(string masterID)
        {
            try
            {
                isportDS.wapisport_contentRow drContent = new isportDS.wapisport_contentDataTable().Newwapisport_contentRow();
                drContent.content_id = Guid.NewGuid().ToString();
                drContent.master_id = masterID;
                drContent.content_icon = ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIcon")).Text;
                drContent.content_createdate = DateTime.Now;
                drContent.content_link = ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtLink")).Text;
                drContent.content_text = ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtText")).Text;
                drContent.content_align = ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlAlign")).SelectedValue;
                drContent.content_breakafter = ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkBreakAfter")).Checked;
                FileUpload filUpload = (FileUpload)selectProductsPopUp.Windows[0].FindControl("filUpload");
                if (filUpload.PostedFile.ContentLength > 0)
                {
                    string fileName = DateTime.Now.ToString("yyyMMddHHss") + filUpload.FileName;
                    filUpload.PostedFile.SaveAs(Server.MapPath(ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileName));
                    drContent.content_image = ConfigurationManager.AppSettings["Isport_FolderUpload"] + fileName;
                }
                else if (((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkDelete")).Checked) drContent.content_image = "";
                return drContent;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GridView
        protected void gvImages_PageIndexChanged(object sender, EventArgs e)
        {
            DevExpress.Web.ASPxGridView.ASPxGridView grid =
        (DevExpress.Web.ASPxGridView.ASPxGridView)selectProductsPopUp.Windows[1].FindControl("gvImages");


            grid.DataSource = new AppImages().ImagesSelect(); ;
            grid.DataBind();
        }

        public string CheckImagePath(string pathImage)
        {
            return pathImage == null || pathImage == "" ? "~/images/no_image.jpg" : pathImage;
        }
        protected void gvImages_RowCommand(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewRowCommandEventArgs e)
        {
            try
            {
                if (e.CommandArgs.CommandName == "Select")
                {
                    ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIcon")).Text = e.CommandArgs.CommandArgument.ToString();
                }

                selectProductsPopUp.Windows[0].ShowOnPageLoad = true;
                selectProductsPopUp.Windows[1].ShowOnPageLoad = false;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
            }
        }

        protected void gvContent_RowCommand(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewRowCommandEventArgs e)
        {
            try
            {
                string comm = e.CommandArgs.CommandName;
                object[] obj = (object[])gvContent.GetRowValues(e.VisibleIndex
                    , new string[] { "footer_id", "footer_index","footer_operator"
                    ,"footer_startdate","footer_enddate","footer_ispayment"
                    ,"content_icon","content_link","content_text"
                    ,"content_align","footer_isdefault","footer_sg_id","content_image","content_breakafter" });
                if (comm == "Edit")
                {
                    // header
                    GetID = obj[0].ToString();
                    ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIndex")).Text = obj[1].ToString();
                    ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtSgId")).Text = obj[11] == null ? "" : obj[11].ToString();
                    ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlOperator")).ClearSelection();
                    ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlOperator")).Items.FindByValue(obj[2].ToString()).Selected = true;
                    ((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateStart")).Date = (DateTime)obj[3];
                    if (obj[4] != null && obj[4].ToString() != "")
                    {
                        ((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateEnd")).Date = (DateTime)obj[4];
                    }
                    ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkPay")).Checked = (bool)obj[5];
                    ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkDefault")).Checked = (bool)obj[10];

                    // Content
                    ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIcon")).Text = obj[6].ToString();
                    ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtLink")).Text = obj[7].ToString();
                    ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtText")).Text = obj[8].ToString();
                    ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkBreakAfter")).Checked = (bool)obj[13];
                    ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkDelete")).Checked = false;
                    ((Image)selectProductsPopUp.Windows[0].FindControl("img")).ImageUrl
                        = obj[12] == null || obj[12].ToString() == "" ? "~/images/no_image.jpg" : obj[12].ToString();
                    ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlAlign")).ClearSelection();
                    ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlAlign")).Items.FindByValue(obj[9].ToString()).Selected = true;
                    selectProductsPopUp.Windows[0].ShowOnPageLoad = true;
                }
                else if (comm == "Delete")
                {
                    // Delete
                    new AppCode().DeleteFooter(obj[0].ToString());
                    GridDataBinding();
                }
                else if (comm == "insert")
                {
                    ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIndex")).Text = ((int)obj[1] - 1).ToString();
                    selectProductsPopUp.Windows[0].ShowOnPageLoad = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("gvContent_RowCommand>>" + ex.Message);
            }
        }
        #endregion

        #region Button
        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string obj = "";
                if (GetID == "")
                {
                    // new
                    obj = Guid.NewGuid().ToString();
                    obj = new AppCode().InsertFooter(GetFooterRow(obj), GetContentRow(obj)).ToString();
                }
                else
                {
                    // update
                    obj = GetID;
                    obj = new AppCode().UpdateFooter(GetFooterRow(obj), GetContentRow(obj)).ToString();
                }

                GetID = "";
                GridDataBinding();
                selectProductsPopUp.Windows[0].ShowOnPageLoad = false;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("btnSubmit>>" + ex.Message);
            }
        }
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            GetID = "";
            ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIndex")).Text = (RowCount + 1).ToString();
            ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtSgId")).Text = "";
            ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlOperator")).ClearSelection();
            ((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateStart")).Date = DateTime.Now;
            ((DevExpress.Web.ASPxEditors.ASPxDateEdit)selectProductsPopUp.Windows[0].FindControl("dateEnd")).Text = "";

            ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkPay")).Checked = false;
            ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkDefault")).Checked = false;

            // Content
            ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtIcon")).Text = "";
            ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtLink")).Text = "";
            ((TextBox)selectProductsPopUp.Windows[0].FindControl("txtText")).Text = "";
            ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkBreakAfter")).Checked = false;
            ((CheckBox)selectProductsPopUp.Windows[0].FindControl("chkDelete")).Checked = false;
            ((Image)selectProductsPopUp.Windows[0].FindControl("img")).ImageUrl = "~/images/no_image.jpg";
            ((DropDownList)selectProductsPopUp.Windows[0].FindControl("ddlAlign")).ClearSelection();
            selectProductsPopUp.Windows[0].ShowOnPageLoad = true;
        }
        protected void btnSearech_Click(object sender, ImageClickEventArgs e)
        {
            DevExpress.Web.ASPxGridView.ASPxGridView grid =
                    (DevExpress.Web.ASPxGridView.ASPxGridView)selectProductsPopUp.Windows[1].FindControl("gvImages");

            grid.DataSource = new AppImages().ImagesSelect();
            grid.DataBind();

            selectProductsPopUp.Windows[0].ShowOnPageLoad = false;
            selectProductsPopUp.Windows[1].ShowOnPageLoad = true;
        }
        protected void btnProjectType_Click(object sender, EventArgs e)
        {
            GridDataBinding();
        }
        #endregion

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProjectType.Text = ddlProject.SelectedValue;
            GridDataBinding();
        }




    }
}
