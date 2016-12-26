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
    public class ServiceWapUI_Footer
    {
        

        #region Footer
        public Control GenFooterVarity(string strConn, string sgId, string classId, string projectType, string pageRedirect, string pCatId, string sexType, int top,bool isRandom)
        {
            try
            {
                Control ctr = new Control();
                pCatId = pCatId == null ? "176" : pCatId;
                ServiceVarity.GenVarity(strConn, ctr, sgId, classId, projectType, pageRedirect, pCatId, sexType, top,isRandom);
                return ctr;
            }
            catch (Exception ex)
            {
                throw new Exception("GenFooterVarity >> " + ex.Message);
            }
        }

        /// <summary>
        /// GenFooter
        /// </summary>
        /// <param name="strConn">connection string</param>
        /// <param name="opt">operator type</param>
        /// <param name="projectType">project type</param>
        /// <param name="urlParameter">url parameter (&n=09282722&s=324324)</param>
        /// <param name="pageName">page name to redirect (indexh.aspx)</param>
        /// <param name="level">level master</param>
        /// <returns></returns>
        public Control GenFooter(string strConn,string opt, string projectType,string urlParameter,string pageName,string level,string mpCode,string prj,string scsId,string classId)
        {
            try
            {
                //Control ctr = new Control();
                //DataSet ds = new AppCode().SelectFooterByoperator(strConn,opt, projectType);
                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    string link = "http://wap.isport.co.th/isportui/redirect.aspx" + "?mp_code=" + mpCode
                //                    + "&prj=" + prj
                //                    + "&sg=" + dr["footer_sg_id"].ToString() + "&scs_id=" + scsId
                //                    + "&r=" + dr["content_link"].ToString().Replace('&', '|');
                //    link += "&class_id=" + classId ;

                //    //string link = dr["content_link"].ToString() + "&sg=" + dr["footer_sg_id"].ToString();

                //    if (dr["content_link"] != null && dr["content_link"].ToString() != "")
                //    {

                //        ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genLink(dr,"", link,"img"));
                //    }
                //    else
                //    {

                //        if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                //        {
                //            ctr.Controls.AddAt(ctr.Controls.Count,ServiceWapUI_GenControls.genImages(dr,pageName,level,projectType,false));

                //        }
                //        else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                //        {
                //            ctr.Controls.AddAt(ctr.Controls.Count,ServiceWapUI_GenControls.genText(dr,pageName,level,projectType,false,(bool)dr["footer_ispayment"]));
                //        }
                //    }
                //}

                return GenFooterListGroup(strConn, opt, projectType, urlParameter, pageName, level, mpCode, prj, scsId, classId);
            }
            catch (Exception ex)
            {
                throw new Exception("GenFooter >>" + ex.Message);
            }
        }


        /// <summary>
        /// GenFooterListGroup
        /// </summary>
        /// <param name="strConn">connection string</param>
        /// <param name="opt">operator type</param>
        /// <param name="projectType">project type</param>
        /// <param name="urlParameter">url parameter (&n=09282722&s=324324)</param>
        /// <param name="pageName">page name to redirect (indexh.aspx)</param>
        /// <param name="level">level master</param>
        /// <returns></returns>
        public Control GenFooterListGroup(string strConn, string opt, string projectType, string urlParameter, string pageName, string level, string mpCode, string prj, string scsId, string classId)
        {
            try
            {
                Control ctr = new Control();
                DataSet ds = new AppCode().SelectFooterByoperator(strConn, opt, projectType);
                ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='col-xs-12 col-md-4 col-sm-6'><ul class='nav nav-pills nav-stacked'>"));
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string link = "http://wap.isport.co.th/isportui/redirect.aspx" + "?mp_code=" + mpCode
                                    + "&prj=" + prj
                                    + "&sg=" + dr["footer_sg_id"].ToString() + "&scs_id=" + scsId
                                    + "&r=" + dr["content_link"].ToString().Replace('&', '|');
                    link += "&class_id=" + classId;

                    //string link = dr["content_link"].ToString() + "&sg=" + dr["footer_sg_id"].ToString();
                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<li role='presentation' >"));

                    if (dr["content_link"] != null && dr["content_link"].ToString() != "")
                    {

                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl( ServiceWapUI_GenControls.genLinkNoDiv(dr, "", link, "img","")));
                    }
                    else
                    {

                        if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                        {
                            ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genImagesHeader(dr, pageName, level, projectType, false,"img-full"));

                        }
                        else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                        {
                            ctr.Controls.AddAt(ctr.Controls.Count, ServiceWapUI_GenControls.genText(dr, pageName, level, projectType, false, (bool)dr["footer_ispayment"]));
                        }
                    }

                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("</li>"));
                }
                ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("</ul></div>"));

                return ctr;
            }
            catch (Exception ex)
            {
                throw new Exception("GenFooter >>" + ex.Message);
            }
        }
        #endregion
    }
}
