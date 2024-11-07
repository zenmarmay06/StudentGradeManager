using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGradeManager.Models
{
    public class RegisterStudent
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string YearSection { get; set; }
    }
}
