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
using WebLibrary;
using MobileLibrary;
namespace isport
{
    public partial class muContent : System.Web.UI.MobileControls.MobileUserControl
    {

        #region Property
        public string MasterID
        {
            get
            {
                return ViewState["MasterID"] == null ? "0" : ViewState["MasterID"].ToString();
            }
            set
            {
                ViewState["MasterID"] = value;
            }
        }
        public int Level
        {
            get
            {
                return ViewState["Level"] == null ? 0 : (int)ViewState["Level"];
            }
            set
            {
                ViewState["Level"] = value;
            }
        }
        public string OperatorType
        {
            get
            {
                return ViewState["OperatorType"] == null ? AppCode.OperatorType.All.ToString() : ViewState["OperatorType"].ToString();
            }
            set
            {
                ViewState["OperatorType"] = value;
            }
        }
        public string ProjectType
        {
            get
            {
                return ViewState["ProjectType"] == null ? AppCode.ProjectType.bb.ToString() : ViewState["ProjectType"].ToString();
            }
            set
            {
                ViewState["ProjectType"] = value;
            }
        }
        #endregion

        #region Pageload & databind
        
        protected void Page_Load(object sender, EventArgs e)
        {



        }
        public void uDataBindNewsDetail(string pcntId,string bProperty_LNG, string bProperty_MPCODE
                       , string bProperty_SIZE, string bProperty_PRJ
                       , string bProperty_SGID, string bProperty_SCSID)
        {
            try
            {

                DataTable dt = new AppCode().News_SelectByIdOracle(pcntId);
                if (dt.Rows.Count > 0)
                {
                    DataRow drContent = dt.Rows[0];
                    this.Controls.AddAt(this.Controls.Count, Utilities.Images(ConfigurationManager.AppSettings["isportPathNewsImages"] + drContent["news_images_190"].ToString()
                            , true
                            , ""
                            , "Center"));

                    this.Controls.AddAt(this.Controls.Count, Utilities.Lable(drContent["news_header_th"].ToString()
                                , true
                                , "Center"
                                , ""
                                , ""));

                    this.Controls.AddAt(this.Controls.Count, Utilities.Lable(drContent["news_title_th"].ToString()
                                , true
                                , "Left"
                                , ""
                                , ""));

                }


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(" uDataBind>> " + ex.Message);
            }
        }

