using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGradeManager.Models
{
    public class LoginStudent
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }

        public string PlatForm { get; set; }

        public bool IsValid() =>
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Password);
    }
}
