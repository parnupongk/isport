using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace isport_edt
{
    public class AppMain
    {
        public static string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["IsportUIConnectionString"].ToString();
        public static string strConnPack = System.Configuration.ConfigurationManager.ConnectionStrings["IsportUIPackConnectionString"].ToString();
        public static string strConnOracle = System.Configuration.ConfigurationManager.ConnectionStrings["IsportUIOracleConnectionString"].ToString();
        //public static string strConnFeed = System.Configuration.ConfigurationManager.ConnectionStrings["IsportUIFeedConnectionString"].ToString();

        public enum ProjectType
        {
            bb
            ,isport
        }
        public enum OperatorType
        {
            All
            ,Ais
            ,Dtac
            ,True
        }
        public enum MasterType
        {
            Header
            ,UI
            ,Footer
        }
        public enum ImagesType
        {
            icon
            ,images
        }
    }
}
