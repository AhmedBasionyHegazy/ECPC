using Microsoft.AspNetCore.Identity;

namespace MasMasr.Authentication
{
    public class ApplicationUser: IdentityUser
    {
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhone { get; set; }
        public string JobTitile { get; set; }
        public string Address { get; set; }
        public string File1 { get; set; }
        public string File2 { get; set; }
        public string File3 { get; set; }
        public bool TermsAndConditions { get; set; }
        public bool UserApproved { get; set; }
        public bool IsUser { get; set; }
    }
}
