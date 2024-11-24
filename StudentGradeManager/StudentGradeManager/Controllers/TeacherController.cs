using Microsoft.AspNetCore.Mvc;
using StudentGradeManager.Models;
using StudentGradeManager.Store;
using StudentGradeManager.Utility;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;

namespace StudentGradeManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TeacherController : ControllerBase
    {

        private List<Teacher> teachers => LoginController.Teachers;

        private List<Student> students => LoginController.Students;
        private static List<StudentGrade> studentGrades = new List<StudentGrade>();



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
        // Assume courseAssignments is populated in a shared context or injected.
        private List<CourseAssignment> courseAssignments => AdminController.CourseAssignments;
        [HttpGet("Teachers/{id}")]
        public ActionResult<Teacher> GetTeacher(int id)
        {
            var teacher = teachers.FirstOrDefault(t => t.TeacherID == id);
            if (teacher == null)
            {
                return NotFound();
            }
            return Ok(teacher);
        }
        [HttpGet("{teacherId}")]
        public ActionResult<object> Get(int teacherId)
        {
            // Check if the teacher exists
            var teacher = teachers.FirstOrDefault(t => t.TeacherID == teacherId);
            if (teacher == null)
            {
                return NotFound("Teacher not found.");
            }

            // Retrieve all course assignments for this TeacherID
            var teacherAssignments = courseAssignments
                .Where(ca => ca.TeacherID == teacherId)
                .ToList();

            if (!teacherAssignments.Any())
            {
                return NotFound("No course assignments found for this teacher.");
            }

            // Create a result list to store the details of each course and assigned students
            var result = teacherAssignments.SelectMany(courseAssignment =>
            {
                string semester = courseAssignment.Semester;
                int courseId = courseAssignment.CourseID;

                // Find assigned students for each course
                var assignedStudents = courseAssignment.StudentIDs.Distinct().ToList();

                // Generate the list of student details for this course
                return assignedStudents.Select(studentId =>
                {
                    // Retrieve the student's grades specific to the course and semester
                    var grades = studentGrades.FirstOrDefault(g =>
                        g.StudentID == studentId &&
                        g.CourseId == courseId &&
                        g.Semester.Equals(semester, StringComparison.OrdinalIgnoreCase));

                    var studentName = GetStudentNameById(studentId); // Retrieve student name

                    return new
                    {
                        CourseID = courseId,
                        CourseName = GetCourseNameById(courseId), // Retrieve course name
                        Semester = semester,
                        StudentID = studentId,
                        StudentName = studentName,

                    };
                });
            }).ToList();

            return Ok(new { Teacher = teacher.Name, AssignedCourses = result });
        }

        // Helper methods to get course and student names
        private string GetCourseNameById(int courseId)
        {
            var course = courses.FirstOrDefault(c => c.CourseID == courseId);
            return course != null ? course.CourseName : "Unknown Course";
        }

        private string GetStudentNameById(int studentId)
        {
            var student = students.FirstOrDefault(s => s.StudentID == studentId);
            return student != null ? student.Name : "Unknown Student";
        }







        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Teacher updatedTeacher)
        {
            if (updatedTeacher == null || updatedTeacher.TeacherID != id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teacher = teachers.FirstOrDefault(t => t.TeacherID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            teacher.Name = updatedTeacher.Name;
            teacher.Email = updatedTeacher.Email;
            teacher.Password = updatedTeacher.Password;

            return NoContent();
        }
        // Helper method to calculate FinalAverage for each course
        // Helper method to calculate FinalAverage for each course
        private double CalculateFinalAverage(double? midtermGrade, double? finalGrade)
        {
            if (midtermGrade.HasValue && finalGrade.HasValue)
            {
                // Both grades exist, return their average
                return (midtermGrade.Value + finalGrade.Value) / 2;
            }
            else if (midtermGrade.HasValue)
            {
                // Only midterm grade exists
                return midtermGrade.Value;
            }
            else if (finalGrade.HasValue)
            {
                // Only final grade exists
                return finalGrade.Value;
            }

            // No grades provided, return default value (e.g., 0)
            return 0;
        }





        // Helper method to calculate overall average across all courses
        private double CalculateOverallAverage(int teacherId, string semester)
        {
            // Filter grades for the teacher and semester
            var relevantGrades = studentGrades
                .Where(g => g.Semester.Equals(semester, StringComparison.OrdinalIgnoreCase) &&
                            courseAssignments.Any(ca => ca.TeacherID == teacherId && ca.CourseID == g.CourseId))
                .ToList();

            // Calculate final averages for the filtered grades
            var finalAverages = relevantGrades.Select(g => CalculateFinalAverage(g.MidtermGrade, g.FinalGrade)).ToList();

            // Return the average of the final averages or 0 if no grades are found
            return finalAverages.Any() ? finalAverages.Average() : 0;
        }



        [HttpPost("{teacherId}/InputGrades")]
        public ActionResult InputGrades(int teacherId, [FromBody] UpdateGradeRequest inputGradesRequest)
        {
            var teacher = teachers.FirstOrDefault(t => t.TeacherID == teacherId);
            if (teacher == null)
            {
                return NotFound("Teacher not found.");
            }

            var courseAssignment = courseAssignments.FirstOrDefault(ca => ca.TeacherID == teacherId);
            if (courseAssignment == null)
            {
                return NotFound("No course assignment found for this teacher.");
            }

            if (!courseAssignment.StudentIDs.Contains(inputGradesRequest.StudentID))
            {
                return BadRequest("The student ID does not belong to the specified course assigned to the teacher.");
            }

            if (inputGradesRequest.MidtermGrade < 0 || inputGradesRequest.MidtermGrade > 100 ||
                inputGradesRequest.FinalGrade < 0 || inputGradesRequest.FinalGrade > 100)
            {
                return BadRequest("Grades must be between 0 and 100.");
            }

            // Update or insert grade record
            var existingGradeEntry = studentGrades.FirstOrDefault(g => g.StudentID == inputGradesRequest.StudentID &&
                                                                         g.CourseId == courseAssignment.CourseID &&
                                                                         g.Semester.Equals(inputGradesRequest.Semester, StringComparison.OrdinalIgnoreCase));

            if (existingGradeEntry != null)
            {
                existingGradeEntry.MidtermGrade = inputGradesRequest.MidtermGrade;
                existingGradeEntry.FinalGrade = inputGradesRequest.FinalGrade;
            }
            else
            {
                var newGrade = new StudentGrade
                {
                    StudentID = inputGradesRequest.StudentID,
                    StudentName = GetStudentNameById(inputGradesRequest.StudentID),
                    CourseId = courseAssignment.CourseID,
                    Semester = inputGradesRequest.Semester,
                    MidtermGrade = inputGradesRequest.MidtermGrade,
                    FinalGrade = inputGradesRequest.FinalGrade
                };

                studentGrades.Add(newGrade);
            }

            return Ok(new { Message = "Grades inputted successfully." });
        }



        [HttpPut("{teacherId}/UpdateGrades/{studentId}")]
        public ActionResult UpdateGrades(int teacherId, int studentId, [FromBody] UpdateGradeRequest updateGradeRequest = null)
        {
            var teacher = teachers.FirstOrDefault(t => t.TeacherID == teacherId);
            if (teacher == null)
            {
                return NotFound("Teacher not found.");
            }

            var courseAssignment = courseAssignments.FirstOrDefault(ca => ca.TeacherID == teacherId);
            if (courseAssignment == null)
            {
                return NotFound("No course assignment found for this teacher.");
            }

            if (!courseAssignment.StudentIDs.Contains(studentId))
            {
                return BadRequest("The student ID does not belong to the specified course assigned to the teacher.");
            }

            if (updateGradeRequest == null)
            {
                // When no update data is provided, return the current grades.
                var currentGrade = studentGrades.FirstOrDefault(g => g.StudentID == studentId && g.CourseId == courseAssignment.CourseID);
                var studentName = GetStudentNameById(studentId);

                return Ok(new
                {
                    StudentName = studentName,
                    CourseID = courseAssignment.CourseID,
                    Semester = courseAssignment.Semester,
                    MidtermGrade = currentGrade?.MidtermGrade ?? 0,
                    FinalGrade = currentGrade?.FinalGrade ?? 0
                });
            }

            if (updateGradeRequest.MidtermGrade < 0 || updateGradeRequest.MidtermGrade > 100 ||
                updateGradeRequest.FinalGrade < 0 || updateGradeRequest.FinalGrade > 100)
            {
                return BadRequest("Grades must be between 0 and 100.");
            }

            var existingGradeEntry = studentGrades.FirstOrDefault(g => g.StudentID == studentId && g.CourseId == courseAssignment.CourseID &&
                                                                         g.Semester.Equals(updateGradeRequest.Semester, StringComparison.OrdinalIgnoreCase));

            if (existingGradeEntry != null)
            {
                // If grades already exist, update the grades
                existingGradeEntry.MidtermGrade = updateGradeRequest.MidtermGrade;
                existingGradeEntry.FinalGrade = updateGradeRequest.FinalGrade;

                return Ok(new { Message = "Grades updated successfully." });
            }
            else
            {
                // If no existing grade entry, create a new grade record
                var newGrade = new StudentGrade
                {
                    StudentID = studentId,
                    StudentName = GetStudentNameById(studentId),
                    CourseId = courseAssignment.CourseID,
                    Semester = updateGradeRequest.Semester,
                    MidtermGrade = updateGradeRequest.MidtermGrade,
                    FinalGrade = updateGradeRequest.FinalGrade
                };

                studentGrades.Add(newGrade);

                return CreatedAtAction(nameof(UpdateGrades), new { studentId = studentId }, new { Message = "Grades updated successfully." });
            }
        }


        [HttpGet("{teacherId}/PerformanceReport")]
        public ActionResult<object> ViewPerformanceReportByTeacher(int teacherId, [FromQuery] string semester)
        {
            // Validate semester
            if (!ValidationUtils.IsValidSemester(semester))
            {
                return BadRequest("Invalid semester. Please specify 'first' or 'second'.");
            }

            // Get all course assignments for the teacher
            var courseAssignmentsForTeacher = courseAssignments
                .Where(ca => ca.TeacherID == teacherId)
                .ToList();

            if (!courseAssignmentsForTeacher.Any())
            {
                return NotFound("No courses assigned to this teacher.");
            }

            // Create a list of students assigned to the teacher across all courses
            var assignedStudents = new List<Student>();

            foreach (var courseAssignment in courseAssignmentsForTeacher)
            {
                // Get the students assigned to this course
                var studentsInCourse = courseAssignment.StudentIDs
                    .Select(studentId => students.FirstOrDefault(s => s.StudentID == studentId))
                    .Where(s => s != null)
                    .ToList();

                assignedStudents.AddRange(studentsInCourse);
            }

            if (!assignedStudents.Any())
            {
                return NotFound("No students assigned to this teacher.");
            }

            // Create a report for each assigned student
            var performanceReports = new List<object>();

            foreach (var student in assignedStudents.Distinct())
            {
                var report = studentGrades
                    .Where(g => g.StudentID == student.StudentID && g.Semester.Equals(semester, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (report.Any())
                {
                    var performanceReport = new
                    {
                        StudentName = student.Name,
                        StudentID = student.StudentID,
                        Grades = report.Select(g => new
                        {
                            g.CourseId,
                            CourseName = courses.FirstOrDefault(c => c.CourseID == g.CourseId)?.CourseName ?? "Unknown Course",
                            g.MidtermGrade,
                            g.FinalGrade,
                            FinalAverage = CalculateFinalAverage(g.MidtermGrade, g.FinalGrade) // Call the method here
                        }).ToList(),
                        AverageGrade = report.Any() ? report.Average(g => CalculateFinalAverage(g.MidtermGrade, g.FinalGrade)) : 0 // Calculate overall average
                    };

                    performanceReports.Add(performanceReport);
                }
            }

            if (!performanceReports.Any())
            {
                return NotFound("No performance data found for the students in the specified semester.");
            }

            return Ok(performanceReports);
        }



        // Example method to retrieve course name by course ID

    }
}