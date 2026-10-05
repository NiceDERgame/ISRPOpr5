using System.IO;
using System.Windows;
using StudyPlanner.Application;
using StudyPlanner.Infrastructure.Json;
using StudyPlanner.Presentation;

namespace StudyPlanner.App
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var storage = new JsonStorageService();
            var path = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
                "StudyPlanner", "studyplanner.json");
            var vm = new MainViewModel(storage, path);
            vm.Load();
            var window = new MainWindow(vm);
            window.Show();
        }
    }
}
