using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Web.UI;
namespace isport_service
{
    public class ServiceWapUI_GenControls
    {
        #region Gen Content

        #region Text
        public static Control genText(DataRow dr,string pageName,string level,string projectType,bool isMaster,bool isHeader)
        {
            try
            {
                Control ctr = new Control();
                string rowCSS = dr["content_bgcolor"] == null || dr["content_bgcolor"].ToString() == "" ? "row" : "row_" + dr["content_bgcolor"].ToString()+"_"+dr["content_align"].ToString();
                string txtCSS = dr["content_txtsize"] == null || dr["content_txtsize"].ToString() == "" ? "p" : dr["content_txtsize"].ToString();
                if (dr["content_icon"] != null && dr["content_icon"].ToString() != "")
                {
                    if (isMaster)
                    {
                        string urlMaster = pageName + "&mid=" + dr["ui_id"].ToString() + "&level=" + (level + 1) + "&p=" + projectType;
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><div class='col-xs-12 col-sm-12 col-lg-12'><a href=" + urlMaster + "><" + txtCSS + "><img class='img' src='http://wap.isport.co.th/isportui/" + dr["content_icon"].ToString().Replace("~/", "") + "'>" + MobileLibrary.Utilities.nl2br( dr["content_text"].ToString() )+ "</" + txtCSS + "></a></div></div>"));
                    }
                    else
                    {
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><div class='col-xs-12 col-sm-12 col-lg-12'><" + txtCSS + "><img class='img' src='http://wap.isport.co.th/isportui/" + dr["content_icon"].ToString().Replace("~/", "") + "'>" + MobileLibrary.Utilities.nl2br(dr["content_text"].ToString() ) + "</" + txtCSS + "></div></div>"));
                    }
                }
                else
                {
                    if (isMaster)
                    {
                        string urlMaster = pageName+"&mid=" + dr["ui_id"].ToString() + "&level=" + (level + 1) + "&p=" + projectType;
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><div class='col-xs-12 col-sm-12 col-lg-12'><a href=" + urlMaster + "><" + txtCSS + ">" + MobileLibrary.Utilities.nl2br(dr["content_text"].ToString()) + "</" + txtCSS + "></a></div></div>"));
                    }
                    else
                    {
                        if (isHeader)
                        {
                            if(dr["content_align"].ToString() == "Left")
                            ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><div class='col-xs-12 col-sm-12 col-lg-12'><p class='row_Header_Left'>" + MobileLibrary.Utilities.nl2br(dr["content_text"].ToString()) + "</p></div></div>"));
                            else
                                ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><div class='col-xs-12 col-sm-12 col-lg-12'><h4>" + MobileLibrary.Utilities.nl2br(dr["content_text"].ToString()) + "</h4></div></div>"));
                        }
                        else
                        {
                            ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><div class='col-xs-12 col-sm-12 col-lg-12'><" + txtCSS + ">" + MobileLibrary.Utilities.nl2br(dr["content_text"].ToString()) + "</" + txtCSS + "></div></div>"));
                        }
                    }
                }
                return ctr;
            }
            catch (Exception ex)
            {
                throw new Exception("genText >> " + ex.Message);
            }
        }
        public static Control genText(string srcIcon,string txtContent,string pageName, string level, string projectType, bool isMaster,string uiId)
        {
            try
            {
                Control ctr = new Control();
                txtContent = MobileLibrary.Utilities.nl2br(txtContent);
                if (srcIcon != "")
                {
                    if (isMaster)
                    {
                        string urlMaster = pageName + "&mid=" + uiId + "&level=" + (level + 1) + "&p=" + projectType;
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowheader'><div class='col-xs-12 col-sm-12 col-md-12'><a href=" + urlMaster + "><p><img class='img' src='" + srcIcon + "'>" + txtContent + "</p></a></div></div>"));
                    }
                    else
                    {
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row'><div class='col-xs-12 col-sm-12 col-md-12'><p><img class='img' src='" + srcIcon + "'>" + txtContent + "</p></div></div>"));
                    }
                }
                else
                {
                    if (isMaster)
                    {
                        string urlMaster = pageName + "&mid=" + uiId + "&level=" + (level + 1) + "&p=" + projectType;
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowheader'><div class='col-xs-12 col-sm-12 col-md-12'><a href=" + urlMaster + "><p>" + txtContent + "</p></a></div></div>"));
                    }
                    else
                    {
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row'><div class='col-xs-12 col-sm-12 col-md-12'><p>" + txtContent + "</p></div></div>"));
                    }
                }
                return ctr;
            }
            catch (Exception ex)
            {
                throw new Exception("genText >> " + ex.Message);
            }
        }
        public static Control genTextHeader(string srcIcon, string txtContent, string pageName, string level, string projectType, bool isMaster, string uiId)
        {
            try
            {
                Control ctr = new Control();
                txtContent = MobileLibrary.Utilities.nl2br(txtContent);
                if (srcIcon != "")
                {
                    if (isMaster)
                    {
                        string urlMaster = pageName + "&mid=" + uiId + "&level=" + (level + 1) + "&p=" + projectType;
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowheader'><div class='col-xs-12 col-sm-12 col-md-12'><a href=" + urlMaster + "><h4><img class='img' src='" + srcIcon + "'>" + txtContent + "</h4></a></div></div>"));
                    }
                    else
                    {
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowheader'><div class='col-xs-12 col-sm-12 col-md-12'><h4><img class='img' src='" + srcIcon + "'>" + txtContent + "</h4></div></div>"));
                    }
                }
                else
                {
                    if (isMaster)
                    {
                        string urlMaster = pageName + "&mid=" + uiId + "&level=" + (level + 1) + "&p=" + projectType;
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowheader'><div class='col-xs-12 col-sm-12 col-md-12'><a href=" + urlMaster + "><h4>" + txtContent + "</h4></a></div></div>"));
                    }
                    else
                    {
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowheader'><div class='col-xs-12 col-sm-12 col-md-12'><h4>" + txtContent + "</h4></div></div>"));
                    }
                }
                return ctr;
            }
            catch (Exception ex)
            {
                throw new Exception("genText >> " + ex.Message);
            }
        }
        
