using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using OracleDataAccress;

namespace IsportApp_Report
{
    public partial class wap_monitor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !IsCallback) return;
        }
        private DataView FillterGrid(string pfileName)
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder(string.Empty);
            DataSet ds = (DataSet)ViewState["ViewPageReport"];
            DataView dv = (DataView)ds.Tables[0].DefaultView;
            str.Append("PFILE_NAME =" + pfileName);
            dv.RowFilter = str.ToString();
            return dv;
        }
        private void GridDataBind()
        {
            string startDate = dateStart.Date.ToString("yyyMMdd");
            DataSet ds = OrclHelper.Fill(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString()
                , CommandType.StoredProcedure, "WAP_UI.i_payment_log_file", "isportapp_report"
                , new OracleParameter[] { OrclHelper.GetOracleParameter("p_date", startDate, OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)});

            ViewState["ViewPageReport"] = ds;
            gvViewPage.DataSource = ds;
            gvViewPage.DataBind();

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            GridDataBind();
        }

        protected void btnViewPage_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void gvViewPage_PageIndexChanged(object sender, EventArgs e)
        {
            GridDataBind();
        }

        protected void gvViewPage_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            DataRow dr = (DataRow) gvViewPage.GetDataRow(e.VisibleIndex);
            string pfileName = dr["PFILE_NAME"].ToString();
            btnSubmit.Text = pfileName;
            DataView dv= FillterGrid(pfileName);
            gvViewPage.DataSource = dv;
            gvViewPage.DataBind();
        }

        protected void gvViewPage_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            btnSubmit.Text = e.Parameters;
        }

    }
}
