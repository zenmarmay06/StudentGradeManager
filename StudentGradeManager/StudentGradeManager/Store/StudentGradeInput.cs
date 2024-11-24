using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGradeManager.Store
{
    public class StudentGradeInput
    {
        public int StudentID { get; set; }
        public double MidtermGrade { get; set; }
        public double FinalGrade { get; set; }
    }
}
