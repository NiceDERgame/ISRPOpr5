using System;
using StudyPlanner.Domain;

namespace StudyPlanner.Deadlines
{
    public enum DeadlineStatus
    {
        Overdue,
        Upcoming,
        Normal,
        Done
    }

    public static class DeadlineService
    {
        public static DeadlineStatus GetStatus(AssignmentItem item, DateTime? now = null)
        {
            if (item.IsDone)
                return DeadlineStatus.Done;
            var today = (now ?? DateTime.Today).Date;
            if (item.Deadline.Date < today)
                return DeadlineStatus.Overdue;
            // Порог ближайших дедлайнов (Коршунов): 5 дней
            if ((item.Deadline.Date - today).TotalDays <= 5)
                return DeadlineStatus.Upcoming;
            return DeadlineStatus.Normal;
        }

        public static int UrgencyRank(AssignmentItem item, DateTime? now = null)
        {
            var status = GetStatus(item, now);
            int baseRank = status switch
            {
                DeadlineStatus.Overdue => 0,
                DeadlineStatus.Upcoming => 1,
                DeadlineStatus.Normal => 2,
                _ => 3
            };
            return baseRank * 10 - (int)item.Priority;
        }
    }
}
