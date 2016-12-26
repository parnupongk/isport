using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mobile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Data.OracleClient;
using MobileLibrary;
using WebLibrary;
namespace isport_payment
{
    public partial class pay_dtac_ok : mBasePage
    {
        private string urlRedirect;
        public override void SetHeader()
        {
            
        }
        public override void SetContent()
        {
            MobileUtilities mU = Utilities.getMISDN(Request);
            try
            {

                string fileName = "";
                // 5,10 บาทจะมาจาก service ใหม่
                string sdpId = Request["cprefid"].ToString();
                DataTable dtSdp = new AppCode_Dtac_Payment().PaymentDtacSDPSelect_Parameter(sdpId);
                string strContent = Request["cid"].ToString();//== "" ? Request["cprefid"].ToString() : Request["cid"].ToString() ; // 4511000010:S:L:142:0000:1:0:1
                //string strContent = Request["cprefid"].ToString();// ตัวใหม่ SDP จะไม่มี cid
                string strStatus  = Request["stts"].ToString();

                //LogsManager.WriteLogs("Test_SDP", "test", "");

                string strServiceId = (Request["svid"] == null || Request["svid"] == "") ? Request["pid"].Substring(0, 8) : Request["svid"];

                //string[] strContents = strContent.Split(':');
                if (dtSdp.Rows.Count > 0 )
                {

                    //bProperty_SIZE = strContents[1];
                    bProperty_SCSID = dtSdp.Rows[0]["PCNT_ID"].ToString(); // TEST 20130610
                    bProperty_LNG = dtSdp.Rows[0]["LNG"].ToString();
                    bProperty_SGID = dtSdp.Rows[0]["SG_ID"].ToString();
                    bProperty_MP = dtSdp.Rows[0]["MP_CODE"].ToString();
                    bProperty_PRJ = dtSdp.Rows[0]["PRJ_ID"].ToString();

                }

                urlRedirect = (bProperty_SGID == "255") ? ConfigurationManager.AppSettings["tdedloveRoot"] : ConfigurationManager.AppSettings["isportRoot"];

                #region Check Session or Subscribe
                string pTypeCode = (strServiceId == "45110531" || strServiceId== "45110530") ? AppCode.GetPTypeCode.pTypeCode_50Bath : AppCode.GetPTypeCode.pTypeCode_5Bath;
                #endregion

                int ss = CheckDup(mU.mobileNumber,mU.mobileOPT ,bProperty_SGID, pTypeCode, bProperty_SCSID);

                if ((strStatus == "707" || strStatus == "708") && ss == 0)
                {
                    DtacPayment_Parameter para = new AppCode_Dtac_Payment().SubAcceptAOC(Request["ssid"].ToString(), Request["tk"].ToString()
                            , Request["cprefid"].ToString(), Request["cmd"].ToString(), mU.mobileNumber
                            , pTypeCode, mU.mobileOPT);

                    strStatus = para.dtacStatus;
                    if (strStatus == "200" || strStatus == "707" || strStatus == "708")
                    {
                        #region Status 200
                        // Insert subscribe
                        int subscribeStatus = -2;
                        //if (strServiceId == "4511001020" || strServiceId == "4511001010") //test service app 4511000213 ,  5 bath ปกติ 4511001020
                        if (strServiceId == "45110037" || strServiceId == "45110036") //test service app 4511000213 ,  5 bath ปกติ 4511001020
                        {// Insert Subscribe Session
                            
                            subscribeStatus = AppCode.SubscribeInsert(mU.mobileNumber
                                , pTypeCode
                               , strServiceId, mU.mobileOPT, bProperty_SGID, bProperty_MP, bProperty_PRJ, strStatus, bProperty_SCSID);

                        }
                        //else if (strServiceId == "8451120500001" || strServiceId == "8451110500001") //(845110002100001 => 10 bath/week) (8451120500001 => 50B/Week) ( 8451110500001 => 50B/M)
                        else if (strServiceId == "45110531" || strServiceId == "45110530") //(845110002100001 => 10 bath/week) (8451120500001 => 50B/Week) ( 8451110500001 => 50B/M)
                        {// Insert Subscribe 

                            subscribeStatus = AppCode.SubscribeInsert(mU.mobileNumber, pTypeCode
                               , strServiceId, mU.mobileOPT, bProperty_SGID, bProperty_MP, bProperty_PRJ, strStatus, bProperty_SCSID);

                        }

                        //if ((bProperty_SGID == "95" || bProperty_SGID == "208" || bProperty_SGID == "255") && strServiceId == "4511001020") ////test service app 4511000210 ,  5 bath ปกติ 4511001020
                        if ((bProperty_SGID == "95" || bProperty_SGID == "208" || bProperty_SGID == "255") && strServiceId == "45110037") ////test service app 4511000210 ,  5 bath ปกติ 4511001020
                        {
                            // ถ้าเป็น subscribe จะ Insert Content Usage Logs
                            ContentUsageLogsInsert(int.Parse(bProperty_SGID), mU.mobileNumber, bProperty_SCSID);
                        }

                        // Write logs file subscript status 
                        fileName = (strServiceId == "45110531" || strServiceId == "45110530") ? "Dtac_AOC_InsertSubscriptStatus" : "Dtac_AOC_InsertSessionStatus";
                        LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), fileName, mU.mobileNumber + " status:" + subscribeStatus.ToString(), "");
                        // Send State to Crie
                        //AppCode.SendStatetoCrie(strServiceId, strContent, mU.mobileNumber, "1");


                        // Get URL Redirect
                        urlRedirect += AppCode.ServiceGroup_GetURLRedirect(bProperty_SGID, bProperty_SCSID) 
                            + "&lng=" + bProperty_LNG + "&size=" + bProperty_SIZE + "&mp=" + bProperty_MP + "&prj=" + bProperty_PRJ +"&sg="+ bProperty_SGID;
                        LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "Dtac_AOC_URLRedirect", mU.mobileNumber + " URL:"+urlRedirect, "");
                        Response.Redirect(urlRedirect , false);
                        #endregion
                    }
                    else
                    {
                        fileName = (strServiceId == "45110531" || strServiceId == "45110530") ? "Dtac_AOC_ConfirmSubscribe_Error" : "Dtac_AOC_ConfirmSession_Error";
                        LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), fileName, mU.mobileNumber + " Error status:" + strStatus + " ,serviceID=" + strServiceId, "");
                        lblMessage.Text = strStatus + ConfigurationManager.AppSettings["subScribeErrorMessage"].ToString();
                    }
                }
                //else if (strStatus == "708" || ss > 0)
                //{
                    //urlRedirect += AppCode.ServiceGroup_GetURLRedirect(bProperty_SGID, bProperty_SCSID)
                   //         + "&lng=" + bProperty_LNG + "&size=" + bProperty_SIZE + "&mp=" + bProperty_MP + "&prj=" + bProperty_PRJ + "&sg=" + bProperty_SGID;
                   // LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "Dtac_AOC_URLRedirect_Dup", mU.mobileNumber + " URL:" + urlRedirect, "");
                   // Response.Redirect(urlRedirect, false);
               // }
                else
                {
                    #region Other Status

                    if (strStatus == "665" || strStatus == "707" || strStatus == "708")
                    {
                        urlRedirect += AppCode.ServiceGroup_GetURLRedirect(bProperty_SGID, bProperty_SCSID)
                            + "&lng=" + bProperty_LNG + "&size=" + bProperty_SIZE + "&mp=" + bProperty_MP + "&prj=" + bProperty_PRJ + "&sg=" + bProperty_SGID;
                        LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "Dtac_AOC_URLRedirect_665", mU.mobileNumber + " URL:" + urlRedirect, "");
                        Response.Redirect(urlRedirect, false);
                    }
                    else
                    {
                        // subscribe & subscribesession ไม่สำเร็จ
                        // Redirect to index.aspx
                        fileName = (strServiceId == "45110531" || strServiceId == "45110530") ? "Dtac_AOC_Error_StatusSubscribe" : "Dtac_AOC_Error_StatusSession";
                        LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), fileName, mU.mobileNumber + " Error status:" + strStatus + " ,serviceID=" + strServiceId + " ,cId=" + strContent, "");
                        string config = (strStatus == "669") ? "subScribeErrorMessage669" : "subScribeErrorMessage";
                        lblMessage.Text = strStatus + ConfigurationManager.AppSettings[config].ToString();
                        return;
                    }
                    #endregion
                }
            }catch(Exception ex)
            {
                LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "Dtac_AOC_Error", mU.mobileNumber + "Error status:" + ex.Message, "");
                lblMessage.Text =  ConfigurationManager.AppSettings["subScribeErrorMessage"].ToString();
            }
            
        }
        public override void SetFooter()
        {
            Setfooter(pnlFooter);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private int   CheckDup(string msisdn, string opt,string sgId, string typeCode, string scsId)
        {
            try
            {

                //DataSet ds = null;
                //if (bProperty_SGID == "95" || bProperty_SGID == "208" || bProperty_SGID == "255")
                //{
                //    ds = OracleDataAccress.OrclHelper.Fill(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), CommandType.StoredProcedure, "WAP.CHK_DUPSUBSCRIBE_BYCNT", "package"
                //        , new OracleParameter[] { OracleDataAccress.OrclHelper.GetOracleParameter("p_phone", msisdn, OracleType.VarChar, ParameterDirection.Input) 
                //                                            , OracleDataAccress.OrclHelper.GetOracleParameter("p_sgId", sgId, OracleType.VarChar, ParameterDirection.Input) 
                //                                            , OracleDataAccress.OrclHelper.GetOracleParameter("p_typeCode", typeCode, OracleType.VarChar, ParameterDirection.Input) 
                //                                            , OracleDataAccress.OrclHelper.GetOracleParameter("p_scsId", scsId, OracleType.VarChar, ParameterDirection.Input) 
                //                                            , OracleDataAccress.OrclHelper.GetOracleParameter("o_Content", scsId, OracleType.Cursor, ParameterDirection.Output)});
                //}
                //else
                //{
                //    ds = OracleDataAccress.OrclHelper.Fill(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), CommandType.StoredProcedure, "WAP.CHK_DUPSUBSCRIBE", "package"
                //       , new OracleParameter[] { OracleDataAccress.OrclHelper.GetOracleParameter("p_phone", msisdn, OracleType.VarChar, ParameterDirection.Input) 
                //                                            , OracleDataAccress.OrclHelper.GetOracleParameter("p_sgId", sgId, OracleType.VarChar, ParameterDirection.Input) 
                //                                            , OracleDataAccress.OrclHelper.GetOracleParameter("p_typeCode", typeCode, OracleType.VarChar, ParameterDirection.Input) 
                //                                            , OracleDataAccress.OrclHelper.GetOracleParameter("o_Content", scsId, OracleType.Cursor, ParameterDirection.Output)});
                //}
                scsId = scsId == null || scsId == "" ? "0" : scsId;
                int ss = AppCode.CheckSessionPayment(msisdn, opt, sgId, scsId, typeCode);
                return ss;
                //LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "Dtac_AOC_CheckDup", msisdn + " ss:" + ss + " opt:" + opt + " sgId:" + sgId + " scsId:" + scsId + " typeCode:" + typeCode, "");
                //if (ss > 0)
                //{
                //    urlRedirect += AppCode.ServiceGroup_GetURLRedirect(bProperty_SGID, bProperty_SCSID)
                //            + "&lng=" + bProperty_LNG + "&size=" + bProperty_SIZE + "&mp=" + bProperty_MP + "&prj=" + bProperty_PRJ + "&sg=" + bProperty_SGID;

                //    LogsManager.WriteLogs(ConfigurationManager.ConnectionStrings["oracleConnectString"].ToString(), "Dtac_AOC_URLRedirect_Dup", msisdn + " URL:" + urlRedirect, "");
                //    Response.Redirect(urlRedirect, false);
                //}
                //else
                //{
                //     return process
                //    return;
                //}
            }
            catch(Exception ex)
            {
                throw new Exception("CheckDupSubc>> " + ex.Message);
            }

        }

        #region ContentUsageLogs
        private void ContentUsageLogsInsert(int strSG, string strPhone, string strScsId)
        {
            try
            {
                paymentDS1.I_CONTENT_USAGE_LOGRow dr = new paymentDS1().I_CONTENT_USAGE_LOG.NewI_CONTENT_USAGE_LOGRow();
                dr.SG_ID = strSG;
                dr.CCAT_ID = 9;
                dr.PHONE_NO = strPhone;
                dr.ACCESS_CHANNEL = "W";
                dr.CONTENT_ID = strScsId;
                dr.USAGE_TYPE = "V";
                dr.USAGE_DATE = DateTime.Now;
                dr.REF_ID = "";
                AppCode.ContentUsageLogsInsert(dr);
            }
            catch (Exception ex)
            {
                throw new Exception(" ContentUsageLogsInsert>> " + ex.Message);
            }
        }
        #endregion

    }
}