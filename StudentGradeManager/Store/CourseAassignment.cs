using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGradeManager.Store
{
    public class CourseAssignment
    {
        public int TeacherID { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; } // New property for course name
        public List<int> StudentIDs { get; set; } = new List<int>();
        public List<int> TeacherIDs { get; set; } = new List<int>();
        public string Semester { get; set; }  // Track semester assignment (e.g., "first" or "second")
        public string YearSection { get; set; }

    }

}