        #endregion

        #region Image
        public static Control genImagesHeader(DataRow dr, string pageName, string level, string projectType, bool isMaster,string classImg)
        {
            try
            {
                Control ctr = new Control();
                string rowCSS = dr["content_bgcolor"] == null || dr["content_bgcolor"].ToString() == "" ? "row" : "row_" + dr["content_bgcolor"].ToString();
                string imgURL ="";
                imgURL = dr["content_image"] == null || dr["content_image"].ToString() == "" ? imgURL : dr["content_image"].ToString();
                imgURL = imgURL.IndexOf("http") > -1 ? imgURL : "http://wap.isport.co.th/isportui/" + imgURL.Replace("~/", "");

                if (isMaster)
                {
                    string urlMaster = pageName + "&mid=" + dr["ui_id"].ToString() + "&level=" + (level + 1) + "&p=" + projectType;
                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><div class='col-xs-12 col-sm-12 col-lg-12'><a href=" + urlMaster + "><img class='" + classImg + "' src='" + imgURL + "'/></a></div></div>"));
                }
                else
                {
                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='" + rowCSS + "'><img class='" + classImg + "' src='" + imgURL + "'/></div>"));
                }


                return ctr;
            }
            catch (Exception ex)
            {
                throw new Exception("genImages >> " + ex.Message);
            }
        }
        public static Control genImagesHeader(string pageName,string src,string classImg, string level, string projectType, bool isMaster,string uiId)
        {
            try
            {
                Control ctr = new Control();
                if (isMaster)
                {
                    string urlMaster = pageName + "&mid=" + uiId + "&level=" + (level + 1) + "&p=" + projectType;
                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowheader'><div class='col-xs-12 col-sm-12 col-lg-12'><a href=" + urlMaster + "><img class='" + classImg + "' src='" + src + "'/></a></div></div>"));
                }
                else
                {
                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowheader'><div class='col-xs-12 col-sm-12 col-lg-12'><img class='" + classImg + "' src='" + src + "'/></div></div>"));
                }
                return ctr;
            }
            catch (Exception ex)
            {
                throw new Exception("genImages >> " + ex.Message);
            }
        }
        public static Control genImages(DataRow dr, string pageName, string level, string projectType,bool isMaster)
        {
            try
            {
                Control ctr = new Control();
                string imgURL = "";
                imgURL = dr["content_image"] == null || dr["content_image"].ToString() == "" ? imgURL : dr["content_image"].ToString();
                imgURL = imgURL.IndexOf("http") > -1 ? imgURL : "http://wap.isport.co.th/isportui/" + imgURL.Replace("~/", "");

                if (isMaster)
                {
                    string urlMaster = pageName + "&mid=" + dr["ui_id"].ToString() + "&level=" + (level + 1) + "&p=" + projectType;
                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowheader'><div class='col-xs-12 col-sm-12 col-lg-12'><a href=" + urlMaster + "><img class='img-responsive' src='" + imgURL + "'/></a></div></div>"));
                }
                else
                {
                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='rowheader'><div class='col-xs-12 col-sm-12 col-lg-12'><img class='img-responsive' src='" + imgURL + "'/></div></div>"));
                }
                return ctr;
            }
            catch (Exception ex)
            {
                throw new Exception("genImages >> " + ex.Message);
            }
        }
        #endregion

