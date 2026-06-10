using LearnStore.Localization.Resources;
using System.ComponentModel.DataAnnotations;

namespace LearnStore.Web.MVC.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "PhoneNumberRequired")]
        [Phone(ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "InvalidPhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "NameRequired")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceType = typeof(WebAppResource), ErrorMessageResourceName = "PasswordsDoNotMatch")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
