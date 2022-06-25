using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasMasr.Authentication
{
    public class UserResultDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
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
    }
}
