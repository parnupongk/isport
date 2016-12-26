using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace isport_kissmodel
{
    public class AppCodeKiss
    {
        public static string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["IsportUIConnectionString"].ToString();
        public static string strConnPack = System.Configuration.ConfigurationManager.ConnectionStrings["IsportUIPackConnectionString"].ToString();
        public static string strConnOracle = System.Configuration.ConfigurationManager.ConnectionStrings["IsportUIOracleConnectionString"].ToString();
        public static string strConnFeed = System.Configuration.ConfigurationManager.ConnectionStrings["IsportUIFeedConnectionString"].ToString();
    }
}