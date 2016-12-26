using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MobileLibrary;
using WebLibrary;
namespace isport
{
    public partial class indexh : System.Web.UI.Page
    {
        //====================================================================================
       //
       //  ************************ ให้ใช่ index.aspx กับ indexl.aspx ก่อน เพราะ indexh.aspx มือถือบางรุ่นเปิดไม่ได้ ****************
       //
       //=====================================================================================

        #region Parameter
        private string imgControl = "";
        #endregion

        #region Property
        private string bPropertySGID;
        public string bProperty_SGID
        {
            get { return bPropertySGID == null ? "" : bPropertySGID; }
            set { bPropertySGID = value; }
        }
        private string bPropertyLNG;
        public string bProperty_LNG
        {
            get { return bPropertyLNG == null ? "L" : bPropertyLNG; }
            set { bPropertyLNG = value; }
        }
        private string bPropertySCSID;
        public string bProperty_SCSID
        {
            get { return bPropertySCSID == null ? "" : bPropertySCSID; }
            set { bPropertySCSID = value; }
        }
        public string bPropertySIZE;
        public string bProperty_SIZE
        {
            get { return bPropertySIZE == null ? "N" : bPropertySIZE; }
            set { bPropertySIZE = value; }
        }
        private string bPropertyMP;
        public string bProperty_MP
        {
            get { return bPropertyMP == null ? "0025" : bPropertyMP; }
            set { bPropertyMP = value; }
        }
        private string bPropertyMPCODE;
        public string bProperty_MPCODE
        {
            get { return bPropertyMPCODE == null ? "0025" : bPropertyMPCODE; }
            set { bPropertyMPCODE = value; }
        }

        private string bPropertyPRJ;
        public string bProperty_PRJ
        {
            get { return bPropertyPRJ == null ? "1" : bPropertyPRJ; }
            set { bPropertyPRJ = value; }
        }
        private string bPropertyUSERAGENT;
        public string bProperty_USERAGENT
        {
            get { return bPropertyUSERAGENT == null ? "" : bPropertyUSERAGENT; }
            set { bPropertyUSERAGENT = value; }
        }
        public string MasterID
        {
            get
            {
                return Request["mid"] == null ? "0" : Request["mid"].ToString();
            }
        }
        public int Level
        {
            get
            {
                return Request["Level"] == null ? 0 : int.Parse(Request["Level"].ToString());
            }
        }

        public string OperatorType
        {
            get
            {
                return ViewState["OperatorType"] == null || ViewState["OperatorType"] == "" ? AppCode.OperatorType.All.ToString() : ViewState["OperatorType"].ToString();
            }
        }
        public string ProjectType
        {
            get
            {
                return Request["p"] == null ? AppCode.ProjectType.bb.ToString() : Request["p"];
            }
        }
        #endregion

