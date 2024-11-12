using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using StudentGradeManager.Models;
using System.Collections.Generic;
using System.Linq;
namespace StudentGradeManager.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public string YearSection {get; set;}
        public string Token { get; set; }

    }
}
