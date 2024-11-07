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
    public class AdminController : ControllerBase
    {

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
        private static List<CourseAssignment> courseAssignments = new List<CourseAssignment>
{
    new CourseAssignment { TeacherID = 1, CourseID = 1, CourseName = "Mathematics", StudentIDs = new List<int> { 202401, 202402 }, Semester = "First Semester" },
    new CourseAssignment { TeacherID = 2, CourseID = 2, CourseName = "Science", StudentIDs = new List<int> { 202403, 202404 }, Semester = "Second Semester" },
    new CourseAssignment { TeacherID = 3, CourseID = 3, CourseName = "History", StudentIDs = new List<int> { 202405, 202406 }, Semester = "First Semester" },
    new CourseAssignment { TeacherID = 4, CourseID = 4, CourseName = "English", StudentIDs = new List<int> { 202407, 202408 }, Semester = "Second Semester" },
    new CourseAssignment { TeacherID = 5, CourseID = 5, CourseName = "Art", StudentIDs = new List<int> { 202409, 202410 }, Semester = "First Semester" },
    new CourseAssignment { TeacherID = 6, CourseID = 6, CourseName = "Physical Education", StudentIDs = new List<int> { 202411, 202412 }, Semester = "Second Semester" },
    new CourseAssignment { TeacherID = 7, CourseID = 7, CourseName = "Geography", StudentIDs = new List<int> { 202413, 202414 }, Semester = "First Semester" },
    new CourseAssignment { TeacherID = 8, CourseID = 8, CourseName = "Chemistry", StudentIDs = new List<int> { 202415, 202416 }, Semester = "Second Semester" },
    new CourseAssignment { TeacherID = 9, CourseID = 9, CourseName = "Physics", StudentIDs = new List<int> { 202417, 202418 }, Semester = "First Semester" },
    new CourseAssignment { TeacherID = 10, CourseID = 10, CourseName = "Biology", StudentIDs = new List<int> { 202419, 202420 }, Semester = "Second Semester" },
    new CourseAssignment { TeacherID = 11, CourseID = 11, CourseName = "Computer Science", StudentIDs = new List<int> { 202401, 202402 }, Semester = "First Semester" },
    new CourseAssignment { TeacherID = 12, CourseID = 12, CourseName = "Economics", StudentIDs = new List<int> { 202403, 202404 }, Semester = "Second Semester" },
    new CourseAssignment { TeacherID = 13, CourseID = 13, CourseName = "Music", StudentIDs = new List<int> { 202405, 202406 }, Semester = "First Semester" },
    new CourseAssignment { TeacherID = 14, CourseID = 14, CourseName = "Drama", StudentIDs = new List<int> { 202407, 202408 }, Semester = "Second Semester" },
    new CourseAssignment { TeacherID = 15, CourseID = 15, CourseName = "Philosophy", StudentIDs = new List<int> { 202409, 202410 }, Semester = "First Semester" },
    new CourseAssignment { TeacherID = 16, CourseID = 16, CourseName = "Sociology", StudentIDs = new List<int> { 202411, 202412 }, Semester = "Second Semester" },
    new CourseAssignment { TeacherID = 17, CourseID = 17, CourseName = "Anthropology", StudentIDs = new List<int> { 202413, 202414 }, Semester = "First Semester" },
    new CourseAssignment { TeacherID = 18, CourseID = 18, CourseName = "Psychology", StudentIDs = new List<int> { 202415, 202416 }, Semester = "Second Semester" },
    new CourseAssignment { TeacherID = 19, CourseID = 19, CourseName = "Environmental Science", StudentIDs = new List<int> { 202417, 202418 }, Semester = "First Semester" },
    new CourseAssignment { TeacherID = 20, CourseID = 20, CourseName = "Statistics", StudentIDs = new List<int> { 202419, 202420 }, Semester = "Second Semester" }
};
        public static List<CourseAssignment> CourseAssignments => courseAssignments;

        private static List<StudentGrade> studentGrades = new List<StudentGrade>();
        // Access the lists from LoginController without redeclaring
        private List<Teacher> teachers => LoginController.Teachers;
        private List<Admin> admins => LoginController.Admins;
        private List<Student> students => LoginController.Students;

        // Admin CRUD Operations
        [HttpGet("Admins")]
        public ActionResult<IEnumerable<Admin>> GetAdmins()
        {
            return Ok(admins);
        }

        [HttpGet("Admins/{id}")]
        public ActionResult<Admin> GetAdmin(int id)
        {
            var admin = admins.FirstOrDefault(a => a.AdminID == id);
            if (admin == null)
            {
                return NotFound();
            }
            return Ok(admin);
        }



        [HttpPut("Admins/{id}")]
        public ActionResult PutAdmin(int id, [FromBody] Admin updatedAdmin)
        {
            if (updatedAdmin == null || updatedAdmin.AdminID != id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var admin = admins.FirstOrDefault(a => a.AdminID == id);
            if (admin == null)
            {
                return NotFound();
            }

            admin.Name = updatedAdmin.Name;
            admin.Email = updatedAdmin.Email;
            admin.Password = updatedAdmin.Password;

            return NoContent();
        }

        [HttpDelete("Admins/{id}")]
        public ActionResult DeleteAdmin(int id)
        {
            var admin = admins.FirstOrDefault(a => a.AdminID == id);
            if (admin == null)
            {
                return NotFound();
            }
            admins.Remove(admin);
            return NoContent();
        }

        // Student CRUD Operations
        [HttpGet("Students")]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            if (students == null || !students.Any())
            {
                return NotFound("No students found.");
            }
            return Ok(students);
        }

        [HttpGet("Students/{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var student = students.FirstOrDefault(s => s.StudentID == id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        // New endpoint to get student performance
        [HttpGet("Performance")]
        public ActionResult<IEnumerable<StudentPerformance>> GetStudentPerformance([FromQuery] string semester)
        {
            if (!ValidationUtils.IsValidSemester(semester))
            {
                return BadRequest("Invalid semester. Please specify 'first' or 'second'.");
            }

            var performances = new List<StudentPerformance>();

            // Loop through each course assignment
            foreach (var assignment in courseAssignments.Where(ca => ca.Semester.Equals(semester, StringComparison.OrdinalIgnoreCase)))
            {
                // Get the assigned teacher
                var teacher = teachers.FirstOrDefault(t => t.TeacherID == assignment.TeacherID);
                var teacherName = teacher != null ? teacher.Name : "Not Assigned";

                // Loop through each student in the assignment
                foreach (var studentId in assignment.StudentIDs)
                {
                    var student = students.FirstOrDefault(s => s.StudentID == studentId);
                    if (student != null)
                    {
                        performances.Add(new StudentPerformance
                        {
                            StudentID = student.StudentID,
                            StudentName = student.Name,
                            CourseName = assignment.CourseName,
                            TeacherName = teacherName,
                            Grades = GetStudentGrades(student.StudentID) // This method needs to be implemented
                        });
                    }
                }
            }

            return Ok(performances);
        }

        // Method to retrieve grades for a specific student (implement this according to your grade storage logic)
        private List<double> GetStudentGrades(int studentId)
        {
            // Replace with your actual logic to get grades for a student
            var gradesForStudent = studentGrades
                .Where(g => g.StudentID == studentId) // Filter by student ID
                .Select(g => g.Grade) // Select the Grade (which is double?)
                .Where(g => g.HasValue) // Filter out null values
                .Select(g => g.Value) // Project to non-nullable double
                .ToList(); // Create a list of double

            return gradesForStudent; // Returns List<double>
        }




        [HttpPut("Students/{id}")]
        public ActionResult PutStudent(int id, [FromBody] Student updatedStudent)
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

        [HttpDelete("Students/{id}")]
        public ActionResult DeleteStudent(int id)
        {
            var student = students.FirstOrDefault(s => s.StudentID == id);
            if (student == null)
            {
                return NotFound();
            }
            students.Remove(student);
            return NoContent();
        }

        // Teacher CRUD Operations
        [HttpGet("Teachers")]
        public ActionResult<IEnumerable<Teacher>> GetTeachers()
        {
            if (teachers == null || !teachers.Any())
            {
                return NotFound("No teachers found.");
            }
            return Ok(teachers);
        }

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



        [HttpPut("Teachers/{id}")]
        public ActionResult PutTeacher(int id, [FromBody] Teacher updatedTeacher)
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

        [HttpDelete("Teachers/{id}")]
        public ActionResult DeleteTeacher(int id)
        {
            var teacher = teachers.FirstOrDefault(t => t.TeacherID == id);
            if (teacher == null)
            {
                return NotFound();
            }
            teachers.Remove(teacher);
            return NoContent();
        }

        // Course CRUD Operations
        [HttpGet("Courses")]
        public ActionResult<IEnumerable<object>> GetCourses()
        {
            // Create a response that includes all course details
            var courseDetails = courses.Select(course => new
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName
                
            });

            return Ok(courseDetails);
        }

        [HttpGet("Courses/{id}")]
        public ActionResult<object> GetCourse(int id)
        {
            var course = courses.FirstOrDefault(c => c.CourseID == id);
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            // Find the course assignment for the specific course
            var courseAssignment = courseAssignments.FirstOrDefault(ca => ca.CourseID == id);
            if (courseAssignment == null)
            {
                return NotFound("No assignment found for this course.");
            }

            // Get the assigned teacher's name
            var teacher = teachers.FirstOrDefault(t => t.TeacherID == courseAssignment.TeacherID);
            var teacherName = teacher != null ? teacher.Name : "Not Assigned";

            // Get the list of student names assigned to this course
            var studentNames = courseAssignment.StudentIDs
                .Select(studentId => GetStudentNameById(studentId))
                .ToList();

            // Display the full YearSection information
            var assignedYearSection = courseAssignment.YearSection ?? "YearSection not assigned";

            // Create a response object that includes the course details, assigned teacher, students, YearSection, and semester
            var courseDetails = new
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName,
                AssignedTeacher = teacherName,
                AssignedStudents = studentNames,
                AssignedSemester = courseAssignment.Semester 
            };

            return Ok(courseDetails);
        }


        [HttpPost("Courses/Add")]
        public ActionResult<List<object>> AddCourses([FromBody] List<Course> newCourses)
        {
            if (newCourses == null || !newCourses.Any())
            {
                return BadRequest("No courses to add.");
            }

            var addedCourses = new List<object>(); // To store added courses

            foreach (var course in newCourses)
            {
                // Ensure the CourseName is not null or empty
                if (string.IsNullOrWhiteSpace(course.CourseName))
                {
                    return BadRequest("Course name cannot be null or empty.");
                }

                // Ensure the CourseID is unique (if applicable)
                if (courses.Any(c => c.CourseID == course.CourseID))
                {
                    return BadRequest($"Course ID {course.CourseID} already exists.");
                }

                // Add the course to the collection
                courses.Add(course);

                // Create an object to return containing only CourseID and CourseName
                addedCourses.Add(new
                {
                    CourseID = course.CourseID,
                    CourseName = course.CourseName
                });
            }

            return Ok(addedCourses); // Return the list of added courses
        }


        [HttpPut("Courses/{id}")]
        public ActionResult PutCourse(int id, [FromBody] Course updatedCourse)
        {
            // Validate that the provided course data is correct and the CourseID matches
            if (updatedCourse == null || updatedCourse.CourseID != id)
            {
                return BadRequest("Invalid course data.");
            }

            // Ensure that the new CourseName is not null or empty
            if (string.IsNullOrWhiteSpace(updatedCourse.CourseName))
            {
                return BadRequest("Course name cannot be null or empty.");
            }

            // Find the existing course in the list by its CourseID
            var course = courses.FirstOrDefault(c => c.CourseID == id);
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            // Update only the CourseName
            course.CourseName = updatedCourse.CourseName;

            return Ok($"Course name updated to '{course.CourseName}' successfully.");
        }


        [HttpDelete("Courses/{id}")]
        public ActionResult DeleteCourse(int id)
        {
            var course = courses.FirstOrDefault(c => c.CourseID == id);
            if (course == null)
            {
                return NotFound("Course not found.");
            }

            // Remove the course
            courses.Remove(course);

            // Additionally remove any related assignments (if needed)
            var relatedAssignments = courseAssignments.Where(ca => ca.CourseID == id).ToList();
            foreach (var assignment in relatedAssignments)
            {
                courseAssignments.Remove(assignment);
            }

            return NoContent(); // Successfully deleted
        }






        [HttpGet("GetAllCourseAssignments")]
        public ActionResult<IEnumerable<object>> GetAllCourseAssignments()
        {
            // Check if there are any course assignments
            if (!courseAssignments.Any())
            {
                return NotFound("No course assignments found.");
            }

            // Retrieve all course assignments with relevant details
            var assignments = courseAssignments.Select(ca => new
            {
                CourseID = ca.CourseID,
                TeacherID = ca.TeacherID,
                Semester = ca.Semester,
                StudentIDs = ca.StudentIDs,
                // Dynamically fetch the CourseName and TeacherName
                CourseName = courses.FirstOrDefault(c => c.CourseID == ca.CourseID)?.CourseName ?? "Course Name Not Available",
                TeacherName = teachers.FirstOrDefault(t => t.TeacherID == ca.TeacherID)?.Name ?? "Teacher Not Assigned"
            }).ToList();

            return Ok(assignments);
        }

        // Add course assignment to a student
        [HttpPost("AssignCourseToStudent")]
        public ActionResult AssignCourseToStudent(int studentId, int courseId, int teacherId, string semester, string yearSection)
        {
            // Check if the course and teacher exist
            var course = courses.FirstOrDefault(c => c.CourseID == courseId);
            var teacher = teachers.FirstOrDefault(t => t.TeacherID == teacherId);
            if (course == null || teacher == null)
            {
                return BadRequest("Invalid course or teacher ID.");
            }

            // Find or create the course assignment for this teacher and course
            var assignment = courseAssignments.FirstOrDefault(ca => ca.CourseID == courseId && ca.TeacherID == teacherId && ca.Semester == semester);
            if (assignment == null)
            {
                assignment = new CourseAssignment
                {
                    CourseID = courseId,
                    TeacherID = teacherId,
                    Semester = semester,
                    YearSection = yearSection, // Include YearSection here
                    StudentIDs = new List<int> { studentId }
                };
                courseAssignments.Add(assignment);
            }
            else
            {
                // Add the student if they're not already assigned
                if (!assignment.StudentIDs.Contains(studentId))
                {
                    assignment.StudentIDs.Add(studentId);
                }
            }

            return Ok("Course assigned to student successfully.");
        }


        // Update an existing course assignment for a student
        [HttpPut("UpdateStudentCourseAssignment")]
        public ActionResult UpdateStudentCourseAssignment(int studentId, int courseId, int teacherId, string newSemester)
        {
            // Find the course assignment for this teacher and course
            var assignment = courseAssignments.FirstOrDefault(ca => ca.CourseID == courseId && ca.TeacherID == teacherId);
            if (assignment == null || !assignment.StudentIDs.Contains(studentId))
            {
                return NotFound("Assignment not found for this student.");
            }

            // Update the semester for this assignment
            assignment.Semester = newSemester;
            return Ok("Course assignment updated successfully.");
        }

        // Remove a student from a course assignment
        [HttpDelete("RemoveStudentFromCourse/{courseId}")]
        public ActionResult RemoveStudentFromCourse(int courseId, int studentId)
        {
            // Find the course assignment for this course
            var assignment = courseAssignments.FirstOrDefault(ca => ca.CourseID == courseId);
            if (assignment == null)
            {
                return NotFound($"No course assignment found with CourseID: {courseId}");
            }
            if (!assignment.StudentIDs.Contains(studentId))
            {
                return NotFound($"Student with ID {studentId} not found in the course assignment.");
            }

            // Remove the student from the assignment
            assignment.StudentIDs.Remove(studentId);
            if (assignment.StudentIDs.Count == 0)
            {
                courseAssignments.Remove(assignment); // Clean up if no students are assigned
            }

            return Ok("Student removed from course successfully.");
        }






        private string GetStudentNameById(int studentId)
        {
            var student = students.FirstOrDefault(s => s.StudentID == studentId);
            return student != null ? student.Name : "Unknown Student";
        }




       
    }
}