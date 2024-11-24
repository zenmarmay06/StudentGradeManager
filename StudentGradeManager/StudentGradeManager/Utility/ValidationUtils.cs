using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGradeManager.Utility
{
    public class ValidationUtils
    {
        public static bool IsValidSemester(string semester)
        {
            return semester.Equals("first", StringComparison.OrdinalIgnoreCase) ||
                   semester.Equals("second", StringComparison.OrdinalIgnoreCase);
        }
    }
}
