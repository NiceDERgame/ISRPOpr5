using System.Collections.Generic;

namespace StudyPlanner.Domain
{
    public class StudyData
    {
        public List<Course> Courses { get; set; } = new();
        public List<AssignmentItem> Assignments { get; set; } = new();
    }
}
