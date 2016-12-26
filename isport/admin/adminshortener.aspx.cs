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
    public partial class adminshortener : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtStartDate.Text = DateTime.Now.ToString("yyyyMMdd");
                txtEndDate.Text = DateTime.Now.AddYears(99).ToString("yyyyMMdd");
                DataBindData();
            }
        }

        private void DataBindData()
        {
            try
            {
                DataSet ds = OrclHelper.Fill(AppCode.strConnOracle, CommandType.StoredProcedure, "ISPORT_UIADMIN.ISPORT_SHORTENER_SELECTALL", "isport_shorturl",
                    new OracleParameter[] {OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output) });

                ViewState["ShortenerDate"] = ds;

                if (cmbOptCode.SelectedIndex > 0)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataView dv = ds.Tables[0].DefaultView;
                        System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
                        str.Append(" opt_code='" + cmbOptCode.SelectedValue + "'");
                        dv.RowFilter = str.ToString();

                        gvData.DataSource = dv;
                        gvData.DataBind();
                    }
                }
                else
                {
                    gvData.DataSource = ds;
                    gvData.DataBind();
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("DataBindData >> " + ex.Message);
            }
        }

        private void DataBindDataFillter()
        {
            try
            {

                DataSet ds = (DataSet)ViewState["ShortenerDate"];
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = ds.Tables[0].DefaultView;
                    System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
                    str.Append(" opt_code='" + cmbOptCode.SelectedValue + "'");
                    dv.RowFilter = str.ToString();
                    gvData.DataSource = dv;
                    gvData.DataBind();
                }

                else { DataBindData(); }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("DataBindDataFillter >> " + ex.Message);
            }
        }
        private void ClareAll()
        {
            txtParameter.Text = "s";
            txtParameterValue.Text = "";
            txtLink.Text = "";
            txtDesc.Text = "";
        }
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                OrclHelper.ExecuteNonQuery(AppMain.strConnOracle, CommandType.StoredProcedure, "ISPORT_UIADMIN.ISPORT_SHORTENER_INSERT"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_url_id",Guid.NewGuid().ToString() ,OracleType.VarChar,ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("p_start_date",DateTime.ParseExact(txtStartDate.Text, "yyyyMMdd", null),OracleType.DateTime,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("p_end_date",DateTime.ParseExact(txtEndDate.Text , "yyyyMMdd", null),OracleType.DateTime,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("p_active","Y",OracleType.VarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("p_opt_code",cmbOptCode.SelectedValue,OracleType.VarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("p_parameter",txtParameter.Text,OracleType.VarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("p_url",txtLink.Text,OracleType.VarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("p_parameter_value",txtParameterValue.Text,OracleType.VarChar,ParameterDirection.Input) 
                                                        ,OrclHelper.GetOracleParameter("p_description",txtDesc.Text,OracleType.VarChar,ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("p_pssv_id",txtPssvId.Text,OracleType.VarChar,ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("p_pssv_name",txtPssvName.Text,OracleType.VarChar,ParameterDirection.Input)
                                                     });

                DataBindData();
                ClareAll();
            }
            catch (Exception ex)
            {
                throw new Exception("btnInsert_Click >> " + ex.Message);
            }
        }

        protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            OrclHelper.ExecuteNonQuery(AppCode.strConnOracle, CommandType.StoredProcedure, "ISPORT_UIADMIN.ISPORT_SHORTENER_Delete"
                , new OracleParameter[] {OrclHelper.GetOracleParameter("p_url_id",gvData.DataKeys[e.RowIndex].Value.ToString(),OracleType.VarChar,ParameterDirection.Input)});
            DataBindData();
        }

        protected void cmbOptCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBindDataFillter();
        }

        protected void gvData_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvData.EditIndex = e.NewEditIndex;
            //DataBindData();
            DataBindDataFillter();
        }

        protected void gvData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            TextBox txtOptCode = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtOptCode");
            TextBox txtUrl = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtURL");
            TextBox txtParaValue = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtParavalue");
            TextBox txtDesc = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtDesc");
            TextBox txtPssvId = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtPssvId");
            TextBox txtPssvName = (TextBox)gvData.Rows[e.RowIndex].FindControl("txtPssvName");
            //txtLink.Text = gvData.DataKeys[e.RowIndex].Value.ToString() + "|" + txtOptCode.Text + "|" + txtUrl.Text;
            OrclHelper.ExecuteNonQuery(AppCode.strConnOracle, CommandType.StoredProcedure, "ISPORT_UIADMIN.ISPORT_SHORTENER_Update"
                , new OracleParameter[] {OrclHelper.GetOracleParameter("p_url_id",gvData.DataKeys[e.RowIndex].Value.ToString(),OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_opt_code",txtOptCode.Text,OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_url",txtUrl.Text,OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_parameter_value",txtParaValue.Text,OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_description",txtDesc.Text,OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_pssv_id",txtPssvId.Text,OracleType.VarChar,ParameterDirection.Input)
                                        ,OrclHelper.GetOracleParameter("p_pssv_name",txtPssvName.Text,OracleType.VarChar,ParameterDirection.Input)
                                        });
            gvData.EditIndex = -1;
            DataBindData();
            //e.NewValues[e.RowIndex].ToString();
        }

        protected void gvData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvData.EditIndex = -1;
            //DataBindData();
            DataBindDataFillter();
        }
    }
}
