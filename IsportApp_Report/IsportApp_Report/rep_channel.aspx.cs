using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using WebLibrary;
using System.Collections;

namespace IsportApp_Report
{
    public partial class rep_channel : System.Web.UI.Page
    {
        string userId = "",partnerId="";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] cookie = MitTool.GetCookie(Request, "ADMIN") == "" ? null : MitTool.GetCookie(Request, "ADMIN").Split('&'); ;
                if ( cookie != null )
                {
                    partnerId = cookie[0].Split('=')[1];
                    userId = cookie[1].Split('=')[1];
                }
                //userId = "157";
                //partnerId = "46";
                BindChannel();
                Select_SipSubService();

                txtEndDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                txtServiceEndDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                txtStartDate.Text = DateTime.Now.AddDays(-7).ToString("MM/dd/yyyy");
                txtServiceStartDate.Text = DateTime.Now.AddDays(-7).ToString("MM/dd/yyyy");

            }
        }

        #region Report by mp code
        private decimal SumNewSubByMpCode(DataTable dt, string repDate, string optCode)
        {
            string optName = "";
            switch (optCode)
            {
                case "01": optName = "ais"; break;
                case "02": optName = "dtac"; break;
                case "04": optName = "truemoveH"; break;
            }
            var where = dt.AsEnumerable().Where(y => y.Field<string>("rep_date") == repDate);
            var sum = where.Sum(x => x.Field<int>(optName));

            return Convert.ToDecimal(sum);
        }
        private void BindChannel()
        {
            try
            {
                DataSet dsPartner = null;
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["IsportRepPackConnectionString"].ToString(), CommandType.StoredProcedure, "usp_rep_group_mpcode");
                var data = from partner in ds.Tables[0].AsEnumerable()
                           select partner;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (partnerId != "")
                    {

                        dsPartner = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["IsportRepPackConnectionString"].ToString(), CommandType.StoredProcedure
                            , "usp_partner_service", new SqlParameter[] { new SqlParameter("@pptn_id", partnerId) });

                        //Response.Write(dsPartner.Tables[0].Rows.Count);

                        if (dsPartner.Tables.Count > 0 && dsPartner.Tables[0].Rows.Count > 0)
                        {
                            string[] mpPartner = new string[dsPartner.Tables[0].Rows.Count];
                            for (int i = 0; i < dsPartner.Tables[0].Rows.Count; i++)
                            {
                                mpPartner[i] = dsPartner.Tables[0].Rows[i][0].ToString();
                                //Response.Write(dsPartner.Tables[0].Rows[i][0].ToString());
                            }
                            ViewState["DataPartner"] = mpPartner;
                            data = from partner in ds.Tables[0].AsEnumerable()
                                   where mpPartner.Contains(partner.Field<string>("mp_code"))
                                   select partner;
                        }

                    }

                    ddlMpCode.DataSource = data.AsDataView();
                    ddlMpCode.DataTextField = "ivr_number";
                    ddlMpCode.DataValueField = "mp_code";
                    ddlMpCode.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertmsg", "alert('ขออภัยไม่พบข้อมูลที่ต้องการ');", true);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("BindChannel >> " + ex.Message);
            }
        }

        protected void btnSubmit_Click1(object sender, EventArgs e)
        {
            try
            {
                DataView dv = null;
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["IsportRepPackConnectionString"].ToString(), CommandType.StoredProcedure, "usp_rep_sub_from_mpcode"
                                                    , new SqlParameter[] {new SqlParameter("@mp_code",ddlMpCode.SelectedValue)
                                                ,new SqlParameter("@start_date",DateTime.Parse( txtStartDate.Text).ToString("yyyyMMdd") )
                                                ,new SqlParameter("@end_date",DateTime.Parse(txtEndDate.Text).ToString("yyyyMMdd") ) });

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    dv = dt.DefaultView;
                    if( ddlMpCodeService.SelectedItem.Value != "All" )
                    {
                        dv.RowFilter = new System.Text.StringBuilder(string.Empty).Append("pssv_id=" + ddlMpCodeService.SelectedValue + "").ToString();
                    }



                    var query = from REP_SMS_NEW in dv.ToTable().AsEnumerable()
                                orderby REP_SMS_NEW[0]
                                group REP_SMS_NEW by REP_SMS_NEW[0] into newGroup
                                select newGroup;

                    //AjaxControlToolkit.LineChartSeries ais = new AjaxControlToolkit.LineChartSeries();

                    string[] x = new string[query.Count()];
                    decimal[] ais = new decimal[query.Count()];
                    decimal[] dtac = new decimal[query.Count()];
                    decimal[] trueH = new decimal[query.Count()];
                    decimal[] sum = new decimal[query.Count()];

                    int index = 0;
                    foreach (var repdate in query)
                    {

                        x[index] = repdate.Key.ToString();
                        ais[index] = SumNewSubByMpCode(dt, repdate.Key.ToString(), "01");
                        dtac[index] = SumNewSubByMpCode(dt, repdate.Key.ToString(), "02");
                        trueH[index] = SumNewSubByMpCode(dt, repdate.Key.ToString(), "04");
                        sum[index] = ais[index] + dtac[index] + trueH[index];
                        index++;
                    }

                    LineChart1.CategoriesAxis = string.Join(",", x);
                    LineChart1.Series[0].Data = ais;
                    LineChart1.Series[1].Data = dtac;
                    LineChart1.Series[2].Data = trueH;
                    LineChart1.Series[3].Data = sum;
                }

                else
                {

                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertmsg", "alert('ขออภัยไม่พบข้อมูลที่ต้องการ');", true);
                    gvData.Visible = false;
                    gvServiceNo.Visible = false;
                    return;

                }



                gvData.Visible = true;
                gvServiceNo.Visible = false;
                gvData.DataSource = dv;
                gvData.DataBind();

                GroupGridView(gvData.Rows, 0, 1);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("btnSubmit_Click1 >> " + ex.Message);
            }
        }

        int totalAis = 0, totalDtac = 0, total3Gx = 0, totalTrue = 0;
        int totalCancelAis = 0, totalCancelDtac = 0, totalCancel3Gx = 0, totalCancelTrue = 0;
        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                totalAis += int.Parse(((Label)e.Row.FindControl("lblAis")).Text);
                totalDtac += int.Parse(((Label)e.Row.FindControl("lblDtac")).Text);
                totalTrue += int.Parse(((Label)e.Row.FindControl("lblTrue")).Text);
                total3Gx += int.Parse(((Label)e.Row.FindControl("lbl3Gx")).Text);

                totalCancelAis += int.Parse(((Label)e.Row.FindControl("lblCancelAis")).Text);
                totalCancelDtac += int.Parse(((Label)e.Row.FindControl("lblCancelDtac")).Text);
                totalCancelTrue += int.Parse(((Label)e.Row.FindControl("lblCancelTrueH")).Text);
                totalCancel3Gx += int.Parse(((Label)e.Row.FindControl("lblCancel3Gx")).Text);

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((Label)e.Row.FindControl("lblSumAis")).Text = totalAis.ToString();
                ((Label)e.Row.FindControl("lblSumDtac")).Text = totalDtac.ToString();
                ((Label)e.Row.FindControl("lblSumTrue")).Text = totalTrue.ToString();
                ((Label)e.Row.FindControl("lblSum3Gx")).Text = total3Gx.ToString();

                ((Label)e.Row.FindControl("lblSumCancelAis")).Text = totalCancelAis.ToString();
                ((Label)e.Row.FindControl("lblSumCancelDtac")).Text = totalCancelDtac.ToString();
                ((Label)e.Row.FindControl("lblSumCancelTrueH")).Text = totalCancelTrue.ToString();
                ((Label)e.Row.FindControl("lblSumCancel3Gx")).Text = totalCancel3Gx.ToString();
            }
        }

        #endregion

        #region Report by Service
        private void Select_SipSubService()
        {
            try
            {
                string pcatId = string.IsNullOrEmpty(ConfigurationManager.AppSettings["PartnerService_" + userId]) ? "" : ConfigurationManager.AppSettings["PartnerService_" + userId];
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["IsportRepPackConnectionString"].ToString(), CommandType.StoredProcedure, "usp_isport_getsipsubservice"
                                                                        , new SqlParameter[] { new SqlParameter("@pcat_id", pcatId) });

                ddlSipService.DataSource = ds;
                ddlSipService.DataTextField = "pssv_desc";
                ddlSipService.DataValueField = "pssv_id";
                ddlSipService.DataBind();

                ddlMpCodeService.DataSource = ds;
                ddlMpCodeService.DataTextField = "pssv_desc";
                ddlMpCodeService.DataValueField = "pssv_id";
                ddlMpCodeService.DataBind();
                ddlMpCodeService.ClearSelection();
                ddlMpCodeService.Items.Insert(0, new ListItem("All Service", "All"));

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Select_SipSubService >> " + ex.Message);
            }
        }
        private decimal SumNewSubByOpt(DataView dt, string repDate, string optCode)
        {
            string sum = dt.ToTable().AsEnumerable()
                            .Where(y => y.Field<string>("rep_date") == repDate && y.Field<string>("opt_code") == optCode)
                            .Sum(x => x.Field<int>("new_sub"))
                            .ToString();

            return Convert.ToDecimal(sum);
        }
        private void BindReportMpCodeFromService()
        {
            try
            {

                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["IsportRepPackConnectionString"].ToString(), CommandType.StoredProcedure, "usp_get_newsub_by_mpcode"
                                                , new SqlParameter[] {new SqlParameter("@ppssv_id",ddlSipService.SelectedValue)
                                                ,new SqlParameter("@pstart_date",DateTime.Parse( txtServiceStartDate.Text).ToString("yyyyMMdd") )
                                                ,new SqlParameter("@pend_date",DateTime.Parse(txtServiceEndDate.Text).ToString("yyyyMMdd") ) });

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    string[] mpPartner = (string[])ViewState["DataPartner"];

                    var data = from REP_SMS_NEW in ds.Tables[0].AsEnumerable()
                               select REP_SMS_NEW;

                    var query = from REP_SMS_NEW in ds.Tables[0].AsEnumerable()
                                orderby REP_SMS_NEW[0]
                                group REP_SMS_NEW by REP_SMS_NEW[0] into newGroup
                                select newGroup;

                    if (ViewState["DataPartner"] != null)
                    {
                        data = from REP_SMS_NEW in ds.Tables[0].AsEnumerable()
                               where mpPartner.Contains(REP_SMS_NEW.Field<string>("mp_code"))
                               select REP_SMS_NEW;

                        query = from REP_SMS_NEW in ds.Tables[0].AsEnumerable()
                                where mpPartner.Contains(REP_SMS_NEW.Field<string>("mp_code"))
                                orderby REP_SMS_NEW[0]
                                group REP_SMS_NEW by REP_SMS_NEW[0] into newGroup
                                select newGroup;

                        if (data.Count<DataRow>() == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertmsg", "alert('ขออภัยไม่พบข้อมูลที่ต้องการ');", true);
                            gvData.Visible = false;
                            gvServiceNo.Visible = false;
                            return;
                        }

                    }

                    string[] x = new string[query.Count()];
                    decimal[] ais = new decimal[query.Count()];
                    decimal[] dtac = new decimal[query.Count()];
                    decimal[] trueH = new decimal[query.Count()];
                    decimal[] sum = new decimal[query.Count()];
                    int index = 0;
                    foreach (var repdate in query)
                    {

                        x[index] = repdate.Key.ToString();
                        ais[index] = SumNewSubByOpt(data.AsDataView(), repdate.Key.ToString(), "01");
                        dtac[index] = SumNewSubByOpt(data.AsDataView(), repdate.Key.ToString(), "02");
                        trueH[index] = SumNewSubByOpt(data.AsDataView(), repdate.Key.ToString(), "04");
                        sum[index] = ais[index] + dtac[index] + trueH[index];
                        index++;
                    }

                    LineChart1.CategoriesAxis = string.Join(",", x);
                    LineChart1.Series[0].Data = ais;
                    LineChart1.Series[1].Data = dtac;
                    LineChart1.Series[2].Data = trueH;
                    LineChart1.Series[3].Data = sum;

                    gvData.Visible = false;
                    gvServiceNo.Visible = true;

                    gvServiceNo.DataSource = data.AsDataView();
                    gvServiceNo.DataBind();
                    GroupGridView(gvServiceNo.Rows, 0, 3);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertmsg", "alert('ขออภัยไม่พบข้อมูลที่ต้องการ');", true);
                }


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("BindReportMpCodeFromService >> " + ex.Message);
            }
        }

        protected void btnServiceSubmit_Click(object sender, EventArgs e)
        {
            BindReportMpCodeFromService();
        }

        int totalServiceNo = 0;
        int totalCancelServiceNo = 0;
        protected void gvServiceNo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string rtn = "";
                switch (e.Row.Cells[2].Text)
                {
                    case "01": rtn = "AIS"; break;
                    case "02": rtn = "DTAC"; break;
                    case "04": rtn = "TrueH"; break;
                }
                e.Row.Cells[2].Text = rtn;
                Label lbl = (Label)e.Row.FindControl("lblSubNo");
                Label lblCancel = (Label)e.Row.FindControl("lblCancelSubNo");
                string s = lbl.Text == "" ? "0" : lbl.Text;
                totalServiceNo += int.Parse(s);
                totalCancelServiceNo += int.Parse(lblCancel.Text);

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblSum = (Label)e.Row.FindControl("lblSumServiceNo");
                lblSum.Text = "Sum = " + totalServiceNo.ToString();

                ((Label)e.Row.FindControl("lblSumCancelServiceNo")).Text = totalCancelServiceNo.ToString();

            }
        }
        #endregion

        #region GroupGirdView
        void GroupGridView(GridViewRowCollection gvrc, int startIndex, int total)
        {
            if (total == 0) return;
            int i, count = 1;
            ArrayList lst = new ArrayList();
            lst.Add(gvrc[0]);
            var ctrl = gvrc[0].Cells[startIndex];
            for (i = 1; i < gvrc.Count; i++)
            {
                TableCell nextCell = gvrc[i].Cells[startIndex];
                if (ctrl.Text == nextCell.Text)
                {
                    count++;
                    nextCell.Visible = false;
                    lst.Add(gvrc[i]);
                }
                else
                {
                    if (count > 1)
                    {
                        ctrl.RowSpan = count;
                        GroupGridView(new GridViewRowCollection(lst), startIndex + 1, total - 1);
                    }
                    count = 1;
                    lst.Clear();
                    ctrl = gvrc[i].Cells[startIndex];
                    lst.Add(gvrc[i]);
                }
            }
            if (count > 1)
            {
                ctrl.RowSpan = count;
                GroupGridView(new GridViewRowCollection(lst), startIndex + 1, total - 1);
            }
            count = 1;
            lst.Clear();
        }
        #endregion



    }
}
