using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
namespace WS_BB
{

    [DataContract]
    public class AppCode_KissModels
    {
        [DataMember(Name = "AppCode_KissModels", IsRequired = false )]
        public List<AppCode_KissModel> kissContent { get; set; }

        //[DataMember(Name = "AppCode_KissModels_Media", IsRequired = false)]
        //public List<AppCode_KissModel_Media> kissMedia { get; set; }

        [DataMember(Name = "errStatus", IsRequired = false)]
        public string errStatus { get; set; }

        [DataMember(Name = "errMessage", IsRequired = false)]
        public string errMessage { get; set; }
    }

    public class AppCode_KissModel
    {
        public string title;
        public string detail;
        public string footer;
        public string isFree;
        public string fName;
        public string lName;
        public string nName;
        public string shape;
        public string km_w;
        public string km_h;
        public string interView;
        public string type;
        public List<AppCode_KissModel_Media> media;
    }

    public class AppCode_KissModel_Media
    {
        public string pic;
        public string clip;
    }
}