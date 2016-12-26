using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLibrary;
namespace isport
{
    public class AppSendEmail
    {
        public void SendEmail(string mailTo,string subject,string body)
        {
            GMailer.GmailUsername = "isport2011.app@gmail.com";
            GMailer.GmailPassword = "isport@2011";

            GMailer mailer = new GMailer();
            mailer.ToEmail = mailTo;//"parnupongk@gmail.com";
            mailer.Subject = subject;//"Error BackOfficre";
            mailer.Body = body;//"Thanks for Registering your account.<br> please verify your email id by clicking the link <br> <a href='youraccount.com/verifycode=12323232'>verify</a>";
            mailer.IsHtml = true;
            mailer.Send();
        }
    }
}
