namespace Taskami.WebUI.Models
{
    internal class PomodoroViewModel
    {
        public string PrimaryColor { get; set; } = "#222222";
        public string SecondaryColor { get; set; } = "#191919";
        public int PomodoroDuration { get; set; } = 25;
        public int PomodoroBreakDuration { get; set; } = 5;
        public int PomodoroLongBreakDuration { get; set; } = 15;
    }
}