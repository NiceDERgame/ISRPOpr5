using System;
using System.Windows;
using StudyPlanner.Application;

namespace StudyPlanner.Presentation
{
    public partial class TaskEditWindow : Window
    {
        private readonly TaskEditViewModel _vm = new();

        public Domain.AssignmentItem? Result { get; private set; }

        public TaskEditWindow()
        {
            InitializeComponent();
            DataContext = _vm;
            DeadlinePicker.SelectedDate = DateTime.Today.AddDays(7);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _vm.Title = TitleBox.Text;
            _vm.CourseName = CourseBox.Text;
            _vm.Description = DescBox.Text;
            _vm.Deadline = DeadlinePicker.SelectedDate ?? DateTime.Today;

            var errors = _vm.Validate();
            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, errors),
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Result = _vm.ToItem();
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
