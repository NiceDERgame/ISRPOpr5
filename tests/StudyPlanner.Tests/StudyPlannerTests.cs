using System;
using System.IO;
using StudyPlanner.Application;
using StudyPlanner.Deadlines;
using StudyPlanner.Domain;
using StudyPlanner.Infrastructure.Json;
using Xunit;

namespace StudyPlanner.Tests
{
    public class DeadlineServiceTests
    {
        [Fact]
        public void Overdue_Yesterday_ReturnsOverdue()
        {
            var item = new AssignmentItem("T", "C", DateTime.Today.AddDays(-1));
            Assert.Equal(DeadlineStatus.Overdue, DeadlineService.GetStatus(item));
        }

        [Fact]
        public void Upcoming_InTwoDays_ReturnsUpcoming()
        {
            var item = new AssignmentItem("T", "C", DateTime.Today.AddDays(2));
            Assert.Equal(DeadlineStatus.Upcoming, DeadlineService.GetStatus(item));
        }

        [Fact]
        public void Normal_InTenDays_ReturnsNormal()
        {
            var item = new AssignmentItem("T", "C", DateTime.Today.AddDays(10));
            Assert.Equal(DeadlineStatus.Normal, DeadlineService.GetStatus(item));
        }

        [Fact]
        public void Done_OverdueDeadline_ReturnsDone()
        {
            var item = new AssignmentItem("T", "C", DateTime.Today.AddDays(-5))
            {
                IsDone = true
            };
            Assert.Equal(DeadlineStatus.Done, DeadlineService.GetStatus(item));
        }
    }

    public class StorageTests
    {
        [Fact]
        public void SaveLoad_Roundtrip_KeepsData()
        {
            var storage = new JsonStorageService();
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            var data = new StudyData();
            data.Courses.Add(new Course("ИСРПО"));
            data.Assignments.Add(new AssignmentItem("ПР5", "ИСРПО", DateTime.Today.AddDays(1)));

            storage.Save(path, data);
            var loaded = storage.Load(path);

            Assert.Single(loaded.Courses);
            Assert.Single(loaded.Assignments);
            Assert.Equal("ПР5", loaded.Assignments[0].Title);
        }

        [Fact]
        public void Load_MissingFile_ReturnsEmpty()
        {
            var storage = new JsonStorageService();
            var loaded = storage.Load(Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json"));
            Assert.Empty(loaded.Assignments);
        }
    }

    public class FilterTests
    {
        private static MainViewModel CreateVm()
        {
            var storage = new JsonStorageService();
            return new MainViewModel(storage, Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json"));
        }

        [Fact]
        public void ApplyFilter_ByCourse_ReturnsOnlyCourse()
        {
            var vm = CreateVm();
            var result = vm.ApplyFilter("", "ИСРПО", false);
            Assert.All(result, a => Assert.Equal("ИСРПО", a.CourseName));
        }

        [Fact]
        public void ApplyFilter_ByText_SearchesTitleAndDescription()
        {
            var vm = CreateVm();
            var result = vm.ApplyFilter("интеграция", "", false);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Validate_EmptyTitle_ReturnsError()
        {
            var edit = new TaskEditViewModel { Title = " ", CourseName = "ИСРПО" };
            Assert.NotEmpty(edit.Validate());
        }
    }
}
