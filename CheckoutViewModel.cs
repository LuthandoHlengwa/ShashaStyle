using System.ComponentModel.DataAnnotations;

namespace ShashaStyle.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;


        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;


        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;


        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;


        [Required]
        [Display(Name = "Street Address")]
        public string Address { get; set; } = string.Empty;


        [Required]
        public string City { get; set; } = string.Empty;


        [Required]
        public string Province { get; set; } = string.Empty;


        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = string.Empty;


        public decimal Subtotal { get; set; }

        public decimal Shipping { get; set; }

        public decimal Total { get; set; }
    }
}