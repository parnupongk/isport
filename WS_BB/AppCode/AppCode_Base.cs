using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WS_BB
{
    public class AppCode_Base
    {
        
        public static string strConnOracle = ConfigurationManager.ConnectionStrings["IsportWSOracleConnectionString"].ToString();
        public static string strConn = ConfigurationManager.ConnectionStrings["IsportWSConnectionString"].ToString();
        public static string strConnPack = ConfigurationManager.ConnectionStrings["IsportWSPackConnectionString"].ToString();
        public static string strConnFeed = ConfigurationManager.ConnectionStrings["IsportWSFeedConnectionString"].ToString();
        
        public enum Country
        {
            en
            ,th
        }
        public enum AppName
        {
            SportPool
            ,SportArena
            ,StarSoccer3GX
            ,StarSoccer
            ,FeedNewToAis
            ,FeedAis
            ,MobileLifeStyle
            ,SportPhone
            ,FeedDtac
            ,MTUTD
            ,KissModel
            ,iSoccer
            ,CheerBallThai
        }
            public static string GETIP()
            {
                return (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null || HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == "") ?
                    HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }

    }

    public class AppCode_ApplicationVersion
    {
        public string Applicationversion;
    }
}
