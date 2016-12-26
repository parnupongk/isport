using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using OracleDataAccress;
using DevExpress.Web.ASPxGridView;
namespace IsportApp_Report
{
    public partial class wap_report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBindServiceGroup();
            }
        }

        private void DataBindServiceGroup()
        {
            try
            {
                

                DataSet ds = OrclHelper.Fill(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString()
                    , CommandType.StoredProcedure, "ISPORT_REPORT.SportApp_GetReportGroupBySG", "i_service_group"
                    , new OracleParameter[] {OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor, ParameterDirection.Output) });

                ddlSg.DataTextField = "sg_name";
                ddlSg.DataValueField  = "sg_id";
                ddlSg.DataSource = ds.Tables[0];
                ddlSg.DataBind();
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DataBindViewByServiceGroup()
        {
            string startDate = dateStart.Date.ToString("yyyMMdd");
            string endDate = dateEnd.Date.ToString("yyyyMMdd");

            DataSet ds = OrclHelper.Fill(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString()
                    , CommandType.StoredProcedure, "ISPORT_REPORT.SportApp_GetReportViewBySG", "i_rep_wap_new"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_startdate",startDate,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_enddate",endDate,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("p_sg_id",ddlSg.SelectedValue,OracleType.VarChar,ParameterDirection.Input)
                                                            ,OrclHelper.GetOracleParameter("o_Content", "", OracleType.Cursor, ParameterDirection.Output) });

            ViewState["Report"] = ds;
            gvReport.DataSource = ds;
            gvReport.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataBindViewByServiceGroup();
        }

        protected void btnReportExport_Click(object sender, ImageClickEventArgs e)
        {
            gvReport.DataSource = (DataSet)ViewState["Report"];
            gvReport.DataBind();

            gvReportExport.WriteXlsToResponse("ViewByClick_" + DateTime.Now.ToString("ddMMyyyy") + ".xls");
        }

        protected void gvReport_PageIndexChanged(object sender, EventArgs e)
        {
            DataBindViewByServiceGroup();
        }

        protected void gvReport_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            string strDate = e.GetValue("REP_DATE").ToString();
            DateTime date = new DateTime(int.Parse(strDate.Substring(0, 4)), int.Parse(strDate.Substring(4, 2)), int.Parse(strDate.Substring(6, 2)));
            if (date.Date.ToString("dddd") == "Saturday" || date.Date.ToString("dddd") == "Sunday")
            {
                e.Row.Cells[0].Text = strDate + " *";
                e.Row.Font.Bold = true;
                e.Row.ForeColor = System.Drawing.Color.Red;
                //e.Row.Cells[0].BackColor = System.Drawing.Color.Red;
                e.Row.BackColor = System.Drawing.Color.LightCyan;

            }
        }
    }
}
