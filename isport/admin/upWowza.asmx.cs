using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Tamir.SharpSsh;
using System.Configuration;
using WebLibrary;
namespace isport.admin
{
    /// <summary>
    /// Summary description for upWowza
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class upWowza : System.Web.Services.WebService
    {

        [WebMethod]
        public string WowZaUpload(string filePath, string fileName)
        {
            SshTransferProtocolBase sshCp;
            sshCp = new Sftp(ConfigurationManager.AppSettings["Isport_SFTP_Host"], ConfigurationManager.AppSettings["Isport_SFTP_User"]);
            sshCp.Password = ConfigurationManager.AppSettings["Isport_SFTP_Password"];
            try
            {
                //filUpload.PostedFile.SaveAs(fullPath);
                sshCp.Connect();
                ExceptionManager.WriteError("Upload file " + filePath + "/" + fileName);
                sshCp.Put(filePath, ConfigurationManager.AppSettings["Isport_PathStreamimg"] + fileName);
                //ExceptionManager.WriteError("upload success");
                sshCp.Close();
                new System.IO.FileInfo(filePath).Delete();

                return "success";
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message);
                sshCp.Close();
                return "error:" + ex.Message;
            }
        }
    }
}
