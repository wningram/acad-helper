using FrontendApp.Data;
using GradesTrackerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontendApp {
    public class BaseGradeService : IGradesService {
        private List<Course> _courses;
        private List<Assignment> _assignments;
        private List<LetterGradeScale> _letterGradeScales;
        public  readonly Dictionary<string, double> GradePointsMap;

        private BaseGradeService() {
            GradePointsMap = new Dictionary<string, double>() {
                {"A", 4.00 },
                {"A-", 3.70 },
                {"B+", 3.33 },
                {"B", 3.00 },
                {"B-", 2.70 },
                {"C+", 2.30 },
                {"C", 2.00 },
                {"C-", 1.70 },
                {"D+", 1.30 },
                {"D", 1.00 },
                {"D-", 0.70 },
            };
        }
        public BaseGradeService(ApplicationDbContext dataContext) : this() {
            _courses = dataContext.Courses.ToList();
            _assignments = dataContext.Assignments.ToList();
            _letterGradeScales = dataContext.LetterGradeScales.ToList();
        }

        public BaseGradeService(Dictionary<string, List<object>> data) : this() {
            // TODO: Add code to populate tables
            _courses = (from course in data["courses"]
                        select (Course)course).ToList();
            _assignments = (from assignment in data["assignments"]
                            select (Assignment)assignment).ToList();
            _letterGradeScales = (from obj in data["lettergradescales"]
                                  select (LetterGradeScale)obj).ToList();
        }

        /// <summary>
        /// Builds a <see cref="CourseLoad"/> object with data from the database.
        /// </summary>
        /// <param name="semesterStart">The start date of the semester to pull courses for.</param>
        /// <param name="semesterEnd">The end date of the semester to pull courses for.</param>
        /// <returns>A ,<see cref="CourseLoad"/> representing the semester within the given time period.</returns>
        public CourseLoad GetCourseLoad(DateTime semesterStart, DateTime semesterEnd) {
            int totalHours = 0;
            double gpa = 0.0;
            string institution = "";
            // Get courses
            List<Course> courses = _courses.Where(c => {
                if (c.StartDate >= semesterStart && c.EndDate <= semesterEnd)
                    return true;
                else
                    return false;
            }).ToList();
            // Get total credit hours
            foreach (Course course in courses) {
                totalHours += course.Credits;
            }
            // Calculate GPA
            gpa = calculateGpa(courses);
            // Get institution name
            institution = courses.First().InstitutionName;

            return new CourseLoad() {
                Courses = courses,
                TotalCreditHours = totalHours,
                SemesterGpa = gpa,
                InstitutionName = institution,
                StartTime = semesterStart,
                EndTime = semesterEnd
            };
        }

        /// <summary>
        /// Builds a <see cref="ReportCard"/> object from data in the database.
        /// </summary>
        /// <param name="semesterEnd">The end date of the semester to pull courses for.</param>
        /// <param name="semesterStart">The start date of the semester to pull courses for.</param>
        /// <returns>A <see cref="ReportCard"/> object with assignments for the current course range.</returns>
        public ReportCard GetReportCard(DateTime semesterStart, DateTime semesterEnd) {
            IEnumerable<Assignment> assignments = _assignments.Where(a => {
                Course course = _courses.First(c => c.CourseId == a.CourseId);
                if (course.StartDate >= semesterStart && course.EndDate <= semesterEnd)
                    return true;
                else
                    return false;
            });
            return new ReportCard() {
                Assignments = assignments.ToList()
            };
        }

        public Transcript GetTranscript(TimeSpan semesterLength) {
            List<Course> courses = _courses;
            List<CourseLoad> courseLoads = new List<CourseLoad>();
            CourseLoad courseLoad = new CourseLoad() {
                Courses = new List<Course>(),
                TotalCreditHours = 0
            };
            double endTime = -1;

            courses.Sort(compareByStartDate);
            foreach (Course course in courses) {
                double courseStartDate = course.StartDate.ToOADate();
                if (endTime == -1 || courseStartDate > endTime) {
                    endTime = (course.StartDate + semesterLength).ToOADate();
                    courseLoad = new CourseLoad() {
                        Courses = new List<Course>(),
                        TotalCreditHours = 0
                    };
                }
                courseLoad.Courses.Add(course);
                // Courseload institution name is the institution name of the last course in the semester date range
                courseLoad.InstitutionName = course.InstitutionName;
                courseLoad.TotalCreditHours += course.Credits;
                if (courseLoad.StartTime > course.StartDate)
                    courseLoad.StartTime = course.StartDate;
                if (courseLoad.EndTime < course.EndDate)
                    courseLoad.EndTime = course.EndDate;
                courseLoads.Add(courseLoad);
            }
            return new Transcript() {
                CourseLoads = courseLoads,
                OverallGPA = calculateCummulativeGpa(courseLoads)
            };
        }

        private double calculateGpa(List<Course> courses) {
            double creditHours = 0,
                gradePoints = 0;
            foreach (Course  course in courses) {
                string letterGrade = null;
                double courseGradePoiints = 0.0;
                // Get total credit hours
                creditHours += course.Credits;
                // Get grades points for class
                foreach (LetterGradeScale lg in _letterGradeScales) {
                    if (course.InstitutionName == lg.InstitutionName
                        && course.OverallGrade >= lg.From
                        && course.OverallGrade <= lg.To) {
                        letterGrade = lg.LetterGrade;
                    }
                }
                if (letterGrade == null) {
                    throw new Exception("Could not map grade to letter.");
                }
                courseGradePoiints = GradePointsMap[letterGrade] * course.Credits;
                gradePoints += courseGradePoiints;
            }
            return gradePoints / creditHours;
        }

        private double calculateCummulativeGpa(List<CourseLoad> courseLoads) {
            List<Course> courses = new List<Course>();
            foreach (CourseLoad load in courseLoads) {
                courses.AddRange(load.Courses);
            }
            return calculateGpa(courses);
        }

        private int compareByStartDate(Course x, Course y) {
            if (x.StartDate > y.StartDate)
                return 1;
            else if (x.StartDate < y.StartDate)
                return -1;
            else if (x.StartDate == y.StartDate)
                return 0;
            else
                return -1;
        }
    }
}
