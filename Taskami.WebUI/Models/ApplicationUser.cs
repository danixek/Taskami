using Microsoft.AspNetCore.Identity;

namespace Taskami.WebUI.Models
{
    public class ApplicationUser : IdentityUser
    {
        // API klíče
        public string? TaskamiApiKey { get; set; } = "YOUR_API_KEY";
        public string? TodoistApiKey { get; set; } = "YOUR_API_KEY";

        // Barvy
        public string? PrimaryColor { get; set; } = "#222222";
        public string? SecondaryColor { get; set; } = "#191919";

        // Pomodoro nastavení
        public int PomodoroDuration { get; set; } = 25;
        public int PomodoroBreakDuration { get; set; } = 5;
        public int PomodoroLongBreakDuration { get; set; } = 15;

        // Další uživatelské preference
        public string? BackendType { get; set; } = "taskami"; // "Todoist" nebo "Taskami"
    }
}
