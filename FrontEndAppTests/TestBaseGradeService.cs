using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontendApp;
using FrontendApp.Data;
using Microsoft.EntityFrameworkCore;
using GradesTrackerLib;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace FrontEndAppTests {
    [TestClass]
    public class TestBaseGradeService {
        private readonly List<Course> _testCourseData = new List<Course>() {
            new Course() {
                CourseId = 1,
                Credits = 2,
                CourseType = "HU",
                OverallGrade = 92.0,
                StartDate = DateTime.Parse("1/1/2020"),
                EndDate = DateTime.Parse("1/1/20") + new TimeSpan(7 * 9, 0, 0, 0), // End date is 9 weeks past start date
                Owner = 1,
                InstitutionName = "Test Institution"
            },
            new Course() {
                CourseId = 1,
                Credits = 3,
                CourseType = "HU",
                OverallGrade = 74.0,
                StartDate = DateTime.Parse("4/1/2020"),
                EndDate = DateTime.Parse("4/1/20") + new TimeSpan(7 * 9, 0, 0, 0), // End date is 9 weeks past start date
                Owner = 1,
                InstitutionName = "Test Institution"
            },
        };

        private readonly List<Assignment> _testAssignmentData = new List<Assignment>() {
            new Assignment() {
                CourseId = 1,
                Grade = 95.0,
                Id = 1,
                Name = "End of Quarter Project",
                Owner = 1,
                Type = AssignmentTypes.PROJECT
            }
        };

        private readonly List<LetterGradeScale> _testLetterGradeScaleData = new List<LetterGradeScale>() {
            new LetterGradeScale() {
                From = 0,
                To = 59.4,
                LetterGrade = "F",
                ScaleId = 1
            },
            new LetterGradeScale() {
                InstitutionName = "Test Institution",
                From = 59.5,
                To = 62.4,
                LetterGrade = "D-",
                ScaleId = 2
            },
            new LetterGradeScale() {
                InstitutionName = "Test Institution",
                From = 62.5,
                To = 67.4,
                LetterGrade = "D",
                ScaleId = 3
            },
            new LetterGradeScale() {
                InstitutionName = "Test Institution",
                From = 67.5,
                To = 69.4,
                LetterGrade = "D+",
                ScaleId = 4
            },
            new LetterGradeScale() {
                InstitutionName = "Test Institution",
                From = 69.5,
                To = 72.4,
                LetterGrade = "C-",
                ScaleId = 5
            },
            new LetterGradeScale() {
                InstitutionName = "Test Institution",
                From = 72.5,
                To = 77.4,
                LetterGrade = "C",
                ScaleId = 6
            },
            new LetterGradeScale() {
                InstitutionName = "Test Institution",
                From = 77.5,
                To = 79.4,
                LetterGrade = "C+",
                ScaleId = 7
            },
            new LetterGradeScale() {
                InstitutionName = "Test Institution",
                From = 79.5,
                To = 82.4,
                LetterGrade = "B-",
                ScaleId = 8
            },
            new LetterGradeScale() {
                InstitutionName = "Test Institution",
                From = 82.5,
                To = 87.4,
                LetterGrade = "B",
                ScaleId = 9
            },
            new LetterGradeScale() {
                InstitutionName = "Test Institution",
                From = 87.5,
                To = 89.4,
                LetterGrade = "B+",
                ScaleId = 10
            },
            new LetterGradeScale() {
                InstitutionName = "Test Institution",
                From = 89.5,
                To = 92.4,
                LetterGrade = "A-",
                ScaleId = 11
            },
            new LetterGradeScale() {
                InstitutionName = "Test Institution",
                From = 92.5,
                To = 97.4,
                LetterGrade = "A",
                ScaleId = 12
            },
            new LetterGradeScale() {
                InstitutionName = "Test Institution",
                From = 97.5,
                To = 100,
                LetterGrade = "A+",
                ScaleId = 13
            }
        };
        
        [TestMethod]
        public void TestGetCourseLoad() {
            Dictionary<string, List<object>> serviceData = new Dictionary<string, List<object>>() {
                {"courses", _testCourseData.ToList<object>() },
                {"assignments", _testAssignmentData.ToList<object>() },
                {"lettergradescales", _testLetterGradeScaleData.ToList<object>() }
            };
            BaseGradeService service = new BaseGradeService(serviceData);

            CourseLoad result = service.GetCourseLoad(
                DateTime.Parse("1/1/2020"),
                DateTime.Parse("1/1/20") + new TimeSpan(7 * 9, 0, 0, 0));

            Assert.AreEqual(result.Courses.Count, 1);
            Assert.AreEqual(result.TotalCreditHours, 2);
            // TODO: Need test for semester GPA calculation
            Assert.AreEqual(result.InstitutionName, "Test Institution");
            Assert.AreEqual(result.StartTime, DateTime.Parse("1/1/2020"));
            Assert.AreEqual(result.EndTime,
                DateTime.Parse("1/1/20") + new TimeSpan(7 * 9, 0, 0, 0));
;        }

        [TestMethod]
        public void TestGetReportCard() {
            // Define variables
            Dictionary<string, List<object>> serviceData = new Dictionary<string, List<object>>() {
                {"courses", _testCourseData.ToList<object>() },
                {"assignments", _testAssignmentData.ToList<object>() },
                {"lettergradescales", _testLetterGradeScaleData.ToList<object>() }
            };
            BaseGradeService service = new BaseGradeService(serviceData);

            // Run test
            ReportCard card = service.GetReportCard(
                DateTime.Parse("1/1/2020"),
                DateTime.Parse("1/1/2020") + new TimeSpan(7 * 9, 0, 0, 0));

            // Assertions
            Assert.AreEqual(1, card.Assignments.Count);
        }

        [TestMethod]
        public void TestGetTranscript() {
            Dictionary<string, List<object>> serviceData = new Dictionary<string, List<object>>() {
                {"courses", _testCourseData.ToList<object>() },
                {"assignments", _testAssignmentData.ToList<object>() },
                {"lettergradescales", _testLetterGradeScaleData.ToList<object>() }
            };
            BaseGradeService service = new BaseGradeService(serviceData);

            // Run test
            Transcript result = service.GetTranscript(new TimeSpan(7 * 9, 0, 0, 0));

            // Assertions
            Assert.AreEqual(2, result.CourseLoads.Count);
            // TODO: Add assertion for OverallGPA property
        }

        [TestMethod]
        public void TestCalculateGpa() {
            Dictionary<string, List<object>> serviceData = new Dictionary<string, List<object>>() {
                {"courses", _testCourseData.ToList<object>() },
                {"assignments", _testAssignmentData.ToList<object>() },
                {"lettergradescales", _testLetterGradeScaleData.ToList<object>() }
            };
            BaseGradeService service = new BaseGradeService(serviceData);

            CourseLoad result = service.GetCourseLoad(DateTime.Parse("1/1/2020"),
                DateTime.Parse("1/1/2020") + new TimeSpan(7 * 9, 0, 0, 0));

            // Assertions
            Assert.AreEqual(3.70, result.SemesterGpa);
        }

        [TestMethod]
        public void TestCalculateCummulativeGpa() {
            Dictionary<string, List<object>> serviceData = new Dictionary<string, List<object>>() {
                {"courses", _testCourseData.ToList<object>() },
                {"assignments", _testAssignmentData.ToList<object>() },
                {"lettergradescales", _testLetterGradeScaleData.ToList<object>() }
            };
            BaseGradeService service = new BaseGradeService(serviceData);

            Transcript result = service.GetTranscript(new TimeSpan(7 * 9, 0, 0, 0));

            Assert.AreEqual(2.68, result.OverallGPA);
        }
    }
}
