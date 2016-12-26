using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using WebLibrary;
using System.Configuration;
using DevExpress.Web.ASPxGridView;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using OracleDataAccress;
using System.Data.OracleClient;

namespace isport.admin
{
    public partial class adminedtreloadapi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            if (txtEdtId.Text != "")
            {
                string rtn = "";
                string[] ids = txtEdtId.Text.Split(',');
                foreach (string id in ids)
                {
                    try {
                        GenXML_EDTData(string.Format(ConfigurationManager.AppSettings["Isport_EDT_GetDATA"], id));
                    }
                    catch(Exception ex)
                    {
                        rtn += ex.Message + ";<br/>";
                    }
                }
                if( rtn != "" )
                {
                    lblResult.Text = rtn;
                }
            }

        }

        private void GenXML_EDTData(string url)
        {
            try
            {
                string xml = new SendService.sendpost().SendGet(url);
                //xml = xml.Replace("[","");
                //xml = xml.Replace("]","");
                //xml = "[{\"type\":\"eat\",\"entry_id\":\"407797\",\"name\":\"\\u0e23\\u0e49\\u0e32\\u0e19\\u0e2d\\u0e32\\u0e2b\\u0e32\\u0e23\\u0e0d\\u0e35\\u0e48\\u0e1b\\u0e38\\u0e48\\u0e19 \\u0e04\\u0e34\\u0e19\\u0e19\\u0e34\\u0e08\\u0e34\",\"detail\":\"Kinniji Japanese Restaurant \\u0e23\\u0e49\\u0e32\\u0e19\\u0e2d\\u0e32\\u0e2b\\u0e32\\u0e23\\u0e0d\\u0e35\\u0e48\\u0e1b\\u0e38\\u0e48\\u0e19\\u0e1a\\u0e23\\u0e23\\u0e22\\u0e32\\u0e01\\u0e32\\u0e28\\u0e2a\\u0e1a\\u0e32\\u0e22\\u0e46 \\u0e2a\\u0e44\\u0e15\\u0e25\\u0e4c\\u0e01\\u0e32\\u0e23\\u0e15\\u0e01\\u0e41\\u0e15\\u0e48\\u0e07\\u0e23\\u0e49\\u0e32\\u0e19\\u0e40\\u0e23\\u0e35\\u0e22\\u0e1a\\u0e07\\u0e48\\u0e32\\u0e22 \\u0e40\\u0e19\\u0e49\\u0e19\\u0e42\\u0e17\\u0e19\\u0e2a\\u0e35\\u0e40\\u0e2b\\u0e25\\u0e37\\u0e2d\\u0e07\\u0e19\\u0e27\\u0e25\\u0e2a\\u0e30\\u0e2d\\u0e32\\u0e14\\u0e15\\u0e32 \\u0e40\\u0e08\\u0e49\\u0e32\\u0e02\\u0e2d\\u0e07\\u0e23\\u0e49\\u0e32\\u0e19\\u0e04\\u0e37\\u0e2d \\u0e04\\u0e38\\u0e13\\u0e1e\\u0e35\\u0e17 \\u0e40\\u0e08\\u0e49\\u0e32\\u0e02\\u0e2d\\u0e07 Blocger \\u0e40\\u0e01\\u0e35\\u0e48\\u0e22\\u0e27\\u0e01\\u0e31\\u0e1a\\u0e23\\u0e49\\u0e32\\u0e19\\u0e2d\\u0e32\\u0e2b\\u0e32\\u0e23 &quot;\\u0e01\\u0e34\\u0e19\\u0e01\\u0e31\\u0e1a\\u0e1e\\u0e35\\u0e17&quot; \\u0e19\\u0e2d\\u0e01\\u0e08\\u0e32\\u0e01\\u0e40\\u0e21\\u0e19\\u0e39\\u0e1b\\u0e23\\u0e30\\u0e08\\u0e33\\u0e2d\\u0e22\\u0e48\\u0e32\\u0e07 \\u0e02\\u0e49\\u0e32\\u0e27\\u0e2b\\u0e19\\u0e49\\u0e32\\u0e41\\u0e0b\\u0e25\\u0e21\\u0e2d\\u0e19\\u0e23\\u0e21\\u0e04\\u0e27\\u0e31\\u0e19 \\u0e41\\u0e25\\u0e30\\u0e2a\\u0e25\\u0e31\\u0e14\\u0e15\\u0e48\\u0e32\\u0e07\\u0e46 \\u0e41\\u0e25\\u0e49\\u0e27 \\u0e17\\u0e32\\u0e07\\u0e23\\u0e49\\u0e32\\u0e19\\u0e08\\u0e30\\u0e21\\u0e35\\u0e01\\u0e32\\u0e23\\u0e04\\u0e34\\u0e14\\u0e04\\u0e49\\u0e19\\u0e40\\u0e21\\u0e19\\u0e39\\u0e43\\u0e2b\\u0e21\\u0e48\\u0e46 \\u0e2d\\u0e22\\u0e39\\u0e48\\u0e40\\u0e2a\\u0e21\\u0e2d \\u0e2d\\u0e32\\u0e17\\u0e34 \\u0e02\\u0e49\\u0e32\\u0e27\\u0e2b\\u0e19\\u0e49\\u0e32\\u0e41\\u0e0b\\u0e25\\u0e21\\u0e2d\\u0e19\\u0e23\\u0e21\\u0e04\\u0e27\\u0e31\\u0e19, \\u0e02\\u0e49\\u0e32\\u0e27\\u0e2b\\u0e19\\u0e49\\u0e32\\u0e04\\u0e34\\u0e19\\u0e19\\u0e34\\u0e08\\u0e34\\u0e14\\u0e49\\u0e07\\u0e08\\u0e32\\u0e19\\u0e22\\u0e31\\u0e01\\u0e29\\u0e4c 7 \\u0e2b\\u0e19\\u0e49\\u0e32, \\u0e02\\u0e49\\u0e32\\u0e27\\u0e2b\\u0e19\\u0e49\\u0e32\\u0e2b\\u0e21\\u0e39\\u0e0a\\u0e32\\u0e0a\\u0e39 \\u0e41\\u0e25\\u0e30\\u0e14\\u0e49\\u0e27\\u0e22\\u0e1b\\u0e23\\u0e30\\u0e2a\\u0e1a\\u0e01\\u0e32\\u0e23\\u0e13\\u0e4c\\u0e41\\u0e25\\u0e30\\u0e04\\u0e27\\u0e32\\u0e21\\u0e0a\\u0e2d\\u0e1a\\u0e43\\u0e19\\u0e01\\u0e32\\u0e23\\u0e17\\u0e32\\u0e19\\u0e2d\\u0e32\\u0e2b\\u0e32\\u0e23\\u0e02\\u0e2d\\u0e07\\u0e40\\u0e08\\u0e49\\u0e32\\u0e02\\u0e2d\\u0e07\\u0e23\\u0e49\\u0e32\\u0e19 \\u0e40\\u0e21\\u0e19\\u0e39\\u0e15\\u0e48\\u0e32\\u0e07\\u0e46 \\u0e08\\u0e36\\u0e07\\u0e15\\u0e2d\\u0e1a\\u0e42\\u0e08\\u0e17\\u0e22\\u0e4c\\u0e25\\u0e39\\u0e01\\u0e04\\u0e49\\u0e32\\u0e17\\u0e31\\u0e49\\u0e07\\u0e01\\u0e25\\u0e38\\u0e48\\u0e21\\u0e27\\u0e31\\u0e22\\u0e23\\u0e38\\u0e48\\u0e19 \\u0e04\\u0e19\\u0e17\\u0e33\\u0e07\\u0e32\\u0e19 \\u0e44\\u0e1b\\u0e08\\u0e19\\u0e16\\u0e36\\u0e07\\u0e04\\u0e23\\u0e2d\\u0e1a\\u0e04\\u0e23\\u0e31\\u0e27\",\"business\":\"\\u0e23\\u0e49\\u0e32\\u0e19\\u0e2d\\u0e32\\u0e2b\\u0e32\\u0e23\",\"address\":\"198 \\u0e0b\\u0e2d\\u0e22\\u0e08\\u0e38\\u0e2c\\u0e32 42 \\u0e2d\\u0e32\\u0e04\\u0e32\\u0e23 \\u0e42\\u0e04\\u0e23\\u0e07\\u0e01\\u0e32\\u0e23 U Center \\u0e16\\u0e19\\u0e19\\u0e1e\\u0e0d\\u0e32\\u0e44\\u0e17 \\u0e41\\u0e02\\u0e27\\u0e07\\u0e27\\u0e31\\u0e07\\u0e43\\u0e2b\\u0e21\\u0e48 \\u0e40\\u0e02\\u0e15\\u0e1b\\u0e17\\u0e38\\u0e21\\u0e27\\u0e31\\u0e19 \\u0e01\\u0e23\\u0e38\\u0e07\\u0e40\\u0e17\\u0e1e\\u0e2f 10330\",\"lat\":\"13.7349005522078\",\"lng\":\"100.528259962396\",\"tel\":\"0899276922\",\"web\":\"https:\\/\\/www.facebook.com\\/kinniji\",\"transport\":\"\\u0e08\\u0e32\\u0e01\\u0e1b\\u0e17\\u0e38\\u0e21\\u0e27\\u0e31\\u0e19 \\u0e21\\u0e38\\u0e48\\u0e07\\u0e2b\\u0e19\\u0e49\\u0e32\\u0e15\\u0e23\\u0e07\\u0e44\\u0e1b\\u0e22\\u0e31\\u0e07\\u0e16\\u0e19\\u0e19 \\u0e1e\\u0e0d\\u0e32\\u0e44\\u0e17 \\u0e41\\u0e25\\u0e49\\u0e27\\u0e40\\u0e25\\u0e35\\u0e49\\u0e22\\u0e27\\u0e40\\u0e02\\u0e49\\u0e32\\u0e08\\u0e38\\u0e2c\\u0e32 42 \\u0e1b\\u0e23\\u0e30\\u0e21\\u0e32\\u0e13 500 \\u0e40\\u0e21\\u0e15\\u0e23 \\u0e01\\u0e47\\u0e08\\u0e30\\u0e1e\\u0e1a\\u0e23\\u0e49\\u0e32\\u0e19\\u0e2d\\u0e32\\u0e2b\\u0e32\\u0e23\\u0e0d\\u0e35\\u0e48\\u0e1b\\u0e38\\u0e48\\u0e19 \\u0e04\\u0e34\\u0e19\\u0e19\\u0e34\\u0e08\\u0e34 \\u0e0b\\u0e36\\u0e48\\u0e07\\u0e15\\u0e31\\u0e49\\u0e07\\u0e2d\\u0e22\\u0e39\\u0e48\\u0e20\\u0e32\\u0e22\\u0e43\\u0e19\\u0e42\\u0e04\\u0e23\\u0e07\\u0e01\\u0e32\\u0e23 U Center \\u0e15\\u0e23\\u0e07\\u0e02\\u0e49\\u0e32\\u0e21\\u0e15\\u0e36\\u0e01\\u0e08\\u0e32\\u0e21\\u0e08\\u0e38\\u0e23\\u0e35\\u0e2a\\u0e41\\u0e04\\u0e27\\u0e23\\u0e4c\",\"location\":\"\",\"opentime\":\"\\u0e40\\u0e1b\\u0e34\\u0e14\\u0e17\\u0e38\\u0e01\\u0e27\\u0e31\\u0e19   \\u0e40\\u0e27\\u0e25\\u0e32 11.00 - 21.00 \\u0e19.\",\"image_cover_thumb\":\"http:\\/\\/ed.files-media.com\\/ud\\/images\\/1\\/136\\/407797\\/LUM_3562_Cover-100x70.jpg\",\"image_cover\":\"http:\\/\\/ed.files-media.com\\/ud\\/images\\/1\\/136\\/407797\\/LUM_3562_Cover-620x392.jpg\",\"gallery_thumb\":[\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3562_Cover-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3505-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3498-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3598-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3521-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3495-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3603-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3595-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3591-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3601-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3599-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3585-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3570-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3576-65x65.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3573-65x65.jpg\"],\"gallery\":[\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3562_Cover-620x392.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3505-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3498-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3598-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3521-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3495-400x599.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3603-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3595-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3591-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3601-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3599-400x599.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3585-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3570-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3576-599x400.jpg\",\"http:\\/\\/ed.files-media.com\\/ud\\/gal\\/dcp\\/29\\/84556\\/LUM_3573-599x400.jpg\"],\"menu_guide\":\"\\u0e02\\u0e49\\u0e32\\u0e27\\u0e2b\\u0e19\\u0e49\\u0e32\\u0e41\\u0e0b\\u0e25\\u0e21\\u0e2d\\u0e19\\u0e23\\u0e21\\u0e04\\u0e27\\u0e31\\u0e19, \\u0e2a\\u0e25\\u0e31\\u0e14 Mix Topping, \\u0e22\\u0e33\\u0e41\\u0e0b\\u0e25\\u0e21\\u0e2d\\u0e19\\u0e0b\\u0e32\\u0e0b\\u0e34\\u0e21\\u0e34\",\"type_of_food\":\"\",\"national_food\":\"\\u0e2d\\u0e32\\u0e2b\\u0e32\\u0e23\\u0e0d\\u0e35\\u0e48\\u0e1b\\u0e38\\u0e48\\u0e19\",\"other_services\":\"\",\"type_of_music\":\"\",\"no_of_table\":\"\\u0e19\\u0e49\\u0e2d\\u0e22\\u0e01\\u0e27\\u0e48\\u0e32 20 \\u0e42\\u0e15\\u0e4a\\u0e30\",\"alcohol\":\"no\",\"corkage_fee\":\"no\",\"payment\":\"\"}]";
                //EDT edt= JsonHelper.JsonDeserialize<EDT>(xml);
                List<AppCode_EDT> myDeserializedObjList = (List<AppCode_EDT>)Newtonsoft.Json.JsonConvert.DeserializeObject(xml, typeof(List<AppCode_EDT>));
                //AppCode_EDT edt = Newtonsoft.Json.JsonConvert.DeserializeObject<AppCode_EDT>(xml);

                foreach (AppCode_EDT edt in myDeserializedObjList)
                {
                    xml = Newtonsoft.Json.JsonConvert.SerializeObject(edt, Newtonsoft.Json.Formatting.Indented);
                    //SaveAsXmlToFile(xml, edt.entry_id+".json");
                    using (FileStream fs = File.Open(string.Format(ConfigurationManager.AppSettings["Isport_EDT_FileUpload"], new string[] { edt.entry_id + ".json" }), FileMode.Create))
                    using (StreamWriter sw = new StreamWriter(fs))
                    using (Newtonsoft.Json.JsonWriter jw = new Newtonsoft.Json.JsonTextWriter(sw))
                    {
                        jw.Formatting = Newtonsoft.Json.Formatting.Indented;
                        Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                        serializer.Serialize(jw, edt);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("GenXML_EDTData >> " + ex.Message);
            }
        }
    }
}