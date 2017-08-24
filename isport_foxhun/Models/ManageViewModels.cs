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

        public foxhun_team team { get; set; }
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