using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGradeManager.Store
{
    public class StudentPerformance
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public List<double> Grades { get; set; } // Assuming you have grades for the students
    }
}
