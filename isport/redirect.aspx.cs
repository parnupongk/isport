using System;
using System.Web.UI;
using MobileLibrary;
using WebLibrary;
namespace isport
{
    public partial class redirect : PageBase
    {

        public override void GenFooter(string optCode, string projectType)
        {
            
        }

        public override void Subscribe_portal_log(string msisdn, string userAgent, string sgId, string optCode, string prjId, string mpCode, string scsId)
        {

            isport_service.ServiceWap_Logs.Subscribe_portal_log(AppMain.strConnOracle, mU.mobileNumber, bProperty_USERAGENT
                            , bProperty_SGID, mU.mobileOPT, bProperty_PRJ, bProperty_MPCODE, "");

        }

        public override void GenHeader(string optCode, string projectType)
        {
            try
            {
                // Set Rredirect
                if (Request["r"] == null || Request["r"] == "")
                {
                    Response.Redirect("http://wap.isport.co.th", false);
                }
                else
                {
                    string re = Request.QueryString.ToString();
                    MobileUtilities mU = Utilities.getMISDN(Request);
                    try
                    {
                        if (mU.mobileOPT.Length > 2 || mU.mobileOPT == null || mU.mobileOPT == "") mU.mobileOPT = "99";
                        
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.WriteError("Insert Portal Logs Error >> " + ex.Message);
                    }

                    if (Request["r"].ToString().IndexOf("wap.isport.co.th") > -1)
                    {
                        string url = Request["r"].ToString().Replace('|', '&');//+ "&lng=" + bProperty_Mobile.bProperty_LNG + "&mp_code=" + bProperty_Mobile.bProperty_MPCODE
                                                                               //+ "&size=" + bProperty_Mobile.bProperty_SIZE + "&prj=" + bProperty_Mobile.bProperty_PRJ
                                                                               //+ "&sg=" + bProperty_Mobile.bProperty_SGID;//+ "&scs_id=" + bProperty_Mobile.bProperty_SCSID;

                        url += Request["r"].IndexOf("lng") > 0 ? "" : "&lng=" + bProperty_LNG;
                        url += Request["r"].IndexOf("mp_code") > 0 ? "&mp_code=" + bProperty_MPCODE : "&mp_code=" + bProperty_MPCODE;
                        url += Request["r"].IndexOf("sg") > 0 ? "&sg=" + bProperty_SGID : "&sg=" + bProperty_SGID;
                        url += Request["r"].IndexOf("prj") > 0 ? "" : "&prj=" + bProperty_PRJ;
                        url += Request["r"].IndexOf("size") > 0 ? "" : "&size=" + bProperty_SIZE;
                        url += Request["r"].IndexOf("mp") > 0 ? "" : "&mp=" + bProperty_MP;
                        url += Request["r"].IndexOf("class_id") > 0 ? "" : "&class_id=" + Request["class_id"];
                        url += Request["cat_id"] != null ? "&cat_id=" + Request["cat_id"].ToString() : "";


                        Response.Redirect(url, false);
                    }
                    else
                    {
                        Response.Redirect(Request["r"].ToString().Replace('|', '&'), false);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("Redirect Error >> " + ex.Message);
                Response.Redirect("http://wap.isport.co.th", false);
            }
        }
        public override void PreGenContent(string optCode, string projectType)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}