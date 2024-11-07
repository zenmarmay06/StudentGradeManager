using System.ComponentModel.DataAnnotations;

namespace StudentGradeManager.Models
{
    public class StudentGrade
    {
        [Required(ErrorMessage = "Student Name is required.")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Course ID is required.")]
        public int CourseId { get; set; }

        public string Semester { get; set; } // Track semester assignment (e.g., "first" or "second")

        public int StudentID { get; set; }
        public string CourseName { get; set; }
        [Range(0, 100, ErrorMessage = "Midterm Grade must be between 0 and 100.")]
        public double? MidtermGrade { get; set; }

        [Range(0, 100, ErrorMessage = "Final Grade must be between 0 and 100.")]
        public double? FinalGrade { get; set; }

        // Calculate the average grade as a double
        public double? Grade => MidtermGrade.HasValue && FinalGrade.HasValue
            ? (MidtermGrade.Value + FinalGrade.Value) / 2
            : null;

    }
}
