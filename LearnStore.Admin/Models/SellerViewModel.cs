using System.ComponentModel.DataAnnotations;

namespace LearnStore.Admin.Models
{
    public class SellerViewModel
    {
        public string Id { get; set; } = string.Empty;
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }= string.Empty;
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; } = string.Empty;

    }
}
