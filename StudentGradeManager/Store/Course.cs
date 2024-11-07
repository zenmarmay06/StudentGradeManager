using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace StudentGradeManager.Models
{
    public class Course
    {
        public int CourseID { get; set; }

        [Required(ErrorMessage = "Course name is required.")]
        public string CourseName { get; set; }

        public List<string> Semester { get; set; } = new List<string>();// Track semester assignment (e.g., "first" or "second")
        public List<string> YearSection { get; set; } = new List<string>();
    }
}
