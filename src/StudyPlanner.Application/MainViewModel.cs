using System;
using System.Collections.ObjectModel;
using System.Linq;
using StudyPlanner.Deadlines;
using StudyPlanner.Domain;

namespace StudyPlanner.Application
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IStorageService _storage;
        private readonly string _filePath;
        private string _searchText = "";
        private string _courseFilter = "";
        private bool _onlyActive = true;

        public ObservableCollection<AssignmentItem> Assignments { get; } = new();
        public ObservableCollection<Course> Courses { get; } = new();

        public string SearchText
        {
            get => _searchText;
            set { if (Set(ref _searchText, value)) Raise(nameof(Filtered)); }
        }

        public string CourseFilter
        {
            get => _courseFilter;
            set { if (Set(ref _courseFilter, value)) Raise(nameof(Filtered)); }
        }

        public bool OnlyActive
        {
            get => _onlyActive;
            set { if (Set(ref _onlyActive, value)) Raise(nameof(Filtered)); }
        }

        public System.Collections.Generic.IEnumerable<AssignmentItem> Filtered
            => ApplyFilter(SearchText, CourseFilter, OnlyActive);

        public int TotalCount => Assignments.Count;
        public int OverdueCount => Assignments.Count(a => DeadlineService.GetStatus(a) == DeadlineStatus.Overdue);
        public int UpcomingCount => Assignments.Count(a => DeadlineService.GetStatus(a) == DeadlineStatus.Upcoming);

        public RelayCommand SaveCommand { get; }
        public RelayCommand LoadCommand { get; }

        public MainViewModel(IStorageService storage, string filePath)
        {
            _storage = storage;
            _filePath = filePath;
            SaveCommand = new RelayCommand(_ => Save());
            LoadCommand = new RelayCommand(_ => Load());
            SeedDemo();
        }

        public System.Collections.Generic.IEnumerable<AssignmentItem> ApplyFilter(
            string search, string course, bool onlyActive)
        {
            var q = Assignments.AsEnumerable();
            if (onlyActive)
                q = q.Where(a => !a.IsDone);
            if (!string.IsNullOrWhiteSpace(course))
                q = q.Where(a => a.CourseName.Equals(course.Trim(),
                    StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(a => a.Title.Contains(s, StringComparison.OrdinalIgnoreCase)
                    || a.Description.Contains(s, StringComparison.OrdinalIgnoreCase));
            }
            return q.OrderBy(a => DeadlineService.UrgencyRank(a)).ToList();
        }

        public void Add(AssignmentItem item)
        {
            Assignments.Add(item);
            Raise(nameof(Filtered));
            Raise(nameof(TotalCount));
            Raise(nameof(OverdueCount));
            Raise(nameof(UpcomingCount));
        }

        public void Save() => _storage.Save(_filePath,
            new StudyData
            {
                Courses = Courses.ToList(),
                Assignments = Assignments.ToList()
            });

        public void Load()
        {
            var data = _storage.Load(_filePath);
            Courses.Clear();
            foreach (var c in data.Courses)
                Courses.Add(c);
            Assignments.Clear();
            foreach (var a in data.Assignments)
                Assignments.Add(a);
            Raise(nameof(Filtered));
            Raise(nameof(TotalCount));
            Raise(nameof(OverdueCount));
            Raise(nameof(UpcomingCount));
        }

        private void SeedDemo()
        {
            Courses.Add(new Course("ИСРПО", "#4A90D9"));
            Courses.Add(new Course("Математика", "#7ED321"));
            Assignments.Add(new AssignmentItem("ПР5 — интеграция модулей", "ИСРПО",
                DateTime.Today.AddDays(2), Priority.High, "Слить ветки и проверить сборку"));
            Assignments.Add(new AssignmentItem("Домашка по пределам", "Математика",
                DateTime.Today.AddDays(-1), Priority.Medium));
        }
    }
}
