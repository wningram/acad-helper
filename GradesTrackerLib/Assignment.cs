using System;

namespace GradesTrackerLib {
    public enum AssignmentTypes {
        TEST,
        QUIZ,
        CLASSWORK,
        HOMEWORK,
        PROJECT
    }

    public class Assignment {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Grade { get; set; }
        public AssignmentTypes Type { get; set; }       
        public int Owner { get; set; }
        public int CourseId { get; set; }
    }
}
