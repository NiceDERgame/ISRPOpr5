using System;

namespace StudyPlanner.Domain
{
    public class AssignmentItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string CourseName { get; set; } = "";
        public Priority Priority { get; set; } = Priority.Medium;
        public DateTime Deadline { get; set; } = DateTime.Today.AddDays(7);
        public bool IsDone { get; set; }

        public AssignmentItem() { }

        public AssignmentItem(string title, string courseName, DateTime deadline,
            Priority priority = Priority.Medium, string description = "")
        {
            Title = title;
            CourseName = courseName;
            Deadline = deadline;
            Priority = priority;
            Description = description;
        }
    }
}
