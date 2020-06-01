using System;
using System.Collections.Generic;
using System.Text;
using GradesTrackerLib;

namespace FrontendApp {
    public interface IGradesService {
        CourseLoad GetCourseLoad(DateTime semesterStart, DateTime semesterEnd);
        ReportCard GetReportCard(DateTime semesterStart, DateTime semesterEnd);
        Transcript GetTranscript(TimeSpan semesterLength);
    }
}
