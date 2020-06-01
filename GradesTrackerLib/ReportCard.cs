using System;
using System.Collections.Generic;
using System.Text;

namespace GradesTrackerLib {
    public class ReportCard {
        public List<Assignment> Assignments { get; set; }

        public void GetNeededGrade(AssignmentTypes type, double classGrade) {
            throw new NotImplementedException();
        }
    }
}
