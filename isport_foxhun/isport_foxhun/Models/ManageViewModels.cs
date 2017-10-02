using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace isport_foxhun.Models
{
 


    public class PlayerHistoryViewModel
    {
        public List<foxhun_player_history> playerHistory { get; set; }
    }
    public class ParameterViewModel
    {
        public string name { get; set; }
        public int value { get; set; }
    }
    public class PlayerViewModel
    {
        public List<ParameterViewModel> Control { get; set; }
        public List<ParameterViewModel> Attack { get; set; }
        public List<ParameterViewModel> Defense { get; set; }
        public List<ParameterViewModel> Tacktick { get; set; }
        public List<ParameterViewModel> Physical { get; set; }
        public List<ParameterViewModel> Mental { get; set; }
        public foxhun_player player { get; set; }
    }
    public class PlayerViewModelList
    {
        public List<foxhun_player> playerList { get; set; }
        public List<ScoutViewModels> listScout { get; set; }

    }
    public class PlayertoScoutedViewModelList
    {
        public List<PlayerVModel> playerList { get; set; }
    }
    public class PlayerVModel
    {
        public string id { get; set; }
        public string region { get; set; }
        public string team { get; set; }
        public System.DateTime datetime { get; set; }
        public int? index { get; set; }
        public int? seq { get; set; }
        public string position { get; set; }
        public string name { get; set; }
        public string control { get; set; }
        public string attack { get; set; }
        public string tacktick { get; set; }
        public string defense { get; set; }
        public string physical { get; set; }
        public string mental { get; set; }
        public string adversary { get; set; }
        public string contact { get; set; }
        public string HT { get; set; }
        public string FT { get; set; }
        public int? competition { get; set; }
        public int? score { get; set; }
        public int? see { get; set; }
        public string detail { get; set; }
        public int? wight { get; set; }
        public int? hight { get; set; }
        public int? age { get; set; }
        public string country { get; set; }
        public string image { get; set; }
        public int? sum { get; set; }
        public string team_id { get; set; }
        public int? number { get; set; }
        public string birthday { get; set; }
        public string nameen { get; set; }
        public string size { get; set; }
        public string sizepants { get; set; }

        public bool isScouted { get; set; }
    }
    public class RegionViewModel
    {
        
        public string id { get; set; }
        [Display(Name ="SEQ")]
        public int seq { get; set;}
        public System.DateTime datetime { get; set; }
        [Display(Name ="Name")]
        public string name { get; set; }
        [Display(Name ="Detail")]
        public string detail { get; set; } 
    }

    public class TeamViewModel
    {
        [Required]
        [Display(Name = "เอกสารใบสมัคร")]
        public string fileDoc { get; set; }
        [Required]
        [Display(Name = "ผู้จัดการทีม")]
        public string image1 { get; set; }
        [Required]
        [Display(Name = "หัวหน้าผู้ฝึกสอน")]
        public string image2 { get; set; }

        [Display(Name = "ผู้ช่วยฝึกสอน")]
        public string image3 { get; set; }

        [Display(Name = "เจ้าหน้าที่ทีม")]
        public string image4 { get; set; }
        public string image5 { get; set; }
        public string id { get; set; }

        [Required]
        [Display(Name = "จังหวัด")]
        public string region { get; set; }
        public int index { get; set; }
        public int seq { get; set; }

        [Required]
        [Display(Name = "ชื่อทีม")]
        public string name { get; set; }
        public string detail { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string email { get; set; }
        [Required]
        public string image { get; set; }
        public string username { get; set; }
        [Required]
        [Display(Name = "password")]
        public string password { get; set; }
        public string file1 { get; set; }
        public string file2 { get; set; }
        public string file3 { get; set; }
        public string file4 { get; set; }
        public string file5 { get; set; }
        public string file6 { get; set; }
        public string file7 { get; set; }
        public string file8 { get; set; }
        public string file9 { get; set; }
        public string file10 { get; set; }
        [Required]
        [Display(Name = "ผู้จัดการทีม")]
        public string contact { get; set; }
        [Required]
        [Display(Name = "หัวหน้าผู้ฝึกสอน")]
        public string contact1 { get; set; }

        [Display(Name = "ผู้ช่วยฝึกสอน")]
        public string contact2 { get; set; }
        public string phone { get; set; }
        public string phone1 { get; set; }

        [Display(Name = "เจ้าหน้าที่ทีม")]
        public string contact3 { get; set; }

        public string contact4 { get; set; }
        public string contact5 { get; set; }
        public string contact6 { get; set; }
        public string phone3 { get; set; }
        public string phone4 { get; set; }
        public string phone5 { get; set; }
        public string phone6 { get; set; }
        public string phone2 { get; set; }
        public string pathExcel { get; set; }
        public string pathDoc { get; set; }
        public string pathImages { get; set; }
        public string pathDirectory { get; set; }
        /*
        public string id { get; set; }
        [Display(Name ="region_id")]
        public string region_id { get; set; }
        [Display(Name = "SEQ")]
        public int seq { get; set; }
        public System.DateTime datetime { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name ="Region Name")]
        public string region_name { get; set; }
        [Display(Name = "Detail")]
        public string detail { get; set; }*/
    }

    public class TeamViewModelList
    {
        public List<foxhun_team> teamList { get; set; }
    }
    public class RegionViewModelList
    {
        public List<RegionViewModel> regionList { get; set; }
    }

        public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}