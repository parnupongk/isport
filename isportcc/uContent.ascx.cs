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
using MobileLibrary;
using WebLibrary;
namespace isportcc
{
    public partial class uContent : System.Web.UI.MobileControls.MobileUserControl
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

        public void uDataBind(string bProperty_LNG, string bProperty_MP
                        , string bProperty_SIZE, string bProperty_PRJ
                        , string bProperty_SGID, string bProperty_SCSID)
        {
            try
            {

                DataSet ds = new AppCode().SelectUIByLevel_Wap(MasterID, Level, OperatorType, ProjectType);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GenContent(dr, bProperty_LNG, bProperty_MP
                        , bProperty_SIZE, bProperty_PRJ
                        , bProperty_SGID, bProperty_SCSID);
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(" uDataBind>> " + ex.Message);
            }
        }
        #endregion

        #region GenCOntent
        private void GenContent(DataRow dr, string bProperty_LNG, string bProperty_MP
                        , string bProperty_SIZE, string bProperty_PRJ
                        , string bProperty_SGID, string bProperty_SCSID)
        {
            try
            {
                if (dr["content_icon"] != null && dr["content_icon"].ToString() != "")
                {
                    // Gen Icon
                    this.Controls.AddAt(this.Controls.Count, Utilities.Images(dr["content_icon"].ToString(), false, "", dr["content_align"].ToString()));
                }
                if (dr["content_link"] != null && dr["content_link"].ToString() != "")
                {
                    // Gen Link
                    if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                    {
                        this.Controls.AddAt(this.Controls.Count, Utilities.Images(dr["content_image"].ToString()
                            , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                            //, "redirect.aspx?sg=" + dr["ui_sg_id"].ToString() + "&r=" + dr["content_link"].ToString()//.Replace('&', '|')
                            , "redirect.aspx" + "?lng=" + bProperty_LNG + "&mp=" + bProperty_MP
                                + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                + "&sg=" + dr["ui_sg_id"].ToString() + "&scs_id=" + bProperty_SCSID
                                + "&r=" + dr["content_link"].ToString().Replace('&', '|')
                            , dr["content_align"].ToString()));
                    }
                    else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                    {
                        this.Controls.AddAt(this.Controls.Count, Utilities.Link(dr["content_text"].ToString()
                            , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                            //, "redirect.aspx?sg=" + dr["ui_sg_id"].ToString() + "&r=" + dr["content_link"].ToString()//.Replace('&', '|')
                            , "redirect.aspx" + "?lng=" + bProperty_LNG + "&mp=" + bProperty_MP
                                + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                + "&sg=" + dr["ui_sg_id"].ToString() + "&scs_id=" + bProperty_SCSID
                                + "&r=" + dr["content_link"].ToString().Replace('&', '|')
                            , dr["content_align"].ToString()));
                    }

                }
                else
                {
                    // ไม่มี link หรือ มี submenu
                    if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                    {
                        this.Controls.AddAt(this.Controls.Count, Utilities.Images(dr["content_image"].ToString()
                            , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                            , (bool)dr["ui_ismaster"] ? "~/index.aspx?mid=" + dr["ui_id"].ToString() + "&level=" + (Level + 1) : ""
                            , dr["content_align"].ToString()));
                    }
                    if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                    {
                        if ((bool)dr["ui_ismaster"])
                        {
                            this.Controls.AddAt(this.Controls.Count, Utilities.Link(dr["content_text"].ToString()
                                , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                                , "~/index.aspx?mid=" + dr["ui_id"].ToString() + "&level=" + (Level + 1)
                                , dr["content_align"].ToString()));
                        }
                        else
                        {

                            this.Controls.AddAt(this.Controls.Count, Utilities.Lable(dr["content_text"].ToString()
                                , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
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
