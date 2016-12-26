using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Configuration;
using OracleDataAccress;
using Microsoft.ApplicationBlocks.Data;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

namespace WS_BB
{

    /// <summary>
    /// Summary description for isport
    /// </summary>
    [WebService(Namespace = "http://wap.isport.co.th/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class isport : System.Web.Services.WebService
    {
        public static string strConn = AppCode_Base.strConn;

        [WebMethod(Description=" GetNews ")]
        public AppCode_News.Out_News SIP_Content_GetNews(string pcatId, string code)
        {

            return new AppCode_News().CommandGetNews(pcatId);
        }

        //==========================================
        //      Live Score
        //==========================================
        #region
        [WebMethod(Description = " ผลสด ")]
        public AppCode_LiveScore.Out_LiveScore GetLiveScore(string strMenu,string scsId,string code)
        {
            return new AppCode_LiveScore().CommandLiveScore(strMenu,scsId,false);
        }
        [WebMethod(Description = " สรุปผลบอล ")]
        public AppCode_LiveScore.Out_LiveScore GetLastScore(string strMenu, string scsId, string code)
        {
            return new AppCode_LiveScore().CommandLiveScore(strMenu, scsId, true);
        }
        [WebMethod(Description = " รายละเอียดผลบอล ")]
        public AppCode_LiveScore.Out_LiveScore GetLiveScoreResult(string mschId,string code)
        {
            return new AppCode_LiveScore().CommandLiveScoreResult(mschId);
        }
        #endregion
        //==========================================
        //      End Live Score
        //==========================================

        //==========================================
        //      program analysis
        //==========================================
        #region
        [WebMethod(Description = "ข้อมูลทั่วไป , เวลา , ช่องถ่ายถอด , Handicap , อัตราพลูเฉลี่ย,มองอย่างเซียน")]
        public AppCode_FootballAnalysis.Out_Analysis GetFootballAnalysisDetail(string mschId,string code)
        {
            AppCode_FootballAnalysis.Out_Analysis outAnalysis = new AppCode_FootballAnalysis.Out_Analysis();
            try
            {
                outAnalysis.analysis = new AppCode_FootballAnalysis().CommandGetFootballAnalysisDetail(mschId);
            }
            catch(Exception ex)
            {
                outAnalysis.status = "Error";
                outAnalysis.errorMess = ex.Message;
            }
            
            return outAnalysis;
        }
        [WebMethod(Description = "5 นัดหลังสุดของ 2 ทีม")]
        public AppCode_FootballAnalysis.Out_Analysis GetFootballAnalysisTop5Last(string teamCode1, string teamCode2,string code)
        {
            AppCode_FootballAnalysis.Out_Analysis outAnalysis = new AppCode_FootballAnalysis.Out_Analysis();
            try
            {
                outAnalysis.analysis = new AppCode_FootballAnalysis().CommandGetFootballAnalysisTop5Last(teamCode1, teamCode2);;
            }
            catch (Exception ex)
            {
                outAnalysis.status = "Error";
                outAnalysis.errorMess = ex.Message;
            }

            return outAnalysis;
        }
        [WebMethod(Description = " 5 นัดหลังสุดที่ทั้งสองทีมพบกัน  ")]
        public AppCode_FootballAnalysis.Out_Analysis GetFootballAnalysisTop5VS(string teamCode1, string teamCode2, string code)
        {
            AppCode_FootballAnalysis.Out_Analysis outAnalysis = new AppCode_FootballAnalysis.Out_Analysis();
            try
            {
                outAnalysis.analysis = new AppCode_FootballAnalysis().CommandGetFootballAnalysisTop5VS(teamCode1, teamCode2);
            }
            catch (Exception ex)
            {
                outAnalysis.status = "Error";
                outAnalysis.errorMess = ex.Message;
            }

            return outAnalysis;
            
        }
        [WebMethod(Description = " สำหรับ Get อันดับผลงานในลีกของ 2 ทีม ")]
        public AppCode_FootballAnalysis.Out_Analysis GetFootballAnalysislevel(string scsId, string teamCode1, string teamCode2, string code)
        {
            AppCode_FootballAnalysis.Out_Analysis outAnalysis = new AppCode_FootballAnalysis.Out_Analysis();
            try
            {
                outAnalysis.analysis = new AppCode_FootballAnalysis().CommandGetFootballAnalysislevel(scsId, teamCode1, teamCode2);
            }
            catch (Exception ex)
            {
                outAnalysis.status = "Error";
                outAnalysis.errorMess = ex.Message;
            }

            return outAnalysis;
        }

        [WebMethod(Description = " สำหรับ Get ข้อมูลของของหน้า วิเคราะห์,มองอย่างเซียน (iPhone) ")]
        public AppCode_FootballAnalysis.Out_Analysis GetFootballAnalysis(string mschId, string code)
        {
            AppCode_FootballAnalysis.Out_Analysis outAnalysis = new AppCode_FootballAnalysis.Out_Analysis();
            try
            {
                outAnalysis.matchDetail = new AppCode_FootballAnalysis().CommandGetFootballAnalysisDetail_IPhone(mschId);
                if (outAnalysis.matchDetail.Count > 0)
                {
                    outAnalysis.matchLevel = 
                        new AppCode_FootballAnalysis().CommandGetFootballAnalysislevel_IPhone(
                        outAnalysis.matchDetail[0].scsId, outAnalysis.matchDetail[0].teamCode1, outAnalysis.matchDetail[0].teamCode2);

                    outAnalysis.matchTop5Last =
                        new AppCode_FootballAnalysis().CommandGetFootballAnalysisTop5Last_IPhone(
                        outAnalysis.matchDetail[0].teamCode1, outAnalysis.matchDetail[0].teamCode2);

                    outAnalysis.matchTop5VS =
                        new AppCode_FootballAnalysis().CommandGetFootballAnalysisTop5VS_IPhone(
                        outAnalysis.matchDetail[0].teamCode1, outAnalysis.matchDetail[0].teamCode2);

                    outAnalysis.status = "Success";
                    outAnalysis.errorMess = "";
                }
            }
            catch (Exception ex)
            {
                outAnalysis.status = "Error";
                outAnalysis.errorMess = ex.Message;
            }

            return outAnalysis;
        }
        #endregion
        //==========================================
        //      end program analysis
        //==========================================


        //==========================================
        //      Check Active
        //==========================================
        #region
        [WebMethod(Description = " Check Subscribe By Code(PIN,UID)")]
        public AppCode_Subscribe.Out_CheckActive GetCheckActive(string codeType, string code, string packageId)
        {
            return new AppCode_Subscribe().CommandGetCheckActive(codeType, code, packageId);
        }

        [WebMethod(Description = " Check Member")]
        public AppCode_Subscribe.Out_CheckActive GetCheckMember(string codeType, string code)
        {
            return new AppCode_Subscribe().CommandGetCheckMember(codeType, code);

        }
        #endregion
        //==========================================
        //      End Check Active
        //==========================================

        //==========================================
        //      SMS service
        //==========================================
        #region
        [WebMethod(Description = " Get SMS Service ")]
        public AppCode_Sms.Out_Sms GetSMSService( string code)
        {
            return new AppCode_Sms().CommandGetSmsService();
        }

        [WebMethod(Description = " Subscribe SMS")]
        public AppCode_Subscribe.Out_CheckActive SubscribePackage(string strMSISDN, string strOpt, string strPssvID
            , string strAction, string strPromo, string strMpCode, string code)
        {

            //    new AppCode_Subscribe().Insert_Sipsendtrans_SportApp(strMSISDN, strOpt, strPssvID
            //, strAction, strPromo, strMpCode);
                return new AppCode_Subscribe().CommandSubscribePackage(strMSISDN, strOpt, strPssvID
                , strAction, strPromo, strMpCode);

            
        }
        [WebMethod(Description = " Active Service SMS")]
        public AppCode_Subscribe.Out_CheckActive ActiveService(string strMSISDN, string strPackage, string strOpt, string code)
        {

            return new AppCode_Subscribe().CommandActiveService(strMSISDN, strPackage,strOpt);


        }
        #endregion
        //==========================================
        //      End SMS service
        //==========================================


        [WebMethod(Description = " สำหรับ Get all class ")]
        public AppCode_FootballList.Out_FootballList GetFootBallClass( string code)
        {
            return new AppCode_FootballList().CommandGetFootBallClass();
        }

        [WebMethod(Description = " สำหรับ Get class Info ")]
        public AppCode_FootballList.Out_FootballList GetFootBallClassByClassID(string classId, string code)
        {
            return new AppCode_FootballList().CommandGetFootBallClassByClassID(classId);
        }

        [WebMethod(Description = " สำหรับ Get ตารางการแข่งขัน ")]
        public AppCode_FootballProgram.Out_Program GetFootballProgram(string scsId, string code)
        {
            return new AppCode_FootballProgram().CommandGetFootballProgram(scsId);
        }

        [WebMethod(Description = " สำหรับ Get ตารางคะแนน ")]
        public AppCode_FootballAnalysis.Out_Analysis GetFootballTableByScsId(string scsId, string code)
        {
            AppCode_FootballAnalysis.Out_Analysis outAnalysis = new AppCode_FootballAnalysis.Out_Analysis();
            try
            {
                outAnalysis.matchLevel = new AppCode_FootballAnalysis().CommandGetFootballAnalysislevelByScsId(scsId) ;
            }
            catch (Exception ex)
            {
                outAnalysis.status = "Error";
                outAnalysis.errorMess = ex.Message;
            }

            return outAnalysis;
        }

        [WebMethod(Description = " สำหรับ Get อันดับดาวซัลโว ")]
        public AppCode_FootballList.Out_FootballList GetFootballTopscore(string scsId, string code)
        {
                return  new AppCode_FootballList().CommandGetFootBallTopScore(scsId);
            
        }

        [WebMethod(Description = " สำหรับ Get Blackberry Application Version ")]
        public AppCode_ApplicationVersion GetBlackberryAppVersion( string code)
        {
            AppCode_ApplicationVersion appVersion = new AppCode_ApplicationVersion();
            appVersion.Applicationversion = ConfigurationManager.AppSettings["ApplicationVersion"].ToString();
            return appVersion;
        }
    }
}
