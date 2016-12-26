using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using OracleDataAccress;
using WebLibrary;
namespace isport.admin
{
    public partial class adminbanner : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBindBanner();
            }
        }
        private void DataBindBanner()
        {
            try
            {
                DataSet ds = OrclHelper.Fill(AppMain.strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_SelectAll_Banner"
                    , "IsportApp_Banner", new OracleParameter[] { OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)});

                ViewState["DataBanner"] = ds;
                gvData.DataKeyNames = new string[] { "ID"};
                gvData.DataSource = ds;
                gvData.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception("DataBindBanner >> " + ex.Message);
            }
        }

        private void DataBindBannerFillter()
        {

            try
            {
                DataSet ds = (DataSet)ViewState["DataBanner"];
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = ds.Tables[0].DefaultView;
                    System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
                    if( cmbOptCode.SelectedValue != "" )
                    {
                        str.Append(" app_name='" + cmbAppName.SelectedValue + "' and opt_code='" + cmbOptCode.SelectedValue + "' and banner_type='" + cmbBannerType.SelectedValue + "'");
                    }
                    else str.Append(" app_name='" + cmbAppName.SelectedValue + "'");
                    dv.RowFilter = str.ToString();
                    gvData.DataSource = dv;
                    gvData.DataBind();
                }
                else
                {
                    DataBindBanner();
                }
            }
            catch(Exception ex)
            {
                throw new Exception("DataBindBannerFillter >> " + ex.Message);
            }
        }
        private void ClareAll()
        {
            
            txtBigURL.Text = "";
            txtMediumURL.Text = "";
            txtSmallURL.Text = "";
            txtPhone.Text = "";
            txtLink.Text = "";
            txtTitle.Text = "";
            txtDetail.Text = "";
            txtFooter.Text = "";
            cmbBannerType.ClearSelection();
            cmbOptCode.ClearSelection();
        }
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                OrclHelper.ExecuteNonQuery(AppMain.strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_Insert_Banner"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("P_ID",Guid.NewGuid().ToString() ,OracleType.NVarChar,ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("P_OPT_CODE",cmbOptCode.SelectedValue,OracleType.NVarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("P_APP_NAME",cmbAppName.SelectedValue ,OracleType.NVarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("P_BIG_IMG",txtBigURL.Text,OracleType.NVarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("P_MEDIUM_IMG",txtMediumURL.Text,OracleType.NVarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("P_SMALL_IMG",txtSmallURL.Text,OracleType.NVarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("P_PHONE_NO",txtPhone.Text,OracleType.NVarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("P_LINK",txtLink.Text,OracleType.NVarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("P_IP",Request.ServerVariables["REMOTE_ADDR"],OracleType.NVarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("P_USR_ID",WebLibrary.MitTool.GetCookie(Request,"admin").ToString(),OracleType.NVarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("P_TITLE",txtTitle.Text,OracleType.NVarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("P_DETAIL",txtDetail.Text,OracleType.NVarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("P_FOOTER",txtFooter.Text,OracleType.NVarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("P_MODEL_TYPE",cmbModel.SelectedValue,OracleType.NVarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("P_BANNER_TYPE",cmbBannerType.SelectedValue,OracleType.NVarChar,ParameterDirection.Input) 
                                                     });

                DataBindBanner();
                ClareAll();
            }
            catch (Exception ex)
            {
                throw new Exception("btnInsert_Click >> " + ex.Message);
            }
        }
        protected string GetImageUrl(string dbImgURL)
        {
            return dbImgURL;
            //if (System.IO.File.Exists(dbImgURL))
            //{
            //    return dbImgURL;
            //}
            //else
            //{
            //    return "Green.gif";
            //}
        }

        protected void cmbAppName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBindBannerFillter();
        }


        protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
            OrclHelper.ExecuteNonQuery(AppMain.strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_Delete_Banner"
                , new OracleParameter []{OrclHelper.GetOracleParameter("P_ID", gvData.DataKeys[e.RowIndex].Value.ToString(), OracleType.NVarChar, ParameterDirection.Input)} );
            DataBindBanner();
        }

        protected void cmbOptCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBindBannerFillter();
        }

        protected void gvData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txtOptCode = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtOptCode");
            TextBox txtAppName = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtAppName");
            TextBox txtPhoneNo = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtPhoneNo");
            TextBox txtLinkEdit = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtLinkEdit");
            TextBox txtTitleEdit = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtTitleEdit");
            TextBox txtDetailEdit = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtDetailEdit");
            TextBox txtBigImg = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtBigImg");
            TextBox txtMediumImg = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtMediumImg");
            TextBox txtSmallImg = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtSmallImg");
            TextBox txtBannerType = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtBannerType");

            //txtLink.Text = gvData.DataKeys[e.RowIndex].Value.ToString() + "|" + txtOptCode.Text + "|" + txtUrl.Text;
            OrclHelper.ExecuteNonQuery(AppCode.strConnOracle, CommandType.StoredProcedure, "ISPORT_APP.SportApp_Update_Banner"
                , new OracleParameter[] {OrclHelper.GetOracleParameter("P_ID",gvData.DataKeys[e.RowIndex].Value.ToString(),OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("P_OPT_CODE",txtOptCode.Text,OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("P_BIG_IMG",txtBigImg.Text,OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("P_MEDIUM_IMG",txtMediumImg.Text,OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("P_SMALL_IMG",txtSmallImg.Text,OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("P_PHONE_NO",txtPhoneNo.Text,OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("P_LINK",txtLinkEdit.Text,OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("P_TITLE",txtTitleEdit.Text,OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("P_DETAIL",txtDetailEdit.Text,OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("P_BANNER_TYPE",txtBannerType.Text,OracleType.VarChar,ParameterDirection.Input)
                                        });
            gvData.EditIndex = -1;
            DataBindBanner();
            //DataBindBannerFillter
        }

        protected void gvData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvData.EditIndex = e.NewEditIndex;
            //DataBindBanner();
            DataBindBannerFillter();
        }

        protected void gvData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvData.EditIndex = -1;
            //DataBindBanner();
            DataBindBannerFillter();
        }
    }
}