        public void uDataBind( string bProperty_LNG ,string bProperty_MPCODE
                        ,string bProperty_SIZE ,string bProperty_PRJ
                        ,string bProperty_SGID ,string bProperty_SCSID)
        {
            try
            {
                if (Request["class_id"] != null)
                {
                    // Add Class Name
                    this.Controls.AddAt(this.Controls.Count, Utilities.Images("../images/class_"+Request["class_id"]+".gif"
                            , true
                            , ""
                            , "Left"));
                    //string className = new AppCode().GetClassNameByClassId(Request["class_id"], bProperty_LNG.ToString());
                    //this.Controls.AddAt(this.Controls.Count, Utilities.Lable(className
                    //            , true
                    //            , "Left"
                    //            , "Blue"
                    //            ,"false"));
                }

                DataSet ds = new AppCode().SelectUIByLevel_Wap(MasterID, Level, OperatorType, ProjectType);
                foreach(DataRow dr in ds.Tables[0].Rows)
                {

                    //HtmlGenericControl NewControl = new HtmlGenericControl("div");
                    //NewControl.Attributes.Add("class", "div2");
                    //NewControl.Controls.Add(Utilities.Lable("sdgfsadgasdgdsag"
                    //            , false
                    //            , "Left"
                    //            , ""
                    //            , ""));
                    //this.Controls.AddAt(this.Controls.Count, NewControl );

                    GenContent(dr, bProperty_LNG, bProperty_MPCODE
                        , bProperty_SIZE , bProperty_PRJ
                        , bProperty_SGID , bProperty_SCSID);


                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(" uDataBind>> " + ex.Message);
            }
        }
        #endregion

        #region GenCOntent
        private void GenContent(DataRow dr, string bProperty_LNG, string bProperty_MPCODE
                        , string bProperty_SIZE, string bProperty_PRJ
                        , string bProperty_SGID, string bProperty_SCSID)
        {
            try
            {
                string link = "redirect.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
                                + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                + "&sg=" + dr["ui_sg_id"].ToString() + "&scs_id=" + bProperty_SCSID
                                + "&r=" + dr["content_link"].ToString().Replace('&', '|');
                link += (Request["class_id"] != null && link.IndexOf("class_id") < 0 )? "&class_id=" + Request["class_id"] : "" ;

                if (dr["ui_contentname"] != null && dr["ui_contentname"].ToString() != "")
                {
                    uFootballLiveScore football = new uFootballLiveScore();
                    football.ClassDataBinding();
                    this.Controls.AddAt(this.Controls.Count, football);
                }
                if (dr["content_icon"] != null && dr["content_icon"].ToString() != "")
                {
                    // Gen Icon
                    this.Controls.AddAt(this.Controls.Count, Utilities.Images(dr["content_icon"].ToString(),false,"", dr["content_align"].ToString()));
                }
                if (dr["content_link"] != null && dr["content_link"].ToString() != "")
                {
                    // Gen Link
                    if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                    {

                        this.Controls.AddAt(this.Controls.Count, Utilities.Images(dr["content_image"].ToString()
                            , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                            , link
                            , dr["content_align"].ToString()));

                        this.Controls.AddAt(this.Controls.Count, Utilities.Lable(""
                                ,false
                                , "Center"
                                , ""
                                , ""));

                    }
                    else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                    {


                        if (dr["content_text"].ToString().Trim() == "สมัคร")
                        {
                            this.Controls.AddAt(this.Controls.Count, Utilities.LinkRed(dr["content_text"].ToString()
                            , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                            , link
                            , dr["content_align"].ToString()));
                        }
                        else if (dr["content_link"].ToString().IndexOf("tel") > -1 )
                        {
                            this.Controls.AddAt(this.Controls.Count, Utilities.LinkRed(dr["content_text"].ToString()
                            , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                            , dr["content_link"].ToString()
                            , dr["content_align"].ToString()));
                        }
                        else
                        {
                            if (!(dr["content_text"].ToString() == "สตาร์ซอคเกอร์" && Request["p"] == "d02ballleague" && new AppCode().GetCountSportContent("248","00076") == 0) )
                            {
                                this.Controls.AddAt(this.Controls.Count, Utilities.LinkRed(dr["content_text"].ToString()
                                , false
                                , link
                                , dr["content_align"].ToString()));
                                this.Controls.AddAt(this.Controls.Count, Utilities.Lable(""
                                    , true
                                    , "Left"
                                    , ""
                                    , ""));
                            }

                        }
                    }
                    
                }
                else
                { 
                    // ไม่มี link หรือ มี submenu
                    if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                    {
                        if ((bool)dr["ui_ismaster"])
                        {
                            this.Controls.AddAt(this.Controls.Count, Utilities.Images(dr["content_image"].ToString()
                                , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                                , (bool)dr["ui_ismaster"] ? "~/sportcc_i.aspx?mid=" + dr["ui_id"].ToString() + "&level=" + (Level + 1) + "&p=" + ProjectType : ""
                                , dr["content_align"].ToString()));
                        }
                        else
                        {
                            this.Controls.AddAt(this.Controls.Count, Utilities.Images(dr["content_image"].ToString()
                                , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                                , ""
                                , dr["content_align"].ToString()));
                        }
                    }
                    if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                    {
                        if ((bool)dr["ui_ismaster"])
                        {
                            this.Controls.AddAt(this.Controls.Count, Utilities.Link(dr["content_text"].ToString()
                                , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                                , "~/sportcc_i.aspx?mid=" + dr["ui_id"].ToString() + "&level=" + (Level + 1) + "&p=" + Request["p"].ToString()
                                , dr["content_align"].ToString()));
                        }
                        else
                        {

                            this.Controls.AddAt(this.Controls.Count, Utilities.Lable(dr["content_text"].ToString()
                                , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                                , dr["content_align"].ToString()
                                , dr["content_color"].ToString()
                                , dr["content_bold"].ToString()));
                        }
                    }
                    else
                    { // add br
                        this.Controls.AddAt(this.Controls.Count, Utilities.Lable(dr["content_text"].ToString()
                                , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                                , dr["content_align"].ToString()));
                    }
                    bool isNews = dr["ui_isnews"].ToString() == "" ? false : (bool)dr["ui_isnews"];
                    if (isNews)
                    {
                        // Gen News
                        string class_id = (Request["class_id"] == null ? "1" : Request["class_id"]);
                        DataTable dt = new AppCode().News_SelectOracle(dr["ui_isnews_top"].ToString(),class_id);
                        foreach (DataRow drContent in dt.Rows)
                        {
                            if (int.Parse(dr["ui_isnews_top"].ToString()) < 10)
                            {
                                this.Controls.AddAt(this.Controls.Count, Utilities.Images(ConfigurationManager.AppSettings["isportPathNewsImages"] + drContent["news_images_190"].ToString()
                                , true
                                , ""
                                , "Left"));
                            }
                            else
                            {
                                this.Controls.AddAt(this.Controls.Count, Utilities.Images("../images/Bullet.gif"
                                , false
                                , ""
                                , "Left"));
                            }

                            this.Controls.AddAt(this.Controls.Count, Utilities.Link(drContent["news_header_th"].ToString()
                                , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                                , "../football_news_detail.aspx?pcnt_id=" + drContent["news_id"].ToString() + "&sg=" + dr["ui_sg_id"].ToString()
                                + "&class_id=" + class_id + "&scs_id=" + bProperty_SCSID + "&mp_code=" + bProperty_MPCODE + "&prj=" + bProperty_PRJ + "&p=" + Request["p"].ToString()
                                , dr["content_align"].ToString()));
                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion



    }
}
