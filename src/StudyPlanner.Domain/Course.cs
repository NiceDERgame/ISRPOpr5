namespace StudyPlanner.Domain
{
    public class Course
    {
        public string Name { get; set; } = "";
        public string Color { get; set; } = "#4A90D9";

        public Course() { }
        public Course(string name, string color = "#4A90D9")
        {
            Name = name;
            Color = color;
        }
    }
}
