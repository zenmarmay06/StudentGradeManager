using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace StudentGradeManager.Store
{
    public class UpdateGradeRequest
    {
        [Required]
        public string Semester { get; set; } // Options: "First Semester" or "Second Semester"

        [Required]
        [Range(0, 100)]
        public double MidtermGrade { get; set; }

        [Required]
        [Range(0, 100)]
        public double FinalGrade { get; set; }
        public int CourseID { get; set; }
    }


}
