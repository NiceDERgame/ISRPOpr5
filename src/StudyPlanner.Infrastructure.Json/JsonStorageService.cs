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
            var json = JsonSerializer.Serialize(data, Options);
            File.WriteAllText(path, json);
        }
    }
}
