using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GiantNationalBank.Models
{
    public class RegistrationModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string LastName { get; set; } = null!;
        [Required]
        [StringLength(60, MinimumLength = 1)]
        public string Street1 { get; set; } = null!;
        public string? Street2 { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string City { get; set; } = null!;
        [Required]
        [StringLength(2, MinimumLength = 1)]
        public string State { get; set; } = null!;
        [Required]
        [Range(0, int.MaxValue)]
        [DataType(DataType.PostalCode)]
        public int ZipCode { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string Username { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [HiddenInput]
        public string UserType { get; set; } = null!;
    }
}
