using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Text;
using System.Text.Json;

namespace GradesTrackerLib {
    public class Course {
        public Course() { }

        public int CourseId { get;  set; }
        public int Credits { get; set; }
        public int TeacherId { get; set; }
        public string CourseType { get; set; }
        public double OverallGrade { get; set; }
        public string ScaleData { get; set; }
        [NotMapped]
        public AssignmentScale Scale { get; set; }
        public string GradesData { get; set; }
        [NotMapped]
        public ReportCard Grades { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Owner { get; set; }
        public string InstitutionName { get; set; }

        public void DesieralizeDataColumns() {
            Scale = JsonSerializer.Deserialize<AssignmentScale>(ScaleData);
            Grades = JsonSerializer.Deserialize<ReportCard>(GradesData);
        }
    }
}
