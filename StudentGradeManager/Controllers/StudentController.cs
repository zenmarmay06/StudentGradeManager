using Microsoft.AspNetCore.Mvc;
using StudentGradeManager.Models;
using StudentGradeManager.Store;
using System.Collections.Generic;
using System.Linq;
using System;
using StudentGradeManager.Utility;
using Microsoft.AspNetCore.Authorization;

namespace StudentGradeManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class StudentController : ControllerBase
    {
        private List<Teacher> teachers => LoginController.Teachers;
       
        private List<Student> students => LoginController.Students;
        private List<CourseAssignment> courseAssignments => AdminController.CourseAssignments;
        private static List<Course> courses = new List<Course>()
        {
            new Course { CourseID = 1, CourseName = "Mathematics" },
    new Course {  CourseID = 2, CourseName = "Science"  },
    new Course {  CourseID = 3, CourseName = "History" },
    new Course{ CourseID = 4, CourseName = "English"  },
    new Course {  CourseID = 5, CourseName = "Art"},
    new Course {  CourseID = 6, CourseName = "Physical Education" },
    new Course {  CourseID = 7, CourseName = "Geography" },
    new Course { CourseID = 8, CourseName = "Chemistry" },
    new Course {  CourseID = 9, CourseName = "Physics" },
    new Course {  CourseID = 10, CourseName = "Biology" },
    new Course {  CourseID = 11, CourseName = "Computer Science" },
    new Course { CourseID = 12, CourseName = "Economics"},
    new Course {  CourseID = 13, CourseName = "Music" },
    new Course {  CourseID = 14, CourseName = "Drama" },
    new Course {  CourseID = 15, CourseName = "Philosophy" },
    new Course {  CourseID = 16, CourseName = "Sociology" },
    new Course {  CourseID = 17, CourseName = "Anthropology"},
    new Course {  CourseID = 18, CourseName = "Psychology" },
    new Course {  CourseID = 19, CourseName = "Environmental Science" },
    new Course { CourseID = 20, CourseName = "Statistics" }
        };
        private static List<StudentGrade> studentGrades = new List<StudentGrade>();



        
       

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Student updatedStudent)
        {
            if (updatedStudent == null || updatedStudent.StudentID != id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = students.FirstOrDefault(s => s.StudentID == id);
            if (student == null)
            {
                return NotFound();
            }

            student.Name = updatedStudent.Name;
            student.Email = updatedStudent.Email;
            student.Password = updatedStudent.Password;

            return NoContent();
        }


        // Endpoint for students to view their assigned courses with teachers
        [HttpGet("{id}/AssignedCourses")]
        public ActionResult<object> ViewAssignedCourses(int id)
        {
            // Find the student by ID
            var student = students.FirstOrDefault(s => s.StudentID == id);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            // Get the course assignments for the student
            var assignedCourses = courseAssignments
                .Where(ca => ca.StudentIDs.Contains(student.StudentID))
                .Select(ca => new
                {
                    ca.CourseID,
            // Dynamically fetch the CourseName from the courses list
            CourseName = courses.FirstOrDefault(course => course.CourseID == ca.CourseID)?.CourseName ?? "Course Name Not Available",
            // Fetch the assigned Teacher's name from the teachers list
            Teacher = teachers.FirstOrDefault(t => t.TeacherID == ca.TeacherID)?.Name ?? "Teacher Not Assigned",
            // Include additional assignment details like YearSection and Semester
            AssignedYearSection = ca.YearSection,
                    AssignedSemester = ca.Semester
                })
                .ToList();

            // If no courses found for the student
            if (!assignedCourses.Any())
            {
                return NotFound("No assigned courses found for the student.");
            }

            return Ok(assignedCourses);
        }


        // Endpoint for students to view their performance report by ID
        [HttpGet("{id}/PerformanceReport")]
        public ActionResult<object> ViewPerformanceReport(int id, [FromQuery] string semester)
        {
            // Validate the semester input
            if (!ValidationUtils.IsValidSemester(semester))
            {
                return BadRequest("Invalid semester. Please specify 'first' or 'second'.");
            }

            // Find the student by ID
            var student = students.FirstOrDefault(s => s.StudentID == id);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            // Retrieve the student's grades for the specified semester
            var report = studentGrades
                .Where(g => g.StudentID == student.StudentID && g.Semester.Equals(semester, StringComparison.OrdinalIgnoreCase)) // Match with the correct student and semester
                .ToList();

            // If no grades found for the student in the specified semester
            if (!report.Any())
            {
                return NotFound("No performance data found for the student in the specified semester.");
            }

            // Calculate average grade safely
            double? averageGrade = report.Average(g => g.Grade ?? 0);

            // Create the performance report response
            var performanceReport = new
            {
                StudentName = student.Name,
                Grades = report.Select(g => new
                {
                    g.CourseId,
                    g.Grade
                }).ToList(),
                AverageGrade = averageGrade // Nullable double
            };

            return Ok(performanceReport);
        }

    }
}
    