        #region Check Parameter
        private void CheckParameter()
        {
            //if (Request["lng"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["lng"], @"^[a-zA-Z'.\s]{1,40}$"))
            //{
               // bProperty_LNG = "L";
            //}
            //else
            //{
            bProperty_LNG = Request["lng"] == null ? "L" : Request["lng"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
            //}

            //if (Request["sg"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["sg"], @"^[a-zA-Z'.\s]$"))
            //{
            bProperty_SGID = Request["sg"] == null? "" : Request["sg"];
           // }
           // else
           // {
               // bProperty_SGID = Request["sg"];
            //}

            if (Request["scs_id"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["scs_id"], @"^[a-zA-Z'.\s]$"))
            {
                bProperty_SCSID = Request["scs_id"].Replace(@"^[a-zA-Z'.\s]$","");
            }
            else
            {
                bProperty_SCSID = Request["scs_id"] != null && Request.QueryString["scs_id"].Length > 1 ? Request.QueryString.GetValues("scs_id")[0] : Request["scs_id"];//Request["scs_id"];
            }

            //if (Request["size"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["size"], @"^[a-zA-Z'.\s]{1,40}$"))
            //{
                //bProperty_SIZE = "";
            //}
            //else
            //{
            bProperty_SIZE = Request["size"] == null ? "" :Request["size"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
            //}

           // if (Request["mp"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["mp"], @"^[a-zA-Z'.\s]$"))
           // {
               // bProperty_MP = "";
            //}
           // else
            //{
                bProperty_MP = Request["mp"] == null ? "0025" : Request["mp"].Replace(@"^[a-zA-Z'.\s]$","");
            //}

            //if (Request["mp_code"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["mp_code"], @"^[a-zA-Z'.\s]{1,40}$"))
            //{
                //bProperty_MPCODE = "";
            //}
            //else
            //{
                bProperty_MPCODE = Request["mp_code"] == null ? "0025" : Request["mp_code"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
            //}

            //if (Request["prj"] != null && !System.Text.RegularExpressions.Regex.IsMatch(Request.QueryString["prj"], @"^[a-zA-Z'.\s]{1,40}$"))
            //{
               // bProperty_PRJ = "";
           // }
           // else
            //{
                bProperty_PRJ = Request["prj"] == null ? "1" : Request["prj"].Replace(@"^[a-zA-Z'.\s]$", ""); ;
            //}
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx?" + Request.QueryString);
            if (!IsPostBack)
            {
                try
                {
                    MobileUtilities mU = Utilities.getMISDN(Request);
                    ViewState["OperatorType"] = mU.mobileOPT;
                    string vStrName = Request.ServerVariables["HTTP_USER_AGENT"];
                    CheckParameter();
                    //GenHeader();
                    
                    GenContent(mU.mobileOPT,Request["p"]);
                    GenFooter();

                    new AppCodeOracle().Subscribe_portal_log( mU.mobileNumber, bProperty_USERAGENT, bProperty_SGID
                         , mU.mobileOPT, bProperty_PRJ, bProperty_MPCODE, bProperty_SCSID);
                }
                catch (Exception ex)
                {
                    ExceptionManager.WriteError("Error >>" + ex.Message);
                }
            }
        }

        #region Content
        private void GenContent(string opt, string projectType)
        {

            string link = "redirect.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
                    + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                     + "&scs_id=" + bProperty_SCSID;
            link += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";

            string pageMaster = "indexh.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
        + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
        + "&sg=" + bProperty_SGID + "&scs_id=" + bProperty_SCSID;
            link += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";

            frmMain.Controls.Add(new isport_service.ServiceWapUI_Content().GenContent(AppMain.strConn
                , AppMain.strConnOracle
                , "0", "0", opt, projectType, link, pageMaster,Request["class_id"]));


            /*DataSet ds = new AppCode().SelectUIByLevel_Wap(MasterID, Level, OperatorType, ProjectType);
            
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                

                if (dr["content_link"] != null && dr["content_link"].ToString() != "")
                {
                   frmMain.Controls.AddAt(frmMain.Controls.Count, genLink(dr, link));
                }
                else
                {
                    
                    if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                    {
                        genImages(dr);
                        
                    }
                    else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                    {
                        genText(dr);
                    }
                }

                genNews(dr);
            }*/


        }
        #endregion

        #region Header
        private void GenHeader()
        {
            string projectType = Request["p1"] == null ? (Request["p"] == null ? "" : Request["p"]) : Request["p1"];
            DataSet ds = new AppCode().SelectHeaderByOperator(OperatorType, projectType);
            List<DataRow> drs = new List<DataRow>();
            bool isRandom = false;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["header_random"].ToString() == "start" || isRandom)
                {
                    isRandom = true;
                    if (dr["header_operator"].ToString() == "All" || OperatorType == dr["header_operator"].ToString()) drs.Add(dr);

                }
                if (!isRandom)
                {
                    if (dr["content_link"] != null && dr["content_link"].ToString() != "")
                    {
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row'><img class='img-full' src='" + dr["content_image"].ToString().Replace("~/", "") + "'></div>"));
                    }
                    else
                    {
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row'><a href='" + dr["content_link"].ToString() + "'><img class='img-full' src='" + dr["content_image"].ToString().Replace("~/", "") + "'></a></div>"));
                    }
                }
                if (dr["header_random"].ToString() == "end")
                {
                    isRandom = false;
                    // GenContent(drs[new Random().Next(0, drs.Count)], bProperty_LNG, bProperty_MP
                    //, bProperty_SIZE, bProperty_PRJ
                    //, bProperty_SGID, bProperty_SCSID);
                    if (dr["content_link"] != null && dr["content_link"].ToString() != "")
                    {
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row'><img class='img-full' src='" + drs[new Random().Next(0, drs.Count)]["content_image"].ToString().Replace("~/", "") + "'></div>"));
                    }
                    else
                    {
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row'><a href='" + drs[new Random().Next(0, drs.Count)]["content_link"].ToString() + "'><img class='img-full' src='" + drs[new Random().Next(0, drs.Count)]["content_image"].ToString().Replace("~/", "") + "'></a></div>"));
                    }
                }

            }
        }
        #endregion

        #region Footer
        private void GenFooter()
        {
            try
            {
                DataSet ds = new AppCode().SelectFooterByoperator(OperatorType, ProjectType);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string link = "redirect.aspx" + "?lng=L" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
                                    + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                    + "&sg=" + dr["footer_sg_id"].ToString() + "&scs_id=" + bProperty_SCSID
                                    + "&r=" + dr["content_link"].ToString().Replace('&', '|');
                    link += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";

                    if (dr["content_link"] != null && dr["content_link"].ToString() != "")
                    {
                        
                        lblFooter.Controls.AddAt(lblFooter.Controls.Count,genLink(dr, link));
                    }
                    else
                    {

                        if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                        {
                            genImages(dr,false);

                        }
                        else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                        {
                            genText(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GenFooter >>" + ex.Message);
            }
        }
        #endregion

        #region Gen Content
        private void genText(DataRow dr)
        {
            try
            {
                if (dr["content_icon"] != null && dr["content_icon"].ToString() != "")
                {
                    if ((bool)dr["ui_ismaster"])
                    {
                        string urlMaster = "indexh.aspx?mid=" + dr["ui_id"].ToString() + "&level=" + (Level + 1) + "&p=" + ProjectType;
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowheader'><a href=" + urlMaster + "><p><img class='img' src='" + dr["content_icon"].ToString().Replace("~/", "") + "'>" + dr["content_text"].ToString() + "</p></a></div>"));
                    }
                    else
                    {
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row'><p><img class='img' src='" + dr["content_icon"].ToString().Replace("~/", "") + "'>" + dr["content_text"].ToString() + "</p></div>"));
                    }
                }
                else
                {
                    if ((bool)dr["ui_ismaster"])
                    {
                        string urlMaster = "indexh.aspx?mid=" + dr["ui_id"].ToString() + "&level=" + (Level + 1) + "&p=" + ProjectType;
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowheader'><a href=" + urlMaster + "><p>" + dr["content_text"].ToString() + "</p></a></div>"));
                    }
                    else
                    {
                        frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row'><p>" + dr["content_text"].ToString() + "</p></div>"));
                    }
                }
            }
            catch (Exception ex)
            {
                new Exception("genText >> " + ex.Message);
            }
        }
        private void genImages(DataRow dr,bool isMaster)
        {
            try
            {
                if (isMaster)
                {
                    string urlMaster = "indexh.aspx?mid=" + dr["ui_id"].ToString() + "&level=" + (Level + 1) + "&p=" + ProjectType;
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowheader'><a href=" + urlMaster + "><img class='img-responsive' src='" + dr["content_image"].ToString().Replace("~/", "") + "'></a></div>"));
                }
                else
                {
                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='rowheader'><h4>" + dr["content_text"].ToString() + "</h4></div>"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("genImages >> " + ex.Message);
            }
        }
        private Control genLink(DataRow dr,string link)
        {
            try
            {
                Control ctr = new Control();

                // Gen Link
                if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                {

                    
                    if ((bool)dr["content_breakafter"])
                    {
                        if (imgControl == "")
                        {
                            ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row featurette'><a href='" + link + "'><div class=col-xs-4 col-sm-4 col-lg-4 col-md-5><img class='img' src='" + dr["content_image"].ToString().Replace("~/", "") + "'></div><div class=col-xs-8 col-sm-8 col-lg-8 col-md-7>" + dr["content_text"] + "</div></a></div>"));
                            imgControl = "";
                        }
                        else
                        {
                            imgControl += "<a href='" + dr["content_link"].ToString() + "'><img class='img' src='" + dr["content_image"].ToString().Replace("~/", "") + "'></a>";
                            ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row'>" + imgControl + "</div>"));
                        }
                        imgControl = "";
                    }
                    else
                    {
                        imgControl += "<a href='" + dr["content_link"].ToString() + "'><img class='img' src='" + dr["content_image"].ToString().Replace("~/", "") + "'></a>";
                    }

                }
                else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                {
                    if (dr["content_icon"] != null && dr["content_icon"].ToString() != "")
                    {
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row featurette'><img class='img' src='" + dr["content_icon"].ToString().Replace("~/", "") + "'><a class='img' href='" + link + "'>" + dr["content_text"].ToString() + "</a></div>"));
                    }
                    else
                    {
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row featurette'><a  href='" + link + "'>" + dr["content_text"].ToString() + "</a></div>"));
                    }

                }
                return ctr;
            }
            catch (Exception ex)
            {
                throw new Exception(" genLink>> " + ex.Message);
            }
        }
        private void genNews(DataRow dr)
        {
            bool isNews = dr["ui_isnews"].ToString() == "" ? false : (bool)dr["ui_isnews"];
            if (isNews)
            {
                // Gen News
                string url = "";
                string class_id = (Request["class_id"] == null ? "1" : Request["class_id"]);
                DataTable dt = new AppCode().News_SelectOracle(dr["ui_isnews_top"].ToString(), class_id);
                foreach (DataRow drContent in dt.Rows)
                {

                    url = "detailh.aspx?pcnt_id=" + drContent["news_id"].ToString() + "&sg=" + dr["ui_sg_id"].ToString()
                        + "&class_id=" + class_id + "&scs_id=" + bProperty_SCSID + "&mp_code=" + bProperty_MPCODE + "&prj=" + bProperty_PRJ + "&p=" + Request["p"];

                    frmMain.Controls.AddAt(frmMain.Controls.Count, new LiteralControl("<div class='row featurette'><a href='" + url + "'><div class=col-xs-4 col-sm-4 col-lg-4 col-md-5><img class='img' src='" + ConfigurationManager.AppSettings["isportPathNewsImages"] + drContent["news_images_190"].ToString() + "'></div><div class=col-xs-8 col-sm-8 col-lg-8 col-md-7>" + drContent["news_header_th"].ToString() + "</div></a></div>"));



                }
            }
        }
        #endregion
    }
}
