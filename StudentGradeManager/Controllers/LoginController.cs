using Microsoft.AspNetCore.Mvc;
using StudentGradeManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace StudentGradeManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private static List<Admin> admins = new List<Admin>()
        {
            new Admin {AdminID=2024001, Name="Mary Jane Embodo", Email="maryjane@gmail.com", Password="password1"},
            new Admin {AdminID=2024002, Name="John Doe", Email="john.doe@gmail.com", Password="password2"},
            new Admin {AdminID=2024003, Name="Anna Smith", Email="anna.smith@gmail.com", Password="password3"},
            new Admin {AdminID=2024004, Name="Peter Parker", Email="peter.parker@gmail.com", Password="password4"},
            new Admin {AdminID=2024005, Name="Bruce Wayne", Email="bruce.wayne@gmail.com", Password="password5"}
        };
        private static List<Student> students = new List<Student>()
        {
              new Student {StudentID=202401, Name="Alice Johnson", Email="alice.johnson@gmail.com", Password="alicePass1", YearSection="2A"},
              new Student {StudentID=202402, Name="Bob Smith", Email="bob.smith@gmail.com", Password="bobPass2", YearSection="2B"},
              new Student {StudentID=202403, Name="Charlie Brown", Email="charlie.brown@gmail.com", Password="charliePass3", YearSection="2C"},
              new Student {StudentID=202404, Name="Diana Prince", Email="diana.prince@gmail.com", Password="dianaPass4", YearSection="2D"},
              new Student {StudentID=202405, Name="Ethan Hunt", Email="ethan.hunt@gmail.com", Password="ethanPass5", YearSection="2A"},
              new Student {StudentID=202406, Name="Fiona Gallagher", Email="fiona.gallagher@gmail.com", Password="fionaPass6", YearSection="2B"},
              new Student {StudentID=202407, Name="George Miller", Email="george.miller@gmail.com", Password="georgePass7", YearSection="2C"},
              new Student {StudentID=202408, Name="Hannah Williams", Email="hannah.williams@gmail.com", Password="hannahPass8", YearSection="2D"},
              new Student {StudentID=202409, Name="Ian Curtis", Email="ian.curtis@gmail.com", Password="ianPass9", YearSection="2A"},
              new Student {StudentID=202410, Name="Julia Roberts", Email="julia.roberts@gmail.com", Password="juliaPass10", YearSection="2B"},
              new Student {StudentID=202411, Name="Kevin Hart", Email="kevin.hart@gmail.com", Password="kevinPass11", YearSection="2C"},
              new Student {StudentID=202412, Name="Laura Palmer", Email="laura.palmer@gmail.com", Password="lauraPass12", YearSection="2D"},
              new Student {StudentID=202413, Name="Michael Scott", Email="michael.scott@gmail.com", Password="michaelPass13", YearSection="2A"},
              new Student {StudentID=202414, Name="Nancy Drew", Email="nancy.drew@gmail.com", Password="nancyPass14", YearSection="2B"},
              new Student {StudentID=202415, Name="Oscar Wilde", Email="oscar.wilde@gmail.com", Password="oscarPass15", YearSection="2C"},
              new Student {StudentID=202416, Name="Pam Beesly", Email="pam.beesly@gmail.com", Password="pamPass16", YearSection="2D"},
              new Student {StudentID=202417, Name="Quentin Tarantino", Email="quentin.tarantino@gmail.com", Password="quentinPass17", YearSection="2A"},
              new Student {StudentID=202418, Name="Rachel Green", Email="rachel.green@gmail.com", Password="rachelPass18", YearSection="2B"},
              new Student {StudentID=202419, Name="Sam Winchester", Email="sam.winchester@gmail.com", Password="samPass19", YearSection="2C"},
              new Student {StudentID=202420, Name="Tina Turner", Email="tina.turner@gmail.com", Password="tinaPass20", YearSection="2D"}
        };
        private static List<Teacher> teachers = new List<Teacher>()
        {
             new Teacher {TeacherID=1, Name="John Doe", Email="john.doe@gmail.com", Password="johnPass1"},
             new Teacher {TeacherID=2, Name="Emily Clark", Email="emily.clark@gmail.com", Password="emilyPass2"},
             new Teacher {TeacherID=3, Name="Robert Frost", Email="robert.frost@gmail.com", Password="robertPass3"},
             new Teacher {TeacherID=4, Name="Sophia Turner", Email="sophia.turner@gmail.com", Password="sophiaPass4"},
             new Teacher {TeacherID=5, Name="William Blake", Email="william.blake@gmail.com", Password="williamPass5"},
             new Teacher {TeacherID=6, Name="Ava Gardner", Email="ava.gardner@gmail.com", Password="avaPass6"},
             new Teacher {TeacherID=7, Name="James Bond", Email="james.bond@gmail.com", Password="jamesPass7"},
             new Teacher {TeacherID=8, Name="Grace Kelly", Email="grace.kelly@gmail.com", Password="gracePass8"},
             new Teacher {TeacherID=9, Name="Henry Ford", Email="henry.ford@gmail.com", Password="henryPass9"},
             new Teacher {TeacherID=10, Name="Isabella Swan", Email="isabella.swan@gmail.com", Password="isabellaPass10"},
             new Teacher {TeacherID=11, Name="Charlie Chaplin", Email="charlie.chaplin@gmail.com", Password="charliePass11"},
             new Teacher {TeacherID=12, Name="Olivia Newton", Email="olivia.newton@gmail.com", Password="oliviaPass12"},
             new Teacher {TeacherID=13, Name="Matthew Perry", Email="matthew.perry@gmail.com", Password="matthewPass13"},
             new Teacher {TeacherID=14, Name="Lily Evans", Email="lily.evans@gmail.com", Password="lilyPass14"},
             new Teacher {TeacherID=15, Name="Ben Affleck", Email="ben.affleck@gmail.com", Password="benPass15"},
             new Teacher {TeacherID=16, Name="Chloe Grace", Email="chloe.grace@gmail.com", Password="chloePass16"},
             new Teacher {TeacherID=17, Name="Tom Hardy", Email="tom.hardy@gmail.com", Password="tomPass17"},
             new Teacher {TeacherID=18, Name="Natalie Portman", Email="natalie.portman@gmail.com", Password="nataliePass18"},
             new Teacher {TeacherID=19, Name="David Tennant", Email="david.tennant@gmail.com", Password="davidPass19"},
             new Teacher {TeacherID=20, Name="Ellen Page", Email="ellen.page@gmail.com", Password="ellenPass20"}
        };

        // Public properties to expose the lists

        public static List<Admin> Admins => admins;
        public static List<Student> Students => students;
        public static List<Teacher> Teachers => teachers;


        [HttpPost("register/admin")]
        public ActionResult RegisterAdmin([FromBody] RegisterAdmin model)
        {
            if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Missing required fields.");
            }

            if (!admins.Any(a => a.Email == model.Email))
            {
                var newAdminId = admins.Any() ? admins.Max(a => a.AdminID) + 1 : 2024001;
                admins.Add(new Admin { AdminID = newAdminId, Name = model.FullName, Email = model.Email, Password = model.Password });
                return Ok("Admin account created successfully.");
            }

            return BadRequest("Admin with this email already exists.");
        }

        [HttpPost("register/teacher")]
        public ActionResult RegisterTeacher([FromBody] RegisterTeacher model)
        {
            if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Missing required fields.");
            }

            if (!teachers.Any(t => t.Email == model.Email))
            {
                var newTeacherId = teachers.Any() ? teachers.Max(t => t.TeacherID) + 1 : 1;
                teachers.Add(new Teacher { TeacherID = newTeacherId, Name = model.FullName, Email = model.Email, Password = model.Password });
                return Ok("Teacher account created successfully.");
            }

            return BadRequest("Teacher with this email already exists.");
        }

        [HttpPost("register/student")]
        public ActionResult RegisterStudent([FromBody] RegisterStudent model)
        {
            if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.YearSection))
            {
                return BadRequest("Missing required fields.");
            }

            if (!students.Any(s => s.Email == model.Email))
            {
                var newStudentId = students.Any() ? students.Max(s => s.StudentID) + 1 : 202401;
                students.Add(new Student { StudentID = newStudentId, Name = model.FullName, Email = model.Email, Password = model.Password, YearSection = model.YearSection });
                return Ok("Student account created successfully.");
            }

            return BadRequest("Student with this email already exists.");
        }

        [HttpPost("LoginAdmin")]
        public ActionResult Login([FromBody] LoginAdmin model)
        {
            var admin = admins.FirstOrDefault(a => a.Email == model.Email && a.Password == model.Password);


            if (admin != null)
            {
                return Ok("Admin logged in successfully.");
            }


            return Unauthorized("Invalid email or password.");
        }

        [HttpPost("LoginTeacher")]
        public ActionResult Login([FromBody] LoginTeacher model)
        {

            var teacher = teachers.FirstOrDefault(t => t.Email == model.Email && t.Password == model.Password);

            if (teacher != null)
            {
                return Ok("Teacher logged in successfully.");
            }


            return Unauthorized("Invalid email or password.");
        }

        [HttpPost("LoginStudent")]
        public ActionResult Login([FromBody] LoginStudent model)
        {

            var student = students.FirstOrDefault(s => s.Email == model.Email && s.Password == model.Password);


            if (student != null)
            {
                return Ok("Student logged in successfully.");
            }

            return Unauthorized("Invalid email or password.");
        }

    }
}