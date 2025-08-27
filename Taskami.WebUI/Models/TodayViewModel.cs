namespace Taskami.WebUI.Models
{
    internal class TodayViewModel
    {
        public required List<TodoistTask> TodaysTasks { get; set; }
        public required List<TodoistTask> OtherTasks { get; set; }
    }
}