using LearnStore.Localization.Resources;
using System.ComponentModel.DataAnnotations;

namespace LearnStore.Web.MVC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string ReturnUrl { get; set; } = string.Empty;
    }

}
