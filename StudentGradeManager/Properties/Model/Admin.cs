
   using System.ComponentModel.DataAnnotations;

    namespace StudentGradeManager.Models
    {
        public class Admin
        {
            public int AdminID { get; set; }

            [Required(ErrorMessage = "Name is required.")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email address.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required.")]
            public string Password { get; set; }
    }
    }


