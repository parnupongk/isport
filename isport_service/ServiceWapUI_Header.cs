using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using Microsoft.ApplicationBlocks.Data;
namespace isport_service
{
    public class ServiceWapUI_Header
    {
        #region Header
        /// <summary>
        /// GenHeader
        /// </summary>
        /// <param name="strConn">connection string</param>
        /// <param name="opt">operator type</param>
        /// <param name="projectType">project type</param>
        /// <param name="urlParameter">url parameter (&n=09282722&s=324324)</param>
        /// <param name="pageName">page name to redirect (indexh.aspx)</param>
        /// <param name="level">level master</param>
        /// <returns></returns>
        public Control GenHeader(string strConn, string opt, string projectType, string urlParameter, string pageName, string level)
        {
            try
            {
                Control ctr = new Control();
                DataSet ds = new AppCode().SelectHeaderByOperator(strConn, opt, projectType);
                List<DataRow> drs = new List<DataRow>();
                bool isRandom = false;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    /* string link = "redirect.aspx" + "?lng=L" + bProperty_LNG + "&mp_code=" + bProperty_MPCODE
                                     + "&size=" + bProperty_SIZE + "&prj=" + bProperty_PRJ
                                     + "&sg=" + dr["Header_sg_id"].ToString() + "&scs_id=" + bProperty_SCSID
                                     + "&r=" + dr["content_link"].ToString().Replace('&', '|');
                     link += (Request["class_id"] != null && link.IndexOf("class_id") < 0) ? "&class_id=" + Request["class_id"] : "";*/

                    if (dr["header_random"].ToString() == "start" || isRandom)
                    {
                        isRandom = true;
                        if (dr["header_operator"].ToString() == "All" || opt == dr["header_operator"].ToString()) drs.Add(dr);
                    }
                    if (!isRandom) ctr.Controls.AddAt(ctr.Controls.Count,GenContent(dr, opt, projectType, urlParameter, pageName, level));
                    if (dr["header_random"].ToString() == "end")
                    {
                        isRandom = false;
                        ctr.Controls.AddAt(ctr.Controls.Count,GenContent(drs[new Random().Next(0, drs.Count)], opt, projectType, urlParameter, pageName, level));
                    }

                    
                }
                return ctr;
            }
            catch (Exception ex)
            {
                throw new Exception("GenHeader >>" + ex.Message);
            }
        }


        private Control GenContent(DataRow dr ,string opt, string projectType, string urlParameter, string pageName, string level)
        {
            Control ctr = new Control();
            string link = dr["content_link"].ToString() + "&sg=" + dr["Header_sg_id"].ToString();

            if (dr["content_link"] != null && dr["content_link"].ToString() != "")
            {

                ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genLinkHeader(dr, "", link, "img-full"));
            }
            else
            {

                if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                {
                    ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genImagesHeader(dr, pageName, level, projectType, false, "img-full"));

                }
                else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                {
                    ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genText(dr, pageName, level, projectType, false, true));
                }
            }
            return ctr;
        }
        #endregion
    }
}
