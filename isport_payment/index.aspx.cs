using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mobile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Security.Permissions;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using WebLibrary;
namespace isport_payment
{
    public partial class index : System.Web.UI.MobileControls.MobilePage
    {
        string DigitalCertificateName = "";
        public static RSACryptoServiceProvider rsa;

        public string GetEncryptedText(string PlainStringToEncrypt)
        {
            X509Store store = new X509Store(StoreName.My);
            X509Certificate2 x509_2 = null;
            store.Open(OpenFlags.ReadWrite);
            if (DigitalCertificateName.Length > 0)
            {
                foreach (X509Certificate2 cert in store.Certificates)
                {
                    if (cert.SubjectName.Name.Contains(DigitalCertificateName))
                    {
                        x509_2 = cert;
                        break;
                    }
                }

                if (x509_2 == null)
                    throw new Exception("No Certificate could be found in name " + DigitalCertificateName);
            }
            else
            {
                x509_2 = store.Certificates[0];
            }

            try
            {
                StreamReader reader = new StreamReader(@"wap_id_rsa_public.xml");
                string publicOnlyKeyXML = reader.ReadToEnd();
                reader.Close();


                string PlainString = PlainStringToEncrypt.Trim();
                byte[] cipherbytes = ASCIIEncoding.ASCII.GetBytes(PlainString);
                
                
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509_2.PublicKey.Key;
                //rsa.FromXmlString(publicOnlyKeyXML);
                byte[] cipher = rsa.Encrypt(cipherbytes, false);
                return Convert.ToBase64String(cipher);
            }
            catch (Exception e)
            {
                //Hadle exception
                throw e;
            }

        }//Method ends here

        /// <summary>
        /// To Decrypt clear text using RSACryptoServer Provider and Digital Certificate having Private Key.
        /// </summary>
        /// <param name="EncryptedStringToDecrypt"></param>
        /// <returns></returns>
        public string GetDecryptedText(string EncryptedStringToDecrypt)
        {
            X509Store store = new X509Store(StoreName.My);
            X509Certificate2 x509_2 = null;
            store.Open(OpenFlags.ReadWrite);
            if (DigitalCertificateName.Length > 0)
            {
                foreach (X509Certificate2 cert in store.Certificates)
                {
                    if (cert.SubjectName.Name.Contains(DigitalCertificateName))
                    {
                        x509_2 = cert;
                        break;
                    }
                }
                if (x509_2 == null)
                    throw new Exception("No Certificate could be found in name " + DigitalCertificateName);
            }
            else
            {
                x509_2 = store.Certificates[0];
            }

            try
            {
                byte[] cipherbytes = Convert.FromBase64String(EncryptedStringToDecrypt);
                if (x509_2.HasPrivateKey)
                {
                    
                    StreamReader reader = new StreamReader(@"wap_id_rsa_public.xml");
                    string publicOnlyKeyXML = reader.ReadToEnd();
                    reader.Close();
                    RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509_2.PrivateKey;
                    //rsa.FromXmlString(publicOnlyKeyXML);
                    byte[] plainbytes = rsa.Decrypt(cipherbytes, false);
                    System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                    return enc.GetString(plainbytes);
                }
                else
                {
                    throw new Exception("Certificate used for has no private key.");
                }
            }
            catch (Exception e)
            {
                //Hadle exception
                throw e;
            }
        }//method ends here


        protected void Page_Load(object sender, EventArgs e)
        {
            #region command
            string command = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "utf-8" + (char)34 + "?>";
            command += "<cpa-request-aoc>";
            command += "<authentication>";
            //command += "<user>TrOps</user>";
            //command += "<password>TqPZSp894</password>";
            command += "<user>Isport511</user>";
            command += "<password>Y466FJXit</password>";
            command += "</authentication>";
            command += "<unregistration>";
            command += "<productid>45110530001</productid>";
            command += "<msisdn>66877998335</msisdn>";
            command += "<cp-ref-id>9f820f32-197e-4963-8982-83a348b0ddab</cp-ref-id>";
            command += "<mobile-model>IQ2</mobile-model>";
            command += "<timestamp>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</timestamp>";
            command += "</unregistration>";
            command += "</cpa-request-aoc>";
            #endregion

            string input = "http://wap.isport.co.th/isportui/index.aspx?p=bb&pri=1";
            UnicodeEncoding ByteConverter = new UnicodeEncoding();

            //Create byte arrays to hold original, encrypted, and decrypted data.
            byte[] dataToEncrypt = ByteConverter.GetBytes(input);
            byte[] encryptedData;
            byte[] decryptedData;

            //Create a new instance of RSACryptoServiceProvider to generate
            //public and private key data.

            //RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            //Pass the data to ENCRYPT, the public key information 
            //(using RSACryptoServiceProvider.ExportParameters(false),
            //and a boolean flag specifying no OAEP padding.
            //encryptedData = EncrptHelper.RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);

            //string encry = EncrptHelper.EncryptString(input, 1, "<RSAKeyValue>	<Modulus>1RVuuN7UkLGmh5Gjr3bQr0ck919eJykUl7wsQqvYlPGAhK+ZpGHlLwtj+9xGAruANLQmNaJY1j4Ln837OfIYva9VAuD99CuVtw6BykXh0+zcoOddCh6mn6HczfcnI4vL06pDLaqlkJiPsr7NwsRK1rCObPCxKXsrr0p4n759u2U=</Modulus><Exponent>Iw==</Exponent></RSAKeyValue>");
            //AssignNewKey();
            //string encry = GetEncryptedText(input);
            //string encry = EncryptData(input);
            string hex = "";
            //foreach (char letter in encry)
            //{
                // Get the integral value of the character.
                //int value = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form.
                //string hexOutput = String.Format("{0:X}", value);
                //hex += hexOutput;
                //Console.WriteLine("Hexadecimal value of {0} is {1}", letter, hexOutput);
            //}
            //string decry = GetDecryptedText(encry);
            //Pass the data to DECRYPT, the private key information 
            //(using RSACryptoServiceProvider.ExportParameters(true),
            //and a boolean flag specifying no OAEP padding.
            //decryptedData = EncrptHelper.RSADecrypt(encryptedData, RSA.ExportParameters(true), false);

            //string ss = ByteConverter.GetString(decryptedData); 

            //string rtn = new sendpost().SendPost("http://sdpapi.dtac.co.th/SAG/services/cpa/aoc/unregistration", command);
            //LogsManager.WriteLogs("Test_SDP", rtn, command);
            //int ss = AppCode.CheckSessionPayment("66818593778", "01", "142", "", "01");
            Response.Write("payment session :" + hex);
        }
    }
}