        #region Link
        public static Control genLinkHeader(DataRow dr, string imgControl, string link, string imgClass)
        {
            try
            {
                Control ctr = new Control();

                // Gen Link
                if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                {


                    ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row'><div class='col-xs-12 col-sm-12 col-lg-12'><a href='" + dr["content_link"].ToString() + "'><img class='" + imgClass + "' src='http://wap.isport.co.th/isportui/" + dr["content_image"].ToString().Replace("~/", "") + "'/></a></div></div>"));

                }
                else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                {
                    if (dr["content_icon"] != null && dr["content_icon"].ToString() != "")
                    {
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row'><div class='col-xs-12 col-sm-12 col-lg-12'><img class='img' src='http://wap.isport.co.th/isportui/" + dr["content_icon"].ToString().Replace("~/", "") + "'/><a class='img' href='" + link + "'>" + dr["content_text"].ToString() + "</a></div></div>"));
                    }
                    else
                    {
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row'><div class='col-xs-12 col-sm-12 col-lg-12'><a  href='" + link + "'>" + dr["content_text"].ToString() + "</a></div></div>"));
                    }

                }
                return ctr;
            }
            catch (Exception ex)
            {
                throw new Exception(" genLink>> " + ex.Message);
            }
        }

        public static Control genLink(DataRow dr, string imgControl, string link,string imgClass)
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
                            //ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row featurette'><a href='" + link + "'><div class=col-xs-4 col-sm-4 col-lg-4 col-md-5><img class='" + imgClass + "' src='http://wap.isport.co.th/isportui/" + dr["content_image"].ToString().Replace("~/", "") + "'></div><div class=col-xs-8 col-sm-8 col-lg-8 col-md-7>" + dr["content_text"] + "</div></a></div>"));
                            imgControl += "<div class='media'><a href='" + link + "'><img class='pull-left' src='http://wap.isport.co.th/isportui/" + dr["content_image"].ToString().Replace("~/", "") + "'></a><div class='media-body'>" + dr["content_text"] + "</div></div>";
                            imgControl = "";
                        }
                        else
                        {
                            imgControl += "<a href='" + dr["content_link"].ToString() + "'><img class='" + imgClass + "' src='http://wap.isport.co.th/isportui/" + dr["content_image"].ToString().Replace("~/", "") + "'></a>";
                            ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row'>" + imgControl + "</div>"));
                        }
                        imgControl = "";
                    }
                    else
                    {
                        imgControl += "<a href='" + dr["content_link"].ToString() + "'><img class='" + imgClass + "' src='http://wap.isport.co.th/isportui/" + dr["content_image"].ToString().Replace("~/", "") + "'></a>";
                    }

                }
                else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                {
                    if (dr["content_icon"] != null && dr["content_icon"].ToString() != "")
                    {
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row featurette'><div class='col-xs-12 col-sm-12 col-lg-12'><img class='img' src='http://wap.isport.co.th/isportui/" + dr["content_icon"].ToString().Replace("~/", "") + "'><a class='img' href='" + link + "'>" + dr["content_text"].ToString() + "</a></div></div>"));
                    }
                    else
                    {
                        ctr.Controls.AddAt(ctr.Controls.Count, new LiteralControl("<div class='row featurette'><div class='col-xs-12 col-sm-12 col-lg-12'><a  href='" + link + "'>" + dr["content_text"].ToString() + "</a></div></div>"));
                    }

                }
                return ctr;
            }
            catch (Exception ex)
            {
                throw new Exception(" genLink>> " + ex.Message);
            }
        }

        public static string genLink(DataRow dr, string imgControl, string link,string imgClass,string type)
        {
            try
            {

                // Gen Link
                if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                {


                    if ((bool)dr["content_breakafter"])
                    {
                        if (imgControl == "")
                        {
                            //imgControl += "<div class='row featurette'><a href='" + link + "'><div class=col-xs-4 col-sm-4 col-lg-4 col-md-5><img class='" + imgClass + "' src='http://wap.isport.co.th/isportui/" + dr["content_image"].ToString().Replace("~/", "") + "'></div><div class=col-xs-8 col-sm-8 col-lg-8 col-md-7>" + dr["content_text"] + "</div></a></div>";
                            imgControl += "<div class='media'><a href='" + link + "'><img class='pull-left' src='http://wap.isport.co.th/isportui/" + dr["content_image"].ToString().Replace("~/", "") + "'></a><div class='media-body'>" + dr["content_text"] + "</div></div>";
                        }
                        else
                        {
                            imgControl += "<a href='" + link + "'><img class='" + imgClass + "' src='http://wap.isport.co.th/isportui/" + dr["content_image"].ToString().Replace("~/", "") + "'></a>";
                            imgControl += "<div class='row'>" + imgControl + "</div>";
                        }
                        //imgControl = "";
                    }
                    else
                    {
                        imgControl += "<a href='" + dr["content_link"].ToString() + "'><img class='" + imgClass + "' src='http://wap.isport.co.th/isportui/" + dr["content_image"].ToString().Replace("~/", "") + "'></a>";
                    }

                }
                else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                {
                    if (dr["content_icon"] != null && dr["content_icon"].ToString() != "")
                    {
                        imgControl += "<div class='row featurette'><div class='col-xs-12 col-sm-12 col-lg-12'><img class='img' src='http://wap.isport.co.th/isportui/" + dr["content_icon"].ToString().Replace("~/", "") + "'><a class='img' href='" + link + "'>" + dr["content_text"].ToString() + "</a></div></div>";
                    }
                    else
                    {
                        imgControl += "<div class='row featurette'><div class='col-xs-12 col-sm-12 col-lg-12'><a  href='" + link + "'>" + dr["content_text"].ToString() + "</a></div></div>";
                    }

                }
                return imgControl;
            }
            catch (Exception ex)
            {
                throw new Exception(" genLink>> " + ex.Message);
            }
        }

        public static string genLinkNoDiv(DataRow dr, string imgControl, string link, string imgClass, string type)
        {
            try
            {

                // Gen Link
                if (dr["content_image"] != null && dr["content_image"].ToString() != "")
                {


                    if ((bool)dr["content_breakafter"])
                    {
                        if (imgControl == "")
                        {
                            imgControl += "<div class='media'><a href='" + link + "'><img class='pull-left' src='http://wap.isport.co.th/isportui/" + dr["content_image"].ToString().Replace("~/", "") + "'></a><div class='media-body'>" + dr["content_text"] + "</div></div>";
                        }
                        else
                        {
                            imgControl += "<a href='" + link + "'><img class='" + imgClass + "' src='http://wap.isport.co.th/isportui/" + dr["content_image"].ToString().Replace("~/", "") + "'></a>";
                            imgControl += "<div class='row'>" + imgControl + "</div>";
                        }
                        //imgControl = "";
                    }
                    else
                    {
                        imgControl += "<a href='" + dr["content_link"].ToString() + "'><img class='" + imgClass + "' src='http://wap.isport.co.th/isportui/" + dr["content_image"].ToString().Replace("~/", "") + "'></a>";
                    }

                }
                else if (dr["content_text"] != null && dr["content_text"].ToString() != "")
                {
                    if (dr["content_icon"] != null && dr["content_icon"].ToString() != "")
                    {
                        //imgControl += "<div class='row featurette'><div class='col-xs-12 col-sm-12 col-lg-12'><img class='img' src='http://wap.isport.co.th/isportui/" + dr["content_icon"].ToString().Replace("~/", "") + "'><a class='img' href='" + link + "'>" + dr["content_text"].ToString() + "</a></div></div>";
                        imgControl += "<a class='img' href='" + link + "'><img class='img' src='http://wap.isport.co.th/isportui/" + dr["content_icon"].ToString().Replace("~/", "") + "'>" + dr["content_text"].ToString() + "</a>";
                    }
                    else
                    {
                        imgControl += "<a  href='" + link + "'>" + dr["content_text"].ToString() + "</a>";
                    }

                }
                return imgControl;
            }
            catch (Exception ex)
            {
                throw new Exception(" genLink>> " + ex.Message);
            }
        }

        public static string genTextLink(string txt, string imgControl,string iconURL, string link, string imgClass, string type)
        {
            try
            {

                if (iconURL != "")
                {
                    imgControl += "<div class='row featurette'><div class='col-xs-12 col-sm-12 col-lg-12'><img class='img' src='" + iconURL + "'><a class='img' href='" + link + "'>" + txt + "</a></div></div>";
                }
                else
                {
                    imgControl += "<div class='row featurette'><div class='col-xs-12 col-sm-12 col-lg-12'><a  href='" + link + "'>" + txt + "</a></div></div>";
                }

                return imgControl;
            }
            catch (Exception ex)
            {
                throw new Exception(" genLink>> " + ex.Message);
            }
        }


        #endregion

        #endregion

    }
}
