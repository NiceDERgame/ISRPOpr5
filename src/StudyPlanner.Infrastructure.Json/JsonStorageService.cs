using System.IO;
using System.Text.Json;
using StudyPlanner.Application;
using StudyPlanner.Domain;

namespace StudyPlanner.Infrastructure.Json
{
    public class JsonStorageService : IStorageService
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true
        };

        public StudyData Load(string path)
        {
            if (!File.Exists(path))
                return new StudyData();
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<StudyData>(json, Options) ?? new StudyData();
        }

        public void Save(string path, StudyData data)
        {
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
            if (File.Exists(path))
                File.Copy(path, path + ".bak", overwrite: true);
            var json = JsonSerializer.Serialize(data, Options);
            File.WriteAllText(path, json);
        }

        public static string BuildSummary(StudyData data)
        {
            int total = data.Assignments.Count;
            int done = 0;
            foreach (var a in data.Assignments)
                if (a.IsDone)
                    done++;
            return $"Всего: {total} | Выполнено: {done} | Активно: {total - done}";
        }
    }
}
