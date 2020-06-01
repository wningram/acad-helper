using System;
using System.Collections.Generic;
using System.Text;

namespace GradesTrackerLib {
    interface IGradesService {
        CourseLoad GetCourseLoad();
        ReportCard GetReportCard();
        Transcript GetTranscript();
    }
}
