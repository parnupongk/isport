using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace isportclip
{
    public class AppMain
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["IsportConnectionString"].ToString();
        public static string strConnIsportPack = ConfigurationManager.ConnectionStrings["IsportPackConnectionString"].ToString();
        public static string strConnOracle = ConfigurationManager.ConnectionStrings["IsportOracleConnectionString"].ToString();

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
