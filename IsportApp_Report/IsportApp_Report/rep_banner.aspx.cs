using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using OracleDataAccress;
using System.Configuration;
using DevExpress.Web.ASPxGridView;

namespace IsportApp_Report
{
    public partial class rep_banner : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) SetFramBanner();
            else
            {
                BindReport();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                BindReport();
            }
            catch { }
        }

        private void BindReport()
        {
            try
            {
                string startDate = dateStart.Date.ToString("yyyMMdd");
                string endDate = dateEnd.Date.ToString("yyyyMMdd");

                DataSet ds = OrclHelper.Fill(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString()
                    , CommandType.StoredProcedure, "ISPORT_REPORT.SportApp_GetReportBanner", "isportapp_report"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_startdate", startDate, OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("p_enddate", endDate, OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("p_app_name", cmbAppName.SelectedItem.Value.ToString(), OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)});

                DataSet dsClick = OrclHelper.Fill(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString()
                    , CommandType.StoredProcedure, "ISPORT_REPORT.SportApp_GetReportClick", "isportapp_report"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_startdate", startDate, OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("p_enddate", endDate, OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("p_app_name", cmbAppName.SelectedItem.Value.ToString(), OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)});

                DataSet dsPage = OrclHelper.Fill(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString()
                    , CommandType.StoredProcedure, "ISPORT_REPORT.SportApp_GetReportByPage", "isportapp_report"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_startdate", startDate, OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("p_enddate", endDate, OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("p_app_name", cmbAppName.SelectedItem.Value.ToString(), OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)});

                ViewState["ReportByOperator"] = ds;
                gvOperator.DataSource = ds;
                gvOperator.DataBind();

                ViewState["ReportByClick"] = dsClick;
                gvClick.DataSource = dsClick;
                gvClick.DataBind();

                ViewState["ReportByPage"] = dsPage; 
                gvPage.DataSource = dsPage;
                gvPage.DataBind();
                gvPage.BeginUpdate();
                gvPage.GroupBy(gvPage.Columns["PAGE_NAME"]);
                gvPage.SortBy(gvPage.Columns["PAGE_NAME"], 0);
                //gvPage.GroupBy(gvPage.Columns[1]);
                gvPage.EndUpdate();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SetFramBanner()
        {
            string appName = cmbAppName.SelectedItem.Value.ToString();
            if (appName == "SportArena") frmBanner.Attributes.Add("src", ConfigurationManager.AppSettings["SportArena_BannerURL"]);
            else if (appName == "StarSoccer") frmBanner.Attributes.Add("src", ConfigurationManager.AppSettings["StarSoccer_BannerURL"]);
            else if (appName == "SportPool") frmBanner.Attributes.Add("src", ConfigurationManager.AppSettings["SportPool_BannerURL"]);
        }
        protected void cmbAppName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFramBanner();
        }

        protected void gvPage_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            DataSet dsPage = (DataSet)ViewState["ReportByPage"];
            gvPage.DataSource = dsPage;
            gvPage.DataBind();
            gvPage.BeginUpdate();
            gvPage.GroupBy(gvPage.Columns["PAGE_NAME"]);
            gvPage.GroupBy(gvPage.Columns[1]);
            gvPage.EndUpdate();
        }

        protected void btnViewPage_Click(object sender, ImageClickEventArgs e)
        {
            gvPage.DataSource = (DataSet)ViewState["ReportByPage"];
            gvPage.DataBind();

            gvPageExport.WriteXlsToResponse("ViewPage_" + DateTime.Now.ToString("ddMMyyyy") + ".xls");
        }

        protected void btnOPT_Click(object sender, ImageClickEventArgs e)
        {
            gvOperator.DataSource = (DataSet)ViewState["ReportByOperator"];
            gvOperator.DataBind();

            gvOptExport.WriteXlsToResponse("ViewByOperator_" + DateTime.Now.ToString("ddMMyyyy") + ".xls");
        }

        protected void btnClick_Click(object sender, ImageClickEventArgs e)
        {
            gvClick.DataSource = (DataSet)ViewState["ReportByClick"];
            gvClick.DataBind();

            gvClickExport.WriteXlsToResponse("ViewByClick_" + DateTime.Now.ToString("ddMMyyyy") + ".xls");
        }

        protected void gvOperator_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            string strDate = e.GetValue("DATE_REPORT").ToString();
            DateTime date = new DateTime(int.Parse(strDate.Substring(0, 4)), int.Parse(strDate.Substring(4, 2)), int.Parse(strDate.Substring(6, 2)));
            if (date.Date.ToString("dddd") == "Saturday" || date.Date.ToString("dddd") == "Sunday")
            {
                e.Row.Cells[0].Text = strDate + " *";
                e.Row.Font.Bold = true;
                e.Row.ForeColor = System.Drawing.Color.Red;
                //e.Row.Cells[0].BackColor = System.Drawing.Color.Red;
                e.Row.BackColor = System.Drawing.Color.LightCyan; 
                
            }
            //e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
        }

        protected void gvPage_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.CellValue.ToString() == "01")
            {
                e.Cell.Text = "AIS";
            }
            else if (e.CellValue.ToString() == "02")
            {
                e.Cell.Text = "Dtac";
            }
            else if (e.CellValue.ToString() == "03")
            {
                e.Cell.Text = "True";
            }
            else if (e.CellValue.ToString() == "04")
            {
                e.Cell.Text = "True H";
            }
            else if (e.CellValue.ToString() == "05")
            {
                e.Cell.Text = "3GX";
            }
            else if (e.CellValue.ToString() == "")
            {
                e.Cell.Text = "Other";
            }
        }

        protected void gvClick_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            string strDate = e.GetValue("DATE_REPORT").ToString();
            DateTime date = new DateTime(int.Parse(strDate.Substring(0, 4)), int.Parse(strDate.Substring(4, 2)), int.Parse(strDate.Substring(6, 2)));
            if (date.Date.ToString("dddd") == "Saturday" || date.Date.ToString("dddd") == "Sunday")
            {
                if (e.CellValue == strDate)
                {
                    e.Cell.Text = strDate + " *";
                    e.Cell.Font.Bold = true;
                    e.Cell.ForeColor = System.Drawing.Color.Red;
                    e.Cell.BackColor = System.Drawing.Color.LightCyan; 
                }
                e.Cell.ForeColor = System.Drawing.Color.Red;
            }
        }



    }
}
