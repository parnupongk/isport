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
using WebLibrary;
using MobileLibrary;
namespace isport
{
    public partial class muFooter : System.Web.UI.MobileControls.MobileUserControl
    {
        #region Property
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

                DataSet ds = new AppCode().SelectFooterByoperator(OperatorType,Request["p"]);
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
                        this.Controls.AddAt(this.Controls.Count, Utilities.ImagesBannerLink(dr["content_image"].ToString()
                            , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                            //, "redirect.aspx?r=" + dr["content_link"].ToString().Replace('&', '|')
                             , "redirect.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MP
                                + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                + "&sg=" + dr["footer_sg_id"].ToString() + "&scs_id=" + bProperty_SCSID
                                + "&r=" + dr["content_link"].ToString().Replace('&', '|')
                            , dr["content_align"].ToString()));
                    }
                    else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                    {
                        this.Controls.AddAt(this.Controls.Count, Utilities.Link(dr["content_text"].ToString()
                            , false//dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                            //, "redirect.aspx?r=" + dr["content_link"].ToString().Replace('&', '|')
                            , "redirect.aspx" + "?lng=" + bProperty_LNG + "&mp_code=" + bProperty_MP
                                + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                + "&sg=" + dr["footer_sg_id"].ToString() + "&scs_id=" + bProperty_SCSID
                                + "&r=" + dr["content_link"].ToString().Replace('&', '|')
                            , dr["content_align"].ToString()));

                        this.Controls.AddAt(this.Controls.Count, Utilities.Lable(""
                            , true//dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                            , "Left"));
                    }

                }
                else
                {
                    // ไม่มี link หรือ มี submenu
                    if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                    {
                        this.Controls.AddAt(this.Controls.Count, Utilities.Images(dr["content_image"].ToString()
                            , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                            ,""
                            , dr["content_align"].ToString()));
                    }
                    if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                    {

                        this.Controls.AddAt(this.Controls.Count, Utilities.Lable(dr["content_text"].ToString()
                            , dr["content_breakafter"] == null ? false : (bool)dr["content_breakafter"]
                            , dr["content_align"].ToString()));

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
