using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using StudentGradeManager.Models;

namespace StudentGradeManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // Static lists initialized with hashed passwords
        private static List<Admin> admins = new List<Admin>()
        {
            new Admin {AdminID=2024001, Name="Mary Jane Embodo", Email="maryjane@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("password1")},
            new Admin {AdminID=2024002, Name="John Doe", Email="john.doe@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("password2")},
            new Admin {AdminID=2024003, Name="Anna Smith", Email="anna.smith@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("password3")},
            new Admin {AdminID=2024004, Name="Peter Parker", Email="peter.parker@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("password4")},
            new Admin {AdminID=2024005, Name="Bruce Wayne", Email="bruce.wayne@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("password5")}
        };

        private static List<Student> students = new List<Student>()
        {
            new Student {StudentID=202401, Name="Alice Johnson", Email="alice.johnson@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("alicePass1"), YearSection="2A"},
            new Student {StudentID=202402, Name="Bob Smith", Email="bob.smith@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("bobPass2"), YearSection="2B"},
            new Student {StudentID=202403, Name="Charlie Brown", Email="charlie.brown@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("charliePass3"), YearSection="2C"},
            new Student {StudentID=202404, Name="Diana Prince", Email="diana.prince@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("dianaPass4"), YearSection="2D"},
            new Student {StudentID=202405, Name="Ethan Hunt", Email="ethan.hunt@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("ethanPass5"), YearSection="2A"},
            new Student {StudentID=202406, Name="Fiona Gallagher", Email="fiona.gallagher@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("fionaPass6"), YearSection="2B"},
            new Student {StudentID=202407, Name="George Miller", Email="george.miller@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("georgePass7"), YearSection="2C"},
            new Student {StudentID=202408, Name="Hannah Williams", Email="hannah.williams@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("hannahPass8"), YearSection="2D"},
            new Student {StudentID=202409, Name="Ian Curtis", Email="ian.curtis@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("ianPass9"), YearSection="2A"},
            new Student {StudentID=202410, Name="Julia Roberts", Email="julia.roberts@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("juliaPass10"), YearSection="2B"},
            new Student {StudentID=202411, Name="Kevin Hart", Email="kevin.hart@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("kevinPass11"), YearSection="2C"},
            new Student {StudentID=202412, Name="Laura Palmer", Email="laura.palmer@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("lauraPass12"), YearSection="2D"},
            new Student {StudentID=202413, Name="Michael Scott", Email="michael.scott@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("michaelPass13"), YearSection="2A"},
            new Student {StudentID=202414, Name="Nancy Drew", Email="nancy.drew@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("nancyPass14"), YearSection="2B"},
            new Student {StudentID=202415, Name="Oscar Wilde", Email="oscar.wilde@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("oscarPass15"), YearSection="2C"},
            new Student {StudentID=202416, Name="Pam Beesly", Email="pam.beesly@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("pamPass16"), YearSection="2D"},
            new Student {StudentID=202417, Name="Quentin Tarantino", Email="quentin.tarantino@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("quentinPass17"), YearSection="2A"},
            new Student {StudentID=202418, Name="Rachel Green", Email="rachel.green@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("rachelPass18"), YearSection="2B"},
            new Student {StudentID=202419, Name="Sam Winchester", Email="sam.winchester@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("samPass19"), YearSection="2C"},
            new Student {StudentID=202420, Name="Tina Turner", Email="tina.turner@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("tinaPass20"), YearSection="2D"}
        };

        private static List<Teacher> teachers = new List<Teacher>()
        {
            new Teacher {TeacherID=1, Name="John Doe", Email="john.doe@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("johnPass1")},
            new Teacher {TeacherID=2, Name="Emily Clark", Email="emily.clark@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("emilyPass2")},
            new Teacher {TeacherID=3, Name="Robert Frost", Email="robert.frost@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("robertPass3")},
            new Teacher {TeacherID=4, Name="Sophia Turner", Email="sophia.turner@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("sophiaPass4")},
            new Teacher {TeacherID=5, Name="William Blake", Email="william.blake@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("williamPass5")},
            new Teacher {TeacherID=6, Name="Ava Gardner", Email="ava.gardner@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("avaPass6")},
            new Teacher {TeacherID=7, Name="James Bond", Email="james.bond@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("jamesPass7")},
            new Teacher {TeacherID=8, Name="Grace Kelly", Email="grace.kelly@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("gracePass8")},
            new Teacher {TeacherID=9, Name="Henry Ford", Email="henry.ford@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("henryPass9")},
            new Teacher {TeacherID=10, Name="Isabella Swan", Email="isabella.swan@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("isabellaPass10")},
            new Teacher {TeacherID=11, Name="Charlie Chaplin", Email="charlie.chaplin@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("charliePass11")},
            new Teacher {TeacherID=12, Name="Olivia Newton", Email="olivia.newton@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("oliviaPass12")},
            new Teacher {TeacherID=13, Name="Matthew Perry", Email="matthew.perry@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("matthewPass13")},
            new Teacher {TeacherID=14, Name="Lily Evans", Email="lily.evans@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("lilyPass14")},
            new Teacher {TeacherID=15, Name="Ben Affleck", Email="ben.affleck@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("benPass15")},
            new Teacher {TeacherID=16, Name="Chloe Grace", Email="chloe.grace@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("chloePass16")},
            new Teacher {TeacherID=17, Name="Tom Hardy", Email="tom.hardy@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("tomPass17")},
            new Teacher {TeacherID=18, Name="Natalie Portman", Email="natalie.portman@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("nataliePass18")},
            new Teacher {TeacherID=19, Name="David Tennant", Email="david.tennant@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("davidPass19")},
            new Teacher {TeacherID=20, Name="Emma Watson", Email="emma.watson@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("emmaPass20")}
        };
        // Public properties to expose the lists

        public static List<Admin> Admins => admins;
        public static List<Student> Students => students;
        public static List<Teacher> Teachers => teachers;


        private readonly string jwtKey = "your_secret_jwt_key"; // Replace with a secure key

        [HttpPost("register/admin")]
        public ActionResult RegisterAdmin([FromBody] RegisterAdmin model)
        {
            if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Missing required fields.");
            }

            // Check if admin with the same email already exists
            if (admins.Any(a => a.Email == model.Email))
            {
                return BadRequest("Admin with this email already exists.");
            }

            // Generate a new Admin ID
            var newAdminId = admins.Any() ? admins.Max(a => a.AdminID) + 1 : 2024001;

            // Hash the password before saving it
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // Add the new admin to the list
            admins.Add(new Admin { AdminID = newAdminId, Name = model.FullName, Email = model.Email, Password = hashedPassword });

            return Ok("Admin account created successfully.");
        }

        [HttpPost("register/teacher")]
        public ActionResult RegisterTeacher([FromBody] RegisterTeacher model)
        {
            if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Missing required fields.");
            }

            // Check if teacher with the same email already exists
            if (teachers.Any(t => t.Email == model.Email))
            {
                return BadRequest("Teacher with this email already exists.");
            }

            // Generate a new Teacher ID
            var newTeacherId = teachers.Any() ? teachers.Max(t => t.TeacherID) + 1 : 1;

            // Hash the password before saving it
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // Add the new teacher to the list
            teachers.Add(new Teacher { TeacherID = newTeacherId, Name = model.FullName, Email = model.Email, Password = hashedPassword });

            return Ok("Teacher account created successfully.");
        }

        [HttpPost("register/student")]
        public ActionResult RegisterStudent([FromBody] RegisterStudent model)
        {
            if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.YearSection))
            {
                return BadRequest("Missing required fields.");
            }

            // Check if student with the same email already exists
            if (students.Any(s => s.Email == model.Email))
            {
                return BadRequest("Student with this email already exists.");
            }

            // Generate a new Student ID
            var newStudentId = students.Any() ? students.Max(s => s.StudentID) + 1 : 202401;

            // Hash the password before saving it
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // Add the new student to the list
            students.Add(new Student { StudentID = newStudentId, Name = model.FullName, Email = model.Email, Password = hashedPassword, YearSection = model.YearSection });

            return Ok("Student account created successfully.");
        }

        [HttpPost("LoginAdmin")]
        public ActionResult LoginAdmin([FromBody] LoginAdmin model, [FromQuery] string platform = "web")
        {
            var admin = admins.FirstOrDefault(a => a.Email == model.Email);

            if (admin != null && BCrypt.Net.BCrypt.Verify(model.Password, admin.Password))
            {
                var token = GenerateJwtToken(admin.Email, "Admin", platform);
                var tokenExpiration = DateTime.UtcNow.AddHours(1);
                return Ok(new { Token = token, TokenExpiration = tokenExpiration, Message = "Admin logged in successfully." });
            }

            return Unauthorized("Invalid email or password.");
        }

        [HttpPost("LoginTeacher")]
        public ActionResult LoginTeacher([FromBody] LoginTeacher model, [FromQuery] string platform = "web")
        {
            var teacher = teachers.FirstOrDefault(t => t.Email == model.Email);

            if (teacher != null && BCrypt.Net.BCrypt.Verify(model.Password, teacher.Password))
            {
                var token = GenerateJwtToken(teacher.Email, "Teacher", platform);
                var tokenExpiration = DateTime.UtcNow.AddHours(1);
                return Ok(new { Token = token, TokenExpiration = tokenExpiration, Message = "Teacher logged in successfully." });
            }

            return Unauthorized("Invalid email or password.");
        }

        [HttpPost("LoginStudent")]
        public ActionResult LoginStudent([FromBody] LoginStudent model, [FromQuery] string platform = "web")
        {
            var student = students.FirstOrDefault(s => s.Email == model.Email);

            if (student != null && BCrypt.Net.BCrypt.Verify(model.Password, student.Password))
            {
                var token = GenerateJwtToken(student.Email, "Student", platform);
                var tokenExpiration = DateTime.UtcNow.AddHours(1);
                return Ok(new { Token = token, TokenExpiration = tokenExpiration, Message = "Student logged in successfully." });
            }

            return Unauthorized("Invalid email or password.");
        }

        private string GenerateJwtToken(string email, string role, string platform)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim("Platform", platform) // Add platform as a claim
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}