using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MasMasr.Authentication
{
    public class RegisterModel
    {
        public string FistName { get; set; }

        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Email Is Not In Correct Format")]
        [Required(ErrorMessage = "Email Is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Is Required")]
        public string ConfirmPassword { get; set; }

        public string CompanyName { get; set; }

        public string CompanyPhone { get; set; }

        public string JobTitile { get; set; }

        public string Address { get; set; }

        public IFormFile File1 { get; set; }

        public IFormFile File2 { get; set; }

        public IFormFile File3 { get; set; }

        public bool? TermsAndConditions { get; set; }
    }
}
