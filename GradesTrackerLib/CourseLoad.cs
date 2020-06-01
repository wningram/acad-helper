using System;
using System.Collections.Generic;
using System.Text;

namespace GradesTrackerLib {
    /// <summary>
    /// This class is intended to be registered as a service in an ASP .Net application. Contains
    /// data on the current course load of the student.
    /// </summary>
    public class CourseLoad {
        public List<Course> Courses { get; set; }
        public int TotalCreditHours { get; set; }
        public double SemesterGpa { get; set; }
        public string InstitutionName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
