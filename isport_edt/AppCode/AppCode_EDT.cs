using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Data;
using System.Data.OracleClient;
using OracleDataAccress;
namespace isport_edt
{
    [DataContract]
    public class AppCode_EDT
    {
        #region Data
        [DataMember(Name = "type", IsRequired = false)]
        public string type { get; set; }

        [DataMember(Name = "entry_id", IsRequired = false)]
        public string entry_id { get; set; }

        [DataMember(Name = "name", IsRequired = false)]
        public string name { get; set; }

        [DataMember(Name = "detail", IsRequired = false)]
        public string detail { get; set; }

        [DataMember(Name = "business", IsRequired = false)]
        public string business { get; set; }

        [DataMember(Name = "address", IsRequired = false)]
        public string address { get; set; }

        [DataMember(Name = "lat", IsRequired = false)]
        public string lat { get; set; }

        [DataMember(Name = "lng", IsRequired = false)]
        public string lng { get; set; }

        [DataMember(Name = "tel", IsRequired = false)]
        public string tel { get; set; }

        [DataMember(Name = "web", IsRequired = false)]
        public string web { get; set; }

        [DataMember(Name = "transport", IsRequired = false)]
        public string transport { get; set; }

        [DataMember(Name = "location", IsRequired = false)]
        public string location { get; set; }

        [DataMember(Name = "opentime", IsRequired = false)]
        public string opentime { get; set; }

        [DataMember(Name = "image_cover_thumb", IsRequired = false)]
        public string image_cover_thumb { get; set; }

        [DataMember(Name = "image_cover", IsRequired = false)]
        public string image_cover { get; set; }

        //==== Eat + drink =========
        [DataMember(Name = "menu_guide", IsRequired = false)]
        public string menu_guide { get; set; }

        [DataMember(Name = "type_of_food", IsRequired = false)]
        public string type_fo_food { get; set; }

        [DataMember(Name = "national_food", IsRequired = false)]
        public string national_food { get; set; }

        [DataMember(Name = "other_services", IsRequired = false)]
        public string other_services { get; set; }

        [DataMember(Name = "type_of_music", IsRequired = false)]
        public string type_of_music { get; set; }

        [DataMember(Name = "no_of_table", IsRequired = false)]
        public string no_of_table { get; set; }

        [DataMember(Name = "alcohol", IsRequired = false)]
        public string alcohol { get; set; }

        [DataMember(Name = "corkage_fee", IsRequired = false)]
        public string corkage_fee { get; set; }

        [DataMember(Name = "payment", IsRequired = false)]
        public string payment { get; set; }

        //====== Travel ==========
        [DataMember(Name = "rooms", IsRequired = false)]
        public string rooms { get; set; }

        [DataMember(Name = "facilities", IsRequired = false)]
        public string facilities { get; set; }

        [DataMember(Name = "lowest_rate", IsRequired = false)]
        public string lowest_rate { get; set; }

        [DataMember(Name = "highest_rate", IsRequired = false)]
        public string hightest_rate { get; set; }

        [DataMember(Name = "purpose", IsRequired = false)]
        public string purpose { get; set; }

        [DataMember(Name = "travel_duration", IsRequired = false)]
        public string travel_duration { get; set; }

        [DataMember(Name = "local_food", IsRequired = false)]
        public string local_food { get; set; }

        [DataMember(Name = "souvenir", IsRequired = false)]
        public string souvenir { get; set; }

        [DataMember(Name = "gallery_thumb", IsRequired = false)]
        public List<string> gallery_thumb { get; set; }

        [DataMember(Name = "gallery", IsRequired = false)]
        public List<string> gallery { get; set; }
        #endregion
    }

    public class AppCode_EDT_Conn : AppMain
    {
        public DataSet CommandGetEDTBySMSId(string smsId)
        {
            try
            {
                return OrclHelper.Fill(strConnOracle, CommandType.StoredProcedure, "Wap_UI.EDT_SELECTBY_SMSID", "edt_xml"
                                                        , new OracleParameter[] {OrclHelper.GetOracleParameter("p_sms_id",smsId,OracleType.VarChar,ParameterDirection.Input)
                                                                                            ,OrclHelper.GetOracleParameter("o_Content","",OracleType.Cursor,ParameterDirection.Output)});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
