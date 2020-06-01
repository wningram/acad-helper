using System;
using System.Collections.Generic;
using System.Text;

namespace GradesTrackerLib {
    public class LetterGradeScale {
        public int Id { get; set; }
        public double From { get; set; }
        public double To { get; set; }
        public string LetterGrade { get; set; }
        public string InstitutionName { get; set; }
    }
}
