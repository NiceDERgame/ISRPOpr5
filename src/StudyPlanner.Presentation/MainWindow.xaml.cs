using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StudyPlanner.Application;
using StudyPlanner.Domain;

namespace StudyPlanner.Presentation
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm;

        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            var items = _vm.ApplyFilter(
                SearchBox?.Text ?? "",
                CourseBox?.Text ?? "",
                OnlyActiveBox?.IsChecked == true).ToList();
            TasksGrid.ItemsSource = items;
            StatsBar.Text = $"Всего: {_vm.TotalCount} | " +
                $"Просрочено: {_vm.OverdueCount} | Ближайшие: {_vm.UpcomingCount}";
            if (items.Count == 0)
                StatsBar.Text += " | Ничего не найдено";
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => RefreshGrid();
        private void Filter_Changed(object sender, RoutedEventArgs e) => RefreshGrid();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new TaskEditWindow();
            if (dlg.ShowDialog() == true && dlg.Result != null)
            {
                _vm.Add(dlg.Result);
                _vm.Save();
                RefreshGrid();
            }
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            if (TasksGrid.SelectedItem is AssignmentItem item)
            {
                item.IsDone = true;
                _vm.Save();
                RefreshGrid();
            }
            else
            {
                MessageBox.Show("Выберите задание для отметки.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (TasksGrid.SelectedItem is AssignmentItem item)
            {
                var answer = MessageBox.Show($"Удалить «{item.Title}?»",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.Yes)
                {
                    _vm.Assignments.Remove(item);
                    _vm.Save();
                    RefreshGrid();
                }
            }
            else
            {
                MessageBox.Show("Выберите задание для удаления.");
            }
        }
    }
}
