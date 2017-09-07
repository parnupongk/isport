using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace isport_foxhun.Models
{

    public class MatchViewModels
    {
        public List<foxhun_team> teamList { get; set; }

        [Display(Name ="Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime matchdate { get; set; }
        public string team1 { get; set; }
        public string team2 { get; set; }
        public string ht { get; set; }
        public string ft { get; set; }
    }
    public class MatchModels
    {

    }
}