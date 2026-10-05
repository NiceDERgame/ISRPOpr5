using StudyPlanner.Domain;

namespace StudyPlanner.Application
{
    public interface IStorageService
    {
        StudyData Load(string path);
        void Save(string path, StudyData data);
    }
}
