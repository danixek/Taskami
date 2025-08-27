namespace Taskami.WebUI.Models
{
    internal class CalendarViewModel
    {
        public required List<TodoistTask> TodaysTasks { get; set; }
        public required List<TodoistTask> TomorrowsTasks { get; set; }
        public required List<TodoistTask> AllTasks { get; set; }
    }
}