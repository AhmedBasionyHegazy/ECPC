using System.ComponentModel.DataAnnotations;

namespace MasMasr.Authentication
{
    public class LoginModel
    {
        [EmailAddress(ErrorMessage = "Email Is Not In Correct Syntax")]
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; }
    }
}
