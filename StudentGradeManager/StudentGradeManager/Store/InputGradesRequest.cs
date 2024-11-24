using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGradeManager.Store
{
    public class InputGradesRequest
    {
        public string Semester { get; set; }
        
        public List<StudentGradeInput> Grades { get; set; }
        public int CourseID { get; set; }
    }
}
