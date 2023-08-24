using System.ComponentModel.DataAnnotations;

namespace MovieStoreApp.ModelsDTO
{
    public class NewCustomerDTO
    {
        [Required(ErrorMessage = "First Name is Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        public string LastName { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Telephone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
