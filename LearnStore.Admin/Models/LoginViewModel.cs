using System.ComponentModel.DataAnnotations;

namespace LearnStore.Admin.Models
{
    public class LoginViewModel
    {
        [Required]
        
        public string Name { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string ReturnUrl { get; set; } = string.Empty;
    }
}
