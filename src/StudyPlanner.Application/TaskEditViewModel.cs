using System;
using System.Collections.Generic;
using StudyPlanner.Domain;

namespace StudyPlanner.Application
{
    public class TaskEditViewModel : ViewModelBase
    {
        private string _title = "";
        private string _courseName = "";
        private string _description = "";
        private Priority _priority = Priority.Medium;
        private DateTime _deadline = DateTime.Today.AddDays(7);

        public string Title { get => _title; set => Set(ref _title, value); }
        public string CourseName { get => _courseName; set => Set(ref _courseName, value); }
        public string Description { get => _description; set => Set(ref _description, value); }
        public Priority Priority { get => _priority; set => Set(ref _priority, value); }
        public DateTime Deadline { get => _deadline; set => Set(ref _deadline, value); }

        public List<string> Validate()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(Title))
                errors.Add("Название задания не должно быть пустым.");
            if (string.IsNullOrWhiteSpace(CourseName))
                errors.Add("Дисциплина должна быть выбрана.");
            if (Title.Trim().Length > 120)
                errors.Add("Название не должно превышать 120 символов.");
            if (!IsDoneAllowedInPast && Deadline.Date < DateTime.Today)
                errors.Add("Срок нового задания не может быть в прошлом.");
            return errors;
        }

        public bool IsDoneAllowedInPast { get; set; }

        public AssignmentItem ToItem() => new(
            Title.Trim(), CourseName.Trim(), Deadline, Priority, Description.Trim());
    }
}
