using Microsoft.AspNetCore.Mvc;
using StudentGradeManager.Models;
using StudentGradeManager.Store;
using StudentGradeManager.Utility;
using System.Collections.Generic;
using System.Linq;
using System;

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
                        MidtermGrade = grades?.MidtermGrade ?? 0,
                        FinalGrade = grades?.FinalGrade ?? 0
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


        [HttpPut("{teacherId}/UpdateGrades/{studentId}")]
        public ActionResult UpdateGrades(int teacherId, int studentId, [FromBody] UpdateGradeRequest updateGradeRequest = null)
        {
            // Check if the teacher exists
            var teacher = teachers.FirstOrDefault(t => t.TeacherID == teacherId);
            if (teacher == null)
            {
                return NotFound("Teacher not found.");
            }

            // Retrieve the course assignment for this TeacherID
            var courseAssignment = courseAssignments.FirstOrDefault(ca => ca.TeacherID == teacherId);
            if (courseAssignment == null)
            {
                return NotFound("No course assignment found for this teacher.");
            }

            // Check if no grade update request is provided, return student details
            if (updateGradeRequest == null)
            {
                // Retrieve the student's existing grade entry
                var currentGrade = studentGrades.FirstOrDefault(g => g.StudentID == studentId && g.CourseId == courseAssignment.CourseID);

                // Get Student Name
                var studentName = GetStudentNameById(studentId);

                // Construct response with required details only
                return Ok(new
                {
                    StudentName = studentName,
                    CourseID = courseAssignment.CourseID, // Automatically use the assigned CourseID
                    Semester = courseAssignment.Semester,
                    MidtermGrade = currentGrade?.MidtermGrade ?? 0, // Default to 0 if no grade exists
                    FinalGrade = currentGrade?.FinalGrade ?? 0      // Default to 0 if no grade exists
                });
            }

            // Proceed with grade update logic if updateGradeRequest is provided
            int courseId = courseAssignment.CourseID; // Automatically get the CourseID from the assignment

            // Check if the student is assigned to the teacher's course
            if (!courseAssignment.StudentIDs.Contains(studentId))
            {
                return BadRequest("The student ID does not belong to the specified course assigned to the teacher.");
            }

            // Validate grade ranges
            if (updateGradeRequest.MidtermGrade < 0 || updateGradeRequest.MidtermGrade > 100 ||
                updateGradeRequest.FinalGrade < 0 || updateGradeRequest.FinalGrade > 100)
            {
                return BadRequest("Grades must be between 0 and 100.");
            }

            // Find existing grade entry for the student or create a new one
            var existingGradeEntry = studentGrades.FirstOrDefault(g => g.StudentID == studentId && g.CourseId == courseId &&
                                                                       g.Semester.Equals(updateGradeRequest.Semester, StringComparison.OrdinalIgnoreCase));

            // Create a response object to be returned
            var response = new
            {
                StudentName = GetStudentNameById(studentId), // Fetch the name again for response
                CourseID = courseId, // Use the CourseID from the course assignment
                Semester = updateGradeRequest.Semester,
                MidtermGrade = updateGradeRequest.MidtermGrade,
                FinalGrade = updateGradeRequest.FinalGrade
            };

            if (existingGradeEntry != null)
            {
                // Update existing grades
                existingGradeEntry.MidtermGrade = updateGradeRequest.MidtermGrade;
                existingGradeEntry.FinalGrade = updateGradeRequest.FinalGrade;
                return Ok(new
                {
                    Message = "Grades updated successfully for " + updateGradeRequest.Semester + ".",
                    Result = response // Include the detailed response here
                });
            }
            else
            {
                // Create new grade entry
                var newGrade = new StudentGrade
                {
                    StudentID = studentId,
                    StudentName = GetStudentNameById(studentId),
                    CourseId = courseId,
                    Semester = updateGradeRequest.Semester,
                    MidtermGrade = updateGradeRequest.MidtermGrade,
                    FinalGrade = updateGradeRequest.FinalGrade
                };
                studentGrades.Add(newGrade);
                return CreatedAtAction(nameof(GeneratePerformanceReport), new { studentId = studentId }, new
                {
                    Message = "Grades inputted successfully for " + updateGradeRequest.Semester + ".",
                    Result = response // Include the detailed response here
                });
            }
        }



        // Calculate GWA for a Student
        [HttpGet("CalculateGWA/{teacherId}/{semester}")]
        public ActionResult<object> CalculateGWA(int teacherId, string semester)
        {
            // Validate the semester input
            if (!ValidationUtils.IsValidSemester(semester))
            {
                return BadRequest("Invalid semester. Please specify 'first' or 'second'.");
            }

            // Get all students assigned to the teacher for the specified semester
            var assignedStudentIds = courseAssignments
                .Where(ca => ca.TeacherIDs.Contains(teacherId) && ca.Semester.Equals(semester, StringComparison.OrdinalIgnoreCase))
                .SelectMany(ca => ca.StudentIDs)
                .Distinct()
                .ToList();

            var studentGradesForTeacher = studentGrades
                .Where(g => assignedStudentIds.Contains(g.StudentID))
                .ToList();

            if (!studentGradesForTeacher.Any())
            {
                return Ok("No grades found for the students assigned to this teacher.");
            }

            double totalWeightedGrades = 0;
            int count = 0;

            foreach (var grade in studentGradesForTeacher)
            {
                double midterm = grade.MidtermGrade ?? 0;
                double final = grade.FinalGrade ?? 0;

                if (midterm != 0 || final != 0)
                {
                    double average = (midterm + final) / (midterm > 0 && final > 0 ? 2 : 1);
                    totalWeightedGrades += average;
                    count++;
                }
            }

            double? gwa = count > 0 ? totalWeightedGrades / count : (double?)null;

            return Ok(new { TeacherID = teacherId, Semester = semester, GWA = gwa });
        }

        // Generate Performance Report for a Student
        [HttpGet("PerformanceReport/{teacherId}/{semester}")]
        public ActionResult<object> GeneratePerformanceReport(int teacherId, string semester)
        {
            // Validate the semester input
            if (!ValidationUtils.IsValidSemester(semester))
            {
                return BadRequest("Invalid semester. Please specify 'first' or 'second'.");
            }

            // Get all students assigned to the teacher for the specified semester
            var assignedStudentIds = courseAssignments
                .Where(ca => ca.TeacherIDs.Contains(teacherId) && ca.Semester.Equals(semester, StringComparison.OrdinalIgnoreCase))
                .SelectMany(ca => ca.StudentIDs)
                .Distinct()
                .ToList();

            // Gather grade data for assigned students
            var report = assignedStudentIds.Select(studentId =>
            {
                var studentGrade = studentGrades.FirstOrDefault(g => g.StudentID == studentId && g.Semester.Equals(semester, StringComparison.OrdinalIgnoreCase));
                return new
                {
                    StudentID = studentId,
                    StudentName = GetStudentNameById(studentId),
                    CourseId = studentGrade?.CourseId,
                    CourseName = studentGrade != null ? GetCourseNameById(studentGrade.CourseId) : null,
                    MidtermGrade = studentGrade?.MidtermGrade,
                    FinalGrade = studentGrade?.FinalGrade
                };
            }).ToList();

            // Calculate averages for midterm and final grades where they exist
            var midtermGrades = report.Select(g => g.MidtermGrade).Where(g => g.HasValue).ToList();
            var finalGrades = report.Select(g => g.FinalGrade).Where(g => g.HasValue).ToList();

            double? midtermAverage = midtermGrades.Any() ? midtermGrades.Average() : (double?)null;
            double? finalAverage = finalGrades.Any() ? finalGrades.Average() : (double?)null;

            double? gwa = null;
            if (midtermAverage.HasValue || finalAverage.HasValue)
            {
                double totalGrades = (midtermAverage ?? 0) + (finalAverage ?? 0);
                int count = (midtermAverage.HasValue ? 1 : 0) + (finalAverage.HasValue ? 1 : 0);

                if (count > 0)
                {
                    gwa = totalGrades / count;
                }
            }

            var performanceReport = new
            {
                TeacherID = teacherId,
                Semester = semester,
                AssignedStudents = report, // List of all students assigned with grades (or null for those without)
                MidtermAverage = midtermAverage,
                FinalAverage = finalAverage,
                GWA = gwa
            };

            return Ok(performanceReport);
        }

        // Example method to retrieve course name by course ID
        
    }
}