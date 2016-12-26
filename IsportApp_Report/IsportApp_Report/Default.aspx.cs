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
namespace IsportApp_Report
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsCallback)
            {
                GridDataBind();
            }
        }
        private void GridDataBind()
        {
            string startDate = dateStart.Date.ToString("yyyMMdd");
            string endDate = dateEnd.Date.ToString("yyyyMMdd");
            DataSet ds = null , dsPageView = null;
            if (ViewState["SportApp_GetReport"] != null)
            {
                ds = (DataSet)ViewState["SportApp_GetReport"];
            }
            else
            {
                ds = OrclHelper.Fill(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString()
                    , CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetReport", "isportapp_report"
                    , new OracleParameter[] { OrclHelper.GetOracleParameter("p_startdate", startDate, OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("p_enddate", endDate, OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("p_app_name", cmbAppName.SelectedItem.Value.ToString(), OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)});
            }

            if (ViewState["SportApp_GetReportPageView"] != null)
            {
                dsPageView = (DataSet)ViewState["SportApp_GetReportPageView"];
            }
            else
            {
                dsPageView = OrclHelper.Fill(ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString()
                   , CommandType.StoredProcedure, "ISPORT_APP.SportApp_GetReportPageView", "isportapp_report"
                   , new OracleParameter[] { OrclHelper.GetOracleParameter("p_startdate", startDate, OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("p_enddate", endDate, OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("p_app_name", cmbAppName.SelectedItem.Value.ToString(), OracleType.VarChar, ParameterDirection.Input)
                                                        ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)});
            }

            System.Web.UI.DataVisualization.Charting.Chart cc = (System.Web.UI.DataVisualization.Charting.Chart)aspBar.Groups[0].FindControl("chart");
            
            DevExpress.Web.ASPxGridView.ASPxGridView gvReport = (DevExpress.Web.ASPxGridView.ASPxGridView)aspBar.Groups[1].FindControl("gvReport");
            

            ViewState["Report"] = ds;
            
            gvReport.DataSource = ds;
            gvReport.DataBind();

            gvNoti.DataSource = ds;
            gvNoti.DataBind();
            
            
            // Set chart data source
            cc.DataSource = ds;

            // Set series members names for the X and Y values
            cc.Series["ssAndroid"].XValueMember = "DATE_REPORT";
            cc.Series["ssAndroid"].YValueMembers = "ANDROID_ACTIVE";

            cc.Series["ssAndroidTab"].XValueMember = "DATE_REPORT";
            cc.Series["ssAndroidTab"].YValueMembers = "ANDROIDTABLET_ACTIVE";

            cc.Series["ssBB"].XValueMember = "DATE_REPORT";
            cc.Series["ssBB"].YValueMembers = "BB_ACTIVE";

            cc.Series["ssiPhone"].XValueMember = "DATE_REPORT";
            cc.Series["ssiPhone"].YValueMembers = "IPHONE_ACTIVE";

            cc.Series["ssiPad"].XValueMember = "DATE_REPORT";
            cc.Series["ssiPad"].YValueMembers = "IPAD_ACTIVE";

            cc.Series["ssJava"].XValueMember = "DATE_REPORT";
            cc.Series["ssJava"].YValueMembers = "JAVA_ACTIVE";




            // Data bind to the selected data source
            cc.DataBind();

            

            BindChartUnique();
            BindChartView();

            ViewState["ViewPageReport"] = dsPageView;
            gvViewPage.DataSource = dsPageView;
            gvViewPage.DataBind();


           
        }
        private void BindChartView()
        {
            System.Web.UI.DataVisualization.Charting.Chart cView = (System.Web.UI.DataVisualization.Charting.Chart)barView.Groups[0].FindControl("chartView");
            DevExpress.Web.ASPxGridView.ASPxGridView gvView = (DevExpress.Web.ASPxGridView.ASPxGridView)barView.Groups[1].FindControl("gvView");

            DataSet ds = (DataSet)ViewState["Report"];

            cView.DataSource = ds;
            cView.Series["ssAndroid"].XValueMember = "DATE_REPORT";
            cView.Series["ssAndroid"].YValueMembers = "ANDROID_View";

            cView.Series["ssAndroidTab"].XValueMember = "DATE_REPORT";
            cView.Series["ssAndroidTab"].YValueMembers = "ANDROIDTABLET_View";

            cView.Series["ssBB"].XValueMember = "DATE_REPORT";
            cView.Series["ssBB"].YValueMembers = "BB_View";

            cView.Series["ssiPhone"].XValueMember = "DATE_REPORT";
            cView.Series["ssiPhone"].YValueMembers = "IPHONE_View";

            cView.Series["ssiPad"].XValueMember = "DATE_REPORT";
            cView.Series["ssiPad"].YValueMembers = "IPAD_View";

            cView.Series["ssJava"].XValueMember = "DATE_REPORT";
            cView.Series["ssJava"].YValueMembers = "JAVA_View";

            cView.DataBind();

            gvView.DataSource = ds;
            gvView.DataBind();
        }
        private void BindChartUnique()
        {
            System.Web.UI.DataVisualization.Charting.Chart cUnique = (System.Web.UI.DataVisualization.Charting.Chart)barUnique.Groups[0].FindControl("chartUnique");
            DevExpress.Web.ASPxGridView.ASPxGridView gvUnique = (DevExpress.Web.ASPxGridView.ASPxGridView)barUnique.Groups[1].FindControl("gvUnique");

            DataSet ds = (DataSet)ViewState["Report"];
           
            cUnique.DataSource = ds;
            cUnique.Series["ssAndroid"].XValueMember = "DATE_REPORT";
            cUnique.Series["ssAndroid"].YValueMembers = "ANDROID_UNIQUE";

            cUnique.Series["ssAndroidTab"].XValueMember = "DATE_REPORT";
            cUnique.Series["ssAndroidTab"].YValueMembers = "ANDROIDTABLET_UNIQUE";

            cUnique.Series["ssBB"].XValueMember = "DATE_REPORT";
            cUnique.Series["ssBB"].YValueMembers = "BB_UNIQUE";

            cUnique.Series["ssiPhone"].XValueMember = "DATE_REPORT";
            cUnique.Series["ssiPhone"].YValueMembers = "IPHONE_UNIQUE";

            cUnique.Series["ssiPad"].XValueMember = "DATE_REPORT";
            cUnique.Series["ssiPad"].YValueMembers = "IPAD_UNIQUE";

            cUnique.Series["ssJava"].XValueMember = "DATE_REPORT";
            cUnique.Series["ssJava"].YValueMembers = "JAVA_UNIQUE";

            cUnique.DataBind();

            gvUnique.DataSource = ds;
            gvUnique.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            GridDataBind();
        }

        protected void gvReport_PageIndexChanged(object sender, EventArgs e)
        {
            //gvReport.PageIndex = 
            GridDataBind();
        }

        protected void gvUnique_PageIndexChanged(object sender, EventArgs e)
        {
            GridDataBind();
        }

        protected void gvView_PageIndexChanged(object sender, EventArgs e)
        {
            GridDataBind();
        }

        protected void btnReportExport_Click(object sender, ImageClickEventArgs e)
        {
            DevExpress.Web.ASPxGridView.ASPxGridView gvReport = (DevExpress.Web.ASPxGridView.ASPxGridView)aspBar.Groups[1].FindControl("gvReport");
            gvReport.DataSource = (DataSet)ViewState["Report"];
            gvReport.DataBind();

            gvReportExport.WriteXlsToResponse("AisSportArena_Active_" + DateTime.Now.ToString("ddMMyyyy") + ".xls");
        }

        protected void btnUniqueExport_Click(object sender, ImageClickEventArgs e)
        {
            DevExpress.Web.ASPxGridView.ASPxGridView gvUnique = (DevExpress.Web.ASPxGridView.ASPxGridView)barUnique.Groups[1].FindControl("gvUnique");
            gvUnique.DataSource = (DataSet)ViewState["Report"];
            gvUnique.DataBind();

            gvUniqueExport.WriteXlsToResponse("AisSportArena_Unique_"+DateTime.Now.ToString("ddMMyyyy")+".xls");
        }

        protected void btnViewExport_Click(object sender, ImageClickEventArgs e)
        {
            DevExpress.Web.ASPxGridView.ASPxGridView gvView = (DevExpress.Web.ASPxGridView.ASPxGridView)barView.Groups[1].FindControl("gvView");
            gvView.DataSource = (DataSet)ViewState["Report"];
            gvView.DataBind();

            gvViewExport.WriteXlsToResponse("AisSportArena_View_" + DateTime.Now.ToString("ddMMyyyy") + ".xls");
        }

        protected void btnViewPage_Click(object sender, ImageClickEventArgs e)
        {
            gvViewPage.DataSource = (DataSet)ViewState["ViewPageReport"];
            gvViewPage.DataBind();

            gvViewPageExport.WriteXlsToResponse("AisSportArena_ViewPage_" + DateTime.Now.ToString("ddMMyyyy") + ".xls");
        }

        protected void gvNoti_PageIndexChanged(object sender, EventArgs e)
        {
            GridDataBind();
        }

        protected void btnNotiExport_Click(object sender, ImageClickEventArgs e)
        {
            gvNoti.DataSource = (DataSet)ViewState["ViewPageReport"];
            gvNoti.DataBind();

            gvNotiExport.WriteXlsToResponse("AisSportArena_ActiveNotification_" + DateTime.Now.ToString("ddMMyyyy") + ".xls");
        }
    }
}
