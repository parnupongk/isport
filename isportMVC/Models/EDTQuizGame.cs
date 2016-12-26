using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
namespace isportMVC.Models
{
    public class EDTQuizGameModelList
    {
        public List<EDTQuizGameModel> listModel { get; set; }
    }
    public class EDTQuizGameModel
    {

        public string type { get; set; }
        public string entry_id { get; set; }
        public string name { get; set; }
        public string detail { get; set; }
        public string business { get; set; }  
        public string image_cover_thumb { get; set; }
        public string image_cover { get; set; }
        public List<string> gallery_thumb { get; set; }
        public List<string> gallery { get; set; }
    }

    public class EDTQuizGameModelInsert
    {
        public string idEDT { get; set; }
        public string image { get; set; }
        public string txtQuestion { get; set; }
        public string txtChoise { get; set; }
        public string txtAnswer { get; set; }
        public string displayDate { get; set; }
    }

}
